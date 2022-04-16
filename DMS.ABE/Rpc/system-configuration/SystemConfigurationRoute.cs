using DMS.ABE.Common; using TrueSight.Common;
using System.Collections.Generic;
using System.ComponentModel;

namespace DMS.ABE.Rpc.system_configuration
{
    [DisplayName("Cấu hình hệ thống")]
    public class SystemConfigurationRoute : Root
    {
        public const string Parent = Module + "/system-configuration";
        public const string Master = Module + "/system-configuration/system-configuration-master";
        private const string Default = Rpc + Module + "/system-configuration";
        public const string Get = Default + "/get";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Quản trị", new List<string> {
                Parent,
                Master, Get} },
        };
    }
}
