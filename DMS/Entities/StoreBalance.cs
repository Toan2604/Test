using TrueSight.Common;
using System;
using System.Collections.Generic;
using DMS.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DMS.Entities
{
    public class StoreBalance : DataEntity, IEquatable<StoreBalance>
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long StoreId { get; set; }
        public decimal? CreditAmount { get; set; }
        public decimal? DebitAmount { get; set; }
        public Organization Organization { get; set; }
        public Store Store { get; set; }
        public Guid RowId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool Equals(StoreBalance other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.OrganizationId != other.OrganizationId) return false;
            if (this.StoreId != other.StoreId) return false;
            if (this.CreditAmount != other.CreditAmount) return false;
            if (this.DebitAmount != other.DebitAmount) return false;
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class StoreBalanceFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter StoreId { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter CodeDraft { get; set; }
        public StringFilter Name { get; set; }
        public DecimalFilter CreditAmount { get; set; }
        public DecimalFilter DebitAmount { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<StoreBalanceFilter> OrFilter { get; set; }
        public StoreBalanceOrder OrderBy { get; set; }
        public StoreBalanceSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum StoreBalanceOrder
    {
        Id = 0,
        Organization = 1,
        StoreCode = 2,
        CreditAmount = 3,
        DebitAmount = 4,
        StoreName = 5,
        StoreCodeDraft = 6,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum StoreBalanceSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Organization = E._1,
        Store = E._2,
        CreditAmount = E._3,
        DebitAmount = E._4,
        UpdatedAt = E._5,
    }
}
