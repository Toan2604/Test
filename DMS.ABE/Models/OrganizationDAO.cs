using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class OrganizationDAO
    {
        public OrganizationDAO()
        {
            AppUsers = new HashSet<AppUserDAO>();
            Banners = new HashSet<BannerDAO>();
            DirectSalesOrderTransactions = new HashSet<DirectSalesOrderTransactionDAO>();
            DirectSalesOrders = new HashSet<DirectSalesOrderDAO>();
            ERoutes = new HashSet<ERouteDAO>();
            IndirectSalesOrderTransactions = new HashSet<IndirectSalesOrderTransactionDAO>();
            IndirectSalesOrders = new HashSet<IndirectSalesOrderDAO>();
            InverseParent = new HashSet<OrganizationDAO>();
            KpiGenerals = new HashSet<KpiGeneralDAO>();
            KpiItems = new HashSet<KpiItemDAO>();
            KpiProductGroupings = new HashSet<KpiProductGroupingDAO>();
            LuckyDraws = new HashSet<LuckyDrawDAO>();
            LuckyNumberGroupings = new HashSet<LuckyNumberGroupingDAO>();
            Notifications = new HashSet<NotificationDAO>();
            PriceLists = new HashSet<PriceListDAO>();
            PromotionCodeOrganizationMappings = new HashSet<PromotionCodeOrganizationMappingDAO>();
            PromotionCodes = new HashSet<PromotionCodeDAO>();
            Promotions = new HashSet<PromotionDAO>();
            ShowingOrderWithDraws = new HashSet<ShowingOrderWithDrawDAO>();
            ShowingOrders = new HashSet<ShowingOrderDAO>();
            StoreBalances = new HashSet<StoreBalanceDAO>();
            StoreCheckings = new HashSet<StoreCheckingDAO>();
            StoreScoutings = new HashSet<StoreScoutingDAO>();
            StoreUncheckings = new HashSet<StoreUncheckingDAO>();
            Stores = new HashSet<StoreDAO>();
            SurveyResults = new HashSet<SurveyResultDAO>();
            Surveys = new HashSet<SurveyDAO>();
            WarehouseOrganizationMappings = new HashSet<WarehouseOrganizationMappingDAO>();
            WorkflowDefinitions = new HashSet<WorkflowDefinitionDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Path { get; set; }
        public long Level { get; set; }
        public long StatusId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }
        public bool IsDisplay { get; set; }

        public virtual OrganizationDAO Parent { get; set; }
        public virtual StatusDAO Status { get; set; }
        public virtual ICollection<AppUserDAO> AppUsers { get; set; }
        public virtual ICollection<BannerDAO> Banners { get; set; }
        public virtual ICollection<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactions { get; set; }
        public virtual ICollection<DirectSalesOrderDAO> DirectSalesOrders { get; set; }
        public virtual ICollection<ERouteDAO> ERoutes { get; set; }
        public virtual ICollection<IndirectSalesOrderTransactionDAO> IndirectSalesOrderTransactions { get; set; }
        public virtual ICollection<IndirectSalesOrderDAO> IndirectSalesOrders { get; set; }
        public virtual ICollection<OrganizationDAO> InverseParent { get; set; }
        public virtual ICollection<KpiGeneralDAO> KpiGenerals { get; set; }
        public virtual ICollection<KpiItemDAO> KpiItems { get; set; }
        public virtual ICollection<KpiProductGroupingDAO> KpiProductGroupings { get; set; }
        public virtual ICollection<LuckyDrawDAO> LuckyDraws { get; set; }
        public virtual ICollection<LuckyNumberGroupingDAO> LuckyNumberGroupings { get; set; }
        public virtual ICollection<NotificationDAO> Notifications { get; set; }
        public virtual ICollection<PriceListDAO> PriceLists { get; set; }
        public virtual ICollection<PromotionCodeOrganizationMappingDAO> PromotionCodeOrganizationMappings { get; set; }
        public virtual ICollection<PromotionCodeDAO> PromotionCodes { get; set; }
        public virtual ICollection<PromotionDAO> Promotions { get; set; }
        public virtual ICollection<ShowingOrderWithDrawDAO> ShowingOrderWithDraws { get; set; }
        public virtual ICollection<ShowingOrderDAO> ShowingOrders { get; set; }
        public virtual ICollection<StoreBalanceDAO> StoreBalances { get; set; }
        public virtual ICollection<StoreCheckingDAO> StoreCheckings { get; set; }
        public virtual ICollection<StoreScoutingDAO> StoreScoutings { get; set; }
        public virtual ICollection<StoreUncheckingDAO> StoreUncheckings { get; set; }
        public virtual ICollection<StoreDAO> Stores { get; set; }
        public virtual ICollection<SurveyResultDAO> SurveyResults { get; set; }
        public virtual ICollection<SurveyDAO> Surveys { get; set; }
        public virtual ICollection<WarehouseOrganizationMappingDAO> WarehouseOrganizationMappings { get; set; }
        public virtual ICollection<WorkflowDefinitionDAO> WorkflowDefinitions { get; set; }
    }
}
