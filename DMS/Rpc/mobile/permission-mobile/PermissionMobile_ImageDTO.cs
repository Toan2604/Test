using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.mobile.permission_mobile
{
    public class PermissionMobile_ImageDTO : DataDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }

        public PermissionMobile_ImageDTO() { }
        public PermissionMobile_ImageDTO(Image Image)
        {

            this.Id = Image.Id;

            this.Name = Image.Name;

            this.Url = Image.Url;
            this.ThumbnailUrl = Image.ThumbnailUrl;
            this.Errors = Image.Errors;
        }
    }
}