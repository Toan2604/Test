using System.Collections.Generic;
using System.ComponentModel;

namespace DMS.Rpc.workflow_step
{
    [DisplayName("Bước thực hiện workflow")]
    public class WorkflowStepRoute : Root
    {
        public const string Parent = Module + "/workflow";
        public const string Master = Module + "/workflow/workflow-step/workflow-step-master";
        public const string Detail = Module + "/workflow/workflow-step/workflow-step-detail/*";
        private const string Default = Rpc + Module + "/workflow-step";
        public const string Count = Default + "/count";
        public const string List = Default + "/list";
        public const string Get = Default + "/get";
        public const string Create = Default + "/create";
        public const string Update = Default + "/update";
        public const string Delete = Default + "/delete";
        public const string Import = Default + "/import";
        public const string Export = Default + "/export";
        public const string ExportTemplate = Default + "/export-tempate";
        public const string BulkDelete = Default + "/bulk-delete";

        public const string FilterListAppUser = Default + "/filter-list-app-user";
        public const string FilterListRole = Default + "/filter-list-role";
        public const string FilterListStatus = Default + "/filter-list-status";
        public const string FilterListWorkflowDefinition = Default + "/filter-list-workflow-definition";

        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListRole = Default + "/single-list-role";
        public const string SingleListStatus = Default + "/single-list-status";
        public const string SingleListWorkflowDefinition = Default + "/single-list-workflow-definition";
        public const string SingleListWorkflowParameter = Default + "/single-list-workflow-parameter";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListRole, FilterListWorkflowDefinition, FilterListAppUser, FilterListStatus,} },
            { "Thêm", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListRole, FilterListWorkflowDefinition, FilterListStatus,
                Detail, Create,
                SingleListRole, SingleListWorkflowDefinition, SingleListAppUser, SingleListStatus, SingleListWorkflowParameter} },
            { "Sửa", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListRole, FilterListWorkflowDefinition, FilterListStatus,
                Detail, Update,
                SingleListRole, SingleListWorkflowDefinition, SingleListAppUser, SingleListStatus, SingleListWorkflowParameter} },
            { "Xoá", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListRole, FilterListWorkflowDefinition, FilterListStatus,
                Delete, } },
            { "Xoá nhiều", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListRole, FilterListWorkflowDefinition, FilterListStatus,
                BulkDelete } },
            { "Nhập excel", new List<string> {
                Parent,
                Master, Count, List, Get,
                FilterListRole, FilterListWorkflowDefinition, FilterListStatus,
                ExportTemplate, } },
        };
    }
}
