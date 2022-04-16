using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Handlers.Configuration;
using DMS.ABE.Helpers;
using DMS.ABE.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DMS.ABE.Services.MStoreUser
{
    public interface IStoreUserProfileService : IServiceScoped
    {
        Task<StoreUser> Login(StoreUser StoreUser);
        Task<StoreUser> ChangePassword(StoreUser StoreUser);
        Task<StoreUser> ResetPassword(StoreUser StoreUser);
        Task<StoreUser> ForgotPassword(StoreUser StoreUser);
        Task<StoreUser> VerifyOtpCode(StoreUser StoreUser);
        Task<StoreUser> RecoveryPassword(StoreUser StoreUser);
        Task<StoreUser> Update(StoreUser StoreUser);
        Task<bool> ToggleFavoriteProduct(long FavoriteProductId, long StoreUserId, bool IsFavorite);
    }

    public class StoreUserProfileService : BaseService, IStoreUserProfileService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IStoreUserValidator StoreUserValidator;
        private IConfiguration Configuration;
        private IRabbitManager RabbitManager;

        public StoreUserProfileService(
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

        public async Task<StoreUser> Login(StoreUser StoreUser)
        {
            if (!await StoreUserValidator.Login(StoreUser))
                return StoreUser;
            StoreUser = await UOW.StoreUserRepository.Get(StoreUser.Id);
            CurrentContext.StoreUserId = StoreUser.Id;
            Logging.CreateAuditLog(new { }, StoreUser, nameof(StoreUserService));
            StoreUser.Token = CreateToken(StoreUser.Id, StoreUser.RowId.ToString(), StoreUser.Username);

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
                await UOW.Begin();
                await UOW.StoreUserRepository.Update(oldData);
                await UOW.Commit();
                var newData = await UOW.StoreUserRepository.Get(StoreUser.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(StoreUserService));
                return newData;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();

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
                oldData.Password = HashPassword("appdailyrangdong");
                await UOW.Begin();
                await UOW.StoreUserRepository.Update(oldData);
                await UOW.Commit();

                var newData = await UOW.StoreUserRepository.Get(StoreUser.Id);

                Logging.CreateAuditLog(newData, oldData, nameof(StoreUserService));
                return newData;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();

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

                await UOW.Begin();
                await UOW.StoreUserRepository.Update(oldData);
                await UOW.Commit();

                var newData = await UOW.StoreUserRepository.Get(oldData.Id);
                Mail mail = new Mail
                {
                    Subject = "Otp Code",
                    Body = $"Otp Code recovery password: {newData.OtpCode}",
                    Recipients = new List<string> { Store.OwnerEmail },
                    RowId = Guid.NewGuid()
                };
                RabbitManager.PublishSingle(mail, RoutingKeyEnum.MailSend.Code);
                return newData;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();

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
            storeUser.Token = CreateToken(storeUser.Id, storeUser.RowId.ToString(), storeUser.Username, 300);
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
                await UOW.Begin();
                await UOW.StoreUserRepository.Update(oldData);
                await UOW.Commit();

                var newData = await UOW.StoreUserRepository.Get(oldData.Id);
                var Store = await UOW.StoreRepository.Get(oldData.StoreId);
                Mail mail = new Mail
                {
                    Subject = "Recovery Password",
                    Body = $"Your password has been recovered.",
                    Recipients = new List<string> { Store.OwnerEmail },
                    RowId = Guid.NewGuid()
                };
                RabbitManager.PublishSingle(mail, RoutingKeyEnum.MailSend.Code);
                Logging.CreateAuditLog(newData, oldData, nameof(StoreUserService));
                return newData;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();

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

                await UOW.Begin();
                await UOW.StoreUserRepository.Update(StoreUser);
                await UOW.Commit();

                StoreUser = await UOW.StoreUserRepository.Get(StoreUser.Id);
                Logging.CreateAuditLog(StoreUser, oldData, nameof(StoreUserService));
                return StoreUser;
            }
            catch (Exception ex)
            {
                await UOW.Rollback();

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

        private string CreateToken(long id, string rowId, string userName, double? expiredTime = null)
        {
            var secretKey = Configuration["Config:SecretKey"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, id.ToString()),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.PrimarySid, rowId),
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

        public async Task<bool> ToggleFavoriteProduct(long FavoriteProductId, long StoreUserId, bool IsFavorite)
        {
            List<StoreUser> StoreUsers = await UOW.StoreUserRepository.List(
                new StoreUserFilter
                {
                    Id = new IdFilter { Equal = StoreUserId },
                    Skip = 0,
                    Take = 1,
                    Selects = StoreUserSelect.ALL
                }
            );
            StoreUser StoreUser = StoreUsers.FirstOrDefault();
            if (StoreUser == null)
            {
                return false;
            }

            ProductFilter ProductFilter = new ProductFilter
            {
                Id = new IdFilter { Equal = FavoriteProductId },
                Skip = 0,
                Take = 1,
                Selects = ProductSelect.Id
            };

            List<Product> Products;
            Products = await UOW.ProductRepository.List(ProductFilter);

            Product Product = Products.FirstOrDefault();
            if (Product == null)
            {
                return false;
            }
            if (IsFavorite)
            {
                await UOW.StoreUserFavoriteProductMappingRepository.Update(StoreUser.Id, Product.Id);
            } // neu thich san pham thi them 
            else
            {
                await UOW.StoreUserFavoriteProductMappingRepository.Delete(StoreUser.Id, Product.Id);
            } // ko thich thi xoa mapping
            return true;
        }
    }
}
