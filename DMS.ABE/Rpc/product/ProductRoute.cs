using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;
using System.ComponentModel;

namespace DMS.ABE.Rpc.product
{
    [DisplayName("Sản phẩm")]
    public class ProductRoute : Root
    {
        private const string Default = Rpc + Module + "/product";
        public const string List = Default + "/list"; // lấy ra list sản phẩm + toàn bộ items phục thuộc, sắp xếp theo thứ tự từ mới -> cũ
        public const string ListPotential = Default + "/list-potential"; // lay ra list san pham co the ban se thich
        public const string Count = Default + "/count";
        public const string Get = Default + "/get";
        //public const string ListNewProduct = Default + "/list-new-product"; // lấy ra list sản phẩm mới, sắp xếp theo thứ tự UpdatedAt
        //public const string CountNewProduct = Default + "/count-new-product"; // lấy ra list sản phẩm mới, sắp xếp theo thứ tự UpdatedAt
        public const string GetItem = Default + "/get-item"; // lấy ra chi tiết sản phẩm + lấy ra một items theo điều kiện lọc (variationGrouping)
        public const string ListItem = Default + "/list-item"; // lấy ra chi tiết sản phẩm + lấy ra một items theo điều kiện lọc (variationGrouping)
        public const string GetItemByVariation = Default + "/get-item-by-variation"; // lấy ra chi tiết sản phẩm + lấy ra một items theo điều kiện lọc (variationGrouping)

        public const string ListCategory = Default + "/list-category";
        public const string CountCategory = Default + "/count-category";
        public const string ListBrand = Default + "/list-brand";
        public const string ListProductGrouping = Default + "/list-product-grouping";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                //SingleListCategory,
                //CountBanner, ListBanner, GetBanner, GetItem, CountProduct, ListProduct, GetProduct,
                //} 
            } },
        };
    }
}
