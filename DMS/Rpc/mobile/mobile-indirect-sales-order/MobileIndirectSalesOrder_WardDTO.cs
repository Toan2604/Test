using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.indirect_sales_order
{
    public class MobileIndirectSalesOrder_WardDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long DistrictId { get; set; }
        public long StatusId { get; set; }
        public MobileIndirectSalesOrder_WardDTO() { }
        public MobileIndirectSalesOrder_WardDTO(Ward Ward)
        {
            this.Id = Ward.Id;
            this.Code = Ward.Code;
            this.Name = Ward.Name;
            this.Priority = Ward.Priority;
            this.DistrictId = Ward.DistrictId;
            this.StatusId = Ward.StatusId;
            this.Errors = Ward.Errors;
        }
    }

    public class MobileIndirectSalesOrder_WardFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter StatusId { get; set; }
        public WardOrder OrderBy { get; set; }
    }
}
