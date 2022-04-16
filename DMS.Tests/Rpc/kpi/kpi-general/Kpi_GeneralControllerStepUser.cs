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

namespace DMS.Tests.Rpc.kpi.kpi_general
{
    public partial class Kpi_GeneralControllerFeature
    {
        private async Task When_UserInput(string path)
        {
            string Payload = File.ReadAllText(path);
            
            KpiGeneral_KpiGeneralDTO = JsonConvert.DeserializeObject<KpiGeneral_KpiGeneralDTO>(Payload);
        }

        private async Task When_Create()
        {
            await KpiGeneralController.Create(KpiGeneral_KpiGeneralDTO);
        }

        private async Task When_Update()
        {
            await KpiGeneralController.Update(KpiGeneral_KpiGeneralDTO);
        }

        private async Task When_Delete()
        {
            await KpiGeneralController.Delete(KpiGeneral_KpiGeneralDTO);
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
            await KpiGeneralController.Import(FormFile);
        }
    }
}
