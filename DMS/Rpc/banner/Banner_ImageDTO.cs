using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.banner
{
    public class Banner_ImageDTO : DataDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }


        public Banner_ImageDTO() { }
        public Banner_ImageDTO(Image Image)
        {

            this.Id = Image.Id;

            this.Name = Image.Name;

            this.Url = Image.Url;

            this.Errors = Image.Errors;
        }
    }

    public class Banner_ImageFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Url { get; set; }

        public ImageOrder OrderBy { get; set; }
    }
}