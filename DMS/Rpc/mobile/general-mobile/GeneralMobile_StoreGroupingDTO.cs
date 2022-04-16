using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobile_StoreGroupingDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? ParentId { get; set; }

        public string Path { get; set; }

        public long Level { get; set; }

        public long StatusId { get; set; }


        public GeneralMobile_StoreGroupingDTO() { }
        public GeneralMobile_StoreGroupingDTO(StoreGrouping StoreGrouping)
        {

            this.Id = StoreGrouping.Id;

            this.Code = StoreGrouping.Code;

            this.Name = StoreGrouping.Name;

            this.ParentId = StoreGrouping.ParentId;

            this.Path = StoreGrouping.Path;

            this.Level = StoreGrouping.Level;
            this.StatusId = StoreGrouping.StatusId;

        }
    }

    public class GeneralMobile_StoreGroupingFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter ParentId { get; set; }

        public StringFilter Path { get; set; }

        public LongFilter Level { get; set; }
        public IdFilter StatusId { get; set; }

        public StoreGroupingOrder OrderBy { get; set; }
    }
}
