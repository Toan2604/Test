using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Services.MStoreUser
{
    public interface IStoreUserService : IServiceScoped
    {
        Task<int> Count(StoreUserFilter StoreUserFilter);
        Task<List<StoreUser>> List(StoreUserFilter StoreUserFilter);
        Task<StoreUser> Get(long Id);
        Task<StoreUser> Login(StoreUser StoreUser);
        Task<StoreUser> ChangePassword(StoreUser StoreUser);
        Task<StoreUser> ResetPassword(StoreUser StoreUser);
        Task<StoreUser> ForgotPassword(StoreUser StoreUser);
        Task<StoreUser> VerifyOtpCode(StoreUser StoreUser);
        Task<StoreUser> RecoveryPassword(StoreUser StoreUser);
        Task<StoreUser> LockStoreUser(StoreUser StoreUser);
        Task<StoreUser> Create(StoreUser StoreUser);
        Task<List<StoreUser>> BulkCreateStoreUser(List<StoreUser> StoreUsers);
        Task<StoreUser> CreateDraft(StoreUser StoreUser);
        Task<StoreUser> Update(StoreUser StoreUser);
        Task<StoreUser> Delete(StoreUser StoreUser);
        Task<List<StoreUser>> BulkDelete(List<StoreUser> StoreUsers);
        Task<List<StoreUser>> Import(List<StoreUser> StoreUsers);
        Task<StoreUserFilter> ToFilter(StoreUserFilter StoreUserFilter);
        Task<string> SaveImage(Image Image);
    }

    public class StoreUserService : BaseService, IStoreUserService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IStoreUserValidator StoreUserValidator;
        private IConfiguration Configuration;
        private IRabbitManager RabbitManager;

        public StoreUserService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IStoreUserValidator StoreUserValidator,
            IConfiguration Configuration,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.StoreUserValidator = StoreUserValidator;
            this.Configuration = Configuration;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(StoreUserFilter StoreUserFilter)
        {
            try
            {
                int result = await UOW.StoreUserRepository.Count(StoreUserFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return 0;
        }

        public async Task<List<StoreUser>> List(StoreUserFilter StoreUserFilter)
        {
            try
            {
                List<StoreUser> StoreUsers = await UOW.StoreUserRepository.List(StoreUserFilter);
                return StoreUsers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }
        public async Task<StoreUser> Get(long Id)
        {
            StoreUser StoreUser = await UOW.StoreUserRepository.Get(Id);
            if (StoreUser == null)
                return null;
            return StoreUser;
        }

        public async Task<StoreUser> Login(StoreUser StoreUser)
        {
            if (!await StoreUserValidator.Login(StoreUser))
                return StoreUser;
            StoreUser = await UOW.StoreUserRepository.Get(StoreUser.Id);
            CurrentContext.StoreUserId = StoreUser.Id;
            Logging.CreateAuditLog(new { }, StoreUser, nameof(StoreUserService));
            StoreUser.Token = CreateToken(StoreUser.Id, StoreUser.Username);

            return StoreUser;
        }

        public async Task<StoreUser> ChangePassword(StoreUser StoreUser)
        {
            if (!await StoreUserValidator.ChangePassword(StoreUser))
                return StoreUser;
            try
            {
                StoreUser oldData = await UOW.StoreUserRepository.Get(StoreUser.Id);
                oldData.Password = HashPassword(StoreUser.NewPassword);

                await UOW.StoreUserRepository.Update(oldData);

                var newData = await UOW.StoreUserRepository.Get(StoreUser.Id);
                Sync(new List<StoreUser> { newData });
                Logging.CreateAuditLog(newData, oldData, nameof(StoreUserService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<StoreUser> ResetPassword(StoreUser StoreUser)
        {
            if (!await StoreUserValidator.ResetPassword(StoreUser))
                return StoreUser;
            try
            {
                StoreUser oldData = await UOW.StoreUserRepository.Get(StoreUser.Id);
                oldData.Password = HashPassword("appdaily");

                await UOW.StoreUserRepository.Update(oldData);


                var newData = await UOW.StoreUserRepository.Get(StoreUser.Id);
                Sync(new List<StoreUser> { newData });
                Logging.CreateAuditLog(newData, oldData, nameof(StoreUserService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<StoreUser> ForgotPassword(StoreUser StoreUser)
        {
            if (!await StoreUserValidator.ForgotPassword(StoreUser))
                return StoreUser;
            try
            {
                StoreUser oldData = (await UOW.StoreUserRepository.List(new StoreUserFilter
                {
                    Skip = 0,
                    Take = 1,
                    Username = new StringFilter { Equal = StoreUser.Username },
                    Selects = StoreUserSelect.ALL
                })).FirstOrDefault();

                CurrentContext.StoreUserId = oldData.Id;
                var Store = await UOW.StoreRepository.Get(oldData.StoreId);
                oldData.OtpCode = GenerateOTPCode();
                oldData.OtpExpired = StaticParams.DateTimeNow.AddHours(1);


                await UOW.StoreUserRepository.Update(oldData);


                var newData = await UOW.StoreUserRepository.Get(oldData.Id);
                Mail mail = new Mail
                {
                    Subject = "Otp Code",
                    Body = $"Otp Code recovery password: {newData.OtpCode}",
                    Recipients = new List<string> { Store.OwnerEmail },
                    RowId = Guid.NewGuid()
                };
                Sync(new List<StoreUser> { newData });
                RabbitManager.PublishSingle(mail, RoutingKeyEnum.MailSend.Code);
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<StoreUser> VerifyOtpCode(StoreUser StoreUser)
        {
            if (!await StoreUserValidator.VerifyOptCode(StoreUser))
                return StoreUser;

            StoreUser storeUser = (await UOW.StoreUserRepository.List(new StoreUserFilter
            {
                Skip = 0,
                Take = 1,
                Username = new StringFilter { Equal = StoreUser.Username },
                Selects = StoreUserSelect.ALL
            })).FirstOrDefault();
            storeUser.Token = CreateToken(storeUser.Id, storeUser.Username, 300);
            return storeUser;
        }

        public async Task<StoreUser> RecoveryPassword(StoreUser StoreUser)
        {
            if (StoreUser.Id == 0)
                return null;
            try
            {
                StoreUser oldData = await UOW.StoreUserRepository.Get(StoreUser.Id);
                CurrentContext.UserId = StoreUser.Id;
                oldData.Password = HashPassword(StoreUser.Password);

                await UOW.StoreUserRepository.Update(oldData);


                var newData = await UOW.StoreUserRepository.Get(oldData.Id);
                var Store = await UOW.StoreRepository.Get(oldData.StoreId);
                Mail mail = new Mail
                {
                    Subject = "Recovery Password",
                    Body = $"Your password has been recovered.",
                    Recipients = new List<string> { Store.OwnerEmail },
                    RowId = Guid.NewGuid()
                };
                Sync(new List<StoreUser> { newData });
                RabbitManager.PublishSingle(mail, RoutingKeyEnum.MailSend.Code);
                Logging.CreateAuditLog(newData, oldData, nameof(StoreUserService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<StoreUser> LockStoreUser(StoreUser StoreUser)
        {
            if (!await StoreUserValidator.Update(StoreUser))
                return StoreUser;
            try
            {
                var oldData = await UOW.StoreUserRepository.Get(StoreUser.Id);
                StoreUser.Password = oldData.Password;


                await UOW.StoreUserRepository.Update(StoreUser);


                var NewData = await UOW.StoreUserRepository.Get(StoreUser.Id);
                Sync(new List<StoreUser> { NewData });
                Logging.CreateAuditLog(NewData, oldData, nameof(StoreUserService));
                return StoreUser;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<StoreUser> CreateDraft(StoreUser StoreUser)
        {
            if (!await StoreUserValidator.Create(StoreUser))
                return StoreUser;

            try
            {
                var Store = await UOW.StoreRepository.Get(StoreUser.StoreId);
                StoreUser.DisplayName = Store.Name;
                StoreUser.Username = Store.Code.Split('.')[2];
                StoreUser.Password = "appdaily";

                return StoreUser;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<StoreUser> Create(StoreUser StoreUser)
        {
            if (!await StoreUserValidator.Create(StoreUser))
                return StoreUser;

            try
            {
                var Store = await UOW.StoreRepository.Get(StoreUser.StoreId);
                StoreUser.DisplayName = Store.Name;
                StoreUser.Username = Store.Code.Split('.')[2];
                StoreUser.Password = HashPassword("appdaily");


                await UOW.StoreUserRepository.Create(StoreUser);

                StoreUser = await UOW.StoreUserRepository.Get(StoreUser.Id);
                Sync(new List<StoreUser> { StoreUser });
                Logging.CreateAuditLog(StoreUser, new { }, nameof(StoreUserService));
                return StoreUser;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<List<StoreUser>> BulkCreateStoreUser(List<StoreUser> StoreUsers)
        {
            if (!await StoreUserValidator.Import(StoreUsers))
                return StoreUsers;

            try
            {
                foreach (var StoreUser in StoreUsers)
                {
                    StoreUser.Password = HashPassword("appdaily");
                    StoreUser.RowId = Guid.NewGuid();
                }


                await UOW.StoreUserRepository.BulkMerge(StoreUsers);

                Logging.CreateAuditLog(StoreUsers, new { }, nameof(StoreUserService));
                return StoreUsers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<StoreUser> Update(StoreUser StoreUser)
        {
            if (!await StoreUserValidator.Update(StoreUser))
                return StoreUser;
            try
            {
                var oldData = await UOW.StoreUserRepository.Get(StoreUser.Id);
                var Store = await UOW.StoreRepository.Get(StoreUser.StoreId);
                StoreUser.Username = Store.Code.Split('.')[2];
                StoreUser.Password = oldData.Password;


                await UOW.StoreUserRepository.Update(StoreUser);


                StoreUser = await UOW.StoreUserRepository.Get(StoreUser.Id);
                Sync(new List<StoreUser> { StoreUser });
                Logging.CreateAuditLog(StoreUser, oldData, nameof(StoreUserService));
                return StoreUser;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<StoreUser> Delete(StoreUser StoreUser)
        {
            if (!await StoreUserValidator.Delete(StoreUser))
                return StoreUser;

            try
            {

                await UOW.StoreUserRepository.Delete(StoreUser);


                StoreUser = (await UOW.StoreUserRepository.List(new List<long> { StoreUser.Id })).FirstOrDefault();
                Sync(new List<StoreUser> { StoreUser });
                Logging.CreateAuditLog(new { }, StoreUser, nameof(StoreUserService));
                return StoreUser;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<List<StoreUser>> BulkDelete(List<StoreUser> StoreUsers)
        {
            if (!await StoreUserValidator.BulkDelete(StoreUsers))
                return StoreUsers;

            try
            {

                await UOW.StoreUserRepository.BulkDelete(StoreUsers);

                Logging.CreateAuditLog(new { }, StoreUsers, nameof(StoreUserService));

                List<long> Ids = StoreUsers.Select(x => x.Id).ToList();
                StoreUsers = await UOW.StoreUserRepository.List(Ids);
                Sync(StoreUsers);
                return StoreUsers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<List<StoreUser>> Import(List<StoreUser> StoreUsers)
        {
            if (!await StoreUserValidator.Import(StoreUsers))
                return StoreUsers;
            try
            {

                await UOW.StoreUserRepository.BulkMerge(StoreUsers);


                Logging.CreateAuditLog(StoreUsers, new { }, nameof(StoreUserService));
                return StoreUsers;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreUserService));
            }
            return null;
        }

        public async Task<StoreUserFilter> ToFilter(StoreUserFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<StoreUserFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                StoreUserFilter subFilter = new StoreUserFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StoreId))
                        subFilter.StoreId = FilterBuilder.Merge(subFilter.StoreId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Username))
                        subFilter.Username = FilterBuilder.Merge(subFilter.Username, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.DisplayName))
                        subFilter.DisplayName = FilterBuilder.Merge(subFilter.DisplayName, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Password))
                        subFilter.Password = FilterBuilder.Merge(subFilter.Password, FilterPermissionDefinition.StringFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StatusId))
                        subFilter.StatusId = FilterBuilder.Merge(subFilter.StatusId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                        }
                    }
                }
            }
            return filter;
        }

        private string HashPassword(string password)
        {
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);
            string savedPasswordHash = Convert.ToBase64String(hashBytes);
            return savedPasswordHash;
        }

        private string CreateToken(long id, string userName, double? expiredTime = null)
        {
            var secretKey = Configuration["Config:SecretKey"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Name, userName)
                }),
                Expires = StaticParams.DateTimeNow.AddSeconds(86400000),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            SecurityToken SecurityToken = tokenHandler.CreateToken(tokenDescriptor);
            string Token = tokenHandler.WriteToken(SecurityToken);
            return Token;
        }

        private string GenerateOTPCode()
        {
            Random rand = new Random();
            return rand.Next(100000, 999999).ToString();
        }

        public async Task<string> SaveImage(Image Image)
        {
            FileInfo fileInfo = new FileInfo(Image.Name);
            string path = $"/avatar/{StaticParams.DateTimeNow.ToString("yyyyMMdd")}/{Guid.NewGuid()}{fileInfo.Extension}";
            RestClient restClient = new RestClient($"http://localhost:{Modules.Utils}");
            RestRequest restRequest = new RestRequest("/rpc/utils/file/upload");
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Method = Method.POST;
            restRequest.AddCookie("Token", CurrentContext.Token);
            restRequest.AddCookie("X-Language", CurrentContext.Language);
            restRequest.AddHeader("Content-Type", "multipart/form-data");
            restRequest.AddFile("file", Image.Content, Image.Name);
            restRequest.AddParameter("path", path);
            try
            {
                var response = await restClient.ExecuteAsync<File>(restRequest);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    Image.Id = response.Data.Id;
                    Image.Url = "/rpc/utils/file/download" + response.Data.Path;
                    return Image.Url;
                }
            }
            catch
            {
                return null;
            }
            return null;
        }

        private void Sync(List<StoreUser> StoreUsers)
        {
            List<Store> Stores = StoreUsers.Select(x => new Store { Id = x.StoreId }).Distinct().ToList();
            RabbitManager.PublishList(StoreUsers, RoutingKeyEnum.StoreUserSync.Code);
            RabbitManager.PublishList(Stores, RoutingKeyEnum.StoreUsed.Code);
        }
    }

    public class File
    {
        public long Id { get; set; }
        public string Path { get; set; }
    }
}
