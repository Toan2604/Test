using DMS.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using TrueSight.Common;

namespace DMS.Rpc.workflow_parameter
{
    [DisplayName("Workflow parameter")]
    public class WorkflowParameterRoute : Root
    {
        public const string Parent = Module + "/workflow";
        public const string Master = Module + "/workflow/workflow-parameter/workflow-parameter-master";
        public const string Detail = Module + "/workflow/workflow-parameter/workflow-parameter-detail";
        private const string Default = Rpc + Module + "/workflow-parameter";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";

        public const string FilterListWorkflowParameterType = Default + "/filter-list-workflow-parameter-type";
        public const string FilterListWorkflowType = Default + "/filter-list-workflow-type";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
            { nameof(WorkflowParameterFilter.Id), FieldTypeEnum.ID.Id },
            { nameof(WorkflowParameterFilter.Code), FieldTypeEnum.STRING.Id },
            { nameof(WorkflowParameterFilter.Name), FieldTypeEnum.STRING.Id },
            { nameof(WorkflowParameterFilter.WorkflowTypeId), FieldTypeEnum.ID.Id },
            { nameof(WorkflowParameterFilter.WorkflowParameterTypeId), FieldTypeEnum.ID.Id },
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List,
                FilterListWorkflowParameterType, FilterListWorkflowType, } },
        };
    }
}
