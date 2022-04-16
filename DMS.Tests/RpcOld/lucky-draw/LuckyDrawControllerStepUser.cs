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

namespace DMS.Tests.Rpc.lucky_draw
{
    public partial class LuckyDrawControllerFeature
    {
        private async Task When_UserInput(string path)
        {
            string Payload = System.IO.File.ReadAllText(path);
            LuckyDraw_LuckyDrawDTO = JsonConvert.DeserializeObject<LuckyDraw_LuckyDrawDTO>(Payload);
        }
        private async Task When_UserInputFilter()
        {
            LuckyDraw_LuckyDrawFilterDTO = new LuckyDraw_LuckyDrawFilterDTO
            {
                Take = int.MaxValue,
                Skip = 0,
                Name = new StringFilter { Contain = "cấp 1" }
            };
        }
        private async Task When_UserInputManyFilter()
        {
            LuckyDraw_LuckyDrawFilterDTO = new LuckyDraw_LuckyDrawFilterDTO
            {
                Take = int.MaxValue,
                Skip = 0,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id },
                OrganizationId = new IdFilter { Equal = 214 },
                LuckyDrawTypeId = new IdFilter { Equal = LuckyDrawTypeEnum.STOREGROUPING.Id },
                StartAt = new DateFilter { GreaterEqual = new DateTime(2021, 08, 19)},
                EndAt = new DateFilter { LessEqual = new DateTime(2021, 08, 20)},
            };
        }
        private async Task When_UserCreate()
        {
            await LuckyDrawController.Create(LuckyDraw_LuckyDrawDTO);
        }
        private async Task When_UserUpdate()
        {
            LuckyDraw_LuckyDrawDTO = (LuckyDraw_LuckyDrawDTO)((BadRequestObjectResult) (await LuckyDrawController.Update(LuckyDraw_LuckyDrawDTO)).Result).Value;
        }
        private async Task When_UserDelete()
        {
            await LuckyDrawController.Delete(LuckyDraw_LuckyDrawDTO);
        }
        private async Task When_UserSearch()
        {
            LuckyDraw_LuckyDrawDTOs = (await LuckyDrawController.List(LuckyDraw_LuckyDrawFilterDTO)).Value;
        }
    }
}
