using DMS.ABE.Common;
using TrueSight.Common;
using System.Collections.Generic;
using System.ComponentModel;

namespace DMS.ABE.Rpc.web.lucky_draw
{
    [DisplayName("Web Đại lý Quay thưởng")]
    public class WebLuckyDrawRoute : Root
    {
        private const string Default = Rpc + Module + "/web/lucky-draw";

        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Draw = Default + "/draw";
        public const string GetDrawHistory = Default + "/get-draw-history";
        public const string ListLuckyDrawHistory = Default + "/list-lucky-draw-history";
        public const string CountLuckyDrawHistory = Default + "/count-lucky-draw-history";


        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Count, List, Get, Draw, GetDrawHistory, ListLuckyDrawHistory, CountLuckyDrawHistory
                } },
        };
    }
}
