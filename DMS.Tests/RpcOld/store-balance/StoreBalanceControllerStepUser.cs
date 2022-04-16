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
using Microsoft.AspNetCore.Mvc;

namespace DMS.Tests.Rpc.store_balance
{
    public partial class StoreBalanceControllerFeature
    {
        #region Create
        private async Task When_UserInput_CreateSuccess()
        {
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO
            {
                OrganizationId = 1,
                StoreId = 101,
                CreditAmount = 50000000,
                DebitAmount = 30000000,
            };
        }
        private async Task When_UserInput_CreateFailed()
        {
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO
            {
                OrganizationId = 215,
                StoreId = 115145,
                CreditAmount = -50000000,
                DebitAmount = -30000000,
            };
        }
        private async Task When_UserInput_MissingRequiredField_CreateFailed()
        {
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO
            {
            };
        }
        private async Task When_UserCreate()
        {
            var Result = await StoreBalanceController.Create(StoreBalance_StoreBalanceDTO);
            if (Result.Value != null)
                StoreBalance_StoreBalanceDTO = Result.Value;
            if (Result.Result != null)
                StoreBalance_StoreBalanceDTO = (StoreBalance_StoreBalanceDTO)((BadRequestObjectResult)(Result).Result).Value;
        }
        #endregion
        #region Update
        private async Task When_UserInput_UpdateSuccess()
        {
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO
            {
                Id = 52,
                OrganizationId = 20283,
                StoreId = 115140,
                CreditAmount = 150000000,
                DebitAmount = 30000000,
            };
        }
        private async Task When_UserInput_UpdateFailed()
        {
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO
            {
                Id = 52,
                OrganizationId = 214, 
                StoreId = 115145, 
                CreditAmount = 150000000,
                DebitAmount = 30000000,
            };
        }
        private async Task When_UserInput_NegativeAmount_UpdateFailed()
        {
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO
            {
                Id = 52,
                OrganizationId = 20283,
                StoreId = 115140,
                CreditAmount = -150000000,
                DebitAmount = -30000000,
            };
        }
        private async Task When_UserInput_AmountEqual0_UpdateSuccess()
        {
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO
            {
                Id = 52,
                OrganizationId = 20283,
                StoreId = 115140,
                CreditAmount = 0,
                DebitAmount = 0,
            };
        }
        private async Task When_UserInput_BlankAmount_UpdateFailed()
        {
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO
            {
                Id = 52,
                OrganizationId = 20283,
                StoreId = 115140,
            };
        }
        private async Task When_UserInput_RoundAmount_UpdateSuccess()
        {
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO
            {
                Id = 52,
                OrganizationId = 20283,
                StoreId = 115140,
                CreditAmount = 11110.5454M,
                DebitAmount = 234252345345.1213231M,
            };
        }
        private async Task When_UserUpdate()
        {
            var Result = await StoreBalanceController.Update(StoreBalance_StoreBalanceDTO);
            if (Result.Value != null)
                StoreBalance_StoreBalanceDTO = Result.Value;
            if (Result.Result != null)
                StoreBalance_StoreBalanceDTO = (StoreBalance_StoreBalanceDTO)((BadRequestObjectResult)(Result).Result).Value;
        }
        #endregion
        #region Delete
        private async Task When_UserInput_DeleteSuccess()
        {
            StoreBalance_StoreBalanceDTO = new StoreBalance_StoreBalanceDTO
            {
                Id = 1,
                OrganizationId = 215,
                StoreId = 115145,
                CreditAmount = 343434,
                DebitAmount = 223223232,
            };
        }
        private async Task When_UserInput_BulkDeleteSuccess()
        {
            StoreBalanceIds = new List<long> { 1, 2, 52 };
        }

        private async Task When_UserDelete()
        {
            await StoreBalanceController.Delete(StoreBalance_StoreBalanceDTO);
        }
        private async Task When_UserBulkDelete()
        {
            await StoreBalanceController.BulkDelete(StoreBalanceIds);
        }
        #endregion

        #region List 
        private async Task When_UserInput_FilterByStoreName()
        {
            StoreBalance_StoreBalanceFilterDTO = new StoreBalance_StoreBalanceFilterDTO
            {
                Name = new TrueSight.Common.StringFilter { Contain = "ch hoa dao" }
            };
        }
        private async Task When_UserInput_FilterByStoreCode()
        {
            StoreBalance_StoreBalanceFilterDTO = new StoreBalance_StoreBalanceFilterDTO
            {
                Code = new TrueSight.Common.StringFilter { Contain = "145" }
            };
        }
        private async Task When_UserInput_FilterByOrganization()
        {
            StoreBalance_StoreBalanceFilterDTO = new StoreBalance_StoreBalanceFilterDTO
            {
                OrganizationId = new TrueSight.Common.IdFilter { Equal = 20277 }
            };
        }
        private async Task When_UserInput_MultiFilter()
        {
            StoreBalance_StoreBalanceFilterDTO = new StoreBalance_StoreBalanceFilterDTO
            {
                Code = new TrueSight.Common.StringFilter { Contain = "11" },
                Name = new TrueSight.Common.StringFilter { Contain = "ch" },
                CodeDraft = new TrueSight.Common.StringFilter { Contain = "k" },
            };
        }
        private async Task When_UserInput_Filter()
        {
            StoreBalance_StoreBalanceFilterDTO = new StoreBalance_StoreBalanceFilterDTO
            {
                Take = int.MaxValue,
                Skip = 0
            };
        }
        private async Task When_UserSearch()
        {
            var Result = await StoreBalanceController.List(StoreBalance_StoreBalanceFilterDTO);
            StoreBalance_StoreBalanceDTOs = Result.Value;
        }
        #endregion
        #region Excel
        private async Task When_UserExport()
        {
            await StoreBalanceController.Export(StoreBalance_StoreBalanceFilterDTO);
        }
        private async Task When_UserExportTemplate()
        {
            await StoreBalanceController.ExportTemplate();
        }
        #endregion
    }
}
