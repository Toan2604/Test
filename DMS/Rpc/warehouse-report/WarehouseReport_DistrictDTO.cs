using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.warehouse_report
{
    public class WarehouseReport_DistrictDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? Priority { get; set; }

        public long ProvinceId { get; set; }

        public long StatusId { get; set; }


        public WarehouseReport_DistrictDTO() { }
        public WarehouseReport_DistrictDTO(District District)
        {

            this.Id = District.Id;

            this.Code = District.Code;

            this.Name = District.Name;

            this.Priority = District.Priority;

            this.ProvinceId = District.ProvinceId;

            this.StatusId = District.StatusId;

            this.Errors = District.Errors;
        }
    }

    public class WarehouseReport_DistrictFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public LongFilter Priority { get; set; }

        public IdFilter ProvinceId { get; set; }

        public IdFilter StatusId { get; set; }

        public IdFilter RowId { get; set; }

        public DistrictOrder OrderBy { get; set; }
    }
}
