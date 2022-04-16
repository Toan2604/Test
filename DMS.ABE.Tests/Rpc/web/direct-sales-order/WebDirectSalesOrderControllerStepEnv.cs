using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using OfficeOpenXml;
using TrueSight.Common;
using DMS.ABE.Models;
using DMS.ABE.Enums;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
namespace DMS.ABE.Tests.Rpc.web.direct_sales_order
{
    public partial class WebDirectSalesOrderControllerFeature
    {
        private List<BrandDAO> BrandDAOs { get; set; }
        private List<CategoryDAO> CategoryDAOs { get; set; }
        private List<ProductTypeDAO> ProductTypeDAOs { get; set; }
        private List<TaxTypeDAO> TaxTypeDAOs { get; set; }
        private List<UnitOfMeasureGroupingDAO> UnitOfMeasureGroupingDAOs { get; set; }
        private List<ProductImageMappingDAO> ProductImageMappingDAOs { get; set; }
        private List<ProductProductGroupingMappingDAO> ProductProductGroupingMappingDAOs { get; set; }
        private List<ProductGroupingDAO> ProductGroupingDAOs { get; set; }
        private List<VariationGroupingDAO> VariationGroupingDAOs { get; set; }
        private List<VariationDAO> VariationDAOs { get; set; }
        private List<UsedVariationDAO> UsedVariationDAOs { get; set; }
        private List<DirectSalesOrderSourceTypeDAO> DirectSalesOrderSourceTypeDAOs { get; set; }
        private List<EditedPriceStatusDAO> EditedPriceStatusDAOs { get; set; }
        private List<ErpApprovalStateDAO> ErpApprovalStateDAOs { get; set; }
        private List<GeneralApprovalStateDAO> GeneralApprovalStateDAOs { get; set; }
        private List<OrganizationDAO> OrganizationDAOs { get; set; }
        private List<RequestStateDAO> RequestStateDAOs { get; set; }
        private List<StoreCheckingImageMappingDAO> StoreCheckingImageMappingDAOs { get; set; }
        private List<StoreCheckingDAO> StoreCheckingDAOs { get; set; }
        private List<AlbumImageMappingDAO> AlbumImageMappingDAOs { get; set; }
        private List<AlbumDAO> AlbumDAOs { get; set; }
        private List<StoreApprovalStateDAO> StoreApprovalStateDAOs { get; set; }
        private List<TransactionTypeDAO> TransactionTypeDAOs { get; set; }
        private List<UnitOfMeasureDAO> UnitOfMeasureDAOs { get; set; }
        private List<DirectSalesOrderDAO> DirectSalesOrderDAOs { get; set; }
        private List<DirectSalesOrderContentDAO> DirectSalesOrderContentDAOs { get; set; }
        private List<DirectSalesOrderPromotionDAO> DirectSalesOrderPromotionDAOs { get; set; }
        private List<DirectSalesOrderTransactionDAO> DirectSalesOrderTransactionDAOs { get; set; }
        private List<ItemDAO> ItemDAOs { get; set; }
        private List<ImageDAO> ImageDAOs { get; set; }
        private List<ProductDAO> ProductDAOs { get; set; }
        private async Task LoadExcel(string path)
        {
            MemoryStream MemoryStream = ReadFile(path);
            using (ExcelPackage excelPackage = new ExcelPackage(MemoryStream))
            {
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
                ExcelWorksheet wsOrganization = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Organization)).FirstOrDefault();
                if (wsOrganization != null)
                    await Given_Organization(wsOrganization);
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
            }
        }
        private async Task Given_Product(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_DirectSalesOrder(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_Item(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_TransactionType(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_UnitOfMeasure(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_DirectSalesOrderSourceType(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_EditedPriceStatus(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_ErpApprovalState(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_GeneralApprovalState(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_Organization(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_RequestState(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_StoreApprovalState(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_DirectSalesOrderContent(ExcelWorksheet ExcelWorksheet)
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
            int UnitOfMeasureIdIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
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
                DirectSalesOrderContentDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdIdColumn].Value?.ParseLong() ?? 0;
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
        private async Task Given_DirectSalesOrderPromotion(ExcelWorksheet ExcelWorksheet)
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
            int UnitOfMeasureIdIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
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
                DirectSalesOrderPromotionDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdIdColumn].Value?.ParseLong() ?? 0;
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
        private async Task Given_DirectSalesOrderTransaction(ExcelWorksheet ExcelWorksheet)
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
            int UnitOfMeasureIdIdColumn = StartColumn + columns.IndexOf("UnitOfMeasureId");
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
                DirectSalesOrderTransactionDAO.ItemId = ExcelWorksheet.Cells[row, ItemIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.UnitOfMeasureId = ExcelWorksheet.Cells[row, UnitOfMeasureIdIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.BuyerStoreId = ExcelWorksheet.Cells[row, BuyerStoreIdColumn].Value?.ParseLong() ?? 0;
                DirectSalesOrderTransactionDAO.SalesEmployeeId = ExcelWorksheet.Cells[row, SalesEmployeeIdColumn].Value?.ParseLong() ?? 0;
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
        private async Task Given_Brand(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_Category(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_ProductType(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_TaxType(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_UnitOfMeasureGrouping(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_UsedVariation(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_ProductImageMapping(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_Image(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_ProductProductGroupingMapping(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_ProductGrouping(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_VariationGrouping(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_Variation(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_StoreChecking(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_Album(ExcelWorksheet ExcelWorksheet)
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
        private async Task Given_AlbumImageMapping(ExcelWorksheet ExcelWorksheet)
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
            int DeleteAtColumn = StartColumn + columns.IndexOf("DeletedAt");
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
                AlbumImageMappingDAO.SaleEmployeeId = ExcelWorksheet.Cells[row, SaleEmployeeIdColumn].Value?.ParseLong() ?? 0;
                AlbumImageMappingDAO.Distance = ExcelWorksheet.Cells[row, DistanceColumn].Value?.ParseNullLong();
                AlbumImageMappingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeleteAtColumn].Value?.ParseNullDateTime();
                AlbumImageMappingDAOs.Add(AlbumImageMappingDAO);
            }
            await DataContext.AlbumImageMapping.BulkMergeAsync(AlbumImageMappingDAOs);
        }
        private async Task Given_StoreCheckingImageMapping(ExcelWorksheet ExcelWorksheet)
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
    }
}
