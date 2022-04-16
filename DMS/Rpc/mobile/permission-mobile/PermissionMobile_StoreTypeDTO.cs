using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_StoreTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long? ColorId { get; set; }
        public long StatusId { get; set; }

        public PermissionMobile_StoreTypeDTO() { }
        public PermissionMobile_StoreTypeDTO(StoreType StoreType)
        {
            this.Id = StoreType.Id;
            this.Code = StoreType.Code;
            this.Name = StoreType.Name;
            this.ColorId = StoreType.ColorId;
            this.StatusId = StoreType.StatusId;
        }
    }

    public class PermissionMobile_StoreTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter ColorId { get; set; }
        public IdFilter StatusId { get; set; }
        public StoreTypeOrder OrderBy { get; set; }
    }
}
