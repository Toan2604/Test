using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store_balance
{
    public class StoreBalance_DistrictDTO : DataDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public long? Priority { get; set; }

        public long ProvinceId { get; set; }

        public long StatusId { get; set; }


        public StoreBalance_DistrictDTO() { }
        public StoreBalance_DistrictDTO(District District)
        {

            this.Id = District.Id;

            this.Name = District.Name;

            this.Priority = District.Priority;

            this.ProvinceId = District.ProvinceId;

            this.StatusId = District.StatusId;

        }
    }

    public class StoreBalance_DistrictFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public LongFilter Priority { get; set; }

        public IdFilter ProvinceId { get; set; }

        public IdFilter StatusId { get; set; }

        public DistrictOrder OrderBy { get; set; }
    }
}