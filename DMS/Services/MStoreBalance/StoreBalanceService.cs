using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Repositories;
using DMS.Services.MOrganization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;

namespace DMS.Services.MStoreBalance
{
    public interface IStoreBalanceService : IServiceScoped
    {
        Task<int> Count(StoreBalanceFilter StoreBalanceFilter);
        Task<List<StoreBalance>> List(StoreBalanceFilter StoreBalanceFilter);
        Task<StoreBalance> Get(long Id);
        Task<StoreBalance> Create(StoreBalance StoreBalance);
        Task<StoreBalance> Update(StoreBalance StoreBalance);
        Task<StoreBalance> Delete(StoreBalance StoreBalance);
        Task<List<StoreBalance>> BulkDelete(List<StoreBalance> StoreBalances);
        Task<List<StoreBalance>> BulkMerge(List<StoreBalance> StoreBalances);
        Task<List<StoreBalance>> Import(List<StoreBalance> StoreBalances);
        Task<StoreBalanceFilter> ToFilter(StoreBalanceFilter StoreBalanceFilter);
    }

    public class StoreBalanceService : BaseService, IStoreBalanceService
    {
        private IUOW UOW;
        private ILogging Logging;
        private IOrganizationService OrganizationService;
        private ICurrentContext CurrentContext;
        private IStoreBalanceValidator StoreBalanceValidator;
        private IRabbitManager RabbitManager;

        public StoreBalanceService(
            IUOW UOW,
            ILogging Logging,
            ICurrentContext CurrentContext,
            IOrganizationService OrganizationService,
            IStoreBalanceValidator StoreBalanceValidator,
            IRabbitManager RabbitManager
        )
        {
            this.UOW = UOW;
            this.Logging = Logging;
            this.CurrentContext = CurrentContext;
            this.OrganizationService = OrganizationService;
            this.StoreBalanceValidator = StoreBalanceValidator;
            this.RabbitManager = RabbitManager;
        }
        public async Task<int> Count(StoreBalanceFilter StoreBalanceFilter)
        {
            try
            {
                int result = await UOW.StoreBalanceRepository.Count(StoreBalanceFilter);
                return result;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreBalanceService));
            }
            return 0;
        }

        public async Task<List<StoreBalance>> List(StoreBalanceFilter StoreBalanceFilter)
        {
            try
            {
                List<StoreBalance> StoreBalances = await UOW.StoreBalanceRepository.List(StoreBalanceFilter);
                return StoreBalances;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreBalanceService));
            }
            return null;
        }
        public async Task<StoreBalance> Get(long Id)
        {
            StoreBalance StoreBalance = await UOW.StoreBalanceRepository.Get(Id);
            if (StoreBalance == null)
                return null;
            return StoreBalance;
        }

        public async Task<StoreBalance> Create(StoreBalance StoreBalance)
        {
            if (!await StoreBalanceValidator.Create(StoreBalance))
                return StoreBalance;

            try
            {
                StoreBalance.CreditAmount = Math.Round(StoreBalance.CreditAmount.Value, 2);
                StoreBalance.DebitAmount = Math.Round(StoreBalance.DebitAmount.Value, 2);
                await UOW.StoreBalanceRepository.Create(StoreBalance);
                await UpdateStoreBalanceCheckState(new List<long> { StoreBalance.StoreId });
                var newData = await UOW.StoreBalanceRepository.Get(StoreBalance.Id);
                Sync(new List<StoreBalance> { newData });
                Logging.CreateAuditLog(StoreBalance, new { }, nameof(StoreBalanceService));
                return await UOW.StoreBalanceRepository.Get(StoreBalance.Id);
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreBalanceService));
            }
            return null;
        }

        public async Task<StoreBalance> Update(StoreBalance StoreBalance)
        {
            if (!await StoreBalanceValidator.Update(StoreBalance))
                return StoreBalance;
            try
            {
                var oldData = await UOW.StoreBalanceRepository.Get(StoreBalance.Id);
                StoreBalance.CreditAmount = Math.Round(StoreBalance.CreditAmount.Value, 2);
                StoreBalance.DebitAmount = Math.Round(StoreBalance.DebitAmount.Value, 2);
                await UOW.StoreBalanceRepository.Update(StoreBalance);
                await UpdateStoreBalanceCheckState(new List<long> { StoreBalance.StoreId });
                var newData = await UOW.StoreBalanceRepository.Get(StoreBalance.Id);
                Sync(new List<StoreBalance> { newData });
                Logging.CreateAuditLog(newData, oldData, nameof(StoreBalanceService));
                return newData;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreBalanceService));
            }
            return null;
        }

        public async Task<StoreBalance> Delete(StoreBalance StoreBalance)
        {
            if (!await StoreBalanceValidator.Delete(StoreBalance))
                return StoreBalance;

            try
            {
                await UOW.StoreBalanceRepository.Delete(StoreBalance);
                await UpdateStoreBalanceCheckState(new List<long> { StoreBalance.StoreId });
                var newData = await UOW.StoreBalanceRepository.List(new List<long> { StoreBalance.Id });
                Sync(newData);
                Logging.CreateAuditLog(new { }, StoreBalance, nameof(StoreBalanceService));
                return StoreBalance;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreBalanceService));
            }
            return null;
        }

        public async Task<List<StoreBalance>> BulkDelete(List<StoreBalance> StoreBalances)
        {
            if (!await StoreBalanceValidator.BulkDelete(StoreBalances))
                return StoreBalances;

            try
            {
                await UOW.StoreBalanceRepository.BulkDelete(StoreBalances);
                var Ids = StoreBalances.Select(x => x.Id).ToList();
                StoreBalances = await UOW.StoreBalanceRepository.List(Ids);
                var StoreIds = StoreBalances.Select(x => x.StoreId).ToList();
                await UpdateStoreBalanceCheckState(StoreIds);
                Sync(StoreBalances);
                Logging.CreateAuditLog(new { }, StoreBalances, nameof(StoreBalanceService));
                return StoreBalances;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreBalanceService));
            }
            return null;
        }
        public async Task<List<StoreBalance>> BulkMerge(List<StoreBalance> StoreBalances)
        {
            if (!await StoreBalanceValidator.Import(StoreBalances))
                return StoreBalances;
            try
            {
                await UOW.StoreBalanceRepository.BulkMerge(StoreBalances);
                var StoreIds = StoreBalances.Select(x => x.StoreId).ToList();
                await UpdateStoreBalanceCheckState(StoreIds);
                var Ids = StoreBalances.Select(x => x.Id).ToList();
                StoreBalances = await UOW.StoreBalanceRepository.List(Ids);
                Sync(StoreBalances);
                Logging.CreateAuditLog(StoreBalances, new { }, nameof(StoreBalanceService));
                return StoreBalances;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreBalanceService));
            }
            return null;
        }
        public async Task<List<StoreBalance>> Import(List<StoreBalance> StoreBalances)
        {
            if (!await StoreBalanceValidator.Import(StoreBalances))
                return StoreBalances;
            try
            {
                await UOW.StoreBalanceRepository.Clean();
                await UOW.StoreBalanceRepository.BulkMerge(StoreBalances);
                var StoreIds = StoreBalances.Select(x => x.StoreId).ToList();
                await UpdateStoreBalanceCheckState(StoreIds);
                Logging.CreateAuditLog(StoreBalances, new { }, nameof(StoreBalanceService));
                return StoreBalances;
            }
            catch (Exception ex)
            {
                Logging.CreateSystemLog(ex, nameof(StoreBalanceService));
            }
            return null;
        }

        public async Task<StoreBalanceFilter> ToFilter(StoreBalanceFilter filter)
        {
            if (filter.OrFilter == null) filter.OrFilter = new List<StoreBalanceFilter>();
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return filter;
            List<Organization> Organizations = await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.ALL,
                OrderBy = OrganizationOrder.Id,
                OrderType = OrderType.ASC
            });
            foreach (var currentFilter in CurrentContext.Filters)
            {
                StoreBalanceFilter subFilter = new StoreBalanceFilter();
                filter.OrFilter.Add(subFilter);
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == nameof(subFilter.Id))
                        subFilter.Id = FilterBuilder.Merge(subFilter.Id, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.OrganizationId))
                    {
                        var organizationIds = FilterOrganization(Organizations, FilterPermissionDefinition.IdFilter);
                        IdFilter IdFilter = new IdFilter { In = organizationIds };
                        subFilter.OrganizationId = FilterBuilder.Merge(subFilter.OrganizationId, IdFilter);
                    }
                    if (FilterPermissionDefinition.Name == nameof(subFilter.StoreId))
                        subFilter.StoreId = FilterBuilder.Merge(subFilter.StoreId, FilterPermissionDefinition.IdFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.CreditAmount))
                        subFilter.CreditAmount = FilterBuilder.Merge(subFilter.CreditAmount, FilterPermissionDefinition.DecimalFilter);
                    if (FilterPermissionDefinition.Name == nameof(subFilter.DebitAmount))
                        subFilter.DebitAmount = FilterBuilder.Merge(subFilter.DebitAmount, FilterPermissionDefinition.DecimalFilter);
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

        private void Sync(List<StoreBalance> StoreBalances)
        {
            //List<Organization> Organizations = new List<Organization>();
            //List<Store> Stores = new List<Store>();
            //Organizations.AddRange(StoreBalances.Select(x => new Organization { Id = x.OrganizationId }));
            //Stores.AddRange(StoreBalances.Select(x => new Store { Id = x.StoreId }));

            //Organizations = Organizations.Distinct().ToList();
            //Stores = Stores.Distinct().ToList();
            RabbitManager.PublishList(StoreBalances, RoutingKeyEnum.StoreBalanceSync.Code);
            //RabbitManager.PublishList(Organizations, RoutingKeyEnum.OrganizationUsed.Code);
            //RabbitManager.PublishList(Stores, RoutingKeyEnum.StoreUsed.Code);
        }
        private async Task UpdateStoreBalanceCheckState(List<long> StoreIds)
        {
            var Stores = await UOW.StoreRepository.List(StoreIds);

            List<DirectSalesOrder> DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(new DirectSalesOrderFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = DirectSalesOrderSelect.Id | DirectSalesOrderSelect.BuyerStore | DirectSalesOrderSelect.TotalAfterTax | DirectSalesOrderSelect.InventoryCheckState | DirectSalesOrderSelect.StoreBalanceCheckState,
                GeneralApprovalStateId = new IdFilter
                {
                    NotIn = new List<long>
                        {
                            GeneralApprovalStateEnum.APPROVED.Id,
                            GeneralApprovalStateEnum.REJECTED.Id,
                            GeneralApprovalStateEnum.STORE_APPROVED.Id,
                            GeneralApprovalStateEnum.STORE_REJECTED.Id
                        }
                },
                BuyerStoreId = new IdFilter { In = StoreIds }
            });
            List<StoreBalance> StoreBalances = await UOW.StoreBalanceRepository.List(new StoreBalanceFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = StoreBalanceSelect.CreditAmount | StoreBalanceSelect.DebitAmount | StoreBalanceSelect.Store,
                StoreId = new IdFilter { In = StoreIds }
            });
            Dictionary<long, decimal> BalanceDict = new Dictionary<long, decimal>();
            foreach (var Store in Stores)
            {
                var storeBalances = StoreBalances.Where(x => x.StoreId == Store.Id).ToList();
                var BalanceAmount = storeBalances?.Sum(x => x.DebitAmount ?? 0) - storeBalances?.Sum(x => x.CreditAmount ?? 0);
                BalanceDict.Add(Store.Id, BalanceAmount.Value);
            }
            if (DirectSalesOrders != null && DirectSalesOrders.Any())
            {
                List<DirectSalesOrder> BulkUpdateList = new List<DirectSalesOrder>();
                foreach (var DirectSalesOrder in DirectSalesOrders)
                {
                    var oldStateId = DirectSalesOrder.StoreBalanceCheckStateId;
                    var Store = Stores.Where(x => x.Id == DirectSalesOrder.BuyerStoreId).FirstOrDefault();
                    if ((BalanceDict[Store.Id] + DirectSalesOrder.TotalAfterTax) <= Store.DebtLimited) DirectSalesOrder.StoreBalanceCheckStateId = CheckStateEnum.PASS.Id;
                    else DirectSalesOrder.StoreBalanceCheckStateId = CheckStateEnum.NOT_PASS.Id;
                    if (DirectSalesOrder.StoreBalanceCheckStateId != oldStateId) BulkUpdateList.Add(DirectSalesOrder);
                }
                if (BulkUpdateList.Any()) await UOW.DirectSalesOrderRepository.BulkUpdateCheckState(BulkUpdateList);
            }

        }
    }
}
