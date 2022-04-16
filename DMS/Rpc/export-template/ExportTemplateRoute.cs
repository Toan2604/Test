using System.Collections.Generic;
using System.Linq;

namespace DMS.Rpc.export_template
{
    public class ExportTemplateRoute : Root
    {
        public const string Parent = Module + "/export";
        public const string Master = Module + "/export/export-template/export-template-master";
        public const string Detail = Module + "/export/export-template/export-template-detail/*";
        public const string Preview = Module + "/export-template/export-template-preview";
        private const string Default = Rpc + Module + "/export-template";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string GetExample = Default + "/get-example";
        public const string Update = Default + "/update";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {

        };

        private static List<string> FilterList = new List<string>
        {
        };
        private static List<string> SingleList = new List<string>
        {
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

            { "Sửa", new List<string> {
                    Parent,
                    Master, Preview, Count, List, Get, GetExample,
                    Detail, Update,
                }.Concat(SingleList).Concat(FilterList).Concat(CountList)
            },

        };
    }
}
