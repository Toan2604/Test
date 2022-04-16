using DMS.Common;
using DMS.Entities;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MStatus;
using DMS.Services.MWorkflow;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.workflow_direction
{
    public class WorkflowDirectionController : RpcController
    {
        private IAppUserService AppUserService;
        private IWorkflowStepService WorkflowStepService;
        private IWorkflowDefinitionService WorkflowDefinitionService;
        private IWorkflowDirectionService WorkflowDirectionService;
        private IWorkflowParameterService WorkflowParameterService;
        private IWorkflowOperatorService WorkflowOperatorService;
        private IOrganizationService OrganizationService;
        private IStatusService StatusService;
        private ICurrentContext CurrentContext;
        public WorkflowDirectionController(
            IAppUserService AppUserService,
            IWorkflowStepService WorkflowStepService,
            IWorkflowDefinitionService WorkflowDefinitionService,
            IWorkflowDirectionService WorkflowDirectionService,
            IWorkflowParameterService WorkflowParameterService,
            IWorkflowOperatorService WorkflowOperatorService,
            IOrganizationService OrganizationService,
            IStatusService StatusService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.WorkflowStepService = WorkflowStepService;
            this.WorkflowDefinitionService = WorkflowDefinitionService;
            this.WorkflowDirectionService = WorkflowDirectionService;
            this.WorkflowParameterService = WorkflowParameterService;
            this.WorkflowOperatorService = WorkflowOperatorService;
            this.OrganizationService = OrganizationService;
            this.StatusService = StatusService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkflowDirectionRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] WorkflowDirection_WorkflowDirectionFilterDTO WorkflowDirection_WorkflowDirectionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDirectionFilter WorkflowDirectionFilter = ConvertFilterDTOToFilterEntity(WorkflowDirection_WorkflowDirectionFilterDTO);
            WorkflowDirectionFilter = WorkflowDirectionService.ToFilter(WorkflowDirectionFilter);
            int count = await WorkflowDirectionService.Count(WorkflowDirectionFilter);
            return count;
        }

        [Route(WorkflowDirectionRoute.List), HttpPost]
        public async Task<ActionResult<List<WorkflowDirection_WorkflowDirectionDTO>>> List([FromBody] WorkflowDirection_WorkflowDirectionFilterDTO WorkflowDirection_WorkflowDirectionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDirectionFilter WorkflowDirectionFilter = ConvertFilterDTOToFilterEntity(WorkflowDirection_WorkflowDirectionFilterDTO);
            WorkflowDirectionFilter = WorkflowDirectionService.ToFilter(WorkflowDirectionFilter);
            List<WorkflowDirection> WorkflowDirections = await WorkflowDirectionService.List(WorkflowDirectionFilter);
            List<WorkflowDirection_WorkflowDirectionDTO> WorkflowDirection_WorkflowDirectionDTOs = WorkflowDirections
                .Select(c => new WorkflowDirection_WorkflowDirectionDTO(c)).ToList();
            return WorkflowDirection_WorkflowDirectionDTOs;
        }

        [Route(WorkflowDirectionRoute.Get), HttpPost]
        public async Task<ActionResult<WorkflowDirection_WorkflowDirectionDTO>> Get([FromBody] WorkflowDirection_WorkflowDirectionDTO WorkflowDirection_WorkflowDirectionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDirection WorkflowDirection = await WorkflowDirectionService.Get(WorkflowDirection_WorkflowDirectionDTO.Id);
            return new WorkflowDirection_WorkflowDirectionDTO(WorkflowDirection);
        }

        [Route(WorkflowDirectionRoute.Create), HttpPost]
        public async Task<ActionResult<WorkflowDirection_WorkflowDirectionDTO>> Create([FromBody] WorkflowDirection_WorkflowDirectionDTO WorkflowDirection_WorkflowDirectionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDirection WorkflowDirection = ConvertDTOToEntity(WorkflowDirection_WorkflowDirectionDTO);
            WorkflowDirection = await WorkflowDirectionService.Create(WorkflowDirection);
            WorkflowDirection_WorkflowDirectionDTO = new WorkflowDirection_WorkflowDirectionDTO(WorkflowDirection);
            if (WorkflowDirection.IsValidated)
                return WorkflowDirection_WorkflowDirectionDTO;
            else
                return BadRequest(WorkflowDirection_WorkflowDirectionDTO);
        }

        [Route(WorkflowDirectionRoute.Update), HttpPost]
        public async Task<ActionResult<WorkflowDirection_WorkflowDirectionDTO>> Update([FromBody] WorkflowDirection_WorkflowDirectionDTO WorkflowDirection_WorkflowDirectionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDirection WorkflowDirection = ConvertDTOToEntity(WorkflowDirection_WorkflowDirectionDTO);
            WorkflowDirection = await WorkflowDirectionService.Update(WorkflowDirection);
            WorkflowDirection_WorkflowDirectionDTO = new WorkflowDirection_WorkflowDirectionDTO(WorkflowDirection);
            if (WorkflowDirection.IsValidated)
                return WorkflowDirection_WorkflowDirectionDTO;
            else
                return BadRequest(WorkflowDirection_WorkflowDirectionDTO);
        }

        [Route(WorkflowDirectionRoute.Delete), HttpPost]
        public async Task<ActionResult<WorkflowDirection_WorkflowDirectionDTO>> Delete([FromBody] WorkflowDirection_WorkflowDirectionDTO WorkflowDirection_WorkflowDirectionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDirection WorkflowDirection = ConvertDTOToEntity(WorkflowDirection_WorkflowDirectionDTO);
            WorkflowDirection = await WorkflowDirectionService.Delete(WorkflowDirection);
            WorkflowDirection_WorkflowDirectionDTO = new WorkflowDirection_WorkflowDirectionDTO(WorkflowDirection);
            if (WorkflowDirection.IsValidated)
                return WorkflowDirection_WorkflowDirectionDTO;
            else
                return BadRequest(WorkflowDirection_WorkflowDirectionDTO);
        }

        [Route(WorkflowDirectionRoute.Export), HttpPost]
        public async Task<FileResult> Export([FromBody] WorkflowDirection_WorkflowDirectionFilterDTO WorkflowDirection_WorkflowDirectionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region WorkflowDirection
                var WorkflowDirectionFilter = ConvertFilterDTOToFilterEntity(WorkflowDirection_WorkflowDirectionFilterDTO);
                WorkflowDirectionFilter.Skip = 0;
                WorkflowDirectionFilter.Take = int.MaxValue;
                WorkflowDirectionFilter = WorkflowDirectionService.ToFilter(WorkflowDirectionFilter);
                List<WorkflowDirection> WorkflowDirections = await WorkflowDirectionService.List(WorkflowDirectionFilter);

                var WorkflowDirectionHeaders = new List<string>()
                {
                        "Id",
                        "WorkflowDefinitionId",
                        "FromStepId",
                        "ToStepId",
                        "SubjectMailForCreator",
                        "SubjectMailForNextStep",
                        "BodyMailForCreator",
                        "BodyMailForNextStep",
                };
                List<object[]> WorkflowDirectionData = new List<object[]>();
                for (int i = 0; i < WorkflowDirections.Count; i++)
                {
                    var WorkflowDirection = WorkflowDirections[i];
                    WorkflowDirectionData.Add(new Object[]
                    {
                        WorkflowDirection.Id,
                        WorkflowDirection.WorkflowDefinitionId,
                        WorkflowDirection.FromStepId,
                        WorkflowDirection.ToStepId,
                        WorkflowDirection.SubjectMailForCreator,
                        WorkflowDirection.SubjectMailForNextStep,
                        WorkflowDirection.BodyMailForCreator,
                        WorkflowDirection.BodyMailForNextStep,
                    });
                }
                excel.GenerateWorksheet("WorkflowDirection", WorkflowDirectionHeaders, WorkflowDirectionData);
                #endregion

                #region WorkflowStep
                var WorkflowStepFilter = new WorkflowStepFilter();
                WorkflowStepFilter.Selects = WorkflowStepSelect.ALL;
                WorkflowStepFilter.OrderBy = WorkflowStepOrder.Id;
                WorkflowStepFilter.OrderType = OrderType.ASC;
                WorkflowStepFilter.Skip = 0;
                WorkflowStepFilter.Take = int.MaxValue;
                List<WorkflowStep> WorkflowSteps = await WorkflowStepService.List(WorkflowStepFilter);

                var WorkflowStepHeaders = new List<string>()
                {
                        "Id",
                        "WorkflowDefinitionId",
                        "Name",
                        "RoleId",
                        "SubjectMailForReject",
                        "BodyMailForReject",
                };
                List<object[]> WorkflowStepData = new List<object[]>();
                for (int i = 0; i < WorkflowSteps.Count; i++)
                {
                    var WorkflowStep = WorkflowSteps[i];
                    WorkflowStepData.Add(new Object[]
                    {
                        WorkflowStep.Id,
                        WorkflowStep.WorkflowDefinitionId,
                        WorkflowStep.Name,
                        WorkflowStep.RoleId,
                        WorkflowStep.SubjectMailForReject,
                        WorkflowStep.BodyMailForReject,
                    });
                }
                excel.GenerateWorksheet("WorkflowStep", WorkflowStepHeaders, WorkflowStepData);
                #endregion
                #region WorkflowDefinition
                var WorkflowDefinitionFilter = new WorkflowDefinitionFilter();
                WorkflowDefinitionFilter.Selects = WorkflowDefinitionSelect.ALL;
                WorkflowDefinitionFilter.OrderBy = WorkflowDefinitionOrder.Id;
                WorkflowDefinitionFilter.OrderType = OrderType.ASC;
                WorkflowDefinitionFilter.Skip = 0;
                WorkflowDefinitionFilter.Take = int.MaxValue;
                List<WorkflowDefinition> WorkflowDefinitions = await WorkflowDefinitionService.List(WorkflowDefinitionFilter);

                var WorkflowDefinitionHeaders = new List<string>()
                {
                    "Id",
                    "Name",
                    "WorkflowTypeId",
                    "StartDate",
                    "EndDate",
                    "StatusId",
                };
                List<object[]> WorkflowDefinitionData = new List<object[]>();
                for (int i = 0; i < WorkflowDefinitions.Count; i++)
                {
                    var WorkflowDefinition = WorkflowDefinitions[i];
                    WorkflowDefinitionData.Add(new Object[]
                    {
                        WorkflowDefinition.Id,
                        WorkflowDefinition.Name,
                        WorkflowDefinition.WorkflowTypeId,
                        WorkflowDefinition.StartDate,
                        WorkflowDefinition.EndDate,
                        WorkflowDefinition.StatusId,
                    });
                }
                excel.GenerateWorksheet("WorkflowDefinition", WorkflowDefinitionHeaders, WorkflowDefinitionData);
                #endregion
                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "WorkflowDirection.xlsx");
        }


        private WorkflowDirection ConvertDTOToEntity(WorkflowDirection_WorkflowDirectionDTO WorkflowDirection_WorkflowDirectionDTO)
        {
            WorkflowDirection_WorkflowDirectionDTO.TrimString();
            WorkflowDirection WorkflowDirection = new WorkflowDirection();
            WorkflowDirection.Id = WorkflowDirection_WorkflowDirectionDTO.Id;
            WorkflowDirection.WorkflowDefinitionId = WorkflowDirection_WorkflowDirectionDTO.WorkflowDefinitionId;
            WorkflowDirection.FromStepId = WorkflowDirection_WorkflowDirectionDTO.FromStepId;
            WorkflowDirection.ToStepId = WorkflowDirection_WorkflowDirectionDTO.ToStepId;
            WorkflowDirection.SubjectMailForCreator = WorkflowDirection_WorkflowDirectionDTO.SubjectMailForCreator;
            WorkflowDirection.SubjectMailForCurrentStep = WorkflowDirection_WorkflowDirectionDTO.SubjectMailForCurrentStep;
            WorkflowDirection.SubjectMailForNextStep = WorkflowDirection_WorkflowDirectionDTO.SubjectMailForNextStep;
            WorkflowDirection.BodyMailForCreator = WorkflowDirection_WorkflowDirectionDTO.BodyMailForCreator;
            WorkflowDirection.BodyMailForCurrentStep = WorkflowDirection_WorkflowDirectionDTO.BodyMailForCurrentStep;
            WorkflowDirection.BodyMailForNextStep = WorkflowDirection_WorkflowDirectionDTO.BodyMailForNextStep;
            WorkflowDirection.UpdatedAt = WorkflowDirection_WorkflowDirectionDTO.UpdatedAt;
            WorkflowDirection.StatusId = WorkflowDirection_WorkflowDirectionDTO.StatusId;
            WorkflowDirection.FromStep = WorkflowDirection_WorkflowDirectionDTO.FromStep == null ? null : new WorkflowStep
            {
                Id = WorkflowDirection_WorkflowDirectionDTO.FromStep.Id,
                WorkflowDefinitionId = WorkflowDirection_WorkflowDirectionDTO.FromStep.WorkflowDefinitionId,
                Name = WorkflowDirection_WorkflowDirectionDTO.FromStep.Name,
                Code = WorkflowDirection_WorkflowDirectionDTO.FromStep.Code,
                RoleId = WorkflowDirection_WorkflowDirectionDTO.FromStep.RoleId,
                SubjectMailForReject = WorkflowDirection_WorkflowDirectionDTO.FromStep.SubjectMailForReject,
                BodyMailForReject = WorkflowDirection_WorkflowDirectionDTO.FromStep.BodyMailForReject,
            };
            WorkflowDirection.ToStep = WorkflowDirection_WorkflowDirectionDTO.ToStep == null ? null : new WorkflowStep
            {
                Id = WorkflowDirection_WorkflowDirectionDTO.ToStep.Id,
                WorkflowDefinitionId = WorkflowDirection_WorkflowDirectionDTO.ToStep.WorkflowDefinitionId,
                Code = WorkflowDirection_WorkflowDirectionDTO.ToStep.Code,
                Name = WorkflowDirection_WorkflowDirectionDTO.ToStep.Name,
                RoleId = WorkflowDirection_WorkflowDirectionDTO.ToStep.RoleId,
                SubjectMailForReject = WorkflowDirection_WorkflowDirectionDTO.ToStep.SubjectMailForReject,
                BodyMailForReject = WorkflowDirection_WorkflowDirectionDTO.ToStep.BodyMailForReject,
            };
            WorkflowDirection.Status = WorkflowDirection_WorkflowDirectionDTO.Status == null ? null : new Status
            {
                Id = WorkflowDirection_WorkflowDirectionDTO.Status.Id,
                Code = WorkflowDirection_WorkflowDirectionDTO.Status.Code,
                Name = WorkflowDirection_WorkflowDirectionDTO.Status.Name,
            };
            WorkflowDirection.WorkflowDefinition = WorkflowDirection_WorkflowDirectionDTO.WorkflowDefinition == null ? null : new WorkflowDefinition
            {
                Id = WorkflowDirection_WorkflowDirectionDTO.WorkflowDefinition.Id,
                Code = WorkflowDirection_WorkflowDirectionDTO.WorkflowDefinition.Code,
                Name = WorkflowDirection_WorkflowDirectionDTO.WorkflowDefinition.Name,
                WorkflowTypeId = WorkflowDirection_WorkflowDirectionDTO.WorkflowDefinition.WorkflowTypeId,
                StartDate = WorkflowDirection_WorkflowDirectionDTO.WorkflowDefinition.StartDate,
                EndDate = WorkflowDirection_WorkflowDirectionDTO.WorkflowDefinition.EndDate,
                StatusId = WorkflowDirection_WorkflowDirectionDTO.WorkflowDefinition.StatusId,
                UpdatedAt = WorkflowDirection_WorkflowDirectionDTO.WorkflowDefinition.UpdatedAt,
            };
            WorkflowDirection.WorkflowDirectionConditions = WorkflowDirection_WorkflowDirectionDTO.WorkflowDirectionConditions?.Select(x => new WorkflowDirectionCondition
            {
                Id = x.Id,
                Value = x.Value,
                ValueString = x.ValueString,
                WorkflowDirectionId = x.WorkflowDirectionId,
                WorkflowOperatorId = x.WorkflowOperatorId,
                WorkflowParameterId = x.WorkflowParameterId,
                WorkflowOperator = x.WorkflowOperator == null ? null : new WorkflowOperator
                {
                    Id = x.WorkflowOperator.Id,
                    Code = x.WorkflowOperator.Code,
                    Name = x.WorkflowOperator.Name,
                },
                WorkflowParameter = x.WorkflowParameter == null ? null : new WorkflowParameter
                {
                    Id = x.WorkflowParameter.Id,
                    Code = x.WorkflowParameter.Code,
                    Name = x.WorkflowParameter.Name,
                    WorkflowParameterTypeId = x.WorkflowParameter.WorkflowParameterTypeId,
                    WorkflowTypeId = x.WorkflowParameter.WorkflowTypeId,
                }
            }).ToList();
            WorkflowDirection.BaseLanguage = CurrentContext.Language;
            return WorkflowDirection;
        }

        private WorkflowDirectionFilter ConvertFilterDTOToFilterEntity(WorkflowDirection_WorkflowDirectionFilterDTO WorkflowDirection_WorkflowDirectionFilterDTO)
        {
            WorkflowDirectionFilter WorkflowDirectionFilter = new WorkflowDirectionFilter();
            WorkflowDirectionFilter.Selects = WorkflowDirectionSelect.ALL;
            WorkflowDirectionFilter.Skip = WorkflowDirection_WorkflowDirectionFilterDTO.Skip;
            WorkflowDirectionFilter.Take = WorkflowDirection_WorkflowDirectionFilterDTO.Take;
            WorkflowDirectionFilter.OrderBy = WorkflowDirection_WorkflowDirectionFilterDTO.OrderBy;
            WorkflowDirectionFilter.OrderType = WorkflowDirection_WorkflowDirectionFilterDTO.OrderType;

            WorkflowDirectionFilter.Id = WorkflowDirection_WorkflowDirectionFilterDTO.Id;
            WorkflowDirectionFilter.WorkflowDefinitionId = WorkflowDirection_WorkflowDirectionFilterDTO.WorkflowDefinitionId;
            WorkflowDirectionFilter.FromStepId = WorkflowDirection_WorkflowDirectionFilterDTO.FromStepId;
            WorkflowDirectionFilter.ToStepId = WorkflowDirection_WorkflowDirectionFilterDTO.ToStepId;
            WorkflowDirectionFilter.StatusId = WorkflowDirection_WorkflowDirectionFilterDTO.StatusId;
            WorkflowDirectionFilter.UpdatedAt = WorkflowDirection_WorkflowDirectionFilterDTO.UpdatedAt;
            return WorkflowDirectionFilter;
        }

        [Route(WorkflowDirectionRoute.FilterListAppUser), HttpPost]
        public async Task<List<WorkflowDirection_AppUserDTO>> FilterListAppUser([FromBody] WorkflowDirection_AppUserFilterDTO WorkflowDirection_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = WorkflowDirection_AppUserFilterDTO.Id;
            AppUserFilter.Username = WorkflowDirection_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = WorkflowDirection_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = WorkflowDirection_AppUserFilterDTO.Address;
            AppUserFilter.Email = WorkflowDirection_AppUserFilterDTO.Email;
            AppUserFilter.Phone = WorkflowDirection_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = WorkflowDirection_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = WorkflowDirection_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = WorkflowDirection_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = WorkflowDirection_AppUserFilterDTO.StatusId;
            AppUserFilter.ProvinceId = WorkflowDirection_AppUserFilterDTO.ProvinceId;
            AppUserFilter.SexId = WorkflowDirection_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = WorkflowDirection_AppUserFilterDTO.Birthday;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<WorkflowDirection_AppUserDTO> WorkflowDirection_AppUserDTOs = AppUsers
                .Select(x => new WorkflowDirection_AppUserDTO(x)).ToList();
            return WorkflowDirection_AppUserDTOs;
        }

        [Route(WorkflowDirectionRoute.FilterListWorkflowStep), HttpPost]
        public async Task<List<WorkflowDirection_WorkflowStepDTO>> FilterListWorkflowStep([FromBody] WorkflowDirection_WorkflowStepFilterDTO WorkflowDirection_WorkflowStepFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowStepFilter WorkflowStepFilter = new WorkflowStepFilter();
            WorkflowStepFilter.Skip = 0;
            WorkflowStepFilter.Take = 20;
            WorkflowStepFilter.OrderBy = WorkflowStepOrder.Id;
            WorkflowStepFilter.OrderType = OrderType.ASC;
            WorkflowStepFilter.Selects = WorkflowStepSelect.ALL;
            WorkflowStepFilter.Id = WorkflowDirection_WorkflowStepFilterDTO.Id;
            WorkflowStepFilter.WorkflowDefinitionId = WorkflowDirection_WorkflowStepFilterDTO.WorkflowDefinitionId;
            WorkflowStepFilter.Name = WorkflowDirection_WorkflowStepFilterDTO.Name;
            WorkflowStepFilter.Code = WorkflowDirection_WorkflowStepFilterDTO.Code;
            WorkflowStepFilter.RoleId = WorkflowDirection_WorkflowStepFilterDTO.RoleId;

            List<WorkflowStep> WorkflowSteps = await WorkflowStepService.List(WorkflowStepFilter);
            List<WorkflowDirection_WorkflowStepDTO> WorkflowDirection_WorkflowStepDTOs = WorkflowSteps
                .Select(x => new WorkflowDirection_WorkflowStepDTO(x)).ToList();
            return WorkflowDirection_WorkflowStepDTOs;
        }
        [Route(WorkflowDirectionRoute.FilterListWorkflowDefinition), HttpPost]
        public async Task<List<WorkflowDirection_WorkflowDefinitionDTO>> FilterListWorkflowDefinition([FromBody] WorkflowDirection_WorkflowDefinitionFilterDTO WorkflowDirection_WorkflowDefinitionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDefinitionFilter WorkflowDefinitionFilter = new WorkflowDefinitionFilter();
            WorkflowDefinitionFilter.Skip = 0;
            WorkflowDefinitionFilter.Take = 20;
            WorkflowDefinitionFilter.OrderBy = WorkflowDefinitionOrder.Id;
            WorkflowDefinitionFilter.OrderType = OrderType.ASC;
            WorkflowDefinitionFilter.Selects = WorkflowDefinitionSelect.ALL;
            WorkflowDefinitionFilter.Id = WorkflowDirection_WorkflowDefinitionFilterDTO.Id;
            WorkflowDefinitionFilter.Code = WorkflowDirection_WorkflowDefinitionFilterDTO.Code;
            WorkflowDefinitionFilter.Name = WorkflowDirection_WorkflowDefinitionFilterDTO.Name;
            WorkflowDefinitionFilter.WorkflowTypeId = WorkflowDirection_WorkflowDefinitionFilterDTO.WorkflowTypeId;
            WorkflowDefinitionFilter.StartDate = WorkflowDirection_WorkflowDefinitionFilterDTO.StartDate;
            WorkflowDefinitionFilter.EndDate = WorkflowDirection_WorkflowDefinitionFilterDTO.EndDate;
            WorkflowDefinitionFilter.StatusId = WorkflowDirection_WorkflowDefinitionFilterDTO.StatusId;
            WorkflowDefinitionFilter.UpdatedAt = WorkflowDirection_WorkflowDefinitionFilterDTO.UpdatedAt;

            List<WorkflowDefinition> WorkflowDefinitions = await WorkflowDefinitionService.List(WorkflowDefinitionFilter);
            List<WorkflowDirection_WorkflowDefinitionDTO> WorkflowDirection_WorkflowDefinitionDTOs = WorkflowDefinitions
                .Select(x => new WorkflowDirection_WorkflowDefinitionDTO(x)).ToList();
            return WorkflowDirection_WorkflowDefinitionDTOs;
        }

        [Route(WorkflowDirectionRoute.FilterListStatus), HttpPost]
        public async Task<List<WorkflowDirection_StatusDTO>> FilterListStatus()
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<WorkflowDirection_StatusDTO> WorkflowDirection_StatusDTOs = Statuses
                .Select(x => new WorkflowDirection_StatusDTO(x)).ToList();
            return WorkflowDirection_StatusDTOs;
        }

        [Route(WorkflowDirectionRoute.SingleListAppUser), HttpPost]
        public async Task<List<WorkflowDirection_AppUserDTO>> SingleListAppUser([FromBody] WorkflowDirection_AppUserFilterDTO WorkflowDirection_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = WorkflowDirection_AppUserFilterDTO.Id;
            AppUserFilter.Username = WorkflowDirection_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = WorkflowDirection_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = WorkflowDirection_AppUserFilterDTO.Address;
            AppUserFilter.Email = WorkflowDirection_AppUserFilterDTO.Email;
            AppUserFilter.Phone = WorkflowDirection_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = WorkflowDirection_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = WorkflowDirection_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = WorkflowDirection_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            AppUserFilter.ProvinceId = WorkflowDirection_AppUserFilterDTO.ProvinceId;
            AppUserFilter.SexId = WorkflowDirection_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = WorkflowDirection_AppUserFilterDTO.Birthday;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<WorkflowDirection_AppUserDTO> WorkflowDirection_AppUserDTOs = AppUsers
                .Select(x => new WorkflowDirection_AppUserDTO(x)).ToList();
            return WorkflowDirection_AppUserDTOs;
        }

        [Route(WorkflowDirectionRoute.SingleListWorkflowStep), HttpPost]
        public async Task<List<WorkflowDirection_WorkflowStepDTO>> SingleListWorkflowStep([FromBody] WorkflowDirection_WorkflowStepFilterDTO WorkflowDirection_WorkflowStepFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowStepFilter WorkflowStepFilter = new WorkflowStepFilter();
            WorkflowStepFilter.Skip = 0;
            WorkflowStepFilter.Take = 20;
            WorkflowStepFilter.OrderBy = WorkflowStepOrder.Id;
            WorkflowStepFilter.OrderType = OrderType.ASC;
            WorkflowStepFilter.Selects = WorkflowStepSelect.ALL;
            WorkflowStepFilter.Id = WorkflowDirection_WorkflowStepFilterDTO.Id;
            WorkflowStepFilter.WorkflowDefinitionId = WorkflowDirection_WorkflowStepFilterDTO.WorkflowDefinitionId;
            WorkflowStepFilter.Code = WorkflowDirection_WorkflowStepFilterDTO.Code;
            WorkflowStepFilter.Name = WorkflowDirection_WorkflowStepFilterDTO.Name;
            WorkflowStepFilter.RoleId = WorkflowDirection_WorkflowStepFilterDTO.RoleId;
            WorkflowStepFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<WorkflowStep> WorkflowSteps = await WorkflowStepService.List(WorkflowStepFilter);
            List<WorkflowDirection_WorkflowStepDTO> WorkflowDirection_WorkflowStepDTOs = WorkflowSteps
                .Select(x => new WorkflowDirection_WorkflowStepDTO(x)).ToList();
            return WorkflowDirection_WorkflowStepDTOs;
        }

        [Route(WorkflowDirectionRoute.SingleListWorkflowDefinition), HttpPost]
        public async Task<List<WorkflowDirection_WorkflowDefinitionDTO>> SingleListWorkflowDefinition([FromBody] WorkflowDirection_WorkflowDefinitionFilterDTO WorkflowDirection_WorkflowDefinitionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDefinitionFilter WorkflowDefinitionFilter = new WorkflowDefinitionFilter();
            WorkflowDefinitionFilter.Skip = 0;
            WorkflowDefinitionFilter.Take = 20;
            WorkflowDefinitionFilter.OrderBy = WorkflowDefinitionOrder.Id;
            WorkflowDefinitionFilter.OrderType = OrderType.ASC;
            WorkflowDefinitionFilter.Selects = WorkflowDefinitionSelect.ALL;
            WorkflowDefinitionFilter.Id = WorkflowDirection_WorkflowDefinitionFilterDTO.Id;
            WorkflowDefinitionFilter.Code = WorkflowDirection_WorkflowDefinitionFilterDTO.Code;
            WorkflowDefinitionFilter.Name = WorkflowDirection_WorkflowDefinitionFilterDTO.Name;
            WorkflowDefinitionFilter.WorkflowTypeId = WorkflowDirection_WorkflowDefinitionFilterDTO.WorkflowTypeId;
            WorkflowDefinitionFilter.StartDate = WorkflowDirection_WorkflowDefinitionFilterDTO.StartDate;
            WorkflowDefinitionFilter.EndDate = WorkflowDirection_WorkflowDefinitionFilterDTO.EndDate;
            // WorkflowDefinitionFilter.StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id };
            WorkflowDefinitionFilter.Used = false;
            WorkflowDefinitionFilter.UpdatedAt = WorkflowDirection_WorkflowDefinitionFilterDTO.UpdatedAt;

            List<WorkflowDefinition> WorkflowDefinitions = await WorkflowDefinitionService.List(WorkflowDefinitionFilter);
            List<WorkflowDirection_WorkflowDefinitionDTO> WorkflowDirection_WorkflowDefinitionDTOs = WorkflowDefinitions
                .Select(x => new WorkflowDirection_WorkflowDefinitionDTO(x)).ToList();
            return WorkflowDirection_WorkflowDefinitionDTOs;
        }

        [Route(WorkflowDirectionRoute.SingleListWorkflowParameter), HttpPost]
        public async Task<List<WorkflowDirection_WorkflowParameterDTO>> SingleListWorkflowParameter([FromBody] WorkflowDirection_WorkflowParameterFilterDTO WorkflowDirection_WorkflowParameterFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowParameterFilter WorkflowParameterFilter = new WorkflowParameterFilter();
            WorkflowParameterFilter.Skip = 0;
            WorkflowParameterFilter.Take = 20;
            WorkflowParameterFilter.OrderBy = WorkflowParameterOrder.Id;
            WorkflowParameterFilter.OrderType = OrderType.ASC;
            WorkflowParameterFilter.Selects = WorkflowParameterSelect.ALL;
            WorkflowParameterFilter.Id = WorkflowDirection_WorkflowParameterFilterDTO.Id;
            WorkflowParameterFilter.Code = WorkflowDirection_WorkflowParameterFilterDTO.Code;
            WorkflowParameterFilter.Name = WorkflowDirection_WorkflowParameterFilterDTO.Name;
            WorkflowParameterFilter.WorkflowTypeId = WorkflowDirection_WorkflowParameterFilterDTO.WorkflowTypeId;
            WorkflowParameterFilter.WorkflowParameterTypeId = WorkflowDirection_WorkflowParameterFilterDTO.WorkflowParameterTypeId;

            List<WorkflowParameter> WorkflowParameters = await WorkflowParameterService.List(WorkflowParameterFilter);
            List<WorkflowDirection_WorkflowParameterDTO> WorkflowDirection_WorkflowParameterDTOs = WorkflowParameters
                .Select(x => new WorkflowDirection_WorkflowParameterDTO(x)).ToList();
            return WorkflowDirection_WorkflowParameterDTOs;
        }

        [Route(WorkflowDirectionRoute.SingleListWorkflowOperator), HttpPost]
        public async Task<List<WorkflowDirection_WorkflowOperatorDTO>> SingleListWorkflowOperator([FromBody] WorkflowDirection_WorkflowOperatorFilterDTO WorkflowDirection_WorkflowOperatorFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowOperatorFilter WorkflowOperatorFilter = new WorkflowOperatorFilter();
            WorkflowOperatorFilter.Skip = 0;
            WorkflowOperatorFilter.Take = 20;
            WorkflowOperatorFilter.OrderBy = WorkflowOperatorOrder.Id;
            WorkflowOperatorFilter.OrderType = OrderType.ASC;
            WorkflowOperatorFilter.Selects = WorkflowOperatorSelect.ALL;
            WorkflowOperatorFilter.Id = WorkflowDirection_WorkflowOperatorFilterDTO.Id;
            WorkflowOperatorFilter.Code = WorkflowDirection_WorkflowOperatorFilterDTO.Code;
            WorkflowOperatorFilter.Name = WorkflowDirection_WorkflowOperatorFilterDTO.Name;
            WorkflowOperatorFilter.WorkflowParameterTypeId = WorkflowDirection_WorkflowOperatorFilterDTO.WorkflowParameterTypeId;

            List<WorkflowOperator> WorkflowOperators = await WorkflowOperatorService.List(WorkflowOperatorFilter);
            List<WorkflowDirection_WorkflowOperatorDTO> WorkflowDirection_WorkflowOperatorDTOs = WorkflowOperators
                .Select(x => new WorkflowDirection_WorkflowOperatorDTO(x)).ToList();
            return WorkflowDirection_WorkflowOperatorDTOs;
        }

        [Route(WorkflowDirectionRoute.SingleListOrganization), HttpPost]
        public async Task<List<WorkflowDirection_OrganizationDTO>> SingleListOrganization([FromBody] WorkflowDirection_OrganizationFilterDTO WorkflowDirection_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = WorkflowDirection_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = WorkflowDirection_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = WorkflowDirection_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = WorkflowDirection_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = WorkflowDirection_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = WorkflowDirection_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = WorkflowDirection_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = WorkflowDirection_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = WorkflowDirection_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<WorkflowDirection_OrganizationDTO> WorkflowDirection_OrganizationDTOs = Organizations
                .Select(x => new WorkflowDirection_OrganizationDTO(x)).ToList();
            return WorkflowDirection_OrganizationDTOs;
        }

        [Route(WorkflowDirectionRoute.SingleListStatus), HttpPost]
        public async Task<List<WorkflowDirection_StatusDTO>> SingleListStatus()
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<WorkflowDirection_StatusDTO> WorkflowDirection_StatusDTOs = Statuses
                .Select(x => new WorkflowDirection_StatusDTO(x)).ToList();
            return WorkflowDirection_StatusDTOs;
        }
    }
}

