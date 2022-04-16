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
using DMS.Rpc.store_balance;
using TrueSight.Common;

namespace DMS.Tests.Rpc.store_balance
{
    public partial class StoreBalanceControllerFeature
    {
        private async Task Then_Create_Success()
        {
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.Id).Camelize()));
            Assert.AreEqual(75, StoreBalance_StoreBalanceDTO.Id);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.OrganizationId).Camelize()));
            Assert.AreEqual(1, StoreBalance_StoreBalanceDTO.OrganizationId);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.StoreId).Camelize()));
            Assert.AreEqual(101, StoreBalance_StoreBalanceDTO.StoreId);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.CreditAmount).Camelize()));
            Assert.AreEqual(50000000, StoreBalance_StoreBalanceDTO.CreditAmount);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.DebitAmount).Camelize()));
            Assert.AreEqual(30000000, StoreBalance_StoreBalanceDTO.DebitAmount);
        }
        private async Task Then_Create_Failed()
        {
            Assert.AreEqual(true, StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.Store).Camelize()));
            Assert.AreEqual("Công nợ của đại lý với đơn vị được chọn đã tồn tại", StoreBalance_StoreBalanceDTO.Errors[nameof(StoreBalance_StoreBalanceDTO.Store).Camelize()]);
            Assert.AreEqual(true, StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.DebitAmount).Camelize()));
            Assert.AreEqual("Dư nợ không hợp lệ", StoreBalance_StoreBalanceDTO.Errors[nameof(StoreBalance_StoreBalanceDTO.DebitAmount).Camelize()]);
            Assert.AreEqual(true, StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.CreditAmount).Camelize()));
            Assert.AreEqual("Dư có không hợp lệ", StoreBalance_StoreBalanceDTO.Errors[nameof(StoreBalance_StoreBalanceDTO.CreditAmount).Camelize()]);
        }
        private async Task Then_Create_MissingRequiredField_Failed()
        {
            Assert.AreEqual(true, StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.Organization).Camelize()));
            Assert.AreEqual("Chưa chọn đơn vị chứa công nợ", StoreBalance_StoreBalanceDTO.Errors[nameof(StoreBalance_StoreBalanceDTO.Organization).Camelize()]);
            Assert.AreEqual(true, StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.Store).Camelize()));
            Assert.AreEqual("Chưa nhập tên đại lý", StoreBalance_StoreBalanceDTO.Errors[nameof(StoreBalance_StoreBalanceDTO.Store).Camelize()]);
        }
        private async Task Then_Update_Success()
        {
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.Id)));
            Assert.AreEqual(52, StoreBalance_StoreBalanceDTO.Id);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.OrganizationId)));
            Assert.AreEqual(20283, StoreBalance_StoreBalanceDTO.OrganizationId);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.StoreId)));
            Assert.AreEqual(115140, StoreBalance_StoreBalanceDTO.StoreId);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.CreditAmount)));
            Assert.AreEqual(150000000, StoreBalance_StoreBalanceDTO.CreditAmount);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.DebitAmount)));
            Assert.AreEqual(30000000, StoreBalance_StoreBalanceDTO.DebitAmount);
        }
        private async Task Then_Update_Failed()
        {
            Assert.AreEqual(true, StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.Organization).Camelize()));
            Assert.AreEqual("Tiêu chí không được phép sửa", StoreBalance_StoreBalanceDTO.Errors[nameof(StoreBalance_StoreBalanceDTO.Organization).Camelize()]);
            Assert.AreEqual(true, StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.Store).Camelize()));
            Assert.AreEqual("Tiêu chí không được phép sửa", StoreBalance_StoreBalanceDTO.Errors[nameof(StoreBalance_StoreBalanceDTO.Store).Camelize()]);
        }
        private async Task Then_Update_NegativeAmount_Failed()
        {
            Assert.AreEqual(true, StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.CreditAmount).Camelize()));
            Assert.AreEqual("Dư có không hợp lệ", StoreBalance_StoreBalanceDTO.Errors[nameof(StoreBalance_StoreBalanceDTO.CreditAmount).Camelize()]);
            Assert.AreEqual(true, StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.DebitAmount).Camelize()));
            Assert.AreEqual("Dư nợ không hợp lệ", StoreBalance_StoreBalanceDTO.Errors[nameof(StoreBalance_StoreBalanceDTO.DebitAmount).Camelize()]);
        }
        private async Task Then_Update_RoundAmount_Success()
        {
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.Id)));
            Assert.AreEqual(52, StoreBalance_StoreBalanceDTO.Id);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.OrganizationId)));
            Assert.AreEqual(20283, StoreBalance_StoreBalanceDTO.OrganizationId);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.StoreId)));
            Assert.AreEqual(115140, StoreBalance_StoreBalanceDTO.StoreId);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.CreditAmount)));
            Assert.AreEqual(11110.55, StoreBalance_StoreBalanceDTO.CreditAmount);
            Assert.AreEqual(false, StoreBalance_StoreBalanceDTO.Errors != null && StoreBalance_StoreBalanceDTO.Errors.ContainsKey(nameof(StoreBalance_StoreBalanceDTO.DebitAmount)));
            Assert.AreEqual(234252345345.12, StoreBalance_StoreBalanceDTO.DebitAmount);
        }
        private async Task Then_Delete_Success()
        {
            int count = await DataContext.StoreBalance.Where(x => x.Id == StoreBalance_StoreBalanceDTO.Id && !x.DeletedAt.HasValue).CountAsync();
            Assert.AreEqual(true, StoreBalance_StoreBalanceDTO.Errors == null);
            Assert.AreEqual(0, count);
        }
        private async Task Then_BulkDelete_Success()
        {
            IdFilter IdFilter = new IdFilter { In = StoreBalanceIds };
            int count = await DataContext.StoreBalance.Where(x => x.Id, IdFilter).Where(x => !x.DeletedAt.HasValue).CountAsync();
            Assert.AreEqual(0, count);
        }
        private async Task Then_SearchByStoreName_Success()
        {
            Assert.AreEqual(1, StoreBalance_StoreBalanceDTOs.Count);
            Assert.AreEqual(115140, StoreBalance_StoreBalanceDTOs[0].StoreId);
            Assert.AreEqual("ch hoa dao", StoreBalance_StoreBalanceDTOs[0].Store.Name);
            Assert.AreEqual(52, StoreBalance_StoreBalanceDTOs[0].Id);
            Assert.AreEqual(50000000.0000M, StoreBalance_StoreBalanceDTOs[0].DebitAmount);
            Assert.AreEqual(30000000.0000M, StoreBalance_StoreBalanceDTOs[0].CreditAmount);
        }
        private async Task Then_SearchByStoreCode_Success()
        {
            Assert.AreEqual(4, StoreBalance_StoreBalanceDTOs.Count);
            Assert.AreEqual(114596, StoreBalance_StoreBalanceDTOs[0].StoreId);
            Assert.AreEqual("Thao Test", StoreBalance_StoreBalanceDTOs[0].Store.Name);
            Assert.AreEqual(70, StoreBalance_StoreBalanceDTOs[0].Id);
            Assert.AreEqual(20270, StoreBalance_StoreBalanceDTOs[0].OrganizationId);
            Assert.AreEqual(50000000.0000M, StoreBalance_StoreBalanceDTOs[0].DebitAmount);
            Assert.AreEqual(30000000.0000M, StoreBalance_StoreBalanceDTOs[0].CreditAmount);

            Assert.AreEqual(115145, StoreBalance_StoreBalanceDTOs[1].StoreId);
            Assert.AreEqual("Hi", StoreBalance_StoreBalanceDTOs[1].Store.Name);
            Assert.AreEqual(55, StoreBalance_StoreBalanceDTOs[1].Id);
            Assert.AreEqual(20280, StoreBalance_StoreBalanceDTOs[1].OrganizationId);
            Assert.AreEqual(2423423.0000M, StoreBalance_StoreBalanceDTOs[1].DebitAmount);
            Assert.AreEqual(346664567.0000M, StoreBalance_StoreBalanceDTOs[1].CreditAmount);

            Assert.AreEqual(115145, StoreBalance_StoreBalanceDTOs[2].StoreId);
            Assert.AreEqual("Hi", StoreBalance_StoreBalanceDTOs[2].Store.Name);
            Assert.AreEqual(2, StoreBalance_StoreBalanceDTOs[2].Id);
            Assert.AreEqual(214, StoreBalance_StoreBalanceDTOs[2].OrganizationId);
            Assert.AreEqual(312300000.0000M, StoreBalance_StoreBalanceDTOs[2].DebitAmount);
            Assert.AreEqual(312300004.0000M, StoreBalance_StoreBalanceDTOs[2].CreditAmount);

            Assert.AreEqual(115145, StoreBalance_StoreBalanceDTOs[3].StoreId);
            Assert.AreEqual("Hi", StoreBalance_StoreBalanceDTOs[3].Store.Name);
            Assert.AreEqual(1, StoreBalance_StoreBalanceDTOs[3].Id);
            Assert.AreEqual(215, StoreBalance_StoreBalanceDTOs[3].OrganizationId);
            Assert.AreEqual(223223232.0000M, StoreBalance_StoreBalanceDTOs[3].DebitAmount);
            Assert.AreEqual(343434.0000M, StoreBalance_StoreBalanceDTOs[3].CreditAmount);
        }
        private async Task Then_SearchByOrganization_Success()
        {
            Assert.AreEqual(2, StoreBalance_StoreBalanceDTOs.Count);
            Assert.AreEqual(115135, StoreBalance_StoreBalanceDTOs[0].StoreId);
            Assert.AreEqual("ch thao test", StoreBalance_StoreBalanceDTOs[0].Store.Name);
            Assert.AreEqual(69, StoreBalance_StoreBalanceDTOs[0].Id);
            Assert.AreEqual(20277, StoreBalance_StoreBalanceDTOs[0].OrganizationId);
            Assert.AreEqual(50000000.0000M, StoreBalance_StoreBalanceDTOs[0].DebitAmount);
            Assert.AreEqual(30000000.0000M, StoreBalance_StoreBalanceDTOs[0].CreditAmount);

            Assert.AreEqual(115123, StoreBalance_StoreBalanceDTOs[1].StoreId);
            Assert.AreEqual("Hanh", StoreBalance_StoreBalanceDTOs[1].Store.Name);
            Assert.AreEqual(57, StoreBalance_StoreBalanceDTOs[1].Id);
            Assert.AreEqual(20277, StoreBalance_StoreBalanceDTOs[1].OrganizationId);
            Assert.AreEqual(50000000.0000M, StoreBalance_StoreBalanceDTOs[1].DebitAmount);
            Assert.AreEqual(30000000.0000M, StoreBalance_StoreBalanceDTOs[1].CreditAmount);
        }
        private async Task Then_SearchMultiFilter_Success()
        {
            Assert.AreEqual(2, StoreBalance_StoreBalanceDTOs.Count);
            Assert.AreEqual(115120, StoreBalance_StoreBalanceDTOs[0].StoreId);
            Assert.AreEqual("Checkin", StoreBalance_StoreBalanceDTOs[0].Store.Name);
            Assert.AreEqual("TV01.Cap2123.0115120", StoreBalance_StoreBalanceDTOs[0].Store.Code);
            Assert.AreEqual("Checkin", StoreBalance_StoreBalanceDTOs[0].Store.CodeDraft);
            Assert.AreEqual(63, StoreBalance_StoreBalanceDTOs[0].Id);
            Assert.AreEqual(259, StoreBalance_StoreBalanceDTOs[0].OrganizationId);
            Assert.AreEqual(20000000.0000M, StoreBalance_StoreBalanceDTOs[0].DebitAmount);
            Assert.AreEqual(10000000.0000M, StoreBalance_StoreBalanceDTOs[0].CreditAmount);

            Assert.AreEqual(115137, StoreBalance_StoreBalanceDTOs[1].StoreId);
            Assert.AreEqual("ch abc", StoreBalance_StoreBalanceDTOs[1].Store.Name);
            Assert.AreEqual("BH.FAST_BL003.0115137", StoreBalance_StoreBalanceDTOs[1].Store.Code);
            Assert.AreEqual("kjhg", StoreBalance_StoreBalanceDTOs[1].Store.CodeDraft);
            Assert.AreEqual(53, StoreBalance_StoreBalanceDTOs[1].Id);
            Assert.AreEqual(20282, StoreBalance_StoreBalanceDTOs[1].OrganizationId);
            Assert.AreEqual(20000000.0000M, StoreBalance_StoreBalanceDTOs[1].DebitAmount);
            Assert.AreEqual(10000000.0000M, StoreBalance_StoreBalanceDTOs[1].CreditAmount);
        }
    }
}
