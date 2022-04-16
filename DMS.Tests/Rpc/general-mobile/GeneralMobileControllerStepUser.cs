using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Linq;
using DMS.Entities;
using DMS.Helpers;
using DMS.Models;
using DMS.Enums;
using DMS.Rpc.lucky_draw;
using Newtonsoft.Json;
using TrueSight.Common;
using Microsoft.AspNetCore.Mvc;
using DMS.Rpc.mobile.general_mobile;

namespace DMS.Tests.Rpc.general_mobile
{
    public partial class LuckyDrawControllerFeature
    {
        private async Task When_UserInput(string path)
        {
            string Payload = System.IO.File.ReadAllText(path);
            GeneralMobile_LuckyDrawDTO = JsonConvert.DeserializeObject<GeneralMobile_LuckyDrawDTO>(Payload);
        }
        private async Task When_UserInputNoFilter()
        {
            GeneralMobile_LuckyDrawFilterDTO = new GeneralMobile_LuckyDrawFilterDTO
            {
                Take = int.MaxValue,
                Skip = 0,
            };
        }

        private async Task Get_ListLuckyDraw()
        {
            await MobileController.ListLuckyDraw(GeneralMobile_LuckyDrawFilterDTO);
        }
    }
}
