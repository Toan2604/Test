using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.promotion
{
    public class Promotion_ImageDTO : DataDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }


        public Promotion_ImageDTO() { }
        public Promotion_ImageDTO(Image Image)
        {

            this.Id = Image.Id;

            this.Name = Image.Name;

            this.Url = Image.Url;
            this.ThumbnailUrl = Image.ThumbnailUrl;

        }
    }

    public class Promotion_ImageFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Url { get; set; }
        public StringFilter ThumbnailUrl { get; set; }

        public ImageOrder OrderBy { get; set; }
    }
}
