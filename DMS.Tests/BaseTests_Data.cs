using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Models;
using DMS.Rpc;
using DMS.Services.MImage;
using LightBDD.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using NUnit.Framework;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.Handlers.Configuration;
using System.Data.SqlClient;

namespace DMS.Tests
{
    public partial class BaseTests : FeatureFixture
    {
        #region DAOs
        protected List<AlbumDAO> AlbumDAOs { get; set; }
        protected List<AlbumImageMappingDAO> AlbumImageMappingDAOs { get; set; }
        protected List<AppUserDAO> AppUserDAOs { get; set; }
        protected List<AppUserGpsDAO> AppUserGpsDAOs { get; set; }
        protected List<AppUserRoleMappingDAO> AppUserRoleMappingDAOs { get; set; }
        protected List<AppUserStoreMappingDAO> AppUserStoreMappingDAOs { get; set; }
        protected List<BannerDAO> BannerDAOs { get; set; }
        protected List<BrandDAO> BrandDAOs { get; set; }
        protected List<BrandHistoryDAO> BrandHistoryDAOs { get; set; }
        protected List<BrandInStoreDAO> BrandInStoreDAOs { get; set; }
        protected List<BrandInStoreHistoryDAO> BrandInStoreHistoryDAOs { get; set; }
        protected List<BrandInStoreProductGroupingMappingDAO> BrandInStoreProductGroupingMappingDAOs { get; set; }
        protected List<BrandInStoreShowingCategoryMappingDAO> BrandInStoreShowingCategoryMappingDAOs { get; set; }
        protected List<CategoryDAO> CategoryDAOs { get; set; }
        protected List<CheckStateDAO> CheckStateDAOs { get; set; }
        protected List<CodeGeneratorRuleDAO> CodeGeneratorRuleDAOs { get; set; }
        protected List<CodeGeneratorRuleEntityComponentMappingDAO> CodeGeneratorRuleEntityComponentMappingDAOs { get; set; }
        protected List<ColorDAO> ColorDAOs { get; set; }
        protected List<AttachmentTypeDAO> ConversationAttachmentTypeDAOs { get; set; }
        protected List<ConversationTypeDAO> ConversationTypeDAOs { get; set; }
        protected List<DirectSalesOrderContentDAO> DirectSalesOrderContentDAOs { get; set; }
        protected List<DirectSalesOrderDAO> DirectSalesOrderDAOs { get; set; }
        protected List<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotionDAOs { get; set; }
        protected List<DirectSalesOrderSourceTypeDAO> DirectSalesOrderSourceTypeDAOs { get; set; }
        protected List<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactionDAOs { get; set; }
        protected List<DistrictDAO> DistrictDAOs { get; set; }
        protected List<EditedPriceStatusDAO> EditedPriceStatusDAOs { get; set; }
        protected List<EntityComponentDAO> EntityComponentDAOs { get; set; }
        protected List<EntityTypeDAO> EntityTypeDAOs { get; set; }
        protected List<ERouteChangeRequestContentDAO> ERouteChangeRequestContentDAOs { get; set; }
        protected List<ERouteChangeRequestDAO> ERouteChangeRequestDAOs { get; set; }
        protected List<ERouteContentDAO> ERouteContentDAOs { get; set; }
        protected List<ERouteContentDayDAO> ERouteContentDayDAOs { get; set; }
        protected List<ERouteDAO> ERouteDAOs { get; set; }
        protected List<ERouteTypeDAO> ERouteTypeDAOs { get; set; }
        protected List<ErpApprovalStateDAO> ErpApprovalStateDAOs { get; set; }
        protected List<EstimatedRevenueDAO> EstimatedRevenueDAOs { get; set; }
        protected List<ExportTemplateDAO> ExportTemplateDAOs { get; set; }
        protected List<FileDAO> FileDAOs { get; set; }
        protected List<GeneralApprovalStateDAO> GeneralApprovalStateDAOs { get; set; }
        protected List<GlobalUserTypeDAO> GlobalUserTypeDAOs { get; set; }
        protected List<ImageDAO> ImageDAOs { get; set; }
        protected List<IndirectSalesOrderContentDAO> IndirectSalesOrderContentDAOs { get; set; }
        protected List<IndirectSalesOrderDAO> IndirectSalesOrderDAOs { get; set; }
        protected List<IndirectSalesOrderPromotionDAO> IndirectSalesOrderPromotionDAOs { get; set; }
        protected List<IndirectSalesOrderTransactionDAO> IndirectSalesOrderTransactionDAOs { get; set; }
        protected List<InventoryDAO> InventoryDAOs { get; set; }
        protected List<InventoryHistoryDAO> InventoryHistoryDAOs { get; set; }
        protected List<ItemDAO> ItemDAOs { get; set; }
        protected List<ItemHistoryDAO> ItemHistoryDAOs { get; set; }
        protected List<ItemImageMappingDAO> ItemImageMappingDAOs { get; set; }
        protected List<KpiCriteriaGeneralDAO> KpiCriteriaGeneralDAOs { get; set; }
        protected List<KpiCriteriaItemDAO> KpiCriteriaItemDAOs { get; set; }
        protected List<KpiGeneralContentDAO> KpiGeneralContentDAOs { get; set; }
        protected List<KpiGeneralContentKpiPeriodMappingDAO> KpiGeneralContentKpiPeriodMappingDAOs { get; set; }
        protected List<KpiGeneralDAO> KpiGeneralDAOs { get; set; }
        protected List<KpiItemContentDAO> KpiItemContentDAOs { get; set; }
        protected List<KpiItemContentKpiCriteriaItemMappingDAO> KpiItemContentKpiCriteriaItemMappingDAOs { get; set; }
        protected List<KpiItemDAO> KpiItemDAOs { get; set; }
        protected List<KpiItemTypeDAO> KpiItemTypeDAOs { get; set; }
        protected List<KpiPeriodDAO> KpiPeriodDAOs { get; set; }
        protected List<KpiProductGroupingContentCriteriaMappingDAO> KpiProductGroupingContentCriteriaMappingDAOs { get; set; }
        protected List<KpiProductGroupingContentDAO> KpiProductGroupingContentDAOs { get; set; }
        protected List<KpiProductGroupingContentItemMappingDAO> KpiProductGroupingContentItemMappingDAOs { get; set; }
        protected List<KpiProductGroupingCriteriaDAO> KpiProductGroupingCriteriaDAOs { get; set; }
        protected List<KpiProductGroupingDAO> KpiProductGroupingDAOs { get; set; }
        protected List<KpiProductGroupingTypeDAO> KpiProductGroupingTypeDAOs { get; set; }
        protected List<KpiYearDAO> KpiYearDAOs { get; set; }
        protected List<LuckyDrawDAO> LuckyDrawDAOs { get; set; }
        protected List<LuckyDrawNumberDAO> LuckyDrawNumberDAOs { get; set; }
        protected List<LuckyDrawRegistrationDAO> LuckyDrawRegistrationDAOs { get; set; }
        protected List<LuckyDrawStoreGroupingMappingDAO> LuckyDrawStoreGroupingMappingDAOs { get; set; }
        protected List<LuckyDrawStoreMappingDAO> LuckyDrawStoreMappingDAOs { get; set; }
        protected List<LuckyDrawStoreTypeMappingDAO> LuckyDrawStoreTypeMappingDAOs { get; set; }
        protected List<LuckyDrawStructureDAO> LuckyDrawStructureDAOs { get; set; }
        protected List<LuckyDrawTypeDAO> LuckyDrawTypeDAOs { get; set; }
        protected List<LuckyDrawWinnerDAO> LuckyDrawWinnerDAOs { get; set; }
        protected List<LuckyNumberDAO> LuckyNumberDAOs { get; set; }
        protected List<LuckyNumberGroupingDAO> LuckyNumberGroupingDAOs { get; set; }
        protected List<NationDAO> NationDAOs { get; set; }
        protected List<NotificationDAO> NotificationDAOs { get; set; }
        protected List<NotificationStatusDAO> NotificationStatusDAOs { get; set; }
        protected List<OrganizationDAO> OrganizationDAOs { get; set; }
        protected List<PositionDAO> PositionDAOs { get; set; }
        protected List<PriceListDAO> PriceListDAOs { get; set; }
        protected List<PriceListItemHistoryDAO> PriceListItemHistoryDAOs { get; set; }
        protected List<PriceListItemMappingDAO> PriceListItemMappingDAOs { get; set; }
        protected List<PriceListStoreGroupingMappingDAO> PriceListStoreGroupingMappingDAOs { get; set; }
        protected List<PriceListStoreMappingDAO> PriceListStoreMappingDAOs { get; set; }
        protected List<PriceListStoreTypeMappingDAO> PriceListStoreTypeMappingDAOs { get; set; }
        protected List<PriceListTypeDAO> PriceListTypeDAOs { get; set; }
        protected List<ProblemDAO> ProblemDAOs { get; set; }
        protected List<ProblemHistoryDAO> ProblemHistoryDAOs { get; set; }
        protected List<ProblemImageMappingDAO> ProblemImageMappingDAOs { get; set; }
        protected List<ProblemStatusDAO> ProblemStatusDAOs { get; set; }
        protected List<ProblemTypeDAO> ProblemTypeDAOs { get; set; }
        protected List<ProductDAO> ProductDAOs { get; set; }
        protected List<ProductGroupingDAO> ProductGroupingDAOs { get; set; }
        protected List<ProductGroupingHistoryDAO> ProductGroupingHistoryDAOs { get; set; }
        protected List<ProductImageMappingDAO> ProductImageMappingDAOs { get; set; }
        protected List<ProductProductGroupingMappingDAO> ProductProductGroupingMappingDAOs { get; set; }
        protected List<ProductTypeDAO> ProductTypeDAOs { get; set; }
        protected List<PromotionCodeDAO> PromotionCodeDAOs { get; set; }
        protected List<PromotionCodeHistoryDAO> PromotionCodeHistoryDAOs { get; set; }
        protected List<PromotionCodeOrganizationMappingDAO> PromotionCodeOrganizationMappingDAOs { get; set; }
        protected List<PromotionCodeProductMappingDAO> PromotionCodeProductMappingDAOs { get; set; }
        protected List<PromotionCodeStoreMappingDAO> PromotionCodeStoreMappingDAOs { get; set; }
        protected List<PromotionComboDAO> PromotionComboDAOs { get; set; }
        protected List<PromotionComboInItemMappingDAO> PromotionComboInItemMappingDAOs { get; set; }
        protected List<PromotionComboOutItemMappingDAO> PromotionComboOutItemMappingDAOs { get; set; }
        protected List<PromotionDAO> PromotionDAOs { get; set; }
        protected List<PromotionDirectSalesOrderDAO> PromotionDirectSalesOrderDAOs { get; set; }
        protected List<PromotionDirectSalesOrderItemMappingDAO> PromotionDirectSalesOrderItemMappingDAOs { get; set; }
        protected List<PromotionDiscountTypeDAO> PromotionDiscountTypeDAOs { get; set; }
        protected List<PromotionPolicyDAO> PromotionPolicyDAOs { get; set; }
        protected List<PromotionProductAppliedTypeDAO> PromotionProductAppliedTypeDAOs { get; set; }
        protected List<PromotionProductDAO> PromotionProductDAOs { get; set; }
        protected List<PromotionProductGroupingDAO> PromotionProductGroupingDAOs { get; set; }
        protected List<PromotionProductGroupingItemMappingDAO> PromotionProductGroupingItemMappingDAOs { get; set; }
        protected List<PromotionProductItemMappingDAO> PromotionProductItemMappingDAOs { get; set; }
        protected List<PromotionProductTypeDAO> PromotionProductTypeDAOs { get; set; }
        protected List<PromotionProductTypeItemMappingDAO> PromotionProductTypeItemMappingDAOs { get; set; }
        protected List<PromotionPromotionPolicyMappingDAO> PromotionPromotionPolicyMappingDAOs { get; set; }
        protected List<PromotionSamePriceDAO> PromotionSamePriceDAOs { get; set; }
        protected List<PromotionStoreDAO> PromotionStoreDAOs { get; set; }
        protected List<PromotionStoreGroupingDAO> PromotionStoreGroupingDAOs { get; set; }
        protected List<PromotionStoreGroupingItemMappingDAO> PromotionStoreGroupingItemMappingDAOs { get; set; }
        protected List<PromotionStoreGroupingMappingDAO> PromotionStoreGroupingMappingDAOs { get; set; }
        protected List<PromotionStoreItemMappingDAO> PromotionStoreItemMappingDAOs { get; set; }
        protected List<PromotionStoreMappingDAO> PromotionStoreMappingDAOs { get; set; }
        protected List<PromotionStoreTypeDAO> PromotionStoreTypeDAOs { get; set; }
        protected List<PromotionStoreTypeItemMappingDAO> PromotionStoreTypeItemMappingDAOs { get; set; }
        protected List<PromotionStoreTypeMappingDAO> PromotionStoreTypeMappingDAOs { get; set; }
        protected List<PromotionTypeDAO> PromotionTypeDAOs { get; set; }
        protected List<ProvinceDAO> ProvinceDAOs { get; set; }
        protected List<RequestStateDAO> RequestStateDAOs { get; set; }
        protected List<RequestWorkflowDefinitionMappingDAO> RequestWorkflowDefinitionMappingDAOs { get; set; }
        protected List<RequestWorkflowHistoryDAO> RequestWorkflowHistoryDAOs { get; set; }
        protected List<RequestWorkflowParameterMappingDAO> RequestWorkflowParameterMappingDAOs { get; set; }
        protected List<RequestWorkflowStepMappingDAO> RequestWorkflowStepMappingDAOs { get; set; }
        protected List<RewardHistoryContentDAO> RewardHistoryContentDAOs { get; set; }
        protected List<RewardHistoryDAO> RewardHistoryDAOs { get; set; }
        protected List<RewardStatusDAO> RewardStatusDAOs { get; set; }
        protected List<RoleDAO> RoleDAOs { get; set; }
        protected List<SalesOrderTypeDAO> SalesOrderTypeDAOs { get; set; }
        protected List<SexDAO> SexDAOs { get; set; }
        protected List<StatusDAO> StatusDAOs { get; set; }
        protected List<StoreApprovalStateDAO> StoreApprovalStateDAOs { get; set; }
        protected List<StoreBalanceDAO> StoreBalanceDAOs { get; set; }
        protected List<StoreCheckingDAO> StoreCheckingDAOs { get; set; }
        protected List<StoreCheckingImageMappingDAO> StoreCheckingImageMappingDAOs { get; set; }
        protected List<StoreDAO> StoreDAOs { get; set; }
        protected List<StoreGroupingDAO> StoreGroupingDAOs { get; set; }
        protected List<StoreHistoryDAO> StoreHistoryDAOs { get; set; }
        protected List<StoreImageDAO> StoreImageDAOs { get; set; }
        protected List<StoreImageMappingDAO> StoreImageMappingDAOs { get; set; }
        protected List<StoreScoutingDAO> StoreScoutingDAOs { get; set; }
        protected List<StoreScoutingImageMappingDAO> StoreScoutingImageMappingDAOs { get; set; }
        protected List<StoreScoutingStatusDAO> StoreScoutingStatusDAOs { get; set; }
        protected List<StoreScoutingTypeDAO> StoreScoutingTypeDAOs { get; set; }
        protected List<StoreStatusDAO> StoreStatusDAOs { get; set; }
        protected List<StoreStatusHistoryDAO> StoreStatusHistoryDAOs { get; set; }
        protected List<StoreStatusHistoryTypeDAO> StoreStatusHistoryTypeDAOs { get; set; }
        protected List<StoreStoreGroupingMappingDAO> StoreStoreGroupingMappingDAOs { get; set; }
        protected List<StoreTypeDAO> StoreTypeDAOs { get; set; }
        protected List<StoreTypeHistoryDAO> StoreTypeHistoryDAOs { get; set; }
        protected List<StoreUncheckingDAO> StoreUncheckingDAOs { get; set; }
        protected List<StoreUserDAO> StoreUserDAOs { get; set; }
        protected List<StoreUserFavoriteProductMappingDAO> StoreUserFavoriteProductMappingDAOs { get; set; }
        protected List<SupplierDAO> SupplierDAOs { get; set; }
        protected List<SurveyDAO> SurveyDAOs { get; set; }
        protected List<SurveyOptionDAO> SurveyOptionDAOs { get; set; }
        protected List<SurveyOptionTypeDAO> SurveyOptionTypeDAOs { get; set; }
        protected List<SurveyQuestionDAO> SurveyQuestionDAOs { get; set; }
        protected List<SurveyQuestionFileMappingDAO> SurveyQuestionFileMappingDAOs { get; set; }
        protected List<SurveyQuestionImageMappingDAO> SurveyQuestionImageMappingDAOs { get; set; }
        protected List<SurveyQuestionTypeDAO> SurveyQuestionTypeDAOs { get; set; }
        protected List<SurveyRespondentTypeDAO> SurveyRespondentTypeDAOs { get; set; }
        protected List<SurveyResultCellDAO> SurveyResultCellDAOs { get; set; }
        protected List<SurveyResultDAO> SurveyResultDAOs { get; set; }
        protected List<SurveyResultSingleDAO> SurveyResultSingleDAOs { get; set; }
        protected List<SurveyResultTextDAO> SurveyResultTextDAOs { get; set; }
        protected List<SystemConfigurationDAO> SystemConfigurationDAOs { get; set; }
        protected List<TaxTypeDAO> TaxTypeDAOs { get; set; }
        protected List<TransactionTypeDAO> TransactionTypeDAOs { get; set; }
        protected List<UnitOfMeasureDAO> UnitOfMeasureDAOs { get; set; }
        protected List<UnitOfMeasureGroupingContentDAO> UnitOfMeasureGroupingContentDAOs { get; set; }
        protected List<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupingDAOs { get; set; }
        protected List<UsedVariationDAO> UsedVariationDAOs { get; set; }
        protected List<VariationDAO> VariationDAOs { get; set; }
        protected List<VariationGroupingDAO> VariationGroupingDAOs { get; set; }
        protected List<WardDAO> WardDAOs { get; set; }
        protected List<WarehouseDAO> WarehouseDAOs { get; set; }
        protected List<WarehouseOrganizationMappingDAO> WarehouseOrganizationMappingDAOs { get; set; }
        protected List<WorkflowDefinitionDAO> WorkflowDefinitionDAOs { get; set; }
        protected List<WorkflowDirectionConditionDAO> WorkflowDirectionConditionDAOs { get; set; }
        protected List<WorkflowDirectionDAO> WorkflowDirectionDAOs { get; set; }
        protected List<WorkflowOperatorDAO> WorkflowOperatorDAOs { get; set; }
        protected List<WorkflowParameterDAO> WorkflowParameterDAOs { get; set; }
        protected List<WorkflowParameterTypeDAO> WorkflowParameterTypeDAOs { get; set; }
        protected List<WorkflowStateDAO> WorkflowStateDAOs { get; set; }
        protected List<WorkflowStepDAO> WorkflowStepDAOs { get; set; }
        protected List<WorkflowTypeDAO> WorkflowTypeDAOs { get; set; }


        protected List<ActionDAO> ActionDAOs;
        protected List<ActionPageMappingDAO> ActionPageMappingDAOs;
        protected List<FieldDAO> FieldDAOs { get; set; }
        protected List<FieldTypeDAO> FieldTypeDAOs { get; set; }
        protected List<MenuDAO> MenuDAOs { get; set; }
        protected List<PageDAO> PageDAOs { get; set; }
        protected List<PermissionActionMappingDAO> PermissionActionMappingDAOs { get; set; }
        protected List<PermissionContentDAO> PermissionContentDAOs { get; set; }
        protected List<PermissionDAO> PermissionDAOs { get; set; }
        protected List<PermissionOperatorDAO> PermissionOperatorDAOs { get; set; }
#endregion
        /// <summary>
        /// 
        /// </summary>
        /// 

        protected async Task LoadExcel(string path, string DatabaseName)
        {
            string SnapshotName = $"{DatabaseName}_Snapshot";
            this.DataContext = ServiceProvider.GetService<DataContext>();

            int result = await DataContext.Store.FromSqlRaw($@"SELECT * FROM sys.databases WHERE NAME = '{SnapshotName}'").CountAsync();
            if (result > 0)
            {
                return;
            }

            MemoryStream MemoryStream = ReadFile(path);
            using (ExcelPackage excelPackage = new ExcelPackage(MemoryStream))
            {
                #region Permission
                ExcelWorksheet wsStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Status)).FirstOrDefault();
                if (wsStatus != null)
                    await Given_Status(wsStatus);
                ExcelWorksheet wsSex = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Sex)).FirstOrDefault();
                if (wsSex != null)
                    await Given_Sex(wsSex);
                ExcelWorksheet wsOrganization = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Organization)).FirstOrDefault();
                if (wsOrganization != null)
                    await Given_Organization(wsOrganization);
                ExcelWorksheet wsProvince = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Province)).FirstOrDefault();
                if (wsProvince != null)
                    await Given_Province(wsProvince);
                ExcelWorksheet wsDistrict = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(District)).FirstOrDefault();
                if (wsDistrict != null)
                    await Given_District(wsDistrict);
                ExcelWorksheet wsAppUser = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(AppUser)).FirstOrDefault();
                if (wsAppUser != null)
                    await Given_AppUser(wsAppUser);
                ExcelWorksheet wsFieldType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(FieldType)).FirstOrDefault();
                if (wsFieldType != null)
                    await Given_FieldType(wsFieldType);
                ExcelWorksheet wsRole = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Role)).FirstOrDefault();
                if (wsRole != null)
                    await Given_Role(wsRole);
                ExcelWorksheet wsMenu = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Menu)).FirstOrDefault();
                if (wsMenu != null)
                    await Given_Menu(wsMenu);
                ExcelWorksheet wsField = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Field)).FirstOrDefault();
                if (wsField != null)
                    await Given_Field(wsField);
                ExcelWorksheet wsAction = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Entities.Action)).FirstOrDefault();
                if (wsAction != null)
                    await Given_Action(wsAction);
                ExcelWorksheet wsPage = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Page)).FirstOrDefault();
                if (wsPage != null)
                    await Given_Page(wsPage);
                ExcelWorksheet wsPermission = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Permission)).FirstOrDefault();
                if (wsPermission != null)
                    await Given_Permission(wsPermission);
                ExcelWorksheet wsPermissonOperator = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(PermissonOperator)).FirstOrDefault();
                if (wsPermissonOperator != null)
                    await Given_PermissionOperator(wsPermissonOperator);
                ExcelWorksheet wsAppUserRoleMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(AppUserRoleMapping)).FirstOrDefault();
                if (wsAppUserRoleMapping != null)
                    await Given_AppUserRoleMapping(wsAppUserRoleMapping);
                ExcelWorksheet wsPermissonContent = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(PermissonContent)).FirstOrDefault();
                if (wsPermissonContent != null)
                    await Given_PermissionContent(wsPermissonContent);
                ExcelWorksheet wsActionPageMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(ActionPageMapping)).FirstOrDefault();
                if (wsActionPageMapping != null)
                    await Given_ActionPageMapping(wsActionPageMapping);
                ExcelWorksheet wsPermissionActionMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(PermissionActionMapping)).FirstOrDefault();
                if (wsPermissionActionMapping != null)
                    await Given_PermissionActionMapping(wsPermissionActionMapping);
                #endregion
                #region Store
                ExcelWorksheet wsEstimatedRevenue = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(EstimatedRevenue)).FirstOrDefault();
                if (wsEstimatedRevenue != null)
                    await Given_EstimatedRevenue(wsEstimatedRevenue);
                ExcelWorksheet wsWard = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Ward)).FirstOrDefault();
                if (wsWard != null)
                    await Given_Ward(wsWard);
                ExcelWorksheet wsColor = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Color)).FirstOrDefault();
                if (wsColor != null)
                    await Given_Color(wsColor);
                ExcelWorksheet wsStoreType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreType)).FirstOrDefault();
                if (wsStoreType != null)
                    await Given_StoreType(wsStoreType);
                ExcelWorksheet wsStoreScoutingStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreScoutingStatus)).FirstOrDefault();
                if (wsStoreScoutingStatus != null)
                    await Given_StoreScoutingStatus(wsStoreScoutingStatus);
                ExcelWorksheet wsStoreScoutingType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreScoutingType)).FirstOrDefault();
                if (wsStoreScoutingType != null)
                    await Given_StoreScoutingType(wsStoreScoutingType);
                ExcelWorksheet wsStoreScouting = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreScouting)).FirstOrDefault();
                if (wsStoreScouting != null)
                    await Given_StoreScouting(wsStoreScouting);
                ExcelWorksheet wsStore = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Store)).FirstOrDefault();
                if (wsStore != null)
                    await Given_Store(wsStore);
                ExcelWorksheet wsStoreGrouping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreGrouping)).FirstOrDefault();
                if (wsStoreGrouping != null)
                    await Given_StoreGrouping(wsStoreGrouping);
                ExcelWorksheet wsStoreStoreGroupingMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreStoreGroupingMapping)).FirstOrDefault();
                if (wsStoreStoreGroupingMapping != null)
                    await Given_StoreStoreGroupingMapping(wsStoreStoreGroupingMapping);
                ExcelWorksheet wsStoreUser = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreUser)).FirstOrDefault();
                if (wsStoreUser != null)
                    await Given_StoreUser(wsStoreUser);
                ExcelWorksheet wsStoreBalance = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreBalance)).FirstOrDefault();
                if (wsStoreBalance != null)
                    await Given_StoreBalance(wsStoreBalance);
                ExcelWorksheet wsAppUserStoreMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(AppUserStoreMapping)).FirstOrDefault();
                if (wsAppUserStoreMapping != null)
                    await Given_AppUserStoreMapping(wsAppUserStoreMapping);

                #endregion Store
                #region Direct sales order
                ExcelWorksheet wsCheckState = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(CheckState)).FirstOrDefault();
                if (wsCheckState != null)
                    await Given_CheckState(wsCheckState);
                ExcelWorksheet wsDirectSalesOrderSourceType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(DirectSalesOrderSourceType)).FirstOrDefault();
                if (wsDirectSalesOrderSourceType != null)
                    await Given_DirectSalesOrderSourceType(wsDirectSalesOrderSourceType);
                ExcelWorksheet wsEditedPriceStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(EditedPriceStatus)).FirstOrDefault();
                if (wsEditedPriceStatus != null)
                    await Given_EditedPriceStatus(wsEditedPriceStatus);
                ExcelWorksheet wsErpApprovalState = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(ErpApprovalState)).FirstOrDefault();
                if (wsErpApprovalState != null)
                    await Given_ErpApprovalState(wsErpApprovalState);
                ExcelWorksheet wsGeneralApprovalState = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(GeneralApprovalState)).FirstOrDefault();
                if (wsGeneralApprovalState != null)
                    await Given_GeneralApprovalState(wsGeneralApprovalState);
                ExcelWorksheet wsRequestState = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(RequestState)).FirstOrDefault();
                if (wsRequestState != null)
                    await Given_RequestState(wsRequestState);
                ExcelWorksheet wsStoreApprovalState = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreApprovalState)).FirstOrDefault();
                if (wsStoreApprovalState != null)
                    await Given_StoreApprovalState(wsStoreApprovalState);
                ExcelWorksheet wsTransactionType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(TransactionType)).FirstOrDefault();
                if (wsTransactionType != null)
                    await Given_TransactionType(wsTransactionType);
                ExcelWorksheet wsStoreChecking = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreChecking)).FirstOrDefault();
                if (wsStoreChecking != null)
                    await Given_StoreChecking(wsStoreChecking);
                ExcelWorksheet wsImage = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Image)).FirstOrDefault();
                if (wsImage != null)
                    await Given_Image(wsImage);
                ExcelWorksheet wsAlbum = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Album)).FirstOrDefault();
                if (wsAlbum != null)
                    await Given_Album(wsAlbum);
                ExcelWorksheet wsStoreCheckingImageMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreCheckingImageMapping)).FirstOrDefault();
                if (wsStoreCheckingImageMapping != null)
                    await Given_StoreCheckingImageMapping(wsStoreCheckingImageMapping);
                ExcelWorksheet wsAlbumImageMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(AlbumImageMapping)).FirstOrDefault();
                if (wsAlbumImageMapping != null)
                    await Given_AlbumImageMapping(wsAlbumImageMapping);
                ExcelWorksheet wsBrand = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Brand)).FirstOrDefault();
                if (wsBrand != null)
                    await Given_Brand(wsBrand);
                ExcelWorksheet wsCategory = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Category)).FirstOrDefault();
                if (wsCategory != null)
                    await Given_Category(wsCategory);
                ExcelWorksheet wsProductType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(ProductType)).FirstOrDefault();
                if (wsProductType != null)
                    await Given_ProductType(wsProductType);
                ExcelWorksheet wsTaxType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(TaxType)).FirstOrDefault();
                if (wsTaxType != null)
                    await Given_TaxType(wsTaxType);
                ExcelWorksheet wsUnitOfMeasure = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(UnitOfMeasure)).FirstOrDefault();
                if (wsUnitOfMeasure != null)
                    await Given_UnitOfMeasure(wsUnitOfMeasure);
                ExcelWorksheet wsUsedVariation = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(UsedVariation)).FirstOrDefault();
                if (wsUsedVariation != null)
                    await Given_UsedVariation(wsUsedVariation);
                ExcelWorksheet wsProductGrouping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(ProductGrouping)).FirstOrDefault();
                if (wsProductGrouping != null)
                    await Given_ProductGrouping(wsProductGrouping);
                ExcelWorksheet wsUnitOfMeasureGrouping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(UnitOfMeasureGrouping)).FirstOrDefault();
                if (wsUnitOfMeasureGrouping != null)
                    await Given_UnitOfMeasureGrouping(wsUnitOfMeasureGrouping);
                ExcelWorksheet wsProduct = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Product)).FirstOrDefault();
                if (wsProduct != null)
                    await Given_Product(wsProduct);
                ExcelWorksheet wsVariationGrouping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(VariationGrouping)).FirstOrDefault();
                if (wsVariationGrouping != null)
                    await Given_VariationGrouping(wsVariationGrouping);
                ExcelWorksheet wsVariation = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Variation)).FirstOrDefault();
                if (wsVariation != null)
                    await Given_Variation(wsVariation);
                ExcelWorksheet wsItem = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Item)).FirstOrDefault();
                if (wsItem != null)
                    await Given_Item(wsItem);
                ExcelWorksheet wsProductProductGroupingMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(ProductProductGroupingMapping)).FirstOrDefault();
                if (wsProductProductGroupingMapping != null)
                    await Given_ProductProductGroupingMapping(wsProductProductGroupingMapping);
                ExcelWorksheet wsDirectSalesOrder = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(DirectSalesOrder)).FirstOrDefault();
                if (wsDirectSalesOrder != null)
                    await Given_DirectSalesOrder(wsDirectSalesOrder);
                ExcelWorksheet wsDirectSalesOrderContent = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(DirectSalesOrderContent)).FirstOrDefault();
                if (wsDirectSalesOrderContent != null)
                    await Given_DirectSalesOrderContent(wsDirectSalesOrderContent);
                ExcelWorksheet wsDirectSalesOrderPromotion = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(DirectSalesOrderPromotion)).FirstOrDefault();
                if (wsDirectSalesOrderPromotion != null)
                    await Given_DirectSalesOrderPromotion(wsDirectSalesOrderPromotion);
                ExcelWorksheet wsDirectSalesOrderTransaction = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(DirectSalesOrderTransaction)).FirstOrDefault();
                if (wsDirectSalesOrderTransaction != null)
                    await Given_DirectSalesOrderTransaction(wsDirectSalesOrderTransaction);
                ExcelWorksheet wsWarehouse = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Warehouse)).FirstOrDefault();
                if (wsWarehouse != null)
                    await Given_Warehouse(wsWarehouse);
                ExcelWorksheet wsWarehouseOrganizationMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(WarehouseOrganizationMapping)).FirstOrDefault();
                if (wsWarehouseOrganizationMapping != null)
                    await Given_WarehouseOrganizationMapping(wsWarehouseOrganizationMapping);
                ExcelWorksheet wsInventory = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Inventory)).FirstOrDefault();
                if (wsInventory != null)
                    await Given_Inventory(wsInventory);
                ExcelWorksheet wsBrandInStore = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(BrandInStore)).FirstOrDefault();
                if (wsBrandInStore != null)
                    await Given_BrandInStore(wsBrandInStore);
                ExcelWorksheet wsBrandInStoreHistory = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(BrandInStoreHistory)).FirstOrDefault();
                if (wsBrandInStoreHistory != null)
                    await Given_BrandInStoreHistory(wsBrandInStoreHistory);
                #endregion
                #region Indirect sales order
                ExcelWorksheet wsIndirectSalesOrder = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(IndirectSalesOrder)).FirstOrDefault();
                if (wsIndirectSalesOrder != null)
                    await Given_IndirectSalesOrder(wsIndirectSalesOrder);
                ExcelWorksheet wsIndirectSalesOrderContent = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(IndirectSalesOrderContent)).FirstOrDefault();
                if (wsIndirectSalesOrderContent != null)
                    await Given_IndirectSalesOrderContent(wsIndirectSalesOrderContent);
                ExcelWorksheet wsIndirectSalesOrderPromotion = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(IndirectSalesOrderPromotion)).FirstOrDefault();
                if (wsIndirectSalesOrderPromotion != null)
                    await Given_IndirectSalesOrderPromotion(wsIndirectSalesOrderPromotion);
                ExcelWorksheet wsIndirectSalesOrderTransaction = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(IndirectSalesOrderTransaction)).FirstOrDefault();
                if (wsIndirectSalesOrderTransaction != null)
                    await Given_IndirectSalesOrderTransaction(wsIndirectSalesOrderTransaction);
                #endregion
               
                #region Kpi General
                ExcelWorksheet wsKpiYear = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiYear)).FirstOrDefault();
                if (wsKpiYear != null)
                    await Given_KpiYear(wsKpiYear);
                ExcelWorksheet wsKpiGeneral = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiGeneral)).FirstOrDefault();
                if (wsKpiGeneral != null)
                    await Given_KpiGeneral(wsKpiGeneral);
                ExcelWorksheet wsKpiCriteriaGeneral = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiCriteriaGeneral)).FirstOrDefault();
                if (wsKpiCriteriaGeneral != null)
                    await Given_KpiCriteriaGeneral(wsKpiCriteriaGeneral);
                ExcelWorksheet wsKpiGeneralContent = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiGeneralContent)).FirstOrDefault();
                if (wsKpiGeneralContent != null)
                    await Given_KpiGeneralContent(wsKpiGeneralContent);
                #endregion Kpi General
                #region KPI Item
                ExcelWorksheet wsKpiItemType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiItemType)).FirstOrDefault();
                if (wsKpiItemType != null)
                    await Given_KpiItemType(wsKpiItemType);
                ExcelWorksheet wsKpiPeriod = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiPeriod)).FirstOrDefault();
                if (wsKpiPeriod != null)
                    await Given_KpiPeriod(wsKpiPeriod);
                ExcelWorksheet wsKpiCriteriaItem = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiCriteriaItem)).FirstOrDefault();
                if (wsKpiCriteriaItem != null)
                    await Given_KpiCriteriaItem(wsKpiCriteriaItem);
                ExcelWorksheet wsKpiItem = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiItem)).FirstOrDefault();
                if (wsKpiItem != null)
                    await Given_KpiItem(wsKpiItem);
                ExcelWorksheet wsKpiItemContent = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiItemContent)).FirstOrDefault();
                if (wsKpiItemContent != null)
                    await Given_KpiItemContent(wsKpiItemContent);
                ExcelWorksheet wsKpiItemContentKpiCriteriaItemMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "KICKCIM").FirstOrDefault();
                if (wsKpiItemContentKpiCriteriaItemMapping != null)
                    await Given_KpiItemContentKpiCriteriaItemMapping(wsKpiItemContentKpiCriteriaItemMapping);
                #endregion
                #region KPI Product grouping
                ExcelWorksheet wsKpiProductGroupingType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiProductGroupingType)).FirstOrDefault();
                if (wsKpiProductGroupingType != null)
                    await Given_KpiProductGroupingType(wsKpiProductGroupingType);
                ExcelWorksheet wsKpiProductGroupingCriteria = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiProductGroupingCriteria)).FirstOrDefault();
                if (wsKpiProductGroupingCriteria != null)
                    await Given_KpiProductGroupingCriteria(wsKpiProductGroupingCriteria);
                ExcelWorksheet wsKpiProductGrouping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiProductGrouping)).FirstOrDefault();
                if (wsKpiProductGrouping != null)
                    await Given_KpiProductGrouping(wsKpiProductGrouping);
                ExcelWorksheet wsKpiProductGroupingContent = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(KpiProductGroupingContent)).FirstOrDefault();
                if (wsKpiProductGroupingContent != null)
                    await Given_KpiProductGroupingContent(wsKpiProductGroupingContent);
                ExcelWorksheet wsKpiProductGroupingContentItemMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "KPGCIM").FirstOrDefault();
                if (wsKpiProductGroupingContentItemMapping != null)
                    await Given_KpiProductGroupingContentItemMapping(wsKpiProductGroupingContentItemMapping);
                ExcelWorksheet wsKpiGeneralContentKpiPeriodMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "KGCKPM").FirstOrDefault();
                if (wsKpiGeneralContentKpiPeriodMapping != null)
                    await Given_KpiGeneralContentKpiPeriodMapping(wsKpiGeneralContentKpiPeriodMapping);
                #endregion

                #region Problem
                ExcelWorksheet wsProblemType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(ProblemType)).FirstOrDefault();
                if (wsProblemType != null)
                    await Given_ProblemType(wsProblemType);
                ExcelWorksheet wsProblem = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Problem)).FirstOrDefault();
                if (wsProblem != null)
                    await Given_Problem(wsProblem);
                #endregion Problem
                #region Lucky Draw
                //ExcelWorksheet wsLuckyDraw = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDraw)).FirstOrDefault();
                //if (wsLuckyDraw != null)
                //    await Given_LuckyDraw(wsLuckyDraw);
                //ExcelWorksheet wsLuckyDrawStoreGroupingMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawStoreGroupingMapping)).FirstOrDefault();
                //if (wsLuckyDrawStoreGroupingMapping != null)
                //    await Given_LuckyDrawStoreGroupingMapping(wsLuckyDrawStoreGroupingMapping);
                //ExcelWorksheet wsLuckyDrawStoreMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawStoreMapping)).FirstOrDefault();
                //if (wsLuckyDrawStoreMapping != null)
                //    await Given_LuckyDrawStoreMapping(wsLuckyDrawStoreMapping);
                //ExcelWorksheet wsLuckyDrawStoreTypeMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawStoreTypeMapping)).FirstOrDefault();
                //if (wsLuckyDrawStoreTypeMapping != null)
                //    await Given_LuckyDrawStoreTypeMapping(wsLuckyDrawStoreTypeMapping);
                //ExcelWorksheet wsLuckyDrawRegistration = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawRegistration)).FirstOrDefault();
                //if (wsLuckyDrawRegistration != null)
                //    await Given_LuckyDrawRegistration(wsLuckyDrawRegistration);
                //ExcelWorksheet wsLuckyDrawStructure = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawStructure)).FirstOrDefault();
                //if (wsLuckyDrawStructure != null)
                //    await Given_LuckyDrawStructure(wsLuckyDrawStructure);
                //ExcelWorksheet wsLuckyDrawWinner = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(LuckyDrawWinner)).FirstOrDefault();
                //if (wsLuckyDrawWinner != null)
                //    await Given_LuckyDrawWinner(wsLuckyDrawWinner);
                #endregion
            }

            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "data source=localhost;initial catalog=master;persist security info=True;Trusted_Connection=True;multipleactiveresultsets=True;";
                conn.Open();

                SqlCommand cmd = new SqlCommand($@"use master; CREATE DATABASE {SnapshotName} ON  (NAME = {DatabaseName}, FILENAME =
                'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\Data\{SnapshotName}')
                AS SNAPSHOT OF {DatabaseName};", conn);

                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        #region Given Methods
        protected async Task Given_Album(ExcelWorksheet ExcelWorksheet)
        {
            this.AlbumDAOs = new List<AlbumDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                AlbumDAO AlbumDAO = new AlbumDAO();
                AlbumDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                AlbumDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                AlbumDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                AlbumDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                AlbumDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                AlbumDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                AlbumDAOs.Add(AlbumDAO);
            }
            await DataContext.Album.BulkMergeAsync(AlbumDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_AlbumImageMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.AlbumImageMappingDAOs = new List<AlbumImageMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            int AlbumIdColumn = StartColumn + columns.IndexOf("AlbumId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int ShootingAtColumn = StartColumn + columns.IndexOf("ShootingAt");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int SaleEmployeeIdColumn = StartColumn + columns.IndexOf("SaleEmployeeId");
            int DistanceColumn = StartColumn + columns.IndexOf("Distance");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                AlbumImageMappingDAO AlbumImageMappingDAO = new AlbumImageMappingDAO();
                AlbumImageMappingDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseLong() ?? 0;
                AlbumImageMappingDAO.AlbumId = ExcelWorksheet.Cells[row, AlbumIdColumn].Value?.ParseLong() ?? 0;
                AlbumImageMappingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                AlbumImageMappingDAO.ShootingAt = ExcelWorksheet.Cells[row, ShootingAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                AlbumImageMappingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                AlbumImageMappingDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseNullLong();
                AlbumImageMappingDAO.Distance = ExcelWorksheet.Cells[row, DistanceColumn].Value?.ParseNullLong();
                AlbumImageMappingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                AlbumImageMappingDAOs.Add(AlbumImageMappingDAO);
            }
            await DataContext.AlbumImageMapping.BulkMergeAsync(AlbumImageMappingDAOs);
        }
        protected async Task Given_AppUser(ExcelWorksheet ExcelWorksheet)
        {
            this.AppUserDAOs = new List<AppUserDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int UsernameColumn = StartColumn + columns.IndexOf("Username");
            int DisplayNameColumn = StartColumn + columns.IndexOf("DisplayName");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int EmailColumn = StartColumn + columns.IndexOf("Email");
            int PhoneColumn = StartColumn + columns.IndexOf("Phone");
            int SexIdColumn = StartColumn + columns.IndexOf("SexId");
            int BirthdayColumn = StartColumn + columns.IndexOf("Birthday");
            int AvatarColumn = StartColumn + columns.IndexOf("Avatar");
            int PositionIdColumn = StartColumn + columns.IndexOf("PositionId");
            int DepartmentColumn = StartColumn + columns.IndexOf("Department");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                AppUserDAO AppUserDAO = new AppUserDAO();
                AppUserDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                AppUserDAO.Username = ExcelWorksheet.Cells[row, UsernameColumn].Value?.ParseString();
                AppUserDAO.DisplayName = ExcelWorksheet.Cells[row, DisplayNameColumn].Value?.ParseString();
                AppUserDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                AppUserDAO.Email = ExcelWorksheet.Cells[row, EmailColumn].Value?.ParseString();
                AppUserDAO.Phone = ExcelWorksheet.Cells[row, PhoneColumn].Value?.ParseString();
                AppUserDAO.SexId = ExcelWorksheet.Cells[row, SexIdColumn].Value?.ParseLong() ?? 0;
                AppUserDAO.Birthday = ExcelWorksheet.Cells[row, BirthdayColumn].Value?.ParseNullDateTime();
                AppUserDAO.Avatar = ExcelWorksheet.Cells[row, AvatarColumn].Value?.ParseString();
                AppUserDAO.PositionId = ExcelWorksheet.Cells[row, PositionIdColumn].Value?.ParseNullLong();
                AppUserDAO.Department = ExcelWorksheet.Cells[row, DepartmentColumn].Value?.ParseString();
                AppUserDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                AppUserDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                AppUserDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                AppUserDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                AppUserDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                AppUserDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                AppUserDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                AppUserDAOs.Add(AppUserDAO);
            }
            await DataContext.AppUser.BulkMergeAsync(AppUserDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_AppUserGps(ExcelWorksheet ExcelWorksheet)
        {
            this.AppUserGpsDAOs = new List<AppUserGpsDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int LatitudeColumn = StartColumn + columns.IndexOf("Latitude");
            int LongitudeColumn = StartColumn + columns.IndexOf("Longitude");
            int GPSUpdatedAtColumn = StartColumn + columns.IndexOf("GPSUpdatedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                AppUserGpsDAO AppUserGpsDAO = new AppUserGpsDAO();
                AppUserGpsDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                AppUserGpsDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                AppUserGpsDAO.Latitude = ExcelWorksheet.Cells[row, LatitudeColumn].Value?.ParseNullDecimal();
                AppUserGpsDAO.Longitude = ExcelWorksheet.Cells[row, LongitudeColumn].Value?.ParseNullDecimal();
                AppUserGpsDAO.GPSUpdatedAt = ExcelWorksheet.Cells[row, GPSUpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                AppUserGpsDAOs.Add(AppUserGpsDAO);
            }
            await DataContext.AppUserGps.BulkMergeAsync(AppUserGpsDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_AppUserRoleMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.AppUserRoleMappingDAOs = new List<AppUserRoleMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int RoleIdColumn = StartColumn + columns.IndexOf("RoleId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                AppUserRoleMappingDAO AppUserRoleMappingDAO = new AppUserRoleMappingDAO();
                AppUserRoleMappingDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                AppUserRoleMappingDAO.RoleId = ExcelWorksheet.Cells[row, RoleIdColumn].Value?.ParseLong() ?? 0;
                AppUserRoleMappingDAOs.Add(AppUserRoleMappingDAO);
            }
            await DataContext.AppUserRoleMapping.BulkMergeAsync(AppUserRoleMappingDAOs);
        }
        protected async Task Given_AppUserStoreMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.AppUserStoreMappingDAOs = new List<AppUserStoreMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                AppUserStoreMappingDAO AppUserStoreMappingDAO = new AppUserStoreMappingDAO();
                AppUserStoreMappingDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                AppUserStoreMappingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                AppUserStoreMappingDAOs.Add(AppUserStoreMappingDAO);
            }
            await DataContext.AppUserStoreMapping.BulkMergeAsync(AppUserStoreMappingDAOs);
        }
        protected async Task Given_Banner(ExcelWorksheet ExcelWorksheet)
        {
            this.BannerDAOs = new List<BannerDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int TitleColumn = StartColumn + columns.IndexOf("Title");
            int PriorityColumn = StartColumn + columns.IndexOf("Priority");
            int ContentColumn = StartColumn + columns.IndexOf("Content");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                BannerDAO BannerDAO = new BannerDAO();
                BannerDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                BannerDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                BannerDAO.Title = ExcelWorksheet.Cells[row, TitleColumn].Value?.ParseString();
                BannerDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseNullLong();
                BannerDAO.Content = ExcelWorksheet.Cells[row, ContentColumn].Value?.ParseString();
                BannerDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                BannerDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                BannerDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseNullLong();
                BannerDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                BannerDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                BannerDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                BannerDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                BannerDAOs.Add(BannerDAO);
            }
            await DataContext.Banner.BulkMergeAsync(BannerDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Brand(ExcelWorksheet ExcelWorksheet)
        {
            this.BrandDAOs = new List<BrandDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                BrandDAO BrandDAO = new BrandDAO();
                BrandDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                BrandDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                BrandDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                BrandDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                BrandDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                BrandDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                BrandDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                BrandDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                BrandDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                BrandDAOs.Add(BrandDAO);
            }
            await DataContext.Brand.BulkMergeAsync(BrandDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_BrandHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.BrandHistoryDAOs = new List<BrandHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int BrandIdColumn = StartColumn + columns.IndexOf("BrandId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                BrandHistoryDAO BrandHistoryDAO = new BrandHistoryDAO();
                BrandHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                BrandHistoryDAO.BrandId = ExcelWorksheet.Cells[row, BrandIdColumn].Value?.ParseLong() ?? 0;
                BrandHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseNullLong();
                BrandHistoryDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                BrandHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                BrandHistoryDAOs.Add(BrandHistoryDAO);
            }
            await DataContext.BrandHistory.BulkMergeAsync(BrandHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_BrandInStore(ExcelWorksheet ExcelWorksheet)
        {
            this.BrandInStoreDAOs = new List<BrandInStoreDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int BrandIdColumn = StartColumn + columns.IndexOf("BrandId");
            int TopColumn = StartColumn + columns.IndexOf("Top");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                BrandInStoreDAO BrandInStoreDAO = new BrandInStoreDAO();
                BrandInStoreDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreDAO.BrandId = ExcelWorksheet.Cells[row, BrandIdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreDAO.Top = ExcelWorksheet.Cells[row, TopColumn].Value?.ParseLong() ?? 0;
                BrandInStoreDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                BrandInStoreDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                BrandInStoreDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                BrandInStoreDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                BrandInStoreDAOs.Add(BrandInStoreDAO);
            }
            await DataContext.BrandInStore.BulkMergeAsync(BrandInStoreDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_BrandInStoreHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.BrandInStoreHistoryDAOs = new List<BrandInStoreHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int BrandInStoreIdColumn = StartColumn + columns.IndexOf("BrandInStoreId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int BrandIdColumn = StartColumn + columns.IndexOf("BrandId");
            int TopColumn = StartColumn + columns.IndexOf("Top");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                BrandInStoreHistoryDAO BrandInStoreHistoryDAO = new BrandInStoreHistoryDAO();
                BrandInStoreHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreHistoryDAO.BrandInStoreId = ExcelWorksheet.Cells[row, BrandInStoreIdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreHistoryDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreHistoryDAO.BrandId = ExcelWorksheet.Cells[row, BrandIdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreHistoryDAO.Top = ExcelWorksheet.Cells[row, TopColumn].Value?.ParseLong() ?? 0;
                BrandInStoreHistoryDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                BrandInStoreHistoryDAOs.Add(BrandInStoreHistoryDAO);
            }
            await DataContext.BrandInStoreHistory.BulkMergeAsync(BrandInStoreHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_BrandInStoreProductGroupingMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.BrandInStoreProductGroupingMappingDAOs = new List<BrandInStoreProductGroupingMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int BrandInStoreIdColumn = StartColumn + columns.IndexOf("BrandInStoreId");
            int ProductGroupingIdColumn = StartColumn + columns.IndexOf("ProductGroupingId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                BrandInStoreProductGroupingMappingDAO BrandInStoreProductGroupingMappingDAO = new BrandInStoreProductGroupingMappingDAO();
                BrandInStoreProductGroupingMappingDAO.BrandInStoreId = ExcelWorksheet.Cells[row, BrandInStoreIdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreProductGroupingMappingDAO.ProductGroupingId = ExcelWorksheet.Cells[row, ProductGroupingIdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreProductGroupingMappingDAOs.Add(BrandInStoreProductGroupingMappingDAO);
            }
            await DataContext.BrandInStoreProductGroupingMapping.BulkMergeAsync(BrandInStoreProductGroupingMappingDAOs);
        }
        protected async Task Given_BrandInStoreShowingCategoryMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.BrandInStoreShowingCategoryMappingDAOs = new List<BrandInStoreShowingCategoryMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int BrandInStoreIdColumn = StartColumn + columns.IndexOf("BrandInStoreId");
            int ShowingCategoryIdColumn = StartColumn + columns.IndexOf("ShowingCategoryId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                BrandInStoreShowingCategoryMappingDAO BrandInStoreShowingCategoryMappingDAO = new BrandInStoreShowingCategoryMappingDAO();
                BrandInStoreShowingCategoryMappingDAO.BrandInStoreId = ExcelWorksheet.Cells[row, BrandInStoreIdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreShowingCategoryMappingDAO.ShowingCategoryId = ExcelWorksheet.Cells[row, ShowingCategoryIdColumn].Value?.ParseLong() ?? 0;
                BrandInStoreShowingCategoryMappingDAOs.Add(BrandInStoreShowingCategoryMappingDAO);
            }
            await DataContext.BrandInStoreShowingCategoryMapping.BulkMergeAsync(BrandInStoreShowingCategoryMappingDAOs);
        }
        protected async Task Given_Category(ExcelWorksheet ExcelWorksheet)
        {
            this.CategoryDAOs = new List<CategoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ParentIdColumn = StartColumn + columns.IndexOf("ParentId");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int LevelColumn = StartColumn + columns.IndexOf("Level");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int OrderNumberColumn = StartColumn + columns.IndexOf("OrderNumber");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                CategoryDAO CategoryDAO = new CategoryDAO();
                CategoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                CategoryDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                CategoryDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                CategoryDAO.ParentId = ExcelWorksheet.Cells[row, ParentIdColumn].Value?.ParseNullLong();
                CategoryDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                CategoryDAO.Level = ExcelWorksheet.Cells[row, LevelColumn].Value?.ParseLong() ?? 0;
                CategoryDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                CategoryDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseNullLong();
                CategoryDAO.OrderNumber = ExcelWorksheet.Cells[row, OrderNumberColumn].Value?.ParseLong() ?? 0;
                CategoryDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                CategoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                CategoryDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                CategoryDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                CategoryDAOs.Add(CategoryDAO);
            }
            await DataContext.Category.BulkMergeAsync(CategoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_CheckState(ExcelWorksheet ExcelWorksheet)
        {
            this.CheckStateDAOs = new List<CheckStateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                CheckStateDAO CheckStateDAO = new CheckStateDAO();
                CheckStateDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                CheckStateDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                CheckStateDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                CheckStateDAOs.Add(CheckStateDAO);
            }
            await DataContext.CheckState.BulkMergeAsync(CheckStateDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_CodeGeneratorRule(ExcelWorksheet ExcelWorksheet)
        {
            this.CodeGeneratorRuleDAOs = new List<CodeGeneratorRuleDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int EntityTypeIdColumn = StartColumn + columns.IndexOf("EntityTypeId");
            int AutoNumberLenthColumn = StartColumn + columns.IndexOf("AutoNumberLenth");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                CodeGeneratorRuleDAO CodeGeneratorRuleDAO = new CodeGeneratorRuleDAO();
                CodeGeneratorRuleDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                CodeGeneratorRuleDAO.EntityTypeId = ExcelWorksheet.Cells[row, EntityTypeIdColumn].Value?.ParseLong() ?? 0;
                CodeGeneratorRuleDAO.AutoNumberLenth = ExcelWorksheet.Cells[row, AutoNumberLenthColumn].Value?.ParseLong() ?? 0;
                CodeGeneratorRuleDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                CodeGeneratorRuleDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                CodeGeneratorRuleDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                CodeGeneratorRuleDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                CodeGeneratorRuleDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                CodeGeneratorRuleDAOs.Add(CodeGeneratorRuleDAO);
            }
            await DataContext.CodeGeneratorRule.BulkMergeAsync(CodeGeneratorRuleDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_CodeGeneratorRuleEntityComponentMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.CodeGeneratorRuleEntityComponentMappingDAOs = new List<CodeGeneratorRuleEntityComponentMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int CodeGeneratorRuleIdColumn = StartColumn + columns.IndexOf("CodeGeneratorRuleId");
            int EntityComponentIdColumn = StartColumn + columns.IndexOf("EntityComponentId");
            int SequenceColumn = StartColumn + columns.IndexOf("Sequence");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                CodeGeneratorRuleEntityComponentMappingDAO CodeGeneratorRuleEntityComponentMappingDAO = new CodeGeneratorRuleEntityComponentMappingDAO();
                CodeGeneratorRuleEntityComponentMappingDAO.CodeGeneratorRuleId = ExcelWorksheet.Cells[row, CodeGeneratorRuleIdColumn].Value?.ParseLong() ?? 0;
                CodeGeneratorRuleEntityComponentMappingDAO.EntityComponentId = ExcelWorksheet.Cells[row, EntityComponentIdColumn].Value?.ParseLong() ?? 0;
                CodeGeneratorRuleEntityComponentMappingDAO.Sequence = ExcelWorksheet.Cells[row, SequenceColumn].Value?.ParseLong() ?? 0;
                CodeGeneratorRuleEntityComponentMappingDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseString();
                CodeGeneratorRuleEntityComponentMappingDAOs.Add(CodeGeneratorRuleEntityComponentMappingDAO);
            }
            await DataContext.CodeGeneratorRuleEntityComponentMapping.BulkMergeAsync(CodeGeneratorRuleEntityComponentMappingDAOs);
        }
        protected async Task Given_Color(ExcelWorksheet ExcelWorksheet)
        {
            this.ColorDAOs = new List<ColorDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ColorDAO ColorDAO = new ColorDAO();
                ColorDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ColorDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ColorDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ColorDAOs.Add(ColorDAO);
            }
            await DataContext.Color.BulkMergeAsync(ColorDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ConversationAttachmentType(ExcelWorksheet ExcelWorksheet)
        {
            this.ConversationAttachmentTypeDAOs = new List<AttachmentTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                AttachmentTypeDAO ConversationAttachmentTypeDAO = new AttachmentTypeDAO();
                ConversationAttachmentTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ConversationAttachmentTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ConversationAttachmentTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ConversationAttachmentTypeDAOs.Add(ConversationAttachmentTypeDAO);
            }
            await DataContext.AttachmentType.BulkMergeAsync(ConversationAttachmentTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ConversationType(ExcelWorksheet ExcelWorksheet)
        {
            this.ConversationTypeDAOs = new List<ConversationTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ConversationTypeDAO ConversationTypeDAO = new ConversationTypeDAO();
                ConversationTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ConversationTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ConversationTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ConversationTypeDAOs.Add(ConversationTypeDAO);
            }
            await DataContext.ConversationType.BulkMergeAsync(ConversationTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_DirectSalesOrderContent(ExcelWorksheet ExcelWorksheet)
        {
            this.DirectSalesOrderContentDAOs = new List<DirectSalesOrderContentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int DirectSalesOrderIdColumn = StartColumn + columns.IndexOf("DirectSalesOrderId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            int PrimaryUnitOfMeasureIdColumn = StartColumn + columns.IndexOf("PrimaryUnitOfMeasureId");
            int RequestedQuantityColumn = StartColumn + columns.IndexOf("RequestedQuantity");
            int PrimaryPriceColumn = StartColumn + columns.IndexOf("PrimaryPrice");
            int SalePriceColumn = StartColumn + columns.IndexOf("SalePrice");
            int EditedPriceStatusIdColumn = StartColumn + columns.IndexOf("EditedPriceStatusId");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int DiscountAmountColumn = StartColumn + columns.IndexOf("DiscountAmount");
            int GeneralDiscountPercentageColumn = StartColumn + columns.IndexOf("GeneralDiscountPercentage");
            int GeneralDiscountAmountColumn = StartColumn + columns.IndexOf("GeneralDiscountAmount");
            int TaxPercentageColumn = StartColumn + columns.IndexOf("TaxPercentage");
            int TaxAmountColumn = StartColumn + columns.IndexOf("TaxAmount");
            int AmountColumn = StartColumn + columns.IndexOf("Amount");
            int FactorColumn = StartColumn + columns.IndexOf("Factor");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                DirectSalesOrderContentDAO DirectSalesOrderContentDAO = new DirectSalesOrderContentDAO();
                DirectSalesOrderContentDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderContentDAO.DirectSalesOrderId = ExcelWorksheet.Cells[row, DirectSalesOrderIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderContentDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderContentDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderContentDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderContentDAO.PrimaryUnitOfMeasureId = ExcelWorksheet.Cells[row, PrimaryUnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderContentDAO.RequestedQuantity = ExcelWorksheet.Cells[row, RequestedQuantityColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderContentDAO.PrimaryPrice = ExcelWorksheet.Cells[row, PrimaryPriceColumn].Value?.ParseDecimal() ?? 0;
                DirectSalesOrderContentDAO.SalePrice = ExcelWorksheet.Cells[row, SalePriceColumn].Value?.ParseDecimal() ?? 0;
                DirectSalesOrderContentDAO.EditedPriceStatusId = ExcelWorksheet.Cells[row, EditedPriceStatusIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderContentDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                DirectSalesOrderContentDAO.DiscountAmount = ExcelWorksheet.Cells[row, DiscountAmountColumn].Value?.ParseNullDecimal();
                DirectSalesOrderContentDAO.GeneralDiscountPercentage = ExcelWorksheet.Cells[row, GeneralDiscountPercentageColumn].Value?.ParseNullDecimal();
                DirectSalesOrderContentDAO.GeneralDiscountAmount = ExcelWorksheet.Cells[row, GeneralDiscountAmountColumn].Value?.ParseNullDecimal();
                DirectSalesOrderContentDAO.TaxPercentage = ExcelWorksheet.Cells[row, TaxPercentageColumn].Value?.ParseNullDecimal();
                DirectSalesOrderContentDAO.TaxAmount = ExcelWorksheet.Cells[row, TaxAmountColumn].Value?.ParseNullDecimal();
                DirectSalesOrderContentDAO.Amount = ExcelWorksheet.Cells[row, AmountColumn].Value?.ParseDecimal() ?? 0;
                DirectSalesOrderContentDAO.Factor = ExcelWorksheet.Cells[row, FactorColumn].Value?.ParseNullLong();
                DirectSalesOrderContentDAOs.Add(DirectSalesOrderContentDAO);
            }
            await DataContext.DirectSalesOrderContent.BulkMergeAsync(DirectSalesOrderContentDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_DirectSalesOrder(ExcelWorksheet ExcelWorksheet)
        {
            this.DirectSalesOrderDAOs = new List<DirectSalesOrderDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int ErpCodeColumn = StartColumn + columns.IndexOf("ErpCode");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int BuyerStoreIdColumn = StartColumn + columns.IndexOf("BuyerStoreId");
            int PhoneNumberColumn = StartColumn + columns.IndexOf("PhoneNumber");
            int StoreAddressColumn = StartColumn + columns.IndexOf("StoreAddress");
            int DeliveryAddressColumn = StartColumn + columns.IndexOf("DeliveryAddress");
            int SaleEmployeeIdColumn = StartColumn + columns.IndexOf("SaleEmployeeId");
            int OrderDateColumn = StartColumn + columns.IndexOf("OrderDate");
            int DeliveryDateColumn = StartColumn + columns.IndexOf("DeliveryDate");
            int ErpApprovalStateIdColumn = StartColumn + columns.IndexOf("ErpApprovalStateId");
            int StoreApprovalStateIdColumn = StartColumn + columns.IndexOf("StoreApprovalStateId");
            int RequestStateIdColumn = StartColumn + columns.IndexOf("RequestStateId");
            int DirectSalesOrderSourceTypeIdColumn = StartColumn + columns.IndexOf("DirectSalesOrderSourceTypeId");
            int EditedPriceStatusIdColumn = StartColumn + columns.IndexOf("EditedPriceStatusId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int SubTotalColumn = StartColumn + columns.IndexOf("SubTotal");
            int GeneralDiscountPercentageColumn = StartColumn + columns.IndexOf("GeneralDiscountPercentage");
            int GeneralDiscountAmountColumn = StartColumn + columns.IndexOf("GeneralDiscountAmount");
            int TotalTaxAmountColumn = StartColumn + columns.IndexOf("TotalTaxAmount");
            int TotalAfterTaxColumn = StartColumn + columns.IndexOf("TotalAfterTax");
            int PromotionCodeColumn = StartColumn + columns.IndexOf("PromotionCode");
            int PromotionValueColumn = StartColumn + columns.IndexOf("PromotionValue");
            int TotalColumn = StartColumn + columns.IndexOf("Total");
            int StoreCheckingIdColumn = StartColumn + columns.IndexOf("StoreCheckingId");
            int StoreUserCreatorIdColumn = StartColumn + columns.IndexOf("StoreUserCreatorId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int GeneralApprovalStateIdColumn = StartColumn + columns.IndexOf("GeneralApprovalStateId");
            int StoreBalanceCheckStateIdColumn = StartColumn + columns.IndexOf("StoreBalanceCheckStateId");
            int InventoryCheckStateIdColumn = StartColumn + columns.IndexOf("InventoryCheckStateId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                DirectSalesOrderDAO DirectSalesOrderDAO = new DirectSalesOrderDAO();
                DirectSalesOrderDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                DirectSalesOrderDAO.ErpCode = ExcelWorksheet.Cells[row, ErpCodeColumn].Value?.ParseString();
                DirectSalesOrderDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderDAO.BuyerStoreId = ExcelWorksheet.Cells[row, BuyerStoreIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderDAO.PhoneNumber = ExcelWorksheet.Cells[row, PhoneNumberColumn].Value?.ParseString();
                DirectSalesOrderDAO.StoreAddress = ExcelWorksheet.Cells[row, StoreAddressColumn].Value?.ParseString();
                DirectSalesOrderDAO.DeliveryAddress = ExcelWorksheet.Cells[row, DeliveryAddressColumn].Value?.ParseString();
                DirectSalesOrderDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderDAO.OrderDate = ExcelWorksheet.Cells[row, OrderDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                DirectSalesOrderDAO.DeliveryDate = ExcelWorksheet.Cells[row, DeliveryDateColumn].Value?.ParseNullDateTime();
                DirectSalesOrderDAO.ErpApprovalStateId = ExcelWorksheet.Cells[row, ErpApprovalStateIdColumn].Value?.ParseNullLong();
                DirectSalesOrderDAO.StoreApprovalStateId = ExcelWorksheet.Cells[row, StoreApprovalStateIdColumn].Value?.ParseNullLong();
                DirectSalesOrderDAO.RequestStateId = ExcelWorksheet.Cells[row, RequestStateIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderDAO.DirectSalesOrderSourceTypeId = ExcelWorksheet.Cells[row, DirectSalesOrderSourceTypeIdColumn].Value?.ParseNullLong();
                DirectSalesOrderDAO.EditedPriceStatusId = ExcelWorksheet.Cells[row, EditedPriceStatusIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                DirectSalesOrderDAO.SubTotal = ExcelWorksheet.Cells[row, SubTotalColumn].Value?.ParseDecimal() ?? 0;
                DirectSalesOrderDAO.GeneralDiscountPercentage = ExcelWorksheet.Cells[row, GeneralDiscountPercentageColumn].Value?.ParseNullDecimal();
                DirectSalesOrderDAO.GeneralDiscountAmount = ExcelWorksheet.Cells[row, GeneralDiscountAmountColumn].Value?.ParseNullDecimal();
                DirectSalesOrderDAO.TotalTaxAmount = ExcelWorksheet.Cells[row, TotalTaxAmountColumn].Value?.ParseDecimal() ?? 0;
                DirectSalesOrderDAO.TotalAfterTax = ExcelWorksheet.Cells[row, TotalAfterTaxColumn].Value?.ParseDecimal() ?? 0;
                DirectSalesOrderDAO.PromotionCode = ExcelWorksheet.Cells[row, PromotionCodeColumn].Value?.ParseString();
                DirectSalesOrderDAO.PromotionValue = ExcelWorksheet.Cells[row, PromotionValueColumn].Value?.ParseNullDecimal();
                DirectSalesOrderDAO.Total = ExcelWorksheet.Cells[row, TotalColumn].Value?.ParseDecimal() ?? 0;
                DirectSalesOrderDAO.StoreCheckingId = ExcelWorksheet.Cells[row, StoreCheckingIdColumn].Value?.ParseNullLong();
                DirectSalesOrderDAO.StoreUserCreatorId = ExcelWorksheet.Cells[row, StoreUserCreatorIdColumn].Value?.ParseNullLong();
                DirectSalesOrderDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseNullLong();
                DirectSalesOrderDAO.GeneralApprovalStateId = ExcelWorksheet.Cells[row, GeneralApprovalStateIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderDAO.StoreBalanceCheckStateId = ExcelWorksheet.Cells[row, StoreBalanceCheckStateIdColumn].Value?.ParseNullLong();
                DirectSalesOrderDAO.InventoryCheckStateId = ExcelWorksheet.Cells[row, InventoryCheckStateIdColumn].Value?.ParseNullLong();
                DirectSalesOrderDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                DirectSalesOrderDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                DirectSalesOrderDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                DirectSalesOrderDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                DirectSalesOrderDAOs.Add(DirectSalesOrderDAO);
            }
            await DataContext.DirectSalesOrder.BulkMergeAsync(DirectSalesOrderDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_DirectSalesOrderPromotion(ExcelWorksheet ExcelWorksheet)
        {
            this.DirectSalesOrderPromotionDAOs = new List<DirectSalesOrderPromotionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int DirectSalesOrderIdColumn = StartColumn + columns.IndexOf("DirectSalesOrderId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            int PrimaryUnitOfMeasureIdColumn = StartColumn + columns.IndexOf("PrimaryUnitOfMeasureId");
            int RequestedQuantityColumn = StartColumn + columns.IndexOf("RequestedQuantity");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int FactorColumn = StartColumn + columns.IndexOf("Factor");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                DirectSalesOrderPromotionDAO DirectSalesOrderPromotionDAO = new DirectSalesOrderPromotionDAO();
                DirectSalesOrderPromotionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderPromotionDAO.DirectSalesOrderId = ExcelWorksheet.Cells[row, DirectSalesOrderIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderPromotionDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderPromotionDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderPromotionDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId = ExcelWorksheet.Cells[row, PrimaryUnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderPromotionDAO.RequestedQuantity = ExcelWorksheet.Cells[row, RequestedQuantityColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderPromotionDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                DirectSalesOrderPromotionDAO.Factor = ExcelWorksheet.Cells[row, FactorColumn].Value?.ParseNullLong();
                DirectSalesOrderPromotionDAOs.Add(DirectSalesOrderPromotionDAO);
            }
            await DataContext.DirectSalesOrderPromotion.BulkMergeAsync(DirectSalesOrderPromotionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_DirectSalesOrderSourceType(ExcelWorksheet ExcelWorksheet)
        {
            this.DirectSalesOrderSourceTypeDAOs = new List<DirectSalesOrderSourceTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                DirectSalesOrderSourceTypeDAO DirectSalesOrderSourceTypeDAO = new DirectSalesOrderSourceTypeDAO();
                DirectSalesOrderSourceTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderSourceTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                DirectSalesOrderSourceTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                DirectSalesOrderSourceTypeDAOs.Add(DirectSalesOrderSourceTypeDAO);
            }
            await DataContext.DirectSalesOrderSourceType.BulkMergeAsync(DirectSalesOrderSourceTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_DirectSalesOrderTransaction(ExcelWorksheet ExcelWorksheet)
        {
            this.DirectSalesOrderTransactionDAOs = new List<DirectSalesOrderTransactionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int DirectSalesOrderIdColumn = StartColumn + columns.IndexOf("DirectSalesOrderId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int BuyerStoreIdColumn = StartColumn + columns.IndexOf("BuyerStoreId");
            int SalesEmployeeIdColumn = StartColumn + columns.IndexOf("SalesEmployeeId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            int DiscountColumn = StartColumn + columns.IndexOf("Discount");
            int RevenueColumn = StartColumn + columns.IndexOf("Revenue");
            int TypeIdColumn = StartColumn + columns.IndexOf("TypeId");
            int OrderDateColumn = StartColumn + columns.IndexOf("OrderDate");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                DirectSalesOrderTransactionDAO DirectSalesOrderTransactionDAO = new DirectSalesOrderTransactionDAO();
                DirectSalesOrderTransactionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.DirectSalesOrderId = ExcelWorksheet.Cells[row, DirectSalesOrderIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.BuyerStoreId = ExcelWorksheet.Cells[row, BuyerStoreIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.SalesEmployeeId = ExcelWorksheet.Cells[row, SalesEmployeeIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.Discount = ExcelWorksheet.Cells[row, DiscountColumn].Value?.ParseNullDecimal();
                DirectSalesOrderTransactionDAO.Revenue = ExcelWorksheet.Cells[row, RevenueColumn].Value?.ParseNullDecimal();
                DirectSalesOrderTransactionDAO.TypeId = ExcelWorksheet.Cells[row, TypeIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.OrderDate = ExcelWorksheet.Cells[row, OrderDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                DirectSalesOrderTransactionDAOs.Add(DirectSalesOrderTransactionDAO);
            }
            await DataContext.DirectSalesOrderTransaction.BulkMergeAsync(DirectSalesOrderTransactionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_District(ExcelWorksheet ExcelWorksheet)
        {
            this.DistrictDAOs = new List<DistrictDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PriorityColumn = StartColumn + columns.IndexOf("Priority");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                DistrictDAO DistrictDAO = new DistrictDAO();
                DistrictDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                DistrictDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                DistrictDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                DistrictDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseNullLong();
                DistrictDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseLong() ?? 0;
                DistrictDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                DistrictDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                DistrictDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                DistrictDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                DistrictDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                DistrictDAOs.Add(DistrictDAO);
            }
            await DataContext.District.BulkMergeAsync(DistrictDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_EditedPriceStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.EditedPriceStatusDAOs = new List<EditedPriceStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                EditedPriceStatusDAO EditedPriceStatusDAO = new EditedPriceStatusDAO();
                EditedPriceStatusDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                EditedPriceStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                EditedPriceStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                EditedPriceStatusDAOs.Add(EditedPriceStatusDAO);
            }
            await DataContext.EditedPriceStatus.BulkMergeAsync(EditedPriceStatusDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_EntityComponent(ExcelWorksheet ExcelWorksheet)
        {
            this.EntityComponentDAOs = new List<EntityComponentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                EntityComponentDAO EntityComponentDAO = new EntityComponentDAO();
                EntityComponentDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                EntityComponentDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                EntityComponentDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                EntityComponentDAOs.Add(EntityComponentDAO);
            }
            await DataContext.EntityComponent.BulkMergeAsync(EntityComponentDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_EntityType(ExcelWorksheet ExcelWorksheet)
        {
            this.EntityTypeDAOs = new List<EntityTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                EntityTypeDAO EntityTypeDAO = new EntityTypeDAO();
                EntityTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                EntityTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                EntityTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                EntityTypeDAOs.Add(EntityTypeDAO);
            }
            await DataContext.EntityType.BulkMergeAsync(EntityTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ERouteChangeRequestContent(ExcelWorksheet ExcelWorksheet)
        {
            this.ERouteChangeRequestContentDAOs = new List<ERouteChangeRequestContentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int ERouteChangeRequestIdColumn = StartColumn + columns.IndexOf("ERouteChangeRequestId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int OrderNumberColumn = StartColumn + columns.IndexOf("OrderNumber");
            int MondayColumn = StartColumn + columns.IndexOf("Monday");
            int TuesdayColumn = StartColumn + columns.IndexOf("Tuesday");
            int WednesdayColumn = StartColumn + columns.IndexOf("Wednesday");
            int ThursdayColumn = StartColumn + columns.IndexOf("Thursday");
            int FridayColumn = StartColumn + columns.IndexOf("Friday");
            int SaturdayColumn = StartColumn + columns.IndexOf("Saturday");
            int SundayColumn = StartColumn + columns.IndexOf("Sunday");
            int Week1Column = StartColumn + columns.IndexOf("Week1");
            int Week2Column = StartColumn + columns.IndexOf("Week2");
            int Week3Column = StartColumn + columns.IndexOf("Week3");
            int Week4Column = StartColumn + columns.IndexOf("Week4");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ERouteChangeRequestContentDAO ERouteChangeRequestContentDAO = new ERouteChangeRequestContentDAO();
                ERouteChangeRequestContentDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ERouteChangeRequestContentDAO.ERouteChangeRequestId = ExcelWorksheet.Cells[row, ERouteChangeRequestIdColumn].Value?.ParseLong() ?? 0;
                ERouteChangeRequestContentDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                ERouteChangeRequestContentDAO.OrderNumber = ExcelWorksheet.Cells[row, OrderNumberColumn].Value?.ParseNullLong();
                ERouteChangeRequestContentDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ERouteChangeRequestContentDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ERouteChangeRequestContentDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                ERouteChangeRequestContentDAOs.Add(ERouteChangeRequestContentDAO);
            }
            await DataContext.ERouteChangeRequestContent.BulkMergeAsync(ERouteChangeRequestContentDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ERouteChangeRequest(ExcelWorksheet ExcelWorksheet)
        {
            this.ERouteChangeRequestDAOs = new List<ERouteChangeRequestDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int ERouteIdColumn = StartColumn + columns.IndexOf("ERouteId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int RequestStateIdColumn = StartColumn + columns.IndexOf("RequestStateId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ERouteChangeRequestDAO ERouteChangeRequestDAO = new ERouteChangeRequestDAO();
                ERouteChangeRequestDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ERouteChangeRequestDAO.ERouteId = ExcelWorksheet.Cells[row, ERouteIdColumn].Value?.ParseLong() ?? 0;
                ERouteChangeRequestDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                ERouteChangeRequestDAO.RequestStateId = ExcelWorksheet.Cells[row, RequestStateIdColumn].Value?.ParseLong() ?? 0;
                ERouteChangeRequestDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ERouteChangeRequestDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ERouteChangeRequestDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                ERouteChangeRequestDAOs.Add(ERouteChangeRequestDAO);
            }
            await DataContext.ERouteChangeRequest.BulkMergeAsync(ERouteChangeRequestDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ERouteContent(ExcelWorksheet ExcelWorksheet)
        {
            this.ERouteContentDAOs = new List<ERouteContentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int ERouteIdColumn = StartColumn + columns.IndexOf("ERouteId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int OrderNumberColumn = StartColumn + columns.IndexOf("OrderNumber");
            int MondayColumn = StartColumn + columns.IndexOf("Monday");
            int TuesdayColumn = StartColumn + columns.IndexOf("Tuesday");
            int WednesdayColumn = StartColumn + columns.IndexOf("Wednesday");
            int ThursdayColumn = StartColumn + columns.IndexOf("Thursday");
            int FridayColumn = StartColumn + columns.IndexOf("Friday");
            int SaturdayColumn = StartColumn + columns.IndexOf("Saturday");
            int SundayColumn = StartColumn + columns.IndexOf("Sunday");
            int Week1Column = StartColumn + columns.IndexOf("Week1");
            int Week2Column = StartColumn + columns.IndexOf("Week2");
            int Week3Column = StartColumn + columns.IndexOf("Week3");
            int Week4Column = StartColumn + columns.IndexOf("Week4");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ERouteContentDAO ERouteContentDAO = new ERouteContentDAO();
                ERouteContentDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ERouteContentDAO.ERouteId = ExcelWorksheet.Cells[row, ERouteIdColumn].Value?.ParseLong() ?? 0;
                ERouteContentDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                ERouteContentDAO.OrderNumber = ExcelWorksheet.Cells[row, OrderNumberColumn].Value?.ParseNullLong();
                ERouteContentDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                ERouteContentDAOs.Add(ERouteContentDAO);
            }
            await DataContext.ERouteContent.BulkMergeAsync(ERouteContentDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ERouteContentDay(ExcelWorksheet ExcelWorksheet)
        {
            this.ERouteContentDayDAOs = new List<ERouteContentDayDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int ERouteContentIdColumn = StartColumn + columns.IndexOf("ERouteContentId");
            int OrderDayColumn = StartColumn + columns.IndexOf("OrderDay");
            int PlannedColumn = StartColumn + columns.IndexOf("Planned");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ERouteContentDayDAO ERouteContentDayDAO = new ERouteContentDayDAO();
                ERouteContentDayDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ERouteContentDayDAO.ERouteContentId = ExcelWorksheet.Cells[row, ERouteContentIdColumn].Value?.ParseLong() ?? 0;
                ERouteContentDayDAO.OrderDay = ExcelWorksheet.Cells[row, OrderDayColumn].Value?.ParseLong() ?? 0;
                ERouteContentDayDAOs.Add(ERouteContentDayDAO);
            }
            await DataContext.ERouteContentDay.BulkMergeAsync(ERouteContentDayDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ERoute(ExcelWorksheet ExcelWorksheet)
        {
            this.ERouteDAOs = new List<ERouteDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int SaleEmployeeIdColumn = StartColumn + columns.IndexOf("SaleEmployeeId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int StartDateColumn = StartColumn + columns.IndexOf("StartDate");
            int RealStartDateColumn = StartColumn + columns.IndexOf("RealStartDate");
            int EndDateColumn = StartColumn + columns.IndexOf("EndDate");
            int ERouteTypeIdColumn = StartColumn + columns.IndexOf("ERouteTypeId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int RequestStateIdColumn = StartColumn + columns.IndexOf("RequestStateId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ERouteDAO ERouteDAO = new ERouteDAO();
                ERouteDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ERouteDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ERouteDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ERouteDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseLong() ?? 0;
                ERouteDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                ERouteDAO.StartDate = ExcelWorksheet.Cells[row, StartDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ERouteDAO.RealStartDate = ExcelWorksheet.Cells[row, RealStartDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ERouteDAO.EndDate = ExcelWorksheet.Cells[row, EndDateColumn].Value?.ParseNullDateTime();
                ERouteDAO.ERouteTypeId = ExcelWorksheet.Cells[row, ERouteTypeIdColumn].Value?.ParseLong() ?? 0;
                ERouteDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                ERouteDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                ERouteDAO.RequestStateId = ExcelWorksheet.Cells[row, RequestStateIdColumn].Value?.ParseLong() ?? 0;
                ERouteDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                ERouteDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ERouteDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ERouteDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                ERouteDAOs.Add(ERouteDAO);
            }
            await DataContext.ERoute.BulkMergeAsync(ERouteDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ERouteType(ExcelWorksheet ExcelWorksheet)
        {
            this.ERouteTypeDAOs = new List<ERouteTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ERouteTypeDAO ERouteTypeDAO = new ERouteTypeDAO();
                ERouteTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ERouteTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ERouteTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ERouteTypeDAOs.Add(ERouteTypeDAO);
            }
            await DataContext.ERouteType.BulkMergeAsync(ERouteTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ErpApprovalState(ExcelWorksheet ExcelWorksheet)
        {
            this.ErpApprovalStateDAOs = new List<ErpApprovalStateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ErpApprovalStateDAO ErpApprovalStateDAO = new ErpApprovalStateDAO();
                ErpApprovalStateDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ErpApprovalStateDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ErpApprovalStateDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ErpApprovalStateDAOs.Add(ErpApprovalStateDAO);
            }
            await DataContext.ErpApprovalState.BulkMergeAsync(ErpApprovalStateDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_EstimatedRevenue(ExcelWorksheet ExcelWorksheet)
        {
            this.EstimatedRevenueDAOs = new List<EstimatedRevenueDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                EstimatedRevenueDAO EstimatedRevenueDAO = new EstimatedRevenueDAO();
                EstimatedRevenueDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                EstimatedRevenueDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                EstimatedRevenueDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                EstimatedRevenueDAOs.Add(EstimatedRevenueDAO);
            }
            await DataContext.EstimatedRevenue.BulkMergeAsync(EstimatedRevenueDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ExportTemplate(ExcelWorksheet ExcelWorksheet)
        {
            this.ExportTemplateDAOs = new List<ExportTemplateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int FileNameColumn = StartColumn + columns.IndexOf("FileName");
            int ExtensionColumn = StartColumn + columns.IndexOf("Extension");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ExportTemplateDAO ExportTemplateDAO = new ExportTemplateDAO();
                ExportTemplateDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ExportTemplateDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ExportTemplateDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ExportTemplateDAO.FileName = ExcelWorksheet.Cells[row, FileNameColumn].Value?.ParseString();
                ExportTemplateDAO.Extension = ExcelWorksheet.Cells[row, ExtensionColumn].Value?.ParseString();
                ExportTemplateDAOs.Add(ExportTemplateDAO);
            }
            await DataContext.ExportTemplate.BulkMergeAsync(ExportTemplateDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_File(ExcelWorksheet ExcelWorksheet)
        {
            this.FileDAOs = new List<FileDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int MimeTypeColumn = StartColumn + columns.IndexOf("MimeType");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                FileDAO FileDAO = new FileDAO();
                FileDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                FileDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                FileDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                FileDAO.MimeType = ExcelWorksheet.Cells[row, MimeTypeColumn].Value?.ParseString();
                FileDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                FileDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                FileDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                FileDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                FileDAOs.Add(FileDAO);
            }
            await DataContext.File.BulkMergeAsync(FileDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_GeneralApprovalState(ExcelWorksheet ExcelWorksheet)
        {
            this.GeneralApprovalStateDAOs = new List<GeneralApprovalStateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                GeneralApprovalStateDAO GeneralApprovalStateDAO = new GeneralApprovalStateDAO();
                GeneralApprovalStateDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                GeneralApprovalStateDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                GeneralApprovalStateDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                GeneralApprovalStateDAOs.Add(GeneralApprovalStateDAO);
            }
            await DataContext.GeneralApprovalState.BulkMergeAsync(GeneralApprovalStateDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_GlobalUserType(ExcelWorksheet ExcelWorksheet)
        {
            this.GlobalUserTypeDAOs = new List<GlobalUserTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                GlobalUserTypeDAO GlobalUserTypeDAO = new GlobalUserTypeDAO();
                GlobalUserTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                GlobalUserTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                GlobalUserTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                GlobalUserTypeDAOs.Add(GlobalUserTypeDAO);
            }
            await DataContext.GlobalUserType.BulkMergeAsync(GlobalUserTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Image(ExcelWorksheet ExcelWorksheet)
        {
            this.ImageDAOs = new List<ImageDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int UrlColumn = StartColumn + columns.IndexOf("Url");
            int ThumbnailUrlColumn = StartColumn + columns.IndexOf("ThumbnailUrl");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ImageDAO ImageDAO = new ImageDAO();
                ImageDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ImageDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ImageDAO.Url = ExcelWorksheet.Cells[row, UrlColumn].Value?.ParseString();
                ImageDAO.ThumbnailUrl = ExcelWorksheet.Cells[row, ThumbnailUrlColumn].Value?.ParseString();
                ImageDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                ImageDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ImageDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ImageDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                ImageDAOs.Add(ImageDAO);
            }
            await DataContext.Image.BulkMergeAsync(ImageDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_IndirectSalesOrderContent(ExcelWorksheet ExcelWorksheet)
        {
            this.IndirectSalesOrderContentDAOs = new List<IndirectSalesOrderContentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int IndirectSalesOrderIdColumn = StartColumn + columns.IndexOf("IndirectSalesOrderId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            int PrimaryUnitOfMeasureIdColumn = StartColumn + columns.IndexOf("PrimaryUnitOfMeasureId");
            int RequestedQuantityColumn = StartColumn + columns.IndexOf("RequestedQuantity");
            int PrimaryPriceColumn = StartColumn + columns.IndexOf("PrimaryPrice");
            int SalePriceColumn = StartColumn + columns.IndexOf("SalePrice");
            int EditedPriceStatusIdColumn = StartColumn + columns.IndexOf("EditedPriceStatusId");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int DiscountAmountColumn = StartColumn + columns.IndexOf("DiscountAmount");
            int GeneralDiscountPercentageColumn = StartColumn + columns.IndexOf("GeneralDiscountPercentage");
            int GeneralDiscountAmountColumn = StartColumn + columns.IndexOf("GeneralDiscountAmount");
            int TaxPercentageColumn = StartColumn + columns.IndexOf("TaxPercentage");
            int TaxAmountColumn = StartColumn + columns.IndexOf("TaxAmount");
            int AmountColumn = StartColumn + columns.IndexOf("Amount");
            int FactorColumn = StartColumn + columns.IndexOf("Factor");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                IndirectSalesOrderContentDAO IndirectSalesOrderContentDAO = new IndirectSalesOrderContentDAO();
                IndirectSalesOrderContentDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderContentDAO.IndirectSalesOrderId = ExcelWorksheet.Cells[row, IndirectSalesOrderIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderContentDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderContentDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderContentDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderContentDAO.PrimaryUnitOfMeasureId = ExcelWorksheet.Cells[row, PrimaryUnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderContentDAO.RequestedQuantity = ExcelWorksheet.Cells[row, RequestedQuantityColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderContentDAO.PrimaryPrice = ExcelWorksheet.Cells[row, PrimaryPriceColumn].Value?.ParseDecimal() ?? 0;
                IndirectSalesOrderContentDAO.SalePrice = ExcelWorksheet.Cells[row, SalePriceColumn].Value?.ParseDecimal() ?? 0;
                IndirectSalesOrderContentDAO.EditedPriceStatusId = ExcelWorksheet.Cells[row, EditedPriceStatusIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderContentDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                IndirectSalesOrderContentDAO.DiscountAmount = ExcelWorksheet.Cells[row, DiscountAmountColumn].Value?.ParseNullDecimal();
                IndirectSalesOrderContentDAO.GeneralDiscountPercentage = ExcelWorksheet.Cells[row, GeneralDiscountPercentageColumn].Value?.ParseNullDecimal();
                IndirectSalesOrderContentDAO.GeneralDiscountAmount = ExcelWorksheet.Cells[row, GeneralDiscountAmountColumn].Value?.ParseNullDecimal();
                IndirectSalesOrderContentDAO.TaxPercentage = ExcelWorksheet.Cells[row, TaxPercentageColumn].Value?.ParseNullDecimal();
                IndirectSalesOrderContentDAO.TaxAmount = ExcelWorksheet.Cells[row, TaxAmountColumn].Value?.ParseNullDecimal();
                IndirectSalesOrderContentDAO.Amount = ExcelWorksheet.Cells[row, AmountColumn].Value?.ParseDecimal() ?? 0;
                IndirectSalesOrderContentDAO.Factor = ExcelWorksheet.Cells[row, FactorColumn].Value?.ParseNullLong();
                IndirectSalesOrderContentDAOs.Add(IndirectSalesOrderContentDAO);
            }
            await DataContext.IndirectSalesOrderContent.BulkMergeAsync(IndirectSalesOrderContentDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_IndirectSalesOrder(ExcelWorksheet ExcelWorksheet)
        {
            this.IndirectSalesOrderDAOs = new List<IndirectSalesOrderDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int BuyerStoreIdColumn = StartColumn + columns.IndexOf("BuyerStoreId");
            int PhoneNumberColumn = StartColumn + columns.IndexOf("PhoneNumber");
            int StoreAddressColumn = StartColumn + columns.IndexOf("StoreAddress");
            int DeliveryAddressColumn = StartColumn + columns.IndexOf("DeliveryAddress");
            int SellerStoreIdColumn = StartColumn + columns.IndexOf("SellerStoreId");
            int SaleEmployeeIdColumn = StartColumn + columns.IndexOf("SaleEmployeeId");
            int OrderDateColumn = StartColumn + columns.IndexOf("OrderDate");
            int DeliveryDateColumn = StartColumn + columns.IndexOf("DeliveryDate");
            int EditedPriceStatusIdColumn = StartColumn + columns.IndexOf("EditedPriceStatusId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int SubTotalColumn = StartColumn + columns.IndexOf("SubTotal");
            int GeneralDiscountPercentageColumn = StartColumn + columns.IndexOf("GeneralDiscountPercentage");
            int GeneralDiscountAmountColumn = StartColumn + columns.IndexOf("GeneralDiscountAmount");
            int TotalColumn = StartColumn + columns.IndexOf("Total");
            int StoreCheckingIdColumn = StartColumn + columns.IndexOf("StoreCheckingId");
            int RequestStateIdColumn = StartColumn + columns.IndexOf("RequestStateId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                IndirectSalesOrderDAO IndirectSalesOrderDAO = new IndirectSalesOrderDAO();
                IndirectSalesOrderDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                IndirectSalesOrderDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderDAO.BuyerStoreId = ExcelWorksheet.Cells[row, BuyerStoreIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderDAO.PhoneNumber = ExcelWorksheet.Cells[row, PhoneNumberColumn].Value?.ParseString();
                IndirectSalesOrderDAO.StoreAddress = ExcelWorksheet.Cells[row, StoreAddressColumn].Value?.ParseString();
                IndirectSalesOrderDAO.DeliveryAddress = ExcelWorksheet.Cells[row, DeliveryAddressColumn].Value?.ParseString();
                IndirectSalesOrderDAO.SellerStoreId = ExcelWorksheet.Cells[row, SellerStoreIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderDAO.OrderDate = ExcelWorksheet.Cells[row, OrderDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                IndirectSalesOrderDAO.DeliveryDate = ExcelWorksheet.Cells[row, DeliveryDateColumn].Value?.ParseNullDateTime();
                IndirectSalesOrderDAO.EditedPriceStatusId = ExcelWorksheet.Cells[row, EditedPriceStatusIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                IndirectSalesOrderDAO.SubTotal = ExcelWorksheet.Cells[row, SubTotalColumn].Value?.ParseDecimal() ?? 0;
                IndirectSalesOrderDAO.GeneralDiscountPercentage = ExcelWorksheet.Cells[row, GeneralDiscountPercentageColumn].Value?.ParseNullDecimal();
                IndirectSalesOrderDAO.GeneralDiscountAmount = ExcelWorksheet.Cells[row, GeneralDiscountAmountColumn].Value?.ParseNullDecimal();
                IndirectSalesOrderDAO.Total = ExcelWorksheet.Cells[row, TotalColumn].Value?.ParseDecimal() ?? 0;
                IndirectSalesOrderDAO.StoreCheckingId = ExcelWorksheet.Cells[row, StoreCheckingIdColumn].Value?.ParseNullLong();
                IndirectSalesOrderDAO.RequestStateId = ExcelWorksheet.Cells[row, RequestStateIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                IndirectSalesOrderDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                IndirectSalesOrderDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                IndirectSalesOrderDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                IndirectSalesOrderDAOs.Add(IndirectSalesOrderDAO);
            }
            await DataContext.IndirectSalesOrder.BulkMergeAsync(IndirectSalesOrderDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_IndirectSalesOrderPromotion(ExcelWorksheet ExcelWorksheet)
        {
            this.IndirectSalesOrderPromotionDAOs = new List<IndirectSalesOrderPromotionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int IndirectSalesOrderIdColumn = StartColumn + columns.IndexOf("IndirectSalesOrderId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            int PrimaryUnitOfMeasureIdColumn = StartColumn + columns.IndexOf("PrimaryUnitOfMeasureId");
            int RequestedQuantityColumn = StartColumn + columns.IndexOf("RequestedQuantity");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int FactorColumn = StartColumn + columns.IndexOf("Factor");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                IndirectSalesOrderPromotionDAO IndirectSalesOrderPromotionDAO = new IndirectSalesOrderPromotionDAO();
                IndirectSalesOrderPromotionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderPromotionDAO.IndirectSalesOrderId = ExcelWorksheet.Cells[row, IndirectSalesOrderIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderPromotionDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderPromotionDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderPromotionDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderPromotionDAO.PrimaryUnitOfMeasureId = ExcelWorksheet.Cells[row, PrimaryUnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderPromotionDAO.RequestedQuantity = ExcelWorksheet.Cells[row, RequestedQuantityColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderPromotionDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                IndirectSalesOrderPromotionDAO.Factor = ExcelWorksheet.Cells[row, FactorColumn].Value?.ParseNullLong();
                IndirectSalesOrderPromotionDAOs.Add(IndirectSalesOrderPromotionDAO);
            }
            await DataContext.IndirectSalesOrderPromotion.BulkMergeAsync(IndirectSalesOrderPromotionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_IndirectSalesOrderTransaction(ExcelWorksheet ExcelWorksheet)
        {
            this.IndirectSalesOrderTransactionDAOs = new List<IndirectSalesOrderTransactionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int IndirectSalesOrderIdColumn = StartColumn + columns.IndexOf("IndirectSalesOrderId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int BuyerStoreIdColumn = StartColumn + columns.IndexOf("BuyerStoreId");
            int SalesEmployeeIdColumn = StartColumn + columns.IndexOf("SalesEmployeeId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            int DiscountColumn = StartColumn + columns.IndexOf("Discount");
            int RevenueColumn = StartColumn + columns.IndexOf("Revenue");
            int TypeIdColumn = StartColumn + columns.IndexOf("TypeId");
            int OrderDateColumn = StartColumn + columns.IndexOf("OrderDate");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                IndirectSalesOrderTransactionDAO IndirectSalesOrderTransactionDAO = new IndirectSalesOrderTransactionDAO();
                IndirectSalesOrderTransactionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderTransactionDAO.IndirectSalesOrderId = ExcelWorksheet.Cells[row, IndirectSalesOrderIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderTransactionDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderTransactionDAO.BuyerStoreId = ExcelWorksheet.Cells[row, BuyerStoreIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderTransactionDAO.SalesEmployeeId = ExcelWorksheet.Cells[row, SalesEmployeeIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderTransactionDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderTransactionDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderTransactionDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderTransactionDAO.Discount = ExcelWorksheet.Cells[row, DiscountColumn].Value?.ParseNullDecimal();
                IndirectSalesOrderTransactionDAO.Revenue = ExcelWorksheet.Cells[row, RevenueColumn].Value?.ParseNullDecimal();
                IndirectSalesOrderTransactionDAO.TypeId = ExcelWorksheet.Cells[row, TypeIdColumn].Value?.ParseLong() ?? 0;
                IndirectSalesOrderTransactionDAO.OrderDate = ExcelWorksheet.Cells[row, OrderDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                IndirectSalesOrderTransactionDAOs.Add(IndirectSalesOrderTransactionDAO);
            }
            await DataContext.IndirectSalesOrderTransaction.BulkMergeAsync(IndirectSalesOrderTransactionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Inventory(ExcelWorksheet ExcelWorksheet)
        {
            this.InventoryDAOs = new List<InventoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int WarehouseIdColumn = StartColumn + columns.IndexOf("WarehouseId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int SaleStockColumn = StartColumn + columns.IndexOf("SaleStock");
            int AccountingStockColumn = StartColumn + columns.IndexOf("AccountingStock");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                InventoryDAO InventoryDAO = new InventoryDAO();
                InventoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                InventoryDAO.WarehouseId = ExcelWorksheet.Cells[row, WarehouseIdColumn].Value?.ParseLong() ?? 0;
                InventoryDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                InventoryDAO.SaleStock = ExcelWorksheet.Cells[row, SaleStockColumn].Value?.ParseLong() ?? 0;
                InventoryDAO.AccountingStock = ExcelWorksheet.Cells[row, AccountingStockColumn].Value?.ParseLong() ?? 0;
                InventoryDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                InventoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                InventoryDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                InventoryDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                InventoryDAOs.Add(InventoryDAO);
            }
            await DataContext.Inventory.BulkMergeAsync(InventoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_InventoryHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.InventoryHistoryDAOs = new List<InventoryHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int InventoryIdColumn = StartColumn + columns.IndexOf("InventoryId");
            int OldSaleStockColumn = StartColumn + columns.IndexOf("OldSaleStock");
            int SaleStockColumn = StartColumn + columns.IndexOf("SaleStock");
            int OldAccountingStockColumn = StartColumn + columns.IndexOf("OldAccountingStock");
            int AccountingStockColumn = StartColumn + columns.IndexOf("AccountingStock");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                InventoryHistoryDAO InventoryHistoryDAO = new InventoryHistoryDAO();
                InventoryHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                InventoryHistoryDAO.InventoryId = ExcelWorksheet.Cells[row, InventoryIdColumn].Value?.ParseLong() ?? 0;
                InventoryHistoryDAO.OldSaleStock = ExcelWorksheet.Cells[row, OldSaleStockColumn].Value?.ParseLong() ?? 0;
                InventoryHistoryDAO.SaleStock = ExcelWorksheet.Cells[row, SaleStockColumn].Value?.ParseLong() ?? 0;
                InventoryHistoryDAO.OldAccountingStock = ExcelWorksheet.Cells[row, OldAccountingStockColumn].Value?.ParseLong() ?? 0;
                InventoryHistoryDAO.AccountingStock = ExcelWorksheet.Cells[row, AccountingStockColumn].Value?.ParseLong() ?? 0;
                InventoryHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                InventoryHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                InventoryHistoryDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                InventoryHistoryDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                InventoryHistoryDAOs.Add(InventoryHistoryDAO);
            }
            await DataContext.InventoryHistory.BulkMergeAsync(InventoryHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Item(ExcelWorksheet ExcelWorksheet)
        {
            this.ItemDAOs = new List<ItemDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int ProductIdColumn = StartColumn + columns.IndexOf("ProductId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int ERPCodeColumn = StartColumn + columns.IndexOf("ERPCode");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ScanCodeColumn = StartColumn + columns.IndexOf("ScanCode");
            int SalePriceColumn = StartColumn + columns.IndexOf("SalePrice");
            int RetailPriceColumn = StartColumn + columns.IndexOf("RetailPrice");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ItemDAO ItemDAO = new ItemDAO();
                ItemDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ItemDAO.ProductId = ExcelWorksheet.Cells[row, ProductIdColumn].Value?.ParseLong() ?? 0;
                ItemDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ItemDAO.ERPCode = ExcelWorksheet.Cells[row, ERPCodeColumn].Value?.ParseString();
                ItemDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ItemDAO.ScanCode = ExcelWorksheet.Cells[row, ScanCodeColumn].Value?.ParseString();
                ItemDAO.SalePrice = ExcelWorksheet.Cells[row, SalePriceColumn].Value?.ParseNullDecimal();
                ItemDAO.RetailPrice = ExcelWorksheet.Cells[row, RetailPriceColumn].Value?.ParseNullDecimal();
                ItemDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                ItemDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                ItemDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ItemDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ItemDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                ItemDAOs.Add(ItemDAO);
            }
            await DataContext.Item.BulkMergeAsync(ItemDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ItemHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.ItemHistoryDAOs = new List<ItemHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int TimeColumn = StartColumn + columns.IndexOf("Time");
            int ModifierIdColumn = StartColumn + columns.IndexOf("ModifierId");
            int OldPriceColumn = StartColumn + columns.IndexOf("OldPrice");
            int NewPriceColumn = StartColumn + columns.IndexOf("NewPrice");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ItemHistoryDAO ItemHistoryDAO = new ItemHistoryDAO();
                ItemHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ItemHistoryDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                ItemHistoryDAO.Time = ExcelWorksheet.Cells[row, TimeColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ItemHistoryDAO.ModifierId = ExcelWorksheet.Cells[row, ModifierIdColumn].Value?.ParseLong() ?? 0;
                ItemHistoryDAO.OldPrice = ExcelWorksheet.Cells[row, OldPriceColumn].Value?.ParseDecimal() ?? 0;
                ItemHistoryDAO.NewPrice = ExcelWorksheet.Cells[row, NewPriceColumn].Value?.ParseDecimal() ?? 0;
                ItemHistoryDAOs.Add(ItemHistoryDAO);
            }
            await DataContext.ItemHistory.BulkMergeAsync(ItemHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ItemImageMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.ItemImageMappingDAOs = new List<ItemImageMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ItemImageMappingDAO ItemImageMappingDAO = new ItemImageMappingDAO();
                ItemImageMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                ItemImageMappingDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseLong() ?? 0;
                ItemImageMappingDAOs.Add(ItemImageMappingDAO);
            }
            await DataContext.ItemImageMapping.BulkMergeAsync(ItemImageMappingDAOs);
        }
        protected async Task Given_KpiCriteriaGeneral(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiCriteriaGeneralDAOs = new List<KpiCriteriaGeneralDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiCriteriaGeneralDAO KpiCriteriaGeneralDAO = new KpiCriteriaGeneralDAO();
                KpiCriteriaGeneralDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiCriteriaGeneralDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                KpiCriteriaGeneralDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                KpiCriteriaGeneralDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                KpiCriteriaGeneralDAOs.Add(KpiCriteriaGeneralDAO);
            }
            await DataContext.KpiCriteriaGeneral.BulkMergeAsync(KpiCriteriaGeneralDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiCriteriaItem(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiCriteriaItemDAOs = new List<KpiCriteriaItemDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiCriteriaItemDAO KpiCriteriaItemDAO = new KpiCriteriaItemDAO();
                KpiCriteriaItemDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiCriteriaItemDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                KpiCriteriaItemDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                KpiCriteriaItemDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                KpiCriteriaItemDAOs.Add(KpiCriteriaItemDAO);
            }
            await DataContext.KpiCriteriaItem.BulkMergeAsync(KpiCriteriaItemDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiGeneralContent(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiGeneralContentDAOs = new List<KpiGeneralContentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int KpiGeneralIdColumn = StartColumn + columns.IndexOf("KpiGeneralId");
            int KpiCriteriaGeneralIdColumn = StartColumn + columns.IndexOf("KpiCriteriaGeneralId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiGeneralContentDAO KpiGeneralContentDAO = new KpiGeneralContentDAO();
                KpiGeneralContentDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralContentDAO.KpiGeneralId = ExcelWorksheet.Cells[row, KpiGeneralIdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralContentDAO.KpiCriteriaGeneralId = ExcelWorksheet.Cells[row, KpiCriteriaGeneralIdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralContentDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralContentDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                KpiGeneralContentDAOs.Add(KpiGeneralContentDAO);
            }
            await DataContext.KpiGeneralContent.BulkMergeAsync(KpiGeneralContentDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiGeneralContentKpiPeriodMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiGeneralContentKpiPeriodMappingDAOs = new List<KpiGeneralContentKpiPeriodMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int KpiGeneralContentIdColumn = StartColumn + columns.IndexOf("KpiGeneralContentId");
            int KpiPeriodIdColumn = StartColumn + columns.IndexOf("KpiPeriodId");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiGeneralContentKpiPeriodMappingDAO KpiGeneralContentKpiPeriodMappingDAO = new KpiGeneralContentKpiPeriodMappingDAO();
                KpiGeneralContentKpiPeriodMappingDAO.KpiGeneralContentId = ExcelWorksheet.Cells[row, KpiGeneralContentIdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralContentKpiPeriodMappingDAO.KpiPeriodId = ExcelWorksheet.Cells[row, KpiPeriodIdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralContentKpiPeriodMappingDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseNullDecimal();
                KpiGeneralContentKpiPeriodMappingDAOs.Add(KpiGeneralContentKpiPeriodMappingDAO);
            }
            await DataContext.KpiGeneralContentKpiPeriodMapping.BulkMergeAsync(KpiGeneralContentKpiPeriodMappingDAOs);
        }
        protected async Task Given_KpiGeneral(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiGeneralDAOs = new List<KpiGeneralDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int EmployeeIdColumn = StartColumn + columns.IndexOf("EmployeeId");
            int KpiYearIdColumn = StartColumn + columns.IndexOf("KpiYearId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiGeneralDAO KpiGeneralDAO = new KpiGeneralDAO();
                KpiGeneralDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralDAO.EmployeeId = ExcelWorksheet.Cells[row, EmployeeIdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralDAO.KpiYearId = ExcelWorksheet.Cells[row, KpiYearIdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                KpiGeneralDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                KpiGeneralDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                KpiGeneralDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                KpiGeneralDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                KpiGeneralDAOs.Add(KpiGeneralDAO);
            }
            await DataContext.KpiGeneral.BulkMergeAsync(KpiGeneralDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiItemContent(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiItemContentDAOs = new List<KpiItemContentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int KpiItemIdColumn = StartColumn + columns.IndexOf("KpiItemId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiItemContentDAO KpiItemContentDAO = new KpiItemContentDAO();
                KpiItemContentDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiItemContentDAO.KpiItemId = ExcelWorksheet.Cells[row, KpiItemIdColumn].Value?.ParseLong() ?? 0;
                KpiItemContentDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                KpiItemContentDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                KpiItemContentDAOs.Add(KpiItemContentDAO);
            }
            await DataContext.KpiItemContent.BulkMergeAsync(KpiItemContentDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiItemContentKpiCriteriaItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiItemContentKpiCriteriaItemMappingDAOs = new List<KpiItemContentKpiCriteriaItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int KpiItemContentIdColumn = StartColumn + columns.IndexOf("KpiItemContentId");
            int KpiCriteriaItemIdColumn = StartColumn + columns.IndexOf("KpiCriteriaItemId");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiItemContentKpiCriteriaItemMappingDAO KpiItemContentKpiCriteriaItemMappingDAO = new KpiItemContentKpiCriteriaItemMappingDAO();
                KpiItemContentKpiCriteriaItemMappingDAO.KpiItemContentId = ExcelWorksheet.Cells[row, KpiItemContentIdColumn].Value?.ParseLong() ?? 0;
                KpiItemContentKpiCriteriaItemMappingDAO.KpiCriteriaItemId = ExcelWorksheet.Cells[row, KpiCriteriaItemIdColumn].Value?.ParseLong() ?? 0;
                KpiItemContentKpiCriteriaItemMappingDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseNullLong();
                KpiItemContentKpiCriteriaItemMappingDAOs.Add(KpiItemContentKpiCriteriaItemMappingDAO);
            }
            await DataContext.KpiItemContentKpiCriteriaItemMapping.BulkMergeAsync(KpiItemContentKpiCriteriaItemMappingDAOs);
        }
        protected async Task Given_KpiItem(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiItemDAOs = new List<KpiItemDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int KpiYearIdColumn = StartColumn + columns.IndexOf("KpiYearId");
            int KpiPeriodIdColumn = StartColumn + columns.IndexOf("KpiPeriodId");
            int KpiItemTypeIdColumn = StartColumn + columns.IndexOf("KpiItemTypeId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int EmployeeIdColumn = StartColumn + columns.IndexOf("EmployeeId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiItemDAO KpiItemDAO = new KpiItemDAO();
                KpiItemDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiItemDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                KpiItemDAO.KpiYearId = ExcelWorksheet.Cells[row, KpiYearIdColumn].Value?.ParseLong() ?? 0;
                KpiItemDAO.KpiPeriodId = ExcelWorksheet.Cells[row, KpiPeriodIdColumn].Value?.ParseLong() ?? 0;
                KpiItemDAO.KpiItemTypeId = ExcelWorksheet.Cells[row, KpiItemTypeIdColumn].Value?.ParseLong() ?? 0;
                KpiItemDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                KpiItemDAO.EmployeeId = ExcelWorksheet.Cells[row, EmployeeIdColumn].Value?.ParseLong() ?? 0;
                KpiItemDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                KpiItemDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                KpiItemDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                KpiItemDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                KpiItemDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                KpiItemDAOs.Add(KpiItemDAO);
            }
            await DataContext.KpiItem.BulkMergeAsync(KpiItemDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiItemType(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiItemTypeDAOs = new List<KpiItemTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiItemTypeDAO KpiItemTypeDAO = new KpiItemTypeDAO();
                KpiItemTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiItemTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                KpiItemTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                KpiItemTypeDAOs.Add(KpiItemTypeDAO);
            }
            await DataContext.KpiItemType.BulkMergeAsync(KpiItemTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiPeriod(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiPeriodDAOs = new List<KpiPeriodDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiPeriodDAO KpiPeriodDAO = new KpiPeriodDAO();
                KpiPeriodDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiPeriodDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                KpiPeriodDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                KpiPeriodDAOs.Add(KpiPeriodDAO);
            }
            await DataContext.KpiPeriod.BulkMergeAsync(KpiPeriodDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiProductGroupingContentCriteriaMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiProductGroupingContentCriteriaMappingDAOs = new List<KpiProductGroupingContentCriteriaMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int KpiProductGroupingContentIdColumn = StartColumn + columns.IndexOf("KpiProductGroupingContentId");
            int KpiProductGroupingCriteriaIdColumn = StartColumn + columns.IndexOf("KpiProductGroupingCriteriaId");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiProductGroupingContentCriteriaMappingDAO KpiProductGroupingContentCriteriaMappingDAO = new KpiProductGroupingContentCriteriaMappingDAO();
                KpiProductGroupingContentCriteriaMappingDAO.KpiProductGroupingContentId = ExcelWorksheet.Cells[row, KpiProductGroupingContentIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingContentCriteriaMappingDAO.KpiProductGroupingCriteriaId = ExcelWorksheet.Cells[row, KpiProductGroupingCriteriaIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingContentCriteriaMappingDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseNullLong();
                KpiProductGroupingContentCriteriaMappingDAOs.Add(KpiProductGroupingContentCriteriaMappingDAO);
            }
            await DataContext.KpiProductGroupingContentCriteriaMapping.BulkMergeAsync(KpiProductGroupingContentCriteriaMappingDAOs);
        }
        protected async Task Given_KpiProductGroupingContent(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiProductGroupingContentDAOs = new List<KpiProductGroupingContentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int KpiProductGroupingIdColumn = StartColumn + columns.IndexOf("KpiProductGroupingId");
            int ProductGroupingIdColumn = StartColumn + columns.IndexOf("ProductGroupingId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiProductGroupingContentDAO KpiProductGroupingContentDAO = new KpiProductGroupingContentDAO();
                KpiProductGroupingContentDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingContentDAO.KpiProductGroupingId = ExcelWorksheet.Cells[row, KpiProductGroupingIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingContentDAO.ProductGroupingId = ExcelWorksheet.Cells[row, ProductGroupingIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingContentDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                KpiProductGroupingContentDAOs.Add(KpiProductGroupingContentDAO);
            }
            await DataContext.KpiProductGroupingContent.BulkMergeAsync(KpiProductGroupingContentDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiProductGroupingContentItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiProductGroupingContentItemMappingDAOs = new List<KpiProductGroupingContentItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int KpiProductGroupingContentIdColumn = StartColumn + columns.IndexOf("KpiProductGroupingContentId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiProductGroupingContentItemMappingDAO KpiProductGroupingContentItemMappingDAO = new KpiProductGroupingContentItemMappingDAO();
                KpiProductGroupingContentItemMappingDAO.KpiProductGroupingContentId = ExcelWorksheet.Cells[row, KpiProductGroupingContentIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingContentItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingContentItemMappingDAOs.Add(KpiProductGroupingContentItemMappingDAO);
            }
            await DataContext.KpiProductGroupingContentItemMapping.BulkMergeAsync(KpiProductGroupingContentItemMappingDAOs);
        }
        protected async Task Given_KpiProductGroupingCriteria(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiProductGroupingCriteriaDAOs = new List<KpiProductGroupingCriteriaDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiProductGroupingCriteriaDAO KpiProductGroupingCriteriaDAO = new KpiProductGroupingCriteriaDAO();
                KpiProductGroupingCriteriaDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingCriteriaDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                KpiProductGroupingCriteriaDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                KpiProductGroupingCriteriaDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingCriteriaDAOs.Add(KpiProductGroupingCriteriaDAO);
            }
            await DataContext.KpiProductGroupingCriteria.BulkMergeAsync(KpiProductGroupingCriteriaDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiProductGrouping(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiProductGroupingDAOs = new List<KpiProductGroupingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int KpiYearIdColumn = StartColumn + columns.IndexOf("KpiYearId");
            int KpiPeriodIdColumn = StartColumn + columns.IndexOf("KpiPeriodId");
            int KpiProductGroupingTypeIdColumn = StartColumn + columns.IndexOf("KpiProductGroupingTypeId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int EmployeeIdColumn = StartColumn + columns.IndexOf("EmployeeId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiProductGroupingDAO KpiProductGroupingDAO = new KpiProductGroupingDAO();
                KpiProductGroupingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingDAO.KpiYearId = ExcelWorksheet.Cells[row, KpiYearIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingDAO.KpiPeriodId = ExcelWorksheet.Cells[row, KpiPeriodIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingDAO.KpiProductGroupingTypeId = ExcelWorksheet.Cells[row, KpiProductGroupingTypeIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingDAO.EmployeeId = ExcelWorksheet.Cells[row, EmployeeIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                KpiProductGroupingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                KpiProductGroupingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                KpiProductGroupingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                KpiProductGroupingDAOs.Add(KpiProductGroupingDAO);
            }
            await DataContext.KpiProductGrouping.BulkMergeAsync(KpiProductGroupingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiProductGroupingType(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiProductGroupingTypeDAOs = new List<KpiProductGroupingTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiProductGroupingTypeDAO KpiProductGroupingTypeDAO = new KpiProductGroupingTypeDAO();
                KpiProductGroupingTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiProductGroupingTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                KpiProductGroupingTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                KpiProductGroupingTypeDAOs.Add(KpiProductGroupingTypeDAO);
            }
            await DataContext.KpiProductGroupingType.BulkMergeAsync(KpiProductGroupingTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_KpiYear(ExcelWorksheet ExcelWorksheet)
        {
            this.KpiYearDAOs = new List<KpiYearDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                KpiYearDAO KpiYearDAO = new KpiYearDAO();
                KpiYearDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                KpiYearDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                KpiYearDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                KpiYearDAOs.Add(KpiYearDAO);
            }
            await DataContext.KpiYear.BulkMergeAsync(KpiYearDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_LuckyDraw(ExcelWorksheet ExcelWorksheet)
        {
            this.LuckyDrawDAOs = new List<LuckyDrawDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int LuckyDrawTypeIdColumn = StartColumn + columns.IndexOf("LuckyDrawTypeId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int RevenuePerTurnColumn = StartColumn + columns.IndexOf("RevenuePerTurn");
            int StartAtColumn = StartColumn + columns.IndexOf("StartAt");
            int EndAtColumn = StartColumn + columns.IndexOf("EndAt");
            int AvatarImageIdColumn = StartColumn + columns.IndexOf("AvatarImageId");
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                LuckyDrawDAO LuckyDrawDAO = new LuckyDrawDAO();
                LuckyDrawDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                LuckyDrawDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                LuckyDrawDAO.LuckyDrawTypeId = ExcelWorksheet.Cells[row, LuckyDrawTypeIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawDAO.RevenuePerTurn = ExcelWorksheet.Cells[row, RevenuePerTurnColumn].Value?.ParseDecimal() ?? 0;
                LuckyDrawDAO.StartAt = ExcelWorksheet.Cells[row, StartAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                LuckyDrawDAO.EndAt = ExcelWorksheet.Cells[row, EndAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                LuckyDrawDAO.AvatarImageId = ExcelWorksheet.Cells[row, AvatarImageIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                LuckyDrawDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                LuckyDrawDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                LuckyDrawDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                LuckyDrawDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                LuckyDrawDAOs.Add(LuckyDrawDAO);
            }
            await DataContext.LuckyDraw.BulkMergeAsync(LuckyDrawDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_LuckyDrawNumber(ExcelWorksheet ExcelWorksheet)
        {
            this.LuckyDrawNumberDAOs = new List<LuckyDrawNumberDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int LuckyDrawStructureIdColumn = StartColumn + columns.IndexOf("LuckyDrawStructureId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                LuckyDrawNumberDAO LuckyDrawNumberDAO = new LuckyDrawNumberDAO();
                LuckyDrawNumberDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawNumberDAO.LuckyDrawStructureId = ExcelWorksheet.Cells[row, LuckyDrawStructureIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawNumberDAOs.Add(LuckyDrawNumberDAO);
            }
            await DataContext.LuckyDrawNumber.BulkMergeAsync(LuckyDrawNumberDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_LuckyDrawRegistration(ExcelWorksheet ExcelWorksheet)
        {
            this.LuckyDrawRegistrationDAOs = new List<LuckyDrawRegistrationDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int LuckyDrawIdColumn = StartColumn + columns.IndexOf("LuckyDrawId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int RevenueColumn = StartColumn + columns.IndexOf("Revenue");
            int TurnCounterColumn = StartColumn + columns.IndexOf("TurnCounter");
            int RemainingTurnCounterColumn = StartColumn + columns.IndexOf("RemainingTurnCounter");
            int IsDrawnByStoreColumn = StartColumn + columns.IndexOf("IsDrawnByStore");
            int TimeColumn = StartColumn + columns.IndexOf("Time");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                LuckyDrawRegistrationDAO LuckyDrawRegistrationDAO = new LuckyDrawRegistrationDAO();
                LuckyDrawRegistrationDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawRegistrationDAO.LuckyDrawId = ExcelWorksheet.Cells[row, LuckyDrawIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawRegistrationDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawRegistrationDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawRegistrationDAO.Revenue = ExcelWorksheet.Cells[row, RevenueColumn].Value?.ParseDecimal() ?? 0;
                LuckyDrawRegistrationDAO.TurnCounter = ExcelWorksheet.Cells[row, TurnCounterColumn].Value?.ParseLong() ?? 0;
                LuckyDrawRegistrationDAO.Time = ExcelWorksheet.Cells[row, TimeColumn].Value?.ParseDateTime() ?? DateTime.Now;
                LuckyDrawRegistrationDAOs.Add(LuckyDrawRegistrationDAO);
            }
            await DataContext.LuckyDrawRegistration.BulkMergeAsync(LuckyDrawRegistrationDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_LuckyDrawStoreGroupingMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.LuckyDrawStoreGroupingMappingDAOs = new List<LuckyDrawStoreGroupingMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int LuckyDrawIdColumn = StartColumn + columns.IndexOf("LuckyDrawId");
            int StoreGroupingIdColumn = StartColumn + columns.IndexOf("StoreGroupingId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                LuckyDrawStoreGroupingMappingDAO LuckyDrawStoreGroupingMappingDAO = new LuckyDrawStoreGroupingMappingDAO();
                LuckyDrawStoreGroupingMappingDAO.LuckyDrawId = ExcelWorksheet.Cells[row, LuckyDrawIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawStoreGroupingMappingDAO.StoreGroupingId = ExcelWorksheet.Cells[row, StoreGroupingIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawStoreGroupingMappingDAOs.Add(LuckyDrawStoreGroupingMappingDAO);
            }
            await DataContext.LuckyDrawStoreGroupingMapping.BulkMergeAsync(LuckyDrawStoreGroupingMappingDAOs);
        }
        protected async Task Given_LuckyDrawStoreMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.LuckyDrawStoreMappingDAOs = new List<LuckyDrawStoreMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int LuckyDrawIdColumn = StartColumn + columns.IndexOf("LuckyDrawId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                LuckyDrawStoreMappingDAO LuckyDrawStoreMappingDAO = new LuckyDrawStoreMappingDAO();
                LuckyDrawStoreMappingDAO.LuckyDrawId = ExcelWorksheet.Cells[row, LuckyDrawIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawStoreMappingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawStoreMappingDAOs.Add(LuckyDrawStoreMappingDAO);
            }
            await DataContext.LuckyDrawStoreMapping.BulkMergeAsync(LuckyDrawStoreMappingDAOs);
        }
        protected async Task Given_LuckyDrawStoreTypeMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.LuckyDrawStoreTypeMappingDAOs = new List<LuckyDrawStoreTypeMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int LuckyDrawIdColumn = StartColumn + columns.IndexOf("LuckyDrawId");
            int StoreTypeIdColumn = StartColumn + columns.IndexOf("StoreTypeId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                LuckyDrawStoreTypeMappingDAO LuckyDrawStoreTypeMappingDAO = new LuckyDrawStoreTypeMappingDAO();
                LuckyDrawStoreTypeMappingDAO.LuckyDrawId = ExcelWorksheet.Cells[row, LuckyDrawIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawStoreTypeMappingDAO.StoreTypeId = ExcelWorksheet.Cells[row, StoreTypeIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawStoreTypeMappingDAOs.Add(LuckyDrawStoreTypeMappingDAO);
            }
            await DataContext.LuckyDrawStoreTypeMapping.BulkMergeAsync(LuckyDrawStoreTypeMappingDAOs);
        }
        protected async Task Given_LuckyDrawStructure(ExcelWorksheet ExcelWorksheet)
        {
            this.LuckyDrawStructureDAOs = new List<LuckyDrawStructureDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int LuckyDrawIdColumn = StartColumn + columns.IndexOf("LuckyDrawId");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                LuckyDrawStructureDAO LuckyDrawStructureDAO = new LuckyDrawStructureDAO();
                LuckyDrawStructureDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawStructureDAO.LuckyDrawId = ExcelWorksheet.Cells[row, LuckyDrawIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawStructureDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                LuckyDrawStructureDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseString();
                LuckyDrawStructureDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                LuckyDrawStructureDAOs.Add(LuckyDrawStructureDAO);
            }
            await DataContext.LuckyDrawStructure.BulkMergeAsync(LuckyDrawStructureDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_LuckyDrawType(ExcelWorksheet ExcelWorksheet)
        {
            this.LuckyDrawTypeDAOs = new List<LuckyDrawTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                LuckyDrawTypeDAO LuckyDrawTypeDAO = new LuckyDrawTypeDAO();
                LuckyDrawTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                LuckyDrawTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                LuckyDrawTypeDAOs.Add(LuckyDrawTypeDAO);
            }
            await DataContext.LuckyDrawType.BulkMergeAsync(LuckyDrawTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_LuckyDrawWinner(ExcelWorksheet ExcelWorksheet)
        {
            this.LuckyDrawWinnerDAOs = new List<LuckyDrawWinnerDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int LuckyDrawIdColumn = StartColumn + columns.IndexOf("LuckyDrawId");
            int LuckyDrawStructureIdColumn = StartColumn + columns.IndexOf("LuckyDrawStructureId");
            int LuckyDrawRegistrationIdColumn = StartColumn + columns.IndexOf("LuckyDrawRegistrationId");
            int TimeColumn = StartColumn + columns.IndexOf("Time");
            int LuckyDrawNumberIdColumn = StartColumn + columns.IndexOf("LuckyDrawNumberId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                LuckyDrawWinnerDAO LuckyDrawWinnerDAO = new LuckyDrawWinnerDAO();
                LuckyDrawWinnerDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawWinnerDAO.LuckyDrawId = ExcelWorksheet.Cells[row, LuckyDrawIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawWinnerDAO.LuckyDrawStructureId = ExcelWorksheet.Cells[row, LuckyDrawStructureIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawWinnerDAO.LuckyDrawRegistrationId = ExcelWorksheet.Cells[row, LuckyDrawRegistrationIdColumn].Value?.ParseLong() ?? 0;
                LuckyDrawWinnerDAO.Time = ExcelWorksheet.Cells[row, TimeColumn].Value?.ParseDateTime() ?? DateTime.Now;
                LuckyDrawWinnerDAO.LuckyDrawNumberId = ExcelWorksheet.Cells[row, LuckyDrawNumberIdColumn].Value?.ParseNullLong();
                LuckyDrawWinnerDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                LuckyDrawWinnerDAOs.Add(LuckyDrawWinnerDAO);
            }
            await DataContext.LuckyDrawWinner.BulkMergeAsync(LuckyDrawWinnerDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_LuckyNumber(ExcelWorksheet ExcelWorksheet)
        {
            this.LuckyNumberDAOs = new List<LuckyNumberDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            int LuckyNumberGroupingIdColumn = StartColumn + columns.IndexOf("LuckyNumberGroupingId");
            int RewardStatusIdColumn = StartColumn + columns.IndexOf("RewardStatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int UsedAtColumn = StartColumn + columns.IndexOf("UsedAt");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                LuckyNumberDAO LuckyNumberDAO = new LuckyNumberDAO();
                LuckyNumberDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                LuckyNumberDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                LuckyNumberDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                LuckyNumberDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseString();
                LuckyNumberDAO.LuckyNumberGroupingId = ExcelWorksheet.Cells[row, LuckyNumberGroupingIdColumn].Value?.ParseLong() ?? 0;
                LuckyNumberDAO.RewardStatusId = ExcelWorksheet.Cells[row, RewardStatusIdColumn].Value?.ParseLong() ?? 0;
                LuckyNumberDAO.UsedAt = ExcelWorksheet.Cells[row, UsedAtColumn].Value?.ParseNullDateTime();
                LuckyNumberDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                LuckyNumberDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                LuckyNumberDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                LuckyNumberDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                LuckyNumberDAOs.Add(LuckyNumberDAO);
            }
            await DataContext.LuckyNumber.BulkMergeAsync(LuckyNumberDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_LuckyNumberGrouping(ExcelWorksheet ExcelWorksheet)
        {
            this.LuckyNumberGroupingDAOs = new List<LuckyNumberGroupingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int StartDateColumn = StartColumn + columns.IndexOf("StartDate");
            int EndDateColumn = StartColumn + columns.IndexOf("EndDate");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                LuckyNumberGroupingDAO LuckyNumberGroupingDAO = new LuckyNumberGroupingDAO();
                LuckyNumberGroupingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                LuckyNumberGroupingDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                LuckyNumberGroupingDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                LuckyNumberGroupingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                LuckyNumberGroupingDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                LuckyNumberGroupingDAO.StartDate = ExcelWorksheet.Cells[row, StartDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                LuckyNumberGroupingDAO.EndDate = ExcelWorksheet.Cells[row, EndDateColumn].Value?.ParseNullDateTime();
                LuckyNumberGroupingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                LuckyNumberGroupingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                LuckyNumberGroupingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                LuckyNumberGroupingDAOs.Add(LuckyNumberGroupingDAO);
            }
            await DataContext.LuckyNumberGrouping.BulkMergeAsync(LuckyNumberGroupingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Nation(ExcelWorksheet ExcelWorksheet)
        {
            this.NationDAOs = new List<NationDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PriorityColumn = StartColumn + columns.IndexOf("Priority");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                NationDAO NationDAO = new NationDAO();
                NationDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                NationDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                NationDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                NationDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseNullLong();
                NationDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                NationDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                NationDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                NationDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                NationDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                NationDAOs.Add(NationDAO);
            }
            await DataContext.Nation.BulkMergeAsync(NationDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Notification(ExcelWorksheet ExcelWorksheet)
        {
            this.NotificationDAOs = new List<NotificationDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int TitleColumn = StartColumn + columns.IndexOf("Title");
            int ContentColumn = StartColumn + columns.IndexOf("Content");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int NotificationStatusIdColumn = StartColumn + columns.IndexOf("NotificationStatusId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                NotificationDAO NotificationDAO = new NotificationDAO();
                NotificationDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                NotificationDAO.Title = ExcelWorksheet.Cells[row, TitleColumn].Value?.ParseString();
                NotificationDAO.Content = ExcelWorksheet.Cells[row, ContentColumn].Value?.ParseString();
                NotificationDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseNullLong();
                NotificationDAO.NotificationStatusId = ExcelWorksheet.Cells[row, NotificationStatusIdColumn].Value?.ParseLong() ?? 0;
                NotificationDAOs.Add(NotificationDAO);
            }
            await DataContext.Notification.BulkMergeAsync(NotificationDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_NotificationStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.NotificationStatusDAOs = new List<NotificationStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                NotificationStatusDAO NotificationStatusDAO = new NotificationStatusDAO();
                NotificationStatusDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                NotificationStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                NotificationStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                NotificationStatusDAOs.Add(NotificationStatusDAO);
            }
            await DataContext.NotificationStatus.BulkMergeAsync(NotificationStatusDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Organization(ExcelWorksheet ExcelWorksheet)
        {
            this.OrganizationDAOs = new List<OrganizationDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ParentIdColumn = StartColumn + columns.IndexOf("ParentId");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int LevelColumn = StartColumn + columns.IndexOf("Level");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int PhoneColumn = StartColumn + columns.IndexOf("Phone");
            int EmailColumn = StartColumn + columns.IndexOf("Email");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int IsDisplayColumn = StartColumn + columns.IndexOf("IsDisplay");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                OrganizationDAO OrganizationDAO = new OrganizationDAO();
                OrganizationDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                OrganizationDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                OrganizationDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                OrganizationDAO.ParentId = ExcelWorksheet.Cells[row, ParentIdColumn].Value?.ParseNullLong();
                OrganizationDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                OrganizationDAO.Level = ExcelWorksheet.Cells[row, LevelColumn].Value?.ParseLong() ?? 0;
                OrganizationDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                OrganizationDAO.Phone = ExcelWorksheet.Cells[row, PhoneColumn].Value?.ParseString();
                OrganizationDAO.Email = ExcelWorksheet.Cells[row, EmailColumn].Value?.ParseString();
                OrganizationDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                OrganizationDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                OrganizationDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                OrganizationDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                OrganizationDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                OrganizationDAOs.Add(OrganizationDAO);
            }
            await DataContext.Organization.BulkMergeAsync(OrganizationDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Position(ExcelWorksheet ExcelWorksheet)
        {
            this.PositionDAOs = new List<PositionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PositionDAO PositionDAO = new PositionDAO();
                PositionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PositionDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                PositionDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PositionDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                PositionDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PositionDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PositionDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PositionDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                PositionDAOs.Add(PositionDAO);
            }
            await DataContext.Position.BulkMergeAsync(PositionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PriceList(ExcelWorksheet ExcelWorksheet)
        {
            this.PriceListDAOs = new List<PriceListDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StartDateColumn = StartColumn + columns.IndexOf("StartDate");
            int EndDateColumn = StartColumn + columns.IndexOf("EndDate");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int PriceListTypeIdColumn = StartColumn + columns.IndexOf("PriceListTypeId");
            int SalesOrderTypeIdColumn = StartColumn + columns.IndexOf("SalesOrderTypeId");
            int RequestStateIdColumn = StartColumn + columns.IndexOf("RequestStateId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PriceListDAO PriceListDAO = new PriceListDAO();
                PriceListDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PriceListDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                PriceListDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PriceListDAO.StartDate = ExcelWorksheet.Cells[row, StartDateColumn].Value?.ParseNullDateTime();
                PriceListDAO.EndDate = ExcelWorksheet.Cells[row, EndDateColumn].Value?.ParseNullDateTime();
                PriceListDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                PriceListDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                PriceListDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                PriceListDAO.PriceListTypeId = ExcelWorksheet.Cells[row, PriceListTypeIdColumn].Value?.ParseLong() ?? 0;
                PriceListDAO.SalesOrderTypeId = ExcelWorksheet.Cells[row, SalesOrderTypeIdColumn].Value?.ParseLong() ?? 0;
                PriceListDAO.RequestStateId = ExcelWorksheet.Cells[row, RequestStateIdColumn].Value?.ParseLong() ?? 0;
                PriceListDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PriceListDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PriceListDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PriceListDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                PriceListDAOs.Add(PriceListDAO);
            }
            await DataContext.PriceList.BulkMergeAsync(PriceListDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PriceListItemHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.PriceListItemHistoryDAOs = new List<PriceListItemHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int PriceListIdColumn = StartColumn + columns.IndexOf("PriceListId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int OldPriceColumn = StartColumn + columns.IndexOf("OldPrice");
            int NewPriceColumn = StartColumn + columns.IndexOf("NewPrice");
            int ModifierIdColumn = StartColumn + columns.IndexOf("ModifierId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PriceListItemHistoryDAO PriceListItemHistoryDAO = new PriceListItemHistoryDAO();
                PriceListItemHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PriceListItemHistoryDAO.PriceListId = ExcelWorksheet.Cells[row, PriceListIdColumn].Value?.ParseLong() ?? 0;
                PriceListItemHistoryDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                PriceListItemHistoryDAO.OldPrice = ExcelWorksheet.Cells[row, OldPriceColumn].Value?.ParseDecimal() ?? 0;
                PriceListItemHistoryDAO.NewPrice = ExcelWorksheet.Cells[row, NewPriceColumn].Value?.ParseDecimal() ?? 0;
                PriceListItemHistoryDAO.ModifierId = ExcelWorksheet.Cells[row, ModifierIdColumn].Value?.ParseLong() ?? 0;
                PriceListItemHistoryDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PriceListItemHistoryDAOs.Add(PriceListItemHistoryDAO);
            }
            await DataContext.PriceListItemHistory.BulkMergeAsync(PriceListItemHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PriceListItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PriceListItemMappingDAOs = new List<PriceListItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PriceListIdColumn = StartColumn + columns.IndexOf("PriceListId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int PriceColumn = StartColumn + columns.IndexOf("Price");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PriceListItemMappingDAO PriceListItemMappingDAO = new PriceListItemMappingDAO();
                PriceListItemMappingDAO.PriceListId = ExcelWorksheet.Cells[row, PriceListIdColumn].Value?.ParseLong() ?? 0;
                PriceListItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                PriceListItemMappingDAO.Price = ExcelWorksheet.Cells[row, PriceColumn].Value?.ParseDecimal() ?? 0;
                PriceListItemMappingDAOs.Add(PriceListItemMappingDAO);
            }
            await DataContext.PriceListItemMapping.BulkMergeAsync(PriceListItemMappingDAOs);
        }
        protected async Task Given_PriceListStoreGroupingMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PriceListStoreGroupingMappingDAOs = new List<PriceListStoreGroupingMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PriceListIdColumn = StartColumn + columns.IndexOf("PriceListId");
            int StoreGroupingIdColumn = StartColumn + columns.IndexOf("StoreGroupingId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PriceListStoreGroupingMappingDAO PriceListStoreGroupingMappingDAO = new PriceListStoreGroupingMappingDAO();
                PriceListStoreGroupingMappingDAO.PriceListId = ExcelWorksheet.Cells[row, PriceListIdColumn].Value?.ParseLong() ?? 0;
                PriceListStoreGroupingMappingDAO.StoreGroupingId = ExcelWorksheet.Cells[row, StoreGroupingIdColumn].Value?.ParseLong() ?? 0;
                PriceListStoreGroupingMappingDAOs.Add(PriceListStoreGroupingMappingDAO);
            }
            await DataContext.PriceListStoreGroupingMapping.BulkMergeAsync(PriceListStoreGroupingMappingDAOs);
        }
        protected async Task Given_PriceListStoreMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PriceListStoreMappingDAOs = new List<PriceListStoreMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PriceListIdColumn = StartColumn + columns.IndexOf("PriceListId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PriceListStoreMappingDAO PriceListStoreMappingDAO = new PriceListStoreMappingDAO();
                PriceListStoreMappingDAO.PriceListId = ExcelWorksheet.Cells[row, PriceListIdColumn].Value?.ParseLong() ?? 0;
                PriceListStoreMappingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                PriceListStoreMappingDAOs.Add(PriceListStoreMappingDAO);
            }
            await DataContext.PriceListStoreMapping.BulkMergeAsync(PriceListStoreMappingDAOs);
        }
        protected async Task Given_PriceListStoreTypeMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PriceListStoreTypeMappingDAOs = new List<PriceListStoreTypeMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PriceListIdColumn = StartColumn + columns.IndexOf("PriceListId");
            int StoreTypeIdColumn = StartColumn + columns.IndexOf("StoreTypeId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PriceListStoreTypeMappingDAO PriceListStoreTypeMappingDAO = new PriceListStoreTypeMappingDAO();
                PriceListStoreTypeMappingDAO.PriceListId = ExcelWorksheet.Cells[row, PriceListIdColumn].Value?.ParseLong() ?? 0;
                PriceListStoreTypeMappingDAO.StoreTypeId = ExcelWorksheet.Cells[row, StoreTypeIdColumn].Value?.ParseLong() ?? 0;
                PriceListStoreTypeMappingDAOs.Add(PriceListStoreTypeMappingDAO);
            }
            await DataContext.PriceListStoreTypeMapping.BulkMergeAsync(PriceListStoreTypeMappingDAOs);
        }
        protected async Task Given_PriceListType(ExcelWorksheet ExcelWorksheet)
        {
            this.PriceListTypeDAOs = new List<PriceListTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PriceListTypeDAO PriceListTypeDAO = new PriceListTypeDAO();
                PriceListTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PriceListTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                PriceListTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PriceListTypeDAOs.Add(PriceListTypeDAO);
            }
            await DataContext.PriceListType.BulkMergeAsync(PriceListTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Problem(ExcelWorksheet ExcelWorksheet)
        {
            this.ProblemDAOs = new List<ProblemDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int StoreCheckingIdColumn = StartColumn + columns.IndexOf("StoreCheckingId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int ProblemTypeIdColumn = StartColumn + columns.IndexOf("ProblemTypeId");
            int NoteAtColumn = StartColumn + columns.IndexOf("NoteAt");
            int CompletedAtColumn = StartColumn + columns.IndexOf("CompletedAt");
            int ContentColumn = StartColumn + columns.IndexOf("Content");
            int ProblemStatusIdColumn = StartColumn + columns.IndexOf("ProblemStatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProblemDAO ProblemDAO = new ProblemDAO();
                ProblemDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ProblemDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ProblemDAO.StoreCheckingId = ExcelWorksheet.Cells[row, StoreCheckingIdColumn].Value?.ParseNullLong();
                ProblemDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                ProblemDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                ProblemDAO.ProblemTypeId = ExcelWorksheet.Cells[row, ProblemTypeIdColumn].Value?.ParseLong() ?? 0;
                ProblemDAO.NoteAt = ExcelWorksheet.Cells[row, NoteAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProblemDAO.CompletedAt = ExcelWorksheet.Cells[row, CompletedAtColumn].Value?.ParseNullDateTime();
                ProblemDAO.Content = ExcelWorksheet.Cells[row, ContentColumn].Value?.ParseString();
                ProblemDAO.ProblemStatusId = ExcelWorksheet.Cells[row, ProblemStatusIdColumn].Value?.ParseLong() ?? 0;
                ProblemDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                ProblemDAOs.Add(ProblemDAO);
            }
            await DataContext.Problem.BulkMergeAsync(ProblemDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ProblemHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.ProblemHistoryDAOs = new List<ProblemHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int ProblemIdColumn = StartColumn + columns.IndexOf("ProblemId");
            int TimeColumn = StartColumn + columns.IndexOf("Time");
            int ModifierIdColumn = StartColumn + columns.IndexOf("ModifierId");
            int ProblemStatusIdColumn = StartColumn + columns.IndexOf("ProblemStatusId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProblemHistoryDAO ProblemHistoryDAO = new ProblemHistoryDAO();
                ProblemHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ProblemHistoryDAO.ProblemId = ExcelWorksheet.Cells[row, ProblemIdColumn].Value?.ParseLong() ?? 0;
                ProblemHistoryDAO.Time = ExcelWorksheet.Cells[row, TimeColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProblemHistoryDAO.ModifierId = ExcelWorksheet.Cells[row, ModifierIdColumn].Value?.ParseLong() ?? 0;
                ProblemHistoryDAO.ProblemStatusId = ExcelWorksheet.Cells[row, ProblemStatusIdColumn].Value?.ParseLong() ?? 0;
                ProblemHistoryDAOs.Add(ProblemHistoryDAO);
            }
            await DataContext.ProblemHistory.BulkMergeAsync(ProblemHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ProblemImageMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.ProblemImageMappingDAOs = new List<ProblemImageMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ProblemIdColumn = StartColumn + columns.IndexOf("ProblemId");
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProblemImageMappingDAO ProblemImageMappingDAO = new ProblemImageMappingDAO();
                ProblemImageMappingDAO.ProblemId = ExcelWorksheet.Cells[row, ProblemIdColumn].Value?.ParseLong() ?? 0;
                ProblemImageMappingDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseLong() ?? 0;
                ProblemImageMappingDAOs.Add(ProblemImageMappingDAO);
            }
            await DataContext.ProblemImageMapping.BulkMergeAsync(ProblemImageMappingDAOs);
        }
        protected async Task Given_ProblemStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.ProblemStatusDAOs = new List<ProblemStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProblemStatusDAO ProblemStatusDAO = new ProblemStatusDAO();
                ProblemStatusDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ProblemStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ProblemStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ProblemStatusDAOs.Add(ProblemStatusDAO);
            }
            await DataContext.ProblemStatus.BulkMergeAsync(ProblemStatusDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ProblemType(ExcelWorksheet ExcelWorksheet)
        {
            this.ProblemTypeDAOs = new List<ProblemTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProblemTypeDAO ProblemTypeDAO = new ProblemTypeDAO();
                ProblemTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ProblemTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ProblemTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ProblemTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                ProblemTypeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProblemTypeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProblemTypeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                ProblemTypeDAOs.Add(ProblemTypeDAO);
            }
            await DataContext.ProblemType.BulkMergeAsync(ProblemTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Product(ExcelWorksheet ExcelWorksheet)
        {
            this.ProductDAOs = new List<ProductDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int ScanCodeColumn = StartColumn + columns.IndexOf("ScanCode");
            int ERPCodeColumn = StartColumn + columns.IndexOf("ERPCode");
            int CategoryIdColumn = StartColumn + columns.IndexOf("CategoryId");
            int ProductTypeIdColumn = StartColumn + columns.IndexOf("ProductTypeId");
            int BrandIdColumn = StartColumn + columns.IndexOf("BrandId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int UnitOfMeasureGroupingIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureGroupingId");
            int SalePriceColumn = StartColumn + columns.IndexOf("SalePrice");
            int RetailPriceColumn = StartColumn + columns.IndexOf("RetailPrice");
            int TaxTypeIdColumn = StartColumn + columns.IndexOf("TaxTypeId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int OtherNameColumn = StartColumn + columns.IndexOf("OtherName");
            int TechnicalNameColumn = StartColumn + columns.IndexOf("TechnicalName");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int IsNewColumn = StartColumn + columns.IndexOf("IsNew");
            int UsedVariationIdColumn = StartColumn + columns.IndexOf("UsedVariationId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int SalesOrderCounterColumn = StartColumn + columns.IndexOf("SalesOrderCounter");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProductDAO ProductDAO = new ProductDAO();
                ProductDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ProductDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ProductDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ProductDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                ProductDAO.ScanCode = ExcelWorksheet.Cells[row, ScanCodeColumn].Value?.ParseString();
                ProductDAO.ERPCode = ExcelWorksheet.Cells[row, ERPCodeColumn].Value?.ParseString();
                ProductDAO.CategoryId = ExcelWorksheet.Cells[row, CategoryIdColumn].Value?.ParseLong() ?? 0;
                ProductDAO.ProductTypeId = ExcelWorksheet.Cells[row, ProductTypeIdColumn].Value?.ParseLong() ?? 0;
                ProductDAO.BrandId = ExcelWorksheet.Cells[row, BrandIdColumn].Value?.ParseNullLong();
                ProductDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                ProductDAO.UnitOfMeasureGroupingId = ExcelWorksheet.Cells[row, UnitOfMeasureGroupingIdColumn].Value?.ParseNullLong();
                ProductDAO.SalePrice = ExcelWorksheet.Cells[row, SalePriceColumn].Value?.ParseNullDecimal();
                ProductDAO.RetailPrice = ExcelWorksheet.Cells[row, RetailPriceColumn].Value?.ParseNullDecimal();
                ProductDAO.TaxTypeId = ExcelWorksheet.Cells[row, TaxTypeIdColumn].Value?.ParseLong() ?? 0;
                ProductDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                ProductDAO.OtherName = ExcelWorksheet.Cells[row, OtherNameColumn].Value?.ParseString();
                ProductDAO.TechnicalName = ExcelWorksheet.Cells[row, TechnicalNameColumn].Value?.ParseString();
                ProductDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                ProductDAO.UsedVariationId = ExcelWorksheet.Cells[row, UsedVariationIdColumn].Value?.ParseLong() ?? 0;
                ProductDAO.SalesOrderCounter = ExcelWorksheet.Cells[row, SalesOrderCounterColumn].Value?.ParseNullLong();
                ProductDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                ProductDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProductDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProductDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                ProductDAOs.Add(ProductDAO);
            }
            await DataContext.Product.BulkMergeAsync(ProductDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ProductGrouping(ExcelWorksheet ExcelWorksheet)
        {
            this.ProductGroupingDAOs = new List<ProductGroupingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int ParentIdColumn = StartColumn + columns.IndexOf("ParentId");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int LevelColumn = StartColumn + columns.IndexOf("Level");
            int HasChildrenColumn = StartColumn + columns.IndexOf("HasChildren");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProductGroupingDAO ProductGroupingDAO = new ProductGroupingDAO();
                ProductGroupingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ProductGroupingDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ProductGroupingDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ProductGroupingDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                ProductGroupingDAO.ParentId = ExcelWorksheet.Cells[row, ParentIdColumn].Value?.ParseNullLong();
                ProductGroupingDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                ProductGroupingDAO.Level = ExcelWorksheet.Cells[row, LevelColumn].Value?.ParseLong() ?? 0;
                ProductGroupingDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                ProductGroupingDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                ProductGroupingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProductGroupingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProductGroupingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                ProductGroupingDAOs.Add(ProductGroupingDAO);
            }
            await DataContext.ProductGrouping.BulkMergeAsync(ProductGroupingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ProductGroupingHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.ProductGroupingHistoryDAOs = new List<ProductGroupingHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int ProductGroupingIdColumn = StartColumn + columns.IndexOf("ProductGroupingId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProductGroupingHistoryDAO ProductGroupingHistoryDAO = new ProductGroupingHistoryDAO();
                ProductGroupingHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ProductGroupingHistoryDAO.ProductGroupingId = ExcelWorksheet.Cells[row, ProductGroupingIdColumn].Value?.ParseLong() ?? 0;
                ProductGroupingHistoryDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ProductGroupingHistoryDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ProductGroupingHistoryDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                ProductGroupingHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseNullLong();
                ProductGroupingHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProductGroupingHistoryDAOs.Add(ProductGroupingHistoryDAO);
            }
            await DataContext.ProductGroupingHistory.BulkMergeAsync(ProductGroupingHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ProductImageMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.ProductImageMappingDAOs = new List<ProductImageMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ProductIdColumn = StartColumn + columns.IndexOf("ProductId");
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProductImageMappingDAO ProductImageMappingDAO = new ProductImageMappingDAO();
                ProductImageMappingDAO.ProductId = ExcelWorksheet.Cells[row, ProductIdColumn].Value?.ParseLong() ?? 0;
                ProductImageMappingDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseLong() ?? 0;
                ProductImageMappingDAOs.Add(ProductImageMappingDAO);
            }
            await DataContext.ProductImageMapping.BulkMergeAsync(ProductImageMappingDAOs);
        }
        protected async Task Given_ProductProductGroupingMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.ProductProductGroupingMappingDAOs = new List<ProductProductGroupingMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ProductIdColumn = StartColumn + columns.IndexOf("ProductId");
            int ProductGroupingIdColumn = StartColumn + columns.IndexOf("ProductGroupingId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProductProductGroupingMappingDAO ProductProductGroupingMappingDAO = new ProductProductGroupingMappingDAO();
                ProductProductGroupingMappingDAO.ProductId = ExcelWorksheet.Cells[row, ProductIdColumn].Value?.ParseLong() ?? 0;
                ProductProductGroupingMappingDAO.ProductGroupingId = ExcelWorksheet.Cells[row, ProductGroupingIdColumn].Value?.ParseLong() ?? 0;
                ProductProductGroupingMappingDAOs.Add(ProductProductGroupingMappingDAO);
            }
            await DataContext.ProductProductGroupingMapping.BulkMergeAsync(ProductProductGroupingMappingDAOs);
        }
        protected async Task Given_ProductType(ExcelWorksheet ExcelWorksheet)
        {
            this.ProductTypeDAOs = new List<ProductTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProductTypeDAO ProductTypeDAO = new ProductTypeDAO();
                ProductTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ProductTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ProductTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ProductTypeDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                ProductTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                ProductTypeDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                ProductTypeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProductTypeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProductTypeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                ProductTypeDAOs.Add(ProductTypeDAO);
            }
            await DataContext.ProductType.BulkMergeAsync(ProductTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionCode(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionCodeDAOs = new List<PromotionCodeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            int PromotionDiscountTypeIdColumn = StartColumn + columns.IndexOf("PromotionDiscountTypeId");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            int MaxValueColumn = StartColumn + columns.IndexOf("MaxValue");
            int PromotionTypeIdColumn = StartColumn + columns.IndexOf("PromotionTypeId");
            int PromotionProductAppliedTypeIdColumn = StartColumn + columns.IndexOf("PromotionProductAppliedTypeId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int StartDateColumn = StartColumn + columns.IndexOf("StartDate");
            int EndDateColumn = StartColumn + columns.IndexOf("EndDate");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionCodeDAO PromotionCodeDAO = new PromotionCodeDAO();
                PromotionCodeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                PromotionCodeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PromotionCodeDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseNullLong();
                PromotionCodeDAO.PromotionDiscountTypeId = ExcelWorksheet.Cells[row, PromotionDiscountTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionCodeDAO.MaxValue = ExcelWorksheet.Cells[row, MaxValueColumn].Value?.ParseNullDecimal();
                PromotionCodeDAO.PromotionTypeId = ExcelWorksheet.Cells[row, PromotionTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeDAO.PromotionProductAppliedTypeId = ExcelWorksheet.Cells[row, PromotionProductAppliedTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeDAO.StartDate = ExcelWorksheet.Cells[row, StartDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PromotionCodeDAO.EndDate = ExcelWorksheet.Cells[row, EndDateColumn].Value?.ParseNullDateTime();
                PromotionCodeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PromotionCodeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PromotionCodeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                PromotionCodeDAOs.Add(PromotionCodeDAO);
            }
            await DataContext.PromotionCode.BulkMergeAsync(PromotionCodeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionCodeHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionCodeHistoryDAOs = new List<PromotionCodeHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int PromotionCodeIdColumn = StartColumn + columns.IndexOf("PromotionCodeId");
            int AppliedAtColumn = StartColumn + columns.IndexOf("AppliedAt");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionCodeHistoryDAO PromotionCodeHistoryDAO = new PromotionCodeHistoryDAO();
                PromotionCodeHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeHistoryDAO.PromotionCodeId = ExcelWorksheet.Cells[row, PromotionCodeIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeHistoryDAO.AppliedAt = ExcelWorksheet.Cells[row, AppliedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PromotionCodeHistoryDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PromotionCodeHistoryDAOs.Add(PromotionCodeHistoryDAO);
            }
            await DataContext.PromotionCodeHistory.BulkMergeAsync(PromotionCodeHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionCodeOrganizationMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionCodeOrganizationMappingDAOs = new List<PromotionCodeOrganizationMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionCodeIdColumn = StartColumn + columns.IndexOf("PromotionCodeId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionCodeOrganizationMappingDAO PromotionCodeOrganizationMappingDAO = new PromotionCodeOrganizationMappingDAO();
                PromotionCodeOrganizationMappingDAO.PromotionCodeId = ExcelWorksheet.Cells[row, PromotionCodeIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeOrganizationMappingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeOrganizationMappingDAOs.Add(PromotionCodeOrganizationMappingDAO);
            }
            await DataContext.PromotionCodeOrganizationMapping.BulkMergeAsync(PromotionCodeOrganizationMappingDAOs);
        }
        protected async Task Given_PromotionCodeProductMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionCodeProductMappingDAOs = new List<PromotionCodeProductMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionCodeIdColumn = StartColumn + columns.IndexOf("PromotionCodeId");
            int ProductIdColumn = StartColumn + columns.IndexOf("ProductId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionCodeProductMappingDAO PromotionCodeProductMappingDAO = new PromotionCodeProductMappingDAO();
                PromotionCodeProductMappingDAO.PromotionCodeId = ExcelWorksheet.Cells[row, PromotionCodeIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeProductMappingDAO.ProductId = ExcelWorksheet.Cells[row, ProductIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeProductMappingDAOs.Add(PromotionCodeProductMappingDAO);
            }
            await DataContext.PromotionCodeProductMapping.BulkMergeAsync(PromotionCodeProductMappingDAOs);
        }
        protected async Task Given_PromotionCodeStoreMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionCodeStoreMappingDAOs = new List<PromotionCodeStoreMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionCodeIdColumn = StartColumn + columns.IndexOf("PromotionCodeId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionCodeStoreMappingDAO PromotionCodeStoreMappingDAO = new PromotionCodeStoreMappingDAO();
                PromotionCodeStoreMappingDAO.PromotionCodeId = ExcelWorksheet.Cells[row, PromotionCodeIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeStoreMappingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                PromotionCodeStoreMappingDAOs.Add(PromotionCodeStoreMappingDAO);
            }
            await DataContext.PromotionCodeStoreMapping.BulkMergeAsync(PromotionCodeStoreMappingDAOs);
        }
        protected async Task Given_PromotionCombo(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionComboDAOs = new List<PromotionComboDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PromotionPolicyIdColumn = StartColumn + columns.IndexOf("PromotionPolicyId");
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int PromotionDiscountTypeIdColumn = StartColumn + columns.IndexOf("PromotionDiscountTypeId");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int DiscountValueColumn = StartColumn + columns.IndexOf("DiscountValue");
            int PriceColumn = StartColumn + columns.IndexOf("Price");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionComboDAO PromotionComboDAO = new PromotionComboDAO();
                PromotionComboDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionComboDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PromotionComboDAO.PromotionPolicyId = ExcelWorksheet.Cells[row, PromotionPolicyIdColumn].Value?.ParseLong() ?? 0;
                PromotionComboDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionComboDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                PromotionComboDAO.PromotionDiscountTypeId = ExcelWorksheet.Cells[row, PromotionDiscountTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionComboDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                PromotionComboDAO.DiscountValue = ExcelWorksheet.Cells[row, DiscountValueColumn].Value?.ParseNullDecimal();
                PromotionComboDAO.Price = ExcelWorksheet.Cells[row, PriceColumn].Value?.ParseNullDecimal();
                PromotionComboDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PromotionComboDAOs.Add(PromotionComboDAO);
            }
            await DataContext.PromotionCombo.BulkMergeAsync(PromotionComboDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionComboInItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionComboInItemMappingDAOs = new List<PromotionComboInItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionComboIdColumn = StartColumn + columns.IndexOf("PromotionComboId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int FromColumn = StartColumn + columns.IndexOf("From");
            int ToColumn = StartColumn + columns.IndexOf("To");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionComboInItemMappingDAO PromotionComboInItemMappingDAO = new PromotionComboInItemMappingDAO();
                PromotionComboInItemMappingDAO.PromotionComboId = ExcelWorksheet.Cells[row, PromotionComboIdColumn].Value?.ParseLong() ?? 0;
                PromotionComboInItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                PromotionComboInItemMappingDAO.From = ExcelWorksheet.Cells[row, FromColumn].Value?.ParseLong() ?? 0;
                PromotionComboInItemMappingDAO.To = ExcelWorksheet.Cells[row, ToColumn].Value?.ParseNullLong();
                PromotionComboInItemMappingDAOs.Add(PromotionComboInItemMappingDAO);
            }
            await DataContext.PromotionComboInItemMapping.BulkMergeAsync(PromotionComboInItemMappingDAOs);
        }
        protected async Task Given_PromotionComboOutItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionComboOutItemMappingDAOs = new List<PromotionComboOutItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionComboIdColumn = StartColumn + columns.IndexOf("PromotionComboId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionComboOutItemMappingDAO PromotionComboOutItemMappingDAO = new PromotionComboOutItemMappingDAO();
                PromotionComboOutItemMappingDAO.PromotionComboId = ExcelWorksheet.Cells[row, PromotionComboIdColumn].Value?.ParseLong() ?? 0;
                PromotionComboOutItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                PromotionComboOutItemMappingDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                PromotionComboOutItemMappingDAOs.Add(PromotionComboOutItemMappingDAO);
            }
            await DataContext.PromotionComboOutItemMapping.BulkMergeAsync(PromotionComboOutItemMappingDAOs);
        }
        protected async Task Given_Promotion(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionDAOs = new List<PromotionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StartDateColumn = StartColumn + columns.IndexOf("StartDate");
            int EndDateColumn = StartColumn + columns.IndexOf("EndDate");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int PromotionTypeIdColumn = StartColumn + columns.IndexOf("PromotionTypeId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int PriorityColumn = StartColumn + columns.IndexOf("Priority");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionDAO PromotionDAO = new PromotionDAO();
                PromotionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                PromotionDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PromotionDAO.StartDate = ExcelWorksheet.Cells[row, StartDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PromotionDAO.EndDate = ExcelWorksheet.Cells[row, EndDateColumn].Value?.ParseNullDateTime();
                PromotionDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                PromotionDAO.PromotionTypeId = ExcelWorksheet.Cells[row, PromotionTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                PromotionDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseLong() ?? 0;
                PromotionDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                PromotionDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PromotionDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PromotionDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                PromotionDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                PromotionDAOs.Add(PromotionDAO);
            }
            await DataContext.Promotion.BulkMergeAsync(PromotionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionDirectSalesOrder(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionDirectSalesOrderDAOs = new List<PromotionDirectSalesOrderDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int PromotionPolicyIdColumn = StartColumn + columns.IndexOf("PromotionPolicyId");
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int FromValueColumn = StartColumn + columns.IndexOf("FromValue");
            int ToValueColumn = StartColumn + columns.IndexOf("ToValue");
            int PromotionDiscountTypeIdColumn = StartColumn + columns.IndexOf("PromotionDiscountTypeId");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int DiscountValueColumn = StartColumn + columns.IndexOf("DiscountValue");
            int PriceColumn = StartColumn + columns.IndexOf("Price");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionDirectSalesOrderDAO PromotionDirectSalesOrderDAO = new PromotionDirectSalesOrderDAO();
                PromotionDirectSalesOrderDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionDirectSalesOrderDAO.PromotionPolicyId = ExcelWorksheet.Cells[row, PromotionPolicyIdColumn].Value?.ParseLong() ?? 0;
                PromotionDirectSalesOrderDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionDirectSalesOrderDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                PromotionDirectSalesOrderDAO.FromValue = ExcelWorksheet.Cells[row, FromValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionDirectSalesOrderDAO.ToValue = ExcelWorksheet.Cells[row, ToValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionDirectSalesOrderDAO.PromotionDiscountTypeId = ExcelWorksheet.Cells[row, PromotionDiscountTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionDirectSalesOrderDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                PromotionDirectSalesOrderDAO.DiscountValue = ExcelWorksheet.Cells[row, DiscountValueColumn].Value?.ParseNullDecimal();
                PromotionDirectSalesOrderDAO.Price = ExcelWorksheet.Cells[row, PriceColumn].Value?.ParseNullDecimal();
                PromotionDirectSalesOrderDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PromotionDirectSalesOrderDAOs.Add(PromotionDirectSalesOrderDAO);
            }
            await DataContext.PromotionDirectSalesOrder.BulkMergeAsync(PromotionDirectSalesOrderDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionDirectSalesOrderItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionDirectSalesOrderItemMappingDAOs = new List<PromotionDirectSalesOrderItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionDirectSalesOrderIdColumn = StartColumn + columns.IndexOf("PromotionDirectSalesOrderId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionDirectSalesOrderItemMappingDAO PromotionDirectSalesOrderItemMappingDAO = new PromotionDirectSalesOrderItemMappingDAO();
                PromotionDirectSalesOrderItemMappingDAO.PromotionDirectSalesOrderId = ExcelWorksheet.Cells[row, PromotionDirectSalesOrderIdColumn].Value?.ParseLong() ?? 0;
                PromotionDirectSalesOrderItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                PromotionDirectSalesOrderItemMappingDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                PromotionDirectSalesOrderItemMappingDAOs.Add(PromotionDirectSalesOrderItemMappingDAO);
            }
            await DataContext.PromotionDirectSalesOrderItemMapping.BulkMergeAsync(PromotionDirectSalesOrderItemMappingDAOs);
        }
        protected async Task Given_PromotionDiscountType(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionDiscountTypeDAOs = new List<PromotionDiscountTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionDiscountTypeDAO PromotionDiscountTypeDAO = new PromotionDiscountTypeDAO();
                PromotionDiscountTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionDiscountTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                PromotionDiscountTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PromotionDiscountTypeDAOs.Add(PromotionDiscountTypeDAO);
            }
            await DataContext.PromotionDiscountType.BulkMergeAsync(PromotionDiscountTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionPolicy(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionPolicyDAOs = new List<PromotionPolicyDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionPolicyDAO PromotionPolicyDAO = new PromotionPolicyDAO();
                PromotionPolicyDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionPolicyDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                PromotionPolicyDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PromotionPolicyDAOs.Add(PromotionPolicyDAO);
            }
            await DataContext.PromotionPolicy.BulkMergeAsync(PromotionPolicyDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionProductAppliedType(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionProductAppliedTypeDAOs = new List<PromotionProductAppliedTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionProductAppliedTypeDAO PromotionProductAppliedTypeDAO = new PromotionProductAppliedTypeDAO();
                PromotionProductAppliedTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionProductAppliedTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                PromotionProductAppliedTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PromotionProductAppliedTypeDAOs.Add(PromotionProductAppliedTypeDAO);
            }
            await DataContext.PromotionProductAppliedType.BulkMergeAsync(PromotionProductAppliedTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionProduct(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionProductDAOs = new List<PromotionProductDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int PromotionPolicyIdColumn = StartColumn + columns.IndexOf("PromotionPolicyId");
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int ProductIdColumn = StartColumn + columns.IndexOf("ProductId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int FromValueColumn = StartColumn + columns.IndexOf("FromValue");
            int ToValueColumn = StartColumn + columns.IndexOf("ToValue");
            int PromotionDiscountTypeIdColumn = StartColumn + columns.IndexOf("PromotionDiscountTypeId");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int DiscountValueColumn = StartColumn + columns.IndexOf("DiscountValue");
            int PriceColumn = StartColumn + columns.IndexOf("Price");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionProductDAO PromotionProductDAO = new PromotionProductDAO();
                PromotionProductDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionProductDAO.PromotionPolicyId = ExcelWorksheet.Cells[row, PromotionPolicyIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductDAO.ProductId = ExcelWorksheet.Cells[row, ProductIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                PromotionProductDAO.FromValue = ExcelWorksheet.Cells[row, FromValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionProductDAO.ToValue = ExcelWorksheet.Cells[row, ToValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionProductDAO.PromotionDiscountTypeId = ExcelWorksheet.Cells[row, PromotionDiscountTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                PromotionProductDAO.DiscountValue = ExcelWorksheet.Cells[row, DiscountValueColumn].Value?.ParseNullDecimal();
                PromotionProductDAO.Price = ExcelWorksheet.Cells[row, PriceColumn].Value?.ParseNullDecimal();
                PromotionProductDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PromotionProductDAOs.Add(PromotionProductDAO);
            }
            await DataContext.PromotionProduct.BulkMergeAsync(PromotionProductDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionProductGrouping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionProductGroupingDAOs = new List<PromotionProductGroupingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int PromotionPolicyIdColumn = StartColumn + columns.IndexOf("PromotionPolicyId");
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int ProductGroupingIdColumn = StartColumn + columns.IndexOf("ProductGroupingId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int FromValueColumn = StartColumn + columns.IndexOf("FromValue");
            int ToValueColumn = StartColumn + columns.IndexOf("ToValue");
            int PromotionDiscountTypeIdColumn = StartColumn + columns.IndexOf("PromotionDiscountTypeId");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int DiscountValueColumn = StartColumn + columns.IndexOf("DiscountValue");
            int PriceColumn = StartColumn + columns.IndexOf("Price");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionProductGroupingDAO PromotionProductGroupingDAO = new PromotionProductGroupingDAO();
                PromotionProductGroupingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionProductGroupingDAO.PromotionPolicyId = ExcelWorksheet.Cells[row, PromotionPolicyIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductGroupingDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductGroupingDAO.ProductGroupingId = ExcelWorksheet.Cells[row, ProductGroupingIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductGroupingDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                PromotionProductGroupingDAO.FromValue = ExcelWorksheet.Cells[row, FromValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionProductGroupingDAO.ToValue = ExcelWorksheet.Cells[row, ToValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionProductGroupingDAO.PromotionDiscountTypeId = ExcelWorksheet.Cells[row, PromotionDiscountTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductGroupingDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                PromotionProductGroupingDAO.DiscountValue = ExcelWorksheet.Cells[row, DiscountValueColumn].Value?.ParseNullDecimal();
                PromotionProductGroupingDAO.Price = ExcelWorksheet.Cells[row, PriceColumn].Value?.ParseNullDecimal();
                PromotionProductGroupingDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PromotionProductGroupingDAOs.Add(PromotionProductGroupingDAO);
            }
            await DataContext.PromotionProductGrouping.BulkMergeAsync(PromotionProductGroupingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionProductGroupingItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionProductGroupingItemMappingDAOs = new List<PromotionProductGroupingItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionProductGroupingIdColumn = StartColumn + columns.IndexOf("PromotionProductGroupingId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionProductGroupingItemMappingDAO PromotionProductGroupingItemMappingDAO = new PromotionProductGroupingItemMappingDAO();
                PromotionProductGroupingItemMappingDAO.PromotionProductGroupingId = ExcelWorksheet.Cells[row, PromotionProductGroupingIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductGroupingItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductGroupingItemMappingDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                PromotionProductGroupingItemMappingDAOs.Add(PromotionProductGroupingItemMappingDAO);
            }
            await DataContext.PromotionProductGroupingItemMapping.BulkMergeAsync(PromotionProductGroupingItemMappingDAOs);
        }
        protected async Task Given_PromotionProductItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionProductItemMappingDAOs = new List<PromotionProductItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionProductIdColumn = StartColumn + columns.IndexOf("PromotionProductId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionProductItemMappingDAO PromotionProductItemMappingDAO = new PromotionProductItemMappingDAO();
                PromotionProductItemMappingDAO.PromotionProductId = ExcelWorksheet.Cells[row, PromotionProductIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductItemMappingDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                PromotionProductItemMappingDAOs.Add(PromotionProductItemMappingDAO);
            }
            await DataContext.PromotionProductItemMapping.BulkMergeAsync(PromotionProductItemMappingDAOs);
        }
        protected async Task Given_PromotionProductType(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionProductTypeDAOs = new List<PromotionProductTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int PromotionPolicyIdColumn = StartColumn + columns.IndexOf("PromotionPolicyId");
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int ProductTypeIdColumn = StartColumn + columns.IndexOf("ProductTypeId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int FromValueColumn = StartColumn + columns.IndexOf("FromValue");
            int ToValueColumn = StartColumn + columns.IndexOf("ToValue");
            int PromotionDiscountTypeIdColumn = StartColumn + columns.IndexOf("PromotionDiscountTypeId");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int DiscountValueColumn = StartColumn + columns.IndexOf("DiscountValue");
            int PriceColumn = StartColumn + columns.IndexOf("Price");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionProductTypeDAO PromotionProductTypeDAO = new PromotionProductTypeDAO();
                PromotionProductTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionProductTypeDAO.PromotionPolicyId = ExcelWorksheet.Cells[row, PromotionPolicyIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductTypeDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductTypeDAO.ProductTypeId = ExcelWorksheet.Cells[row, ProductTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductTypeDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                PromotionProductTypeDAO.FromValue = ExcelWorksheet.Cells[row, FromValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionProductTypeDAO.ToValue = ExcelWorksheet.Cells[row, ToValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionProductTypeDAO.PromotionDiscountTypeId = ExcelWorksheet.Cells[row, PromotionDiscountTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductTypeDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                PromotionProductTypeDAO.DiscountValue = ExcelWorksheet.Cells[row, DiscountValueColumn].Value?.ParseNullDecimal();
                PromotionProductTypeDAO.Price = ExcelWorksheet.Cells[row, PriceColumn].Value?.ParseNullDecimal();
                PromotionProductTypeDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PromotionProductTypeDAOs.Add(PromotionProductTypeDAO);
            }
            await DataContext.PromotionProductType.BulkMergeAsync(PromotionProductTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionProductTypeItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionProductTypeItemMappingDAOs = new List<PromotionProductTypeItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionProductTypeIdColumn = StartColumn + columns.IndexOf("PromotionProductTypeId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionProductTypeItemMappingDAO PromotionProductTypeItemMappingDAO = new PromotionProductTypeItemMappingDAO();
                PromotionProductTypeItemMappingDAO.PromotionProductTypeId = ExcelWorksheet.Cells[row, PromotionProductTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductTypeItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                PromotionProductTypeItemMappingDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                PromotionProductTypeItemMappingDAOs.Add(PromotionProductTypeItemMappingDAO);
            }
            await DataContext.PromotionProductTypeItemMapping.BulkMergeAsync(PromotionProductTypeItemMappingDAOs);
        }
        protected async Task Given_PromotionPromotionPolicyMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionPromotionPolicyMappingDAOs = new List<PromotionPromotionPolicyMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int PromotionPolicyIdColumn = StartColumn + columns.IndexOf("PromotionPolicyId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionPromotionPolicyMappingDAO PromotionPromotionPolicyMappingDAO = new PromotionPromotionPolicyMappingDAO();
                PromotionPromotionPolicyMappingDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionPromotionPolicyMappingDAO.PromotionPolicyId = ExcelWorksheet.Cells[row, PromotionPolicyIdColumn].Value?.ParseLong() ?? 0;
                PromotionPromotionPolicyMappingDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                PromotionPromotionPolicyMappingDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                PromotionPromotionPolicyMappingDAOs.Add(PromotionPromotionPolicyMappingDAO);
            }
            await DataContext.PromotionPromotionPolicyMapping.BulkMergeAsync(PromotionPromotionPolicyMappingDAOs);
        }
        protected async Task Given_PromotionSamePrice(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionSamePriceDAOs = new List<PromotionSamePriceDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int PromotionPolicyIdColumn = StartColumn + columns.IndexOf("PromotionPolicyId");
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int PriceColumn = StartColumn + columns.IndexOf("Price");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionSamePriceDAO PromotionSamePriceDAO = new PromotionSamePriceDAO();
                PromotionSamePriceDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionSamePriceDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                PromotionSamePriceDAO.PromotionPolicyId = ExcelWorksheet.Cells[row, PromotionPolicyIdColumn].Value?.ParseLong() ?? 0;
                PromotionSamePriceDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionSamePriceDAO.Price = ExcelWorksheet.Cells[row, PriceColumn].Value?.ParseDecimal() ?? 0;
                PromotionSamePriceDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PromotionSamePriceDAOs.Add(PromotionSamePriceDAO);
            }
            await DataContext.PromotionSamePrice.BulkMergeAsync(PromotionSamePriceDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionStore(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionStoreDAOs = new List<PromotionStoreDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int PromotionPolicyIdColumn = StartColumn + columns.IndexOf("PromotionPolicyId");
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int FromValueColumn = StartColumn + columns.IndexOf("FromValue");
            int ToValueColumn = StartColumn + columns.IndexOf("ToValue");
            int PromotionDiscountTypeIdColumn = StartColumn + columns.IndexOf("PromotionDiscountTypeId");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int DiscountValueColumn = StartColumn + columns.IndexOf("DiscountValue");
            int PriceColumn = StartColumn + columns.IndexOf("Price");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionStoreDAO PromotionStoreDAO = new PromotionStoreDAO();
                PromotionStoreDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreDAO.PromotionPolicyId = ExcelWorksheet.Cells[row, PromotionPolicyIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                PromotionStoreDAO.FromValue = ExcelWorksheet.Cells[row, FromValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionStoreDAO.ToValue = ExcelWorksheet.Cells[row, ToValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionStoreDAO.PromotionDiscountTypeId = ExcelWorksheet.Cells[row, PromotionDiscountTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                PromotionStoreDAO.DiscountValue = ExcelWorksheet.Cells[row, DiscountValueColumn].Value?.ParseNullDecimal();
                PromotionStoreDAO.Price = ExcelWorksheet.Cells[row, PriceColumn].Value?.ParseNullDecimal();
                PromotionStoreDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PromotionStoreDAOs.Add(PromotionStoreDAO);
            }
            await DataContext.PromotionStore.BulkMergeAsync(PromotionStoreDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionStoreGrouping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionStoreGroupingDAOs = new List<PromotionStoreGroupingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int PromotionPolicyIdColumn = StartColumn + columns.IndexOf("PromotionPolicyId");
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int FromValueColumn = StartColumn + columns.IndexOf("FromValue");
            int ToValueColumn = StartColumn + columns.IndexOf("ToValue");
            int PromotionDiscountTypeIdColumn = StartColumn + columns.IndexOf("PromotionDiscountTypeId");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int DiscountValueColumn = StartColumn + columns.IndexOf("DiscountValue");
            int PriceColumn = StartColumn + columns.IndexOf("Price");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionStoreGroupingDAO PromotionStoreGroupingDAO = new PromotionStoreGroupingDAO();
                PromotionStoreGroupingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreGroupingDAO.PromotionPolicyId = ExcelWorksheet.Cells[row, PromotionPolicyIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreGroupingDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreGroupingDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                PromotionStoreGroupingDAO.FromValue = ExcelWorksheet.Cells[row, FromValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionStoreGroupingDAO.ToValue = ExcelWorksheet.Cells[row, ToValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionStoreGroupingDAO.PromotionDiscountTypeId = ExcelWorksheet.Cells[row, PromotionDiscountTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreGroupingDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                PromotionStoreGroupingDAO.DiscountValue = ExcelWorksheet.Cells[row, DiscountValueColumn].Value?.ParseNullDecimal();
                PromotionStoreGroupingDAO.Price = ExcelWorksheet.Cells[row, PriceColumn].Value?.ParseNullDecimal();
                PromotionStoreGroupingDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PromotionStoreGroupingDAOs.Add(PromotionStoreGroupingDAO);
            }
            await DataContext.PromotionStoreGrouping.BulkMergeAsync(PromotionStoreGroupingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionStoreGroupingItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionStoreGroupingItemMappingDAOs = new List<PromotionStoreGroupingItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionStoreGroupingIdColumn = StartColumn + columns.IndexOf("PromotionStoreGroupingId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionStoreGroupingItemMappingDAO PromotionStoreGroupingItemMappingDAO = new PromotionStoreGroupingItemMappingDAO();
                PromotionStoreGroupingItemMappingDAO.PromotionStoreGroupingId = ExcelWorksheet.Cells[row, PromotionStoreGroupingIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreGroupingItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreGroupingItemMappingDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseString();
                PromotionStoreGroupingItemMappingDAOs.Add(PromotionStoreGroupingItemMappingDAO);
            }
            await DataContext.PromotionStoreGroupingItemMapping.BulkMergeAsync(PromotionStoreGroupingItemMappingDAOs);
        }
        protected async Task Given_PromotionStoreGroupingMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionStoreGroupingMappingDAOs = new List<PromotionStoreGroupingMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int StoreGroupingIdColumn = StartColumn + columns.IndexOf("StoreGroupingId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionStoreGroupingMappingDAO PromotionStoreGroupingMappingDAO = new PromotionStoreGroupingMappingDAO();
                PromotionStoreGroupingMappingDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreGroupingMappingDAO.StoreGroupingId = ExcelWorksheet.Cells[row, StoreGroupingIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreGroupingMappingDAOs.Add(PromotionStoreGroupingMappingDAO);
            }
            await DataContext.PromotionStoreGroupingMapping.BulkMergeAsync(PromotionStoreGroupingMappingDAOs);
        }
        protected async Task Given_PromotionStoreItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionStoreItemMappingDAOs = new List<PromotionStoreItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionStoreIdColumn = StartColumn + columns.IndexOf("PromotionStoreId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionStoreItemMappingDAO PromotionStoreItemMappingDAO = new PromotionStoreItemMappingDAO();
                PromotionStoreItemMappingDAO.PromotionStoreId = ExcelWorksheet.Cells[row, PromotionStoreIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreItemMappingDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                PromotionStoreItemMappingDAOs.Add(PromotionStoreItemMappingDAO);
            }
            await DataContext.PromotionStoreItemMapping.BulkMergeAsync(PromotionStoreItemMappingDAOs);
        }
        protected async Task Given_PromotionStoreMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionStoreMappingDAOs = new List<PromotionStoreMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionStoreMappingDAO PromotionStoreMappingDAO = new PromotionStoreMappingDAO();
                PromotionStoreMappingDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreMappingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreMappingDAOs.Add(PromotionStoreMappingDAO);
            }
            await DataContext.PromotionStoreMapping.BulkMergeAsync(PromotionStoreMappingDAOs);
        }
        protected async Task Given_PromotionStoreType(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionStoreTypeDAOs = new List<PromotionStoreTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int PromotionPolicyIdColumn = StartColumn + columns.IndexOf("PromotionPolicyId");
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int NoteColumn = StartColumn + columns.IndexOf("Note");
            int FromValueColumn = StartColumn + columns.IndexOf("FromValue");
            int ToValueColumn = StartColumn + columns.IndexOf("ToValue");
            int PromotionDiscountTypeIdColumn = StartColumn + columns.IndexOf("PromotionDiscountTypeId");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int DiscountValueColumn = StartColumn + columns.IndexOf("DiscountValue");
            int PriceColumn = StartColumn + columns.IndexOf("Price");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionStoreTypeDAO PromotionStoreTypeDAO = new PromotionStoreTypeDAO();
                PromotionStoreTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreTypeDAO.PromotionPolicyId = ExcelWorksheet.Cells[row, PromotionPolicyIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreTypeDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreTypeDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                PromotionStoreTypeDAO.FromValue = ExcelWorksheet.Cells[row, FromValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionStoreTypeDAO.ToValue = ExcelWorksheet.Cells[row, ToValueColumn].Value?.ParseDecimal() ?? 0;
                PromotionStoreTypeDAO.PromotionDiscountTypeId = ExcelWorksheet.Cells[row, PromotionDiscountTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreTypeDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                PromotionStoreTypeDAO.DiscountValue = ExcelWorksheet.Cells[row, DiscountValueColumn].Value?.ParseNullDecimal();
                PromotionStoreTypeDAO.Price = ExcelWorksheet.Cells[row, PriceColumn].Value?.ParseNullDecimal();
                PromotionStoreTypeDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                PromotionStoreTypeDAOs.Add(PromotionStoreTypeDAO);
            }
            await DataContext.PromotionStoreType.BulkMergeAsync(PromotionStoreTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PromotionStoreTypeItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionStoreTypeItemMappingDAOs = new List<PromotionStoreTypeItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionStoreTypeIdColumn = StartColumn + columns.IndexOf("PromotionStoreTypeId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionStoreTypeItemMappingDAO PromotionStoreTypeItemMappingDAO = new PromotionStoreTypeItemMappingDAO();
                PromotionStoreTypeItemMappingDAO.PromotionStoreTypeId = ExcelWorksheet.Cells[row, PromotionStoreTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreTypeItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreTypeItemMappingDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                PromotionStoreTypeItemMappingDAOs.Add(PromotionStoreTypeItemMappingDAO);
            }
            await DataContext.PromotionStoreTypeItemMapping.BulkMergeAsync(PromotionStoreTypeItemMappingDAOs);
        }
        protected async Task Given_PromotionStoreTypeMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionStoreTypeMappingDAOs = new List<PromotionStoreTypeMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PromotionIdColumn = StartColumn + columns.IndexOf("PromotionId");
            int StoreTypeIdColumn = StartColumn + columns.IndexOf("StoreTypeId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionStoreTypeMappingDAO PromotionStoreTypeMappingDAO = new PromotionStoreTypeMappingDAO();
                PromotionStoreTypeMappingDAO.PromotionId = ExcelWorksheet.Cells[row, PromotionIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreTypeMappingDAO.StoreTypeId = ExcelWorksheet.Cells[row, StoreTypeIdColumn].Value?.ParseLong() ?? 0;
                PromotionStoreTypeMappingDAOs.Add(PromotionStoreTypeMappingDAO);
            }
            await DataContext.PromotionStoreTypeMapping.BulkMergeAsync(PromotionStoreTypeMappingDAOs);
        }
        protected async Task Given_PromotionType(ExcelWorksheet ExcelWorksheet)
        {
            this.PromotionTypeDAOs = new List<PromotionTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PromotionTypeDAO PromotionTypeDAO = new PromotionTypeDAO();
                PromotionTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PromotionTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                PromotionTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PromotionTypeDAOs.Add(PromotionTypeDAO);
            }
            await DataContext.PromotionType.BulkMergeAsync(PromotionTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Province(ExcelWorksheet ExcelWorksheet)
        {
            this.ProvinceDAOs = new List<ProvinceDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PriorityColumn = StartColumn + columns.IndexOf("Priority");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProvinceDAO ProvinceDAO = new ProvinceDAO();
                ProvinceDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ProvinceDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ProvinceDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ProvinceDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseNullLong();
                ProvinceDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                ProvinceDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                ProvinceDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProvinceDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProvinceDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                ProvinceDAOs.Add(ProvinceDAO);
            }
            await DataContext.Province.BulkMergeAsync(ProvinceDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_RequestState(ExcelWorksheet ExcelWorksheet)
        {
            this.RequestStateDAOs = new List<RequestStateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                RequestStateDAO RequestStateDAO = new RequestStateDAO();
                RequestStateDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                RequestStateDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                RequestStateDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                RequestStateDAOs.Add(RequestStateDAO);
            }
            await DataContext.RequestState.BulkMergeAsync(RequestStateDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_RequestWorkflowDefinitionMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.RequestWorkflowDefinitionMappingDAOs = new List<RequestWorkflowDefinitionMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int RequestIdColumn = StartColumn + columns.IndexOf("RequestId");
            int WorkflowDefinitionIdColumn = StartColumn + columns.IndexOf("WorkflowDefinitionId");
            int RequestStateIdColumn = StartColumn + columns.IndexOf("RequestStateId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int CounterColumn = StartColumn + columns.IndexOf("Counter");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                RequestWorkflowDefinitionMappingDAO RequestWorkflowDefinitionMappingDAO = new RequestWorkflowDefinitionMappingDAO();
                RequestWorkflowDefinitionMappingDAO.RequestId = ExcelWorksheet.Cells[row, RequestIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                RequestWorkflowDefinitionMappingDAO.WorkflowDefinitionId = ExcelWorksheet.Cells[row, WorkflowDefinitionIdColumn].Value?.ParseLong() ?? 0;
                RequestWorkflowDefinitionMappingDAO.RequestStateId = ExcelWorksheet.Cells[row, RequestStateIdColumn].Value?.ParseLong() ?? 0;
                RequestWorkflowDefinitionMappingDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                RequestWorkflowDefinitionMappingDAO.Counter = ExcelWorksheet.Cells[row, CounterColumn].Value?.ParseLong() ?? 0;
                RequestWorkflowDefinitionMappingDAOs.Add(RequestWorkflowDefinitionMappingDAO);
            }
            await DataContext.RequestWorkflowDefinitionMapping.BulkMergeAsync(RequestWorkflowDefinitionMappingDAOs);
        }
        protected async Task Given_RequestWorkflowHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.RequestWorkflowHistoryDAOs = new List<RequestWorkflowHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int CounterColumn = StartColumn + columns.IndexOf("Counter");
            int RequestIdColumn = StartColumn + columns.IndexOf("RequestId");
            int WorkflowStepIdColumn = StartColumn + columns.IndexOf("WorkflowStepId");
            int WorkflowStateIdColumn = StartColumn + columns.IndexOf("WorkflowStateId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                RequestWorkflowHistoryDAO RequestWorkflowHistoryDAO = new RequestWorkflowHistoryDAO();
                RequestWorkflowHistoryDAO.Counter = ExcelWorksheet.Cells[row, CounterColumn].Value?.ParseLong() ?? 0;
                RequestWorkflowHistoryDAO.RequestId = ExcelWorksheet.Cells[row, RequestIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                RequestWorkflowHistoryDAO.WorkflowStepId = ExcelWorksheet.Cells[row, WorkflowStepIdColumn].Value?.ParseLong() ?? 0;
                RequestWorkflowHistoryDAO.WorkflowStateId = ExcelWorksheet.Cells[row, WorkflowStateIdColumn].Value?.ParseLong() ?? 0;
                RequestWorkflowHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseNullLong();
                RequestWorkflowHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                RequestWorkflowHistoryDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                RequestWorkflowHistoryDAOs.Add(RequestWorkflowHistoryDAO);
            }
            await DataContext.RequestWorkflowHistory.BulkMergeAsync(RequestWorkflowHistoryDAOs);
        }
        protected async Task Given_RequestWorkflowParameterMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.RequestWorkflowParameterMappingDAOs = new List<RequestWorkflowParameterMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int WorkflowParameterIdColumn = StartColumn + columns.IndexOf("WorkflowParameterId");
            int RequestIdColumn = StartColumn + columns.IndexOf("RequestId");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                RequestWorkflowParameterMappingDAO RequestWorkflowParameterMappingDAO = new RequestWorkflowParameterMappingDAO();
                RequestWorkflowParameterMappingDAO.WorkflowParameterId = ExcelWorksheet.Cells[row, WorkflowParameterIdColumn].Value?.ParseLong() ?? 0;
                RequestWorkflowParameterMappingDAO.RequestId = ExcelWorksheet.Cells[row, RequestIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                RequestWorkflowParameterMappingDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseString();
                RequestWorkflowParameterMappingDAOs.Add(RequestWorkflowParameterMappingDAO);
            }
            await DataContext.RequestWorkflowParameterMapping.BulkMergeAsync(RequestWorkflowParameterMappingDAOs);
        }
        protected async Task Given_RequestWorkflowStepMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.RequestWorkflowStepMappingDAOs = new List<RequestWorkflowStepMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int RequestIdColumn = StartColumn + columns.IndexOf("RequestId");
            int WorkflowStepIdColumn = StartColumn + columns.IndexOf("WorkflowStepId");
            int WorkflowStateIdColumn = StartColumn + columns.IndexOf("WorkflowStateId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                RequestWorkflowStepMappingDAO RequestWorkflowStepMappingDAO = new RequestWorkflowStepMappingDAO();
                RequestWorkflowStepMappingDAO.RequestId = ExcelWorksheet.Cells[row, RequestIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                RequestWorkflowStepMappingDAO.WorkflowStepId = ExcelWorksheet.Cells[row, WorkflowStepIdColumn].Value?.ParseLong() ?? 0;
                RequestWorkflowStepMappingDAO.WorkflowStateId = ExcelWorksheet.Cells[row, WorkflowStateIdColumn].Value?.ParseLong() ?? 0;
                RequestWorkflowStepMappingDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseNullLong();
                RequestWorkflowStepMappingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                RequestWorkflowStepMappingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                RequestWorkflowStepMappingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                RequestWorkflowStepMappingDAOs.Add(RequestWorkflowStepMappingDAO);
            }
            await DataContext.RequestWorkflowStepMapping.BulkMergeAsync(RequestWorkflowStepMappingDAOs);
        }
        protected async Task Given_RewardHistoryContent(ExcelWorksheet ExcelWorksheet)
        {
            this.RewardHistoryContentDAOs = new List<RewardHistoryContentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int RewardHistoryIdColumn = StartColumn + columns.IndexOf("RewardHistoryId");
            int LuckyNumberIdColumn = StartColumn + columns.IndexOf("LuckyNumberId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                RewardHistoryContentDAO RewardHistoryContentDAO = new RewardHistoryContentDAO();
                RewardHistoryContentDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                RewardHistoryContentDAO.RewardHistoryId = ExcelWorksheet.Cells[row, RewardHistoryIdColumn].Value?.ParseLong() ?? 0;
                RewardHistoryContentDAO.LuckyNumberId = ExcelWorksheet.Cells[row, LuckyNumberIdColumn].Value?.ParseLong() ?? 0;
                RewardHistoryContentDAOs.Add(RewardHistoryContentDAO);
            }
            await DataContext.RewardHistoryContent.BulkMergeAsync(RewardHistoryContentDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_RewardHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.RewardHistoryDAOs = new List<RewardHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int TurnCounterColumn = StartColumn + columns.IndexOf("TurnCounter");
            int RevenueColumn = StartColumn + columns.IndexOf("Revenue");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                RewardHistoryDAO RewardHistoryDAO = new RewardHistoryDAO();
                RewardHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                RewardHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                RewardHistoryDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                RewardHistoryDAO.TurnCounter = ExcelWorksheet.Cells[row, TurnCounterColumn].Value?.ParseLong() ?? 0;
                RewardHistoryDAO.Revenue = ExcelWorksheet.Cells[row, RevenueColumn].Value?.ParseDecimal() ?? 0;
                RewardHistoryDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                RewardHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                RewardHistoryDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                RewardHistoryDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                RewardHistoryDAOs.Add(RewardHistoryDAO);
            }
            await DataContext.RewardHistory.BulkMergeAsync(RewardHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_RewardStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.RewardStatusDAOs = new List<RewardStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                RewardStatusDAO RewardStatusDAO = new RewardStatusDAO();
                RewardStatusDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                RewardStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                RewardStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                RewardStatusDAOs.Add(RewardStatusDAO);
            }
            await DataContext.RewardStatus.BulkMergeAsync(RewardStatusDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Role(ExcelWorksheet ExcelWorksheet)
        {
            this.RoleDAOs = new List<RoleDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                RoleDAO RoleDAO = new RoleDAO();
                RoleDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                RoleDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                RoleDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                RoleDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                RoleDAOs.Add(RoleDAO);
            }
            await DataContext.Role.BulkMergeAsync(RoleDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_SalesOrderType(ExcelWorksheet ExcelWorksheet)
        {
            this.SalesOrderTypeDAOs = new List<SalesOrderTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SalesOrderTypeDAO SalesOrderTypeDAO = new SalesOrderTypeDAO();
                SalesOrderTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SalesOrderTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                SalesOrderTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                SalesOrderTypeDAOs.Add(SalesOrderTypeDAO);
            }
            await DataContext.SalesOrderType.BulkMergeAsync(SalesOrderTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Sex(ExcelWorksheet ExcelWorksheet)
        {
            this.SexDAOs = new List<SexDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SexDAO SexDAO = new SexDAO();
                SexDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SexDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                SexDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                SexDAOs.Add(SexDAO);
            }
            await DataContext.Sex.BulkMergeAsync(SexDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Status(ExcelWorksheet ExcelWorksheet)
        {
            this.StatusDAOs = new List<StatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StatusDAO StatusDAO = new StatusDAO();
                StatusDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StatusDAOs.Add(StatusDAO);
            }
            await DataContext.Status.BulkMergeAsync(StatusDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreApprovalState(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreApprovalStateDAOs = new List<StoreApprovalStateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreApprovalStateDAO StoreApprovalStateDAO = new StoreApprovalStateDAO();
                StoreApprovalStateDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreApprovalStateDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreApprovalStateDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreApprovalStateDAOs.Add(StoreApprovalStateDAO);
            }
            await DataContext.StoreApprovalState.BulkMergeAsync(StoreApprovalStateDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreBalance(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreBalanceDAOs = new List<StoreBalanceDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int CreditAmountColumn = StartColumn + columns.IndexOf("CreditAmount");
            int DebitAmountColumn = StartColumn + columns.IndexOf("DebitAmount");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreBalanceDAO StoreBalanceDAO = new StoreBalanceDAO();
                StoreBalanceDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreBalanceDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                StoreBalanceDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreBalanceDAO.CreditAmount = ExcelWorksheet.Cells[row, CreditAmountColumn].Value?.ParseDecimal() ?? 0;
                StoreBalanceDAO.DebitAmount = ExcelWorksheet.Cells[row, DebitAmountColumn].Value?.ParseDecimal() ?? 0;
                StoreBalanceDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreBalanceDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreBalanceDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreBalanceDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreBalanceDAOs.Add(StoreBalanceDAO);
            }
            await DataContext.StoreBalance.BulkMergeAsync(StoreBalanceDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreChecking(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreCheckingDAOs = new List<StoreCheckingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int SaleEmployeeIdColumn = StartColumn + columns.IndexOf("SaleEmployeeId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int LongitudeColumn = StartColumn + columns.IndexOf("Longitude");
            int LatitudeColumn = StartColumn + columns.IndexOf("Latitude");
            int CheckOutLongitudeColumn = StartColumn + columns.IndexOf("CheckOutLongitude");
            int CheckOutLatitudeColumn = StartColumn + columns.IndexOf("CheckOutLatitude");
            int CheckInAtColumn = StartColumn + columns.IndexOf("CheckInAt");
            int CheckOutAtColumn = StartColumn + columns.IndexOf("CheckOutAt");
            int CheckInDistanceColumn = StartColumn + columns.IndexOf("CheckInDistance");
            int CheckOutDistanceColumn = StartColumn + columns.IndexOf("CheckOutDistance");
            int IndirectSalesOrderCounterColumn = StartColumn + columns.IndexOf("IndirectSalesOrderCounter");
            int ImageCounterColumn = StartColumn + columns.IndexOf("ImageCounter");
            int PlannedColumn = StartColumn + columns.IndexOf("Planned");
            int IsOpenedStoreColumn = StartColumn + columns.IndexOf("IsOpenedStore");
            int DeviceNameColumn = StartColumn + columns.IndexOf("DeviceName");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreCheckingDAO StoreCheckingDAO = new StoreCheckingDAO();
                StoreCheckingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreCheckingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreCheckingDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseLong() ?? 0;
                StoreCheckingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                StoreCheckingDAO.Longitude = ExcelWorksheet.Cells[row, LongitudeColumn].Value?.ParseNullDecimal();
                StoreCheckingDAO.Latitude = ExcelWorksheet.Cells[row, LatitudeColumn].Value?.ParseNullDecimal();
                StoreCheckingDAO.CheckOutLongitude = ExcelWorksheet.Cells[row, CheckOutLongitudeColumn].Value?.ParseNullDecimal();
                StoreCheckingDAO.CheckOutLatitude = ExcelWorksheet.Cells[row, CheckOutLatitudeColumn].Value?.ParseNullDecimal();
                StoreCheckingDAO.CheckInAt = ExcelWorksheet.Cells[row, CheckInAtColumn].Value?.ParseNullDateTime();
                StoreCheckingDAO.CheckOutAt = ExcelWorksheet.Cells[row, CheckOutAtColumn].Value?.ParseNullDateTime();
                StoreCheckingDAO.CheckInDistance = ExcelWorksheet.Cells[row, CheckInDistanceColumn].Value?.ParseNullLong();
                StoreCheckingDAO.CheckOutDistance = ExcelWorksheet.Cells[row, CheckOutDistanceColumn].Value?.ParseNullLong();
                StoreCheckingDAO.IndirectSalesOrderCounter = ExcelWorksheet.Cells[row, IndirectSalesOrderCounterColumn].Value?.ParseNullLong();
                StoreCheckingDAO.ImageCounter = ExcelWorksheet.Cells[row, ImageCounterColumn].Value?.ParseNullLong();
                StoreCheckingDAO.DeviceName = ExcelWorksheet.Cells[row, DeviceNameColumn].Value?.ParseString();
                StoreCheckingDAOs.Add(StoreCheckingDAO);
            }
            await DataContext.StoreChecking.BulkMergeAsync(StoreCheckingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreCheckingImageMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreCheckingImageMappingDAOs = new List<StoreCheckingImageMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            int StoreCheckingIdColumn = StartColumn + columns.IndexOf("StoreCheckingId");
            int AlbumIdColumn = StartColumn + columns.IndexOf("AlbumId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int SaleEmployeeIdColumn = StartColumn + columns.IndexOf("SaleEmployeeId");
            int ShootingAtColumn = StartColumn + columns.IndexOf("ShootingAt");
            int DistanceColumn = StartColumn + columns.IndexOf("Distance");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreCheckingImageMappingDAO StoreCheckingImageMappingDAO = new StoreCheckingImageMappingDAO();
                StoreCheckingImageMappingDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseLong() ?? 0;
                StoreCheckingImageMappingDAO.StoreCheckingId = ExcelWorksheet.Cells[row, StoreCheckingIdColumn].Value?.ParseLong() ?? 0;
                StoreCheckingImageMappingDAO.AlbumId = ExcelWorksheet.Cells[row, AlbumIdColumn].Value?.ParseLong() ?? 0;
                StoreCheckingImageMappingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreCheckingImageMappingDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseLong() ?? 0;
                StoreCheckingImageMappingDAO.ShootingAt = ExcelWorksheet.Cells[row, ShootingAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreCheckingImageMappingDAO.Distance = ExcelWorksheet.Cells[row, DistanceColumn].Value?.ParseNullLong();
                StoreCheckingImageMappingDAOs.Add(StoreCheckingImageMappingDAO);
            }
            await DataContext.StoreCheckingImageMapping.BulkMergeAsync(StoreCheckingImageMappingDAOs);
        }
        protected async Task Given_Store(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreDAOs = new List<StoreDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int CodeDraftColumn = StartColumn + columns.IndexOf("CodeDraft");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int UnsignNameColumn = StartColumn + columns.IndexOf("UnsignName");
            int ParentStoreIdColumn = StartColumn + columns.IndexOf("ParentStoreId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int StoreTypeIdColumn = StartColumn + columns.IndexOf("StoreTypeId");
            int TelephoneColumn = StartColumn + columns.IndexOf("Telephone");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
            int WardIdColumn = StartColumn + columns.IndexOf("WardId");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int UnsignAddressColumn = StartColumn + columns.IndexOf("UnsignAddress");
            int DeliveryAddressColumn = StartColumn + columns.IndexOf("DeliveryAddress");
            int LatitudeColumn = StartColumn + columns.IndexOf("Latitude");
            int LongitudeColumn = StartColumn + columns.IndexOf("Longitude");
            int DeliveryLatitudeColumn = StartColumn + columns.IndexOf("DeliveryLatitude");
            int DeliveryLongitudeColumn = StartColumn + columns.IndexOf("DeliveryLongitude");
            int OwnerNameColumn = StartColumn + columns.IndexOf("OwnerName");
            int OwnerPhoneColumn = StartColumn + columns.IndexOf("OwnerPhone");
            int OwnerEmailColumn = StartColumn + columns.IndexOf("OwnerEmail");
            int TaxCodeColumn = StartColumn + columns.IndexOf("TaxCode");
            int LegalEntityColumn = StartColumn + columns.IndexOf("LegalEntity");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int StoreScoutingIdColumn = StartColumn + columns.IndexOf("StoreScoutingId");
            int StoreStatusIdColumn = StartColumn + columns.IndexOf("StoreStatusId");
            int IsStoreApprovalDirectSalesOrderColumn = StartColumn + columns.IndexOf("IsStoreApprovalDirectSalesOrder");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int DebtLimitedColumn = StartColumn + columns.IndexOf("DebtLimited");
            int EstimatedRevenueIdColumn = StartColumn + columns.IndexOf("EstimatedRevenueId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreDAO StoreDAO = new StoreDAO();
                StoreDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreDAO.CodeDraft = ExcelWorksheet.Cells[row, CodeDraftColumn].Value?.ParseString();
                StoreDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreDAO.UnsignName = ExcelWorksheet.Cells[row, UnsignNameColumn].Value?.ParseString();
                StoreDAO.ParentStoreId = ExcelWorksheet.Cells[row, ParentStoreIdColumn].Value?.ParseNullLong();
                StoreDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.StoreTypeId = ExcelWorksheet.Cells[row, StoreTypeIdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.Telephone = ExcelWorksheet.Cells[row, TelephoneColumn].Value?.ParseString();
                StoreDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                StoreDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseNullLong();
                StoreDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                StoreDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                StoreDAO.UnsignAddress = ExcelWorksheet.Cells[row, UnsignAddressColumn].Value?.ParseString();
                StoreDAO.DeliveryAddress = ExcelWorksheet.Cells[row, DeliveryAddressColumn].Value?.ParseString();
                StoreDAO.Latitude = ExcelWorksheet.Cells[row, LatitudeColumn].Value?.ParseDecimal() ?? 0;
                StoreDAO.Longitude = ExcelWorksheet.Cells[row, LongitudeColumn].Value?.ParseDecimal() ?? 0;
                StoreDAO.DeliveryLatitude = ExcelWorksheet.Cells[row, DeliveryLatitudeColumn].Value?.ParseNullDecimal();
                StoreDAO.DeliveryLongitude = ExcelWorksheet.Cells[row, DeliveryLongitudeColumn].Value?.ParseNullDecimal();
                StoreDAO.OwnerName = ExcelWorksheet.Cells[row, OwnerNameColumn].Value?.ParseString();
                StoreDAO.OwnerPhone = ExcelWorksheet.Cells[row, OwnerPhoneColumn].Value?.ParseString();
                StoreDAO.OwnerEmail = ExcelWorksheet.Cells[row, OwnerEmailColumn].Value?.ParseString();
                StoreDAO.TaxCode = ExcelWorksheet.Cells[row, TaxCodeColumn].Value?.ParseString();
                StoreDAO.LegalEntity = ExcelWorksheet.Cells[row, LegalEntityColumn].Value?.ParseString();
                StoreDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseNullLong();
                StoreDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.StoreScoutingId = ExcelWorksheet.Cells[row, StoreScoutingIdColumn].Value?.ParseNullLong();
                StoreDAO.StoreStatusId = ExcelWorksheet.Cells[row, StoreStatusIdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                StoreDAO.DebtLimited = ExcelWorksheet.Cells[row, DebtLimitedColumn].Value?.ParseNullDecimal();
                StoreDAO.EstimatedRevenueId = ExcelWorksheet.Cells[row, EstimatedRevenueIdColumn].Value?.ParseNullLong();
                StoreDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreDAOs.Add(StoreDAO);
            }
            await DataContext.Store.BulkMergeAsync(StoreDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreGrouping(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreGroupingDAOs = new List<StoreGroupingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ParentIdColumn = StartColumn + columns.IndexOf("ParentId");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int LevelColumn = StartColumn + columns.IndexOf("Level");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreGroupingDAO StoreGroupingDAO = new StoreGroupingDAO();
                StoreGroupingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreGroupingDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreGroupingDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreGroupingDAO.ParentId = ExcelWorksheet.Cells[row, ParentIdColumn].Value?.ParseNullLong();
                StoreGroupingDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                StoreGroupingDAO.Level = ExcelWorksheet.Cells[row, LevelColumn].Value?.ParseLong() ?? 0;
                StoreGroupingDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                StoreGroupingDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreGroupingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreGroupingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreGroupingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreGroupingDAOs.Add(StoreGroupingDAO);
            }
            await DataContext.StoreGrouping.BulkMergeAsync(StoreGroupingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreHistoryDAOs = new List<StoreHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int StoreStatusIdColumn = StartColumn + columns.IndexOf("StoreStatusId");
            int EstimatedRevenueIdColumn = StartColumn + columns.IndexOf("EstimatedRevenueId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreHistoryDAO StoreHistoryDAO = new StoreHistoryDAO();
                StoreHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreHistoryDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreHistoryDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreHistoryDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseNullLong();
                StoreHistoryDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                StoreHistoryDAO.StoreStatusId = ExcelWorksheet.Cells[row, StoreStatusIdColumn].Value?.ParseLong() ?? 0;
                StoreHistoryDAO.EstimatedRevenueId = ExcelWorksheet.Cells[row, EstimatedRevenueIdColumn].Value?.ParseNullLong();
                StoreHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreHistoryDAOs.Add(StoreHistoryDAO);
            }
            await DataContext.StoreHistory.BulkMergeAsync(StoreHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreImage(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreImageDAOs = new List<StoreImageDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            int UrlColumn = StartColumn + columns.IndexOf("Url");
            int ThumbnailUrlColumn = StartColumn + columns.IndexOf("ThumbnailUrl");
            int AlbumIdColumn = StartColumn + columns.IndexOf("AlbumId");
            int AlbumNameColumn = StartColumn + columns.IndexOf("AlbumName");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int StoreNameColumn = StartColumn + columns.IndexOf("StoreName");
            int StoreAddressColumn = StartColumn + columns.IndexOf("StoreAddress");
            int ShootingAtColumn = StartColumn + columns.IndexOf("ShootingAt");
            int SaleEmployeeIdColumn = StartColumn + columns.IndexOf("SaleEmployeeId");
            int DisplayNameColumn = StartColumn + columns.IndexOf("DisplayName");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int DistanceColumn = StartColumn + columns.IndexOf("Distance");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreImageDAO StoreImageDAO = new StoreImageDAO();
                StoreImageDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseLong() ?? 0;
                StoreImageDAO.Url = ExcelWorksheet.Cells[row, UrlColumn].Value?.ParseString();
                StoreImageDAO.ThumbnailUrl = ExcelWorksheet.Cells[row, ThumbnailUrlColumn].Value?.ParseString();
                StoreImageDAO.AlbumId = ExcelWorksheet.Cells[row, AlbumIdColumn].Value?.ParseLong() ?? 0;
                StoreImageDAO.AlbumName = ExcelWorksheet.Cells[row, AlbumNameColumn].Value?.ParseString();
                StoreImageDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreImageDAO.StoreName = ExcelWorksheet.Cells[row, StoreNameColumn].Value?.ParseString();
                StoreImageDAO.StoreAddress = ExcelWorksheet.Cells[row, StoreAddressColumn].Value?.ParseString();
                StoreImageDAO.ShootingAt = ExcelWorksheet.Cells[row, ShootingAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreImageDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseNullLong();
                StoreImageDAO.DisplayName = ExcelWorksheet.Cells[row, DisplayNameColumn].Value?.ParseString();
                StoreImageDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                StoreImageDAO.Distance = ExcelWorksheet.Cells[row, DistanceColumn].Value?.ParseNullLong();
                StoreImageDAOs.Add(StoreImageDAO);
            }
            await DataContext.StoreImage.BulkMergeAsync(StoreImageDAOs);
        }
        protected async Task Given_StoreImageMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreImageMappingDAOs = new List<StoreImageMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreImageMappingDAO StoreImageMappingDAO = new StoreImageMappingDAO();
                StoreImageMappingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreImageMappingDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseLong() ?? 0;
                StoreImageMappingDAOs.Add(StoreImageMappingDAO);
            }
            await DataContext.StoreImageMapping.BulkMergeAsync(StoreImageMappingDAOs);
        }
        protected async Task Given_StoreScouting(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreScoutingDAOs = new List<StoreScoutingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int OwnerPhoneColumn = StartColumn + columns.IndexOf("OwnerPhone");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
            int WardIdColumn = StartColumn + columns.IndexOf("WardId");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int LatitudeColumn = StartColumn + columns.IndexOf("Latitude");
            int LongitudeColumn = StartColumn + columns.IndexOf("Longitude");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int StoreScoutingStatusIdColumn = StartColumn + columns.IndexOf("StoreScoutingStatusId");
            int StoreScoutingTypeIdColumn = StartColumn + columns.IndexOf("StoreScoutingTypeId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreScoutingDAO StoreScoutingDAO = new StoreScoutingDAO();
                StoreScoutingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreScoutingDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreScoutingDAO.OwnerPhone = ExcelWorksheet.Cells[row, OwnerPhoneColumn].Value?.ParseString();
                StoreScoutingDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                StoreScoutingDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseNullLong();
                StoreScoutingDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                StoreScoutingDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                StoreScoutingDAO.Latitude = ExcelWorksheet.Cells[row, LatitudeColumn].Value?.ParseDecimal() ?? 0;
                StoreScoutingDAO.Longitude = ExcelWorksheet.Cells[row, LongitudeColumn].Value?.ParseDecimal() ?? 0;
                StoreScoutingDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingDAO.StoreScoutingStatusId = ExcelWorksheet.Cells[row, StoreScoutingStatusIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingDAO.StoreScoutingTypeId = ExcelWorksheet.Cells[row, StoreScoutingTypeIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreScoutingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreScoutingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreScoutingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreScoutingDAOs.Add(StoreScoutingDAO);
            }
            await DataContext.StoreScouting.BulkMergeAsync(StoreScoutingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreScoutingImageMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreScoutingImageMappingDAOs = new List<StoreScoutingImageMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreScoutingIdColumn = StartColumn + columns.IndexOf("StoreScoutingId");
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreScoutingImageMappingDAO StoreScoutingImageMappingDAO = new StoreScoutingImageMappingDAO();
                StoreScoutingImageMappingDAO.StoreScoutingId = ExcelWorksheet.Cells[row, StoreScoutingIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingImageMappingDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingImageMappingDAOs.Add(StoreScoutingImageMappingDAO);
            }
            await DataContext.StoreScoutingImageMapping.BulkMergeAsync(StoreScoutingImageMappingDAOs);
        }
        protected async Task Given_StoreScoutingStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreScoutingStatusDAOs = new List<StoreScoutingStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreScoutingStatusDAO StoreScoutingStatusDAO = new StoreScoutingStatusDAO();
                StoreScoutingStatusDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreScoutingStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreScoutingStatusDAOs.Add(StoreScoutingStatusDAO);
            }
            await DataContext.StoreScoutingStatus.BulkMergeAsync(StoreScoutingStatusDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreScoutingType(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreScoutingTypeDAOs = new List<StoreScoutingTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreScoutingTypeDAO StoreScoutingTypeDAO = new StoreScoutingTypeDAO();
                StoreScoutingTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreScoutingTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreScoutingTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingTypeDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreScoutingTypeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreScoutingTypeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreScoutingTypeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreScoutingTypeDAOs.Add(StoreScoutingTypeDAO);
            }
            await DataContext.StoreScoutingType.BulkMergeAsync(StoreScoutingTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreStatusDAOs = new List<StoreStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreStatusDAO StoreStatusDAO = new StoreStatusDAO();
                StoreStatusDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreStatusDAOs.Add(StoreStatusDAO);
            }
            await DataContext.StoreStatus.BulkMergeAsync(StoreStatusDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreStatusHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreStatusHistoryDAOs = new List<StoreStatusHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int PreviousCreatedAtColumn = StartColumn + columns.IndexOf("PreviousCreatedAt");
            int PreviousStoreStatusIdColumn = StartColumn + columns.IndexOf("PreviousStoreStatusId");
            int StoreStatusIdColumn = StartColumn + columns.IndexOf("StoreStatusId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreStatusHistoryDAO StoreStatusHistoryDAO = new StoreStatusHistoryDAO();
                StoreStatusHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreStatusHistoryDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreStatusHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                StoreStatusHistoryDAO.PreviousCreatedAt = ExcelWorksheet.Cells[row, PreviousCreatedAtColumn].Value?.ParseNullDateTime();
                StoreStatusHistoryDAO.PreviousStoreStatusId = ExcelWorksheet.Cells[row, PreviousStoreStatusIdColumn].Value?.ParseNullLong();
                StoreStatusHistoryDAO.StoreStatusId = ExcelWorksheet.Cells[row, StoreStatusIdColumn].Value?.ParseLong() ?? 0;
                StoreStatusHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreStatusHistoryDAOs.Add(StoreStatusHistoryDAO);
            }
            await DataContext.StoreStatusHistory.BulkMergeAsync(StoreStatusHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreStatusHistoryType(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreStatusHistoryTypeDAOs = new List<StoreStatusHistoryTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreStatusHistoryTypeDAO StoreStatusHistoryTypeDAO = new StoreStatusHistoryTypeDAO();
                StoreStatusHistoryTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreStatusHistoryTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreStatusHistoryTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreStatusHistoryTypeDAOs.Add(StoreStatusHistoryTypeDAO);
            }
            await DataContext.StoreStatusHistoryType.BulkMergeAsync(StoreStatusHistoryTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreStoreGroupingMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreStoreGroupingMappingDAOs = new List<StoreStoreGroupingMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int StoreGroupingIdColumn = StartColumn + columns.IndexOf("StoreGroupingId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreStoreGroupingMappingDAO StoreStoreGroupingMappingDAO = new StoreStoreGroupingMappingDAO();
                StoreStoreGroupingMappingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreStoreGroupingMappingDAO.StoreGroupingId = ExcelWorksheet.Cells[row, StoreGroupingIdColumn].Value?.ParseLong() ?? 0;
                StoreStoreGroupingMappingDAOs.Add(StoreStoreGroupingMappingDAO);
            }
            await DataContext.StoreStoreGroupingMapping.BulkMergeAsync(StoreStoreGroupingMappingDAOs);
        }
        protected async Task Given_StoreType(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreTypeDAOs = new List<StoreTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ColorIdColumn = StartColumn + columns.IndexOf("ColorId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreTypeDAO StoreTypeDAO = new StoreTypeDAO();
                StoreTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreTypeDAO.ColorId = ExcelWorksheet.Cells[row, ColorIdColumn].Value?.ParseNullLong();
                StoreTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                StoreTypeDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreTypeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreTypeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreTypeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreTypeDAOs.Add(StoreTypeDAO);
            }
            await DataContext.StoreType.BulkMergeAsync(StoreTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreTypeHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreTypeHistoryDAOs = new List<StoreTypeHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int PreviousCreatedAtColumn = StartColumn + columns.IndexOf("PreviousCreatedAt");
            int PreviousStoreTypeIdColumn = StartColumn + columns.IndexOf("PreviousStoreTypeId");
            int StoreTypeIdColumn = StartColumn + columns.IndexOf("StoreTypeId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreTypeHistoryDAO StoreTypeHistoryDAO = new StoreTypeHistoryDAO();
                StoreTypeHistoryDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreTypeHistoryDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreTypeHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                StoreTypeHistoryDAO.PreviousCreatedAt = ExcelWorksheet.Cells[row, PreviousCreatedAtColumn].Value?.ParseNullDateTime();
                StoreTypeHistoryDAO.PreviousStoreTypeId = ExcelWorksheet.Cells[row, PreviousStoreTypeIdColumn].Value?.ParseNullLong();
                StoreTypeHistoryDAO.StoreTypeId = ExcelWorksheet.Cells[row, StoreTypeIdColumn].Value?.ParseLong() ?? 0;
                StoreTypeHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreTypeHistoryDAOs.Add(StoreTypeHistoryDAO);
            }
            await DataContext.StoreTypeHistory.BulkMergeAsync(StoreTypeHistoryDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreUnchecking(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreUncheckingDAOs = new List<StoreUncheckingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int DateColumn = StartColumn + columns.IndexOf("Date");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreUncheckingDAO StoreUncheckingDAO = new StoreUncheckingDAO();
                StoreUncheckingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreUncheckingDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                StoreUncheckingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreUncheckingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                StoreUncheckingDAO.Date = ExcelWorksheet.Cells[row, DateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreUncheckingDAOs.Add(StoreUncheckingDAO);
            }
            await DataContext.StoreUnchecking.BulkMergeAsync(StoreUncheckingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreUser(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreUserDAOs = new List<StoreUserDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int UsernameColumn = StartColumn + columns.IndexOf("Username");
            int DisplayNameColumn = StartColumn + columns.IndexOf("DisplayName");
            int PasswordColumn = StartColumn + columns.IndexOf("Password");
            int OtpCodeColumn = StartColumn + columns.IndexOf("OtpCode");
            int OtpExpiredColumn = StartColumn + columns.IndexOf("OtpExpired");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreUserDAO StoreUserDAO = new StoreUserDAO();
                StoreUserDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreUserDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreUserDAO.Username = ExcelWorksheet.Cells[row, UsernameColumn].Value?.ParseString();
                StoreUserDAO.DisplayName = ExcelWorksheet.Cells[row, DisplayNameColumn].Value?.ParseString();
                StoreUserDAO.Password = ExcelWorksheet.Cells[row, PasswordColumn].Value?.ParseString();
                StoreUserDAO.OtpCode = ExcelWorksheet.Cells[row, OtpCodeColumn].Value?.ParseString();
                StoreUserDAO.OtpExpired = ExcelWorksheet.Cells[row, OtpExpiredColumn].Value?.ParseNullDateTime();
                StoreUserDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                StoreUserDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreUserDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreUserDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreUserDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreUserDAOs.Add(StoreUserDAO);
            }
            await DataContext.StoreUser.BulkMergeAsync(StoreUserDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_StoreUserFavoriteProductMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreUserFavoriteProductMappingDAOs = new List<StoreUserFavoriteProductMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int FavoriteProductIdColumn = StartColumn + columns.IndexOf("FavoriteProductId");
            int StoreUserIdColumn = StartColumn + columns.IndexOf("StoreUserId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreUserFavoriteProductMappingDAO StoreUserFavoriteProductMappingDAO = new StoreUserFavoriteProductMappingDAO();
                StoreUserFavoriteProductMappingDAO.FavoriteProductId = ExcelWorksheet.Cells[row, FavoriteProductIdColumn].Value?.ParseLong() ?? 0;
                StoreUserFavoriteProductMappingDAO.StoreUserId = ExcelWorksheet.Cells[row, StoreUserIdColumn].Value?.ParseLong() ?? 0;
                StoreUserFavoriteProductMappingDAOs.Add(StoreUserFavoriteProductMappingDAO);
            }
            await DataContext.StoreUserFavoriteProductMapping.BulkMergeAsync(StoreUserFavoriteProductMappingDAOs);
        }
        protected async Task Given_Supplier(ExcelWorksheet ExcelWorksheet)
        {
            this.SupplierDAOs = new List<SupplierDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int TaxCodeColumn = StartColumn + columns.IndexOf("TaxCode");
            int PhoneColumn = StartColumn + columns.IndexOf("Phone");
            int EmailColumn = StartColumn + columns.IndexOf("Email");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int NationIdColumn = StartColumn + columns.IndexOf("NationId");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
            int WardIdColumn = StartColumn + columns.IndexOf("WardId");
            int OwnerNameColumn = StartColumn + columns.IndexOf("OwnerName");
            int PersonInChargeIdColumn = StartColumn + columns.IndexOf("PersonInChargeId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SupplierDAO SupplierDAO = new SupplierDAO();
                SupplierDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SupplierDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                SupplierDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                SupplierDAO.TaxCode = ExcelWorksheet.Cells[row, TaxCodeColumn].Value?.ParseString();
                SupplierDAO.Phone = ExcelWorksheet.Cells[row, PhoneColumn].Value?.ParseString();
                SupplierDAO.Email = ExcelWorksheet.Cells[row, EmailColumn].Value?.ParseString();
                SupplierDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                SupplierDAO.NationId = ExcelWorksheet.Cells[row, NationIdColumn].Value?.ParseNullLong();
                SupplierDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                SupplierDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseNullLong();
                SupplierDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                SupplierDAO.OwnerName = ExcelWorksheet.Cells[row, OwnerNameColumn].Value?.ParseString();
                SupplierDAO.PersonInChargeId = ExcelWorksheet.Cells[row, PersonInChargeIdColumn].Value?.ParseNullLong();
                SupplierDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                SupplierDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                SupplierDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                SupplierDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                SupplierDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                SupplierDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                SupplierDAOs.Add(SupplierDAO);
            }
            await DataContext.Supplier.BulkMergeAsync(SupplierDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Survey(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyDAOs = new List<SurveyDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int TitleColumn = StartColumn + columns.IndexOf("Title");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int StartAtColumn = StartColumn + columns.IndexOf("StartAt");
            int EndAtColumn = StartColumn + columns.IndexOf("EndAt");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyDAO SurveyDAO = new SurveyDAO();
                SurveyDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SurveyDAO.Title = ExcelWorksheet.Cells[row, TitleColumn].Value?.ParseString();
                SurveyDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                SurveyDAO.StartAt = ExcelWorksheet.Cells[row, StartAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                SurveyDAO.EndAt = ExcelWorksheet.Cells[row, EndAtColumn].Value?.ParseNullDateTime();
                SurveyDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                SurveyDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                SurveyDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                SurveyDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                SurveyDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                SurveyDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                SurveyDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                SurveyDAOs.Add(SurveyDAO);
            }
            await DataContext.Survey.BulkMergeAsync(SurveyDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_SurveyOption(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyOptionDAOs = new List<SurveyOptionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int SurveyQuestionIdColumn = StartColumn + columns.IndexOf("SurveyQuestionId");
            int SurveyOptionTypeIdColumn = StartColumn + columns.IndexOf("SurveyOptionTypeId");
            int ContentColumn = StartColumn + columns.IndexOf("Content");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyOptionDAO SurveyOptionDAO = new SurveyOptionDAO();
                SurveyOptionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SurveyOptionDAO.SurveyQuestionId = ExcelWorksheet.Cells[row, SurveyQuestionIdColumn].Value?.ParseLong() ?? 0;
                SurveyOptionDAO.SurveyOptionTypeId = ExcelWorksheet.Cells[row, SurveyOptionTypeIdColumn].Value?.ParseLong() ?? 0;
                SurveyOptionDAO.Content = ExcelWorksheet.Cells[row, ContentColumn].Value?.ParseString();
                SurveyOptionDAOs.Add(SurveyOptionDAO);
            }
            await DataContext.SurveyOption.BulkMergeAsync(SurveyOptionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_SurveyOptionType(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyOptionTypeDAOs = new List<SurveyOptionTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyOptionTypeDAO SurveyOptionTypeDAO = new SurveyOptionTypeDAO();
                SurveyOptionTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SurveyOptionTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                SurveyOptionTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                SurveyOptionTypeDAOs.Add(SurveyOptionTypeDAO);
            }
            await DataContext.SurveyOptionType.BulkMergeAsync(SurveyOptionTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_SurveyQuestion(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyQuestionDAOs = new List<SurveyQuestionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int SurveyIdColumn = StartColumn + columns.IndexOf("SurveyId");
            int ContentColumn = StartColumn + columns.IndexOf("Content");
            int SurveyQuestionTypeIdColumn = StartColumn + columns.IndexOf("SurveyQuestionTypeId");
            int IsMandatoryColumn = StartColumn + columns.IndexOf("IsMandatory");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyQuestionDAO SurveyQuestionDAO = new SurveyQuestionDAO();
                SurveyQuestionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SurveyQuestionDAO.SurveyId = ExcelWorksheet.Cells[row, SurveyIdColumn].Value?.ParseLong() ?? 0;
                SurveyQuestionDAO.Content = ExcelWorksheet.Cells[row, ContentColumn].Value?.ParseString();
                SurveyQuestionDAO.SurveyQuestionTypeId = ExcelWorksheet.Cells[row, SurveyQuestionTypeIdColumn].Value?.ParseLong() ?? 0;
                SurveyQuestionDAOs.Add(SurveyQuestionDAO);
            }
            await DataContext.SurveyQuestion.BulkMergeAsync(SurveyQuestionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_SurveyQuestionFileMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyQuestionFileMappingDAOs = new List<SurveyQuestionFileMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int SurveyQuestionIdColumn = StartColumn + columns.IndexOf("SurveyQuestionId");
            int FileIdColumn = StartColumn + columns.IndexOf("FileId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyQuestionFileMappingDAO SurveyQuestionFileMappingDAO = new SurveyQuestionFileMappingDAO();
                SurveyQuestionFileMappingDAO.SurveyQuestionId = ExcelWorksheet.Cells[row, SurveyQuestionIdColumn].Value?.ParseLong() ?? 0;
                SurveyQuestionFileMappingDAO.FileId = ExcelWorksheet.Cells[row, FileIdColumn].Value?.ParseLong() ?? 0;
                SurveyQuestionFileMappingDAOs.Add(SurveyQuestionFileMappingDAO);
            }
            await DataContext.SurveyQuestionFileMapping.BulkMergeAsync(SurveyQuestionFileMappingDAOs);
        }
        protected async Task Given_SurveyQuestionImageMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyQuestionImageMappingDAOs = new List<SurveyQuestionImageMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int SurveyQuestionIdColumn = StartColumn + columns.IndexOf("SurveyQuestionId");
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyQuestionImageMappingDAO SurveyQuestionImageMappingDAO = new SurveyQuestionImageMappingDAO();
                SurveyQuestionImageMappingDAO.SurveyQuestionId = ExcelWorksheet.Cells[row, SurveyQuestionIdColumn].Value?.ParseLong() ?? 0;
                SurveyQuestionImageMappingDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseLong() ?? 0;
                SurveyQuestionImageMappingDAOs.Add(SurveyQuestionImageMappingDAO);
            }
            await DataContext.SurveyQuestionImageMapping.BulkMergeAsync(SurveyQuestionImageMappingDAOs);
        }
        protected async Task Given_SurveyQuestionType(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyQuestionTypeDAOs = new List<SurveyQuestionTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyQuestionTypeDAO SurveyQuestionTypeDAO = new SurveyQuestionTypeDAO();
                SurveyQuestionTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SurveyQuestionTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                SurveyQuestionTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                SurveyQuestionTypeDAOs.Add(SurveyQuestionTypeDAO);
            }
            await DataContext.SurveyQuestionType.BulkMergeAsync(SurveyQuestionTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_SurveyRespondentType(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyRespondentTypeDAOs = new List<SurveyRespondentTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyRespondentTypeDAO SurveyRespondentTypeDAO = new SurveyRespondentTypeDAO();
                SurveyRespondentTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SurveyRespondentTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                SurveyRespondentTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                SurveyRespondentTypeDAOs.Add(SurveyRespondentTypeDAO);
            }
            await DataContext.SurveyRespondentType.BulkMergeAsync(SurveyRespondentTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_SurveyResultCell(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyResultCellDAOs = new List<SurveyResultCellDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int SurveyResultIdColumn = StartColumn + columns.IndexOf("SurveyResultId");
            int SurveyQuestionIdColumn = StartColumn + columns.IndexOf("SurveyQuestionId");
            int RowOptionIdColumn = StartColumn + columns.IndexOf("RowOptionId");
            int ColumnOptionIdColumn = StartColumn + columns.IndexOf("ColumnOptionId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyResultCellDAO SurveyResultCellDAO = new SurveyResultCellDAO();
                SurveyResultCellDAO.SurveyResultId = ExcelWorksheet.Cells[row, SurveyResultIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultCellDAO.SurveyQuestionId = ExcelWorksheet.Cells[row, SurveyQuestionIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultCellDAO.RowOptionId = ExcelWorksheet.Cells[row, RowOptionIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultCellDAO.ColumnOptionId = ExcelWorksheet.Cells[row, ColumnOptionIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultCellDAOs.Add(SurveyResultCellDAO);
            }
            await DataContext.SurveyResultCell.BulkMergeAsync(SurveyResultCellDAOs);
        }
        protected async Task Given_SurveyResult(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyResultDAOs = new List<SurveyResultDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int SurveyIdColumn = StartColumn + columns.IndexOf("SurveyId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StoreScoutingIdColumn = StartColumn + columns.IndexOf("StoreScoutingId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int TimeColumn = StartColumn + columns.IndexOf("Time");
            int SurveyRespondentTypeIdColumn = StartColumn + columns.IndexOf("SurveyRespondentTypeId");
            int RespondentNameColumn = StartColumn + columns.IndexOf("RespondentName");
            int RespondentPhoneColumn = StartColumn + columns.IndexOf("RespondentPhone");
            int RespondentEmailColumn = StartColumn + columns.IndexOf("RespondentEmail");
            int RespondentAddressColumn = StartColumn + columns.IndexOf("RespondentAddress");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyResultDAO SurveyResultDAO = new SurveyResultDAO();
                SurveyResultDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SurveyResultDAO.SurveyId = ExcelWorksheet.Cells[row, SurveyIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultDAO.StoreScoutingId = ExcelWorksheet.Cells[row, StoreScoutingIdColumn].Value?.ParseNullLong();
                SurveyResultDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseNullLong();
                SurveyResultDAO.Time = ExcelWorksheet.Cells[row, TimeColumn].Value?.ParseDateTime() ?? DateTime.Now;
                SurveyResultDAO.SurveyRespondentTypeId = ExcelWorksheet.Cells[row, SurveyRespondentTypeIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultDAO.RespondentName = ExcelWorksheet.Cells[row, RespondentNameColumn].Value?.ParseString();
                SurveyResultDAO.RespondentPhone = ExcelWorksheet.Cells[row, RespondentPhoneColumn].Value?.ParseString();
                SurveyResultDAO.RespondentEmail = ExcelWorksheet.Cells[row, RespondentEmailColumn].Value?.ParseString();
                SurveyResultDAO.RespondentAddress = ExcelWorksheet.Cells[row, RespondentAddressColumn].Value?.ParseString();
                SurveyResultDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                SurveyResultDAOs.Add(SurveyResultDAO);
            }
            await DataContext.SurveyResult.BulkMergeAsync(SurveyResultDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_SurveyResultSingle(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyResultSingleDAOs = new List<SurveyResultSingleDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int SurveyResultIdColumn = StartColumn + columns.IndexOf("SurveyResultId");
            int SurveyQuestionIdColumn = StartColumn + columns.IndexOf("SurveyQuestionId");
            int SurveyOptionIdColumn = StartColumn + columns.IndexOf("SurveyOptionId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyResultSingleDAO SurveyResultSingleDAO = new SurveyResultSingleDAO();
                SurveyResultSingleDAO.SurveyResultId = ExcelWorksheet.Cells[row, SurveyResultIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultSingleDAO.SurveyQuestionId = ExcelWorksheet.Cells[row, SurveyQuestionIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultSingleDAO.SurveyOptionId = ExcelWorksheet.Cells[row, SurveyOptionIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultSingleDAOs.Add(SurveyResultSingleDAO);
            }
            await DataContext.SurveyResultSingle.BulkMergeAsync(SurveyResultSingleDAOs);
        }
        protected async Task Given_SurveyResultText(ExcelWorksheet ExcelWorksheet)
        {
            this.SurveyResultTextDAOs = new List<SurveyResultTextDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int SurveyResultIdColumn = StartColumn + columns.IndexOf("SurveyResultId");
            int SurveyQuestionIdColumn = StartColumn + columns.IndexOf("SurveyQuestionId");
            int ContentColumn = StartColumn + columns.IndexOf("Content");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SurveyResultTextDAO SurveyResultTextDAO = new SurveyResultTextDAO();
                SurveyResultTextDAO.SurveyResultId = ExcelWorksheet.Cells[row, SurveyResultIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultTextDAO.SurveyQuestionId = ExcelWorksheet.Cells[row, SurveyQuestionIdColumn].Value?.ParseLong() ?? 0;
                SurveyResultTextDAO.Content = ExcelWorksheet.Cells[row, ContentColumn].Value?.ParseString();
                SurveyResultTextDAOs.Add(SurveyResultTextDAO);
            }
            await DataContext.SurveyResultText.BulkMergeAsync(SurveyResultTextDAOs);
        }
        protected async Task Given_SystemConfiguration(ExcelWorksheet ExcelWorksheet)
        {
            this.SystemConfigurationDAOs = new List<SystemConfigurationDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SystemConfigurationDAO SystemConfigurationDAO = new SystemConfigurationDAO();
                SystemConfigurationDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SystemConfigurationDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                SystemConfigurationDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                SystemConfigurationDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseString();
                SystemConfigurationDAOs.Add(SystemConfigurationDAO);
            }
            await DataContext.SystemConfiguration.BulkMergeAsync(SystemConfigurationDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_TaxType(ExcelWorksheet ExcelWorksheet)
        {
            this.TaxTypeDAOs = new List<TaxTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PercentageColumn = StartColumn + columns.IndexOf("Percentage");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                TaxTypeDAO TaxTypeDAO = new TaxTypeDAO();
                TaxTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                TaxTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                TaxTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                TaxTypeDAO.Percentage = ExcelWorksheet.Cells[row, PercentageColumn].Value?.ParseDecimal() ?? 0;
                TaxTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                TaxTypeDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                TaxTypeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                TaxTypeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                TaxTypeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                TaxTypeDAOs.Add(TaxTypeDAO);
            }
            await DataContext.TaxType.BulkMergeAsync(TaxTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_TransactionType(ExcelWorksheet ExcelWorksheet)
        {
            this.TransactionTypeDAOs = new List<TransactionTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                TransactionTypeDAO TransactionTypeDAO = new TransactionTypeDAO();
                TransactionTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                TransactionTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                TransactionTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                TransactionTypeDAOs.Add(TransactionTypeDAO);
            }
            await DataContext.TransactionType.BulkMergeAsync(TransactionTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_UnitOfMeasure(ExcelWorksheet ExcelWorksheet)
        {
            this.UnitOfMeasureDAOs = new List<UnitOfMeasureDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                UnitOfMeasureDAO UnitOfMeasureDAO = new UnitOfMeasureDAO();
                UnitOfMeasureDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                UnitOfMeasureDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                UnitOfMeasureDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                UnitOfMeasureDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                UnitOfMeasureDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                UnitOfMeasureDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                UnitOfMeasureDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                UnitOfMeasureDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                UnitOfMeasureDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                UnitOfMeasureDAOs.Add(UnitOfMeasureDAO);
            }
            await DataContext.UnitOfMeasure.BulkMergeAsync(UnitOfMeasureDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_UnitOfMeasureGroupingContent(ExcelWorksheet ExcelWorksheet)
        {
            this.UnitOfMeasureGroupingContentDAOs = new List<UnitOfMeasureGroupingContentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int UnitOfMeasureGroupingIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureGroupingId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int FactorColumn = StartColumn + columns.IndexOf("Factor");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                UnitOfMeasureGroupingContentDAO UnitOfMeasureGroupingContentDAO = new UnitOfMeasureGroupingContentDAO();
                UnitOfMeasureGroupingContentDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                UnitOfMeasureGroupingContentDAO.UnitOfMeasureGroupingId = ExcelWorksheet.Cells[row, UnitOfMeasureGroupingIdColumn].Value?.ParseLong() ?? 0;
                UnitOfMeasureGroupingContentDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                UnitOfMeasureGroupingContentDAO.Factor = ExcelWorksheet.Cells[row, FactorColumn].Value?.ParseNullLong();
                UnitOfMeasureGroupingContentDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                UnitOfMeasureGroupingContentDAOs.Add(UnitOfMeasureGroupingContentDAO);
            }
            await DataContext.UnitOfMeasureGroupingContent.BulkMergeAsync(UnitOfMeasureGroupingContentDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_UnitOfMeasureGrouping(ExcelWorksheet ExcelWorksheet)
        {
            this.UnitOfMeasureGroupingDAOs = new List<UnitOfMeasureGroupingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                UnitOfMeasureGroupingDAO UnitOfMeasureGroupingDAO = new UnitOfMeasureGroupingDAO();
                UnitOfMeasureGroupingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                UnitOfMeasureGroupingDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                UnitOfMeasureGroupingDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                UnitOfMeasureGroupingDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                UnitOfMeasureGroupingDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                UnitOfMeasureGroupingDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                UnitOfMeasureGroupingDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                UnitOfMeasureGroupingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                UnitOfMeasureGroupingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                UnitOfMeasureGroupingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                UnitOfMeasureGroupingDAOs.Add(UnitOfMeasureGroupingDAO);
            }
            await DataContext.UnitOfMeasureGrouping.BulkMergeAsync(UnitOfMeasureGroupingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_UsedVariation(ExcelWorksheet ExcelWorksheet)
        {
            this.UsedVariationDAOs = new List<UsedVariationDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                UsedVariationDAO UsedVariationDAO = new UsedVariationDAO();
                UsedVariationDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                UsedVariationDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                UsedVariationDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                UsedVariationDAOs.Add(UsedVariationDAO);
            }
            await DataContext.UsedVariation.BulkMergeAsync(UsedVariationDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Variation(ExcelWorksheet ExcelWorksheet)
        {
            this.VariationDAOs = new List<VariationDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int VariationGroupingIdColumn = StartColumn + columns.IndexOf("VariationGroupingId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                VariationDAO VariationDAO = new VariationDAO();
                VariationDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                VariationDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                VariationDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                VariationDAO.VariationGroupingId = ExcelWorksheet.Cells[row, VariationGroupingIdColumn].Value?.ParseLong() ?? 0;
                VariationDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                VariationDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                VariationDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                VariationDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                VariationDAOs.Add(VariationDAO);
            }
            await DataContext.Variation.BulkMergeAsync(VariationDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_VariationGrouping(ExcelWorksheet ExcelWorksheet)
        {
            this.VariationGroupingDAOs = new List<VariationGroupingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ProductIdColumn = StartColumn + columns.IndexOf("ProductId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                VariationGroupingDAO VariationGroupingDAO = new VariationGroupingDAO();
                VariationGroupingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                VariationGroupingDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                VariationGroupingDAO.ProductId = ExcelWorksheet.Cells[row, ProductIdColumn].Value?.ParseLong() ?? 0;
                VariationGroupingDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                VariationGroupingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                VariationGroupingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                VariationGroupingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                VariationGroupingDAOs.Add(VariationGroupingDAO);
            }
            await DataContext.VariationGrouping.BulkMergeAsync(VariationGroupingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Ward(ExcelWorksheet ExcelWorksheet)
        {
            this.WardDAOs = new List<WardDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PriorityColumn = StartColumn + columns.IndexOf("Priority");
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WardDAO WardDAO = new WardDAO();
                WardDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WardDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                WardDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                WardDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseNullLong();
                WardDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseLong() ?? 0;
                WardDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                WardDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                WardDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                WardDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                WardDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                WardDAOs.Add(WardDAO);
            }
            await DataContext.Ward.BulkMergeAsync(WardDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Warehouse(ExcelWorksheet ExcelWorksheet)
        {
            this.WarehouseDAOs = new List<WarehouseDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
            int WardIdColumn = StartColumn + columns.IndexOf("WardId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WarehouseDAO WarehouseDAO = new WarehouseDAO();
                WarehouseDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WarehouseDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                WarehouseDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                WarehouseDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                WarehouseDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                WarehouseDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseNullLong();
                WarehouseDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                WarehouseDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                WarehouseDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                WarehouseDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                WarehouseDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                WarehouseDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                WarehouseDAOs.Add(WarehouseDAO);
            }
            await DataContext.Warehouse.BulkMergeAsync(WarehouseDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_WarehouseOrganizationMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.WarehouseOrganizationMappingDAOs = new List<WarehouseOrganizationMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int WarehouseIdColumn = StartColumn + columns.IndexOf("WarehouseId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WarehouseOrganizationMappingDAO WarehouseOrganizationMappingDAO = new WarehouseOrganizationMappingDAO();
                WarehouseOrganizationMappingDAO.WarehouseId = ExcelWorksheet.Cells[row, WarehouseIdColumn].Value?.ParseLong() ?? 0;
                WarehouseOrganizationMappingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                WarehouseOrganizationMappingDAOs.Add(WarehouseOrganizationMappingDAO);
            }
            await DataContext.WarehouseOrganizationMapping.BulkMergeAsync(WarehouseOrganizationMappingDAOs);
        }
        protected async Task Given_WorkflowDefinition(ExcelWorksheet ExcelWorksheet)
        {
            this.WorkflowDefinitionDAOs = new List<WorkflowDefinitionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int ModifierIdColumn = StartColumn + columns.IndexOf("ModifierId");
            int WorkflowTypeIdColumn = StartColumn + columns.IndexOf("WorkflowTypeId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int StartDateColumn = StartColumn + columns.IndexOf("StartDate");
            int EndDateColumn = StartColumn + columns.IndexOf("EndDate");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WorkflowDefinitionDAO WorkflowDefinitionDAO = new WorkflowDefinitionDAO();
                WorkflowDefinitionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WorkflowDefinitionDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                WorkflowDefinitionDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                WorkflowDefinitionDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDefinitionDAO.ModifierId = ExcelWorksheet.Cells[row, ModifierIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDefinitionDAO.WorkflowTypeId = ExcelWorksheet.Cells[row, WorkflowTypeIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDefinitionDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDefinitionDAO.StartDate = ExcelWorksheet.Cells[row, StartDateColumn].Value?.ParseNullDateTime();
                WorkflowDefinitionDAO.EndDate = ExcelWorksheet.Cells[row, EndDateColumn].Value?.ParseNullDateTime();
                WorkflowDefinitionDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDefinitionDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                WorkflowDefinitionDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                WorkflowDefinitionDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                WorkflowDefinitionDAOs.Add(WorkflowDefinitionDAO);
            }
            await DataContext.WorkflowDefinition.BulkMergeAsync(WorkflowDefinitionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_WorkflowDirectionCondition(ExcelWorksheet ExcelWorksheet)
        {
            this.WorkflowDirectionConditionDAOs = new List<WorkflowDirectionConditionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int WorkflowDirectionIdColumn = StartColumn + columns.IndexOf("WorkflowDirectionId");
            int WorkflowParameterIdColumn = StartColumn + columns.IndexOf("WorkflowParameterId");
            int WorkflowOperatorIdColumn = StartColumn + columns.IndexOf("WorkflowOperatorId");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WorkflowDirectionConditionDAO WorkflowDirectionConditionDAO = new WorkflowDirectionConditionDAO();
                WorkflowDirectionConditionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WorkflowDirectionConditionDAO.WorkflowDirectionId = ExcelWorksheet.Cells[row, WorkflowDirectionIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDirectionConditionDAO.WorkflowParameterId = ExcelWorksheet.Cells[row, WorkflowParameterIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDirectionConditionDAO.WorkflowOperatorId = ExcelWorksheet.Cells[row, WorkflowOperatorIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDirectionConditionDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseString();
                WorkflowDirectionConditionDAOs.Add(WorkflowDirectionConditionDAO);
            }
            await DataContext.WorkflowDirectionCondition.BulkMergeAsync(WorkflowDirectionConditionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_WorkflowDirection(ExcelWorksheet ExcelWorksheet)
        {
            this.WorkflowDirectionDAOs = new List<WorkflowDirectionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int WorkflowDefinitionIdColumn = StartColumn + columns.IndexOf("WorkflowDefinitionId");
            int FromStepIdColumn = StartColumn + columns.IndexOf("FromStepId");
            int ToStepIdColumn = StartColumn + columns.IndexOf("ToStepId");
            int SubjectMailForCreatorColumn = StartColumn + columns.IndexOf("SubjectMailForCreator");
            int SubjectMailForCurrentStepColumn = StartColumn + columns.IndexOf("SubjectMailForCurrentStep");
            int SubjectMailForNextStepColumn = StartColumn + columns.IndexOf("SubjectMailForNextStep");
            int BodyMailForCreatorColumn = StartColumn + columns.IndexOf("BodyMailForCreator");
            int BodyMailForCurrentStepColumn = StartColumn + columns.IndexOf("BodyMailForCurrentStep");
            int BodyMailForNextStepColumn = StartColumn + columns.IndexOf("BodyMailForNextStep");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WorkflowDirectionDAO WorkflowDirectionDAO = new WorkflowDirectionDAO();
                WorkflowDirectionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WorkflowDirectionDAO.WorkflowDefinitionId = ExcelWorksheet.Cells[row, WorkflowDefinitionIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDirectionDAO.FromStepId = ExcelWorksheet.Cells[row, FromStepIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDirectionDAO.ToStepId = ExcelWorksheet.Cells[row, ToStepIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDirectionDAO.SubjectMailForCreator = ExcelWorksheet.Cells[row, SubjectMailForCreatorColumn].Value?.ParseString();
                WorkflowDirectionDAO.SubjectMailForCurrentStep = ExcelWorksheet.Cells[row, SubjectMailForCurrentStepColumn].Value?.ParseString();
                WorkflowDirectionDAO.SubjectMailForNextStep = ExcelWorksheet.Cells[row, SubjectMailForNextStepColumn].Value?.ParseString();
                WorkflowDirectionDAO.BodyMailForCreator = ExcelWorksheet.Cells[row, BodyMailForCreatorColumn].Value?.ParseString();
                WorkflowDirectionDAO.BodyMailForCurrentStep = ExcelWorksheet.Cells[row, BodyMailForCurrentStepColumn].Value?.ParseString();
                WorkflowDirectionDAO.BodyMailForNextStep = ExcelWorksheet.Cells[row, BodyMailForNextStepColumn].Value?.ParseString();
                WorkflowDirectionDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                WorkflowDirectionDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                WorkflowDirectionDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                WorkflowDirectionDAOs.Add(WorkflowDirectionDAO);
            }
            await DataContext.WorkflowDirection.BulkMergeAsync(WorkflowDirectionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_WorkflowOperator(ExcelWorksheet ExcelWorksheet)
        {
            this.WorkflowOperatorDAOs = new List<WorkflowOperatorDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int WorkflowParameterTypeIdColumn = StartColumn + columns.IndexOf("WorkflowParameterTypeId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WorkflowOperatorDAO WorkflowOperatorDAO = new WorkflowOperatorDAO();
                WorkflowOperatorDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WorkflowOperatorDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                WorkflowOperatorDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                WorkflowOperatorDAO.WorkflowParameterTypeId = ExcelWorksheet.Cells[row, WorkflowParameterTypeIdColumn].Value?.ParseLong() ?? 0;
                WorkflowOperatorDAOs.Add(WorkflowOperatorDAO);
            }
            await DataContext.WorkflowOperator.BulkMergeAsync(WorkflowOperatorDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_WorkflowParameter(ExcelWorksheet ExcelWorksheet)
        {
            this.WorkflowParameterDAOs = new List<WorkflowParameterDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int WorkflowTypeIdColumn = StartColumn + columns.IndexOf("WorkflowTypeId");
            int WorkflowParameterTypeIdColumn = StartColumn + columns.IndexOf("WorkflowParameterTypeId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WorkflowParameterDAO WorkflowParameterDAO = new WorkflowParameterDAO();
                WorkflowParameterDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WorkflowParameterDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                WorkflowParameterDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                WorkflowParameterDAO.WorkflowTypeId = ExcelWorksheet.Cells[row, WorkflowTypeIdColumn].Value?.ParseLong() ?? 0;
                WorkflowParameterDAO.WorkflowParameterTypeId = ExcelWorksheet.Cells[row, WorkflowParameterTypeIdColumn].Value?.ParseLong() ?? 0;
                WorkflowParameterDAOs.Add(WorkflowParameterDAO);
            }
            await DataContext.WorkflowParameter.BulkMergeAsync(WorkflowParameterDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_WorkflowParameterType(ExcelWorksheet ExcelWorksheet)
        {
            this.WorkflowParameterTypeDAOs = new List<WorkflowParameterTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WorkflowParameterTypeDAO WorkflowParameterTypeDAO = new WorkflowParameterTypeDAO();
                WorkflowParameterTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WorkflowParameterTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                WorkflowParameterTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                WorkflowParameterTypeDAOs.Add(WorkflowParameterTypeDAO);
            }
            await DataContext.WorkflowParameterType.BulkMergeAsync(WorkflowParameterTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_WorkflowState(ExcelWorksheet ExcelWorksheet)
        {
            this.WorkflowStateDAOs = new List<WorkflowStateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WorkflowStateDAO WorkflowStateDAO = new WorkflowStateDAO();
                WorkflowStateDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WorkflowStateDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                WorkflowStateDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                WorkflowStateDAOs.Add(WorkflowStateDAO);
            }
            await DataContext.WorkflowState.BulkMergeAsync(WorkflowStateDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_WorkflowStep(ExcelWorksheet ExcelWorksheet)
        {
            this.WorkflowStepDAOs = new List<WorkflowStepDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int WorkflowDefinitionIdColumn = StartColumn + columns.IndexOf("WorkflowDefinitionId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int RoleIdColumn = StartColumn + columns.IndexOf("RoleId");
            int SubjectMailForRejectColumn = StartColumn + columns.IndexOf("SubjectMailForReject");
            int BodyMailForRejectColumn = StartColumn + columns.IndexOf("BodyMailForReject");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WorkflowStepDAO WorkflowStepDAO = new WorkflowStepDAO();
                WorkflowStepDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WorkflowStepDAO.WorkflowDefinitionId = ExcelWorksheet.Cells[row, WorkflowDefinitionIdColumn].Value?.ParseLong() ?? 0;
                WorkflowStepDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                WorkflowStepDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                WorkflowStepDAO.RoleId = ExcelWorksheet.Cells[row, RoleIdColumn].Value?.ParseLong() ?? 0;
                WorkflowStepDAO.SubjectMailForReject = ExcelWorksheet.Cells[row, SubjectMailForRejectColumn].Value?.ParseString();
                WorkflowStepDAO.BodyMailForReject = ExcelWorksheet.Cells[row, BodyMailForRejectColumn].Value?.ParseString();
                WorkflowStepDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                WorkflowStepDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                WorkflowStepDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                WorkflowStepDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                WorkflowStepDAOs.Add(WorkflowStepDAO);
            }
            await DataContext.WorkflowStep.BulkMergeAsync(WorkflowStepDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_WorkflowType(ExcelWorksheet ExcelWorksheet)
        {
            this.WorkflowTypeDAOs = new List<WorkflowTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WorkflowTypeDAO WorkflowTypeDAO = new WorkflowTypeDAO();
                WorkflowTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WorkflowTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                WorkflowTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                WorkflowTypeDAOs.Add(WorkflowTypeDAO);
            }
            await DataContext.WorkflowType.BulkMergeAsync(WorkflowTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }

        
        protected async Task Given_Action(ExcelWorksheet ExcelWorksheet)
        {
            this.ActionDAOs = new List<ActionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int MenuIdColumn = StartColumn + columns.IndexOf("MenuId");
            int IsDeletedColumn = StartColumn + columns.IndexOf("IsDeleted");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ActionDAO ActionDAO = new ActionDAO();
                ActionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ActionDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ActionDAO.MenuId = ExcelWorksheet.Cells[row, MenuIdColumn].Value?.ParseLong() ?? 0;
                ActionDAO.IsDeleted = ExcelWorksheet.Cells[row, IsDeletedColumn].Value?.ParseBool() ?? false;
                ActionDAOs.Add(ActionDAO);
            }
            await DataContext.Action.BulkMergeAsync(ActionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_ActionPageMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.ActionPageMappingDAOs = new List<ActionPageMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ActionIdColumn = StartColumn + columns.IndexOf("ActionId");
            int PageIdColumn = StartColumn + columns.IndexOf("PageId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ActionPageMappingDAO ActionPageMappingDAO = new ActionPageMappingDAO();
                ActionPageMappingDAO.ActionId = ExcelWorksheet.Cells[row, ActionIdColumn].Value?.ParseLong() ?? 0;
                ActionPageMappingDAO.PageId = ExcelWorksheet.Cells[row, PageIdColumn].Value?.ParseLong() ?? 0;
                ActionPageMappingDAOs.Add(ActionPageMappingDAO);
            }
            await DataContext.ActionPageMapping.BulkMergeAsync(ActionPageMappingDAOs);
        }
        protected async Task Given_Field(ExcelWorksheet ExcelWorksheet)
        {
            this.FieldDAOs = new List<FieldDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int FieldTypeIdColumn = StartColumn + columns.IndexOf("FieldTypeId");
            int MenuIdColumn = StartColumn + columns.IndexOf("MenuId");
            int IsDeletedColumn = StartColumn + columns.IndexOf("IsDeleted");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                FieldDAO FieldDAO = new FieldDAO();
                FieldDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                FieldDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                FieldDAO.FieldTypeId = ExcelWorksheet.Cells[row, FieldTypeIdColumn].Value?.ParseLong() ?? 0;
                FieldDAO.MenuId = ExcelWorksheet.Cells[row, MenuIdColumn].Value?.ParseLong() ?? 0;
                FieldDAO.IsDeleted = ExcelWorksheet.Cells[row, MenuIdColumn].Value?.ParseBool() ?? false;
                FieldDAOs.Add(FieldDAO);
            }
            await DataContext.Field.BulkMergeAsync(FieldDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_FieldType(ExcelWorksheet ExcelWorksheet)
        {
            this.FieldTypeDAOs = new List<FieldTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                FieldTypeDAO FieldTypeDAO = new FieldTypeDAO();
                FieldTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                FieldTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                FieldTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                FieldTypeDAOs.Add(FieldTypeDAO);
            }
            await DataContext.FieldType.BulkMergeAsync(FieldTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Menu(ExcelWorksheet ExcelWorksheet)
        {
            this.MenuDAOs = new List<MenuDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int IsDeletedColumn = StartColumn + columns.IndexOf("IsDeleted");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                MenuDAO MenuDAO = new MenuDAO();
                MenuDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                MenuDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                MenuDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                MenuDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                MenuDAO.IsDeleted = ExcelWorksheet.Cells[row, IsDeletedColumn].Value?.ParseBool() ?? false;
                MenuDAOs.Add(MenuDAO);
            }
            await DataContext.Menu.BulkMergeAsync(MenuDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Page(ExcelWorksheet ExcelWorksheet)
        {
            this.PageDAOs = new List<PageDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int IsDeletedColumn = StartColumn + columns.IndexOf("IsDeleted");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PageDAO PageDAO = new PageDAO();
                PageDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PageDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PageDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                PageDAO.IsDeleted = ExcelWorksheet.Cells[row, IsDeletedColumn].Value?.ParseBool() ?? false;
                PageDAOs.Add(PageDAO);
            }
            await DataContext.Page.BulkMergeAsync(PageDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_Permission(ExcelWorksheet ExcelWorksheet)
        {
            this.PermissionDAOs = new List<PermissionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int RoleIdColumn = StartColumn + columns.IndexOf("RoleId");
            int MenuIdColumn = StartColumn + columns.IndexOf("MenuId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PermissionDAO PermissionDAO = new PermissionDAO();
                PermissionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PermissionDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                PermissionDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PermissionDAO.RoleId = ExcelWorksheet.Cells[row, RoleIdColumn].Value?.ParseLong() ?? 0;
                PermissionDAO.MenuId = ExcelWorksheet.Cells[row, MenuIdColumn].Value?.ParseLong() ?? 0;
                PermissionDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                PermissionDAOs.Add(PermissionDAO);
            }
            await DataContext.Permission.BulkMergeAsync(PermissionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PermissionContent(ExcelWorksheet ExcelWorksheet)
        {
            this.PermissionContentDAOs = new List<PermissionContentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            int PermissionIdColumn = StartColumn + columns.IndexOf("PermissionId");
            int FieldIdColumn = StartColumn + columns.IndexOf("FieldId");
            int PermissionOperatorIdColumn = StartColumn + columns.IndexOf("PermissionOperatorId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PermissionContentDAO PermissionContentDAO = new PermissionContentDAO();
                PermissionContentDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PermissionContentDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseString();
                PermissionContentDAO.PermissionId = ExcelWorksheet.Cells[row, PermissionIdColumn].Value?.ParseLong() ?? 0;
                PermissionContentDAO.FieldId = ExcelWorksheet.Cells[row, FieldIdColumn].Value?.ParseLong() ?? 0;
                PermissionContentDAO.PermissionOperatorId = ExcelWorksheet.Cells[row, PermissionOperatorIdColumn].Value?.ParseLong() ?? 0;
                PermissionContentDAOs.Add(PermissionContentDAO);
            }
            await DataContext.PermissionContent.BulkMergeAsync(PermissionContentDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_PermissionActionMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.PermissionActionMappingDAOs = new List<PermissionActionMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int PermissionIdColumn = StartColumn + columns.IndexOf("PermissionId");
            int ActionIdColumn = StartColumn + columns.IndexOf("ActionId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PermissionActionMappingDAO PermissionActionMappingDAO = new PermissionActionMappingDAO();
                PermissionActionMappingDAO.PermissionId = ExcelWorksheet.Cells[row, PermissionIdColumn].Value?.ParseLong() ?? 0;
                PermissionActionMappingDAO.ActionId = ExcelWorksheet.Cells[row, ActionIdColumn].Value?.ParseLong() ?? 0;
                PermissionActionMappingDAOs.Add(PermissionActionMappingDAO);
            }
            await DataContext.PermissionActionMapping.BulkMergeAsync(PermissionActionMappingDAOs);
        }
        protected async Task Given_PermissionOperator(ExcelWorksheet ExcelWorksheet)
        {
            this.PermissionOperatorDAOs = new List<PermissionOperatorDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int FieldTypeIdColumn = StartColumn + columns.IndexOf("FieldTypeId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                PermissionOperatorDAO PermissionOperatorDAO = new PermissionOperatorDAO();
                PermissionOperatorDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                PermissionOperatorDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                PermissionOperatorDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                PermissionOperatorDAO.FieldTypeId = ExcelWorksheet.Cells[row, FieldTypeIdColumn].Value?.ParseLong() ?? 0;
                PermissionOperatorDAOs.Add(PermissionOperatorDAO);
            }
            await DataContext.PermissionOperator.BulkMergeAsync(PermissionOperatorDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        #endregion
    }
}
