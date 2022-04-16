using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class StatusDAO
    {
        public StatusDAO()
        {
            Albums = new HashSet<AlbumDAO>();
            AppUsers = new HashSet<AppUserDAO>();
            Banners = new HashSet<BannerDAO>();
            Brands = new HashSet<BrandDAO>();
            Categories = new HashSet<CategoryDAO>();
            CodeGeneratorRules = new HashSet<CodeGeneratorRuleDAO>();
            ConversationConfigurations = new HashSet<ConversationConfigurationDAO>();
            Districts = new HashSet<DistrictDAO>();
            ERoutes = new HashSet<ERouteDAO>();
            Items = new HashSet<ItemDAO>();
            KpiCriteriaGenerals = new HashSet<KpiCriteriaGeneralDAO>();
            KpiCriteriaItems = new HashSet<KpiCriteriaItemDAO>();
            KpiGeneralContents = new HashSet<KpiGeneralContentDAO>();
            KpiGenerals = new HashSet<KpiGeneralDAO>();
            KpiItems = new HashSet<KpiItemDAO>();
            KpiProductGroupingCriterias = new HashSet<KpiProductGroupingCriteriaDAO>();
            KpiProductGroupings = new HashSet<KpiProductGroupingDAO>();
            LuckyDraws = new HashSet<LuckyDrawDAO>();
            LuckyNumberGroupings = new HashSet<LuckyNumberGroupingDAO>();
            Nations = new HashSet<NationDAO>();
            Organizations = new HashSet<OrganizationDAO>();
            Permissions = new HashSet<PermissionDAO>();
            Positions = new HashSet<PositionDAO>();
            PriceLists = new HashSet<PriceListDAO>();
            ProductGroupings = new HashSet<ProductGroupingDAO>();
            ProductTypes = new HashSet<ProductTypeDAO>();
            Products = new HashSet<ProductDAO>();
            PromotionCodes = new HashSet<PromotionCodeDAO>();
            PromotionPromotionPolicyMappings = new HashSet<PromotionPromotionPolicyMappingDAO>();
            Promotions = new HashSet<PromotionDAO>();
            Provinces = new HashSet<ProvinceDAO>();
            Roles = new HashSet<RoleDAO>();
            ShowingCategories = new HashSet<ShowingCategoryDAO>();
            ShowingItems = new HashSet<ShowingItemDAO>();
            ShowingOrderWithDraws = new HashSet<ShowingOrderWithDrawDAO>();
            ShowingOrders = new HashSet<ShowingOrderDAO>();
            StoreGroupings = new HashSet<StoreGroupingDAO>();
            StoreScoutingTypes = new HashSet<StoreScoutingTypeDAO>();
            StoreTypes = new HashSet<StoreTypeDAO>();
            StoreUsers = new HashSet<StoreUserDAO>();
            Stores = new HashSet<StoreDAO>();
            Suppliers = new HashSet<SupplierDAO>();
            Surveys = new HashSet<SurveyDAO>();
            TaxTypes = new HashSet<TaxTypeDAO>();
            UnitOfMeasureGroupings = new HashSet<UnitOfMeasureGroupingDAO>();
            UnitOfMeasures = new HashSet<UnitOfMeasureDAO>();
            Wards = new HashSet<WardDAO>();
            Warehouses = new HashSet<WarehouseDAO>();
            WorkflowDefinitions = new HashSet<WorkflowDefinitionDAO>();
            WorkflowDirections = new HashSet<WorkflowDirectionDAO>();
            WorkflowSteps = new HashSet<WorkflowStepDAO>();
        }

        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<AlbumDAO> Albums { get; set; }
        public virtual ICollection<AppUserDAO> AppUsers { get; set; }
        public virtual ICollection<BannerDAO> Banners { get; set; }
        public virtual ICollection<BrandDAO> Brands { get; set; }
        public virtual ICollection<CategoryDAO> Categories { get; set; }
        public virtual ICollection<CodeGeneratorRuleDAO> CodeGeneratorRules { get; set; }
        public virtual ICollection<ConversationConfigurationDAO> ConversationConfigurations { get; set; }
        public virtual ICollection<DistrictDAO> Districts { get; set; }
        public virtual ICollection<ERouteDAO> ERoutes { get; set; }
        public virtual ICollection<ItemDAO> Items { get; set; }
        public virtual ICollection<KpiCriteriaGeneralDAO> KpiCriteriaGenerals { get; set; }
        public virtual ICollection<KpiCriteriaItemDAO> KpiCriteriaItems { get; set; }
        public virtual ICollection<KpiGeneralContentDAO> KpiGeneralContents { get; set; }
        public virtual ICollection<KpiGeneralDAO> KpiGenerals { get; set; }
        public virtual ICollection<KpiItemDAO> KpiItems { get; set; }
        public virtual ICollection<KpiProductGroupingCriteriaDAO> KpiProductGroupingCriterias { get; set; }
        public virtual ICollection<KpiProductGroupingDAO> KpiProductGroupings { get; set; }
        public virtual ICollection<LuckyDrawDAO> LuckyDraws { get; set; }
        public virtual ICollection<LuckyNumberGroupingDAO> LuckyNumberGroupings { get; set; }
        public virtual ICollection<NationDAO> Nations { get; set; }
        public virtual ICollection<OrganizationDAO> Organizations { get; set; }
        public virtual ICollection<PermissionDAO> Permissions { get; set; }
        public virtual ICollection<PositionDAO> Positions { get; set; }
        public virtual ICollection<PriceListDAO> PriceLists { get; set; }
        public virtual ICollection<ProductGroupingDAO> ProductGroupings { get; set; }
        public virtual ICollection<ProductTypeDAO> ProductTypes { get; set; }
        public virtual ICollection<ProductDAO> Products { get; set; }
        public virtual ICollection<PromotionCodeDAO> PromotionCodes { get; set; }
        public virtual ICollection<PromotionPromotionPolicyMappingDAO> PromotionPromotionPolicyMappings { get; set; }
        public virtual ICollection<PromotionDAO> Promotions { get; set; }
        public virtual ICollection<ProvinceDAO> Provinces { get; set; }
        public virtual ICollection<RoleDAO> Roles { get; set; }
        public virtual ICollection<ShowingCategoryDAO> ShowingCategories { get; set; }
        public virtual ICollection<ShowingItemDAO> ShowingItems { get; set; }
        public virtual ICollection<ShowingOrderWithDrawDAO> ShowingOrderWithDraws { get; set; }
        public virtual ICollection<ShowingOrderDAO> ShowingOrders { get; set; }
        public virtual ICollection<StoreGroupingDAO> StoreGroupings { get; set; }
        public virtual ICollection<StoreScoutingTypeDAO> StoreScoutingTypes { get; set; }
        public virtual ICollection<StoreTypeDAO> StoreTypes { get; set; }
        public virtual ICollection<StoreUserDAO> StoreUsers { get; set; }
        public virtual ICollection<StoreDAO> Stores { get; set; }
        public virtual ICollection<SupplierDAO> Suppliers { get; set; }
        public virtual ICollection<SurveyDAO> Surveys { get; set; }
        public virtual ICollection<TaxTypeDAO> TaxTypes { get; set; }
        public virtual ICollection<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupings { get; set; }
        public virtual ICollection<UnitOfMeasureDAO> UnitOfMeasures { get; set; }
        public virtual ICollection<WardDAO> Wards { get; set; }
        public virtual ICollection<WarehouseDAO> Warehouses { get; set; }
        public virtual ICollection<WorkflowDefinitionDAO> WorkflowDefinitions { get; set; }
        public virtual ICollection<WorkflowDirectionDAO> WorkflowDirections { get; set; }
        public virtual ICollection<WorkflowStepDAO> WorkflowSteps { get; set; }
    }
}
