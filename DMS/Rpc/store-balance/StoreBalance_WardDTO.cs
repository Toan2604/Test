using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store_balance
{
    public class StoreBalance_WardDTO : DataDTO
    {

        public long Id { get; set; }
        public string Name { get; set; }
        public long? Priority { get; set; }
        public long DistrictId { get; set; }
        public long StatusId { get; set; }
        public StoreBalance_WardDTO() { }
        public StoreBalance_WardDTO(Ward Ward)
        {

            this.Id = Ward.Id;
            this.Name = Ward.Name;
            this.Priority = Ward.Priority;
            this.DistrictId = Ward.DistrictId;
            this.StatusId = Ward.StatusId;
        }
    }

    public class StoreBalance_WardFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }
        public StringFilter Name { get; set; }
        public LongFilter Priority { get; set; }
        public IdFilter DistrictId { get; set; }
        public IdFilter StatusId { get; set; }
        public WardOrder OrderBy { get; set; }
    }
}