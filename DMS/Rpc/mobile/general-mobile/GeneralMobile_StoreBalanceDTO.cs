using TrueSight.Common;
using DMS.Common;
using System;
using System.Linq;
using System.Collections.Generic;
using DMS.Entities;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreBalanceDTO : DataDTO
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long StoreId { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? DebitAmount { get; set; }
        public decimal BalanceAmount { get; set; }
        public GeneralMobile_OrganizationDTO Organization { get; set; }
        public GeneralMobile_StoreDTO Store { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public GeneralMobile_StoreBalanceDTO() { }
        public GeneralMobile_StoreBalanceDTO(StoreBalance StoreBalance)
        {
            this.Id = StoreBalance.Id;
            this.OrganizationId = StoreBalance.OrganizationId;
            this.StoreId = StoreBalance.StoreId;
            this.CreditAmount = StoreBalance.CreditAmount;
            this.DebitAmount = StoreBalance.DebitAmount;
            this.Organization = StoreBalance.Organization == null ? null : new GeneralMobile_OrganizationDTO(StoreBalance.Organization);
            this.Store = StoreBalance.Store == null ? null : new GeneralMobile_StoreDTO(StoreBalance.Store);
            this.RowId = StoreBalance.RowId;
            this.CreatedAt = StoreBalance.CreatedAt;
            this.UpdatedAt = StoreBalance.UpdatedAt;
            this.Informations = StoreBalance.Informations;
            this.Warnings = StoreBalance.Warnings;
            this.Errors = StoreBalance.Errors;
        }
    }

    public class GeneralMobile_StoreBalanceFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter StoreId { get; set; }
        public DecimalFilter CreditAmount { get; set; }
        public DecimalFilter DebitAmount { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public StoreBalanceOrder OrderBy { get; set; }
    }
}
