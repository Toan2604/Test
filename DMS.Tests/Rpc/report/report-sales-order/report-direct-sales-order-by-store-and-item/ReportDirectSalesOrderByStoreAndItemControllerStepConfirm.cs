using DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_store_and_item;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_direct_sales_order_by_store_and_item
{
    public partial class ReportDirectSalesOrderByStoreAndItemControllerFeature
    {
        public async Task Then_ReportDirectSalesOrderByStoreAndItem_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportDirectSalesOrderByStoreAndItem_ReportDirectSalesOrderByStoreAndItemDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportDirectSalesOrderByStoreAndItemDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationName, ReportDirectSalesOrderByStoreAndItemDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].Stores.Count, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores.Count);

                for (int j = 0; j < Expected[i].Stores.Count; j++)
                {
                    Assert.AreEqual(Expected[i].Stores[j].Code, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Code);
                    Assert.AreEqual(Expected[i].Stores[j].Name, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Name);
                    Assert.AreEqual(Expected[i].Stores[j].StoreStatusId, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].StoreStatusId);
                    Assert.AreEqual(Expected[i].Stores[j].CodeDraft, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].CodeDraft);
                    Assert.AreEqual(Expected[i].Stores[j].StoreTypeId, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].StoreTypeId);
                    Assert.AreEqual(Expected[i].Stores[j].Address, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Address);

                    Assert.AreEqual(Expected[i].Stores[j].Items.Count, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Items.Count);

                    for (int k = 0; k < Expected[i].Stores[j].Items.Count; k++)
                    {
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].Id, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].Id);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].Code, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].Code);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].Name, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].Name);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].UnitOfMeasureName, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].UnitOfMeasureName);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].SaleStock, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].SaleStock);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].PromotionStock, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].PromotionStock);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].SalePriceAverage, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].SalePriceAverage);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].Discount, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].Discount);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].Revenue, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].Revenue);
                        Assert.AreEqual(Expected[i].Stores[j].Items[k].DirectSalesOrderCounter, ReportDirectSalesOrderByStoreAndItemDTOs[i].Stores[j].Items[k].DirectSalesOrderCounter);
                    }
                }
            }
        }
    }
}
