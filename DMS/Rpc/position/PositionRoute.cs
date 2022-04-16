using System.Collections.Generic;

namespace DMS.Rpc.position
{
    public class PositionRoute : Root
    {
        public const string Parent = Module + "/account";
        public const string Master = Module + "/account/position/position-master";
        public const string Detail = Module + "/account/position/position-detail/*";
        private const string Default = Rpc + Module + "/position";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";

        public const string FilterListStatus = Default + "/filter-list-status";
        public const string SingleListStatus = Default + "/single-list-status";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListStatus, SingleListStatus, } },
        };
    }
}
