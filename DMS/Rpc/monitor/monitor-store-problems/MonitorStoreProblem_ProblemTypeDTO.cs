using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.monitor_store_problems
{
    public class MonitorStoreProblem_ProblemTypeDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }


        public MonitorStoreProblem_ProblemTypeDTO() { }
        public MonitorStoreProblem_ProblemTypeDTO(ProblemType ProblemType)
        {

            this.Id = ProblemType.Id;

            this.Code = ProblemType.Code;

            this.Name = ProblemType.Name;

            this.Errors = ProblemType.Errors;
        }
    }

    public class MonitorStoreProblem_ProblemTypeFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public ProblemTypeOrder OrderBy { get; set; }
    }
}