using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order
{
    public class ShowingOrder_ShowingOrderDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public long AppUserId { get; set; }
        public long OrganizationId { get; set; }
        public long StoreId { get; set; }
        public DateTime Date { get; set; }
        public string eDate { get; set; }
        public long ShowingWarehouseId { get; set; }
        public long StatusId { get; set; }
        public decimal Total { get; set; }
        public string eTotal { get; set; }
        public Guid RowId { get; set; }
        public ShowingOrder_AppUserDTO AppUser { get; set; }
        public ShowingOrder_OrganizationDTO Organization { get; set; }
        public ShowingOrder_ShowingWarehouseDTO ShowingWarehouse { get; set; }
        public ShowingOrder_StatusDTO Status { get; set; }
        public ShowingOrder_StoreDTO Store { get; set; }
        public List<ShowingOrder_ShowingOrderContentDTO> ShowingOrderContents { get; set; }
        public List<ShowingOrder_StoreDTO> Stores { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ShowingOrder_ShowingOrderDTO() { }
        public ShowingOrder_ShowingOrderDTO(ShowingOrder ShowingOrder)
        {
            this.Id = ShowingOrder.Id;
            this.Code = ShowingOrder.Code;
            this.AppUserId = ShowingOrder.AppUserId;
            this.OrganizationId = ShowingOrder.OrganizationId;
            this.StoreId = ShowingOrder.StoreId;
            this.Date = ShowingOrder.Date;
            this.ShowingWarehouseId = ShowingOrder.ShowingWarehouseId;
            this.StatusId = ShowingOrder.StatusId;
            this.Total = ShowingOrder.Total;
            this.RowId = ShowingOrder.RowId;
            this.AppUser = ShowingOrder.AppUser == null ? null : new ShowingOrder_AppUserDTO(ShowingOrder.AppUser);
            this.Organization = ShowingOrder.Organization == null ? null : new ShowingOrder_OrganizationDTO(ShowingOrder.Organization);
            this.ShowingWarehouse = ShowingOrder.ShowingWarehouse == null ? null : new ShowingOrder_ShowingWarehouseDTO(ShowingOrder.ShowingWarehouse);
            this.Status = ShowingOrder.Status == null ? null : new ShowingOrder_StatusDTO(ShowingOrder.Status);
            this.Store = ShowingOrder.Store == null ? null : new ShowingOrder_StoreDTO(ShowingOrder.Store);
            this.ShowingOrderContents = ShowingOrder.ShowingOrderContents?.Select(x => new ShowingOrder_ShowingOrderContentDTO(x)).ToList();
            this.Stores = ShowingOrder.Stores == null ? null : ShowingOrder.Stores.Select(x => new ShowingOrder_StoreDTO(x)).ToList();
            this.CreatedAt = ShowingOrder.CreatedAt;
            this.UpdatedAt = ShowingOrder.UpdatedAt;
            this.Errors = ShowingOrder.Errors;
        }
    }

    public class ShowingOrder_ShowingOrderFilterDTO : FilterDTO
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
        public ShowingOrderOrder OrderBy { get; set; }
    }
}
