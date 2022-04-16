using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_order_with_draw
{
    public class ShowingOrderWithDraw_StoreTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? ColorId { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public ShowingOrderWithDraw_ColorDTO Color { get; set; }
        public ShowingOrderWithDraw_StatusDTO Status { get; set; }
        public ShowingOrderWithDraw_StoreTypeDTO() { }
        public ShowingOrderWithDraw_StoreTypeDTO(StoreType StoreType)
        {
            this.Id = StoreType.Id;
            this.Code = StoreType.Code;
            this.Name = StoreType.Name;
            this.ColorId = StoreType.ColorId;
            this.StatusId = StoreType.StatusId;
            this.Used = StoreType.Used;
            this.Color = StoreType.Color == null ? null : new ShowingOrderWithDraw_ColorDTO(StoreType.Color);
            this.Status = StoreType.Status == null ? null : new ShowingOrderWithDraw_StatusDTO(StoreType.Status);
            this.Errors = StoreType.Errors;
        }
    }

    public class ShowingOrderWithDraw_StoreTypeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public StoreTypeOrder OrderBy { get; set; }
    }
}
