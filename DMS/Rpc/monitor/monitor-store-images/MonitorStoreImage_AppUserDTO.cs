using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.monitor.monitor_store_images
{
    public class MonitorStoreImage_AppUserDTO : DataDTO
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string DisplayName { get; set; }

        public MonitorStoreImage_AppUserDTO() { }
        public MonitorStoreImage_AppUserDTO(AppUser AppUser)
        {
            this.Id = AppUser.Id;
            this.Username = AppUser.Username;
            this.DisplayName = AppUser.DisplayName;
            this.Errors = AppUser.Errors;
        }
    }

    public class MonitorStoreImage_AppUserFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Username { get; set; }
        public StringFilter DisplayName { get; set; }
        public IdFilter OrganizationId { get; set; }
        public AppUserOrder OrderBy { get; set; }
    }
}
