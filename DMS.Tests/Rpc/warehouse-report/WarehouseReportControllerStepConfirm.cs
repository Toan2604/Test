using DMS.Rpc.warehouse_report;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DMS.Tests.Rpc.warehouse_report
{
    public partial class WarehouseReportControllerFeature
    {
        public async Task Then_WarehouseReport_Result(string path)
        {
            var Expected = ReadFileFromJson<List<WarehouseReport_WarehouseReportDTO>>(path);

            Assert.AreEqual(Expected.Count, WarehouseReport_WarehouseReportDTOs.Count);

            for (int i = 0; i < Expected.Count; i++)
            {
                Assert.AreEqual(Expected[i].ItemId, WarehouseReport_WarehouseReportDTOs[i].ItemId);
                Assert.AreEqual(Expected[i].UnitOfMeasureId, WarehouseReport_WarehouseReportDTOs[i].UnitOfMeasureId);
                Assert.AreEqual(Expected[i].UnitOfMeasure.Code, WarehouseReport_WarehouseReportDTOs[i].UnitOfMeasure.Code);
                Assert.AreEqual(Expected[i].Item.Code, WarehouseReport_WarehouseReportDTOs[i].Item.Code);
                Assert.AreEqual(Expected[i].Item.Name, WarehouseReport_WarehouseReportDTOs[i].Item.Name);

                Assert.AreEqual(Expected[i].Warehouses.Count, WarehouseReport_WarehouseReportDTOs[i].Warehouses.Count);
                for (int j = 0; j < Expected[i].Warehouses.Count; j++)
                {
                    Assert.AreEqual(Expected[i].Warehouses[j].WarehouseId, WarehouseReport_WarehouseReportDTOs[i].Warehouses[j].WarehouseId);
                    Assert.AreEqual(Expected[i].Warehouses[j].WarehouseCode, WarehouseReport_WarehouseReportDTOs[i].Warehouses[j].WarehouseCode);
                    Assert.AreEqual(Expected[i].Warehouses[j].TotalSaleStock, WarehouseReport_WarehouseReportDTOs[i].Warehouses[j].TotalSaleStock);

                }
            }
        }
    }
}
