using DMS.Entities;
using TrueSight.Common;

namespace DMS.Rpc.monitor_store_problems
{
    public class MonitorStoreProblem_ProblemImageMappingDTO : DataDTO
    {
        public long ProblemId { get; set; }
        public long ImageId { get; set; }
        public MonitorStoreProblem_ImageDTO Image { get; set; }

        public MonitorStoreProblem_ProblemImageMappingDTO() { }
        public MonitorStoreProblem_ProblemImageMappingDTO(ProblemImageMapping ProblemImageMapping)
        {
            this.ProblemId = ProblemImageMapping.ProblemId;
            this.ImageId = ProblemImageMapping.ImageId;
            this.Image = ProblemImageMapping.Image == null ? null : new MonitorStoreProblem_ImageDTO(ProblemImageMapping.Image);
            this.Errors = ProblemImageMapping.Errors;
        }
    }

    public class MonitorStoreProblem_ProblemImageMappingFilterDTO : FilterDTO
    {

        public IdFilter ProblemId { get; set; }

        public IdFilter ImageId { get; set; }

        public ProblemImageMappingOrder OrderBy { get; set; }
    }
}