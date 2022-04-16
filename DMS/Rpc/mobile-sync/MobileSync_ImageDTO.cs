using DMS.Entities;

namespace DMS.Rpc.mobile_sync
{
    public class MobileSync_ImageDTO
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }


        public MobileSync_ImageDTO() { }
        public MobileSync_ImageDTO(Image Image)
        {
            this.Id = Image.Id;
            this.Name = Image.Name;
            this.Url = Image.Url;
        }
    }
}
