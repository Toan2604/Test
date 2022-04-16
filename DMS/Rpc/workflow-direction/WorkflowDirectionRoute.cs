using System.Collections.Generic;
using System.ComponentModel;

namespace DMS.Rpc.workflow_direction
{
    [DisplayName("Luồng thực hiện workflow")]
    public class WorkflowDirectionRoute : Root
    {
        public const string Parent = Module + "/workflow";
        public const string Master = Module + "/workflow/workflow-direction/workflow-direction-master";
        public const string Detail = Module + "/workflow/workflow-direction/workflow-direction-detail/*";
        private const string Default = Rpc + Module + "/workflow-direction";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Export = Default + "/export";

        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListWorkflowStep = Default + "/filter-list-workflow-step";
        public const string FilterListWorkflowDefinition = Default + "/filter-list-workflow-definition";
        public const string FilterListStatus = Default + "/filter-list-status";

        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListWorkflowStep = Default + "/single-list-workflow-step";
        public const string SingleListWorkflowDefinition = Default + "/single-list-workflow-definition";
        public const string SingleListWorkflowParameter = Default + "/single-list-workflow-parameter";
        public const string SingleListWorkflowOperator = Default + "/single-list-workflow-operator";
        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListOrganization = Default + "/single-list-organization";
        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListWorkflowStep, FilterListWorkflowDefinition, FilterListAppUser, FilterListStatus, } },
            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListWorkflowStep, FilterListWorkflowDefinition, FilterListStatus,
                Detail, Create,
                SingleListWorkflowStep, SingleListWorkflowDefinition, SingleListAppUser, SingleListWorkflowParameter, SingleListWorkflowOperator, SingleListStatus, SingleListOrganization} },
            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListWorkflowStep, FilterListWorkflowDefinition, FilterListStatus,
                Detail, Update,
                SingleListWorkflowStep, SingleListWorkflowDefinition, SingleListAppUser, SingleListWorkflowParameter, SingleListWorkflowOperator, SingleListStatus, SingleListOrganization} },
            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListWorkflowStep, FilterListWorkflowDefinition, FilterListStatus,
                Delete, } },
        };
    }
}
