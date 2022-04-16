using DMS.Common;
using DMS.Entities;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MRole
{
    public interface IRoleService : IServiceScoped
    {
        Task<int> Count(RoleFilter RoleFilter);
        Task<List<Role>> List(RoleFilter RoleFilter);
        Task<Role> Get(long Id);
        Task<Role> Create(Role Role);
        Task<Role> Clone(long Id);
        Task<Role> Update(Role Role);
        Task<Role> AssignAppUser(Role Role);
        Task<Role> Delete(Role Role);
        Task<List<Role>> BulkDelete(List<Role> Roles);
        Task<List<Role>> Import(List<Role> Roles);
        RoleFilter ToFilter(RoleFilter RoleFilter);
    }

    public class RoleService : BaseService, IRoleService
    {
        private IUOW UOW;
        private ILogging Logging;
        private ICurrentContext CurrentContext;
        private IRoleValidator RoleValidator;

        public RoleService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IRoleValidator RoleValidator
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.RoleValidator = RoleValidator;
        }
        public async Task<int> Count(RoleFilter RoleFilter)
        {
            try
            {
                int result = await UOW.RoleRepository.Count(RoleFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RoleService));
            }
            return 0;
        }

        public async Task<List<Role>> List(RoleFilter RoleFilter)
        {
            try
            {
                List<Role> Roles = await UOW.RoleRepository.List(RoleFilter);
                return Roles;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RoleService));
            }
            return null;
        }
        public async Task<Role> Get(long Id)
        {
            Role Role = await UOW.RoleRepository.Get(Id);
            if (Role == null)
                return null;
            return Role;
        }

        public async Task<Role> Clone(long Id)
        {
            try
            {
                Role Role = await UOW.RoleRepository.Get(Id);
                var listRolesInDb = await UOW.RoleRepository.List(new RoleFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Code = new StringFilter { StartWith = Role.Code + "_Clone" },
                    Name = new StringFilter { StartWith = Role.Name + "_Clone" },
                    Selects = RoleSelect.ALL
                });

                var listPermissionsInDb = await UOW.PermissionRepository.List(new PermissionFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    RoleId = new IdFilter { Equal = Role.Id },
                    Selects = PermissionSelect.ALL
                });

                for (int index = 1; index < 1000; index++)
                {
                    if (listRolesInDb.Any(x => x.Code == Role.Code + "_Clone" + index.ToString() && x.Name == Role.Name + "_Clone" + index.ToString()))
                        continue;
                    Role.Code = Role.Code + "_Clone" + index.ToString();
                    Role.Name = Role.Name + "_Clone" + index.ToString();
                    break;
                }
                Role.Id = 0;
                Role.AppUserRoleMappings = new List<AppUserRoleMapping>();

                await UOW.RoleRepository.Create(Role);
                foreach (Permission Permission in listPermissionsInDb)
                {
                    Permission.RoleId = Role.Id;
                }
                await UOW.PermissionRepository.BulkMerge(listPermissionsInDb);


                Logging.CreateAuditLog(Role, new { }, nameof(RoleService));
                return await UOW.RoleRepository.Get(Role.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RoleService));
            }
            return null;
        }
        public async Task<Role> Create(Role Role)
        {
            if (!await RoleValidator.Create(Role))
                return Role;

            try
            {
                Role.Id = 0;

                await UOW.RoleRepository.Create(Role);


                Logging.CreateAuditLog(Role, new { }, nameof(RoleService));
                return await UOW.RoleRepository.Get(Role.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RoleService));
            }
            return null;
        }

        public async Task<Role> Update(Role Role)
        {
            if (!await RoleValidator.Update(Role))
                return Role;
            try
            {
                var oldData = await UOW.RoleRepository.Get(Role.Id);


                Role.AppUserRoleMappings = oldData.AppUserRoleMappings;
                await UOW.RoleRepository.Update(Role);


                var newData = await UOW.RoleRepository.Get(Role.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(RoleService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RoleService));
            }
            return null;
        }

        public async Task<Role> AssignAppUser(Role Role)
        {
            if (!await RoleValidator.AssignAppUser(Role))
                return Role;
            try
            {
                var oldData = await UOW.RoleRepository.Get(Role.Id);
                oldData.AppUserRoleMappings = Role.AppUserRoleMappings;

                await UOW.RoleRepository.Update(oldData);


                var newData = await UOW.RoleRepository.Get(Role.Id);
                Logging.CreateAuditLog(newData, oldData, nameof(RoleService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RoleService));
            }
            return null;
        }

        public async Task<Role> Delete(Role Role)
        {
            if (!await RoleValidator.Delete(Role))
                return Role;

            try
            {

                await UOW.RoleRepository.Delete(Role);

                Logging.CreateAuditLog(new { }, Role, nameof(RoleService));
                return Role;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RoleService));
            }
            return null;
        }

        public async Task<List<Role>> BulkDelete(List<Role> Roles)
        {
            if (!await RoleValidator.BulkDelete(Roles))
                return Roles;

            try
            {

                await UOW.RoleRepository.BulkDelete(Roles);

                Logging.CreateAuditLog(new { }, Roles, nameof(RoleService));
                return Roles;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RoleService));
            }
            return null;
        }

        public async Task<List<Role>> Import(List<Role> Roles)
        {
            if (!await RoleValidator.Import(Roles))
                return Roles;

            try
            {
                Roles.ForEach(r => r.Id = 0);

                await UOW.RoleRepository.BulkMerge(Roles);
                var listRolesInDb = await UOW.RoleRepository.List(new RoleFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = RoleSelect.ALL
                });

                List<Permission> Permissions = new List<Permission>();
                foreach (var Role in Roles)
                {
                    var r = listRolesInDb.Where(r => r.Code == Role.Code).FirstOrDefault();
                    Role.Id = r != null ? r.Id : 0;
                    if (Role.Permissions.Any())
                    {
                        foreach (var Permission in Role.Permissions)
                        {
                            Permission.RoleId = Role.Id;
                            Permissions.Add(Permission);
                        }
                    }
                }
                var listPermissionInDB = Roles.SelectMany(r => r.Permissions).ToList();
                await UOW.PermissionRepository.BulkDelete(listPermissionInDB);
                await UOW.PermissionRepository.BulkMerge(Permissions);


                Logging.CreateAuditLog(Roles, new { }, nameof(RoleService));
                return Roles;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(RoleService));
            }
            return null;
        }

        public RoleFilter ToFilter(RoleFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<RoleFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                RoleFilter subFilter = new RoleFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                }
            }
            return filter;
        }
    }
}
