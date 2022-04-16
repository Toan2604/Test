using DMS.Common;
using DMS.Entities;
using DMS.Services.MWorkflow;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.workflow_parameter
{
    public partial class WorkflowParameterController : RpcController
    {
        private IWorkflowParameterTypeService WorkflowParameterTypeService;
        private IWorkflowTypeService WorkflowTypeService;
        private IWorkflowParameterService WorkflowParameterService;
        private ICurrentContext CurrentContext;
        public WorkflowParameterController(
            IWorkflowParameterTypeService WorkflowParameterTypeService,
            IWorkflowTypeService WorkflowTypeService,
            IWorkflowParameterService WorkflowParameterService,
            ICurrentContext CurrentContext
        )
        {
            this.WorkflowParameterTypeService = WorkflowParameterTypeService;
            this.WorkflowTypeService = WorkflowTypeService;
            this.WorkflowParameterService = WorkflowParameterService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkflowParameterRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] WorkflowParameter_WorkflowParameterFilterDTO WorkflowParameter_WorkflowParameterFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowParameterFilter WorkflowParameterFilter = ConvertFilterDTOToFilterEntity(WorkflowParameter_WorkflowParameterFilterDTO);
            int count = await WorkflowParameterService.Count(WorkflowParameterFilter);
            return count;
        }

        [Route(WorkflowParameterRoute.List), HttpPost]
        public async Task<ActionResult<List<WorkflowParameter_WorkflowParameterDTO>>> List([FromBody] WorkflowParameter_WorkflowParameterFilterDTO WorkflowParameter_WorkflowParameterFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowParameterFilter WorkflowParameterFilter = ConvertFilterDTOToFilterEntity(WorkflowParameter_WorkflowParameterFilterDTO);
            List<WorkflowParameter> WorkflowParameters = await WorkflowParameterService.List(WorkflowParameterFilter);
            List<WorkflowParameter_WorkflowParameterDTO> WorkflowParameter_WorkflowParameterDTOs = WorkflowParameters
                .Select(c => new WorkflowParameter_WorkflowParameterDTO(c)).ToList();
            return WorkflowParameter_WorkflowParameterDTOs;
        }


        private WorkflowParameterFilter ConvertFilterDTOToFilterEntity(WorkflowParameter_WorkflowParameterFilterDTO WorkflowParameter_WorkflowParameterFilterDTO)
        {
            WorkflowParameterFilter WorkflowParameterFilter = new WorkflowParameterFilter();
            WorkflowParameterFilter.Selects = WorkflowParameterSelect.ALL;
            WorkflowParameterFilter.Skip = WorkflowParameter_WorkflowParameterFilterDTO.Skip;
            WorkflowParameterFilter.Take = WorkflowParameter_WorkflowParameterFilterDTO.Take;
            WorkflowParameterFilter.OrderBy = WorkflowParameter_WorkflowParameterFilterDTO.OrderBy;
            WorkflowParameterFilter.OrderType = WorkflowParameter_WorkflowParameterFilterDTO.OrderType;

            WorkflowParameterFilter.Id = WorkflowParameter_WorkflowParameterFilterDTO.Id;
            WorkflowParameterFilter.Code = WorkflowParameter_WorkflowParameterFilterDTO.Code;
            WorkflowParameterFilter.Name = WorkflowParameter_WorkflowParameterFilterDTO.Name;
            WorkflowParameterFilter.WorkflowTypeId = WorkflowParameter_WorkflowParameterFilterDTO.WorkflowTypeId;
            WorkflowParameterFilter.WorkflowParameterTypeId = WorkflowParameter_WorkflowParameterFilterDTO.WorkflowParameterTypeId;
            return WorkflowParameterFilter;
        }
    }
}

