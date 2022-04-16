using DMS.Common;
using DMS.Enums;
using DMS.Rpc.direct_sales_order;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace DMS.Tests.Rpc.direct_sales_order
{
    public partial class DirectSalesOrderControllerFeature
    {
        private async Task Then_Equals(DirectSalesOrder_DirectSalesOrderDTO Expected, DirectSalesOrder_DirectSalesOrderDTO Result)
        {
            Assert.AreEqual(Expected.Id, Result.Id);
            Assert.AreEqual(Expected.Code, Result.Code);
            Assert.AreEqual(Expected.OrganizationId, Result.OrganizationId);
            Assert.AreEqual(Expected.BuyerStoreId, Result.BuyerStoreId);
            Assert.AreEqual(Expected.PhoneNumber, Result.PhoneNumber);
            Assert.AreEqual(Expected.StoreAddress, Result.StoreAddress);
            Assert.AreEqual(Expected.DeliveryAddress, Result.DeliveryAddress);
            Assert.AreEqual(Expected.SaleEmployeeId, Result.SaleEmployeeId);
            Assert.AreEqual(Expected.OrderDate.Date, Result.OrderDate.Date);
            Assert.AreEqual(Expected.DeliveryDate?.Date, Result.DeliveryDate?.Date);
            Assert.AreEqual(Expected.ErpApprovalStateId, Result.ErpApprovalStateId);
            Assert.AreEqual(Expected.StoreApprovalStateId, Result.StoreApprovalStateId);
            Assert.AreEqual(Expected.RequestStateId, Result.RequestStateId);
            Assert.AreEqual(Expected.EditedPriceStatusId, Result.EditedPriceStatusId);
            Assert.AreEqual(Expected.Note, Result.Note);
            Assert.AreEqual(Expected.SubTotal, Result.SubTotal);
            Assert.AreEqual(Expected.GeneralDiscountPercentage, Result.GeneralDiscountPercentage);
            Assert.AreEqual(Expected.GeneralDiscountAmount, Result.GeneralDiscountAmount);
            Assert.AreEqual(Expected.TotalTaxAmount, Result.TotalTaxAmount);
            Assert.AreEqual(Expected.TotalAfterTax, Result.TotalAfterTax);
            Assert.AreEqual(Expected.PromotionCode, Result.PromotionCode);
            Assert.AreEqual(Expected.PromotionValue, Result.PromotionValue);
            Assert.AreEqual(Expected.Total, Result.Total);
            //Assert.AreEqual(Expected.StoreCheckingId, Result.StoreCheckingId);
            //Assert.AreEqual(Expected.StoreUserCreatorId, Result.StoreUserCreatorId);
            //Assert.AreEqual(Expected.CreatorId, Result.CreatorId);
            Assert.AreEqual(Expected.GeneralApprovalStateId, Result.GeneralApprovalStateId);
            if (Expected.DirectSalesOrderContents != null && Expected.DirectSalesOrderContents.Count > 0
                && Result.DirectSalesOrderContents != null & Result.DirectSalesOrderContents.Count > 0)
            {
                Assert.AreEqual(Expected.DirectSalesOrderContents.Count, Result.DirectSalesOrderContents.Count);
                if (Expected.DirectSalesOrderContents.Count == Result.DirectSalesOrderContents.Count)
                {
                    for (int i = 0; i < Expected.DirectSalesOrderContents.Count; i++)
                    {
                        await Then_ContentEquals(Expected.DirectSalesOrderContents[i], Result.DirectSalesOrderContents[i]);
                    }
                }
            }
        }
        private async Task Then_ContentEquals(DirectSalesOrder_DirectSalesOrderContentDTO Expected, DirectSalesOrder_DirectSalesOrderContentDTO Result)
        {
            Assert.AreEqual(Expected.DirectSalesOrderId, Result.DirectSalesOrderId);
            Assert.AreEqual(Expected.ItemId, Result.ItemId);
            Assert.AreEqual(Expected.UnitOfMeasureId, Result.UnitOfMeasureId);
            Assert.AreEqual(Expected.Quantity, Result.Quantity);
            Assert.AreEqual(Expected.PrimaryUnitOfMeasureId, Result.PrimaryUnitOfMeasureId);
            Assert.AreEqual(Expected.RequestedQuantity, Result.RequestedQuantity);
            Assert.AreEqual(Expected.PrimaryPrice, Result.PrimaryPrice);
            Assert.AreEqual(Expected.SalePrice, Result.SalePrice);
            Assert.AreEqual(Expected.EditedPriceStatusId, Result.EditedPriceStatusId);
            Assert.AreEqual(Expected.DiscountPercentage, Result.DiscountPercentage);
            Assert.AreEqual(Expected.DiscountAmount, Result.DiscountAmount);
            Assert.AreEqual(Expected.GeneralDiscountPercentage, Result.GeneralDiscountPercentage);
            Assert.AreEqual(Expected.GeneralDiscountAmount, Result.GeneralDiscountAmount);
            Assert.AreEqual(Expected.TaxPercentage, Result.TaxPercentage);
            Assert.AreEqual(Expected.TaxAmount, Result.TaxAmount);
            Assert.AreEqual(Expected.Amount, Result.Amount);
            Assert.AreEqual(Expected.Factor, Result.Factor);
        }
        private async Task Then_ErrorEquals(DirectSalesOrder_DirectSalesOrderDTO Expected, DirectSalesOrder_DirectSalesOrderDTO Result)
        {
            Assert.AreEqual(Expected.Errors.Count, Result.Errors.Count);
            Assert.AreEqual(Expected.GeneralErrors.Count, Result.GeneralErrors.Count);

        }
        private async Task Then_Create_Success()
        {
            Assert.AreEqual(641, DirectSalesOrder_DirectSalesOrderDTO.Id);
            Assert.AreEqual("641", DirectSalesOrder_DirectSalesOrderDTO.Code);
            Assert.AreEqual(new DateTime(2021, 09, 10), DirectSalesOrder_DirectSalesOrderDTO.OrderDate.Date);
            Assert.AreEqual("Hà Nội", DirectSalesOrder_DirectSalesOrderDTO.DeliveryAddress);
            Assert.AreEqual(4428600.0000M, DirectSalesOrder_DirectSalesOrderDTO.TotalAfterTax);
            Assert.AreEqual(402600.0000M, DirectSalesOrder_DirectSalesOrderDTO.TotalTaxAmount);
            Assert.AreEqual(4026000.0000M, DirectSalesOrder_DirectSalesOrderDTO.SubTotal);
            Assert.AreEqual(GeneralApprovalStateEnum.STORE_DRAFT.Id, DirectSalesOrder_DirectSalesOrderDTO.GeneralApprovalStateId);
            Assert.AreEqual(StoreApprovalStateEnum.DRAFT.Id, DirectSalesOrder_DirectSalesOrderDTO.StoreApprovalStateId);
            Assert.AreEqual(RequestStateEnum.APPROVED.Id, DirectSalesOrder_DirectSalesOrderDTO.RequestStateId);
        }
        private async Task Then_Classify_Success(string path)
        {
            DirectSalesOrder_DirectSalesOrderDTO Expected = ReadFileFromJson<DirectSalesOrder_DirectSalesOrderDTO>(path);
            Assert.AreEqual(Expected.InventoryCheckStateId, DirectSalesOrder_DirectSalesOrderDTO.InventoryCheckStateId);
            Assert.AreEqual(Expected.StoreBalanceCheckStateId, DirectSalesOrder_DirectSalesOrderDTO.StoreBalanceCheckStateId);
        }
        private async Task Then_CheckResult(string path)
        {
            string Payload = System.IO.File.ReadAllText(path);
            DirectSalesOrder_DirectSalesOrderDTO Expected = JsonConvert.DeserializeObject<DirectSalesOrder_DirectSalesOrderDTO>(Payload);
            Assert.AreEqual(true, DirectSalesOrder_DirectSalesOrderDTO.Errors == null);
            await Then_Equals(Expected, DirectSalesOrder_DirectSalesOrderDTO);
        }
        private async Task Then_CheckResultFailed(string path)
        {
            string Payload = System.IO.File.ReadAllText(path);
            DirectSalesOrder_DirectSalesOrderDTO Expected = JsonConvert.DeserializeObject<DirectSalesOrder_DirectSalesOrderDTO>(Payload);
            await Then_ErrorEquals(Expected, DirectSalesOrder_DirectSalesOrderDTO);
        }
        private async Task Then_Search_Success(string path)
        {
            List<DirectSalesOrder_DirectSalesOrderDTO> Expected = ReadFileFromJson<List<DirectSalesOrder_DirectSalesOrderDTO>>(path);
            Assert.AreEqual(Expected.Count, DirectSalesOrder_DirectSalesOrderDTOs.Count);
            if (Expected.Count == DirectSalesOrder_DirectSalesOrderDTOs.Count)
            {
                for (int i = 0; i < DirectSalesOrder_DirectSalesOrderDTOs.Count; i++)
                {
                    await Then_Equals(Expected[i], DirectSalesOrder_DirectSalesOrderDTOs[i]);
                }
            }
            Assert.AreEqual(Expected[0].InventoryCheckStateId, DirectSalesOrder_DirectSalesOrderDTOs[0].InventoryCheckStateId);
            Assert.AreEqual(Expected[0].StoreBalanceCheckStateId, DirectSalesOrder_DirectSalesOrderDTOs[0].StoreBalanceCheckStateId);
        }
        private async Task Then_ListOrder_WithCheckState_Success(string path)
        {
            List<DirectSalesOrder_DirectSalesOrderDTO> Expected = ReadFileFromJson<List<DirectSalesOrder_DirectSalesOrderDTO>>(path);
            Assert.AreEqual(Expected.Count, DirectSalesOrder_DirectSalesOrderDTOs.Count);
            Assert.AreEqual(Expected[0].InventoryCheckStateId, DirectSalesOrder_DirectSalesOrderDTOs[0].InventoryCheckStateId);
            Assert.AreEqual(Expected[0].StoreBalanceCheckStateId, DirectSalesOrder_DirectSalesOrderDTOs[0].StoreBalanceCheckStateId);
        }
        private async Task Then_GetState_Success(long InventoryCheckStateId, long StoreBalanceCheckStateId)
        {
            Assert.AreEqual(InventoryCheckStateId, DirectSalesOrder_DirectSalesOrderDTOs[0].InventoryCheckStateId);
            Assert.AreEqual(StoreBalanceCheckStateId, DirectSalesOrder_DirectSalesOrderDTOs[0].StoreBalanceCheckStateId);
        }
    }
}
