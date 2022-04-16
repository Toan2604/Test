using System.Collections.Generic;
using System.ComponentModel;

namespace DMS.Rpc.workflow_definition
{
    [DisplayName("Workflow")]
    public class WorkflowDefinitionRoute : Root
    {
        public const string Parent = Module + "/workflow";
        public const string Master = Module + "/workflow/workflow-definition/workflow-definition-master";
        public const string Detail = Module + "/workflow/workflow-definition/workflow-definition-detail/*";
        public const string Preview = Module + "/workflow/workflow-definition/workflow-definition-preview/*";
        private const string Default = Rpc + Module + "/workflow-definition";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string GetDirection = Default + "/get-direction";
        public const string Check = Default + "/check";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Clone = Default + "/clone";
        public const string Delete = Default + "/delete";
        public const string Export = Default + "/export";

        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListWorkflowType = Default + "/filter-list-workflow-type";
        public const string FilterListWorkflowDirection = Default + "/filter-list-workflow-direction";
        public const string FilterListWorkflowStep = Default + "/filter-list-workflow-step";
        public const string FilterListWorkflowParameter = Default + "/filter-list-workflow-parameter";
        public const string FilterListRole = Default + "/filter-list-role";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListOrganization = Default + "/filter-list-organization";

        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListWorkflowType = Default + "/single-list-workflow-type";
        public const string SingleListWorkflowDirection = Default + "/single-list-workflow-direction";
        public const string SingleListWorkflowStep = Default + "/single-list-workflow-step";
        public const string SingleListWorkflowParameter = Default + "/single-list-workflow-parameter";
        public const string SingleListRole = Default + "/single-list-role";
        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListOrganization = Default + "/single-list-organization";

        public const string CountDirection = Default + "/count-direction";
        public const string ListDirection = Default + "/list-direction";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };
        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get, Preview, GetDirection,
                FilterListAppUser, FilterListWorkflowType, FilterListWorkflowDirection, FilterListWorkflowStep, FilterListWorkflowParameter,
                FilterListRole, FilterListStatus, FilterListOrganization } },
            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListWorkflowType, FilterListWorkflowDirection, FilterListWorkflowStep, FilterListWorkflowParameter,
                FilterListRole, FilterListStatus, FilterListOrganization,
                Detail, Create, Check,
                SingleListAppUser, SingleListWorkflowType, SingleListWorkflowDirection, SingleListWorkflowStep, SingleListWorkflowParameter,
                SingleListRole, SingleListStatus, SingleListOrganization } },
            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListWorkflowType, FilterListWorkflowDirection, FilterListWorkflowStep, FilterListWorkflowParameter,
                FilterListRole, FilterListStatus, FilterListOrganization,
                Detail, Update, Clone, Check,
                SingleListAppUser, SingleListWorkflowType, SingleListWorkflowDirection, SingleListWorkflowStep, SingleListWorkflowParameter,
                SingleListRole, SingleListStatus, SingleListOrganization } },
            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListAppUser, FilterListWorkflowType, FilterListWorkflowDirection, FilterListWorkflowStep, FilterListWorkflowParameter,
                FilterListRole, FilterListStatus, FilterListOrganization,
                Detail, Delete,
                SingleListAppUser, SingleListWorkflowType, SingleListWorkflowDirection, SingleListWorkflowStep, SingleListWorkflowParameter,
                SingleListRole, SingleListStatus, SingleListOrganization } },
        };
    }
}
