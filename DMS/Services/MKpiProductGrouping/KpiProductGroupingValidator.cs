using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Services.MKpiProductGrouping
{
    public interface IKpiProductGroupingValidator : IServiceScoped
    {
        Task<bool> Create(KpiProductGrouping KpiProductGrouping);
        Task<bool> Update(KpiProductGrouping KpiProductGrouping);
        Task<bool> Delete(KpiProductGrouping KpiProductGrouping);
        Task<bool> BulkDelete(List<KpiProductGrouping> KpiProductGroupings);
        Task<bool> Import(List<KpiProductGrouping> KpiProductGroupings);
    }

    public class KpiProductGroupingValidator : IKpiProductGroupingValidator
    {
        public enum ErrorCode
        {
            IdNotExisted,
            OrganizationEmpty,
            OrganizationIdNotExisted,
            EmployeesEmpty,
            StatusNotExisted,
            KpiPeriodIdNotExisted,
            KpiYearEmpty,
            KpiYearIdNotExisted,
            KpiYearAndKpiPeriodMustInTheFuture,
            KpiProductGroupingTypeIdNotExisted,
            KpiProductGroupingContentsEmpty,
            ProductGroupingEmpty,
            ProductGroupingNotExisted,
            ItemEmpty,
            ItemNotExisted,
            ValueCannotBeNull,
            EmployeeHasKpi
        }

        private IUOW UOW;
        private ICurrentContext CurrentContext;

        public KpiProductGroupingValidator(IUOW UOW, ICurrentContext CurrentContext)
        {
            this.UOW = UOW;
            this.CurrentContext = CurrentContext;
        }

        public async Task<bool> ValidateId(KpiProductGrouping KpiProductGrouping)
        {
            KpiProductGroupingFilter KpiProductGroupingFilter = new KpiProductGroupingFilter
            {
                Skip = 0,
                Take = 10,
                Id = new IdFilter { Equal = KpiProductGrouping.Id },
                Selects = KpiProductGroupingSelect.Id
            };

            int count = await UOW.KpiProductGroupingRepository.Count(KpiProductGroupingFilter);
            if (count == 0)
                KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.Id), ErrorCode.IdNotExisted);
            return count == 1;
        }

        private async Task<bool> ValidateOrganization(KpiProductGrouping KpiProductGrouping)
        {
            if (KpiProductGrouping.OrganizationId == 0)
            {
                KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.Organization), ErrorCode.OrganizationEmpty);
            }
            else
            {
                OrganizationFilter OrganizationFilter = new OrganizationFilter
                {
                    Id = new IdFilter { Equal = KpiProductGrouping.OrganizationId }
                };

                var count = await UOW.OrganizationRepository.Count(OrganizationFilter);
                if (count == 0)
                    KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.Organization), ErrorCode.OrganizationIdNotExisted);
            }

            return KpiProductGrouping.IsValidated;
        }

        private async Task<bool> ValidateEmployees(KpiProductGrouping KpiProductGrouping)
        {
            if (KpiProductGrouping.Employees == null || !KpiProductGrouping.Employees.Any())
                KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.Employees), ErrorCode.EmployeesEmpty);
            else
            {
                var EmployeeIds = KpiProductGrouping.Employees.Select(x => x.Id).ToList();
                AppUserFilter AppUserFilter = new AppUserFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Id = new IdFilter { In = EmployeeIds },
                    OrganizationId = new IdFilter(),
                    Selects = AppUserSelect.Id
                };

                var EmployeeIdsInDB = (await UOW.AppUserRepository.List(AppUserFilter)).Select(x => x.Id).ToList();
                var listIdsNotExisted = EmployeeIds.Except(EmployeeIdsInDB).ToList();

                if (listIdsNotExisted != null && listIdsNotExisted.Any())
                {
                    foreach (var Id in listIdsNotExisted)
                    {
                        KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.Employees), ErrorCode.IdNotExisted);
                    }
                }

            }
            return KpiProductGrouping.IsValidated;
        }

        private async Task<bool> ValidateStatus(KpiProductGrouping KpiProductGrouping)
        {
            if (StatusEnum.ACTIVE.Id != KpiProductGrouping.StatusId && StatusEnum.INACTIVE.Id != KpiProductGrouping.StatusId)
                KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.Status), ErrorCode.StatusNotExisted);
            return KpiProductGrouping.IsValidated;
        }

        private async Task<bool> ValidateKpiPeriod(KpiProductGrouping KpiProductGrouping)
        {
            KpiPeriodFilter KpiPeriodFilter = new KpiPeriodFilter
            {
                Id = new IdFilter { Equal = KpiProductGrouping.KpiPeriodId }
            };

            int count = await UOW.KpiPeriodRepository.Count(KpiPeriodFilter);
            if (count == 0)
                KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.KpiPeriod), ErrorCode.KpiPeriodIdNotExisted);
            return KpiProductGrouping.IsValidated;
        }

        private async Task<bool> ValidateKpiYear(KpiProductGrouping KpiProductGrouping)
        {
            KpiYearFilter KpiYearFilter = new KpiYearFilter
            {
                Id = new IdFilter { Equal = KpiProductGrouping.KpiYearId }
            };

            int count = await UOW.KpiYearRepository.Count(KpiYearFilter);
            if (count == 0)
                KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.KpiYear), ErrorCode.KpiYearIdNotExisted);
            return KpiProductGrouping.IsValidated;
        }

        private async Task<bool> ValidateTime(KpiProductGrouping KpiProductGrouping)
        {
            await ValidateKpiPeriod(KpiProductGrouping);
            await ValidateKpiYear(KpiProductGrouping);
            if (!KpiProductGrouping.IsValidated) return false;
            DateTime now = StaticParams.DateTimeNow;
            DateTime StartDate, EndDate;
            (StartDate, EndDate) = DateTimeConvert(KpiProductGrouping.KpiPeriodId, KpiProductGrouping.KpiYearId);
            if (now > EndDate)
                KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.KpiYear), ErrorCode.KpiYearAndKpiPeriodMustInTheFuture);
            return KpiProductGrouping.IsValidated;
        }

        private async Task<bool> ValidateKpiProductGroupingType(KpiProductGrouping KpiProductGrouping)
        {
            KpiProductGroupingTypeFilter KpiProductGroupingTypeFilter = new KpiProductGroupingTypeFilter
            {
                Id = new IdFilter { Equal = KpiProductGrouping.KpiProductGroupingTypeId }
            };

            int count = await UOW.KpiProductGroupingTypeRepository.Count(KpiProductGroupingTypeFilter);
            if (count == 0)
                KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.KpiProductGroupingType), ErrorCode.KpiProductGroupingTypeIdNotExisted);
            return KpiProductGrouping.IsValidated;
        }


        private async Task<bool> ValidateContent(KpiProductGrouping KpiProductGrouping)
        {
            if (KpiProductGrouping.KpiProductGroupingContents == null || !KpiProductGrouping.KpiProductGroupingContents.Any())
                KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.KpiProductGroupingContents), ErrorCode.KpiProductGroupingContentsEmpty);
            else
            {
                var ProductGroupingIds = KpiProductGrouping.KpiProductGroupingContents
                    .Select(x => x.ProductGroupingId)
                    .ToList();
                ProductGroupingFilter ProductGroupingFilter = new ProductGroupingFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = ProductGroupingSelect.Id,
                    Id = new IdFilter { In = ProductGroupingIds },
                };
                var ProductGroupingInDB = await UOW.ProductGroupingRepository.List(ProductGroupingFilter);
                foreach (var KpiProductGroupingContent in KpiProductGrouping.KpiProductGroupingContents)
                {
                    if (KpiProductGroupingContent.ProductGroupingId == 0)
                        KpiProductGroupingContent.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGroupingContent.ProductGrouping), ErrorCode.ProductGroupingEmpty);
                    else
                    {
                        ProductGrouping ProductGrouping = ProductGroupingInDB.Where(x => x.Id == KpiProductGroupingContent.ProductGroupingId).FirstOrDefault();
                        if (ProductGrouping == null)
                            KpiProductGroupingContent.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGroupingContent.ProductGrouping), ErrorCode.ProductGroupingNotExisted);
                    }
                }
            }
            return KpiProductGrouping.IsValidated;
        }

        private async Task<bool> ValidateItem(KpiProductGrouping KpiProductGrouping)
        {
            if (KpiProductGrouping.KpiProductGroupingContents != null && KpiProductGrouping.KpiProductGroupingContents.Any())
            {
                List<KpiProductGroupingContentItemMapping> KpiProductGroupingContentItemMappings = KpiProductGrouping.KpiProductGroupingContents
                    .SelectMany(x => x.KpiProductGroupingContentItemMappings)
                    .ToList();
                List<long> ItemIds = new List<long>();
                List<Item> Items = new List<Item>();
                if (KpiProductGroupingContentItemMappings.Count > 0)
                {
                    ItemIds = KpiProductGroupingContentItemMappings
                        .Select(x => x.ItemId)
                        .Distinct()
                        .ToList();
                    Items = await UOW.ItemRepository.List(new ItemFilter
                    {
                        Skip = 0,
                        Take = int.MaxValue,
                        Id = new IdFilter
                        {
                            In = ItemIds
                        },
                        Selects = ItemSelect.Id
                    });
                }
                foreach (var KpiProductGroupingContent in KpiProductGrouping.KpiProductGroupingContents)
                {
                    bool SelectAllCurrentItem = KpiProductGroupingContent.SelectAllCurrentItem;
                    if (SelectAllCurrentItem) continue;
                    if ( KpiProductGroupingContent.KpiProductGroupingContentItemMappings == null || !KpiProductGroupingContent.KpiProductGroupingContentItemMappings.Any())
                        KpiProductGroupingContent.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGroupingContent.KpiProductGroupingContentItemMappings), ErrorCode.ItemEmpty);
                    if (Items.Count > 0)
                    {
                        foreach (var KpiProductGroupingItemMapping in KpiProductGroupingContent.KpiProductGroupingContentItemMappings)
                        {
                            Item Item = Items.Where(x => x.Id == KpiProductGroupingItemMapping.ItemId).FirstOrDefault();
                            if (Item == null)
                                KpiProductGroupingContent.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGroupingContent.KpiProductGroupingContentItemMappings), ErrorCode.ItemNotExisted);
                        }
                    }
                }
            }

            return KpiProductGrouping.IsValidated;
        }

        private async Task<bool> ValidateValue(KpiProductGrouping KpiProductGrouping)
        {
            bool flag = false;
            if (KpiProductGrouping.KpiProductGroupingContents != null && KpiProductGrouping.KpiProductGroupingContents.Any())
            {
                foreach (var KpiProductGroupingContent in KpiProductGrouping.KpiProductGroupingContents)
                {
                    flag = false;
                    foreach (var KpiProductGroupingContentCriteriaMapping in KpiProductGroupingContent.KpiProductGroupingContentCriteriaMappings)
                    {
                        if (KpiProductGroupingContentCriteriaMapping.Value != null) flag = true;
                    }
                    if (!flag) break;
                }
                if (!flag)
                    KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.Id), ErrorCode.ValueCannotBeNull);
            }
            return KpiProductGrouping.IsValidated;
        }

        private async Task<bool> ValidateOldKpi(KpiProductGrouping KpiProductGrouping)
        {
            if (KpiProductGrouping.Employees != null && KpiProductGrouping.Employees.Any())
            {
                var EmployeeIds = KpiProductGrouping.Employees.Select(x => x.Id).ToList();
                KpiProductGroupingFilter KpiProductGroupingFilter = new KpiProductGroupingFilter
                {
                    Skip = 0,
                    Take = int.MaxValue,
                    Selects = KpiProductGroupingSelect.Id | KpiProductGroupingSelect.Employee,
                    EmployeeId = new IdFilter { In = EmployeeIds },
                    StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                    KpiPeriodId = new IdFilter { Equal = KpiProductGrouping.KpiPeriodId },
                    KpiYearId = new IdFilter { Equal = KpiProductGrouping.KpiYearId },
                    KpiProductGroupingTypeId = new IdFilter { Equal = KpiProductGrouping.KpiProductGroupingTypeId }
                };
                var OldKpiProductGroupings = await UOW.KpiProductGroupingRepository.List(KpiProductGroupingFilter);
                var oldEmployeeIds = OldKpiProductGroupings.Select(x => x.EmployeeId).ToList();
                foreach (var Employee in KpiProductGrouping.Employees)
                {
                    if (oldEmployeeIds.Contains(Employee.Id))
                    {
                        KpiProductGrouping.AddError(nameof(KpiProductGroupingValidator), nameof(KpiProductGrouping.Employee), ErrorCode.EmployeeHasKpi);
                    }
                }
            }
            return KpiProductGrouping.IsValidated;
        }

        public async Task<bool> Create(KpiProductGrouping KpiProductGrouping)
        {
            await ValidateOrganization(KpiProductGrouping);
            await ValidateEmployees(KpiProductGrouping);
            await ValidateOldKpi(KpiProductGrouping);
            await ValidateStatus(KpiProductGrouping);
            await ValidateKpiPeriod(KpiProductGrouping);
            await ValidateKpiYear(KpiProductGrouping);
            await ValidateTime(KpiProductGrouping);
            await ValidateKpiProductGroupingType(KpiProductGrouping);
            await ValidateContent(KpiProductGrouping);
            await ValidateItem(KpiProductGrouping);
            await ValidateValue(KpiProductGrouping);
            return KpiProductGrouping.IsValidated;
        }

        public async Task<bool> Update(KpiProductGrouping KpiProductGrouping)
        {
            if (await ValidateId(KpiProductGrouping))
            {
                await ValidateOrganization(KpiProductGrouping);
                await ValidateStatus(KpiProductGrouping);
                await ValidateTime(KpiProductGrouping);
                await ValidateKpiProductGroupingType(KpiProductGrouping);
                await ValidateContent(KpiProductGrouping);
                await ValidateItem(KpiProductGrouping);
                await ValidateValue(KpiProductGrouping);
            }
            return KpiProductGrouping.IsValidated;
        }

        public async Task<bool> Delete(KpiProductGrouping KpiProductGrouping)
        {
            if (await ValidateId(KpiProductGrouping))
            {
            }
            return KpiProductGrouping.IsValidated;
        }

        public async Task<bool> BulkDelete(List<KpiProductGrouping> KpiProductGroupings)
        {
            foreach (KpiProductGrouping KpiProductGrouping in KpiProductGroupings)
            {
                await Delete(KpiProductGrouping);
            }
            return KpiProductGroupings.All(x => x.IsValidated);
        }

        public async Task<bool> Import(List<KpiProductGrouping> KpiProductGroupings)
        {
            return true;
        }

        private Tuple<DateTime, DateTime> DateTimeConvert(long KpiPeriodId, long KpiYearId)
        {
            DateTime startDate = StaticParams.DateTimeNow;
            DateTime endDate = StaticParams.DateTimeNow;
            if (KpiPeriodId <= Enums.KpiPeriodEnum.PERIOD_MONTH12.Id)
            {
                startDate = new DateTime((int)KpiYearId, (int)(KpiPeriodId % 100), 1);
                endDate = startDate.AddMonths(1).AddSeconds(-1);
            }
            else
            {
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER01.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 1, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER02.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 4, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER03.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 7, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_QUATER04.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 10, 1);
                    endDate = startDate.AddMonths(3).AddSeconds(-1);
                }
                if (KpiPeriodId == Enums.KpiPeriodEnum.PERIOD_YEAR01.Id)
                {
                    startDate = new DateTime((int)KpiYearId, 1, 1);
                    endDate = startDate.AddYears(1).AddSeconds(-1);
                }
            }

            return Tuple.Create(startDate, endDate);
        }
    }
}
