using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;
using System.ComponentModel;

namespace DMS.ABE.Rpc.banner
{
    [DisplayName("Banner thông tin")]
    public class BannerRoute : Root
    {
        private const string Default = Rpc + Module + "/banner";

        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";


        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Count, List, Get
                } },
        };
    }
}
