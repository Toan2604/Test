using DMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order_with_draw
{
    public class ShowingOrderWithDraw_ShowingOrderWithDrawDTO : DataDTO
    {
        public long STT { get; set; }
        public long Id { get; set; }
        public string Code { get; set; }
        public long AppUserId { get; set; }
        public long OrganizationId { get; set; }
        public long StoreId { get; set; }
        public DateTime Date { get; set; }
        public string eDate { get; set; } // for export excel
        public long ShowingWarehouseId { get; set; }
        public long StatusId { get; set; }
        public decimal Total { get; set; }
        public string eTotal { get; set; } // for export excel
        public Guid RowId { get; set; }
        public ShowingOrderWithDraw_AppUserDTO AppUser { get; set; }
        public ShowingOrderWithDraw_OrganizationDTO Organization { get; set; }
        public ShowingOrderWithDraw_ShowingWarehouseDTO ShowingWarehouse { get; set; }
        public ShowingOrderWithDraw_StatusDTO Status { get; set; }
        public ShowingOrderWithDraw_StoreDTO Store { get; set; }
        public List<ShowingOrderWithDraw_ShowingOrderContentWithDrawDTO> ShowingOrderContentWithDraws { get; set; }
        public List<ShowingOrderWithDraw_StoreDTO> Stores { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ShowingOrderWithDraw_ShowingOrderWithDrawDTO() { }
        public ShowingOrderWithDraw_ShowingOrderWithDrawDTO(ShowingOrderWithDraw ShowingOrderWithDraw)
        {
            this.Id = ShowingOrderWithDraw.Id;
            this.Code = ShowingOrderWithDraw.Code;
            this.AppUserId = ShowingOrderWithDraw.AppUserId;
            this.OrganizationId = ShowingOrderWithDraw.OrganizationId;
            this.StoreId = ShowingOrderWithDraw.StoreId;
            this.Date = ShowingOrderWithDraw.Date;
            this.ShowingWarehouseId = ShowingOrderWithDraw.ShowingWarehouseId;
            this.StatusId = ShowingOrderWithDraw.StatusId;
            this.Total = ShowingOrderWithDraw.Total;
            this.RowId = ShowingOrderWithDraw.RowId;
            this.AppUser = ShowingOrderWithDraw.AppUser == null ? null : new ShowingOrderWithDraw_AppUserDTO(ShowingOrderWithDraw.AppUser);
            this.Organization = ShowingOrderWithDraw.Organization == null ? null : new ShowingOrderWithDraw_OrganizationDTO(ShowingOrderWithDraw.Organization);
            this.ShowingWarehouse = ShowingOrderWithDraw.ShowingWarehouse == null ? null : new ShowingOrderWithDraw_ShowingWarehouseDTO(ShowingOrderWithDraw.ShowingWarehouse);
            this.Status = ShowingOrderWithDraw.Status == null ? null : new ShowingOrderWithDraw_StatusDTO(ShowingOrderWithDraw.Status);
            this.Store = ShowingOrderWithDraw.Store == null ? null : new ShowingOrderWithDraw_StoreDTO(ShowingOrderWithDraw.Store);
            this.ShowingOrderContentWithDraws = ShowingOrderWithDraw.ShowingOrderContentWithDraws?.Select(x => new ShowingOrderWithDraw_ShowingOrderContentWithDrawDTO(x)).ToList();
            this.Stores = ShowingOrderWithDraw.Stores?.Select(x => new ShowingOrderWithDraw_StoreDTO(x)).ToList();
            this.CreatedAt = ShowingOrderWithDraw.CreatedAt;
            this.UpdatedAt = ShowingOrderWithDraw.UpdatedAt;
            this.Errors = ShowingOrderWithDraw.Errors;
        }
    }

    public class ShowingOrderWithDraw_ShowingOrderWithDrawFilterDTO : FilterDTO
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
        public ShowingOrderWithDrawOrder OrderBy { get; set; }
    }
}
