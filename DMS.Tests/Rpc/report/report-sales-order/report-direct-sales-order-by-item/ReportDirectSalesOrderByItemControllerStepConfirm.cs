using DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_item;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_direct_sales_order_by_item
{
    public partial class ReportDirectSalesOrderByItemControllerFeature
    {
        public async Task Then_ReportDirectSalesOrderByItem_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportDirectSalesOrderByItem_ReportDirectSalesOrderByItemDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportDirectSalesOrderByItemDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationId, ReportDirectSalesOrderByItemDTOs[i].OrganizationId);
                Assert.AreEqual(Expected[i].OrganizationName, ReportDirectSalesOrderByItemDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].ItemDetails.Count, ReportDirectSalesOrderByItemDTOs[i].ItemDetails.Count);

                for (int j = 0; j < Expected[i].ItemDetails.Count; j++)
                {
                    
                        Assert.AreEqual(Expected[i].ItemDetails[j].ItemId, ReportDirectSalesOrderByItemDTOs[i].ItemDetails[j].ItemId);
                        Assert.AreEqual(Expected[i].ItemDetails[j].ItemCode, ReportDirectSalesOrderByItemDTOs[i].ItemDetails[j].ItemCode);
                        Assert.AreEqual(Expected[i].ItemDetails[j].ItemName, ReportDirectSalesOrderByItemDTOs[i].ItemDetails[j].ItemName);
                        Assert.AreEqual(Expected[i].ItemDetails[j].UnitOfMeasureName, ReportDirectSalesOrderByItemDTOs[i].ItemDetails[j].UnitOfMeasureName);
                        Assert.AreEqual(Expected[i].ItemDetails[j].SaleStock, ReportDirectSalesOrderByItemDTOs[i].ItemDetails[j].SaleStock);
                        Assert.AreEqual(Expected[i].ItemDetails[j].PromotionStock, ReportDirectSalesOrderByItemDTOs[i].ItemDetails[j].PromotionStock);
                        Assert.AreEqual(Expected[i].ItemDetails[j].Discount, ReportDirectSalesOrderByItemDTOs[i].ItemDetails[j].Discount);
                        Assert.AreEqual(Expected[i].ItemDetails[j].Revenue, ReportDirectSalesOrderByItemDTOs[i].ItemDetails[j].Revenue);
                        Assert.AreEqual(Expected[i].ItemDetails[j].BuyerStoreCounter, ReportDirectSalesOrderByItemDTOs[i].ItemDetails[j].BuyerStoreCounter);
                        Assert.AreEqual(Expected[i].ItemDetails[j].SalesOrderCounter, ReportDirectSalesOrderByItemDTOs[i].ItemDetails[j].SalesOrderCounter);
                    
                }
            }
        }
    }
}
