using DMS.Common;
using DMS.Entities;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MPermission
{
    public interface IPermissionValidator : IServiceScoped
    {
        Task<bool> Create(Permission Permission);
        Task<bool> Update(Permission Permission);
        Task<bool> Delete(Permission Permission);
    }

    public class PermissionValidator : IPermissionValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            CodeExisted,
            CodeNotExisted,
            CodeEmpty,
            CodeOverLength,
            NameEmpty,
            NameOverLength,
            NameExisted,
            StatusNotExisted,
            PageNotExisted,
            FieldNotExisted,
            PermissionOperatorNotExisted,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public PermissionValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(Permission Permission)
        {
            PermissionFilter PermissionFilter = new PermissionFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Permission.Id },
                Selects = PermissionSelect.Id
            };

            int count = await UOW.PermissionRepository.Count(PermissionFilter);
            if (count == 0)
                Permission.AddError(nameof(PermissionValidator), nameof(Permission.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        public async Task<bool> ValidateCode(Permission Permission)
        {
            if (string.IsNullOrWhiteSpace(Permission.Code))
                Permission.AddError(nameof(PermissionValidator), nameof(Permission.Code), ErrorCode.CodeEmpty);
            else
            {
                if (Permission.Code.Length > 255)
                    Permission.AddError(nameof(PermissionValidator), nameof(Permission.Code), ErrorCode.CodeOverLength);
                PermissionFilter PermissionFilter = new PermissionFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = Permission.Id },
                    Code = new StringFilter { Equal = Permission.Code },
                    MenuId = new IdFilter { Equal = Permission.MenuId },
                    RoleId = new IdFilter { Equal = Permission.RoleId },
                    Selects = PermissionSelect.Code
                };

                int count = await UOW.PermissionRepository.Count(PermissionFilter);
                if (count != 0)
                    Permission.AddError(nameof(PermissionValidator), nameof(Permission.Code), ErrorCode.CodeExisted);
            }
            return Permission.IsValidated;
        }

        public async Task<bool> ValidateName(Permission Permission)
        {
            if (string.IsNullOrWhiteSpace(Permission.Name))
                Permission.AddError(nameof(PermissionValidator), nameof(Permission.Name), ErrorCode.NameEmpty);
            else
            {
                if (Permission.Name.Length > 255)
                    Permission.AddError(nameof(PermissionValidator), nameof(Permission.Name), ErrorCode.NameOverLength);
                PermissionFilter PermissionFilter = new PermissionFilter
                {
                    Skip = 0,
                    Take = 10,
                    Id = new IdFilter { NotEqual = Permission.Id },
                    Name = new StringFilter { Equal = Permission.Name },
                    MenuId = new IdFilter { Equal = Permission.MenuId },
                    RoleId = new IdFilter { Equal = Permission.RoleId },
                    Selects = PermissionSelect.Name
                };

                int count = await UOW.PermissionRepository.Count(PermissionFilter);
                if (count != 0)
                    Permission.AddError(nameof(PermissionValidator), nameof(Permission.Name), ErrorCode.NameExisted);
            }
            return Permission.IsValidated;

        }

        public async Task<bool> ValidateStatus(Permission Permission)
        {
            StatusFilter StatusFilter = new StatusFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = Permission.StatusId },
                Selects = StatusSelect.Id
            };
            int count = await UOW.StatusRepository.Count(StatusFilter);
            if (count == 0)
                Permission.AddError(nameof(PermissionValidator), nameof(Permission.Status), ErrorCode.StatusNotExisted);
            return count != 0;
        }

        public async Task<bool> Create(Permission Permission)
        {
            await ValidateCode(Permission);
            await ValidateName(Permission);
            await ValidateStatus(Permission);
            await ValidatePermissionContent(Permission);
            return Permission.IsValidated;
        }

        public async Task<bool> Update(Permission Permission)
        {
            if (await ValidateId(Permission))
            {
                await ValidateCode(Permission);
                await ValidateName(Permission);
                await ValidateStatus(Permission);
                await ValidatePermissionContent(Permission);
            }
            return Permission.IsValidated;
        }

        public async Task<bool> Delete(Permission Permission)
        {
            if (await ValidateId(Permission))
            {
            }
            return Permission.IsValidated;
        }

        public async Task<bool> ValidatePermissionContent(Permission Permission)
        {
            if (Permission.PermissionContents != null)
            {
                List<Field> Fields = await UOW.FieldRepository.List(new FieldFilter
                {
                    Selects = FieldSelect.ALL,
                    Skip = 0,
                    Take = int.MaxValue
                });
                List<PermissionOperator> PermissionOperators = await UOW.PermissionOperatorRepository.List(new PermissionOperatorFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                });

                foreach (PermissionContent PermissionContent in Permission.PermissionContents)
                {
                    Field Field = Fields.Where(f => f.Id == PermissionContent.FieldId).FirstOrDefault();
                    if (Field == null)
                    {
                        PermissionContent.AddError(nameof(PermissionValidator), nameof(PermissionContent.Field), ErrorCode.FieldNotExisted);
                    }
                    else
                    {
                        PermissionOperator PermissionOperator = PermissionOperators
                            .Where(po => po.FieldTypeId == Field.FieldTypeId && po.Id == PermissionContent.PermissionOperatorId).FirstOrDefault();
                        if (PermissionOperator == null)
                            PermissionContent.AddError(nameof(PermissionValidator), nameof(PermissionContent.PermissionOperator), ErrorCode.PermissionOperatorNotExisted);
                    }
                }
            }
            return Permission.IsValidated;
        }
    }
}
