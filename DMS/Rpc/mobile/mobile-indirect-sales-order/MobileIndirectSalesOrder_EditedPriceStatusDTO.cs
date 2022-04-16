using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_EditedPriceStatusDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public MobileIndirectSalesOrder_EditedPriceStatusDTO() { }
        public MobileIndirectSalesOrder_EditedPriceStatusDTO(EditedPriceStatus EditedPriceStatus)
        {
            this.Id = EditedPriceStatus.Id;
            this.Code = EditedPriceStatus.Code;
            this.Name = EditedPriceStatus.Name;
            this.Errors = EditedPriceStatus.Errors;
        }
    }

    public class MobileIndirectSalesOrder_EditedPriceStatusFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public EditedPriceStatusOrder OrderBy { get; set; }
    }
}
