using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Rpc.monitor_store_problems
{
    public class MonitorStoreProblem_ExportDTO : DataDTO
    {
        public string OrganizationName { get; set; }
        public List<MonitorStoreProblem_ProblemDTO> MonitorStoreProblems { get; set; }
    }
}
