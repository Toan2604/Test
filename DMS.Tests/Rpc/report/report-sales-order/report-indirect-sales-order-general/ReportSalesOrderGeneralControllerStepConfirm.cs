using DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_general;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;


namespace DMS.Tests.Rpc.report.report_sales_order.report_indirect_sales_order_general
{
    public partial class ReportSalesOrderGeneralControllerFeature
    {
        public async Task Then_ReportSalesOrderGeneral_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportSalesOrderGeneral_ReportSalesOrderGeneralDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportSalesOrderGeneralDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationId, ReportSalesOrderGeneralDTOs[i].OrganizationId);
                Assert.AreEqual(Expected[i].OrganizationName, ReportSalesOrderGeneralDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].SalesOrders.Count, ReportSalesOrderGeneralDTOs[i].SalesOrders.Count);

                for (int j = 0; j < Expected[i].SalesOrders.Count; j++)
                {

                    Assert.AreEqual(Expected[i].SalesOrders[j].Id, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].Id);
                    Assert.AreEqual(Expected[i].SalesOrders[j].Code, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].Code);
                    Assert.AreEqual(Expected[i].SalesOrders[j].BuyerStoreCode, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].BuyerStoreCode);
                    Assert.AreEqual(Expected[i].SalesOrders[j].BuyerStoreCodeDraft, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].BuyerStoreCodeDraft);
                    Assert.AreEqual(Expected[i].SalesOrders[j].BuyerStoreName, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].BuyerStoreName);
                    Assert.AreEqual(Expected[i].SalesOrders[j].BuyerStoreStatusName, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].BuyerStoreStatusName);
                    Assert.AreEqual(Expected[i].SalesOrders[j].SaleEmployeeName, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].SaleEmployeeName);
                    Assert.AreEqual(Expected[i].SalesOrders[j].SaleEmployeeUsername, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].SaleEmployeeUsername);
                    Assert.AreEqual(Expected[i].SalesOrders[j].OrderDate, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].OrderDate);
                    Assert.AreEqual(Expected[i].SalesOrders[j].eOrderDate, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].eOrderDate);
                    Assert.AreEqual(Expected[i].SalesOrders[j].Discount, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].Discount);
                    Assert.AreEqual(Expected[i].SalesOrders[j].SubTotal, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].SubTotal);
                    Assert.AreEqual(Expected[i].SalesOrders[j].Total, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].Total);
                    Assert.AreEqual(Expected[i].SalesOrders[j].StoreCheckingId, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].StoreCheckingId);
                    Assert.AreEqual(Expected[i].SalesOrders[j].CreatedInCheckin, ReportSalesOrderGeneralDTOs[i].SalesOrders[j].CreatedInCheckin);
                }
            }
        }
    }
}
