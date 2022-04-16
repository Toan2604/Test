using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Models;
using System;
using System.Threading.Tasks;
using Nest;

namespace DMS.ABE.Repositories
{
    public interface IUOW : IServiceScoped, IDisposable
    {
        Task Begin();
        Task Commit();
        Task Rollback();

        IAppUserRepository AppUserRepository { get; }
        IBannerRepository BannerRepository { get; }
        IBrandRepository BrandRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ICommentRepository CommentRepository { get; }
        IConversationConfigurationRepository ConversationConfigurationRepository { get; }
        IConversationMessageRepository ConversationMessageRepository { get; }
        IConversationTypeRepository ConversationTypeRepository { get; }
        IConversationReadHistoryRepository ConversationReadHistoryRepository { get; }
        IConversationRepository ConversationRepository { get; }
        IDirectSalesOrderContentRepository DirectSalesOrderContentRepository { get; }
        IDirectSalesOrderRepository DirectSalesOrderRepository { get; }
        IDirectSalesOrderPromotionRepository DirectSalesOrderPromotionRepository { get; }
        IErpApprovalStateRepository ErpApprovalStateRepository { get; }
        IExportTemplateRepository ExportTemplateRepository { get; }
        IGeneralApprovalStateRepository GeneralApprovalStateRepository { get; }
        IFieldRepository FieldRepository { get; }
        IFileRepository FileRepository { get; }
        IGlobalUserRepository GlobalUserRepository { get; }
        IImageRepository ImageRepository { get; }
        IEsItemRepository EsItemRepository { get; }
        IItemRepository ItemRepository { get; }
        IInventoryRepository InventoryRepository { get; }
        ILuckyDrawRepository LuckyDrawRepository { get; }
        ILuckyDrawNumberRepository LuckyDrawNumberRepository { get; }
        ILuckyDrawRegistrationRepository LuckyDrawRegistrationRepository { get; }
        ILuckyDrawStoreMappingRepository LuckyDrawStoreMappingRepository { get; }
        ILuckyDrawStoreTypeMappingRepository LuckyDrawStoreTypeMappingRepository { get; }
        ILuckyDrawStoreGroupingMappingRepository LuckyDrawStoreGroupingMappingRepository { get; }
        ILuckyDrawStructureRepository LuckyDrawStructureRepository { get; }
        ILuckyDrawWinnerRepository LuckyDrawWinnerRepository { get; }
        IOrganizationRepository OrganizationRepository { get; }
        IPermissionRepository PermissionRepository { get; }
        IPriceListItemMappingItemMappingRepository PriceListItemMappingItemMappingRepository { get; }
        IEsProductRepository EsProductRepository { get; }
        IProductRepository ProductRepository { get; }
        IProductGroupingRepository ProductGroupingRepository { get; }
        IProductTypeRepository ProductTypeRepository { get; }
        IRequestWorkflowDefinitionMappingRepository RequestWorkflowDefinitionMappingRepository { get; }
        IRequestWorkflowHistoryRepository RequestWorkflowHistoryRepository { get; }
        IRequestWorkflowParameterMappingRepository RequestWorkflowParameterMappingRepository { get; }
        IRequestWorkflowStepMappingRepository RequestWorkflowStepMappingRepository { get; }
        IStoreRepository StoreRepository { get; }
        IStoreApprovalStateRepository StoreApprovalStateRepository { get; }
        IStoreTypeRepository StoreTypeRepository { get; }
        ISystemConfigurationRepository SystemConfigurationRepository { get; }
        IUnitOfMeasureGroupingRepository UnitOfMeasureGroupingRepository { get; }
        IVariationRepository VariationRepository { get; }
        IVariationGroupingRepository VariationGroupingRepository { get; }
        IWarehouseRepository WarehouseRepository { get; }
        IStoreUserRepository StoreUserRepository { get; }
        IStoreBalanceRepository StoreBalanceRepository { get; }
        IStoreUserFavoriteProductMappingRepository StoreUserFavoriteProductMappingRepository { get; }
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

        public IAppUserRepository AppUserRepository { get; private set; }
        public IBannerRepository BannerRepository { get; private set; }
        public IBrandRepository BrandRepository { get; private set; }
        public ICategoryRepository CategoryRepository { get; private set; }
        public ICommentRepository CommentRepository { get; private set; }
        public IConversationConfigurationRepository ConversationConfigurationRepository { get; private set; }
        public IConversationMessageRepository ConversationMessageRepository { get; private set; }
        public IConversationTypeRepository ConversationTypeRepository { get; private set; }
        public IConversationReadHistoryRepository ConversationReadHistoryRepository { get; private set; }
        public IConversationRepository ConversationRepository { get; private set; }
        public IDirectSalesOrderContentRepository DirectSalesOrderContentRepository { get; private set; }
        public IDirectSalesOrderRepository DirectSalesOrderRepository { get; private set; }
        public IDirectSalesOrderPromotionRepository DirectSalesOrderPromotionRepository { get; private set; }
        public IErpApprovalStateRepository ErpApprovalStateRepository { get; private set; }
        public IExportTemplateRepository ExportTemplateRepository { get; private set; }
        public IGeneralApprovalStateRepository GeneralApprovalStateRepository { get; private set; }
        public IFieldRepository FieldRepository { get; private set; }
        public IFileRepository FileRepository { get; private set; }
        public IGlobalUserRepository GlobalUserRepository { get; private set; }
        public IImageRepository ImageRepository { get; private set; }
        public IEsItemRepository EsItemRepository { get; private set; }
        public IItemRepository ItemRepository { get; private set; }
        public IInventoryRepository InventoryRepository { get; private set; }
        public ILuckyDrawRepository LuckyDrawRepository { get; private set; }
        public ILuckyDrawNumberRepository LuckyDrawNumberRepository { get; private set; }
        public ILuckyDrawRegistrationRepository LuckyDrawRegistrationRepository { get; private set; }
        public ILuckyDrawStoreMappingRepository LuckyDrawStoreMappingRepository { get; private set; }
        public ILuckyDrawStoreTypeMappingRepository LuckyDrawStoreTypeMappingRepository { get; private set; }
        public ILuckyDrawStoreGroupingMappingRepository LuckyDrawStoreGroupingMappingRepository { get; private set; }
        public ILuckyDrawStructureRepository LuckyDrawStructureRepository { get; private set; }
        public ILuckyDrawWinnerRepository LuckyDrawWinnerRepository { get; private set; }
        public IOrganizationRepository OrganizationRepository { get; private set; }
        public IPermissionRepository PermissionRepository { get; private set; }
        public IPriceListItemMappingItemMappingRepository PriceListItemMappingItemMappingRepository { get; private set; }
        public IEsProductRepository EsProductRepository { get; private set; }
        public IProductRepository ProductRepository { get; private set; }
        public IProductGroupingRepository ProductGroupingRepository { get; private set; }
        public IProductTypeRepository ProductTypeRepository { get; private set; }
        public IRequestWorkflowDefinitionMappingRepository RequestWorkflowDefinitionMappingRepository { get; private set; }
        public IRequestWorkflowHistoryRepository RequestWorkflowHistoryRepository { get; private set; }
        public IRequestWorkflowParameterMappingRepository RequestWorkflowParameterMappingRepository { get; private set; }
        public IRequestWorkflowStepMappingRepository RequestWorkflowStepMappingRepository { get; private set; }
        public IStoreRepository StoreRepository { get; private set; }
        public IStoreApprovalStateRepository StoreApprovalStateRepository { get; private set; }
        public IStoreTypeRepository StoreTypeRepository { get; private set; }
        public ISystemConfigurationRepository SystemConfigurationRepository { get; private set; }
        public IUnitOfMeasureGroupingRepository UnitOfMeasureGroupingRepository { get; private set; }
        public IVariationRepository VariationRepository { get; private set; }
        public IVariationGroupingRepository VariationGroupingRepository { get; private set; }
        public IWarehouseRepository WarehouseRepository { get; private set; }
        public IStoreUserRepository StoreUserRepository { get; private set; }
        public IStoreBalanceRepository StoreBalanceRepository { get; private set; }
        public IStoreUserFavoriteProductMappingRepository StoreUserFavoriteProductMappingRepository { get; private set; }
        public IWorkflowDefinitionRepository WorkflowDefinitionRepository { get; private set; }
        public IWorkflowDirectionRepository WorkflowDirectionRepository { get; private set; }
        public IWorkflowOperatorRepository WorkflowOperatorRepository { get; private set; }
        public IWorkflowParameterRepository WorkflowParameterRepository { get; private set; }
        public IWorkflowParameterTypeRepository WorkflowParameterTypeRepository { get; private set; }
        public IWorkflowStateRepository WorkflowStateRepository { get; private set; }
        public IWorkflowStepRepository WorkflowStepRepository { get; private set; }
        public IWorkflowTypeRepository WorkflowTypeRepository { get; private set; }

        public UOW(DataContext DataContext, IElasticClient ElasticClient)
        {
            this.DataContext = DataContext;
            AppUserRepository = new AppUserRepository(DataContext);
            BannerRepository = new BannerRepository(DataContext);
            BrandRepository = new BrandRepository(DataContext);
            CategoryRepository = new CategoryRepository(DataContext);
            CommentRepository = new CommentRepository(DataContext);
            ConversationConfigurationRepository = new ConversationConfigurationRepository(DataContext);
            ConversationMessageRepository = new ConversationMessageRepository(DataContext);
            ConversationTypeRepository = new ConversationTypeRepository(DataContext);
            ConversationReadHistoryRepository = new ConversationReadHistoryRepository(DataContext);
            ConversationRepository = new ConversationRepository(DataContext);
            DirectSalesOrderContentRepository = new DirectSalesOrderContentRepository(DataContext);
            DirectSalesOrderRepository = new DirectSalesOrderRepository(DataContext);
            DirectSalesOrderPromotionRepository = new DirectSalesOrderPromotionRepository(DataContext);
            ErpApprovalStateRepository = new ErpApprovalStateRepository(DataContext);
            GeneralApprovalStateRepository = new GeneralApprovalStateRepository(DataContext);
            FieldRepository = new FieldRepository(DataContext);
            GlobalUserRepository = new GlobalUserRepository(DataContext);
            ImageRepository = new ImageRepository(DataContext);
            EsItemRepository = new EsItemRepository(ElasticClient);
            ExportTemplateRepository = new ExportTemplateRepository(DataContext);
            FileRepository = new FileRepository(DataContext);
            ItemRepository = new ItemRepository(DataContext);
            InventoryRepository = new InventoryRepository(DataContext);
            LuckyDrawRepository = new LuckyDrawRepository(DataContext);
            LuckyDrawNumberRepository = new LuckyDrawNumberRepository(DataContext);
            LuckyDrawRegistrationRepository = new LuckyDrawRegistrationRepository(DataContext);
            LuckyDrawStoreMappingRepository = new LuckyDrawStoreMappingRepository(DataContext);
            LuckyDrawStoreGroupingMappingRepository = new LuckyDrawStoreGroupingMappingRepository(DataContext);
            LuckyDrawStoreTypeMappingRepository = new LuckyDrawStoreTypeMappingRepository(DataContext);
            LuckyDrawStructureRepository = new LuckyDrawStructureRepository(DataContext);
            LuckyDrawWinnerRepository = new LuckyDrawWinnerRepository(DataContext);
            OrganizationRepository = new OrganizationRepository(DataContext);
            PermissionRepository = new PermissionRepository(DataContext);
            PriceListItemMappingItemMappingRepository = new PriceListItemMappingItemMappingRepository(DataContext);
            ProductRepository = new ProductRepository(DataContext);
            RequestWorkflowDefinitionMappingRepository = new RequestWorkflowDefinitionMappingRepository(DataContext);
            RequestWorkflowHistoryRepository = new RequestWorkflowHistoryRepository(DataContext);
            RequestWorkflowParameterMappingRepository = new RequestWorkflowParameterMappingRepository(DataContext);
            RequestWorkflowStepMappingRepository = new RequestWorkflowStepMappingRepository(DataContext);
            EsProductRepository = new EsProductRepository(ElasticClient);
            ProductGroupingRepository = new ProductGroupingRepository(DataContext);
            ProductTypeRepository = new ProductTypeRepository(DataContext);
            StoreRepository = new StoreRepository(DataContext);
            StoreApprovalStateRepository = new StoreApprovalStateRepository(DataContext);
            StoreTypeRepository = new StoreTypeRepository(DataContext);
            SystemConfigurationRepository = new SystemConfigurationRepository(DataContext);
            UnitOfMeasureGroupingRepository = new UnitOfMeasureGroupingRepository(DataContext);
            VariationRepository = new VariationRepository(DataContext);
            VariationGroupingRepository = new VariationGroupingRepository(DataContext);
            WarehouseRepository = new WarehouseRepository(DataContext);
            StoreUserRepository = new StoreUserRepository(DataContext);
            StoreBalanceRepository = new StoreBalanceRepository(DataContext);
            StoreUserFavoriteProductMappingRepository = new StoreUserFavoriteProductMappingRepository(DataContext);
            WorkflowDefinitionRepository = new WorkflowDefinitionRepository(DataContext);
            WorkflowDirectionRepository = new WorkflowDirectionRepository(DataContext);
            WorkflowOperatorRepository = new WorkflowOperatorRepository(DataContext);
            WorkflowParameterRepository = new WorkflowParameterRepository(DataContext);
            WorkflowParameterTypeRepository = new WorkflowParameterTypeRepository(DataContext);
            WorkflowStateRepository = new WorkflowStateRepository(DataContext);
            WorkflowStepRepository = new WorkflowStepRepository(DataContext);
            WorkflowTypeRepository = new WorkflowTypeRepository(DataContext);
        }
        public async Task Begin()
        {
            return;
        }

        public Task Commit()
        {
            return Task.CompletedTask;
        }

        public Task Rollback()
        {
            return Task.CompletedTask;
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