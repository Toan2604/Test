using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MPermission;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MRole
{
    public interface IPermissionService : IServiceScoped
    {
        Task<int> Count(PermissionFilter PermissionFilter);
        Task<List<Permission>> List(PermissionFilter PermissionFilter);
        Task<Permission> Get(long Id);
        Task<Permission> Create(Permission Permission);
        Task<Permission> Update(Permission Permission);
        Task<Permission> Delete(Permission Permission);
        Task<List<string>> ListPath(long AppUserId);
    }
    public class PermissionService : IPermissionService
    {
        private IUOW UOW;
        private IPermissionValidator PermissionValidator;
        private ILogging Logging;
        public PermissionService(IUOW UOW, IPermissionValidator PermissionValidator, ILogging Logging)
        {
            this.UOW = UOW;
            this.PermissionValidator = PermissionValidator;
            this.Logging = Logging;
        }

        public async Task<int> Count(PermissionFilter PermissionFilter)
        {
            try
            {
                return await UOW.PermissionRepository.Count(PermissionFilter);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PermissionService));
            }
            return 0;
        }

        public async Task<List<Permission>> List(PermissionFilter PermissionFilter)
        {
            try
            {
                List<Permission> Permissions = await UOW.PermissionRepository.List(PermissionFilter);
                return Permissions;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PermissionService));
            }
            return null;
        }
        public async Task<Permission> Get(long Id)
        {
            try
            {
                Permission Permission = await UOW.PermissionRepository.Get(Id);
                return Permission;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PermissionService));
            }
            return null;
        }

        public async Task<Permission> Create(Permission Permission)
        {
            if (!await PermissionValidator.Create(Permission))
                return Permission;
            try
            {
                await UOW.PermissionRepository.Create(Permission);
                return await UOW.PermissionRepository.Get(Permission.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PermissionService));
            }
            return null;
        }

        public async Task<Permission> Update(Permission Permission)
        {
            if (!await PermissionValidator.Update(Permission))
                return Permission;
            try
            {
                await UOW.PermissionRepository.Update(Permission);
                return await UOW.PermissionRepository.Get(Permission.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PermissionService));
            }
            return null;
        }

        public async Task<Permission> Delete(Permission Permission)
        {
            if (!await PermissionValidator.Delete(Permission))
                return Permission;

            try
            {
                await UOW.PermissionRepository.Delete(Permission);
                return Permission;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PermissionService));
            }
            return null;
        }

        public async Task<List<string>> ListPath(long AppUserId)
        {
            try
            {
                List<string> Paths = await UOW.PermissionRepository.ListPath(AppUserId);
                return Paths;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(PermissionService));
            }
            return null;
        }
    }
}
