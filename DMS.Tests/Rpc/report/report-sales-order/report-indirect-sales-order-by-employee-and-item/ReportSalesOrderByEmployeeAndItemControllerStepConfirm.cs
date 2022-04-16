using DMS.Rpc.reports.report_sales_order.report_indirect_sales_order_by_employee_and_item;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_sales_order.report_indirect_sales_order_by_employee_and_item
{
    public partial class ReportSalesOrderByEmployeeAndItemControllerFeature
    {
        public async Task Then_ReportSalesOrderByEmployeeAndItem_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportSalesOrderByEmployeeAndItem_ReportSalesOrderByEmployeeAndItemDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportSalesOrderByEmployeeAndItemDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationId, ReportSalesOrderByEmployeeAndItemDTOs[i].OrganizationId);
                Assert.AreEqual(Expected[i].OrganizationName, ReportSalesOrderByEmployeeAndItemDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].SaleEmployees.Count, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees.Count);

                for (int j = 0; j < Expected[i].SaleEmployees.Count; j++)
                {
                    Assert.AreEqual(Expected[i].SaleEmployees[j].Username, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Username);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].DisplayName, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].DisplayName);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].Items.Count, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items.Count);

                    for (int k = 0; k < Expected[i].SaleEmployees[j].Items.Count; k++)
                    {
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].Id, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items[k].Id);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].Code, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items[k].Code);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].Name, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items[k].Name);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].UnitOfMeasureName, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items[k].UnitOfMeasureName);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].SaleStock, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items[k].SaleStock);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].PromotionStock, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items[k].PromotionStock);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].SalePriceAverage, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items[k].SalePriceAverage);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].Discount, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items[k].Discount);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].Revenue, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items[k].Revenue);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].BuyerStoreCounter, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items[k].BuyerStoreCounter);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Items[k].SalesOrderCounter, ReportSalesOrderByEmployeeAndItemDTOs[i].SaleEmployees[j].Items[k].SalesOrderCounter);
                    }
                }
            }
        }
    }
}
