using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;

namespace DMS.ABE.Rpc.direct_sales_order
{
    public class DirectSalesOrder_ImageDTO : DataDTO
    {

        public long Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }


        public DirectSalesOrder_ImageDTO() { }
        public DirectSalesOrder_ImageDTO(Image Image)
        {

            this.Id = Image.Id;

            this.Name = Image.Name;

            this.Url = Image.Url;
            this.ThumbnailUrl = Image.ThumbnailUrl;

        }
    }

    public class DirectSalesOrder_ImageFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Name { get; set; }

        public StringFilter Url { get; set; }
        public StringFilter ThumbnailUrl { get; set; }

        public ImageOrder OrderBy { get; set; }
    }
}