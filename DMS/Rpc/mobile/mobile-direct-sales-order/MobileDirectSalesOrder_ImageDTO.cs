using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.mobile.direct_sales_order
{
    public class MobileDirectSalesOrder_ImageDTO : DataDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
        public string ThumbnailUrl { get; set; }
        public MobileDirectSalesOrder_ImageDTO() { }
        public MobileDirectSalesOrder_ImageDTO(Image Image)
        {
            this.Id = Image.Id;
            this.Name = Image.Name;
            this.Url = Image.Url;
            this.ThumbnailUrl = Image.ThumbnailUrl;
            this.Errors = Image.Errors;
        }    
    }
}