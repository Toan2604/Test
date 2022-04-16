using DMS.Rpc.reports.report_sales_order.report_direct_sales_order_by_employee_and_item;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_direct_sales_order_by_employee_and_item
{
    public partial class ReportDirectSalesOrderByEmployeeAndItemControllerFeature
    {
        public async Task Then_ReportDirectSalesOrderByEmployeeAndItem_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportDirectSalesOrderByEmployeeAndItem_ReportDirectSalesOrderByEmployeeAndItemDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportDirectSalesOrderByEmployeeAndItemDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationId, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].OrganizationId);
                Assert.AreEqual(Expected[i].OrganizationName, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].SaleEmployees.Count, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees.Count);

                for (int j = 0; j < Expected[i].SaleEmployees.Count; j++)
                {
                    Assert.AreEqual(Expected[i].SaleEmployees[j].Username, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Username);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].DisplayName, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].DisplayName);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].Items.Count, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items.Count);

                    for (int k = 0; k < Expected[i].SaleEmployees[j].Items.Count; k++)
                    {
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].Id, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items [k].Id);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].Code, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items [k].Code);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].Name, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items [k].Name);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].UnitOfMeasureName, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items [k].UnitOfMeasureName);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].SaleStock, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items [k].SaleStock);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].PromotionStock, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items [k].PromotionStock);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].SalePriceAverage, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items [k].SalePriceAverage);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].Discount, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items [k].Discount);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].Revenue, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items [k].Revenue);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].BuyerStoreCounter, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items [k].BuyerStoreCounter);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].SalesOrderCounter, ReportDirectSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items [k].SalesOrderCounter);
                    }
                }
            }
        }
    }
}
