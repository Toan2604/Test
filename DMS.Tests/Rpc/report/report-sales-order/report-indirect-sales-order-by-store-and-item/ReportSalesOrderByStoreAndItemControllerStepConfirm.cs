using DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_store_and_item;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_indirect_sales_order_by_store_and_item
{
    public partial class ReportSalesOrderByStoreAndItemControllerFeature
    {
        public async Task Then_ReportSalesOrderByStoreAndItem_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportSalesOrderByStoreAndItem_ReportSalesOrderByStoreAndItemDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportSalesOrderByStoreAndItemDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationName, ReportSalesOrderByStoreAndItemDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].Stores.Count, ReportSalesOrderByStoreAndItemDTOs[i].Stores.Count);

                for (int j = 0; j < Expected[i].Stores.Count; j++)
                {
                    Assert.AreEqual(Expected[i].Stores[j].Code, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Code);
                    Assert.AreEqual(Expected[i].Stores[j].Name, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Name);
                    Assert.AreEqual(Expected[i].Stores[j].StoreStatusId, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].StoreStatusId);
                    Assert.AreEqual(Expected[i].Stores[j].CodeDraft, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].CodeDraft);
                    Assert.AreEqual(Expected[i].Stores[j].StoreTypeId, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].StoreTypeId);
                    Assert.AreEqual(Expected[i].Stores[j].Address, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Address);

                    Assert.AreEqual(Expected[i].Stores[j].Items.Count, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Items.Count);

                    for (int k = 0; k < Expected[i].Stores[j].Items.Count; k++)
                    {
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].Id, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].Id);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].Code, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].Code);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].Name, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].Name);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].UnitOfMeasureName, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].UnitOfMeasureName);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].SaleStock, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].SaleStock);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].PromotionStock, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].PromotionStock);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].SalePriceAverage, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].SalePriceAverage);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].Discount, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].Discount);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].Revenue, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].Revenue);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].SalesOrderCounter, ReportSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].SalesOrderCounter);
                    }
                }
            }
        }
    }
}
