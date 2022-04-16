using System.Collections.Generic;
using System.ComponentModel;

namespace DMS.ABE.Rpc.store_balance
{
    [DisplayName("Công nợ đại lý")]
    public class StoreBalanceRoute : Root
    {
        private const string Default = Rpc + Module + "/store-balance";
        public const string Get = Default + "/get";
        public const string List = Default + "/list";


        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Get, List
                } },
        };
    }
}
