using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.e_route
{
    public class ERoute_StoreTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long StatusId { get; set; }


        public ERoute_StoreTypeDTO() { }
        public ERoute_StoreTypeDTO(StoreType StoreType)
        {

            this.Id = StoreType.Id;

            this.Code = StoreType.Code;

            this.Name = StoreType.Name;

            this.StatusId = StoreType.StatusId;

        }
    }

    public class ERoute_StoreTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter StatusId { get; set; }

        public StoreTypeOrder OrderBy { get; set; }
    }
}
