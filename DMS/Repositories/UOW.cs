using DMS.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Nest;
using System;
using System.Threading.Tasks;
using TrueSight.Common;
using Microsoft.EntityFrameworkCore;

namespace DMS.Repositories
{
    public interface IUOW : IServiceScoped, IDisposable
    {
        IAlbumRepository AlbumRepository { get; }
        IAppUserRepository AppUserRepository { get; }
        IAppUserStoreMappingRepository AppUserStoreMappingRepository { get; }
        IBannerRepository BannerRepository { get; }
        IBrandRepository BrandRepository { get; }
        IBrandHistoryRepository BrandHistoryRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICheckStateRepository CheckStateRepository { get; }
        IColorRepository ColorRepository { get; }
        ICodeGeneratorRuleRepository CodeGeneratorRuleRepository { get; }
        ICommentRepository CommentRepository { get; }
        IConversationConfigurationRepository ConversationConfigurationRepository { get; }
        IConversationMessageRepository ConversationMessageRepository { get; }
        IConversationTypeRepository ConversationTypeRepository { get; }
        IConversationReadHistoryRepository ConversationReadHistoryRepository { get; }
        IConversationRepository ConversationRepository { get; }
        IDirectSalesOrderContentRepository DirectSalesOrderContentRepository { get; }
        IDirectSalesOrderRepository DirectSalesOrderRepository { get; }
        IDirectSalesOrderPromotionRepository DirectSalesOrderPromotionRepository { get; }
        IDirectSalesOrderSourceTypeRepository DirectSalesOrderSourceTypeRepository { get; }
        IDistrictRepository DistrictRepository { get; }
        IEntityComponentRepository EntityComponentRepository { get; }
        IEntityTypeRepository EntityTypeRepository { get; }
        IEditedPriceStatusRepository EditedPriceStatusRepository { get; }
        IERouteChangeRequestContentRepository ERouteChangeRequestContentRepository { get; }
        IERouteChangeRequestRepository ERouteChangeRequestRepository { get; }
        IERouteContentRepository ERouteContentRepository { get; }
        IERouteRepository ERouteRepository { get; }
        IERouteTypeRepository ERouteTypeRepository { get; }
        IErpApprovalStateRepository ErpApprovalStateRepository { get; }
        IExportTemplateRepository ExportTemplateRepository { get; }
        IEsItemRepository EsItemRepository { get; }
        IEstimatedRevenueRepository EstimatedRevenueRepository { get; }
        IEsProductRepository EsProductRepository { get; }
        IEsProvinceRepository EsProvinceRepository { get; }
        IEsStoreRepository EsStoreRepository { get; }
        IFieldRepository FieldRepository { get; }
        IImageRepository ImageRepository { get; }
        IFileRepository FileRepository { get; }
        IGeneralApprovalStateRepository GeneralApprovalStateRepository { get; set; }
        IGlobalUserRepository GlobalUserRepository { get; set; }
        IGlobalUserTypeRepository GlobalUserTypeRepository { get; set; }
        IIndirectSalesOrderContentRepository IndirectSalesOrderContentRepository { get; }
        IIndirectSalesOrderRepository IndirectSalesOrderRepository { get; }
        IIndirectSalesOrderPromotionRepository IndirectSalesOrderPromotionRepository { get; }
        IIndirectSalesOrderTransactionRepository IndirectSalesOrderTransactionRepository { get; }
        IItemRepository ItemRepository { get; }
        IItemHistoryRepository ItemHistoryRepository { get; }
        IKpiCriteriaGeneralRepository KpiCriteriaGeneralRepository { get; }
        IKpiCriteriaItemRepository KpiCriteriaItemRepository { get; }
        IKpiGeneralRepository KpiGeneralRepository { get; }
        IKpiGeneralContentRepository KpiGeneralContentRepository { get; }
        IKpiItemContentRepository KpiItemContentRepository { get; }
        IKpiItemRepository KpiItemRepository { get; }
        IKpiItemTypeRepository KpiItemTypeRepository { get; }
        IKpiPeriodRepository KpiPeriodRepository { get; }
        IKpiYearRepository KpiYearRepository { get; }
        ILuckyNumberRepository LuckyNumberRepository { get; }
        IKpiProductGroupingRepository KpiProductGroupingRepository { get; }
        IKpiProductGroupingContentRepository KpiProductGroupingContentRepository { get; }
        IKpiProductGroupingCriteriaRepository KpiProductGroupingCriteriaRepository { get; }
        IKpiProductGroupingTypeRepository KpiProductGroupingTypeRepository { get; }
        ILuckyDrawRepository LuckyDrawRepository { get; }
        ILuckyDrawNumberRepository LuckyDrawNumberRepository { get; }
        ILuckyDrawRegistrationRepository LuckyDrawRegistrationRepository { get; }
        ILuckyDrawStructureRepository LuckyDrawStructureRepository { get; }
        ILuckyDrawTypeRepository LuckyDrawTypeRepository { get; }
        ILuckyDrawWinnerRepository LuckyDrawWinnerRepository { get; }
        ILuckyDrawStoreMappingRepository LuckyDrawStoreMappingRepository { get; }
        ILuckyDrawStoreTypeMappingRepository LuckyDrawStoreTypeMappingRepository { get; }
        ILuckyDrawStoreGroupingMappingRepository LuckyDrawStoreGroupingMappingRepository { get; }
        ILuckyNumberGroupingRepository LuckyNumberGroupingRepository { get; }
        IInventoryRepository InventoryRepository { get; }
        IInventoryHistoryRepository InventoryHistoryRepository { get; }
        IMenuRepository MenuRepository { get; }
        INationRepository NationRepository { get; }
        INotificationRepository NotificationRepository { get; }
        INotificationStatusRepository NotificationStatusRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IPermissionOperatorRepository PermissionOperatorRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IPositionRepository PositionRepository { get; }
        IPriceListRepository PriceListRepository { get; }
        IPriceListTypeRepository PriceListTypeRepository { get; }
        IPriceListItemHistoryRepository PriceListItemHistoryRepository { get; }
        IPriceListItemMappingRepository PriceListItemMappingRepository { get; }
        IProblemRepository ProblemRepository { get; }
        IProblemHistoryRepository ProblemHistoryRepository { get; }
        IProblemTypeRepository ProblemTypeRepository { get; }
        IProblemStatusRepository ProblemStatusRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductGroupingRepository ProductGroupingRepository { get; }
        IProductGroupingHistoryRepository ProductGroupingHistoryRepository { get; }
        IProductTypeRepository ProductTypeRepository { get; }
        IPromotionComboRepository PromotionComboRepository { get; }
        IPromotionRepository PromotionRepository { get; }
        IPromotionCodeRepository PromotionCodeRepository { get; }
        IPromotionCodeHistoryRepository PromotionCodeHistoryRepository { get; }
        IPromotionDirectSalesOrderRepository PromotionDirectSalesOrderRepository { get; }
        IPromotionDiscountTypeRepository PromotionDiscountTypeRepository { get; }
        IPromotionProductTypeRepository PromotionProductTypeRepository { get; }
        IPromotionPolicyRepository PromotionPolicyRepository { get; }
        IPromotionProductAppliedTypeRepository PromotionProductAppliedTypeRepository { get; }
        IPromotionProductRepository PromotionProductRepository { get; }
        IPromotionProductGroupingRepository PromotionProductGroupingRepository { get; }
        IPromotionSamePriceRepository PromotionSamePriceRepository { get; }
        IPromotionStoreRepository PromotionStoreRepository { get; }
        IPromotionStoreGroupingRepository PromotionStoreGroupingRepository { get; }
        IPromotionStoreTypeRepository PromotionStoreTypeRepository { get; }
        IPromotionTypeRepository PromotionTypeRepository { get; }
        IProvinceRepository ProvinceRepository { get; }
        IRequestHistoryRepository RequestHistoryRepository { get; }
        IRequestStateRepository RequestStateRepository { get; }
        IRequestWorkflowDefinitionMappingRepository RequestWorkflowDefinitionMappingRepository { get; }
        IRequestWorkflowHistoryRepository RequestWorkflowHistoryRepository { get; }
        IRequestWorkflowParameterMappingRepository RequestWorkflowParameterMappingRepository { get; }
        IRequestWorkflowStepMappingRepository RequestWorkflowStepMappingRepository { get; }
        IRewardHistoryContentRepository RewardHistoryContentRepository { get; }
        IRewardHistoryRepository RewardHistoryRepository { get; }
        IRewardStatusRepository RewardStatusRepository { get; }
        IRoleRepository RoleRepository { get; }
        ISalesOrderTypeRepository SalesOrderTypeRepository { get; }
        ISexRepository SexRepository { get; }
        IShowingCategoryRepository ShowingCategoryRepository { get; }
        IShowingItemRepository ShowingItemRepository { get; }
        IShowingOrderContentRepository ShowingOrderContentRepository { get; }
        IShowingOrderRepository ShowingOrderRepository { get; }
        IShowingOrderContentWithDrawRepository ShowingOrderContentWithDrawRepository { get; }
        IShowingOrderWithDrawRepository ShowingOrderWithDrawRepository { get; }
        IStatusRepository StatusRepository { get; }
        IStoreApprovalStateRepository StoreApprovalStateRepository { get; }
        IStoreBalanceRepository StoreBalanceRepository { get; }
        IStoreScoutingRepository StoreScoutingRepository { get; }
        IStoreScoutingTypeRepository StoreScoutingTypeRepository { get; }
        IStoreScoutingStatusRepository StoreScoutingStatusRepository { get; }
        IStoreRepository StoreRepository { get; }
        IStoreHistoryRepository StoreHistoryRepository { get; }
        IStoreCheckingRepository StoreCheckingRepository { get; }
        IStoreGroupingRepository StoreGroupingRepository { get; }
        IStoreStatusHistoryTypeRepository StoreStatusHistoryTypeRepository { get; }
        IStoreStatusHistoryRepository StoreStatusHistoryRepository { get; }
        IStoreTypeRepository StoreTypeRepository { get; }
        IStoreTypeHistoryRepository StoreTypeHistoryRepository { get; }
        IStoreStatusRepository StoreStatusRepository { get; }
        IStoreUserRepository StoreUserRepository { get; }
        ISupplierRepository SupplierRepository { get; }
        ISurveyOptionTypeRepository SurveyOptionTypeRepository { get; }
        ISurveyQuestionTypeRepository SurveyQuestionTypeRepository { get; }
        ISurveyRepository SurveyRepository { get; }
        ISurveyResultRepository SurveyResultRepository { get; }
        ISystemConfigurationRepository SystemConfigurationRepository { get; }
        ITaxTypeRepository TaxTypeRepository { get; }
        ITransactionTypeRepository TransactionTypeRepository { get; }
        IUnitOfMeasureRepository UnitOfMeasureRepository { get; }
        IUnitOfMeasureGroupingContentRepository UnitOfMeasureGroupingContentRepository { get; }
        IUnitOfMeasureGroupingRepository UnitOfMeasureGroupingRepository { get; }
        IUsedVariationRepository UsedVariationRepository { get; }
        IVariationRepository VariationRepository { get; }
        IVariationGroupingRepository VariationGroupingRepository { get; }
        IWardRepository WardRepository { get; }
        IWarehouseRepository WarehouseRepository { get; }
        IWorkflowDefinitionRepository WorkflowDefinitionRepository { get; }
        IWorkflowDirectionRepository WorkflowDirectionRepository { get; }
        IWorkflowOperatorRepository WorkflowOperatorRepository { get; }
        IWorkflowParameterRepository WorkflowParameterRepository { get; }
        IWorkflowParameterTypeRepository WorkflowParameterTypeRepository { get; }
        IWorkflowStateRepository WorkflowStateRepository { get; }
        IWorkflowStepRepository WorkflowStepRepository { get; }
        IWorkflowTypeRepository WorkflowTypeRepository { get; }

    }

    public class UOW : IUOW
    {
        private DataContext DataContext;
        public IAlbumRepository AlbumRepository { get; private set; }
        public IAppUserRepository AppUserRepository { get; private set; }
        public IAppUserStoreMappingRepository AppUserStoreMappingRepository { get; private set; }
        public IBannerRepository BannerRepository { get; private set; }
        public IBrandRepository BrandRepository { get; private set; }
        public IBrandHistoryRepository BrandHistoryRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public ICheckStateRepository CheckStateRepository { get; private set; }
        public IColorRepository ColorRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }
        public ICodeGeneratorRuleRepository CodeGeneratorRuleRepository { get; private set; }
        public IConversationConfigurationRepository ConversationConfigurationRepository { get; private set; }
        public IConversationMessageRepository ConversationMessageRepository { get; private set; }
        public IConversationTypeRepository ConversationTypeRepository { get; private set; }
        public IConversationReadHistoryRepository ConversationReadHistoryRepository { get; private set; }
        public IConversationRepository ConversationRepository { get; private set; }
        public IDirectSalesOrderContentRepository DirectSalesOrderContentRepository { get; private set; }
        public IDirectSalesOrderRepository DirectSalesOrderRepository { get; private set; }
        public IDirectSalesOrderPromotionRepository DirectSalesOrderPromotionRepository { get; private set; }
        public IDirectSalesOrderSourceTypeRepository DirectSalesOrderSourceTypeRepository { get; private set; }
        public IDistrictRepository DistrictRepository { get; private set; }
        public IEditedPriceStatusRepository EditedPriceStatusRepository { get; private set; }
        public IEntityComponentRepository EntityComponentRepository { get; private set; }
        public IEntityTypeRepository EntityTypeRepository { get; private set; }
        public IERouteChangeRequestContentRepository ERouteChangeRequestContentRepository { get; private set; }
        public IERouteChangeRequestRepository ERouteChangeRequestRepository { get; private set; }
        public IERouteContentRepository ERouteContentRepository { get; private set; }
        public IERouteRepository ERouteRepository { get; private set; }
        public IERouteTypeRepository ERouteTypeRepository { get; private set; }
        public IExportTemplateRepository ExportTemplateRepository { get; private set; }
        public IEsItemRepository EsItemRepository { get; private set; }
        public IEstimatedRevenueRepository EstimatedRevenueRepository { get; private set; }
        public IFieldRepository FieldRepository { get; private set; }
        public IGeneralApprovalStateRepository GeneralApprovalStateRepository { get; set; }
        public IGlobalUserRepository GlobalUserRepository { get; set; }
        public IGlobalUserTypeRepository GlobalUserTypeRepository { get; set; }
        public IImageRepository ImageRepository { get; private set; }
        public IFileRepository FileRepository { get; private set; }
        public IIndirectSalesOrderContentRepository IndirectSalesOrderContentRepository { get; private set; }
        public IIndirectSalesOrderRepository IndirectSalesOrderRepository { get; private set; }
        public IIndirectSalesOrderPromotionRepository IndirectSalesOrderPromotionRepository { get; private set; }
        public IIndirectSalesOrderTransactionRepository IndirectSalesOrderTransactionRepository { get; private set; }
        public IItemRepository ItemRepository { get; private set; }
        public IItemHistoryRepository ItemHistoryRepository { get; private set; }
        public IInventoryRepository InventoryRepository { get; private set; }
        public IInventoryHistoryRepository InventoryHistoryRepository { get; private set; }
        public IKpiCriteriaGeneralRepository KpiCriteriaGeneralRepository { get; private set; }
        public IKpiCriteriaItemRepository KpiCriteriaItemRepository { get; private set; }
        public IKpiGeneralRepository KpiGeneralRepository { get; private set; }
        public IKpiGeneralContentRepository KpiGeneralContentRepository { get; private set; }
        public IKpiItemContentRepository KpiItemContentRepository { get; private set; }
        public IKpiItemRepository KpiItemRepository { get; private set; }
        public IKpiPeriodRepository KpiPeriodRepository { get; private set; }
        public IKpiYearRepository KpiYearRepository { get; private set; }
        public IKpiItemTypeRepository KpiItemTypeRepository { get; private set; }
        public ILuckyDrawRepository LuckyDrawRepository { get; private set; }
        public ILuckyDrawNumberRepository LuckyDrawNumberRepository { get; private set; }
        public ILuckyDrawRegistrationRepository LuckyDrawRegistrationRepository { get; private set; }
        public ILuckyDrawStructureRepository LuckyDrawStructureRepository { get; private set; }
        public ILuckyDrawTypeRepository LuckyDrawTypeRepository { get; private set; }
        public ILuckyDrawWinnerRepository LuckyDrawWinnerRepository { get; private set; }
        public ILuckyDrawStoreMappingRepository LuckyDrawStoreMappingRepository { get; private set; }
        public ILuckyDrawStoreTypeMappingRepository LuckyDrawStoreTypeMappingRepository { get; private set; }
        public ILuckyDrawStoreGroupingMappingRepository LuckyDrawStoreGroupingMappingRepository { get; private set; }
        public ILuckyNumberRepository LuckyNumberRepository { get; private set; }
        public ILuckyNumberGroupingRepository LuckyNumberGroupingRepository { get; private set; }
        public IMenuRepository MenuRepository { get; private set; }
        public INationRepository NationRepository { get; private set; }
        public INotificationRepository NotificationRepository { get; private set; }
        public INotificationStatusRepository NotificationStatusRepository { get; private set; }
        public IOrganizationRepository OrganizationRepository { get; private set; }
        public IPermissionOperatorRepository PermissionOperatorRepository { get; private set; }
        public IPermissionRepository PermissionRepository { get; private set; }
        public IPositionRepository PositionRepository { get; private set; }
        public IPriceListRepository PriceListRepository { get; private set; }
        public IPriceListTypeRepository PriceListTypeRepository { get; private set; }
        public IPriceListItemHistoryRepository PriceListItemHistoryRepository { get; private set; }
        public IPriceListItemMappingRepository PriceListItemMappingRepository { get; private set; }
        public IProblemRepository ProblemRepository { get; private set; }
        public IProblemHistoryRepository ProblemHistoryRepository { get; private set; }
        public IProblemTypeRepository ProblemTypeRepository { get; private set; }
        public IProblemStatusRepository ProblemStatusRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public IProductGroupingRepository ProductGroupingRepository { get; private set; }
        public IProductGroupingHistoryRepository ProductGroupingHistoryRepository { get; private set; }
        public IProductTypeRepository ProductTypeRepository { get; private set; }
        public IPromotionCodeRepository PromotionCodeRepository { get; private set; }
        public IPromotionCodeHistoryRepository PromotionCodeHistoryRepository { get; private set; }
        public IPromotionComboRepository PromotionComboRepository { get; private set; }
        public IPromotionRepository PromotionRepository { get; private set; }
        public IPromotionDirectSalesOrderRepository PromotionDirectSalesOrderRepository { get; private set; }
        public IPromotionDiscountTypeRepository PromotionDiscountTypeRepository { get; private set; }
        public IPromotionProductTypeRepository PromotionProductTypeRepository { get; private set; }
        public IPromotionPolicyRepository PromotionPolicyRepository { get; private set; }
        public IRewardHistoryContentRepository RewardHistoryContentRepository { get; private set; }
        public IRewardHistoryRepository RewardHistoryRepository { get; private set; }
        public IRewardStatusRepository RewardStatusRepository { get; private set; }
        public IPromotionProductAppliedTypeRepository PromotionProductAppliedTypeRepository { get; private set; }
        public IPromotionProductRepository PromotionProductRepository { get; private set; }
        public IPromotionProductGroupingRepository PromotionProductGroupingRepository { get; private set; }
        public IPromotionSamePriceRepository PromotionSamePriceRepository { get; private set; }
        public IPromotionStoreRepository PromotionStoreRepository { get; private set; }
        public IPromotionStoreGroupingRepository PromotionStoreGroupingRepository { get; private set; }
        public IPromotionStoreTypeRepository PromotionStoreTypeRepository { get; private set; }
        public IPromotionTypeRepository PromotionTypeRepository { get; private set; }
        public IProvinceRepository ProvinceRepository { get; private set; }
        public IRequestHistoryRepository RequestHistoryRepository { get; private set; }
        public IRequestStateRepository RequestStateRepository { get; private set; }
        public IRequestWorkflowDefinitionMappingRepository RequestWorkflowDefinitionMappingRepository { get; private set; }
        public IRequestWorkflowHistoryRepository RequestWorkflowHistoryRepository { get; private set; }
        public IRequestWorkflowParameterMappingRepository RequestWorkflowParameterMappingRepository { get; private set; }
        public IRequestWorkflowStepMappingRepository RequestWorkflowStepMappingRepository { get; private set; }
        public IRoleRepository RoleRepository { get; private set; }
        public ISalesOrderTypeRepository SalesOrderTypeRepository { get; private set; }
        public ISexRepository SexRepository { get; private set; }
        public IShowingCategoryRepository ShowingCategoryRepository { get; private set; }
        public IShowingItemRepository ShowingItemRepository { get; private set; }
        public IShowingOrderContentRepository ShowingOrderContentRepository { get; private set; }
        public IShowingOrderRepository ShowingOrderRepository { get; private set; }
        public IShowingOrderContentWithDrawRepository ShowingOrderContentWithDrawRepository { get; }
        public IShowingOrderWithDrawRepository ShowingOrderWithDrawRepository { get; }
        public IStatusRepository StatusRepository { get; private set; }
        public IStoreBalanceRepository StoreBalanceRepository { get; private set; }
        public IStoreScoutingRepository StoreScoutingRepository { get; private set; }
        public IStoreScoutingTypeRepository StoreScoutingTypeRepository { get; private set; }
        public IStoreScoutingStatusRepository StoreScoutingStatusRepository { get; private set; }
        public IStoreRepository StoreRepository { get; private set; }
        public IStoreHistoryRepository StoreHistoryRepository { get; private set; }
        public IStoreCheckingRepository StoreCheckingRepository { get; private set; }
        public IStoreGroupingRepository StoreGroupingRepository { get; private set; }
        public IStoreStatusHistoryTypeRepository StoreStatusHistoryTypeRepository { get; private set; }
        public IStoreStatusHistoryRepository StoreStatusHistoryRepository { get; private set; }
        public IStoreTypeRepository StoreTypeRepository { get; private set; }
        public IStoreStatusRepository StoreStatusRepository { get; private set; }
        public ISupplierRepository SupplierRepository { get; private set; }
        public ISurveyOptionTypeRepository SurveyOptionTypeRepository { get; private set; }
        public ISurveyQuestionTypeRepository SurveyQuestionTypeRepository { get; private set; }
        public ISurveyRepository SurveyRepository { get; private set; }
        public ISurveyResultRepository SurveyResultRepository { get; private set; }
        public ISystemConfigurationRepository SystemConfigurationRepository { get; private set; }
        public ITaxTypeRepository TaxTypeRepository { get; private set; }
        public ITransactionTypeRepository TransactionTypeRepository { get; private set; }
        public IUnitOfMeasureRepository UnitOfMeasureRepository { get; private set; }
        public IUnitOfMeasureGroupingContentRepository UnitOfMeasureGroupingContentRepository { get; private set; }
        public IUnitOfMeasureGroupingRepository UnitOfMeasureGroupingRepository { get; private set; }
        public IUsedVariationRepository UsedVariationRepository { get; private set; }
        public IVariationRepository VariationRepository { get; private set; }
        public IVariationGroupingRepository VariationGroupingRepository { get; private set; }
        public IWardRepository WardRepository { get; private set; }
        public IWarehouseRepository WarehouseRepository { get; private set; }
        public IWorkflowDefinitionRepository WorkflowDefinitionRepository { get; private set; }
        public IWorkflowDirectionRepository WorkflowDirectionRepository { get; private set; }
        public IWorkflowOperatorRepository WorkflowOperatorRepository { get; private set; }
        public IWorkflowParameterRepository WorkflowParameterRepository { get; private set; }
        public IWorkflowParameterTypeRepository WorkflowParameterTypeRepository { get; private set; }
        public IWorkflowStateRepository WorkflowStateRepository { get; private set; }
        public IWorkflowStepRepository WorkflowStepRepository { get; private set; }
        public IWorkflowTypeRepository WorkflowTypeRepository { get; private set; }
        public IStoreUserRepository StoreUserRepository { get; private set; }
        public IKpiProductGroupingRepository KpiProductGroupingRepository { get; private set; }
        public IKpiProductGroupingContentRepository KpiProductGroupingContentRepository { get; private set; }
        public IKpiProductGroupingCriteriaRepository KpiProductGroupingCriteriaRepository { get; private set; }
        public IKpiProductGroupingTypeRepository KpiProductGroupingTypeRepository { get; private set; }
        public IStoreApprovalStateRepository StoreApprovalStateRepository { get; private set; }
        public IErpApprovalStateRepository ErpApprovalStateRepository { get; private set; }
        public IStoreTypeHistoryRepository StoreTypeHistoryRepository { get; private set; }
        public IEsDistrictRepository EsDistrictRepository { get; private set; }
        public IEsProductRepository EsProductRepository { get; private set; }
        public IEsProvinceRepository EsProvinceRepository { get; private set; }
        public IEsStoreRepository EsStoreRepository { get; private set; }
        public IEsWardRepository EsWardRepository { get; private set; }

        public UOW(DataContext DataContext, IConfiguration Configuration, IElasticClient ElasticClient, IRedisStore RedisStore)
        {
            this.DataContext = DataContext;
            //this.DataContext.Database.ExecuteSqlCommand("SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;");
            var MongoClient = new MongoClient(Configuration["MongoConnection:ConnectionString"]);

            AlbumRepository = new AlbumRepository(DataContext);
            AppUserRepository = new AppUserRepository(DataContext, RedisStore, Configuration);
            AppUserStoreMappingRepository = new AppUserStoreMappingRepository(DataContext);
            BrandRepository = new BrandRepository(DataContext);
            BrandHistoryRepository = new BrandHistoryRepository(DataContext);
            BannerRepository = new BannerRepository(DataContext);
            CategoryRepository = new CategoryRepository(DataContext);
            CheckStateRepository = new CheckStateRepository(DataContext);
            ColorRepository = new ColorRepository(DataContext);
            CommentRepository = new CommentRepository(DataContext);
            CodeGeneratorRuleRepository = new CodeGeneratorRuleRepository(DataContext);
            ConversationConfigurationRepository = new ConversationConfigurationRepository(DataContext);
            ConversationMessageRepository = new ConversationMessageRepository(DataContext);
            ConversationTypeRepository = new ConversationTypeRepository(DataContext);
            ConversationReadHistoryRepository = new ConversationReadHistoryRepository(DataContext);
            ConversationRepository = new ConversationRepository(DataContext);
            DirectSalesOrderContentRepository = new DirectSalesOrderContentRepository(DataContext);
            DirectSalesOrderRepository = new DirectSalesOrderRepository(DataContext);
            DirectSalesOrderPromotionRepository = new DirectSalesOrderPromotionRepository(DataContext);
            DirectSalesOrderSourceTypeRepository = new DirectSalesOrderSourceTypeRepository(DataContext);
            DistrictRepository = new DistrictRepository(DataContext);
            EditedPriceStatusRepository = new EditedPriceStatusRepository(DataContext);
            EntityComponentRepository = new EntityComponentRepository(DataContext);
            EntityTypeRepository = new EntityTypeRepository(DataContext);
            ERouteChangeRequestContentRepository = new ERouteChangeRequestContentRepository(DataContext);
            ERouteChangeRequestRepository = new ERouteChangeRequestRepository(DataContext);
            ERouteContentRepository = new ERouteContentRepository(DataContext, RedisStore,Configuration);
            ERouteRepository = new ERouteRepository(DataContext, RedisStore, Configuration);
            ERouteTypeRepository = new ERouteTypeRepository(DataContext);
            ExportTemplateRepository = new ExportTemplateRepository(DataContext);
            EsItemRepository = new EsItemRepository(ElasticClient);
            EstimatedRevenueRepository = new EstimatedRevenueRepository(DataContext);
            FieldRepository = new FieldRepository(DataContext);
            ImageRepository = new ImageRepository(DataContext);
            FileRepository = new FileRepository(DataContext);
            GeneralApprovalStateRepository = new GeneralApprovalStateRepository(DataContext);
            GlobalUserRepository = new GlobalUserRepository(DataContext);
            GlobalUserTypeRepository = new GlobalUserTypeRepository(DataContext);
            IndirectSalesOrderContentRepository = new IndirectSalesOrderContentRepository(DataContext);
            IndirectSalesOrderRepository = new IndirectSalesOrderRepository(DataContext);
            IndirectSalesOrderPromotionRepository = new IndirectSalesOrderPromotionRepository(DataContext);
            IndirectSalesOrderTransactionRepository = new IndirectSalesOrderTransactionRepository(DataContext);
            ItemRepository = new ItemRepository(DataContext);
            ItemHistoryRepository = new ItemHistoryRepository(DataContext);
            InventoryRepository = new InventoryRepository(DataContext);
            InventoryHistoryRepository = new InventoryHistoryRepository(DataContext);
            KpiCriteriaGeneralRepository = new KpiCriteriaGeneralRepository(DataContext);
            KpiCriteriaItemRepository = new KpiCriteriaItemRepository(DataContext);
            KpiGeneralRepository = new KpiGeneralRepository(DataContext);
            KpiGeneralContentRepository = new KpiGeneralContentRepository(DataContext);
            KpiItemContentRepository = new KpiItemContentRepository(DataContext);
            KpiItemRepository = new KpiItemRepository(DataContext);
            KpiPeriodRepository = new KpiPeriodRepository(DataContext);
            KpiYearRepository = new KpiYearRepository(DataContext);
            KpiItemTypeRepository = new KpiItemTypeRepository(DataContext);
            LuckyDrawRepository = new LuckyDrawRepository(DataContext);
            LuckyDrawNumberRepository = new LuckyDrawNumberRepository(DataContext);
            LuckyDrawRegistrationRepository = new LuckyDrawRegistrationRepository(DataContext);
            LuckyDrawStructureRepository = new LuckyDrawStructureRepository(DataContext);
            LuckyDrawTypeRepository = new LuckyDrawTypeRepository(DataContext);
            LuckyDrawWinnerRepository = new LuckyDrawWinnerRepository(DataContext);
            LuckyNumberRepository = new LuckyNumberRepository(DataContext);
            LuckyDrawStoreMappingRepository = new LuckyDrawStoreMappingRepository(DataContext);
            LuckyDrawStoreGroupingMappingRepository = new LuckyDrawStoreGroupingMappingRepository(DataContext);
            LuckyDrawStoreTypeMappingRepository = new LuckyDrawStoreTypeMappingRepository(DataContext);
            LuckyNumberGroupingRepository = new LuckyNumberGroupingRepository(DataContext);
            MenuRepository = new MenuRepository(DataContext);
            NationRepository = new NationRepository(DataContext);
            NotificationRepository = new NotificationRepository(DataContext);
            NotificationStatusRepository = new NotificationStatusRepository(DataContext);
            OrganizationRepository = new OrganizationRepository(DataContext);
            PermissionOperatorRepository = new PermissionOperatorRepository(DataContext);
            PermissionRepository = new PermissionRepository(DataContext, RedisStore, Configuration);
            PositionRepository = new PositionRepository(DataContext);
            PriceListRepository = new PriceListRepository(DataContext);
            PriceListTypeRepository = new PriceListTypeRepository(DataContext);
            PriceListItemHistoryRepository = new PriceListItemHistoryRepository(DataContext);
            PriceListItemMappingRepository = new PriceListItemMappingRepository(DataContext);
            ProblemRepository = new ProblemRepository(DataContext);
            ProblemTypeRepository = new ProblemTypeRepository(DataContext);
            ProblemHistoryRepository = new ProblemHistoryRepository(DataContext);
            ProblemStatusRepository = new ProblemStatusRepository(DataContext);
            ProductRepository = new ProductRepository(DataContext);
            ProductGroupingRepository = new ProductGroupingRepository(DataContext);
            ProductGroupingHistoryRepository = new ProductGroupingHistoryRepository(DataContext);
            ProductTypeRepository = new ProductTypeRepository(DataContext);
            PromotionComboRepository = new PromotionComboRepository(DataContext);
            PromotionRepository = new PromotionRepository(DataContext);
            PromotionCodeRepository = new PromotionCodeRepository(DataContext);
            PromotionCodeHistoryRepository = new PromotionCodeHistoryRepository(DataContext);
            PromotionDirectSalesOrderRepository = new PromotionDirectSalesOrderRepository(DataContext);
            PromotionDiscountTypeRepository = new PromotionDiscountTypeRepository(DataContext);
            PromotionProductAppliedTypeRepository = new PromotionProductAppliedTypeRepository(DataContext);
            PromotionPolicyRepository = new PromotionPolicyRepository(DataContext);
            PromotionProductRepository = new PromotionProductRepository(DataContext);
            PromotionProductGroupingRepository = new PromotionProductGroupingRepository(DataContext);
            PromotionProductTypeRepository = new PromotionProductTypeRepository(DataContext);
            PromotionSamePriceRepository = new PromotionSamePriceRepository(DataContext);
            PromotionStoreRepository = new PromotionStoreRepository(DataContext);
            PromotionStoreGroupingRepository = new PromotionStoreGroupingRepository(DataContext);
            PromotionStoreTypeRepository = new PromotionStoreTypeRepository(DataContext);
            PromotionTypeRepository = new PromotionTypeRepository(DataContext);
            ProvinceRepository = new ProvinceRepository(DataContext);
            RequestHistoryRepository = new RequestHistoryRepository(DataContext, MongoClient);
            RequestStateRepository = new RequestStateRepository(DataContext);
            RequestWorkflowDefinitionMappingRepository = new RequestWorkflowDefinitionMappingRepository(DataContext);
            RequestWorkflowHistoryRepository = new RequestWorkflowHistoryRepository(DataContext);
            RequestWorkflowParameterMappingRepository = new RequestWorkflowParameterMappingRepository(DataContext);
            RequestWorkflowStepMappingRepository = new RequestWorkflowStepMappingRepository(DataContext);
            RewardHistoryContentRepository = new RewardHistoryContentRepository(DataContext);
            RewardHistoryRepository = new RewardHistoryRepository(DataContext);
            RewardStatusRepository = new RewardStatusRepository(DataContext);
            RoleRepository = new RoleRepository(DataContext, RedisStore, Configuration);
            SalesOrderTypeRepository = new SalesOrderTypeRepository(DataContext);
            SexRepository = new SexRepository(DataContext);
            ShowingCategoryRepository = new ShowingCategoryRepository(DataContext);
            ShowingItemRepository = new ShowingItemRepository(DataContext);
            ShowingOrderContentRepository = new ShowingOrderContentRepository(DataContext);
            ShowingOrderRepository = new ShowingOrderRepository(DataContext);
            ShowingOrderContentWithDrawRepository = new ShowingOrderContentWithDrawRepository(DataContext);
            ShowingOrderWithDrawRepository = new ShowingOrderWithDrawRepository(DataContext);
            StatusRepository = new StatusRepository(DataContext);
            StoreBalanceRepository = new StoreBalanceRepository(DataContext);
            StoreScoutingStatusRepository = new StoreScoutingStatusRepository(DataContext);
            StoreScoutingRepository = new StoreScoutingRepository(DataContext);
            StoreScoutingTypeRepository = new StoreScoutingTypeRepository(DataContext);
            StoreRepository = new StoreRepository(DataContext);
            StoreHistoryRepository = new StoreHistoryRepository(DataContext);
            StoreCheckingRepository = new StoreCheckingRepository(DataContext);
            StoreGroupingRepository = new StoreGroupingRepository(DataContext);
            StoreStatusHistoryTypeRepository = new StoreStatusHistoryTypeRepository(DataContext);
            StoreStatusHistoryRepository = new StoreStatusHistoryRepository(DataContext);
            StoreTypeRepository = new StoreTypeRepository(DataContext);
            StoreStatusRepository = new StoreStatusRepository(DataContext);
            SupplierRepository = new SupplierRepository(DataContext);
            SurveyOptionTypeRepository = new SurveyOptionTypeRepository(DataContext);
            SurveyQuestionTypeRepository = new SurveyQuestionTypeRepository(DataContext);
            SurveyRepository = new SurveyRepository(DataContext);
            SurveyResultRepository = new SurveyResultRepository(DataContext);
            SystemConfigurationRepository = new SystemConfigurationRepository(DataContext, RedisStore, Configuration);
            TaxTypeRepository = new TaxTypeRepository(DataContext);
            TransactionTypeRepository = new TransactionTypeRepository(DataContext);
            UnitOfMeasureRepository = new UnitOfMeasureRepository(DataContext);
            UnitOfMeasureGroupingContentRepository = new UnitOfMeasureGroupingContentRepository(DataContext);
            UnitOfMeasureGroupingRepository = new UnitOfMeasureGroupingRepository(DataContext);
            UsedVariationRepository = new UsedVariationRepository(DataContext);
            VariationRepository = new VariationRepository(DataContext);
            VariationGroupingRepository = new VariationGroupingRepository(DataContext);
            WardRepository = new WardRepository(DataContext);
            WarehouseRepository = new WarehouseRepository(DataContext);
            WorkflowDefinitionRepository = new WorkflowDefinitionRepository(DataContext);
            WorkflowDirectionRepository = new WorkflowDirectionRepository(DataContext);
            WorkflowOperatorRepository = new WorkflowOperatorRepository(DataContext);
            WorkflowParameterRepository = new WorkflowParameterRepository(DataContext);
            WorkflowParameterTypeRepository = new WorkflowParameterTypeRepository(DataContext);
            WorkflowStateRepository = new WorkflowStateRepository(DataContext);
            WorkflowStepRepository = new WorkflowStepRepository(DataContext);
            WorkflowTypeRepository = new WorkflowTypeRepository(DataContext);
            StoreUserRepository = new StoreUserRepository(DataContext);
            KpiProductGroupingRepository = new KpiProductGroupingRepository(DataContext);
            KpiProductGroupingContentRepository = new KpiProductGroupingContentRepository(DataContext);
            KpiProductGroupingCriteriaRepository = new KpiProductGroupingCriteriaRepository(DataContext);
            KpiProductGroupingTypeRepository = new KpiProductGroupingTypeRepository(DataContext);
            StoreApprovalStateRepository = new StoreApprovalStateRepository(DataContext);
            ErpApprovalStateRepository = new ErpApprovalStateRepository(DataContext);
            StoreTypeHistoryRepository = new StoreTypeHistoryRepository(DataContext);
            EsDistrictRepository = new EsDistrictRepository(ElasticClient);
            EsProductRepository = new EsProductRepository(ElasticClient);
            EsProvinceRepository = new EsProvinceRepository(ElasticClient);
            EsStoreRepository = new EsStoreRepository(ElasticClient);
            EsWardRepository = new EsWardRepository(ElasticClient);
        }

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposing)
            {
                return;
            }

            if (this.DataContext == null)
            {
                return;
            }
            this.DataContext.Dispose();
            this.DataContext = null;

        }
    }
}