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
        protected List<Dim_AddressMappingDAO> Dim_AddressMappingDAOs { get; set; }
        protected List<Dim_AlbumDAO> Dim_AlbumDAOs { get; set; }
        protected List<Dim_AppUserDAO> Dim_AppUserDAOs { get; set; }
        protected List<Dim_AppUserMappingDAO> Dim_AppUserMappingDAOs { get; set; }
        protected List<Dim_BrandDAO> Dim_BrandDAOs { get; set; }
        protected List<Dim_BrandInStoreProductGroupingMappingDAO> Dim_BrandInStoreProductGroupingMappingDAOs { get; set; }
        protected List<Dim_CategoryDAO> Dim_CategoryDAOs { get; set; }
        protected List<Dim_DateDAO> Dim_DateDAOs { get; set; }
        protected List<Dim_DateMappingDAO> Dim_DateMappingDAOs { get; set; }
        protected List<Dim_DirectSalesOrderSourceTypeDAO> Dim_DirectSalesOrderSourceTypeDAOs { get; set; }
        protected List<Dim_DistrictDAO> Dim_DistrictDAOs { get; set; }
        protected List<Dim_EditedPriceStatusDAO> Dim_EditedPriceStatusDAOs { get; set; }
        protected List<Dim_ERouteTypeDAO> Dim_ERouteTypeDAOs { get; set; }
        protected List<Dim_ErpApprovalStateDAO> Dim_ErpApprovalStateDAOs { get; set; }
        protected List<Dim_GeneralApprovalStateDAO> Dim_GeneralApprovalStateDAOs { get; set; }
        protected List<Dim_ItemDAO> Dim_ItemDAOs { get; set; }
        protected List<Dim_ItemMappingDAO> Dim_ItemMappingDAOs { get; set; }
        protected List<Dim_KpiCriteriaGeneralDAO> Dim_KpiCriteriaGeneralDAOs { get; set; }
        protected List<Dim_KpiGeneralDAO> Dim_KpiGeneralDAOs { get; set; }
        protected List<Dim_MonthDAO> Dim_MonthDAOs { get; set; }
        protected List<Dim_OrganizationDAO> Dim_OrganizationDAOs { get; set; }
        protected List<Dim_POSMTransactionTypeDAO> Dim_POSMTransactionTypeDAOs { get; set; }
        protected List<Dim_ProblemStatusDAO> Dim_ProblemStatusDAOs { get; set; }
        protected List<Dim_ProblemTypeDAO> Dim_ProblemTypeDAOs { get; set; }
        protected List<Dim_ProductDAO> Dim_ProductDAOs { get; set; }
        protected List<Dim_ProductGroupingDAO> Dim_ProductGroupingDAOs { get; set; }
        protected List<Dim_ProductTypeDAO> Dim_ProductTypeDAOs { get; set; }
        protected List<Dim_ProvinceDAO> Dim_ProvinceDAOs { get; set; }
        protected List<Dim_QuarterDAO> Dim_QuarterDAOs { get; set; }
        protected List<Dim_RequestStateDAO> Dim_RequestStateDAOs { get; set; }
        protected List<Dim_ShowingItemDAO> Dim_ShowingItemDAOs { get; set; }
        protected List<Dim_ShowingItemMappingDAO> Dim_ShowingItemMappingDAOs { get; set; }
        protected List<Dim_StatusDAO> Dim_StatusDAOs { get; set; }
        protected List<Dim_StoreApprovalStateDAO> Dim_StoreApprovalStateDAOs { get; set; }
        protected List<Dim_StoreCheckingStatusDAO> Dim_StoreCheckingStatusDAOs { get; set; }
        protected List<Dim_StoreDAO> Dim_StoreDAOs { get; set; }
        protected List<Dim_StoreGroupingDAO> Dim_StoreGroupingDAOs { get; set; }
        protected List<Dim_StoreMappingDAO> Dim_StoreMappingDAOs { get; set; }
        protected List<Dim_StoreScoutingStatusDAO> Dim_StoreScoutingStatusDAOs { get; set; }
        protected List<Dim_StoreScoutingTypeDAO> Dim_StoreScoutingTypeDAOs { get; set; }
        protected List<Dim_StoreStatusDAO> Dim_StoreStatusDAOs { get; set; }
        protected List<Dim_StoreStatusHistoryTypeDAO> Dim_StoreStatusHistoryTypeDAOs { get; set; }
        protected List<Dim_StoreTypeDAO> Dim_StoreTypeDAOs { get; set; }
        protected List<Dim_SupplierDAO> Dim_SupplierDAOs { get; set; }
        protected List<Dim_TaxTypeDAO> Dim_TaxTypeDAOs { get; set; }
        protected List<Dim_TransactionTypeDAO> Dim_TransactionTypeDAOs { get; set; }
        protected List<Dim_UnitOfMeasureDAO> Dim_UnitOfMeasureDAOs { get; set; }
        protected List<Dim_UsedVariationDAO> Dim_UsedVariationDAOs { get; set; }
        protected List<Dim_WardDAO> Dim_WardDAOs { get; set; }
        protected List<Dim_WarehouseDAO> Dim_WarehouseDAOs { get; set; }
        protected List<Dim_YearDAO> Dim_YearDAOs { get; set; }
        protected List<Fact_BrandHistoryDAO> Fact_BrandHistoryDAOs { get; set; }
        protected List<Fact_BrandInStoreDAO> Fact_BrandInStoreDAOs { get; set; }
        protected List<Fact_DirectSalesOrderDAO> Fact_DirectSalesOrderDAOs { get; set; }
        protected List<Fact_DirectSalesOrderTransactionDAO> Fact_DirectSalesOrderTransactionDAOs { get; set; }
        protected List<Fact_ImageDAO> Fact_ImageDAOs { get; set; }
        protected List<Fact_IndirectSalesOrderDAO> Fact_IndirectSalesOrderDAOs { get; set; }
        protected List<Fact_IndirectSalesOrderTransactionDAO> Fact_IndirectSalesOrderTransactionDAOs { get; set; }
        protected List<Fact_KpiGeneralContentDAO> Fact_KpiGeneralContentDAOs { get; set; }
        protected List<Fact_POSMTransactionDAO> Fact_POSMTransactionDAOs { get; set; }
        protected List<Fact_ProblemDAO> Fact_ProblemDAOs { get; set; }
        protected List<Fact_ProductGroupingHistoryDAO> Fact_ProductGroupingHistoryDAOs { get; set; }
        protected List<Fact_StoreCheckingDAO> Fact_StoreCheckingDAOs { get; set; }
        protected List<Fact_StoreHistoryDAO> Fact_StoreHistoryDAOs { get; set; }
        protected List<Fact_StoreScoutingDAO> Fact_StoreScoutingDAOs { get; set; }
        protected List<Fact_StoreStatusHistoryDAO> Fact_StoreStatusHistoryDAOs { get; set; }
        protected List<Fact_StoreUncheckingDAO> Fact_StoreUncheckingDAOs { get; set; }
        protected List<View_KPIDoanhThuDAO> View_KPIDoanhThuDAOs { get; set; }
        #endregion
        protected async Task LoadDWExcel(string path, string DatabaseName)
        {
            string SnapshotName = $"{DatabaseName}_Snapshot";
            this.DWContext = ServiceProvider.GetService<DWContext>();

            int result = await DWContext.Dim_Album.FromSqlRaw($@"SELECT * FROM sys.databases WHERE NAME = '{SnapshotName}'").CountAsync();
            if (result > 0)
            {
                return;
            }

            MemoryStream MemoryStream = ReadFile(path);
            using (ExcelPackage excelPackage = new ExcelPackage(MemoryStream))
            {
                //ExcelWorksheet wsAddressMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimAddressMapping").FirstOrDefault();
                //if (wsAddressMapping != null)
                //    await Given_DW_Dim_AddressMapping(wsAddressMapping);
                ExcelWorksheet wsAlbum = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimAlbum").FirstOrDefault();
                if (wsAlbum != null)
                    await Given_DW_Dim_Album(wsAlbum);
                ExcelWorksheet wsAppUser = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimAppUser").FirstOrDefault();
                if (wsAppUser != null)
                    await Given_DW_Dim_AppUser(wsAppUser);
                ExcelWorksheet wsAppUserMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimAppUserMapping").FirstOrDefault();
                if (wsAppUserMapping != null)
                    await Given_DW_Dim_AppUserMapping(wsAppUserMapping);
                ExcelWorksheet wsBrand = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimBrand").FirstOrDefault();
                if (wsBrand != null)
                    await Given_DW_Dim_Brand(wsBrand);
                ExcelWorksheet wsBrandInStoreProductGroupingMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DBISPGM").FirstOrDefault();
                if (wsBrandInStoreProductGroupingMapping != null)
                    await Given_DW_Dim_BrandInStoreProductGroupingMapping(wsBrandInStoreProductGroupingMapping);
                ExcelWorksheet wsCategory = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimCategory").FirstOrDefault();
                if (wsCategory != null)
                    await Given_DW_Dim_Category(wsCategory);
                //ExcelWorksheet wsDate = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimDate").FirstOrDefault();
                //if (wsDate != null)
                //    await Given_DW_Dim_Date(wsDate);
                //ExcelWorksheet wsDateMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimDateMapping").FirstOrDefault();
                //if (wsDateMapping != null)
                //    await Given_DW_Dim_DateMapping(wsDateMapping);
                //ExcelWorksheet wsDirectSalesOrderSourceType = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimDirectSalesOrderSourceType").FirstOrDefault();
                //if (wsDirectSalesOrderSourceType != null)
                //    await Given_DW_Dim_DirectSalesOrderSourceType(wsDirectSalesOrderSourceType);
                ExcelWorksheet wsDistrict = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimDistrict").FirstOrDefault();
                if (wsDistrict != null)
                    await Given_DW_Dim_District(wsDistrict);
                //ExcelWorksheet wsEditedPriceStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimEditedPriceStatus").FirstOrDefault();
                //if (wsEditedPriceStatus != null)
                //    await Given_DW_Dim_EditedPriceStatus(wsEditedPriceStatus);
                //ExcelWorksheet wsERouteType = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimERouteType").FirstOrDefault();
                //if (wsERouteType != null)
                //    await Given_DW_Dim_ERouteType(wsERouteType);
                //ExcelWorksheet wsErpApprovalState = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimErpApprovalState").FirstOrDefault();
                //if (wsErpApprovalState != null)
                //    await Given_DW_Dim_ErpApprovalState(wsErpApprovalState);
                //ExcelWorksheet wsGeneralApprovalState = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimGeneralApprovalState").FirstOrDefault();
                //if (wsGeneralApprovalState != null)
                //    await Given_DW_Dim_GeneralApprovalState(wsGeneralApprovalState);
                ExcelWorksheet wsItem = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimItem").FirstOrDefault();
                if (wsItem != null)
                    await Given_DW_Dim_Item(wsItem);
                ExcelWorksheet wsItemMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimItemMapping").FirstOrDefault();
                if (wsItemMapping != null)
                    await Given_DW_Dim_ItemMapping(wsItemMapping);
                ExcelWorksheet wsKpiCriteriaGeneral = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimKpiCriteriaGeneral").FirstOrDefault();
                if (wsKpiCriteriaGeneral != null)
                    await Given_DW_Dim_KpiCriteriaGeneral(wsKpiCriteriaGeneral);
                ExcelWorksheet wsKpiGeneral = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimKpiGeneral").FirstOrDefault();
                if (wsKpiGeneral != null)
                    await Given_DW_Dim_KpiGeneral(wsKpiGeneral);
                //ExcelWorksheet wsMonth = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimMonth").FirstOrDefault();
                //if (wsMonth != null)
                //    await Given_DW_Dim_Month(wsMonth);
                ExcelWorksheet wsOrganization = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimOrganization").FirstOrDefault();
                if (wsOrganization != null)
                    await Given_DW_Dim_Organization(wsOrganization);
                //ExcelWorksheet wsPOSMTransactionType = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimPOSMTransactionType").FirstOrDefault();
                //if (wsPOSMTransactionType != null)
                //    await Given_DW_Dim_POSMTransactionType(wsPOSMTransactionType);
                //ExcelWorksheet wsProblemStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimProblemStatus").FirstOrDefault();
                //if (wsProblemStatus != null)
                //    await Given_DW_Dim_ProblemStatus(wsProblemStatus);
                ExcelWorksheet wsProblemType = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimProblemType").FirstOrDefault();
                if (wsProblemType != null)
                    await Given_DW_Dim_ProblemType(wsProblemType);
                ExcelWorksheet wsProduct = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimProduct").FirstOrDefault();
                if (wsProduct != null)
                    await Given_DW_Dim_Product(wsProduct);
                ExcelWorksheet wsProductGrouping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimProductGrouping").FirstOrDefault();
                if (wsProductGrouping != null)
                    await Given_DW_Dim_ProductGrouping(wsProductGrouping);
                ExcelWorksheet wsProductType = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimProductType").FirstOrDefault();
                if (wsProductType != null)
                    await Given_DW_Dim_ProductType(wsProductType);
                ExcelWorksheet wsProvince = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimProvince").FirstOrDefault();
                if (wsProvince != null)
                    await Given_DW_Dim_Province(wsProvince);
                //ExcelWorksheet wsQuarter = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimQuarter").FirstOrDefault();
                //if (wsQuarter != null)
                //    await Given_DW_Dim_Quarter(wsQuarter);
                //ExcelWorksheet wsRequestState = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimRequestState").FirstOrDefault();
                //if (wsRequestState != null)
                //    await Given_DW_Dim_RequestState(wsRequestState);
                ExcelWorksheet wsShowingItem = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimShowingItem").FirstOrDefault();
                if (wsShowingItem != null)
                    await Given_DW_Dim_ShowingItem(wsShowingItem);
                ExcelWorksheet wsShowingItemMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimShowingItemMapping").FirstOrDefault();
                if (wsShowingItemMapping != null)
                    await Given_DW_Dim_ShowingItemMapping(wsShowingItemMapping);
                ExcelWorksheet wsStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimStatus").FirstOrDefault();
                if (wsStatus != null)
                    await Given_DW_Dim_Status(wsStatus);
                //ExcelWorksheet wsStoreApprovalState = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimStoreApprovalState").FirstOrDefault();
                //if (wsStoreApprovalState != null)
                //    await Given_DW_Dim_StoreApprovalState(wsStoreApprovalState);
                //ExcelWorksheet wsStoreCheckingStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimStoreCheckingStatus").FirstOrDefault();
                //if (wsStoreCheckingStatus != null)
                //    await Given_DW_Dim_StoreCheckingStatus(wsStoreCheckingStatus);
                ExcelWorksheet wsStore = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimStore").FirstOrDefault();
                if (wsStore != null)
                    await Given_DW_Dim_Store(wsStore);
                ExcelWorksheet wsStoreGrouping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimStoreGrouping").FirstOrDefault();
                if (wsStoreGrouping != null)
                    await Given_DW_Dim_StoreGrouping(wsStoreGrouping);
                ExcelWorksheet wsStoreMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimStoreMapping").FirstOrDefault();
                if (wsStoreMapping != null)
                    await Given_DW_Dim_StoreMapping(wsStoreMapping);
                //ExcelWorksheet wsStoreScoutingStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimStoreScoutingStatus").FirstOrDefault();
                //if (wsStoreScoutingStatus != null)
                //    await Given_DW_Dim_StoreScoutingStatus(wsStoreScoutingStatus);
                ExcelWorksheet wsStoreScoutingType = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimStoreScoutingType").FirstOrDefault();
                if (wsStoreScoutingType != null)
                    await Given_DW_Dim_StoreScoutingType(wsStoreScoutingType);
                ExcelWorksheet wsStoreStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimStoreStatus").FirstOrDefault();
                if (wsStoreStatus != null)
                    await Given_DW_Dim_StoreStatus(wsStoreStatus);
                //ExcelWorksheet wsStoreStatusHistoryType = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimStoreStatusHistoryType").FirstOrDefault();
                //if (wsStoreStatusHistoryType != null)
                //    await Given_DW_Dim_StoreStatusHistoryType(wsStoreStatusHistoryType);
                ExcelWorksheet wsStoreType = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimStoreType").FirstOrDefault();
                if (wsStoreType != null)
                    await Given_DW_Dim_StoreType(wsStoreType);
                //ExcelWorksheet wsSupplier = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimSupplier").FirstOrDefault();
                //if (wsSupplier != null)
                //    await Given_DW_Dim_Supplier(wsSupplier);
                //ExcelWorksheet wsTaxType = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimTaxType").FirstOrDefault();
                //if (wsTaxType != null)
                //    await Given_DW_Dim_TaxType(wsTaxType);
                //ExcelWorksheet wsTransactionType = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimTransactionType").FirstOrDefault();
                //if (wsTransactionType != null)
                //    await Given_DW_Dim_TransactionType(wsTransactionType);
                ExcelWorksheet wsUnitOfMeasure = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimUnitOfMeasure").FirstOrDefault();
                if (wsUnitOfMeasure != null)
                    await Given_DW_Dim_UnitOfMeasure(wsUnitOfMeasure);
                ExcelWorksheet wsUsedVariation = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimUsedVariation").FirstOrDefault();
                if (wsUsedVariation != null)
                    await Given_DW_Dim_UsedVariation(wsUsedVariation);
                ExcelWorksheet wsWard = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimWard").FirstOrDefault();
                if (wsWard != null)
                    await Given_DW_Dim_Ward(wsWard);
                //ExcelWorksheet wsWarehouse = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimWarehouse").FirstOrDefault();
                //if (wsWarehouse != null)
                //    await Given_DW_Dim_Warehouse(wsWarehouse);
                //ExcelWorksheet wsYear = excelPackage.Workbook.Worksheets.Where(x => x.Name == "DimYear").FirstOrDefault();
                //if (wsYear != null)
                //    await Given_DW_Dim_Year(wsYear);
                ExcelWorksheet wsBrandHistory = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactBrandHistory").FirstOrDefault();
                if (wsBrandHistory != null)
                    await Given_DW_Fact_BrandHistory(wsBrandHistory);
                ExcelWorksheet wsBrandInStore = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactBrandInStore").FirstOrDefault();
                if (wsBrandInStore != null)
                    await Given_DW_Fact_BrandInStore(wsBrandInStore);
                ExcelWorksheet wsDirectSalesOrder = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactDirectSalesOrder").FirstOrDefault();
                if (wsDirectSalesOrder != null)
                    await Given_DW_Fact_DirectSalesOrder(wsDirectSalesOrder);
                ExcelWorksheet wsDirectSalesOrderTransaction = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactDirectSalesOrderTransaction").FirstOrDefault();
                if (wsDirectSalesOrderTransaction != null)
                    await Given_DW_Fact_DirectSalesOrderTransaction(wsDirectSalesOrderTransaction);
                ExcelWorksheet wsImage = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactImage").FirstOrDefault();
                if (wsImage != null)
                    await Given_DW_Fact_Image(wsImage);
                ExcelWorksheet wsIndirectSalesOrder = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactIndirectSalesOrder").FirstOrDefault();
                if (wsIndirectSalesOrder != null)
                    await Given_DW_Fact_IndirectSalesOrder(wsIndirectSalesOrder);
                ExcelWorksheet wsIndirectSalesOrderTransaction = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactIndirectSalesOrderTransaction").FirstOrDefault();
                if (wsIndirectSalesOrderTransaction != null)
                    await Given_DW_Fact_IndirectSalesOrderTransaction(wsIndirectSalesOrderTransaction);
                ExcelWorksheet wsKpiGeneralContent = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactKpiGeneralContent").FirstOrDefault();
                if (wsKpiGeneralContent != null)
                    await Given_DW_Fact_KpiGeneralContent(wsKpiGeneralContent);
                //ExcelWorksheet wsPOSMTransaction = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactPOSMTransaction").FirstOrDefault();
                //if (wsPOSMTransaction != null)
                //    await Given_DW_Fact_POSMTransaction(wsPOSMTransaction);
                ExcelWorksheet wsProblem = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactProblem").FirstOrDefault();
                if (wsProblem != null)
                    await Given_DW_Fact_Problem(wsProblem);
                ExcelWorksheet wsProductGroupingHistory = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactProductGroupingHistory").FirstOrDefault();
                if (wsProductGroupingHistory != null)
                    await Given_DW_Fact_ProductGroupingHistory(wsProductGroupingHistory);
                ExcelWorksheet wsStoreChecking = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactStoreChecking").FirstOrDefault();
                if (wsStoreChecking != null)
                    await Given_DW_Fact_StoreChecking(wsStoreChecking);
                ExcelWorksheet wsStoreHistory = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactStoreHistory").FirstOrDefault();
                if (wsStoreHistory != null)
                    await Given_DW_Fact_StoreHistory(wsStoreHistory);
                ExcelWorksheet wsStoreScouting = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactStoreScouting").FirstOrDefault();
                if (wsStoreScouting != null)
                    await Given_DW_Fact_StoreScouting(wsStoreScouting);
                ExcelWorksheet wsStoreStatusHistory = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactStoreStatusHistory").FirstOrDefault();
                if (wsStoreStatusHistory != null)
                    await Given_DW_Fact_StoreStatusHistory(wsStoreStatusHistory);
                ExcelWorksheet wsStoreUnchecking = excelPackage.Workbook.Worksheets.Where(x => x.Name == "FactStoreUnchecking").FirstOrDefault();
                if (wsStoreUnchecking != null)
                    await Given_DW_Fact_StoreUnchecking(wsStoreUnchecking);
                //ExcelWorksheet wsKPIDoanhThu = excelPackage.Workbook.Worksheets.Where(x => x.Name == "ViewKPIDoanhThu").FirstOrDefault();
                //if (wsKPIDoanhThu != null)
                //    await Given_DW_View_KPIDoanhThu(wsKPIDoanhThu);

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
        #region Given methods
        protected async Task Given_DW_Dim_AddressMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_AddressMappingDAOs = new List<Dim_AddressMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int AddressMappingIdColumn = StartColumn + columns.IndexOf("AddressMappingId");
            int WardIdColumn = StartColumn + columns.IndexOf("WardId");
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int CountryIdColumn = StartColumn + columns.IndexOf("CountryId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_AddressMappingDAO Dim_AddressMappingDAO = new Dim_AddressMappingDAO();
                Dim_AddressMappingDAO.AddressMappingId = ExcelWorksheet.Cells[row, AddressMappingIdColumn].Value?.ParseLong() ?? 0;
                Dim_AddressMappingDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                Dim_AddressMappingDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseNullLong();
                Dim_AddressMappingDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                Dim_AddressMappingDAO.CountryId = ExcelWorksheet.Cells[row, CountryIdColumn].Value?.ParseNullLong();
                Dim_AddressMappingDAOs.Add(Dim_AddressMappingDAO);
            }
            await DWContext.Dim_AddressMapping.BulkMergeAsync(Dim_AddressMappingDAOs);
        }
        protected async Task Given_DW_Dim_Album(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_AlbumDAOs = new List<Dim_AlbumDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int AlbumIdColumn = StartColumn + columns.IndexOf("AlbumId");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_AlbumDAO Dim_AlbumDAO = new Dim_AlbumDAO();
                Dim_AlbumDAO.AlbumId = ExcelWorksheet.Cells[row, AlbumIdColumn].Value?.ParseLong() ?? 0;
                Dim_AlbumDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_AlbumDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_AlbumDAOs.Add(Dim_AlbumDAO);
            }
            await DWContext.Dim_Album.BulkMergeAsync(Dim_AlbumDAOs);
        }
        protected async Task Given_DW_Dim_AppUser(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_AppUserDAOs = new List<Dim_AppUserDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int UsernameColumn = StartColumn + columns.IndexOf("Username");
            int DisplayNameColumn = StartColumn + columns.IndexOf("DisplayName");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int EmailColumn = StartColumn + columns.IndexOf("Email");
            int PhoneColumn = StartColumn + columns.IndexOf("Phone");
            int SexIdColumn = StartColumn + columns.IndexOf("SexId");
            int BirthdayColumn = StartColumn + columns.IndexOf("Birthday");
            int AvatarColumn = StartColumn + columns.IndexOf("Avatar");
            int DepartmentColumn = StartColumn + columns.IndexOf("Department");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
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
                Dim_AppUserDAO Dim_AppUserDAO = new Dim_AppUserDAO();
                Dim_AppUserDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                Dim_AppUserDAO.Username = ExcelWorksheet.Cells[row, UsernameColumn].Value?.ParseString();
                Dim_AppUserDAO.DisplayName = ExcelWorksheet.Cells[row, DisplayNameColumn].Value?.ParseString();
                Dim_AppUserDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                Dim_AppUserDAO.Email = ExcelWorksheet.Cells[row, EmailColumn].Value?.ParseString();
                Dim_AppUserDAO.Phone = ExcelWorksheet.Cells[row, PhoneColumn].Value?.ParseString();
                Dim_AppUserDAO.SexId = ExcelWorksheet.Cells[row, SexIdColumn].Value?.ParseLong() ?? 0;
                Dim_AppUserDAO.Birthday = ExcelWorksheet.Cells[row, BirthdayColumn].Value?.ParseNullDateTime();
                Dim_AppUserDAO.Avatar = ExcelWorksheet.Cells[row, AvatarColumn].Value?.ParseString();
                Dim_AppUserDAO.Department = ExcelWorksheet.Cells[row, DepartmentColumn].Value?.ParseString();
                Dim_AppUserDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Dim_AppUserDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_AppUserDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_AppUserDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_AppUserDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_AppUserDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_AppUserDAOs.Add(Dim_AppUserDAO);
            }
            await DWContext.Dim_AppUser.BulkMergeAsync(Dim_AppUserDAOs);
        }
        protected async Task Given_DW_Dim_AppUserMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_AppUserMappingDAOs = new List<Dim_AppUserMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int AppUserMappingIdColumn = StartColumn + columns.IndexOf("AppUserMappingId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_AppUserMappingDAO Dim_AppUserMappingDAO = new Dim_AppUserMappingDAO();
                Dim_AppUserMappingDAO.AppUserMappingId = ExcelWorksheet.Cells[row, AppUserMappingIdColumn].Value?.ParseLong() ?? 0;
                Dim_AppUserMappingDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                Dim_AppUserMappingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseNullLong();
                Dim_AppUserMappingDAOs.Add(Dim_AppUserMappingDAO);
            }
            await DWContext.Dim_AppUserMapping.BulkMergeAsync(Dim_AppUserMappingDAOs);
        }
        protected async Task Given_DW_Dim_Brand(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_BrandDAOs = new List<Dim_BrandDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int BrandIdColumn = StartColumn + columns.IndexOf("BrandId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
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
                Dim_BrandDAO Dim_BrandDAO = new Dim_BrandDAO();
                Dim_BrandDAO.BrandId = ExcelWorksheet.Cells[row, BrandIdColumn].Value?.ParseLong() ?? 0;
                Dim_BrandDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_BrandDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_BrandDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_BrandDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                Dim_BrandDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_BrandDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_BrandDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_BrandDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_BrandDAOs.Add(Dim_BrandDAO);
            }
            await DWContext.Dim_Brand.BulkMergeAsync(Dim_BrandDAOs);
        }
        protected async Task Given_DW_Dim_BrandInStoreProductGroupingMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_BrandInStoreProductGroupingMappingDAOs = new List<Dim_BrandInStoreProductGroupingMappingDAO>();
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
                Dim_BrandInStoreProductGroupingMappingDAO Dim_BrandInStoreProductGroupingMappingDAO = new Dim_BrandInStoreProductGroupingMappingDAO();
                Dim_BrandInStoreProductGroupingMappingDAO.BrandInStoreId = ExcelWorksheet.Cells[row, BrandInStoreIdColumn].Value?.ParseLong() ?? 0;
                Dim_BrandInStoreProductGroupingMappingDAO.ProductGroupingId = ExcelWorksheet.Cells[row, ProductGroupingIdColumn].Value?.ParseLong() ?? 0;
                Dim_BrandInStoreProductGroupingMappingDAOs.Add(Dim_BrandInStoreProductGroupingMappingDAO);
            }
            await DWContext.Dim_BrandInStoreProductGroupingMapping.BulkMergeAsync(Dim_BrandInStoreProductGroupingMappingDAOs);
        }
        protected async Task Given_DW_Dim_Category(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_CategoryDAOs = new List<Dim_CategoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int CategoryIdColumn = StartColumn + columns.IndexOf("CategoryId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int CategoryParentIdColumn = StartColumn + columns.IndexOf("CategoryParentId");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int LevelColumn = StartColumn + columns.IndexOf("Level");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int ImageIdColumn = StartColumn + columns.IndexOf("ImageId");
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
                Dim_CategoryDAO Dim_CategoryDAO = new Dim_CategoryDAO();
                Dim_CategoryDAO.CategoryId = ExcelWorksheet.Cells[row, CategoryIdColumn].Value?.ParseLong() ?? 0;
                Dim_CategoryDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_CategoryDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_CategoryDAO.CategoryParentId = ExcelWorksheet.Cells[row, CategoryParentIdColumn].Value?.ParseNullLong();
                Dim_CategoryDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                Dim_CategoryDAO.Level = ExcelWorksheet.Cells[row, LevelColumn].Value?.ParseLong() ?? 0;
                Dim_CategoryDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_CategoryDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseNullLong();
                Dim_CategoryDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_CategoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_CategoryDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_CategoryDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_CategoryDAOs.Add(Dim_CategoryDAO);
            }
            await DWContext.Dim_Category.BulkMergeAsync(Dim_CategoryDAOs);
        }
        protected async Task Given_DW_Dim_Date(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_DateDAOs = new List<Dim_DateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int DateIdColumn = StartColumn + columns.IndexOf("DateId");
            int DateColumn = StartColumn + columns.IndexOf("Date");
            int DayColumn = StartColumn + columns.IndexOf("Day");
            int DayOfWeekNameColumn = StartColumn + columns.IndexOf("DayOfWeekName");
            int DayOfWeekColumn = StartColumn + columns.IndexOf("DayOfWeek");
            int DayOfYearColumn = StartColumn + columns.IndexOf("DayOfYear");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_DateDAO Dim_DateDAO = new Dim_DateDAO();
                Dim_DateDAO.DateId = ExcelWorksheet.Cells[row, DateIdColumn].Value?.ParseLong() ?? 0;
                Dim_DateDAO.Date = ExcelWorksheet.Cells[row, DateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_DateDAO.Day = ExcelWorksheet.Cells[row, DayColumn].Value?.ParseLong() ?? 0;
                Dim_DateDAO.DayOfWeekName = ExcelWorksheet.Cells[row, DayOfWeekNameColumn].Value?.ParseString();
                Dim_DateDAO.DayOfWeek = ExcelWorksheet.Cells[row, DayOfWeekColumn].Value?.ParseLong() ?? 0;
                Dim_DateDAO.DayOfYear = ExcelWorksheet.Cells[row, DayOfYearColumn].Value?.ParseLong() ?? 0;
                Dim_DateDAOs.Add(Dim_DateDAO);
            }
            await DWContext.Dim_Date.BulkMergeAsync(Dim_DateDAOs);
        }
        protected async Task Given_DW_Dim_DateMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_DateMappingDAOs = new List<Dim_DateMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int DateMappingIdColumn = StartColumn + columns.IndexOf("DateMappingId");
            int DateColumn = StartColumn + columns.IndexOf("Date");
            int DateIdColumn = StartColumn + columns.IndexOf("DateId");
            int MonthIdColumn = StartColumn + columns.IndexOf("MonthId");
            int QuarterIdColumn = StartColumn + columns.IndexOf("QuarterId");
            int YearIdColumn = StartColumn + columns.IndexOf("YearId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_DateMappingDAO Dim_DateMappingDAO = new Dim_DateMappingDAO();
                Dim_DateMappingDAO.DateMappingId = ExcelWorksheet.Cells[row, DateMappingIdColumn].Value?.ParseLong() ?? 0;
                Dim_DateMappingDAO.Date = ExcelWorksheet.Cells[row, DateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_DateMappingDAO.DateId = ExcelWorksheet.Cells[row, DateIdColumn].Value?.ParseLong() ?? 0;
                Dim_DateMappingDAO.MonthId = ExcelWorksheet.Cells[row, MonthIdColumn].Value?.ParseLong() ?? 0;
                Dim_DateMappingDAO.QuarterId = ExcelWorksheet.Cells[row, QuarterIdColumn].Value?.ParseLong() ?? 0;
                Dim_DateMappingDAO.YearId = ExcelWorksheet.Cells[row, YearIdColumn].Value?.ParseLong() ?? 0;
                Dim_DateMappingDAOs.Add(Dim_DateMappingDAO);
            }
            await DWContext.Dim_DateMapping.BulkMergeAsync(Dim_DateMappingDAOs);
        }
        protected async Task Given_DW_Dim_DirectSalesOrderSourceType(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_DirectSalesOrderSourceTypeDAOs = new List<Dim_DirectSalesOrderSourceTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int DirectSalesOrderSourceTypeIdColumn = StartColumn + columns.IndexOf("DirectSalesOrderSourceTypeId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_DirectSalesOrderSourceTypeDAO Dim_DirectSalesOrderSourceTypeDAO = new Dim_DirectSalesOrderSourceTypeDAO();
                Dim_DirectSalesOrderSourceTypeDAO.DirectSalesOrderSourceTypeId = ExcelWorksheet.Cells[row, DirectSalesOrderSourceTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_DirectSalesOrderSourceTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_DirectSalesOrderSourceTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_DirectSalesOrderSourceTypeDAOs.Add(Dim_DirectSalesOrderSourceTypeDAO);
            }
            await DWContext.Dim_DirectSalesOrderSourceType.BulkMergeAsync(Dim_DirectSalesOrderSourceTypeDAOs);
        }
        protected async Task Given_DW_Dim_District(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_DistrictDAOs = new List<Dim_DistrictDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
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
                Dim_DistrictDAO Dim_DistrictDAO = new Dim_DistrictDAO();
                Dim_DistrictDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseLong() ?? 0;
                Dim_DistrictDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_DistrictDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_DistrictDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseNullLong();
                Dim_DistrictDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseLong() ?? 0;
                Dim_DistrictDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_DistrictDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_DistrictDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_DistrictDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_DistrictDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_DistrictDAOs.Add(Dim_DistrictDAO);
            }
            await DWContext.Dim_District.BulkMergeAsync(Dim_DistrictDAOs);
        }
        protected async Task Given_DW_Dim_EditedPriceStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_EditedPriceStatusDAOs = new List<Dim_EditedPriceStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int EditedPriceStatusIdColumn = StartColumn + columns.IndexOf("EditedPriceStatusId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_EditedPriceStatusDAO Dim_EditedPriceStatusDAO = new Dim_EditedPriceStatusDAO();
                Dim_EditedPriceStatusDAO.EditedPriceStatusId = ExcelWorksheet.Cells[row, EditedPriceStatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_EditedPriceStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_EditedPriceStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_EditedPriceStatusDAOs.Add(Dim_EditedPriceStatusDAO);
            }
            await DWContext.Dim_EditedPriceStatus.BulkMergeAsync(Dim_EditedPriceStatusDAOs);
        }
        protected async Task Given_DW_Dim_ERouteType(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ERouteTypeDAOs = new List<Dim_ERouteTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ERouteTypeIdColumn = StartColumn + columns.IndexOf("ERouteTypeId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_ERouteTypeDAO Dim_ERouteTypeDAO = new Dim_ERouteTypeDAO();
                Dim_ERouteTypeDAO.ERouteTypeId = ExcelWorksheet.Cells[row, ERouteTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_ERouteTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_ERouteTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_ERouteTypeDAOs.Add(Dim_ERouteTypeDAO);
            }
            await DWContext.Dim_ERouteType.BulkMergeAsync(Dim_ERouteTypeDAOs);
        }
        protected async Task Given_DW_Dim_ErpApprovalState(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ErpApprovalStateDAOs = new List<Dim_ErpApprovalStateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ErpApprovalStateIdColumn = StartColumn + columns.IndexOf("ErpApprovalStateId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_ErpApprovalStateDAO Dim_ErpApprovalStateDAO = new Dim_ErpApprovalStateDAO();
                Dim_ErpApprovalStateDAO.ErpApprovalStateId = ExcelWorksheet.Cells[row, ErpApprovalStateIdColumn].Value?.ParseLong() ?? 0;
                Dim_ErpApprovalStateDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_ErpApprovalStateDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_ErpApprovalStateDAOs.Add(Dim_ErpApprovalStateDAO);
            }
            await DWContext.Dim_ErpApprovalState.BulkMergeAsync(Dim_ErpApprovalStateDAOs);
        }
        protected async Task Given_DW_Dim_GeneralApprovalState(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_GeneralApprovalStateDAOs = new List<Dim_GeneralApprovalStateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int GeneralApprovalStateIdColumn = StartColumn + columns.IndexOf("GeneralApprovalStateId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_GeneralApprovalStateDAO Dim_GeneralApprovalStateDAO = new Dim_GeneralApprovalStateDAO();
                Dim_GeneralApprovalStateDAO.GeneralApprovalStateId = ExcelWorksheet.Cells[row, GeneralApprovalStateIdColumn].Value?.ParseLong() ?? 0;
                Dim_GeneralApprovalStateDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_GeneralApprovalStateDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_GeneralApprovalStateDAOs.Add(Dim_GeneralApprovalStateDAO);
            }
            await DWContext.Dim_GeneralApprovalState.BulkMergeAsync(Dim_GeneralApprovalStateDAOs);
        }
        protected async Task Given_DW_Dim_Item(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ItemDAOs = new List<Dim_ItemDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int ProductIdColumn = StartColumn + columns.IndexOf("ProductId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int SupplierCodeColumn = StartColumn + columns.IndexOf("SupplierCode");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int ScanCodeColumn = StartColumn + columns.IndexOf("ScanCode");
            int ERPCodeColumn = StartColumn + columns.IndexOf("ERPCode");
            int CategoryIdColumn = StartColumn + columns.IndexOf("CategoryId");
            int ProductTypeIdColumn = StartColumn + columns.IndexOf("ProductTypeId");
            int BrandIdColumn = StartColumn + columns.IndexOf("BrandId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int CodeGeneratorRuleIdColumn = StartColumn + columns.IndexOf("CodeGeneratorRuleId");
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
                Dim_ItemDAO Dim_ItemDAO = new Dim_ItemDAO();
                Dim_ItemDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                Dim_ItemDAO.ProductId = ExcelWorksheet.Cells[row, ProductIdColumn].Value?.ParseLong() ?? 0;
                Dim_ItemDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_ItemDAO.SupplierCode = ExcelWorksheet.Cells[row, SupplierCodeColumn].Value?.ParseString();
                Dim_ItemDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_ItemDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                Dim_ItemDAO.ScanCode = ExcelWorksheet.Cells[row, ScanCodeColumn].Value?.ParseString();
                Dim_ItemDAO.ERPCode = ExcelWorksheet.Cells[row, ERPCodeColumn].Value?.ParseString();
                Dim_ItemDAO.CategoryId = ExcelWorksheet.Cells[row, CategoryIdColumn].Value?.ParseLong() ?? 0;
                Dim_ItemDAO.ProductTypeId = ExcelWorksheet.Cells[row, ProductTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_ItemDAO.BrandId = ExcelWorksheet.Cells[row, BrandIdColumn].Value?.ParseNullLong();
                Dim_ItemDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                Dim_ItemDAO.CodeGeneratorRuleId = ExcelWorksheet.Cells[row, CodeGeneratorRuleIdColumn].Value?.ParseNullLong();
                Dim_ItemDAO.UnitOfMeasureGroupingId = ExcelWorksheet.Cells[row, UnitOfMeasureGroupingIdColumn].Value?.ParseNullLong();
                Dim_ItemDAO.SalePrice = ExcelWorksheet.Cells[row, SalePriceColumn].Value?.ParseNullDecimal();
                Dim_ItemDAO.RetailPrice = ExcelWorksheet.Cells[row, RetailPriceColumn].Value?.ParseNullDecimal();
                Dim_ItemDAO.TaxTypeId = ExcelWorksheet.Cells[row, TaxTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_ItemDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_ItemDAO.OtherName = ExcelWorksheet.Cells[row, OtherNameColumn].Value?.ParseString();
                Dim_ItemDAO.TechnicalName = ExcelWorksheet.Cells[row, TechnicalNameColumn].Value?.ParseString();
                Dim_ItemDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                Dim_ItemDAO.UsedVariationId = ExcelWorksheet.Cells[row, UsedVariationIdColumn].Value?.ParseLong() ?? 0;
                Dim_ItemDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_ItemDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ItemDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ItemDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_ItemDAOs.Add(Dim_ItemDAO);
            }
            await DWContext.Dim_Item.BulkMergeAsync(Dim_ItemDAOs);
        }
        protected async Task Given_DW_Dim_ItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ItemMappingDAOs = new List<Dim_ItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ItemMappingIdColumn = StartColumn + columns.IndexOf("ItemMappingId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int ProductIdColumn = StartColumn + columns.IndexOf("ProductId");
            int ProductGroupingIdColumn = StartColumn + columns.IndexOf("ProductGroupingId");
            int ProductTypeIdColumn = StartColumn + columns.IndexOf("ProductTypeId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int CategoryIdColumn = StartColumn + columns.IndexOf("CategoryId");
            int BrandIdColumn = StartColumn + columns.IndexOf("BrandId");
            int SupplierIdColumn = StartColumn + columns.IndexOf("SupplierId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_ItemMappingDAO Dim_ItemMappingDAO = new Dim_ItemMappingDAO();
                Dim_ItemMappingDAO.ItemMappingId = ExcelWorksheet.Cells[row, ItemMappingIdColumn].Value?.ParseLong() ?? 0;
                Dim_ItemMappingDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                Dim_ItemMappingDAO.ProductId = ExcelWorksheet.Cells[row, ProductIdColumn].Value?.ParseNullLong();
                Dim_ItemMappingDAO.ProductGroupingId = ExcelWorksheet.Cells[row, ProductGroupingIdColumn].Value?.ParseNullLong();
                Dim_ItemMappingDAO.ProductTypeId = ExcelWorksheet.Cells[row, ProductTypeIdColumn].Value?.ParseNullLong();
                Dim_ItemMappingDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseNullLong();
                Dim_ItemMappingDAO.CategoryId = ExcelWorksheet.Cells[row, CategoryIdColumn].Value?.ParseNullLong();
                Dim_ItemMappingDAO.BrandId = ExcelWorksheet.Cells[row, BrandIdColumn].Value?.ParseNullLong();
                Dim_ItemMappingDAO.SupplierId = ExcelWorksheet.Cells[row, SupplierIdColumn].Value?.ParseNullLong();
                Dim_ItemMappingDAOs.Add(Dim_ItemMappingDAO);
            }
            await DWContext.Dim_ItemMapping.BulkMergeAsync(Dim_ItemMappingDAOs);
        }
        protected async Task Given_DW_Dim_KpiCriteriaGeneral(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_KpiCriteriaGeneralDAOs = new List<Dim_KpiCriteriaGeneralDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int KpiCriteriaGeneralIdColumn = StartColumn + columns.IndexOf("KpiCriteriaGeneralId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_KpiCriteriaGeneralDAO Dim_KpiCriteriaGeneralDAO = new Dim_KpiCriteriaGeneralDAO();
                Dim_KpiCriteriaGeneralDAO.KpiCriteriaGeneralId = ExcelWorksheet.Cells[row, KpiCriteriaGeneralIdColumn].Value?.ParseLong() ?? 0;
                Dim_KpiCriteriaGeneralDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_KpiCriteriaGeneralDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_KpiCriteriaGeneralDAOs.Add(Dim_KpiCriteriaGeneralDAO);
            }
            await DWContext.Dim_KpiCriteriaGeneral.BulkMergeAsync(Dim_KpiCriteriaGeneralDAOs);
        }
        protected async Task Given_DW_Dim_KpiGeneral(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_KpiGeneralDAOs = new List<Dim_KpiGeneralDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int KpiGeneralIdColumn = StartColumn + columns.IndexOf("KpiGeneralId");
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
                Dim_KpiGeneralDAO Dim_KpiGeneralDAO = new Dim_KpiGeneralDAO();
                Dim_KpiGeneralDAO.KpiGeneralId = ExcelWorksheet.Cells[row, KpiGeneralIdColumn].Value?.ParseLong() ?? 0;
                Dim_KpiGeneralDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Dim_KpiGeneralDAO.EmployeeId = ExcelWorksheet.Cells[row, EmployeeIdColumn].Value?.ParseLong() ?? 0;
                Dim_KpiGeneralDAO.KpiYearId = ExcelWorksheet.Cells[row, KpiYearIdColumn].Value?.ParseLong() ?? 0;
                Dim_KpiGeneralDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_KpiGeneralDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                Dim_KpiGeneralDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_KpiGeneralDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_KpiGeneralDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_KpiGeneralDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_KpiGeneralDAOs.Add(Dim_KpiGeneralDAO);
            }
            await DWContext.Dim_KpiGeneral.BulkMergeAsync(Dim_KpiGeneralDAOs);
        }
        protected async Task Given_DW_Dim_Month(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_MonthDAOs = new List<Dim_MonthDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int MonthIdColumn = StartColumn + columns.IndexOf("MonthId");
            int MonthColumn = StartColumn + columns.IndexOf("Month");
            int YearColumn = StartColumn + columns.IndexOf("Year");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_MonthDAO Dim_MonthDAO = new Dim_MonthDAO();
                Dim_MonthDAO.MonthId = ExcelWorksheet.Cells[row, MonthIdColumn].Value?.ParseLong() ?? 0;
                Dim_MonthDAO.Month = ExcelWorksheet.Cells[row, MonthColumn].Value?.ParseLong() ?? 0;
                Dim_MonthDAO.Year = ExcelWorksheet.Cells[row, YearColumn].Value?.ParseLong() ?? 0;
                Dim_MonthDAOs.Add(Dim_MonthDAO);
            }
            await DWContext.Dim_Month.BulkMergeAsync(Dim_MonthDAOs);
        }
        protected async Task Given_DW_Dim_Organization(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_OrganizationDAOs = new List<Dim_OrganizationDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ParentIdColumn = StartColumn + columns.IndexOf("ParentId");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int LevelColumn = StartColumn + columns.IndexOf("Level");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int PhoneColumn = StartColumn + columns.IndexOf("Phone");
            int EmailColumn = StartColumn + columns.IndexOf("Email");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
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
                Dim_OrganizationDAO Dim_OrganizationDAO = new Dim_OrganizationDAO();
                Dim_OrganizationDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Dim_OrganizationDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_OrganizationDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_OrganizationDAO.ParentId = ExcelWorksheet.Cells[row, ParentIdColumn].Value?.ParseNullLong();
                Dim_OrganizationDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                Dim_OrganizationDAO.Level = ExcelWorksheet.Cells[row, LevelColumn].Value?.ParseLong() ?? 0;
                Dim_OrganizationDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_OrganizationDAO.Phone = ExcelWorksheet.Cells[row, PhoneColumn].Value?.ParseString();
                Dim_OrganizationDAO.Email = ExcelWorksheet.Cells[row, EmailColumn].Value?.ParseString();
                Dim_OrganizationDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                Dim_OrganizationDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_OrganizationDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_OrganizationDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_OrganizationDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_OrganizationDAOs.Add(Dim_OrganizationDAO);
            }
            await DWContext.Dim_Organization.BulkMergeAsync(Dim_OrganizationDAOs);
        }
        protected async Task Given_DW_Dim_POSMTransactionType(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_POSMTransactionTypeDAOs = new List<Dim_POSMTransactionTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int TransactionTypeIdColumn = StartColumn + columns.IndexOf("TransactionTypeId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_POSMTransactionTypeDAO Dim_POSMTransactionTypeDAO = new Dim_POSMTransactionTypeDAO();
                Dim_POSMTransactionTypeDAO.TransactionTypeId = ExcelWorksheet.Cells[row, TransactionTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_POSMTransactionTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_POSMTransactionTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_POSMTransactionTypeDAOs.Add(Dim_POSMTransactionTypeDAO);
            }
            await DWContext.Dim_POSMTransactionType.BulkMergeAsync(Dim_POSMTransactionTypeDAOs);
        }
        protected async Task Given_DW_Dim_ProblemStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ProblemStatusDAOs = new List<Dim_ProblemStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ProblemStatusIdColumn = StartColumn + columns.IndexOf("ProblemStatusId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_ProblemStatusDAO Dim_ProblemStatusDAO = new Dim_ProblemStatusDAO();
                Dim_ProblemStatusDAO.ProblemStatusId = ExcelWorksheet.Cells[row, ProblemStatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProblemStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_ProblemStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_ProblemStatusDAOs.Add(Dim_ProblemStatusDAO);
            }
            await DWContext.Dim_ProblemStatus.BulkMergeAsync(Dim_ProblemStatusDAOs);
        }
        protected async Task Given_DW_Dim_ProblemType(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ProblemTypeDAOs = new List<Dim_ProblemTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ProblemTypeIdColumn = StartColumn + columns.IndexOf("ProblemTypeId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_ProblemTypeDAO Dim_ProblemTypeDAO = new Dim_ProblemTypeDAO();
                Dim_ProblemTypeDAO.ProblemTypeId = ExcelWorksheet.Cells[row, ProblemTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProblemTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_ProblemTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_ProblemTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProblemTypeDAOs.Add(Dim_ProblemTypeDAO);
            }
            await DWContext.Dim_ProblemType.BulkMergeAsync(Dim_ProblemTypeDAOs);
        }
        protected async Task Given_DW_Dim_Product(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ProductDAOs = new List<Dim_ProductDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ProductIdColumn = StartColumn + columns.IndexOf("ProductId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int SupplierCodeColumn = StartColumn + columns.IndexOf("SupplierCode");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int ScanCodeColumn = StartColumn + columns.IndexOf("ScanCode");
            int ERPCodeColumn = StartColumn + columns.IndexOf("ERPCode");
            int CategoryIdColumn = StartColumn + columns.IndexOf("CategoryId");
            int ProductTypeIdColumn = StartColumn + columns.IndexOf("ProductTypeId");
            int BrandIdColumn = StartColumn + columns.IndexOf("BrandId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int CodeGeneratorRuleIdColumn = StartColumn + columns.IndexOf("CodeGeneratorRuleId");
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
                Dim_ProductDAO Dim_ProductDAO = new Dim_ProductDAO();
                Dim_ProductDAO.ProductId = ExcelWorksheet.Cells[row, ProductIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProductDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_ProductDAO.SupplierCode = ExcelWorksheet.Cells[row, SupplierCodeColumn].Value?.ParseString();
                Dim_ProductDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_ProductDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                Dim_ProductDAO.ScanCode = ExcelWorksheet.Cells[row, ScanCodeColumn].Value?.ParseString();
                Dim_ProductDAO.ERPCode = ExcelWorksheet.Cells[row, ERPCodeColumn].Value?.ParseString();
                Dim_ProductDAO.CategoryId = ExcelWorksheet.Cells[row, CategoryIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProductDAO.ProductTypeId = ExcelWorksheet.Cells[row, ProductTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProductDAO.BrandId = ExcelWorksheet.Cells[row, BrandIdColumn].Value?.ParseNullLong();
                Dim_ProductDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProductDAO.CodeGeneratorRuleId = ExcelWorksheet.Cells[row, CodeGeneratorRuleIdColumn].Value?.ParseNullLong();
                Dim_ProductDAO.UnitOfMeasureGroupingId = ExcelWorksheet.Cells[row, UnitOfMeasureGroupingIdColumn].Value?.ParseNullLong();
                Dim_ProductDAO.SalePrice = ExcelWorksheet.Cells[row, SalePriceColumn].Value?.ParseNullDecimal();
                Dim_ProductDAO.RetailPrice = ExcelWorksheet.Cells[row, RetailPriceColumn].Value?.ParseNullDecimal();
                Dim_ProductDAO.TaxTypeId = ExcelWorksheet.Cells[row, TaxTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProductDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProductDAO.OtherName = ExcelWorksheet.Cells[row, OtherNameColumn].Value?.ParseString();
                Dim_ProductDAO.TechnicalName = ExcelWorksheet.Cells[row, TechnicalNameColumn].Value?.ParseString();
                Dim_ProductDAO.Note = ExcelWorksheet.Cells[row, NoteColumn].Value?.ParseString();
                Dim_ProductDAO.UsedVariationId = ExcelWorksheet.Cells[row, UsedVariationIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProductDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_ProductDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ProductDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ProductDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_ProductDAOs.Add(Dim_ProductDAO);
            }
            await DWContext.Dim_Product.BulkMergeAsync(Dim_ProductDAOs);
        }
        protected async Task Given_DW_Dim_ProductGrouping(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ProductGroupingDAOs = new List<Dim_ProductGroupingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ProductGroupingIdColumn = StartColumn + columns.IndexOf("ProductGroupingId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int ParentIdColumn = StartColumn + columns.IndexOf("ParentId");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int LevelColumn = StartColumn + columns.IndexOf("Level");
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
                Dim_ProductGroupingDAO Dim_ProductGroupingDAO = new Dim_ProductGroupingDAO();
                Dim_ProductGroupingDAO.ProductGroupingId = ExcelWorksheet.Cells[row, ProductGroupingIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProductGroupingDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_ProductGroupingDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_ProductGroupingDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                Dim_ProductGroupingDAO.ParentId = ExcelWorksheet.Cells[row, ParentIdColumn].Value?.ParseNullLong();
                Dim_ProductGroupingDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                Dim_ProductGroupingDAO.Level = ExcelWorksheet.Cells[row, LevelColumn].Value?.ParseLong() ?? 0;
                Dim_ProductGroupingDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_ProductGroupingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ProductGroupingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ProductGroupingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_ProductGroupingDAOs.Add(Dim_ProductGroupingDAO);
            }
            await DWContext.Dim_ProductGrouping.BulkMergeAsync(Dim_ProductGroupingDAOs);
        }
        protected async Task Given_DW_Dim_ProductType(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ProductTypeDAOs = new List<Dim_ProductTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ProductTypeIdColumn = StartColumn + columns.IndexOf("ProductTypeId");
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
                Dim_ProductTypeDAO Dim_ProductTypeDAO = new Dim_ProductTypeDAO();
                Dim_ProductTypeDAO.ProductTypeId = ExcelWorksheet.Cells[row, ProductTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProductTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_ProductTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_ProductTypeDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                Dim_ProductTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProductTypeDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_ProductTypeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ProductTypeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ProductTypeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_ProductTypeDAOs.Add(Dim_ProductTypeDAO);
            }
            await DWContext.Dim_ProductType.BulkMergeAsync(Dim_ProductTypeDAOs);
        }
        protected async Task Given_DW_Dim_Province(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ProvinceDAOs = new List<Dim_ProvinceDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
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
                Dim_ProvinceDAO Dim_ProvinceDAO = new Dim_ProvinceDAO();
                Dim_ProvinceDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProvinceDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_ProvinceDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_ProvinceDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseNullLong();
                Dim_ProvinceDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_ProvinceDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_ProvinceDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ProvinceDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ProvinceDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_ProvinceDAOs.Add(Dim_ProvinceDAO);
            }
            await DWContext.Dim_Province.BulkMergeAsync(Dim_ProvinceDAOs);
        }
        protected async Task Given_DW_Dim_Quarter(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_QuarterDAOs = new List<Dim_QuarterDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int QuarterIdColumn = StartColumn + columns.IndexOf("QuarterId");
            int QuarterColumn = StartColumn + columns.IndexOf("Quarter");
            int YearColumn = StartColumn + columns.IndexOf("Year");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_QuarterDAO Dim_QuarterDAO = new Dim_QuarterDAO();
                Dim_QuarterDAO.QuarterId = ExcelWorksheet.Cells[row, QuarterIdColumn].Value?.ParseLong() ?? 0;
                Dim_QuarterDAO.Quarter = ExcelWorksheet.Cells[row, QuarterColumn].Value?.ParseLong() ?? 0;
                Dim_QuarterDAO.Year = ExcelWorksheet.Cells[row, YearColumn].Value?.ParseLong() ?? 0;
                Dim_QuarterDAOs.Add(Dim_QuarterDAO);
            }
            await DWContext.Dim_Quarter.BulkMergeAsync(Dim_QuarterDAOs);
        }
        protected async Task Given_DW_Dim_RequestState(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_RequestStateDAOs = new List<Dim_RequestStateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int RequestStateIdColumn = StartColumn + columns.IndexOf("RequestStateId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_RequestStateDAO Dim_RequestStateDAO = new Dim_RequestStateDAO();
                Dim_RequestStateDAO.RequestStateId = ExcelWorksheet.Cells[row, RequestStateIdColumn].Value?.ParseLong() ?? 0;
                Dim_RequestStateDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_RequestStateDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_RequestStateDAOs.Add(Dim_RequestStateDAO);
            }
            await DWContext.Dim_RequestState.BulkMergeAsync(Dim_RequestStateDAOs);
        }
        protected async Task Given_DW_Dim_ShowingItem(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ShowingItemDAOs = new List<Dim_ShowingItemDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ShowingItemIdColumn = StartColumn + columns.IndexOf("ShowingItemId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ShowingCategoryIdColumn = StartColumn + columns.IndexOf("ShowingCategoryId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int SalePriceColumn = StartColumn + columns.IndexOf("SalePrice");
            int ERPCodeColumn = StartColumn + columns.IndexOf("ERPCode");
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
                Dim_ShowingItemDAO Dim_ShowingItemDAO = new Dim_ShowingItemDAO();
                Dim_ShowingItemDAO.ShowingItemId = ExcelWorksheet.Cells[row, ShowingItemIdColumn].Value?.ParseLong() ?? 0;
                Dim_ShowingItemDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_ShowingItemDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_ShowingItemDAO.ShowingCategoryId = ExcelWorksheet.Cells[row, ShowingCategoryIdColumn].Value?.ParseLong() ?? 0;
                Dim_ShowingItemDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                Dim_ShowingItemDAO.SalePrice = ExcelWorksheet.Cells[row, SalePriceColumn].Value?.ParseDecimal() ?? 0;
                Dim_ShowingItemDAO.ERPCode = ExcelWorksheet.Cells[row, ERPCodeColumn].Value?.ParseString();
                Dim_ShowingItemDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                Dim_ShowingItemDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_ShowingItemDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_ShowingItemDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ShowingItemDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_ShowingItemDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_ShowingItemDAOs.Add(Dim_ShowingItemDAO);
            }
            await DWContext.Dim_ShowingItem.BulkMergeAsync(Dim_ShowingItemDAOs);
        }
        protected async Task Given_DW_Dim_ShowingItemMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_ShowingItemMappingDAOs = new List<Dim_ShowingItemMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ShowingItemMappingIdColumn = StartColumn + columns.IndexOf("ShowingItemMappingId");
            int ShowingItemIdColumn = StartColumn + columns.IndexOf("ShowingItemId");
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int ProductGroupingIdColumn = StartColumn + columns.IndexOf("ProductGroupingId");
            int ProductTypeIdColumn = StartColumn + columns.IndexOf("ProductTypeId");
            int CategoryIdColumn = StartColumn + columns.IndexOf("CategoryId");
            int BrandIdColumn = StartColumn + columns.IndexOf("BrandId");
            int SupplierIdColumn = StartColumn + columns.IndexOf("SupplierId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_ShowingItemMappingDAO Dim_ShowingItemMappingDAO = new Dim_ShowingItemMappingDAO();
                Dim_ShowingItemMappingDAO.ShowingItemMappingId = ExcelWorksheet.Cells[row, ShowingItemMappingIdColumn].Value?.ParseLong() ?? 0;
                Dim_ShowingItemMappingDAO.ShowingItemId = ExcelWorksheet.Cells[row, ShowingItemIdColumn].Value?.ParseNullLong();
                Dim_ShowingItemMappingDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseNullLong();
                Dim_ShowingItemMappingDAO.ProductGroupingId = ExcelWorksheet.Cells[row, ProductGroupingIdColumn].Value?.ParseNullLong();
                Dim_ShowingItemMappingDAO.ProductTypeId = ExcelWorksheet.Cells[row, ProductTypeIdColumn].Value?.ParseNullLong();
                Dim_ShowingItemMappingDAO.CategoryId = ExcelWorksheet.Cells[row, CategoryIdColumn].Value?.ParseNullLong();
                Dim_ShowingItemMappingDAO.BrandId = ExcelWorksheet.Cells[row, BrandIdColumn].Value?.ParseNullLong();
                Dim_ShowingItemMappingDAO.SupplierId = ExcelWorksheet.Cells[row, SupplierIdColumn].Value?.ParseNullLong();
                Dim_ShowingItemMappingDAOs.Add(Dim_ShowingItemMappingDAO);
            }
            await DWContext.Dim_ShowingItemMapping.BulkMergeAsync(Dim_ShowingItemMappingDAOs);
        }
        protected async Task Given_DW_Dim_Status(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_StatusDAOs = new List<Dim_StatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_StatusDAO Dim_StatusDAO = new Dim_StatusDAO();
                Dim_StatusDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_StatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_StatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_StatusDAOs.Add(Dim_StatusDAO);
            }
            await DWContext.Dim_Status.BulkMergeAsync(Dim_StatusDAOs);
        }
        protected async Task Given_DW_Dim_StoreApprovalState(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_StoreApprovalStateDAOs = new List<Dim_StoreApprovalStateDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreApprovalStateIdColumn = StartColumn + columns.IndexOf("StoreApprovalStateId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_StoreApprovalStateDAO Dim_StoreApprovalStateDAO = new Dim_StoreApprovalStateDAO();
                Dim_StoreApprovalStateDAO.StoreApprovalStateId = ExcelWorksheet.Cells[row, StoreApprovalStateIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreApprovalStateDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_StoreApprovalStateDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_StoreApprovalStateDAOs.Add(Dim_StoreApprovalStateDAO);
            }
            await DWContext.Dim_StoreApprovalState.BulkMergeAsync(Dim_StoreApprovalStateDAOs);
        }
        protected async Task Given_DW_Dim_StoreCheckingStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_StoreCheckingStatusDAOs = new List<Dim_StoreCheckingStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreCheckingStatusIdColumn = StartColumn + columns.IndexOf("StoreCheckingStatusId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_StoreCheckingStatusDAO Dim_StoreCheckingStatusDAO = new Dim_StoreCheckingStatusDAO();
                Dim_StoreCheckingStatusDAO.StoreCheckingStatusId = ExcelWorksheet.Cells[row, StoreCheckingStatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreCheckingStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_StoreCheckingStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_StoreCheckingStatusDAOs.Add(Dim_StoreCheckingStatusDAO);
            }
            await DWContext.Dim_StoreCheckingStatus.BulkMergeAsync(Dim_StoreCheckingStatusDAOs);
        }
        protected async Task Given_DW_Dim_Store(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_StoreDAOs = new List<Dim_StoreDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int CodeDraftColumn = StartColumn + columns.IndexOf("CodeDraft");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int UnsignNameColumn = StartColumn + columns.IndexOf("UnsignName");
            int ParentStoreIdColumn = StartColumn + columns.IndexOf("ParentStoreId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int StoreTypeIdColumn = StartColumn + columns.IndexOf("StoreTypeId");
            int StoreGroupingIdColumn = StartColumn + columns.IndexOf("StoreGroupingId");
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
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int StoreStatusIdColumn = StartColumn + columns.IndexOf("StoreStatusId");
            int StoreScoutingIdColumn = StartColumn + columns.IndexOf("StoreScoutingId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_StoreDAO Dim_StoreDAO = new Dim_StoreDAO();
                Dim_StoreDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_StoreDAO.CodeDraft = ExcelWorksheet.Cells[row, CodeDraftColumn].Value?.ParseString();
                Dim_StoreDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_StoreDAO.UnsignName = ExcelWorksheet.Cells[row, UnsignNameColumn].Value?.ParseString();
                Dim_StoreDAO.ParentStoreId = ExcelWorksheet.Cells[row, ParentStoreIdColumn].Value?.ParseNullLong();
                Dim_StoreDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreDAO.StoreTypeId = ExcelWorksheet.Cells[row, StoreTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreDAO.StoreGroupingId = ExcelWorksheet.Cells[row, StoreGroupingIdColumn].Value?.ParseNullLong();
                Dim_StoreDAO.Telephone = ExcelWorksheet.Cells[row, TelephoneColumn].Value?.ParseString();
                Dim_StoreDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                Dim_StoreDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseNullLong();
                Dim_StoreDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                Dim_StoreDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                Dim_StoreDAO.UnsignAddress = ExcelWorksheet.Cells[row, UnsignAddressColumn].Value?.ParseString();
                Dim_StoreDAO.DeliveryAddress = ExcelWorksheet.Cells[row, DeliveryAddressColumn].Value?.ParseString();
                Dim_StoreDAO.Latitude = ExcelWorksheet.Cells[row, LatitudeColumn].Value?.ParseDecimal() ?? 0;
                Dim_StoreDAO.Longitude = ExcelWorksheet.Cells[row, LongitudeColumn].Value?.ParseDecimal() ?? 0;
                Dim_StoreDAO.DeliveryLatitude = ExcelWorksheet.Cells[row, DeliveryLatitudeColumn].Value?.ParseNullDecimal();
                Dim_StoreDAO.DeliveryLongitude = ExcelWorksheet.Cells[row, DeliveryLongitudeColumn].Value?.ParseNullDecimal();
                Dim_StoreDAO.OwnerName = ExcelWorksheet.Cells[row, OwnerNameColumn].Value?.ParseString();
                Dim_StoreDAO.OwnerPhone = ExcelWorksheet.Cells[row, OwnerPhoneColumn].Value?.ParseString();
                Dim_StoreDAO.OwnerEmail = ExcelWorksheet.Cells[row, OwnerEmailColumn].Value?.ParseString();
                Dim_StoreDAO.TaxCode = ExcelWorksheet.Cells[row, TaxCodeColumn].Value?.ParseString();
                Dim_StoreDAO.LegalEntity = ExcelWorksheet.Cells[row, LegalEntityColumn].Value?.ParseString();
                Dim_StoreDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseNullLong();
                Dim_StoreDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreDAO.StoreStatusId = ExcelWorksheet.Cells[row, StoreStatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreDAO.StoreScoutingId = ExcelWorksheet.Cells[row, StoreScoutingIdColumn].Value?.ParseNullLong();
                Dim_StoreDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_StoreDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_StoreDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_StoreDAOs.Add(Dim_StoreDAO);
            }
            await DWContext.Dim_Store.BulkMergeAsync(Dim_StoreDAOs);
        }
        protected async Task Given_DW_Dim_StoreGrouping(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_StoreGroupingDAOs = new List<Dim_StoreGroupingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreGroupingIdColumn = StartColumn + columns.IndexOf("StoreGroupingId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ParentIdColumn = StartColumn + columns.IndexOf("ParentId");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int LevelColumn = StartColumn + columns.IndexOf("Level");
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
                Dim_StoreGroupingDAO Dim_StoreGroupingDAO = new Dim_StoreGroupingDAO();
                Dim_StoreGroupingDAO.StoreGroupingId = ExcelWorksheet.Cells[row, StoreGroupingIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreGroupingDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_StoreGroupingDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_StoreGroupingDAO.ParentId = ExcelWorksheet.Cells[row, ParentIdColumn].Value?.ParseNullLong();
                Dim_StoreGroupingDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                Dim_StoreGroupingDAO.Level = ExcelWorksheet.Cells[row, LevelColumn].Value?.ParseLong() ?? 0;
                Dim_StoreGroupingDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreGroupingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_StoreGroupingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_StoreGroupingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_StoreGroupingDAOs.Add(Dim_StoreGroupingDAO);
            }
            await DWContext.Dim_StoreGrouping.BulkMergeAsync(Dim_StoreGroupingDAOs);
        }
        protected async Task Given_DW_Dim_StoreMapping(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_StoreMappingDAOs = new List<Dim_StoreMappingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreMappingIdColumn = StartColumn + columns.IndexOf("StoreMappingId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int StoreGroupingIdColumn = StartColumn + columns.IndexOf("StoreGroupingId");
            int StoreStatusIdColumn = StartColumn + columns.IndexOf("StoreStatusId");
            int StoreTypeIdColumn = StartColumn + columns.IndexOf("StoreTypeId");
            int WardIdColumn = StartColumn + columns.IndexOf("WardId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_StoreMappingDAO Dim_StoreMappingDAO = new Dim_StoreMappingDAO();
                Dim_StoreMappingDAO.StoreMappingId = ExcelWorksheet.Cells[row, StoreMappingIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreMappingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreMappingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseNullLong();
                Dim_StoreMappingDAO.StoreGroupingId = ExcelWorksheet.Cells[row, StoreGroupingIdColumn].Value?.ParseNullLong();
                Dim_StoreMappingDAO.StoreStatusId = ExcelWorksheet.Cells[row, StoreStatusIdColumn].Value?.ParseNullLong();
                Dim_StoreMappingDAO.StoreTypeId = ExcelWorksheet.Cells[row, StoreTypeIdColumn].Value?.ParseNullLong();
                Dim_StoreMappingDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                Dim_StoreMappingDAOs.Add(Dim_StoreMappingDAO);
            }
            await DWContext.Dim_StoreMapping.BulkMergeAsync(Dim_StoreMappingDAOs);
        }
        protected async Task Given_DW_Dim_StoreScoutingStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_StoreScoutingStatusDAOs = new List<Dim_StoreScoutingStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreScoutingStatusIdColumn = StartColumn + columns.IndexOf("StoreScoutingStatusId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_StoreScoutingStatusDAO Dim_StoreScoutingStatusDAO = new Dim_StoreScoutingStatusDAO();
                Dim_StoreScoutingStatusDAO.StoreScoutingStatusId = ExcelWorksheet.Cells[row, StoreScoutingStatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreScoutingStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_StoreScoutingStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_StoreScoutingStatusDAOs.Add(Dim_StoreScoutingStatusDAO);
            }
            await DWContext.Dim_StoreScoutingStatus.BulkMergeAsync(Dim_StoreScoutingStatusDAOs);
        }
        protected async Task Given_DW_Dim_StoreScoutingType(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_StoreScoutingTypeDAOs = new List<Dim_StoreScoutingTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreScoutingTypeIdColumn = StartColumn + columns.IndexOf("StoreScoutingTypeId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
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
                Dim_StoreScoutingTypeDAO Dim_StoreScoutingTypeDAO = new Dim_StoreScoutingTypeDAO();
                Dim_StoreScoutingTypeDAO.StoreScoutingTypeId = ExcelWorksheet.Cells[row, StoreScoutingTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreScoutingTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_StoreScoutingTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_StoreScoutingTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreScoutingTypeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_StoreScoutingTypeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_StoreScoutingTypeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_StoreScoutingTypeDAOs.Add(Dim_StoreScoutingTypeDAO);
            }
            await DWContext.Dim_StoreScoutingType.BulkMergeAsync(Dim_StoreScoutingTypeDAOs);
        }
        protected async Task Given_DW_Dim_StoreStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_StoreStatusDAOs = new List<Dim_StoreStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreStatusIdColumn = StartColumn + columns.IndexOf("StoreStatusId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_StoreStatusDAO Dim_StoreStatusDAO = new Dim_StoreStatusDAO();
                Dim_StoreStatusDAO.StoreStatusId = ExcelWorksheet.Cells[row, StoreStatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_StoreStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_StoreStatusDAOs.Add(Dim_StoreStatusDAO);
            }
            await DWContext.Dim_StoreStatus.BulkMergeAsync(Dim_StoreStatusDAOs);
        }
        protected async Task Given_DW_Dim_StoreStatusHistoryType(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_StoreStatusHistoryTypeDAOs = new List<Dim_StoreStatusHistoryTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreStatusHistoryTypeIdColumn = StartColumn + columns.IndexOf("StoreStatusHistoryTypeId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_StoreStatusHistoryTypeDAO Dim_StoreStatusHistoryTypeDAO = new Dim_StoreStatusHistoryTypeDAO();
                Dim_StoreStatusHistoryTypeDAO.StoreStatusHistoryTypeId = ExcelWorksheet.Cells[row, StoreStatusHistoryTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreStatusHistoryTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_StoreStatusHistoryTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_StoreStatusHistoryTypeDAOs.Add(Dim_StoreStatusHistoryTypeDAO);
            }
            await DWContext.Dim_StoreStatusHistoryType.BulkMergeAsync(Dim_StoreStatusHistoryTypeDAOs);
        }
        protected async Task Given_DW_Dim_StoreType(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_StoreTypeDAOs = new List<Dim_StoreTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreTypeIdColumn = StartColumn + columns.IndexOf("StoreTypeId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ColorIdColumn = StartColumn + columns.IndexOf("ColorId");
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
                Dim_StoreTypeDAO Dim_StoreTypeDAO = new Dim_StoreTypeDAO();
                Dim_StoreTypeDAO.StoreTypeId = ExcelWorksheet.Cells[row, StoreTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_StoreTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_StoreTypeDAO.ColorId = ExcelWorksheet.Cells[row, ColorIdColumn].Value?.ParseNullLong();
                Dim_StoreTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_StoreTypeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_StoreTypeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_StoreTypeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_StoreTypeDAOs.Add(Dim_StoreTypeDAO);
            }
            await DWContext.Dim_StoreType.BulkMergeAsync(Dim_StoreTypeDAOs);
        }
        protected async Task Given_DW_Dim_Supplier(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_SupplierDAOs = new List<Dim_SupplierDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int SupplierIdColumn = StartColumn + columns.IndexOf("SupplierId");
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
                Dim_SupplierDAO Dim_SupplierDAO = new Dim_SupplierDAO();
                Dim_SupplierDAO.SupplierId = ExcelWorksheet.Cells[row, SupplierIdColumn].Value?.ParseLong() ?? 0;
                Dim_SupplierDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_SupplierDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_SupplierDAO.TaxCode = ExcelWorksheet.Cells[row, TaxCodeColumn].Value?.ParseString();
                Dim_SupplierDAO.Phone = ExcelWorksheet.Cells[row, PhoneColumn].Value?.ParseString();
                Dim_SupplierDAO.Email = ExcelWorksheet.Cells[row, EmailColumn].Value?.ParseString();
                Dim_SupplierDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                Dim_SupplierDAO.NationId = ExcelWorksheet.Cells[row, NationIdColumn].Value?.ParseNullLong();
                Dim_SupplierDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                Dim_SupplierDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseNullLong();
                Dim_SupplierDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                Dim_SupplierDAO.OwnerName = ExcelWorksheet.Cells[row, OwnerNameColumn].Value?.ParseString();
                Dim_SupplierDAO.PersonInChargeId = ExcelWorksheet.Cells[row, PersonInChargeIdColumn].Value?.ParseNullLong();
                Dim_SupplierDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_SupplierDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                Dim_SupplierDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_SupplierDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_SupplierDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_SupplierDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_SupplierDAOs.Add(Dim_SupplierDAO);
            }
            await DWContext.Dim_Supplier.BulkMergeAsync(Dim_SupplierDAOs);
        }
        protected async Task Given_DW_Dim_TaxType(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_TaxTypeDAOs = new List<Dim_TaxTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int TaxTypeIdColumn = StartColumn + columns.IndexOf("TaxTypeId");
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
                Dim_TaxTypeDAO Dim_TaxTypeDAO = new Dim_TaxTypeDAO();
                Dim_TaxTypeDAO.TaxTypeId = ExcelWorksheet.Cells[row, TaxTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_TaxTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_TaxTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_TaxTypeDAO.Percentage = ExcelWorksheet.Cells[row, PercentageColumn].Value?.ParseDecimal() ?? 0;
                Dim_TaxTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_TaxTypeDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_TaxTypeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_TaxTypeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_TaxTypeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_TaxTypeDAOs.Add(Dim_TaxTypeDAO);
            }
            await DWContext.Dim_TaxType.BulkMergeAsync(Dim_TaxTypeDAOs);
        }
        protected async Task Given_DW_Dim_TransactionType(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_TransactionTypeDAOs = new List<Dim_TransactionTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int TransactionTypeIdColumn = StartColumn + columns.IndexOf("TransactionTypeId");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_TransactionTypeDAO Dim_TransactionTypeDAO = new Dim_TransactionTypeDAO();
                Dim_TransactionTypeDAO.TransactionTypeId = ExcelWorksheet.Cells[row, TransactionTypeIdColumn].Value?.ParseLong() ?? 0;
                Dim_TransactionTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_TransactionTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_TransactionTypeDAOs.Add(Dim_TransactionTypeDAO);
            }
            await DWContext.Dim_TransactionType.BulkMergeAsync(Dim_TransactionTypeDAOs);
        }
        protected async Task Given_DW_Dim_UnitOfMeasure(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_UnitOfMeasureDAOs = new List<Dim_UnitOfMeasureDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int UnitOfMeasureIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
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
                Dim_UnitOfMeasureDAO Dim_UnitOfMeasureDAO = new Dim_UnitOfMeasureDAO();
                Dim_UnitOfMeasureDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdColumn].Value?.ParseLong() ?? 0;
                Dim_UnitOfMeasureDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_UnitOfMeasureDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_UnitOfMeasureDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                Dim_UnitOfMeasureDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_UnitOfMeasureDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_UnitOfMeasureDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_UnitOfMeasureDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_UnitOfMeasureDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_UnitOfMeasureDAOs.Add(Dim_UnitOfMeasureDAO);
            }
            await DWContext.Dim_UnitOfMeasure.BulkMergeAsync(Dim_UnitOfMeasureDAOs);
        }
        protected async Task Given_DW_Dim_UsedVariation(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_UsedVariationDAOs = new List<Dim_UsedVariationDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int UsedVariationIdColumn = StartColumn + columns.IndexOf("UsedVariationId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_UsedVariationDAO Dim_UsedVariationDAO = new Dim_UsedVariationDAO();
                Dim_UsedVariationDAO.UsedVariationId = ExcelWorksheet.Cells[row, UsedVariationIdColumn].Value?.ParseLong() ?? 0;
                Dim_UsedVariationDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_UsedVariationDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_UsedVariationDAOs.Add(Dim_UsedVariationDAO);
            }
            await DWContext.Dim_UsedVariation.BulkMergeAsync(Dim_UsedVariationDAOs);
        }
        protected async Task Given_DW_Dim_Ward(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_WardDAOs = new List<Dim_WardDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int WardIdColumn = StartColumn + columns.IndexOf("WardId");
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
                Dim_WardDAO Dim_WardDAO = new Dim_WardDAO();
                Dim_WardDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseLong() ?? 0;
                Dim_WardDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_WardDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_WardDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseNullLong();
                Dim_WardDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseLong() ?? 0;
                Dim_WardDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_WardDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Dim_WardDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_WardDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Dim_WardDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Dim_WardDAOs.Add(Dim_WardDAO);
            }
            await DWContext.Dim_Ward.BulkMergeAsync(Dim_WardDAOs);
        }
        protected async Task Given_DW_Dim_Warehouse(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_WarehouseDAOs = new List<Dim_WarehouseDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int WarehouseIdColumn = StartColumn + columns.IndexOf("WarehouseId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
            int WardIdColumn = StartColumn + columns.IndexOf("WardId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_WarehouseDAO Dim_WarehouseDAO = new Dim_WarehouseDAO();
                Dim_WarehouseDAO.WarehouseId = ExcelWorksheet.Cells[row, WarehouseIdColumn].Value?.ParseLong() ?? 0;
                Dim_WarehouseDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Dim_WarehouseDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Dim_WarehouseDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                Dim_WarehouseDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Dim_WarehouseDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                Dim_WarehouseDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseNullLong();
                Dim_WarehouseDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                Dim_WarehouseDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Dim_WarehouseDAOs.Add(Dim_WarehouseDAO);
            }
            await DWContext.Dim_Warehouse.BulkMergeAsync(Dim_WarehouseDAOs);
        }
        protected async Task Given_DW_Dim_Year(ExcelWorksheet ExcelWorksheet)
        {
            this.Dim_YearDAOs = new List<Dim_YearDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int YearIdColumn = StartColumn + columns.IndexOf("YearId");
            int YearColumn = StartColumn + columns.IndexOf("Year");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Dim_YearDAO Dim_YearDAO = new Dim_YearDAO();
                Dim_YearDAO.YearId = ExcelWorksheet.Cells[row, YearIdColumn].Value?.ParseLong() ?? 0;
                Dim_YearDAO.Year = ExcelWorksheet.Cells[row, YearColumn].Value?.ParseLong() ?? 0;
                Dim_YearDAOs.Add(Dim_YearDAO);
            }
            await DWContext.Dim_Year.BulkMergeAsync(Dim_YearDAOs);
        }
        protected async Task Given_DW_Fact_BrandHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_BrandHistoryDAOs = new List<Fact_BrandHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int BrandHistoryIdColumn = StartColumn + columns.IndexOf("BrandHistoryId");
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
                Fact_BrandHistoryDAO Fact_BrandHistoryDAO = new Fact_BrandHistoryDAO();
                Fact_BrandHistoryDAO.BrandHistoryId = ExcelWorksheet.Cells[row, BrandHistoryIdColumn].Value?.ParseLong() ?? 0;
                Fact_BrandHistoryDAO.BrandId = ExcelWorksheet.Cells[row, BrandIdColumn].Value?.ParseLong() ?? 0;
                Fact_BrandHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseNullLong();
                Fact_BrandHistoryDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Fact_BrandHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_BrandHistoryDAOs.Add(Fact_BrandHistoryDAO);
            }
            await DWContext.Fact_BrandHistory.BulkMergeAsync(Fact_BrandHistoryDAOs);
        }
        protected async Task Given_DW_Fact_BrandInStore(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_BrandInStoreDAOs = new List<Fact_BrandInStoreDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int BrandInStoreIdColumn = StartColumn + columns.IndexOf("BrandInStoreId");
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
                Fact_BrandInStoreDAO Fact_BrandInStoreDAO = new Fact_BrandInStoreDAO();
                Fact_BrandInStoreDAO.BrandInStoreId = ExcelWorksheet.Cells[row, BrandInStoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_BrandInStoreDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_BrandInStoreDAO.BrandId = ExcelWorksheet.Cells[row, BrandIdColumn].Value?.ParseLong() ?? 0;
                Fact_BrandInStoreDAO.Top = ExcelWorksheet.Cells[row, TopColumn].Value?.ParseLong() ?? 0;
                Fact_BrandInStoreDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                Fact_BrandInStoreDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                Fact_BrandInStoreDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_BrandInStoreDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_BrandInStoreDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Fact_BrandInStoreDAOs.Add(Fact_BrandInStoreDAO);
            }
            await DWContext.Fact_BrandInStore.BulkMergeAsync(Fact_BrandInStoreDAOs);
        }
        protected async Task Given_DW_Fact_DirectSalesOrder(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_DirectSalesOrderDAOs = new List<Fact_DirectSalesOrderDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int DirectSalesOrderIdColumn = StartColumn + columns.IndexOf("DirectSalesOrderId");
            int BuyerStoreIdColumn = StartColumn + columns.IndexOf("BuyerStoreId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int SaleEmployeeIdColumn = StartColumn + columns.IndexOf("SaleEmployeeId");
            int OrderDateColumn = StartColumn + columns.IndexOf("OrderDate");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int DeliveryDateColumn = StartColumn + columns.IndexOf("DeliveryDate");
            int RequestStateIdColumn = StartColumn + columns.IndexOf("RequestStateId");
            int DirectSalesOrderSourceTypeIdColumn = StartColumn + columns.IndexOf("DirectSalesOrderSourceTypeId");
            int EditedPriceStatusIdColumn = StartColumn + columns.IndexOf("EditedPriceStatusId");
            int SubTotalColumn = StartColumn + columns.IndexOf("SubTotal");
            int GeneralDiscountPercentageColumn = StartColumn + columns.IndexOf("GeneralDiscountPercentage");
            int GeneralDiscountAmountColumn = StartColumn + columns.IndexOf("GeneralDiscountAmount");
            int TotalDiscountAmountColumn = StartColumn + columns.IndexOf("TotalDiscountAmount");
            int TotalTaxAmountColumn = StartColumn + columns.IndexOf("TotalTaxAmount");
            int TotalAfterTaxColumn = StartColumn + columns.IndexOf("TotalAfterTax");
            int PromotionCodeColumn = StartColumn + columns.IndexOf("PromotionCode");
            int PromotionValueColumn = StartColumn + columns.IndexOf("PromotionValue");
            int TotalColumn = StartColumn + columns.IndexOf("Total");
            int StoreCheckingIdColumn = StartColumn + columns.IndexOf("StoreCheckingId");
            int StoreUserCreatorIdColumn = StartColumn + columns.IndexOf("StoreUserCreatorId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int ErpApprovalStateIdColumn = StartColumn + columns.IndexOf("ErpApprovalStateId");
            int StoreApprovalStateIdColumn = StartColumn + columns.IndexOf("StoreApprovalStateId");
            int GeneralApprovalStateIdColumn = StartColumn + columns.IndexOf("GeneralApprovalStateId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Fact_DirectSalesOrderDAO Fact_DirectSalesOrderDAO = new Fact_DirectSalesOrderDAO();
                Fact_DirectSalesOrderDAO.DirectSalesOrderId = ExcelWorksheet.Cells[row, DirectSalesOrderIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderDAO.BuyerStoreId = ExcelWorksheet.Cells[row, BuyerStoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderDAO.OrderDate = ExcelWorksheet.Cells[row, OrderDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_DirectSalesOrderDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Fact_DirectSalesOrderDAO.DeliveryDate = ExcelWorksheet.Cells[row, DeliveryDateColumn].Value?.ParseNullDateTime();
                Fact_DirectSalesOrderDAO.RequestStateId = ExcelWorksheet.Cells[row, RequestStateIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderDAO.DirectSalesOrderSourceTypeId = ExcelWorksheet.Cells[row, DirectSalesOrderSourceTypeIdColumn].Value?.ParseNullLong();
                Fact_DirectSalesOrderDAO.EditedPriceStatusId = ExcelWorksheet.Cells[row, EditedPriceStatusIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderDAO.SubTotal = ExcelWorksheet.Cells[row, SubTotalColumn].Value?.ParseDecimal() ?? 0;
                Fact_DirectSalesOrderDAO.GeneralDiscountPercentage = ExcelWorksheet.Cells[row, GeneralDiscountPercentageColumn].Value?.ParseNullDecimal();
                Fact_DirectSalesOrderDAO.GeneralDiscountAmount = ExcelWorksheet.Cells[row, GeneralDiscountAmountColumn].Value?.ParseNullDecimal();
                Fact_DirectSalesOrderDAO.TotalDiscountAmount = ExcelWorksheet.Cells[row, TotalDiscountAmountColumn].Value?.ParseNullDecimal();
                Fact_DirectSalesOrderDAO.TotalTaxAmount = ExcelWorksheet.Cells[row, TotalTaxAmountColumn].Value?.ParseDecimal() ?? 0;
                Fact_DirectSalesOrderDAO.TotalAfterTax = ExcelWorksheet.Cells[row, TotalAfterTaxColumn].Value?.ParseDecimal() ?? 0;
                Fact_DirectSalesOrderDAO.PromotionCode = ExcelWorksheet.Cells[row, PromotionCodeColumn].Value?.ParseString();
                Fact_DirectSalesOrderDAO.PromotionValue = ExcelWorksheet.Cells[row, PromotionValueColumn].Value?.ParseNullDecimal();
                Fact_DirectSalesOrderDAO.Total = ExcelWorksheet.Cells[row, TotalColumn].Value?.ParseDecimal() ?? 0;
                Fact_DirectSalesOrderDAO.StoreCheckingId = ExcelWorksheet.Cells[row, StoreCheckingIdColumn].Value?.ParseNullLong();
                Fact_DirectSalesOrderDAO.StoreUserCreatorId = ExcelWorksheet.Cells[row, StoreUserCreatorIdColumn].Value?.ParseNullLong();
                Fact_DirectSalesOrderDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseNullLong();
                Fact_DirectSalesOrderDAO.ErpApprovalStateId = ExcelWorksheet.Cells[row, ErpApprovalStateIdColumn].Value?.ParseNullLong();
                Fact_DirectSalesOrderDAO.StoreApprovalStateId = ExcelWorksheet.Cells[row, StoreApprovalStateIdColumn].Value?.ParseNullLong();
                Fact_DirectSalesOrderDAO.GeneralApprovalStateId = ExcelWorksheet.Cells[row, GeneralApprovalStateIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_DirectSalesOrderDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_DirectSalesOrderDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Fact_DirectSalesOrderDAOs.Add(Fact_DirectSalesOrderDAO);
            }
            await DWContext.Fact_DirectSalesOrder.BulkMergeAsync(Fact_DirectSalesOrderDAOs);
        }
        protected async Task Given_DW_Fact_DirectSalesOrderTransaction(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_DirectSalesOrderTransactionDAOs = new List<Fact_DirectSalesOrderTransactionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int DirectSalesOrderTransactionIdColumn = StartColumn + columns.IndexOf("DirectSalesOrderTransactionId");
            int DirectSalesOrderIdColumn = StartColumn + columns.IndexOf("DirectSalesOrderId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int BuyerStoreIdColumn = StartColumn + columns.IndexOf("BuyerStoreId");
            int SalesEmployeeIdColumn = StartColumn + columns.IndexOf("SalesEmployeeId");
            int OrderDateColumn = StartColumn + columns.IndexOf("OrderDate");
            int RequestStateIdColumn = StartColumn + columns.IndexOf("RequestStateId");
            int DeliveryDateColumn = StartColumn + columns.IndexOf("DeliveryDate");
            int TransactionTypeIdColumn = StartColumn + columns.IndexOf("TransactionTypeId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int SalePriceColumn = StartColumn + columns.IndexOf("SalePrice");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            int RequestedQuantityColumn = StartColumn + columns.IndexOf("RequestedQuantity");
            int DiscountAmountColumn = StartColumn + columns.IndexOf("DiscountAmount");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int GeneralDiscountPercentageColumn = StartColumn + columns.IndexOf("GeneralDiscountPercentage");
            int GeneralDiscountAmountColumn = StartColumn + columns.IndexOf("GeneralDiscountAmount");
            int TaxPercentageColumn = StartColumn + columns.IndexOf("TaxPercentage");
            int TaxAmountColumn = StartColumn + columns.IndexOf("TaxAmount");
            int AmountColumn = StartColumn + columns.IndexOf("Amount");
            int FactorColumn = StartColumn + columns.IndexOf("Factor");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Fact_DirectSalesOrderTransactionDAO Fact_DirectSalesOrderTransactionDAO = new Fact_DirectSalesOrderTransactionDAO();
                Fact_DirectSalesOrderTransactionDAO.DirectSalesOrderTransactionId = ExcelWorksheet.Cells[row, DirectSalesOrderTransactionIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderTransactionDAO.DirectSalesOrderId = ExcelWorksheet.Cells[row, DirectSalesOrderIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderTransactionDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderTransactionDAO.BuyerStoreId = ExcelWorksheet.Cells[row, BuyerStoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderTransactionDAO.SalesEmployeeId = ExcelWorksheet.Cells[row, SalesEmployeeIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderTransactionDAO.OrderDate = ExcelWorksheet.Cells[row, OrderDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_DirectSalesOrderTransactionDAO.RequestStateId = ExcelWorksheet.Cells[row, RequestStateIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderTransactionDAO.DeliveryDate = ExcelWorksheet.Cells[row, DeliveryDateColumn].Value?.ParseNullDateTime();
                Fact_DirectSalesOrderTransactionDAO.TransactionTypeId = ExcelWorksheet.Cells[row, TransactionTypeIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderTransactionDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderTransactionDAO.SalePrice = ExcelWorksheet.Cells[row, SalePriceColumn].Value?.ParseNullDecimal();
                Fact_DirectSalesOrderTransactionDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                Fact_DirectSalesOrderTransactionDAO.RequestedQuantity = ExcelWorksheet.Cells[row, RequestedQuantityColumn].Value?.ParseNullLong();
                Fact_DirectSalesOrderTransactionDAO.DiscountAmount = ExcelWorksheet.Cells[row, DiscountAmountColumn].Value?.ParseNullDecimal();
                Fact_DirectSalesOrderTransactionDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                Fact_DirectSalesOrderTransactionDAO.GeneralDiscountPercentage = ExcelWorksheet.Cells[row, GeneralDiscountPercentageColumn].Value?.ParseNullDecimal();
                Fact_DirectSalesOrderTransactionDAO.GeneralDiscountAmount = ExcelWorksheet.Cells[row, GeneralDiscountAmountColumn].Value?.ParseNullDecimal();
                Fact_DirectSalesOrderTransactionDAO.TaxPercentage = ExcelWorksheet.Cells[row, TaxPercentageColumn].Value?.ParseNullDecimal();
                Fact_DirectSalesOrderTransactionDAO.TaxAmount = ExcelWorksheet.Cells[row, TaxAmountColumn].Value?.ParseNullDecimal();
                Fact_DirectSalesOrderTransactionDAO.Amount = ExcelWorksheet.Cells[row, AmountColumn].Value?.ParseDecimal() ?? 0;
                Fact_DirectSalesOrderTransactionDAO.Factor = ExcelWorksheet.Cells[row, FactorColumn].Value?.ParseNullLong();
                Fact_DirectSalesOrderTransactionDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Fact_DirectSalesOrderTransactionDAOs.Add(Fact_DirectSalesOrderTransactionDAO);
            }
            await DWContext.Fact_DirectSalesOrderTransaction.BulkMergeAsync(Fact_DirectSalesOrderTransactionDAOs);
        }
        protected async Task Given_DW_Fact_Image(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_ImageDAOs = new List<Fact_ImageDAO>();
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
            int ShootingAtColumn = StartColumn + columns.IndexOf("ShootingAt");
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
                Fact_ImageDAO Fact_ImageDAO = new Fact_ImageDAO();
                Fact_ImageDAO.ImageId = ExcelWorksheet.Cells[row, ImageIdColumn].Value?.ParseLong() ?? 0;
                Fact_ImageDAO.StoreCheckingId = ExcelWorksheet.Cells[row, StoreCheckingIdColumn].Value?.ParseNullLong();
                Fact_ImageDAO.AlbumId = ExcelWorksheet.Cells[row, AlbumIdColumn].Value?.ParseLong() ?? 0;
                Fact_ImageDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_ImageDAO.ShootingAt = ExcelWorksheet.Cells[row, ShootingAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_ImageDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseNullLong();
                Fact_ImageDAO.Distance = ExcelWorksheet.Cells[row, DistanceColumn].Value?.ParseNullLong();
                Fact_ImageDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Fact_ImageDAOs.Add(Fact_ImageDAO);
            }
            await DWContext.Fact_Image.BulkMergeAsync(Fact_ImageDAOs);
        }
        protected async Task Given_DW_Fact_IndirectSalesOrder(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_IndirectSalesOrderDAOs = new List<Fact_IndirectSalesOrderDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IndirectSalesOrderIdColumn = StartColumn + columns.IndexOf("IndirectSalesOrderId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int BuyerStoreIdColumn = StartColumn + columns.IndexOf("BuyerStoreId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int SellerStoreIdColumn = StartColumn + columns.IndexOf("SellerStoreId");
            int SaleEmployeeIdColumn = StartColumn + columns.IndexOf("SaleEmployeeId");
            int OrderDateColumn = StartColumn + columns.IndexOf("OrderDate");
            int DeliveryDateColumn = StartColumn + columns.IndexOf("DeliveryDate");
            int RequestStateIdColumn = StartColumn + columns.IndexOf("RequestStateId");
            int EditedPriceStatusIdColumn = StartColumn + columns.IndexOf("EditedPriceStatusId");
            int SubTotalColumn = StartColumn + columns.IndexOf("SubTotal");
            int GeneralDiscountPercentageColumn = StartColumn + columns.IndexOf("GeneralDiscountPercentage");
            int GeneralDiscountAmountColumn = StartColumn + columns.IndexOf("GeneralDiscountAmount");
            int TotalDiscountAmountColumn = StartColumn + columns.IndexOf("TotalDiscountAmount");
            int TotalColumn = StartColumn + columns.IndexOf("Total");
            int StoreCheckingIdColumn = StartColumn + columns.IndexOf("StoreCheckingId");
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
                Fact_IndirectSalesOrderDAO Fact_IndirectSalesOrderDAO = new Fact_IndirectSalesOrderDAO();
                Fact_IndirectSalesOrderDAO.IndirectSalesOrderId = ExcelWorksheet.Cells[row, IndirectSalesOrderIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Fact_IndirectSalesOrderDAO.BuyerStoreId = ExcelWorksheet.Cells[row, BuyerStoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderDAO.SellerStoreId = ExcelWorksheet.Cells[row, SellerStoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderDAO.OrderDate = ExcelWorksheet.Cells[row, OrderDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_IndirectSalesOrderDAO.DeliveryDate = ExcelWorksheet.Cells[row, DeliveryDateColumn].Value?.ParseNullDateTime();
                Fact_IndirectSalesOrderDAO.RequestStateId = ExcelWorksheet.Cells[row, RequestStateIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderDAO.EditedPriceStatusId = ExcelWorksheet.Cells[row, EditedPriceStatusIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderDAO.SubTotal = ExcelWorksheet.Cells[row, SubTotalColumn].Value?.ParseDecimal() ?? 0;
                Fact_IndirectSalesOrderDAO.GeneralDiscountPercentage = ExcelWorksheet.Cells[row, GeneralDiscountPercentageColumn].Value?.ParseNullDecimal();
                Fact_IndirectSalesOrderDAO.GeneralDiscountAmount = ExcelWorksheet.Cells[row, GeneralDiscountAmountColumn].Value?.ParseNullDecimal();
                Fact_IndirectSalesOrderDAO.TotalDiscountAmount = ExcelWorksheet.Cells[row, TotalDiscountAmountColumn].Value?.ParseNullDecimal();
                Fact_IndirectSalesOrderDAO.Total = ExcelWorksheet.Cells[row, TotalColumn].Value?.ParseDecimal() ?? 0;
                Fact_IndirectSalesOrderDAO.StoreCheckingId = ExcelWorksheet.Cells[row, StoreCheckingIdColumn].Value?.ParseNullLong();
                Fact_IndirectSalesOrderDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseNullLong();
                Fact_IndirectSalesOrderDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_IndirectSalesOrderDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_IndirectSalesOrderDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Fact_IndirectSalesOrderDAOs.Add(Fact_IndirectSalesOrderDAO);
            }
            await DWContext.Fact_IndirectSalesOrder.BulkMergeAsync(Fact_IndirectSalesOrderDAOs);
        }
        protected async Task Given_DW_Fact_IndirectSalesOrderTransaction(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_IndirectSalesOrderTransactionDAOs = new List<Fact_IndirectSalesOrderTransactionDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IndirectSalesOrderTransactionIdColumn = StartColumn + columns.IndexOf("IndirectSalesOrderTransactionId");
            int IndirectSalesOrderIdColumn = StartColumn + columns.IndexOf("IndirectSalesOrderId");
            int SellerStoreIdColumn = StartColumn + columns.IndexOf("SellerStoreId");
            int BuyerStoreIdColumn = StartColumn + columns.IndexOf("BuyerStoreId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int SalesEmployeeIdColumn = StartColumn + columns.IndexOf("SalesEmployeeId");
            int OrderDateColumn = StartColumn + columns.IndexOf("OrderDate");
            int RequestStateIdColumn = StartColumn + columns.IndexOf("RequestStateId");
            int DeliveryDateColumn = StartColumn + columns.IndexOf("DeliveryDate");
            int TransactionTypeIdColumn = StartColumn + columns.IndexOf("TransactionTypeId");
            int ItemIdColumn = StartColumn + columns.IndexOf("ItemId");
            int SalePriceColumn = StartColumn + columns.IndexOf("SalePrice");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            int RequestedQuantityColumn = StartColumn + columns.IndexOf("RequestedQuantity");
            int DiscountAmountColumn = StartColumn + columns.IndexOf("DiscountAmount");
            int DiscountPercentageColumn = StartColumn + columns.IndexOf("DiscountPercentage");
            int GeneralDiscountPercentageColumn = StartColumn + columns.IndexOf("GeneralDiscountPercentage");
            int GeneralDiscountAmountColumn = StartColumn + columns.IndexOf("GeneralDiscountAmount");
            int TaxPercentageColumn = StartColumn + columns.IndexOf("TaxPercentage");
            int TaxAmountColumn = StartColumn + columns.IndexOf("TaxAmount");
            int AmountColumn = StartColumn + columns.IndexOf("Amount");
            int FactorColumn = StartColumn + columns.IndexOf("Factor");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Fact_IndirectSalesOrderTransactionDAO Fact_IndirectSalesOrderTransactionDAO = new Fact_IndirectSalesOrderTransactionDAO();
                Fact_IndirectSalesOrderTransactionDAO.IndirectSalesOrderTransactionId = ExcelWorksheet.Cells[row, IndirectSalesOrderTransactionIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderTransactionDAO.IndirectSalesOrderId = ExcelWorksheet.Cells[row, IndirectSalesOrderIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderTransactionDAO.SellerStoreId = ExcelWorksheet.Cells[row, SellerStoreIdColumn].Value?.ParseNullLong();
                Fact_IndirectSalesOrderTransactionDAO.BuyerStoreId = ExcelWorksheet.Cells[row, BuyerStoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderTransactionDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderTransactionDAO.SalesEmployeeId = ExcelWorksheet.Cells[row, SalesEmployeeIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderTransactionDAO.OrderDate = ExcelWorksheet.Cells[row, OrderDateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_IndirectSalesOrderTransactionDAO.RequestStateId = ExcelWorksheet.Cells[row, RequestStateIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderTransactionDAO.DeliveryDate = ExcelWorksheet.Cells[row, DeliveryDateColumn].Value?.ParseNullDateTime();
                Fact_IndirectSalesOrderTransactionDAO.TransactionTypeId = ExcelWorksheet.Cells[row, TransactionTypeIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderTransactionDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderTransactionDAO.SalePrice = ExcelWorksheet.Cells[row, SalePriceColumn].Value?.ParseNullDecimal();
                Fact_IndirectSalesOrderTransactionDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                Fact_IndirectSalesOrderTransactionDAO.RequestedQuantity = ExcelWorksheet.Cells[row, RequestedQuantityColumn].Value?.ParseNullLong();
                Fact_IndirectSalesOrderTransactionDAO.DiscountAmount = ExcelWorksheet.Cells[row, DiscountAmountColumn].Value?.ParseNullDecimal();
                Fact_IndirectSalesOrderTransactionDAO.DiscountPercentage = ExcelWorksheet.Cells[row, DiscountPercentageColumn].Value?.ParseNullDecimal();
                Fact_IndirectSalesOrderTransactionDAO.GeneralDiscountPercentage = ExcelWorksheet.Cells[row, GeneralDiscountPercentageColumn].Value?.ParseNullDecimal();
                Fact_IndirectSalesOrderTransactionDAO.GeneralDiscountAmount = ExcelWorksheet.Cells[row, GeneralDiscountAmountColumn].Value?.ParseNullDecimal();
                Fact_IndirectSalesOrderTransactionDAO.TaxPercentage = ExcelWorksheet.Cells[row, TaxPercentageColumn].Value?.ParseNullDecimal();
                Fact_IndirectSalesOrderTransactionDAO.TaxAmount = ExcelWorksheet.Cells[row, TaxAmountColumn].Value?.ParseNullDecimal();
                Fact_IndirectSalesOrderTransactionDAO.Amount = ExcelWorksheet.Cells[row, AmountColumn].Value?.ParseDecimal() ?? 0;
                Fact_IndirectSalesOrderTransactionDAO.Factor = ExcelWorksheet.Cells[row, FactorColumn].Value?.ParseNullLong();
                Fact_IndirectSalesOrderTransactionDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Fact_IndirectSalesOrderTransactionDAOs.Add(Fact_IndirectSalesOrderTransactionDAO);
            }
            await DWContext.Fact_IndirectSalesOrderTransaction.BulkMergeAsync(Fact_IndirectSalesOrderTransactionDAOs);
        }
        protected async Task Given_DW_Fact_KpiGeneralContent(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_KpiGeneralContentDAOs = new List<Fact_KpiGeneralContentDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int KpiGeneralContentIdColumn = StartColumn + columns.IndexOf("KpiGeneralContentId");
            int KpiGeneralIdColumn = StartColumn + columns.IndexOf("KpiGeneralId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int EmployeeIdColumn = StartColumn + columns.IndexOf("EmployeeId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int KpiCriteriaGeneralIdColumn = StartColumn + columns.IndexOf("KpiCriteriaGeneralId");
            int YearIdColumn = StartColumn + columns.IndexOf("YearId");
            int QuarterIdColumn = StartColumn + columns.IndexOf("QuarterId");
            int MonthIdColumn = StartColumn + columns.IndexOf("MonthId");
            int ValueColumn = StartColumn + columns.IndexOf("Value");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Fact_KpiGeneralContentDAO Fact_KpiGeneralContentDAO = new Fact_KpiGeneralContentDAO();
                Fact_KpiGeneralContentDAO.KpiGeneralContentId = ExcelWorksheet.Cells[row, KpiGeneralContentIdColumn].Value?.ParseLong() ?? 0;
                Fact_KpiGeneralContentDAO.KpiGeneralId = ExcelWorksheet.Cells[row, KpiGeneralIdColumn].Value?.ParseLong() ?? 0;
                Fact_KpiGeneralContentDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Fact_KpiGeneralContentDAO.EmployeeId = ExcelWorksheet.Cells[row, EmployeeIdColumn].Value?.ParseLong() ?? 0;
                Fact_KpiGeneralContentDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Fact_KpiGeneralContentDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                Fact_KpiGeneralContentDAO.KpiCriteriaGeneralId = ExcelWorksheet.Cells[row, KpiCriteriaGeneralIdColumn].Value?.ParseLong() ?? 0;
                Fact_KpiGeneralContentDAO.YearId = ExcelWorksheet.Cells[row, YearIdColumn].Value?.ParseNullLong();
                Fact_KpiGeneralContentDAO.QuarterId = ExcelWorksheet.Cells[row, QuarterIdColumn].Value?.ParseNullLong();
                Fact_KpiGeneralContentDAO.MonthId = ExcelWorksheet.Cells[row, MonthIdColumn].Value?.ParseNullLong();
                Fact_KpiGeneralContentDAO.Value = ExcelWorksheet.Cells[row, ValueColumn].Value?.ParseNullDecimal();
                Fact_KpiGeneralContentDAOs.Add(Fact_KpiGeneralContentDAO);
            }
            await DWContext.Fact_KpiGeneralContent.BulkMergeAsync(Fact_KpiGeneralContentDAOs);
        }
        protected async Task Given_DW_Fact_POSMTransaction(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_POSMTransactionDAOs = new List<Fact_POSMTransactionDAO>();
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
            int ShowingOrderIdColumn = StartColumn + columns.IndexOf("ShowingOrderId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int ShowingItemIdColumn = StartColumn + columns.IndexOf("ShowingItemId");
            int QuantityColumn = StartColumn + columns.IndexOf("Quantity");
            int DateColumn = StartColumn + columns.IndexOf("Date");
            int AmountColumn = StartColumn + columns.IndexOf("Amount");
            int TransactionTypeIdColumn = StartColumn + columns.IndexOf("TransactionTypeId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Fact_POSMTransactionDAO Fact_POSMTransactionDAO = new Fact_POSMTransactionDAO();
                Fact_POSMTransactionDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                Fact_POSMTransactionDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Fact_POSMTransactionDAO.ShowingOrderId = ExcelWorksheet.Cells[row, ShowingOrderIdColumn].Value?.ParseLong() ?? 0;
                Fact_POSMTransactionDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                Fact_POSMTransactionDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_POSMTransactionDAO.ShowingItemId = ExcelWorksheet.Cells[row, ShowingItemIdColumn].Value?.ParseLong() ?? 0;
                Fact_POSMTransactionDAO.Quantity = ExcelWorksheet.Cells[row, QuantityColumn].Value?.ParseLong() ?? 0;
                Fact_POSMTransactionDAO.Date = ExcelWorksheet.Cells[row, DateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_POSMTransactionDAO.Amount = ExcelWorksheet.Cells[row, AmountColumn].Value?.ParseDecimal() ?? 0;
                Fact_POSMTransactionDAO.TransactionTypeId = ExcelWorksheet.Cells[row, TransactionTypeIdColumn].Value?.ParseLong() ?? 0;
                Fact_POSMTransactionDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_POSMTransactionDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_POSMTransactionDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Fact_POSMTransactionDAOs.Add(Fact_POSMTransactionDAO);
            }
            await DWContext.Fact_POSMTransaction.BulkMergeAsync(Fact_POSMTransactionDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        protected async Task Given_DW_Fact_Problem(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_ProblemDAOs = new List<Fact_ProblemDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ProblemIdColumn = StartColumn + columns.IndexOf("ProblemId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int StoreCheckingIdColumn = StartColumn + columns.IndexOf("StoreCheckingId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int ProblemTypeIdColumn = StartColumn + columns.IndexOf("ProblemTypeId");
            int NoteAtColumn = StartColumn + columns.IndexOf("NoteAt");
            int CompletedAtColumn = StartColumn + columns.IndexOf("CompletedAt");
            int ContentColumn = StartColumn + columns.IndexOf("Content");
            int ProblemStatusIdColumn = StartColumn + columns.IndexOf("ProblemStatusId");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Fact_ProblemDAO Fact_ProblemDAO = new Fact_ProblemDAO();
                Fact_ProblemDAO.ProblemId = ExcelWorksheet.Cells[row, ProblemIdColumn].Value?.ParseLong() ?? 0;
                Fact_ProblemDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Fact_ProblemDAO.StoreCheckingId = ExcelWorksheet.Cells[row, StoreCheckingIdColumn].Value?.ParseNullLong();
                Fact_ProblemDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_ProblemDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                Fact_ProblemDAO.ProblemTypeId = ExcelWorksheet.Cells[row, ProblemTypeIdColumn].Value?.ParseLong() ?? 0;
                Fact_ProblemDAO.NoteAt = ExcelWorksheet.Cells[row, NoteAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_ProblemDAO.CompletedAt = ExcelWorksheet.Cells[row, CompletedAtColumn].Value?.ParseNullDateTime();
                Fact_ProblemDAO.Content = ExcelWorksheet.Cells[row, ContentColumn].Value?.ParseString();
                Fact_ProblemDAO.ProblemStatusId = ExcelWorksheet.Cells[row, ProblemStatusIdColumn].Value?.ParseLong() ?? 0;
                Fact_ProblemDAOs.Add(Fact_ProblemDAO);
            }
            await DWContext.Fact_Problem.BulkMergeAsync(Fact_ProblemDAOs);
        }
        protected async Task Given_DW_Fact_ProductGroupingHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_ProductGroupingHistoryDAOs = new List<Fact_ProductGroupingHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int ProductGroupingHistoryIdColumn = StartColumn + columns.IndexOf("ProductGroupingHistoryId");
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
                Fact_ProductGroupingHistoryDAO Fact_ProductGroupingHistoryDAO = new Fact_ProductGroupingHistoryDAO();
                Fact_ProductGroupingHistoryDAO.ProductGroupingHistoryId = ExcelWorksheet.Cells[row, ProductGroupingHistoryIdColumn].Value?.ParseLong() ?? 0;
                Fact_ProductGroupingHistoryDAO.ProductGroupingId = ExcelWorksheet.Cells[row, ProductGroupingIdColumn].Value?.ParseLong() ?? 0;
                Fact_ProductGroupingHistoryDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Fact_ProductGroupingHistoryDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Fact_ProductGroupingHistoryDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Fact_ProductGroupingHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseNullLong();
                Fact_ProductGroupingHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_ProductGroupingHistoryDAOs.Add(Fact_ProductGroupingHistoryDAO);
            }
            await DWContext.Fact_ProductGroupingHistory.BulkMergeAsync(Fact_ProductGroupingHistoryDAOs);
        }
        protected async Task Given_DW_Fact_StoreChecking(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_StoreCheckingDAOs = new List<Fact_StoreCheckingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreCheckingIdColumn = StartColumn + columns.IndexOf("StoreCheckingId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int SaleEmployeeIdColumn = StartColumn + columns.IndexOf("SaleEmployeeId");
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
            int CheckedColumn = StartColumn + columns.IndexOf("Checked");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Fact_StoreCheckingDAO Fact_StoreCheckingDAO = new Fact_StoreCheckingDAO();
                Fact_StoreCheckingDAO.StoreCheckingId = ExcelWorksheet.Cells[row, StoreCheckingIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreCheckingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreCheckingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreCheckingDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreCheckingDAO.Longitude = ExcelWorksheet.Cells[row, LongitudeColumn].Value?.ParseNullDecimal();
                Fact_StoreCheckingDAO.Latitude = ExcelWorksheet.Cells[row, LatitudeColumn].Value?.ParseNullDecimal();
                Fact_StoreCheckingDAO.CheckOutLongitude = ExcelWorksheet.Cells[row, CheckOutLongitudeColumn].Value?.ParseNullDecimal();
                Fact_StoreCheckingDAO.CheckOutLatitude = ExcelWorksheet.Cells[row, CheckOutLatitudeColumn].Value?.ParseNullDecimal();
                Fact_StoreCheckingDAO.CheckInAt = ExcelWorksheet.Cells[row, CheckInAtColumn].Value?.ParseNullDateTime();
                Fact_StoreCheckingDAO.CheckOutAt = ExcelWorksheet.Cells[row, CheckOutAtColumn].Value?.ParseNullDateTime();
                Fact_StoreCheckingDAO.CheckInDistance = ExcelWorksheet.Cells[row, CheckInDistanceColumn].Value?.ParseNullLong();
                Fact_StoreCheckingDAO.CheckOutDistance = ExcelWorksheet.Cells[row, CheckOutDistanceColumn].Value?.ParseNullLong();
                Fact_StoreCheckingDAO.IndirectSalesOrderCounter = ExcelWorksheet.Cells[row, IndirectSalesOrderCounterColumn].Value?.ParseNullLong();
                Fact_StoreCheckingDAO.ImageCounter = ExcelWorksheet.Cells[row, ImageCounterColumn].Value?.ParseNullLong();
                Fact_StoreCheckingDAO.DeviceName = ExcelWorksheet.Cells[row, DeviceNameColumn].Value?.ParseString();
                Fact_StoreCheckingDAOs.Add(Fact_StoreCheckingDAO);
            }
            await DWContext.Fact_StoreChecking.BulkMergeAsync(Fact_StoreCheckingDAOs);
        }
        protected async Task Given_DW_Fact_StoreHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_StoreHistoryDAOs = new List<Fact_StoreHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreHistoryIdColumn = StartColumn + columns.IndexOf("StoreHistoryId");
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
                Fact_StoreHistoryDAO Fact_StoreHistoryDAO = new Fact_StoreHistoryDAO();
                Fact_StoreHistoryDAO.StoreHistoryId = ExcelWorksheet.Cells[row, StoreHistoryIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreHistoryDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreHistoryDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Fact_StoreHistoryDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Fact_StoreHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseNullLong();
                Fact_StoreHistoryDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreHistoryDAO.StoreStatusId = ExcelWorksheet.Cells[row, StoreStatusIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreHistoryDAO.EstimatedRevenueId = ExcelWorksheet.Cells[row, EstimatedRevenueIdColumn].Value?.ParseNullLong();
                Fact_StoreHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_StoreHistoryDAOs.Add(Fact_StoreHistoryDAO);
            }
            await DWContext.Fact_StoreHistory.BulkMergeAsync(Fact_StoreHistoryDAOs);
        }
        protected async Task Given_DW_Fact_StoreScouting(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_StoreScoutingDAOs = new List<Fact_StoreScoutingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreScoutingIdColumn = StartColumn + columns.IndexOf("StoreScoutingId");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int OwnerPhoneColumn = StartColumn + columns.IndexOf("OwnerPhone");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
            int WardIdColumn = StartColumn + columns.IndexOf("WardId");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int LatitudeColumn = StartColumn + columns.IndexOf("Latitude");
            int LongitudeColumn = StartColumn + columns.IndexOf("Longitude");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int StoreScoutingStatusIdColumn = StartColumn + columns.IndexOf("StoreScoutingStatusId");
            int StoreScoutingTypeIdColumn = StartColumn + columns.IndexOf("StoreScoutingTypeId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Fact_StoreScoutingDAO Fact_StoreScoutingDAO = new Fact_StoreScoutingDAO();
                Fact_StoreScoutingDAO.StoreScoutingId = ExcelWorksheet.Cells[row, StoreScoutingIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreScoutingDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                Fact_StoreScoutingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseNullLong();
                Fact_StoreScoutingDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                Fact_StoreScoutingDAO.OwnerPhone = ExcelWorksheet.Cells[row, OwnerPhoneColumn].Value?.ParseString();
                Fact_StoreScoutingDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                Fact_StoreScoutingDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseNullLong();
                Fact_StoreScoutingDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                Fact_StoreScoutingDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                Fact_StoreScoutingDAO.Latitude = ExcelWorksheet.Cells[row, LatitudeColumn].Value?.ParseDecimal() ?? 0;
                Fact_StoreScoutingDAO.Longitude = ExcelWorksheet.Cells[row, LongitudeColumn].Value?.ParseDecimal() ?? 0;
                Fact_StoreScoutingDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreScoutingDAO.StoreScoutingStatusId = ExcelWorksheet.Cells[row, StoreScoutingStatusIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreScoutingDAO.StoreScoutingTypeId = ExcelWorksheet.Cells[row, StoreScoutingTypeIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreScoutingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_StoreScoutingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_StoreScoutingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                Fact_StoreScoutingDAOs.Add(Fact_StoreScoutingDAO);
            }
            await DWContext.Fact_StoreScouting.BulkMergeAsync(Fact_StoreScoutingDAOs);
        }
        protected async Task Given_DW_Fact_StoreStatusHistory(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_StoreStatusHistoryDAOs = new List<Fact_StoreStatusHistoryDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreStatusHistoryIdColumn = StartColumn + columns.IndexOf("StoreStatusHistoryId");
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
                Fact_StoreStatusHistoryDAO Fact_StoreStatusHistoryDAO = new Fact_StoreStatusHistoryDAO();
                Fact_StoreStatusHistoryDAO.StoreStatusHistoryId = ExcelWorksheet.Cells[row, StoreStatusHistoryIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreStatusHistoryDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreStatusHistoryDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreStatusHistoryDAO.PreviousCreatedAt = ExcelWorksheet.Cells[row, PreviousCreatedAtColumn].Value?.ParseNullDateTime();
                Fact_StoreStatusHistoryDAO.PreviousStoreStatusId = ExcelWorksheet.Cells[row, PreviousStoreStatusIdColumn].Value?.ParseNullLong();
                Fact_StoreStatusHistoryDAO.StoreStatusId = ExcelWorksheet.Cells[row, StoreStatusIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreStatusHistoryDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_StoreStatusHistoryDAOs.Add(Fact_StoreStatusHistoryDAO);
            }
            await DWContext.Fact_StoreStatusHistory.BulkMergeAsync(Fact_StoreStatusHistoryDAOs);
        }
        protected async Task Given_DW_Fact_StoreUnchecking(ExcelWorksheet ExcelWorksheet)
        {
            this.Fact_StoreUncheckingDAOs = new List<Fact_StoreUncheckingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int StoreUncheckingIdColumn = StartColumn + columns.IndexOf("StoreUncheckingId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int DateColumn = StartColumn + columns.IndexOf("Date");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                Fact_StoreUncheckingDAO Fact_StoreUncheckingDAO = new Fact_StoreUncheckingDAO();
                Fact_StoreUncheckingDAO.StoreUncheckingId = ExcelWorksheet.Cells[row, StoreUncheckingIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreUncheckingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreUncheckingDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreUncheckingDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                Fact_StoreUncheckingDAO.Date = ExcelWorksheet.Cells[row, DateColumn].Value?.ParseDateTime() ?? DateTime.Now;
                Fact_StoreUncheckingDAOs.Add(Fact_StoreUncheckingDAO);
            }
            await DWContext.Fact_StoreUnchecking.BulkMergeAsync(Fact_StoreUncheckingDAOs);
        }
        protected async Task Given_DW_View_KPIDoanhThu(ExcelWorksheet ExcelWorksheet)
        {
            this.View_KPIDoanhThuDAOs = new List<View_KPIDoanhThuDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int Month1Column = StartColumn + columns.IndexOf("Month1");
            int Year1Column = StartColumn + columns.IndexOf("Year1");
            int ProvinceColumn = StartColumn + columns.IndexOf("Province");
            int KPIColumn = StartColumn + columns.IndexOf("KPI");
            int RevenueColumn = StartColumn + columns.IndexOf("Revenue");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                View_KPIDoanhThuDAO View_KPIDoanhThuDAO = new View_KPIDoanhThuDAO();
                View_KPIDoanhThuDAO.Month1 = ExcelWorksheet.Cells[row, Month1Column].Value?.ParseNullInt();
                View_KPIDoanhThuDAO.Year1 = ExcelWorksheet.Cells[row, Year1Column].Value?.ParseNullInt();
                View_KPIDoanhThuDAO.Province = ExcelWorksheet.Cells[row, ProvinceColumn].Value?.ParseString();
                View_KPIDoanhThuDAO.KPI = ExcelWorksheet.Cells[row, KPIColumn].Value?.ParseString();
                View_KPIDoanhThuDAO.Revenue = ExcelWorksheet.Cells[row, RevenueColumn].Value?.ParseNullDecimal();
                View_KPIDoanhThuDAOs.Add(View_KPIDoanhThuDAO);
            }
            await DWContext.View_KPIDoanhThu.BulkMergeAsync(View_KPIDoanhThuDAOs);
        }
        #endregion
    }

}
