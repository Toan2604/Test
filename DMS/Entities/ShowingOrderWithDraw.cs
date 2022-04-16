using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Entities
{
    public class ShowingOrderWithDraw : DataEntity, IEquatable<ShowingOrderWithDraw>
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public long AppUserId { get; set; }
        public long OrganizationId { get; set; }
        public long StoreId { get; set; }
        public DateTime Date { get; set; }
        public long ShowingWarehouseId { get; set; }
        public long StatusId { get; set; }
        public decimal Total { get; set; }
        public Guid RowId { get; set; }
        public AppUser AppUser { get; set; }
        public Organization Organization { get; set; }
        public ShowingWarehouse ShowingWarehouse { get; set; }
        public Status Status { get; set; }
        public Store Store { get; set; }
        public List<ShowingOrderContentWithDraw> ShowingOrderContentWithDraws { get; set; }
        public List<Store> Stores { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public bool Equals(ShowingOrderWithDraw other)
        {
            if (other == null) return false;
            if (this.Id != other.Id) return false;
            if (this.Code != other.Code) return false;
            if (this.AppUserId != other.AppUserId) return false;
            if (this.OrganizationId != other.OrganizationId) return false;
            if (this.StoreId != other.StoreId) return false;
            if (this.Date != other.Date) return false;
            if (this.ShowingWarehouseId != other.ShowingWarehouseId) return false;
            if (this.StatusId != other.StatusId) return false;
            if (this.Total != other.Total) return false;
            if (this.RowId != other.RowId) return false;
            if (this.ShowingOrderContentWithDraws?.Count != other.ShowingOrderContentWithDraws?.Count) return false;
            else if (this.ShowingOrderContentWithDraws != null && other.ShowingOrderContentWithDraws != null)
            {
                for (int i = 0; i < ShowingOrderContentWithDraws.Count; i++)
                {
                    ShowingOrderContentWithDraw ShowingOrderContentWithDraw = ShowingOrderContentWithDraws[i];
                    ShowingOrderContentWithDraw otherShowingOrderWithDrawContent = other.ShowingOrderContentWithDraws[i];
                    if (ShowingOrderContentWithDraw == null && otherShowingOrderWithDrawContent != null)
                        return false;
                    if (ShowingOrderContentWithDraw != null && otherShowingOrderWithDrawContent == null)
                        return false;
                    if (ShowingOrderContentWithDraw.Equals(otherShowingOrderWithDrawContent) == false)
                        return false;
                }
            }
            return true;
        }
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

    public class ShowingOrderWithDrawFilter : FilterEntity
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public IdFilter AppUserId { get; set; }
        public IdFilter OrganizationId { get; set; }
        public IdFilter StoreId { get; set; }
        public IdFilter ShowingItemId { get; set; }
        public DateFilter Date { get; set; }
        public IdFilter ShowingWarehouseId { get; set; }
        public IdFilter StatusId { get; set; }
        public DecimalFilter Total { get; set; }
        public GuidFilter RowId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public List<ShowingOrderWithDrawFilter> OrFilter { get; set; }
        public ShowingOrderWithDrawOrder OrderBy { get; set; }
        public ShowingOrderWithDrawSelect Selects { get; set; }
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShowingOrderWithDrawOrder
    {
        Id = 0,
        Code = 1,
        AppUser = 2,
        Organization = 3,
        Store = 4,
        Date = 5,
        ShowingWarehouse = 6,
        Status = 7,
        Total = 8,
        Row = 12,
        CreatedAt = 50,
        UpdatedAt = 51,
    }

    [Flags]
    public enum ShowingOrderWithDrawSelect : long
    {
        ALL = E.ALL,
        Id = E._0,
        Code = E._1,
        AppUser = E._2,
        Organization = E._3,
        Store = E._4,
        Date = E._5,
        ShowingWarehouse = E._6,
        Status = E._7,
        Total = E._8,
        Row = E._12,
    }
}
