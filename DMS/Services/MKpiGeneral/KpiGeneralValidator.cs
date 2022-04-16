using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiGeneral
{
    public interface IKpiGeneralValidator : IServiceScoped
    {
        Task<bool> Create(KpiGeneral KpiGeneral);
        Task<bool> Update(KpiGeneral KpiGeneral);
        Task<bool> Delete(KpiGeneral KpiGeneral);
        Task<bool> BulkDelete(List<KpiGeneral> KpiGenerals);
        Task<bool> Import(List<KpiGeneral> KpiGenerals);
    }

    public class KpiGeneralValidator : IKpiGeneralValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            OrganizationIdNotExisted,
            OrganizationEmpty,
            EmployeeIdsEmpty,
            StatusNotExisted,
            KpiYearIdNotExisted,
            KpiYearIdIsInThePast,
            KpiYearExisted,
            KpiGeneralContentsEmpty,
            ValueCannotBeNull,
            KpiGeneralContentFormIsWrong,
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiGeneralValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiGeneral KpiGeneral)
        {
            KpiGeneralFilter KpiGeneralFilter = new KpiGeneralFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiGeneral.Id },
                Selects = KpiGeneralSelect.Id
            };

            int count = await UOW.KpiGeneralRepository.Count(KpiGeneralFilter);
            if (count == 0)
                KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateOrganization(KpiGeneral KpiGeneral)
        {
            if (KpiGeneral.OrganizationId == 0)
            {
                KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.Organization), ErrorCode.OrganizationEmpty);
            }
            else
            {
                OrganizationFilter OrganizationFilter = new OrganizationFilter
                {
                    Id = new IdFilter { Equal = KpiGeneral.OrganizationId }
                };

                var count = await UOW.OrganizationRepository.Count(OrganizationFilter);
                if (count == 0)
                    KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.Organization), ErrorCode.OrganizationIdNotExisted);
            }
            return KpiGeneral.IsValidated;
        }

        private async Task<bool> ValidateEmployees(KpiGeneral KpiGeneral)
        {
            if (KpiGeneral.EmployeeIds == null || !KpiGeneral.EmployeeIds.Any())
                KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.EmployeeIds), ErrorCode.EmployeeIdsEmpty);
            else
            {
                AppUserFilter AppUserFilter = new AppUserFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Id = new IdFilter { In = KpiGeneral.EmployeeIds },
                    OrganizationId = new IdFilter(),
                    Selects = AppUserSelect.Id
                };

                var EmployeeIdsInDB = (await UOW.AppUserRepository.List(AppUserFilter)).Select(x => x.Id).ToList();
                var listIdsNotExisted = KpiGeneral.EmployeeIds.Except(EmployeeIdsInDB).ToList();

                if (listIdsNotExisted != null && listIdsNotExisted.Any())
                {
                    foreach (var Id in listIdsNotExisted)
                    {
                        KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.EmployeeIds), ErrorCode.IdNotExisted);
                    }
                }

            }
            return KpiGeneral.IsValidated;
        }

        private async Task<bool> ValidateStatus(KpiGeneral KpiGeneral)
        {
            if (StatusEnum.ACTIVE.Id != KpiGeneral.StatusId && StatusEnum.INACTIVE.Id != KpiGeneral.StatusId)
                KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.Status), ErrorCode.StatusNotExisted);
            return KpiGeneral.IsValidated;
        }

        private async Task<bool> ValidateKpiGeneralContent(KpiGeneral KpiGeneral)
        {
            KpiPeriodFilter KpiPeriodFilter = new KpiPeriodFilter();

            int count = await UOW.KpiPeriodRepository.Count(KpiPeriodFilter);
            foreach (var content in KpiGeneral.KpiGeneralContents)
            {
                int countPeriod = content.KpiGeneralContentKpiPeriodMappings.Select(x => x.KpiPeriodId).Distinct().Count();
                if (countPeriod != count)
                {
                    KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.KpiGeneralContents), ErrorCode.KpiGeneralContentFormIsWrong);
                }
            }
            return KpiGeneral.IsValidated;
        }

        private async Task<bool> ValidateKpiYear(KpiGeneral KpiGeneral)
        {
            KpiYearFilter KpiYearFilter = new KpiYearFilter
            {
                Id = new IdFilter { Equal = KpiGeneral.KpiYearId }
            };

            int count = await UOW.KpiYearRepository.Count(KpiYearFilter);
            if (count == 0)
                KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.KpiYear), ErrorCode.KpiYearIdNotExisted);
            if (KpiGeneral.KpiYearId < StaticParams.DateTimeNow.Year)
                KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.KpiYear), ErrorCode.KpiYearIdIsInThePast);
            return KpiGeneral.IsValidated;
        }
        private async Task<bool> ValidateKpiGeneral(KpiGeneral KpiGeneral)
        {
            if (KpiGeneral.EmployeeIds == null || !KpiGeneral.EmployeeIds.Any())
                KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.EmployeeIds), ErrorCode.EmployeeIdsEmpty);
            else
                foreach (var id in KpiGeneral.EmployeeIds)
                {
                    KpiGeneralFilter KpiGeneralFilter = new KpiGeneralFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        AppUserId = new IdFilter { Equal = id },
                        KpiYearId = new IdFilter { Equal = KpiGeneral.KpiYearId },
                        Selects = KpiGeneralSelect.Employee | KpiGeneralSelect.KpiYear
                    };
                    int count = await UOW.KpiGeneralRepository.Count(KpiGeneralFilter);
                    if (count != 0)
                        KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.KpiYear), ErrorCode.KpiYearExisted);
                }
            return KpiGeneral.IsValidated;
        }

        private async Task<bool> ValidateValue(KpiGeneral KpiGeneral)
        {
            bool flag = false;
            foreach (var KpiGeneralContent in KpiGeneral.KpiGeneralContents)
            {
                foreach (var item in KpiGeneralContent.KpiGeneralContentKpiPeriodMappings)
                {
                    if (item.Value != null) flag = true;
                }
            }
            if (!flag) KpiGeneral.AddError(nameof(KpiGeneralValidator), nameof(KpiGeneral.Id), ErrorCode.ValueCannotBeNull);
            return KpiGeneral.IsValidated;
        }
        public async Task<bool> Create(KpiGeneral KpiGeneral)
        {
            await ValidateOrganization(KpiGeneral);
            await ValidateEmployees(KpiGeneral);
            await ValidateStatus(KpiGeneral);
            await ValidateKpiYear(KpiGeneral);
            await ValidateValue(KpiGeneral);
            await ValidateKpiGeneral(KpiGeneral);
            await ValidateKpiGeneralContent(KpiGeneral);
            return KpiGeneral.IsValidated;
        }

        public async Task<bool> Update(KpiGeneral KpiGeneral)
        {
            if (await ValidateId(KpiGeneral))
            {
                await ValidateOrganization(KpiGeneral);
                await ValidateStatus(KpiGeneral);
                await ValidateValue(KpiGeneral);
                await ValidateKpiGeneralContent(KpiGeneral);
                //await ValidateKpiGeneral(KpiGeneral);
            }
            return KpiGeneral.IsValidated;
        }

        public async Task<bool> Delete(KpiGeneral KpiGeneral)
        {
            if (await ValidateId(KpiGeneral))
            {
            }
            return KpiGeneral.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiGeneral> KpiGenerals)
        {
            return true;
        }

        public async Task<bool> Import(List<KpiGeneral> KpiGenerals)
        {
            return true;
        }
    }
}
