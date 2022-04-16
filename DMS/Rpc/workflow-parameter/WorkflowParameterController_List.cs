using DMS.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.workflow_parameter
{
    public partial class WorkflowParameterController : RpcController
    {
        [Route(WorkflowParameterRoute.FilterListWorkflowParameterType), HttpPost]
        public async Task<List<WorkflowParameter_WorkflowParameterTypeDTO>> FilterListWorkflowParameterType()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowParameterTypeFilter WorkflowParameterTypeFilter = new WorkflowParameterTypeFilter();
            WorkflowParameterTypeFilter.Skip = 0;
            WorkflowParameterTypeFilter.Take = 20;
            WorkflowParameterTypeFilter.OrderBy = WorkflowParameterTypeOrder.Id;
            WorkflowParameterTypeFilter.OrderType = OrderType.ASC;
            WorkflowParameterTypeFilter.Selects = WorkflowParameterTypeSelect.ALL;

            List<WorkflowParameterType> WorkflowParameterTypes = await WorkflowParameterTypeService.List(WorkflowParameterTypeFilter);
            List<WorkflowParameter_WorkflowParameterTypeDTO> WorkflowParameter_WorkflowParameterTypeDTOs = WorkflowParameterTypes
                .Select(x => new WorkflowParameter_WorkflowParameterTypeDTO(x)).ToList();
            return WorkflowParameter_WorkflowParameterTypeDTOs;
        }

        [Route(WorkflowParameterRoute.FilterListWorkflowType), HttpPost]
        public async Task<List<WorkflowParameter_WorkflowTypeDTO>> FilterListWorkflowType()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowTypeFilter WorkflowTypeFilter = new WorkflowTypeFilter();
            WorkflowTypeFilter.Skip = 0;
            WorkflowTypeFilter.Take = 20;
            WorkflowTypeFilter.OrderBy = WorkflowTypeOrder.Id;
            WorkflowTypeFilter.OrderType = OrderType.ASC;
            WorkflowTypeFilter.Selects = WorkflowTypeSelect.ALL;

            List<WorkflowType> WorkflowTypes = await WorkflowTypeService.List(WorkflowTypeFilter);
            List<WorkflowParameter_WorkflowTypeDTO> WorkflowParameter_WorkflowTypeDTOs = WorkflowTypes
                .Select(x => new WorkflowParameter_WorkflowTypeDTO(x)).ToList();
            return WorkflowParameter_WorkflowTypeDTOs;
        }
    }
}

