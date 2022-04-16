using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.store
{
    public class Store_StoreTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? ColorId { get; set; }
        public long StatusId { get; set; }
        public Store_ColorDTO Color { get; set; }

        public Store_StoreTypeDTO() { }
        public Store_StoreTypeDTO(StoreType StoreType)
        {

            this.Id = StoreType.Id;

            this.Code = StoreType.Code;

            this.Name = StoreType.Name;

            this.ColorId = StoreType.ColorId;
            this.StatusId = StoreType.StatusId;
            this.Color = StoreType.Color == null ? null : new Store_ColorDTO(StoreType.Color);

        }
    }

    public class Store_StoreTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter StatusId { get; set; }

        public StoreTypeOrder OrderBy { get; set; }
    }
}