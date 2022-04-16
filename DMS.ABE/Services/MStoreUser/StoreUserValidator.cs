using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DMS.ABE.Services.MStoreUser
{
    public interface IStoreUserValidator : IServiceScoped
    {
        Task<bool> Login(StoreUser StoreUser);
        Task<bool> ChangePassword(StoreUser StoreUser);
        Task<bool> ResetPassword(StoreUser StoreUser);
        Task<bool> ForgotPassword(StoreUser StoreUser);
        Task<bool> VerifyOptCode(StoreUser StoreUser);
        Task<bool> Create(StoreUser StoreUser);
        Task<bool> Update(StoreUser StoreUser);
        Task<bool> Delete(StoreUser StoreUser);
        Task<bool> BulkDelete(List<StoreUser> StoreUsers);
        Task<bool> Import(List<StoreUser> StoreUsers);
    }

    public class StoreUserValidator : IStoreUserValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            StoreEmpty,
            StoreNotExisted,
            UsernameNotExisted,
            PasswordNotMatch,
            EmailEmpty,
            OtpCodeInvalid,
            OtpExpired,
            StoreUserExisted,
            StoreUserLocked,
            StoreEmailNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public StoreUserValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(StoreUser StoreUser)
        {
            StoreUserFilter StoreUserFilter = new StoreUserFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = StoreUser.Id },
                Selects = StoreUserSelect.Id
            };

            int count = await UOW.StoreUserRepository.Count(StoreUserFilter);
            if (count == 0)
                StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateStore(StoreUser StoreUser)
        {
            if (StoreUser.StoreId == 0)
                StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Store), ErrorCode.StoreEmpty);
            else
            {
                StoreFilter StoreFilter = new StoreFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { Equal = StoreUser.StoreId },
                    StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id },
                    Selects = StoreSelect.Id
                };

                int count = await UOW.StoreRepository.Count(StoreFilter);
                if (count == 0)
                    StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Store), ErrorCode.StoreNotExisted);
                else
                {
                    StoreUserFilter StoreUserFilter = new StoreUserFilter
                    {
                        Skip = 0,
                        Take = 1,
                        StoreId = new IdFilter { Equal = StoreUser.StoreId }
                    };

                    count = await UOW.StoreUserRepository.Count(StoreUserFilter);
                    if (count > 0)
                    {
                        StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Store), ErrorCode.StoreUserExisted);
                    }
                }
            }

            return StoreUser.IsValidated;
        }

        public async Task<bool> Login(StoreUser StoreUser)
        {
            if (string.IsNullOrWhiteSpace(StoreUser.Username))
            {
                StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Username), ErrorCode.UsernameNotExisted);
                return false;
            }
            List<StoreUser> StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
            {
                Skip = 0,
                Take = 1,
                Username = new StringFilter { Equal = StoreUser.Username },
                Selects = StoreUserSelect.ALL,
            });
            if (StoreUsers.Count == 0)
            {
                StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Username), ErrorCode.UsernameNotExisted);
            }
            else
            {
                StoreUser storeUser = StoreUsers.FirstOrDefault();
                if (storeUser.StatusId == StatusEnum.INACTIVE.Id)
                {
                    StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Status), ErrorCode.StoreUserLocked);
                }
                else
                {
                    bool verify = VerifyPassword(storeUser.Password, StoreUser.Password);
                    if (verify == false)
                    {
                        StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Password), ErrorCode.PasswordNotMatch);
                    }
                    else
                    {
                        StoreUser.Id = storeUser.Id;
                    }
                }
            }
            return StoreUser.IsValidated;
        }

        private bool VerifyPassword(string oldPassword, string newPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(oldPassword);
            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);
            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(newPassword, salt, 10000);
            byte[] hash = pbkdf2.GetBytes(20);
            /* Compare the results */
            for (int i = 0; i < 20; i++)
                if (hashBytes[i + 16] != hash[i])
                    return false;
            return true;
        }

        public async Task<bool> ChangePassword(StoreUser StoreUser)
        {
            //List<StoreUser> StoreUsers = await UOW.StoreUserRepository.List(new StoreUserFilter
            //{
            //    Skip = 0,
            //    Take = 1,
            //    Id = new IdFilter { Equal = StoreUser.Id },
            //    Selects = StoreUserSelect.ALL,
            //});
            //if (StoreUsers.Count == 0)
            //{
            //    StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Username), ErrorCode.IdNotExisted);
            //}
            //else
            //{
            //    StoreUser storeUser = StoreUsers.FirstOrDefault();
            //    bool verify = VerifyPassword(storeUser.Password, StoreUser.Password);
            //    if (verify == false)
            //    {
            //        StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Password), ErrorCode.PasswordNotMatch);
            //    }
            //    List<Store> Stores = await UOW.StoreRepository.List(new StoreFilter
            //    {
            //        Id = new IdFilter { Equal = storeUser.StoreId },
            //        Skip = 0,
            //        Take = 1,
            //        Selects = StoreSelect.Id | StoreSelect.OwnerEmail
            //    });

            //    Store OwnerStore = Stores.FirstOrDefault();
            //    if (OwnerStore.OwnerEmail == null)
            //    {
            //        StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Store), ErrorCode.StoreEmailNotExisted);
            //    }
            //}
            //return StoreUser.IsValidated;
            if (await ValidateId(StoreUser))
            {
            }
            return StoreUser.IsValidated;
        }

        public async Task<bool> ResetPassword(StoreUser StoreUser)
        {
            await ValidateId(StoreUser);
            return StoreUser.IsValidated;
        }

        public async Task<bool> ForgotPassword(StoreUser StoreUser)
        {
            StoreUser = (await UOW.StoreUserRepository.List(new StoreUserFilter
            {
                Skip = 0,
                Take = 1,
                Selects = StoreUserSelect.ALL,
                Username = new StringFilter { Equal = StoreUser.Username }
            })).FirstOrDefault();
            if (StoreUser != null)
            {
                var Store = await UOW.StoreRepository.Get(StoreUser.StoreId);
                if (Store != null)
                {
                    if (string.IsNullOrWhiteSpace(Store.OwnerEmail))
                    {
                        StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Id), ErrorCode.EmailEmpty);
                    }
                }
            }
            else
            {
                StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Username), ErrorCode.UsernameNotExisted);
            }

            return StoreUser.IsValidated;
        }

        public async Task<bool> VerifyOptCode(StoreUser StoreUser)
        {
            StoreUser oldData = (await UOW.StoreUserRepository.List(new StoreUserFilter
            {
                Skip = 0,
                Take = 1,
                Username = new StringFilter { Equal = StoreUser.Username },
                Selects = StoreUserSelect.ALL
            })).FirstOrDefault();
            if (oldData == null)
                StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.Id), ErrorCode.UsernameNotExisted);
            if (oldData.OtpCode != StoreUser.OtpCode)
            {
                StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.OtpCode), ErrorCode.OtpCodeInvalid);
            }
            if (DateTime.Now > oldData.OtpExpired)
            {
                StoreUser.AddError(nameof(StoreUserValidator), nameof(StoreUser.OtpExpired), ErrorCode.OtpExpired);
            }

            return StoreUser.IsValidated;
        }

        public async Task<bool> Create(StoreUser StoreUser)
        {
            await ValidateStore(StoreUser);
            return StoreUser.IsValidated;
        }

        public async Task<bool> Update(StoreUser StoreUser)
        {
            if (await ValidateId(StoreUser))
            {
            }
            return StoreUser.IsValidated;
        }

        public async Task<bool> Delete(StoreUser StoreUser)
        {
            if (await ValidateId(StoreUser))
            {
            }
            return StoreUser.IsValidated;
        }

        public async Task<bool> BulkDelete(List<StoreUser> StoreUsers)
        {
            foreach (StoreUser StoreUser in StoreUsers)
            {
                await Delete(StoreUser);
            }
            return StoreUsers.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<StoreUser> StoreUsers)
        {
            var Ids = StoreUsers.Select(x => x.StoreId).ToList();
            StoreUserFilter StoreUserFilter = new StoreUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                StoreId = new IdFilter { In = Ids },
                Selects = StoreUserSelect.Id | StoreUserSelect.Store
            };

            var list = await UOW.StoreUserRepository.List(StoreUserFilter);
            foreach (var item in list)
            {
                var user = StoreUsers.Where(x => x.StoreId == item.StoreId).FirstOrDefault();
                if (user != null)
                {
                    user.AddError(nameof(StoreUserValidator), nameof(user.Store), ErrorCode.StoreUserExisted);
                }
            }
            return StoreUsers.All(x => x.IsValidated);
        }
    }
}
