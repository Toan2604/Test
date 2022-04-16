using DMS.Rpc.reports.report_store_checking.report_store_checked;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.report.report_store_checking.report_store_checked
{
    public partial class ReportStoreCheckedControllerFeature
    {
        public async Task Then_ReportStoreChecked_Result(string path)
        {
            var Expected = ReadFileFromJson<List<ReportStoreChecked_ReportStoreCheckedDTO>>(path);

            Assert.AreEqual(Expected.Count, ReportStoreChecked_ReportStoreCheckedDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].OrganizationName, ReportStoreChecked_ReportStoreCheckedDTOs[i].OrganizationName);

                Assert.AreEqual(Expected[i].SaleEmployees.Count, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees.Count);

                for (int j = 0; j < Expected[i].SaleEmployees.Count; j++)
                {
                    Assert.AreEqual(Expected[i].SaleEmployees[j].SaleEmployeeId, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].SaleEmployeeId);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].Username, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].Username);
                    Assert.AreEqual(Expected[i].SaleEmployees[j].DisplayName, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].DisplayName);

                    Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates.Count, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates.Count);

                    for (int k = 0; k < Expected[i].SaleEmployees[j].StoreCheckingGroupByDates.Count; k++)
                    {
                        Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].DateString, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].DateString);
                        Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].DayOfWeek, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].DayOfWeek);

                        Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings.Count, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings.Count);

                        for (int l = 0; l < Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings.Count; l++)
                        {
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].CheckIn, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].CheckIn);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].CheckInDistance, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].CheckInDistance);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].CheckOut, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].CheckOut);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].CheckOutDistance, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].CheckOutDistance);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].Closed, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].Closed);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].Duaration, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].Duaration);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].eCheckIn, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].eCheckIn);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].eCheckOut, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].eCheckOut);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].ePlanned, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].ePlanned);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].eSalesOrder, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].eSalesOrder);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].Id, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].Id);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].ImageCounter, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].ImageCounter);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].IsClosed, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].IsClosed);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].Planned, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].Planned);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].SaleEmployeeId, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].SaleEmployeeId);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].SalesOrderCounter, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].SalesOrderCounter);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreAddress, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreAddress);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreAddress, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreAddress);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreCode, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreCode);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreCodeDraft, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreCodeDraft);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreName, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreName);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreStatusId, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreStatusId);
                            Assert.AreEqual(Expected[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreStatusName, ReportStoreChecked_ReportStoreCheckedDTOs[i].SaleEmployees[j].StoreCheckingGroupByDates[k].StoreCheckings[l].StoreStatusName);

                        }
                    }
                }
            }
        }
    }
}
