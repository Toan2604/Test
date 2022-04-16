using DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_item;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_indirect_sales_order_by_item
{
    public partial class ReportSalesOrderByItemControllerFeature
    {
        public async Task Then_ReportSalesOrderByItem_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportSalesOrderByItem_ReportSalesOrderByItemDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportSalesOrderByItemDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationId, ReportSalesOrderByItemDTOs[i].OrganizationId);
                Assert.AreEqual(Expected[i].OrganizationName, ReportSalesOrderByItemDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].ItemDetails.Count, ReportSalesOrderByItemDTOs[i].ItemDetails.Count);

                for (int j = 0; j < Expected[i].ItemDetails.Count; j++)
                {

                    Assert.AreEqual(Expected[i].ItemDetails[j].ItemId, ReportSalesOrderByItemDTOs[i].ItemDetails[j].ItemId);
                    Assert.AreEqual(Expected[i].ItemDetails[j].ItemCode, ReportSalesOrderByItemDTOs[i].ItemDetails[j].ItemCode);
                    Assert.AreEqual(Expected[i].ItemDetails[j].ItemName, ReportSalesOrderByItemDTOs[i].ItemDetails[j].ItemName);
                    Assert.AreEqual(Expected[i].ItemDetails[j].UnitOfMeasureName, ReportSalesOrderByItemDTOs[i].ItemDetails[j].UnitOfMeasureName);
                    Assert.AreEqual(Expected[i].ItemDetails[j].SaleStock, ReportSalesOrderByItemDTOs[i].ItemDetails[j].SaleStock);
                    Assert.AreEqual(Expected[i].ItemDetails[j].PromotionStock, ReportSalesOrderByItemDTOs[i].ItemDetails[j].PromotionStock);
                    Assert.AreEqual(Expected[i].ItemDetails[j].Discount, ReportSalesOrderByItemDTOs[i].ItemDetails[j].Discount);
                    Assert.AreEqual(Expected[i].ItemDetails[j].Revenue, ReportSalesOrderByItemDTOs[i].ItemDetails[j].Revenue);
                    Assert.AreEqual(Expected[i].ItemDetails[j].BuyerStoreCounter, ReportSalesOrderByItemDTOs[i].ItemDetails[j].BuyerStoreCounter);
                    Assert.AreEqual(Expected[i].ItemDetails[j].SalesOrderCounter, ReportSalesOrderByItemDTOs[i].ItemDetails[j].SalesOrderCounter);

                }
            }
        }
    }
}
