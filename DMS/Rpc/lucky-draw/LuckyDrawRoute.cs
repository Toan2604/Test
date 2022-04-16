using TrueSight.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DMS.Common;
using DMS.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using OfficeOpenXml;
using DMS.Entities;
using DMS.Services.MLuckyDraw;
using DMS.Services.MImage;
using DMS.Services.MLuckyDrawType;
using DMS.Services.MOrganization;
using DMS.Services.MStatus;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStore;
using DMS.Services.MStoreType;
using DMS.Services.MLuckyDrawStructure;
using DMS.Services.MLuckyDrawWinner;
using DMS.Services.MAppUser;
using System.ComponentModel;

namespace DMS.Rpc.lucky_draw
{
    [DisplayName("Chương trình quay thưởng")]
    public class LuckyDrawRoute : Root
    {
        public const string Parent = Module + "/price-list-and-promotion";
        public const string Master = Module + "/price-list-and-promotion/lucky-draw/lucky-draw-master";
        public const string Detail = Module + "/price-list-and-promotion/lucky-draw/lucky-draw-detail/*";
        public const string Preview = Module + "/price-list-and-promotion/lucky-draw/lucky-draw-preview";
        public const string Mobile = Module + ".lucky-draw.*";

        public const string MasterABE = WebABEModule + "/lucky-draw/lucky-draw-master";
        public const string MobileABE = MobileABEModule + ".lucky-draw.*";

        private const string Default = Rpc + Module + "/lucky-draw";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string ExportTemplate = Default + "/export-template";
        public const string BulkDelete = Default + "/bulk-delete";
        public const string LoadImage = Default + "/load-image";
        public const string ImportStore = Default + "/import-store";
        public const string ExportStore = Default + "/export-store";
        public const string ExportTemplateStore = Default + "/export-template-store";
        public const string ExportWinnerStore = Default + "/export-winner-store";

        public const string FilterListImage = Default + "/filter-list-image";
        public const string FilterListLuckyDrawType = Default + "/filter-list-lucky-draw-type";
        public const string FilterListOrganization = Default + "/filter-list-organization";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListStoreGrouping = Default + "/filter-list-store-grouping";
        public const string FilterListStore = Default + "/filter-list-store";
        public const string FilterListStoreType = Default + "/filter-list-store-type";
        public const string FilterListLuckyDrawStructure = Default + "/filter-list-lucky-draw-structure";
        public const string FilterListLuckyDrawWinner = Default + "/filter-list-lucky-draw-winner";
        public const string FilterListAppUser = Default + "/filter-list-app-user";

        public const string SingleListImage = Default + "/single-list-image";
        public const string SingleListLuckyDrawType = Default + "/single-list-lucky-draw-type";
        public const string SingleListOrganization = Default + "/single-list-organization";
        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListStoreGrouping = Default + "/single-list-store-grouping";
        public const string SingleListStore = Default + "/single-list-store";
        public const string SingleListStoreType = Default + "/single-list-store-type";
        public const string SingleListLuckyDrawStructure = Default + "/single-list-lucky-draw-structure";
        public const string SingleListLuckyDrawWinner = Default + "/single-list-lucky-draw-winner";
        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListProvince = Default + "/single-list-province";

        public const string CountStoreGrouping = Default + "/count-store-grouping";
        public const string ListStoreGrouping = Default + "/list-store-grouping";
        public const string CountStore = Default + "/count-store";
        public const string ListStore = Default + "/list-store";
        public const string CountStoreType = Default + "/count-store-type";
        public const string ListStoreType = Default + "/list-store-type";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(LuckyDrawFilter.Id), FieldTypeEnum.ID.Id },
            { nameof(LuckyDrawFilter.Code), FieldTypeEnum.STRING.Id },
            { nameof(LuckyDrawFilter.Name), FieldTypeEnum.STRING.Id },
            { nameof(LuckyDrawFilter.LuckyDrawTypeId), FieldTypeEnum.ID.Id },
            { nameof(LuckyDrawFilter.OrganizationId), FieldTypeEnum.ID.Id },
            { nameof(LuckyDrawFilter.RevenuePerTurn), FieldTypeEnum.DECIMAL.Id },
            { nameof(LuckyDrawFilter.StartAt), FieldTypeEnum.DATE.Id },
            { nameof(LuckyDrawFilter.EndAt), FieldTypeEnum.DATE.Id },
            { nameof(LuckyDrawFilter.ImageId), FieldTypeEnum.ID.Id },
            { nameof(LuckyDrawFilter.StatusId), FieldTypeEnum.ID.Id },
        };

        private static List<string> FilterList = new List<string> { 
            FilterListImage,FilterListLuckyDrawType,FilterListOrganization,FilterListStatus,FilterListStoreGrouping,FilterListStore,FilterListStoreType,FilterListLuckyDrawStructure,FilterListLuckyDrawWinner,FilterListAppUser,
        };
        private static List<string> SingleList = new List<string> { 
            SingleListImage, SingleListLuckyDrawType, SingleListOrganization, SingleListStatus, SingleListStoreGrouping, SingleListStore, SingleListStoreType, SingleListLuckyDrawStructure, SingleListLuckyDrawWinner, SingleListAppUser, SingleListProvince,
        };
        private static List<string> CountList = new List<string> { 
            CountStoreGrouping, ListStoreGrouping, CountStore, ListStore, CountStoreType, ListStoreType, 
        };
        
        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> { 
                    Parent,
                    Master, Preview, Count, List, 
                    Get,  LoadImage,
                }.Concat(FilterList)
            },
            { "Thêm", new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get, LoadImage,
                    Detail, Create, 
                }.Concat(SingleList).Concat(FilterList).Concat(CountList)
            },

            { "Sửa", new List<string> { 
                    Parent,            
                    Master, Preview, Count, List, Get, LoadImage,
                    Detail, Update, 
                }.Concat(SingleList).Concat(FilterList).Concat(CountList)
            },

            { "Xoá", new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get, LoadImage,
                    Delete, 
                }.Concat(SingleList).Concat(FilterList) 
            },

            { "Xoá nhiều", new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get, LoadImage,
                    BulkDelete
                }.Concat(FilterList) 
            },

            { "Xuất excel", new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get, LoadImage,
                    Export, ExportStore, ExportWinnerStore,
                }.Concat(FilterList) 
            },

            { "Nhập excel", new List<string> { 
                    Parent,
                    Master, Preview, Count, List, Get, LoadImage,
                    ExportTemplate, Import, ExportTemplateStore, ImportStore,
                }.Concat(FilterList) 
            },
        };
    }
}
