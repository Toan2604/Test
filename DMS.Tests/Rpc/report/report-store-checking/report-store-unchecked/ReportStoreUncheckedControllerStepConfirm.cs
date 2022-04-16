using DMS.Rpc.reports.report_store_checking.report_store_checked;
using DMS.Rpc.reports.report_store_checking.report_store_unchecked;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_store_checking.report_store_unchecked
{
    public partial class ReportStoreUncheckedControllerFeature
    {
        public async Task Then_ReportStoreUnchecked_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportStoreUnchecked_ReportStoreUncheckedDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportStoreUnchecked_ReportStoreUncheckedDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationName, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].SaleEmployees.Count, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees.Count);

                for (int j = 0; j < Expected[i].SaleEmployees.Count; j++)
                {
                    Assert.AreEqual(Expected[i].SaleEmployees[j].SaleEmployeeId, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].SaleEmployeeId);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].Username, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].Username);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].DisplayName, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].DisplayName);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].Stores.Count, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].Stores.Count);

                    for (int k = 0; k < Expected[i].SaleEmployees[j].Stores.Count; k++)
                    {
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Stores[k].AppUserId, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].Stores[k].AppUserId);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Stores[k].Date, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].Stores[k].Date);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Stores[k].DateDisplay, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].Stores[k].DateDisplay);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Stores[k].StoreAddress, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].Stores[k].StoreAddress);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Stores[k].StoreCode, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].Stores[k].StoreCode);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Stores[k].StoreName, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].Stores[k].StoreName);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Stores[k].StorePhone, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].Stores[k].StorePhone);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Stores[k].StoreStatusName, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].Stores[k].StoreStatusName);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].Stores[k].StoreTypeName, ReportStoreUnchecked_ReportStoreUncheckedDTOs[i].SaleEmployees[j].Stores[k].StoreTypeName);
                    }
                }
            }
        }
    }
}
