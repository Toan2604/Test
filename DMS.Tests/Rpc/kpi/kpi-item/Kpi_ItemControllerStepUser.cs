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
using DMS.Rpc.kpi_item;

namespace DMS.Tests.Rpc.kpi.kpi_item
{
    public partial class Kpi_ItemControllerFeature
    {
        private async Task When_UserInput(string path)
        {
            string Payload = File.ReadAllText(path);

            KpiItem_KpiItemDTO = JsonConvert.DeserializeObject<KpiItem_KpiItemDTO>(Payload);
        }

        private async Task When_Create()
        {
            await KpiItemController.Create(KpiItem_KpiItemDTO);
        }

        private async Task When_Update()
        {
            await KpiItemController.Update(KpiItem_KpiItemDTO);
        }

        private async Task When_Delete()
        {
            await KpiItemController.Delete(KpiItem_KpiItemDTO);
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
            await KpiItemController.Import(FormFile);
        }
    }
}
