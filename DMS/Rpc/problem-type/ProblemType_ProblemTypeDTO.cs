using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.problem_type
{
    public class ProblemType_ProblemTypeDTO : DataDTO
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public long StatusId { get; set; }
        public bool Used { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ProblemType_ProblemTypeDTO() { }
        public ProblemType_ProblemTypeDTO(ProblemType ProblemType)
        {
            this.Id = ProblemType.Id;
            this.Code = ProblemType.Code;
            this.Name = ProblemType.Name;
            this.StatusId = ProblemType.StatusId;
            this.CreatedAt = ProblemType.CreatedAt;
            this.UpdatedAt = ProblemType.UpdatedAt;
            this.Used = ProblemType.Used;
            this.Errors = ProblemType.Errors;
        }
    }

    public class ProblemType_ProblemTypeFilterDTO : FilterDTO
    {
        public IdFilter Id { get; set; }
        public StringFilter Code { get; set; }
        public StringFilter Name { get; set; }
        public IdFilter StatusId { get; set; }
        public DateFilter CreatedAt { get; set; }
        public DateFilter UpdatedAt { get; set; }
        public ProblemTypeOrder OrderBy { get; set; }
    }
}
