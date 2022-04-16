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
namespace DMS.Tests.Rpc.general_mobile
{
    public partial class LuckyDrawControllerFeature
    {
        private async Task Then_SearchNoFilter_Success()
        {
            Assert.AreEqual(1, GeneralMobile_LuckyDrawDTOs.Count);
            Assert.AreEqual("inscryjebe", GeneralMobile_LuckyDrawDTOs[0].Name);
            Assert.AreEqual("w3r3wre", GeneralMobile_LuckyDrawDTOs[0].Code);
            //TO DO
        }


        #region Create + Update Success
        private async Task Then_Id(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.Id)));
            Assert.AreEqual(108, LuckyDraw_LuckyDrawDTO.Id);
        }
        private async Task Then_Update_Id(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.Id)));
            Assert.AreEqual(107, LuckyDraw_LuckyDrawDTO.Id);
        }
        private async Task Then_Code(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.Code)));
            Assert.AreEqual("BDD2222", LuckyDraw_LuckyDrawDTO.Code);
        }
        private async Task Then_Name(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.Name)));
            Assert.AreEqual("chương trình test BDD2222", LuckyDraw_LuckyDrawDTO.Name);
        }
        private async Task Then_LuckyDrawTypeId(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.LuckyDrawTypeId)));
            Assert.AreEqual(1, LuckyDraw_LuckyDrawDTO.LuckyDrawTypeId);
        }
        private async Task Then_OrganizationId(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.OrganizationId)));
            Assert.AreEqual(214, LuckyDraw_LuckyDrawDTO.OrganizationId);
        }
        private async Task Then_RevenuePerTurn(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.RevenuePerTurn)));
            Assert.AreEqual(50000000.0000M, LuckyDraw_LuckyDrawDTO.RevenuePerTurn);
        }
        private async Task Then_StartAt(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.StartAt)));
            Assert.AreEqual(new DateTime(2021, 11, 30), LuckyDraw_LuckyDrawDTO.StartAt?.Date);
        }
        private async Task Then_EndAt(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.EndAt)));
            Assert.AreEqual(new DateTime(2022, 3, 2), LuckyDraw_LuckyDrawDTO.EndAt?.Date);
        }
        private async Task Then_AvatarImageId(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.AvatarImageId)));
            Assert.AreEqual(17, LuckyDraw_LuckyDrawDTO.AvatarImageId);
        }
        private async Task Then_ImageId(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.ImageId)));
            Assert.AreEqual(17, LuckyDraw_LuckyDrawDTO.ImageId);
        }
        private async Task Then_Description(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.Description)));
            Assert.AreEqual("<p>chương tr&igrave;nh test BDD</p>", LuckyDraw_LuckyDrawDTO.Description);
        }
        private async Task Then_StatusId(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.StatusId)));
            Assert.AreEqual(1, LuckyDraw_LuckyDrawDTO.StatusId);
        }
        private async Task Then_Used(LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO)
        {
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Errors != null && LuckyDraw_LuckyDrawDTO.Errors.ContainsKey(nameof(LuckyDraw.Used)));
            Assert.AreEqual(false, LuckyDraw_LuckyDrawDTO.Used);
        }
        #endregion

    }
}
