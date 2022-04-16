using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store_balance
{
    public class StoreBalance_ProvinceDTO : DataDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public long? Priority { get; set; }

        public long StatusId { get; set; }


        public StoreBalance_ProvinceDTO() { }
        public StoreBalance_ProvinceDTO(Province Province)
        {

            this.Id = Province.Id;

            this.Name = Province.Name;

            this.Priority = Province.Priority;

            this.StatusId = Province.StatusId;

        }
    }

    public class StoreBalance_ProvinceFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public LongFilter Priority { get; set; }

        public IdFilter StatusId { get; set; }

        public ProvinceOrder OrderBy { get; set; }
    }
}