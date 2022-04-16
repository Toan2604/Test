using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.posm.showing_category
{
    [DisplayName("Danh mục sản phẩm trưng bày")]
    public class ShowingCategoryRoute : Root
    {
        public const string Parent = Module + "/posm";
        public const string Master = Module + "/posm/showing-category/showing-category-master";
        public const string Detail = Module + "/posm/showing-category/showing-category-detail/*";
        public const string Preview = Module + "/showing-category/showing-category-preview";
        private const string Default = Rpc + Module + "/showing-category";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string SaveImage = Default + "/save-image";

        public const string FilterListShowingCategory = Default + "/filter-list-showing-category";
        public const string FilterListStatus = Default + "/filter-list-status";

        public const string SingleListShowingCategory = Default + "/single-list-showing-category";
        public const string SingleListStatus = Default + "/single-list-status";


        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(ShowingCategoryFilter.Id), FieldTypeEnum.ID.Id },
            { nameof(ShowingCategoryFilter.Code), FieldTypeEnum.STRING.Id },
            { nameof(ShowingCategoryFilter.Name), FieldTypeEnum.STRING.Id },
            { nameof(ShowingCategoryFilter.ParentId), FieldTypeEnum.ID.Id },
            { nameof(ShowingCategoryFilter.Path), FieldTypeEnum.STRING.Id },
            { nameof(ShowingCategoryFilter.Level), FieldTypeEnum.LONG.Id },
            { nameof(ShowingCategoryFilter.StatusId), FieldTypeEnum.ID.Id },
            { nameof(ShowingCategoryFilter.ImageId), FieldTypeEnum.ID.Id },
            { nameof(ShowingCategoryFilter.RowId), FieldTypeEnum.ID.Id },
        };

        private static List<string> FilterList = new List<string> {
            FilterListShowingCategory,FilterListStatus,
        };
        private static List<string> SingleList = new List<string> {
            SingleListShowingCategory, SingleListStatus,
        };
        private static List<string> CountList = new List<string>
        {

        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                    Parent,
                    Master, Preview, Count, List,
                    Get,
                }.Concat(FilterList)
            },
            { "Thêm", new List<string> {
                    Parent,
                    Master, Preview, Count, List, Get,
                    Detail, Create, SaveImage,
                }.Concat(SingleList).Concat(FilterList).Concat(CountList)
            },

            { "Sửa", new List<string> {
                    Parent,
                    Master, Preview, Count, List, Get,
                    Detail, Update, SaveImage,
                }.Concat(SingleList).Concat(FilterList).Concat(CountList)
            },

            { "Xoá", new List<string> {
                    Parent,
                    Master, Preview, Count, List, Get,
                    Delete,
                }.Concat(SingleList).Concat(FilterList)
            },
        };
    }
}
