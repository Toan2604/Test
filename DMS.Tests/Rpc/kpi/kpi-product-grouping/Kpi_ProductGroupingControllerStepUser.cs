using DMS.Enums;
using Newtonsoft.Json;
using DMS.Rpc.kpi_general;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TrueSight.Common;
using System.IO;
using Microsoft.AspNetCore.Http;
using DMS.Rpc.kpi_product_grouping;

namespace DMS.Tests.Rpc.kpi.kpi_product_grouping
{
    public partial class Kpi_ProductGroupingControllerFeature
    {
        private async Task When_UserInput(string path)
        {
            string Payload = File.ReadAllText(path);

            KpiProductGrouping_KpiProductGroupingDTO = JsonConvert.DeserializeObject<KpiProductGrouping_KpiProductGroupingDTO>(Payload);
        }

        private async Task When_Create()
        {
            await KpiProductGroupingController.Create(KpiProductGrouping_KpiProductGroupingDTO);
        }

        private async Task When_Update()
        {
            await KpiProductGroupingController.Update(KpiProductGrouping_KpiProductGroupingDTO);
        }

        private async Task When_Delete()
        {
            await KpiProductGroupingController.Delete(KpiProductGrouping_KpiProductGroupingDTO);
        }

        private async Task When_UserImportExcel(string path)
        {
            byte[] array = System.IO.File.ReadAllBytes(path);
            MemoryStream MemoryStream = new MemoryStream(array);
            FormFile = new FormFile(MemoryStream, 0, MemoryStream.Length, null, Path.GetFileName(path))
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            };
        }
        private async Task When_Import()
        {
            await KpiProductGroupingController.Import(FormFile);
        }
    }
}
