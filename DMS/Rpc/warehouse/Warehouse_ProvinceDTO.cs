using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.warehouse
{
    public class Warehouse_ProvinceDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? Priority { get; set; }

        public long StatusId { get; set; }


        public Warehouse_ProvinceDTO() { }
        public Warehouse_ProvinceDTO(Province Province)
        {

            this.Id = Province.Id;

            this.Code = Province.Code;

            this.Name = Province.Name;

            this.Priority = Province.Priority;

            this.StatusId = Province.StatusId;

            this.Errors = Province.Errors;
        }
    }

    public class Warehouse_ProvinceFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public LongFilter Priority { get; set; }

        public IdFilter StatusId { get; set; }

        public IdFilter RowId { get; set; }

        public ProvinceOrder OrderBy { get; set; }
    }
}