using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.monitor_store_problems
{
    public class MonitorStoreProblem_ProblemStatusDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public MonitorStoreProblem_ProblemStatusDTO() { }
        public MonitorStoreProblem_ProblemStatusDTO(ProblemStatus ProblemStatus)
        {

            this.Id = ProblemStatus.Id;

            this.Code = ProblemStatus.Code;

            this.Name = ProblemStatus.Name;

            this.Errors = ProblemStatus.Errors;
        }
    }

    public class MonitorStoreProblem_ProblemStatusFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public ProblemStatusOrder OrderBy { get; set; }
    }
}