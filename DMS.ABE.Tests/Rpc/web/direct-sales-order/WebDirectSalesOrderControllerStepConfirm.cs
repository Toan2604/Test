using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using DMS.ABE.Entities;
using DMS.ABE.Helpers;
using DMS.ABE.Models;
using DMS.ABE.Enums;
using DMS.ABE.Common;
using DMS.ABE.Rpc.web.direct_sales_order;
using Newtonsoft.Json;

namespace DMS.ABE.Tests.Rpc.web.direct_sales_order
{
    public partial class WebDirectSalesOrderControllerFeature
    {
        private async Task Then_Equals(WebDirectSalesOrder_DirectSalesOrderDTO Expected, WebDirectSalesOrder_DirectSalesOrderDTO Result)
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
            Assert.AreEqual(Expected.DirectSalesOrderSourceTypeId, Result.DirectSalesOrderSourceTypeId);
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
            Assert.AreEqual(Expected.StoreCheckingId, Result.StoreCheckingId);
            Assert.AreEqual(Expected.StoreUserCreatorId, Result.StoreUserCreatorId);
            Assert.AreEqual(Expected.CreatorId, Result.CreatorId);
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
        private async Task Then_ContentEquals(WebDirectSalesOrder_DirectSalesOrderContentDTO Expected, WebDirectSalesOrder_DirectSalesOrderContentDTO Result)
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
        private async Task Then_ErrorEquals(WebDirectSalesOrder_DirectSalesOrderDTO Expected, WebDirectSalesOrder_DirectSalesOrderDTO Result)
        {
            Assert.AreEqual(Expected.Errors.Count, Result.Errors.Count);
            Assert.AreEqual(Expected.GeneralErrors.Count, Result.GeneralErrors.Count);

        }
        private async Task Then_Create_Success()
        {
            Assert.AreEqual(641, WebDirectSalesOrder_DirectSalesOrderDTO.Id);
            Assert.AreEqual("641", WebDirectSalesOrder_DirectSalesOrderDTO.Code);
            Assert.AreEqual(new DateTime(2021, 09, 10), WebDirectSalesOrder_DirectSalesOrderDTO.OrderDate.Date);
            Assert.AreEqual("Hà Nội", WebDirectSalesOrder_DirectSalesOrderDTO.DeliveryAddress);
            Assert.AreEqual(4428600.0000M, WebDirectSalesOrder_DirectSalesOrderDTO.TotalAfterTax);
            Assert.AreEqual(402600.0000M, WebDirectSalesOrder_DirectSalesOrderDTO.TotalTaxAmount);
            Assert.AreEqual(4026000.0000M, WebDirectSalesOrder_DirectSalesOrderDTO.SubTotal);
            Assert.AreEqual(GeneralApprovalStateEnum.STORE_DRAFT.Id, WebDirectSalesOrder_DirectSalesOrderDTO.GeneralApprovalStateId);
            Assert.AreEqual(StoreApprovalStateEnum.DRAFT.Id, WebDirectSalesOrder_DirectSalesOrderDTO.StoreApprovalStateId);
            Assert.AreEqual(RequestStateEnum.APPROVED.Id, WebDirectSalesOrder_DirectSalesOrderDTO.RequestStateId);
        }
        private async Task Then_CheckResult(string path)
        {
            string Payload = System.IO.File.ReadAllText(path);
            WebDirectSalesOrder_DirectSalesOrderDTO Expected = JsonConvert.DeserializeObject<WebDirectSalesOrder_DirectSalesOrderDTO>(Payload);
            Assert.AreEqual(true, WebDirectSalesOrder_DirectSalesOrderDTO.Errors == null);
            await Then_Equals(Expected, WebDirectSalesOrder_DirectSalesOrderDTO);
        }
        private async Task Then_CheckResultFailed(string path)
        {
            string Payload = System.IO.File.ReadAllText(path);
            WebDirectSalesOrder_DirectSalesOrderDTO Expected = JsonConvert.DeserializeObject<WebDirectSalesOrder_DirectSalesOrderDTO>(Payload);
            await Then_ErrorEquals(Expected, WebDirectSalesOrder_DirectSalesOrderDTO);
        }
        private async Task Then_Search_Success(string path)
        {
            string Payload = System.IO.File.ReadAllText(path);
            List<WebDirectSalesOrder_DirectSalesOrderDTO> Expected = JsonConvert.DeserializeObject<List<WebDirectSalesOrder_DirectSalesOrderDTO>>(Payload);
            Assert.AreEqual(Expected.Count, WebDirectSalesOrder_DirectSalesOrderDTOs.Count);
            if (Expected.Count == WebDirectSalesOrder_DirectSalesOrderDTOs.Count)
            {
                for (int i = 0; i < WebDirectSalesOrder_DirectSalesOrderDTOs.Count; i++)
                {
                    await Then_Equals(Expected[i], WebDirectSalesOrder_DirectSalesOrderDTOs[i]);
                }
            }
        }
    }
}
