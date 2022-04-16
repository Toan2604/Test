using DMS.Rpc.reports.report_sales_order.report_direct_sales_order_general;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_direct_sales_order_general
{
    public partial class ReportDirectSalesOrderGeneralControllerFeature
    {
        public async Task Then_ReportDirectSalesOrderGeneral_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportDirectSalesOrderGeneral_ReportDirectSalesOrderGeneralDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportDirectSalesOrderGeneralDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationId, ReportDirectSalesOrderGeneralDTOs[i].OrganizationId);
                Assert.AreEqual(Expected[i].OrganizationName, ReportDirectSalesOrderGeneralDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].SalesOrders.Count, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders.Count);

                for (int j = 0; j < Expected[i].SalesOrders.Count; j++)
                {

                    Assert.AreEqual(Expected[i].SalesOrders[j].Id, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].Id);
                    Assert.AreEqual(Expected[i].SalesOrders[j].Code, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].Code);
                    Assert.AreEqual(Expected[i].SalesOrders[j].BuyerStoreCode, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].BuyerStoreCode);
                    Assert.AreEqual(Expected[i].SalesOrders[j].BuyerStoreCodeDraft, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].BuyerStoreCodeDraft);
                    Assert.AreEqual(Expected[i].SalesOrders[j].BuyerStoreName, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].BuyerStoreName);
                    Assert.AreEqual(Expected[i].SalesOrders[j].BuyerStoreStatusName, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].BuyerStoreStatusName);
                    Assert.AreEqual(Expected[i].SalesOrders[j].SaleEmployeeName, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].SaleEmployeeName);
                    Assert.AreEqual(Expected[i].SalesOrders[j].SaleEmployeeUsername, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].SaleEmployeeUsername);
                    Assert.AreEqual(Expected[i].SalesOrders[j].OrderDate, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].OrderDate);
                    Assert.AreEqual(Expected[i].SalesOrders[j].eOrderDate, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].eOrderDate);
                    Assert.AreEqual(Expected[i].SalesOrders[j].Discount, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].Discount);
                    Assert.AreEqual(Expected[i].SalesOrders[j].TaxValue, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].TaxValue);
                    Assert.AreEqual(Expected[i].SalesOrders[j].SubTotal, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].SubTotal);
                    Assert.AreEqual(Expected[i].SalesOrders[j].Total, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].Total);
                    Assert.AreEqual(Expected[i].SalesOrders[j].PromotionValue, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].PromotionValue);
                    Assert.AreEqual(Expected[i].SalesOrders[j].TotalAfterPromotion, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].TotalAfterPromotion);
                    Assert.AreEqual(Expected[i].SalesOrders[j].StoreCheckingId, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].StoreCheckingId);
                    Assert.AreEqual(Expected[i].SalesOrders[j].CreatedInCheckin, ReportDirectSalesOrderGeneralDTOs[i].SalesOrders[j].CreatedInCheckin);
                }
            }
        }
    }
}
