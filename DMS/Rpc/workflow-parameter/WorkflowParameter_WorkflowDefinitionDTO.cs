using DMS.Entities;
using System;
using TrueSight.Common;

namespace DMS.Rpc.workflow_parameter
{
    public class WorkflowParameter_WorkflowDefinitionDTO : DataDTO
    {

        public long Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public long CreatorId { get; set; }

        public long? ModifierId { get; set; }

        public long WorkflowTypeId { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public long StatusId { get; set; }


        public WorkflowParameter_WorkflowDefinitionDTO() { }
        public WorkflowParameter_WorkflowDefinitionDTO(WorkflowDefinition WorkflowDefinition)
        {

            this.Id = WorkflowDefinition.Id;

            this.Code = WorkflowDefinition.Code;

            this.Name = WorkflowDefinition.Name;

            this.CreatorId = WorkflowDefinition.CreatorId;

            this.ModifierId = WorkflowDefinition.ModifierId;

            this.WorkflowTypeId = WorkflowDefinition.WorkflowTypeId;

            this.StartDate = WorkflowDefinition.StartDate;

            this.EndDate = WorkflowDefinition.EndDate;

            this.StatusId = WorkflowDefinition.StatusId;

            this.Errors = WorkflowDefinition.Errors;
        }
    }

    public class WorkflowParameter_WorkflowDefinitionFilterDTO : FilterDTO
    {

        public IdFilter Id { get; set; }

        public StringFilter Code { get; set; }

        public StringFilter Name { get; set; }

        public IdFilter CreatorId { get; set; }

        public IdFilter ModifierId { get; set; }

        public IdFilter WorkflowTypeId { get; set; }

        public DateFilter StartDate { get; set; }

        public DateFilter EndDate { get; set; }

        public IdFilter StatusId { get; set; }

        public WorkflowDefinitionOrder OrderBy { get; set; }
    }
}