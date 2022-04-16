using DMS.Common;
using DMS.Entities;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MRole;
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

namespace DMS.Rpc.workflow_definition
{

    public class WorkflowDefinitionController : RpcController
    {
        private IAppUserService AppUserService;
        private IWorkflowTypeService WorkflowTypeService;
        private IWorkflowDirectionService WorkflowDirectionService;
        private IWorkflowStepService WorkflowStepService;
        private IWorkflowParameterService WorkflowParameterService;
        private IRoleService RoleService;
        private IWorkflowDefinitionService WorkflowDefinitionService;
        private IStatusService StatusService;
        private IOrganizationService OrganizationService;
        private ICurrentContext CurrentContext;
        public WorkflowDefinitionController(
            IAppUserService AppUserService,
            IWorkflowTypeService WorkflowTypeService,
            IWorkflowDirectionService WorkflowDirectionService,
            IWorkflowStepService WorkflowStepService,
            IWorkflowParameterService WorkflowParameterService,
            IRoleService RoleService,
            IWorkflowDefinitionService WorkflowDefinitionService,
            IStatusService StatusService,
            IOrganizationService OrganizationService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.WorkflowTypeService = WorkflowTypeService;
            this.WorkflowDirectionService = WorkflowDirectionService;
            this.WorkflowStepService = WorkflowStepService;
            this.WorkflowParameterService = WorkflowParameterService;
            this.RoleService = RoleService;
            this.WorkflowDefinitionService = WorkflowDefinitionService;
            this.StatusService = StatusService;
            this.OrganizationService = OrganizationService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkflowDefinitionRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] WorkflowDefinition_WorkflowDefinitionFilterDTO WorkflowDefinition_WorkflowDefinitionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDefinitionFilter WorkflowDefinitionFilter = ConvertFilterDTOToFilterEntity(WorkflowDefinition_WorkflowDefinitionFilterDTO);
            WorkflowDefinitionFilter = WorkflowDefinitionService.ToFilter(WorkflowDefinitionFilter);
            int count = await WorkflowDefinitionService.Count(WorkflowDefinitionFilter);
            return count;
        }

        [Route(WorkflowDefinitionRoute.List), HttpPost]
        public async Task<ActionResult<List<WorkflowDefinition_WorkflowDefinitionDTO>>> List([FromBody] WorkflowDefinition_WorkflowDefinitionFilterDTO WorkflowDefinition_WorkflowDefinitionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDefinitionFilter WorkflowDefinitionFilter = ConvertFilterDTOToFilterEntity(WorkflowDefinition_WorkflowDefinitionFilterDTO);
            WorkflowDefinitionFilter = WorkflowDefinitionService.ToFilter(WorkflowDefinitionFilter);
            List<WorkflowDefinition> WorkflowDefinitions = await WorkflowDefinitionService.List(WorkflowDefinitionFilter);
            List<WorkflowDefinition_WorkflowDefinitionDTO> WorkflowDefinition_WorkflowDefinitionDTOs = WorkflowDefinitions
                .Select(c => new WorkflowDefinition_WorkflowDefinitionDTO(c)).ToList();
            return WorkflowDefinition_WorkflowDefinitionDTOs;
        }

        [Route(WorkflowDefinitionRoute.Get), HttpPost]
        public async Task<ActionResult<WorkflowDefinition_WorkflowDefinitionDTO>> Get([FromBody] WorkflowDefinition_WorkflowDefinitionDTO WorkflowDefinition_WorkflowDefinitionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkflowDefinition_WorkflowDefinitionDTO.Id))
                return Forbid();

            WorkflowDefinition WorkflowDefinition = await WorkflowDefinitionService.Get(WorkflowDefinition_WorkflowDefinitionDTO.Id);
            return new WorkflowDefinition_WorkflowDefinitionDTO(WorkflowDefinition);
        }

        [Route(WorkflowDefinitionRoute.GetDirection), HttpPost]
        public async Task<ActionResult<WorkflowDefinition_WorkflowDirectionDTO>> GetDirection([FromBody] WorkflowDefinition_WorkflowDirectionDTO WorkflowDefinition_WorkflowDirectionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDirection WorkflowDirection = await WorkflowDirectionService.Get(WorkflowDefinition_WorkflowDirectionDTO.Id);
            return new WorkflowDefinition_WorkflowDirectionDTO(WorkflowDirection);
        }

        [Route(WorkflowDefinitionRoute.Check), HttpPost]
        public async Task<ActionResult<WorkflowDefinition_WorkflowDefinitionDTO>> Check([FromBody] WorkflowDefinition_WorkflowDefinitionDTO WorkflowDefinition_WorkflowDefinitionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkflowDefinition_WorkflowDefinitionDTO.Id))
                return Forbid();

            WorkflowDefinition WorkflowDefinition = ConvertDTOToEntity(WorkflowDefinition_WorkflowDefinitionDTO);
            WorkflowDefinition = await WorkflowDefinitionService.Check(WorkflowDefinition);
            WorkflowDefinition_WorkflowDefinitionDTO = new WorkflowDefinition_WorkflowDefinitionDTO(WorkflowDefinition);
            if (WorkflowDefinition.IsValidated)
                return WorkflowDefinition_WorkflowDefinitionDTO;
            else
                return BadRequest(WorkflowDefinition_WorkflowDefinitionDTO);
        }

        [Route(WorkflowDefinitionRoute.Create), HttpPost]
        public async Task<ActionResult<WorkflowDefinition_WorkflowDefinitionDTO>> Create([FromBody] WorkflowDefinition_WorkflowDefinitionDTO WorkflowDefinition_WorkflowDefinitionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkflowDefinition_WorkflowDefinitionDTO.Id))
                return Forbid();

            WorkflowDefinition WorkflowDefinition = ConvertDTOToEntity(WorkflowDefinition_WorkflowDefinitionDTO);
            WorkflowDefinition = await WorkflowDefinitionService.Create(WorkflowDefinition);
            WorkflowDefinition_WorkflowDefinitionDTO = new WorkflowDefinition_WorkflowDefinitionDTO(WorkflowDefinition);
            if (WorkflowDefinition.IsValidated)
                return WorkflowDefinition_WorkflowDefinitionDTO;
            else
                return BadRequest(WorkflowDefinition_WorkflowDefinitionDTO);
        }

        [Route(WorkflowDefinitionRoute.Update), HttpPost]
        public async Task<ActionResult<WorkflowDefinition_WorkflowDefinitionDTO>> Update([FromBody] WorkflowDefinition_WorkflowDefinitionDTO WorkflowDefinition_WorkflowDefinitionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkflowDefinition_WorkflowDefinitionDTO.Id))
                return Forbid();

            WorkflowDefinition WorkflowDefinition = ConvertDTOToEntity(WorkflowDefinition_WorkflowDefinitionDTO);
            WorkflowDefinition = await WorkflowDefinitionService.Update(WorkflowDefinition);
            WorkflowDefinition_WorkflowDefinitionDTO = new WorkflowDefinition_WorkflowDefinitionDTO(WorkflowDefinition);
            if (WorkflowDefinition.IsValidated)
                return WorkflowDefinition_WorkflowDefinitionDTO;
            else
                return BadRequest(WorkflowDefinition_WorkflowDefinitionDTO);
        }

        [Route(WorkflowDefinitionRoute.Clone), HttpPost]
        public async Task<ActionResult<WorkflowDefinition_WorkflowDefinitionDTO>> Clone([FromBody] WorkflowDefinition_WorkflowDefinitionDTO WorkflowDefinition_WorkflowDefinitionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkflowDefinition_WorkflowDefinitionDTO.Id))
                return Forbid();

            WorkflowDefinition WorkflowDefinition = await WorkflowDefinitionService.Clone(WorkflowDefinition_WorkflowDefinitionDTO.Id);
            WorkflowDefinition_WorkflowDefinitionDTO = new WorkflowDefinition_WorkflowDefinitionDTO(WorkflowDefinition);
            if (WorkflowDefinition.IsValidated)
                return WorkflowDefinition_WorkflowDefinitionDTO;
            else
                return BadRequest(WorkflowDefinition_WorkflowDefinitionDTO);
        }

        [Route(WorkflowDefinitionRoute.Delete), HttpPost]
        public async Task<ActionResult<WorkflowDefinition_WorkflowDefinitionDTO>> Delete([FromBody] WorkflowDefinition_WorkflowDefinitionDTO WorkflowDefinition_WorkflowDefinitionDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkflowDefinition_WorkflowDefinitionDTO.Id))
                return Forbid();

            WorkflowDefinition WorkflowDefinition = ConvertDTOToEntity(WorkflowDefinition_WorkflowDefinitionDTO);
            WorkflowDefinition = await WorkflowDefinitionService.Delete(WorkflowDefinition);
            WorkflowDefinition_WorkflowDefinitionDTO = new WorkflowDefinition_WorkflowDefinitionDTO(WorkflowDefinition);
            if (WorkflowDefinition.IsValidated)
                return WorkflowDefinition_WorkflowDefinitionDTO;
            else
                return BadRequest(WorkflowDefinition_WorkflowDefinitionDTO);
        }

        [Route(WorkflowDefinitionRoute.Export), HttpPost]
        public async Task<FileResult> Export([FromBody] WorkflowDefinition_WorkflowDefinitionFilterDTO WorkflowDefinition_WorkflowDefinitionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region WorkflowDefinition
                var WorkflowDefinitionFilter = ConvertFilterDTOToFilterEntity(WorkflowDefinition_WorkflowDefinitionFilterDTO);
                WorkflowDefinitionFilter.Skip = 0;
                WorkflowDefinitionFilter.Take = int.MaxValue;
                WorkflowDefinitionFilter = WorkflowDefinitionService.ToFilter(WorkflowDefinitionFilter);
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

                #region WorkflowType
                var WorkflowTypeFilter = new WorkflowTypeFilter();
                WorkflowTypeFilter.Selects = WorkflowTypeSelect.ALL;
                WorkflowTypeFilter.OrderBy = WorkflowTypeOrder.Id;
                WorkflowTypeFilter.OrderType = OrderType.ASC;
                WorkflowTypeFilter.Skip = 0;
                WorkflowTypeFilter.Take = int.MaxValue;
                List<WorkflowType> WorkflowTypes = await WorkflowTypeService.List(WorkflowTypeFilter);

                var WorkflowTypeHeaders = new List<string>()
                {
                    "Id",
                    "Code",
                    "Name",
                };
                List<object[]> WorkflowTypeData = new List<object[]>();
                for (int i = 0; i < WorkflowTypes.Count; i++)
                {
                    var WorkflowType = WorkflowTypes[i];
                    WorkflowTypeData.Add(new Object[]
                    {
                        WorkflowType.Id,
                        WorkflowType.Code,
                        WorkflowType.Name,
                    });
                }
                excel.GenerateWorksheet("WorkflowType", WorkflowTypeHeaders, WorkflowTypeData);
                #endregion
                #region WorkflowDirection
                var WorkflowDirectionFilter = new WorkflowDirectionFilter();
                WorkflowDirectionFilter.Selects = WorkflowDirectionSelect.ALL;
                WorkflowDirectionFilter.OrderBy = WorkflowDirectionOrder.Id;
                WorkflowDirectionFilter.OrderType = OrderType.ASC;
                WorkflowDirectionFilter.Skip = 0;
                WorkflowDirectionFilter.Take = int.MaxValue;
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
                #region WorkflowParameter
                var WorkflowParameterFilter = new WorkflowParameterFilter();
                WorkflowParameterFilter.Selects = WorkflowParameterSelect.ALL;
                WorkflowParameterFilter.OrderBy = WorkflowParameterOrder.Id;
                WorkflowParameterFilter.OrderType = OrderType.ASC;
                WorkflowParameterFilter.Skip = 0;
                WorkflowParameterFilter.Take = int.MaxValue;
                List<WorkflowParameter> WorkflowParameters = await WorkflowParameterService.List(WorkflowParameterFilter);

                var WorkflowParameterHeaders = new List<string>()
                {
                        "Id",
                        "WorkflowDefinitionId",
                        "Name",
                };
                List<object[]> WorkflowParameterData = new List<object[]>();
                for (int i = 0; i < WorkflowParameters.Count; i++)
                {
                    var WorkflowParameter = WorkflowParameters[i];
                    WorkflowParameterData.Add(new Object[]
                    {
                        WorkflowParameter.Id,
                        WorkflowParameter.WorkflowTypeId,
                        WorkflowParameter.Name,
                    });
                }
                excel.GenerateWorksheet("WorkflowParameter", WorkflowParameterHeaders, WorkflowParameterData);
                #endregion
                #region Role
                var RoleFilter = new RoleFilter();
                RoleFilter.Selects = RoleSelect.ALL;
                RoleFilter.OrderBy = RoleOrder.Id;
                RoleFilter.OrderType = OrderType.ASC;
                RoleFilter.Skip = 0;
                RoleFilter.Take = int.MaxValue;
                List<Role> Roles = await RoleService.List(RoleFilter);

                var RoleHeaders = new List<string>()
                {
                    "Id",
                    "Code",
                    "Name",
                    "StatusId",
                };
                List<object[]> RoleData = new List<object[]>();
                for (int i = 0; i < Roles.Count; i++)
                {
                    var Role = Roles[i];
                    RoleData.Add(new Object[]
                    {
                        Role.Id,
                        Role.Code,
                        Role.Name,
                        Role.StatusId,
                    });
                }
                excel.GenerateWorksheet("Role", RoleHeaders, RoleData);
                #endregion
                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "WorkflowDefinition.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            WorkflowDefinitionFilter WorkflowDefinitionFilter = new WorkflowDefinitionFilter();
            WorkflowDefinitionFilter = WorkflowDefinitionService.ToFilter(WorkflowDefinitionFilter);
            if (Id == 0)
            {

            }
            else
            {
                WorkflowDefinitionFilter.Id = new IdFilter { Equal = Id };
                int count = await WorkflowDefinitionService.Count(WorkflowDefinitionFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private WorkflowDefinition ConvertDTOToEntity(WorkflowDefinition_WorkflowDefinitionDTO WorkflowDefinition_WorkflowDefinitionDTO)
        {
            WorkflowDefinition_WorkflowDefinitionDTO.TrimString();
            WorkflowDefinition WorkflowDefinition = new WorkflowDefinition();
            WorkflowDefinition.Id = WorkflowDefinition_WorkflowDefinitionDTO.Id;
            WorkflowDefinition.Code = WorkflowDefinition_WorkflowDefinitionDTO.Code;
            WorkflowDefinition.Name = WorkflowDefinition_WorkflowDefinitionDTO.Name;
            WorkflowDefinition.Used = WorkflowDefinition_WorkflowDefinitionDTO.Used;
            WorkflowDefinition.CreatorId = WorkflowDefinition_WorkflowDefinitionDTO.CreatorId;
            WorkflowDefinition.ModifierId = WorkflowDefinition_WorkflowDefinitionDTO.ModifierId;
            WorkflowDefinition.WorkflowTypeId = WorkflowDefinition_WorkflowDefinitionDTO.WorkflowTypeId;
            WorkflowDefinition.StartDate = WorkflowDefinition_WorkflowDefinitionDTO.StartDate;
            WorkflowDefinition.EndDate = WorkflowDefinition_WorkflowDefinitionDTO.EndDate;
            WorkflowDefinition.StatusId = WorkflowDefinition_WorkflowDefinitionDTO.StatusId;
            WorkflowDefinition.OrganizationId = WorkflowDefinition_WorkflowDefinitionDTO.OrganizationId;
            WorkflowDefinition.CreatedAt = WorkflowDefinition_WorkflowDefinitionDTO.CreatedAt;
            WorkflowDefinition.UpdatedAt = WorkflowDefinition_WorkflowDefinitionDTO.UpdatedAt;
            WorkflowDefinition.WorkflowType = WorkflowDefinition_WorkflowDefinitionDTO.WorkflowType == null ? null : new WorkflowType
            {
                Id = WorkflowDefinition_WorkflowDefinitionDTO.WorkflowType.Id,
                Code = WorkflowDefinition_WorkflowDefinitionDTO.WorkflowType.Code,
                Name = WorkflowDefinition_WorkflowDefinitionDTO.WorkflowType.Name,
            };
            WorkflowDefinition.Organization = WorkflowDefinition_WorkflowDefinitionDTO.Organization == null ? null : new Organization
            {
                Id = WorkflowDefinition_WorkflowDefinitionDTO.Organization.Id,
                Code = WorkflowDefinition_WorkflowDefinitionDTO.Organization.Code,
                Name = WorkflowDefinition_WorkflowDefinitionDTO.Organization.Name,
                ParentId = WorkflowDefinition_WorkflowDefinitionDTO.Organization.ParentId,
                Path = WorkflowDefinition_WorkflowDefinitionDTO.Organization.Path,
                Level = WorkflowDefinition_WorkflowDefinitionDTO.Organization.Level,
                StatusId = WorkflowDefinition_WorkflowDefinitionDTO.Organization.StatusId,
                Phone = WorkflowDefinition_WorkflowDefinitionDTO.Organization.Phone,
                Address = WorkflowDefinition_WorkflowDefinitionDTO.Organization.Address,
                Email = WorkflowDefinition_WorkflowDefinitionDTO.Organization.Email,
            };
            WorkflowDefinition.WorkflowDirections = WorkflowDefinition_WorkflowDefinitionDTO.WorkflowDirections?
                .Select(x => new WorkflowDirection
                {
                    Id = x.Id,
                    FromStepId = x.FromStepId,
                    ToStepId = x.ToStepId,
                    SubjectMailForCreator = x.SubjectMailForCreator,
                    SubjectMailForNextStep = x.SubjectMailForNextStep,
                    BodyMailForCreator = x.BodyMailForCreator,
                    BodyMailForNextStep = x.BodyMailForNextStep,
                    UpdatedAt = x.UpdatedAt,
                    FromStep = x.FromStep == null ? null : new WorkflowStep
                    {
                        Id = x.FromStep.Id,
                        WorkflowDefinitionId = x.FromStep.WorkflowDefinitionId,
                        Code = x.FromStep.Code,
                        Name = x.FromStep.Name,
                        RoleId = x.FromStep.RoleId,
                        SubjectMailForReject = x.FromStep.SubjectMailForReject,
                        BodyMailForReject = x.FromStep.BodyMailForReject,
                    },
                    ToStep = x.ToStep == null ? null : new WorkflowStep
                    {
                        Id = x.ToStep.Id,
                        WorkflowDefinitionId = x.ToStep.WorkflowDefinitionId,
                        Code = x.ToStep.Code,
                        Name = x.ToStep.Name,
                        RoleId = x.ToStep.RoleId,
                        SubjectMailForReject = x.ToStep.SubjectMailForReject,
                        BodyMailForReject = x.ToStep.BodyMailForReject,
                    },
                }).ToList();
            WorkflowDefinition.WorkflowParameters = WorkflowDefinition_WorkflowDefinitionDTO.WorkflowParameters?
                .Select(x => new WorkflowParameter
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                }).ToList();
            WorkflowDefinition.WorkflowSteps = WorkflowDefinition_WorkflowDefinitionDTO.WorkflowSteps?
                .Select(x => new WorkflowStep
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name,
                    RoleId = x.RoleId,
                    SubjectMailForReject = x.SubjectMailForReject,
                    BodyMailForReject = x.BodyMailForReject,
                    Role = x.Role == null ? null : new Role
                    {
                        Id = x.Role.Id,
                        Code = x.Role.Code,
                        Name = x.Role.Name,
                        StatusId = x.Role.StatusId,
                    },
                }).ToList();
            WorkflowDefinition.BaseLanguage = CurrentContext.Language;
            return WorkflowDefinition;
        }

        private WorkflowDefinitionFilter ConvertFilterDTOToFilterEntity(WorkflowDefinition_WorkflowDefinitionFilterDTO WorkflowDefinition_WorkflowDefinitionFilterDTO)
        {
            WorkflowDefinitionFilter WorkflowDefinitionFilter = new WorkflowDefinitionFilter();
            WorkflowDefinitionFilter.Selects = WorkflowDefinitionSelect.ALL;
            WorkflowDefinitionFilter.Skip = WorkflowDefinition_WorkflowDefinitionFilterDTO.Skip;
            WorkflowDefinitionFilter.Take = WorkflowDefinition_WorkflowDefinitionFilterDTO.Take;
            WorkflowDefinitionFilter.OrderBy = WorkflowDefinition_WorkflowDefinitionFilterDTO.OrderBy;
            WorkflowDefinitionFilter.OrderType = WorkflowDefinition_WorkflowDefinitionFilterDTO.OrderType;

            WorkflowDefinitionFilter.Id = WorkflowDefinition_WorkflowDefinitionFilterDTO.Id;
            WorkflowDefinitionFilter.Code = WorkflowDefinition_WorkflowDefinitionFilterDTO.Code;
            WorkflowDefinitionFilter.Name = WorkflowDefinition_WorkflowDefinitionFilterDTO.Name;
            WorkflowDefinitionFilter.CreatorId = WorkflowDefinition_WorkflowDefinitionFilterDTO.CreatorId;
            WorkflowDefinitionFilter.ModifierId = WorkflowDefinition_WorkflowDefinitionFilterDTO.ModifierId;
            WorkflowDefinitionFilter.WorkflowTypeId = WorkflowDefinition_WorkflowDefinitionFilterDTO.WorkflowTypeId;
            WorkflowDefinitionFilter.StartDate = WorkflowDefinition_WorkflowDefinitionFilterDTO.StartDate;
            WorkflowDefinitionFilter.EndDate = WorkflowDefinition_WorkflowDefinitionFilterDTO.EndDate;
            WorkflowDefinitionFilter.StatusId = WorkflowDefinition_WorkflowDefinitionFilterDTO.StatusId;
            WorkflowDefinitionFilter.OrganizationId = WorkflowDefinition_WorkflowDefinitionFilterDTO.OrganizationId;
            WorkflowDefinitionFilter.CreatedAt = WorkflowDefinition_WorkflowDefinitionFilterDTO.CreatedAt;
            WorkflowDefinitionFilter.UpdatedAt = WorkflowDefinition_WorkflowDefinitionFilterDTO.UpdatedAt;
            return WorkflowDefinitionFilter;
        }

        [Route(WorkflowDefinitionRoute.FilterListAppUser), HttpPost]
        public async Task<List<WorkflowDefinition_AppUserDTO>> FilterListAppUser([FromBody] WorkflowDefinition_AppUserFilterDTO WorkflowDefinition_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = WorkflowDefinition_AppUserFilterDTO.Id;
            AppUserFilter.Username = WorkflowDefinition_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = WorkflowDefinition_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = WorkflowDefinition_AppUserFilterDTO.Address;
            AppUserFilter.Email = WorkflowDefinition_AppUserFilterDTO.Email;
            AppUserFilter.Phone = WorkflowDefinition_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = WorkflowDefinition_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = WorkflowDefinition_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = WorkflowDefinition_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = WorkflowDefinition_AppUserFilterDTO.StatusId;
            AppUserFilter.ProvinceId = WorkflowDefinition_AppUserFilterDTO.ProvinceId;
            AppUserFilter.SexId = WorkflowDefinition_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = WorkflowDefinition_AppUserFilterDTO.Birthday;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<WorkflowDefinition_AppUserDTO> WorkflowDefinition_AppUserDTOs = AppUsers
                .Select(x => new WorkflowDefinition_AppUserDTO(x)).ToList();
            return WorkflowDefinition_AppUserDTOs;
        }

        [Route(WorkflowDefinitionRoute.FilterListWorkflowType), HttpPost]
        public async Task<List<WorkflowDefinition_WorkflowTypeDTO>> FilterListWorkflowType([FromBody] WorkflowDefinition_WorkflowTypeFilterDTO WorkflowDefinition_WorkflowTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowTypeFilter WorkflowTypeFilter = new WorkflowTypeFilter();
            WorkflowTypeFilter.Skip = 0;
            WorkflowTypeFilter.Take = int.MaxValue;
            WorkflowTypeFilter.Take = 20;
            WorkflowTypeFilter.OrderBy = WorkflowTypeOrder.Id;
            WorkflowTypeFilter.OrderType = OrderType.ASC;
            WorkflowTypeFilter.Selects = WorkflowTypeSelect.ALL;

            List<WorkflowType> WorkflowTypes = await WorkflowTypeService.List(WorkflowTypeFilter);
            List<WorkflowDefinition_WorkflowTypeDTO> WorkflowDefinition_WorkflowTypeDTOs = WorkflowTypes
                .Select(x => new WorkflowDefinition_WorkflowTypeDTO(x)).ToList();
            return WorkflowDefinition_WorkflowTypeDTOs;
        }
        [Route(WorkflowDefinitionRoute.FilterListWorkflowDirection), HttpPost]
        public async Task<List<WorkflowDefinition_WorkflowDirectionDTO>> FilterListWorkflowDirection([FromBody] WorkflowDefinition_WorkflowDirectionFilterDTO WorkflowDefinition_WorkflowDirectionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDirectionFilter WorkflowDirectionFilter = new WorkflowDirectionFilter();
            WorkflowDirectionFilter.Skip = 0;
            WorkflowDirectionFilter.Take = 20;
            WorkflowDirectionFilter.OrderBy = WorkflowDirectionOrder.Id;
            WorkflowDirectionFilter.OrderType = OrderType.ASC;
            WorkflowDirectionFilter.Selects = WorkflowDirectionSelect.ALL;
            WorkflowDirectionFilter.Id = WorkflowDefinition_WorkflowDirectionFilterDTO.Id;
            WorkflowDirectionFilter.WorkflowDefinitionId = WorkflowDefinition_WorkflowDirectionFilterDTO.WorkflowDefinitionId;
            WorkflowDirectionFilter.FromStepId = WorkflowDefinition_WorkflowDirectionFilterDTO.FromStepId;
            WorkflowDirectionFilter.ToStepId = WorkflowDefinition_WorkflowDirectionFilterDTO.ToStepId;

            List<WorkflowDirection> WorkflowDirections = await WorkflowDirectionService.List(WorkflowDirectionFilter);
            List<WorkflowDefinition_WorkflowDirectionDTO> WorkflowDefinition_WorkflowDirectionDTOs = WorkflowDirections
                .Select(x => new WorkflowDefinition_WorkflowDirectionDTO(x)).ToList();
            return WorkflowDefinition_WorkflowDirectionDTOs;
        }
        [Route(WorkflowDefinitionRoute.FilterListWorkflowStep), HttpPost]
        public async Task<List<WorkflowDefinition_WorkflowStepDTO>> FilterListWorkflowStep([FromBody] WorkflowDefinition_WorkflowStepFilterDTO WorkflowDefinition_WorkflowStepFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowStepFilter WorkflowStepFilter = new WorkflowStepFilter();
            WorkflowStepFilter.Skip = 0;
            WorkflowStepFilter.Take = 20;
            WorkflowStepFilter.OrderBy = WorkflowStepOrder.Id;
            WorkflowStepFilter.OrderType = OrderType.ASC;
            WorkflowStepFilter.Selects = WorkflowStepSelect.ALL;
            WorkflowStepFilter.Id = WorkflowDefinition_WorkflowStepFilterDTO.Id;
            WorkflowStepFilter.WorkflowDefinitionId = WorkflowDefinition_WorkflowStepFilterDTO.WorkflowDefinitionId;
            WorkflowStepFilter.Code = WorkflowDefinition_WorkflowStepFilterDTO.Code;
            WorkflowStepFilter.Name = WorkflowDefinition_WorkflowStepFilterDTO.Name;
            WorkflowStepFilter.RoleId = WorkflowDefinition_WorkflowStepFilterDTO.RoleId;

            List<WorkflowStep> WorkflowSteps = await WorkflowStepService.List(WorkflowStepFilter);
            List<WorkflowDefinition_WorkflowStepDTO> WorkflowDefinition_WorkflowStepDTOs = WorkflowSteps
                .Select(x => new WorkflowDefinition_WorkflowStepDTO(x)).ToList();
            return WorkflowDefinition_WorkflowStepDTOs;
        }
        [Route(WorkflowDefinitionRoute.FilterListWorkflowParameter), HttpPost]
        public async Task<List<WorkflowDefinition_WorkflowParameterDTO>> FilterListWorkflowParameter([FromBody] WorkflowDefinition_WorkflowParameterFilterDTO WorkflowDefinition_WorkflowParameterFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowParameterFilter WorkflowParameterFilter = new WorkflowParameterFilter();
            WorkflowParameterFilter.Skip = 0;
            WorkflowParameterFilter.Take = 20;
            WorkflowParameterFilter.OrderBy = WorkflowParameterOrder.Id;
            WorkflowParameterFilter.OrderType = OrderType.ASC;
            WorkflowParameterFilter.Selects = WorkflowParameterSelect.ALL;
            WorkflowParameterFilter.Id = WorkflowDefinition_WorkflowParameterFilterDTO.Id;
            WorkflowParameterFilter.WorkflowTypeId = WorkflowDefinition_WorkflowParameterFilterDTO.WorkflowDefinitionId;
            WorkflowParameterFilter.Code = WorkflowDefinition_WorkflowParameterFilterDTO.Code;
            WorkflowParameterFilter.Name = WorkflowDefinition_WorkflowParameterFilterDTO.Name;

            List<WorkflowParameter> WorkflowParameters = await WorkflowParameterService.List(WorkflowParameterFilter);
            List<WorkflowDefinition_WorkflowParameterDTO> WorkflowDefinition_WorkflowParameterDTOs = WorkflowParameters
                .Select(x => new WorkflowDefinition_WorkflowParameterDTO(x)).ToList();
            return WorkflowDefinition_WorkflowParameterDTOs;
        }
        [Route(WorkflowDefinitionRoute.FilterListRole), HttpPost]
        public async Task<List<WorkflowDefinition_RoleDTO>> FilterListRole([FromBody] WorkflowDefinition_RoleFilterDTO WorkflowDefinition_RoleFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RoleFilter RoleFilter = new RoleFilter();
            RoleFilter.Skip = 0;
            RoleFilter.Take = 20;
            RoleFilter.OrderBy = RoleOrder.Id;
            RoleFilter.OrderType = OrderType.ASC;
            RoleFilter.Selects = RoleSelect.ALL;
            RoleFilter.Id = WorkflowDefinition_RoleFilterDTO.Id;
            RoleFilter.Code = WorkflowDefinition_RoleFilterDTO.Code;
            RoleFilter.Name = WorkflowDefinition_RoleFilterDTO.Name;
            RoleFilter.StatusId = WorkflowDefinition_RoleFilterDTO.StatusId;

            List<Role> Roles = await RoleService.List(RoleFilter);
            List<WorkflowDefinition_RoleDTO> WorkflowDefinition_RoleDTOs = Roles
                .Select(x => new WorkflowDefinition_RoleDTO(x)).ToList();
            return WorkflowDefinition_RoleDTOs;
        }
        [Route(WorkflowDefinitionRoute.FilterListOrganization), HttpPost]
        public async Task<List<WorkflowDefinition_OrganizationDTO>> FilterListOrganization([FromBody] WorkflowDefinition_OrganizationFilterDTO WorkflowDefinition_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = WorkflowDefinition_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = WorkflowDefinition_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = WorkflowDefinition_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = WorkflowDefinition_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = WorkflowDefinition_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = WorkflowDefinition_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = WorkflowDefinition_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = WorkflowDefinition_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = WorkflowDefinition_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = WorkflowDefinition_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.OrFilter == null) OrganizationFilter.OrFilter = new List<OrganizationFilter>();
            if (CurrentContext.Filters != null)
            {
                foreach (var currentFilter in CurrentContext.Filters)
                {
                    OrganizationFilter subFilter = new OrganizationFilter();
                    OrganizationFilter.OrFilter.Add(subFilter);
                    List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                    foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                    {
                        if (FilterPermissionDefinition.Name == nameof(AppUserFilter.OrganizationId))
                            subFilter.Id = FilterPermissionDefinition.IdFilter;
                    }
                }
            }

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<WorkflowDefinition_OrganizationDTO> WorkflowDefinition_OrganizationDTOs = Organizations
                .Select(x => new WorkflowDefinition_OrganizationDTO(x)).ToList();
            return WorkflowDefinition_OrganizationDTOs;
        }


        [Route(WorkflowDefinitionRoute.SingleListAppUser), HttpPost]
        public async Task<List<WorkflowDefinition_AppUserDTO>> SingleListAppUser([FromBody] WorkflowDefinition_AppUserFilterDTO WorkflowDefinition_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = WorkflowDefinition_AppUserFilterDTO.Id;
            AppUserFilter.Username = WorkflowDefinition_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = WorkflowDefinition_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = WorkflowDefinition_AppUserFilterDTO.Address;
            AppUserFilter.Email = WorkflowDefinition_AppUserFilterDTO.Email;
            AppUserFilter.Phone = WorkflowDefinition_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = WorkflowDefinition_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = WorkflowDefinition_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = WorkflowDefinition_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            AppUserFilter.ProvinceId = WorkflowDefinition_AppUserFilterDTO.ProvinceId;
            AppUserFilter.SexId = WorkflowDefinition_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = WorkflowDefinition_AppUserFilterDTO.Birthday;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<WorkflowDefinition_AppUserDTO> WorkflowDefinition_AppUserDTOs = AppUsers
                .Select(x => new WorkflowDefinition_AppUserDTO(x)).ToList();
            return WorkflowDefinition_AppUserDTOs;
        }

        [Route(WorkflowDefinitionRoute.SingleListWorkflowType), HttpPost]
        public async Task<List<WorkflowDefinition_WorkflowTypeDTO>> SingleListWorkflowType([FromBody] WorkflowDefinition_WorkflowTypeFilterDTO WorkflowDefinition_WorkflowTypeFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowTypeFilter WorkflowTypeFilter = new WorkflowTypeFilter();
            WorkflowTypeFilter.Skip = 0;
            WorkflowTypeFilter.Take = int.MaxValue;
            WorkflowTypeFilter.Take = 20;
            WorkflowTypeFilter.OrderBy = WorkflowTypeOrder.Id;
            WorkflowTypeFilter.OrderType = OrderType.ASC;
            WorkflowTypeFilter.Selects = WorkflowTypeSelect.ALL;

            List<WorkflowType> WorkflowTypes = await WorkflowTypeService.List(WorkflowTypeFilter);
            List<WorkflowDefinition_WorkflowTypeDTO> WorkflowDefinition_WorkflowTypeDTOs = WorkflowTypes
                .Select(x => new WorkflowDefinition_WorkflowTypeDTO(x)).ToList();
            return WorkflowDefinition_WorkflowTypeDTOs;
        }
        [Route(WorkflowDefinitionRoute.SingleListWorkflowDirection), HttpPost]
        public async Task<List<WorkflowDefinition_WorkflowDirectionDTO>> SingleListWorkflowDirection([FromBody] WorkflowDefinition_WorkflowDirectionFilterDTO WorkflowDefinition_WorkflowDirectionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (WorkflowDefinition_WorkflowDirectionFilterDTO.WorkflowDefinitionId.HasValue == false)
                return new List<WorkflowDefinition_WorkflowDirectionDTO>();

            WorkflowDirectionFilter WorkflowDirectionFilter = new WorkflowDirectionFilter();
            WorkflowDirectionFilter.Skip = 0;
            WorkflowDirectionFilter.Take = 20;
            WorkflowDirectionFilter.OrderBy = WorkflowDirectionOrder.Id;
            WorkflowDirectionFilter.OrderType = OrderType.ASC;
            WorkflowDirectionFilter.Selects = WorkflowDirectionSelect.ALL;
            WorkflowDirectionFilter.Id = WorkflowDefinition_WorkflowDirectionFilterDTO.Id;
            WorkflowDirectionFilter.WorkflowDefinitionId = WorkflowDefinition_WorkflowDirectionFilterDTO.WorkflowDefinitionId;
            WorkflowDirectionFilter.FromStepId = WorkflowDefinition_WorkflowDirectionFilterDTO.FromStepId;
            WorkflowDirectionFilter.ToStepId = WorkflowDefinition_WorkflowDirectionFilterDTO.ToStepId;

            List<WorkflowDirection> WorkflowDirections = await WorkflowDirectionService.List(WorkflowDirectionFilter);
            List<WorkflowDefinition_WorkflowDirectionDTO> WorkflowDefinition_WorkflowDirectionDTOs = WorkflowDirections
                .Select(x => new WorkflowDefinition_WorkflowDirectionDTO(x)).ToList();
            return WorkflowDefinition_WorkflowDirectionDTOs;
        }
        [Route(WorkflowDefinitionRoute.SingleListWorkflowStep), HttpPost]
        public async Task<List<WorkflowDefinition_WorkflowStepDTO>> SingleListWorkflowStep([FromBody] WorkflowDefinition_WorkflowStepFilterDTO WorkflowDefinition_WorkflowStepFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!WorkflowDefinition_WorkflowStepFilterDTO.WorkflowDefinitionId.Equal.HasValue)
                return new List<WorkflowDefinition_WorkflowStepDTO>();

            WorkflowStepFilter WorkflowStepFilter = new WorkflowStepFilter();
            WorkflowStepFilter.Skip = 0;
            WorkflowStepFilter.Take = 20;
            WorkflowStepFilter.OrderBy = WorkflowStepOrder.Id;
            WorkflowStepFilter.OrderType = OrderType.ASC;
            WorkflowStepFilter.Selects = WorkflowStepSelect.ALL;
            WorkflowStepFilter.Id = WorkflowDefinition_WorkflowStepFilterDTO.Id;
            WorkflowStepFilter.WorkflowDefinitionId = WorkflowDefinition_WorkflowStepFilterDTO.WorkflowDefinitionId;
            WorkflowStepFilter.Code = WorkflowDefinition_WorkflowStepFilterDTO.Code;
            WorkflowStepFilter.Name = WorkflowDefinition_WorkflowStepFilterDTO.Name;
            WorkflowStepFilter.RoleId = WorkflowDefinition_WorkflowStepFilterDTO.RoleId;

            List<WorkflowStep> WorkflowSteps = await WorkflowStepService.List(WorkflowStepFilter);
            List<WorkflowDefinition_WorkflowStepDTO> WorkflowDefinition_WorkflowStepDTOs = WorkflowSteps
                .Select(x => new WorkflowDefinition_WorkflowStepDTO(x)).ToList();
            return WorkflowDefinition_WorkflowStepDTOs;
        }
        [Route(WorkflowDefinitionRoute.SingleListWorkflowParameter), HttpPost]
        public async Task<List<WorkflowDefinition_WorkflowParameterDTO>> SingleListWorkflowParameter([FromBody] WorkflowDefinition_WorkflowParameterFilterDTO WorkflowDefinition_WorkflowParameterFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowParameterFilter WorkflowParameterFilter = new WorkflowParameterFilter();
            WorkflowParameterFilter.Skip = 0;
            WorkflowParameterFilter.Take = 20;
            WorkflowParameterFilter.OrderBy = WorkflowParameterOrder.Id;
            WorkflowParameterFilter.OrderType = OrderType.ASC;
            WorkflowParameterFilter.Selects = WorkflowParameterSelect.ALL;
            WorkflowParameterFilter.Id = WorkflowDefinition_WorkflowParameterFilterDTO.Id;
            WorkflowParameterFilter.WorkflowTypeId = WorkflowDefinition_WorkflowParameterFilterDTO.WorkflowDefinitionId;
            WorkflowParameterFilter.Code = WorkflowDefinition_WorkflowParameterFilterDTO.Code;
            WorkflowParameterFilter.Name = WorkflowDefinition_WorkflowParameterFilterDTO.Name;

            List<WorkflowParameter> WorkflowParameters = await WorkflowParameterService.List(WorkflowParameterFilter);
            List<WorkflowDefinition_WorkflowParameterDTO> WorkflowDefinition_WorkflowParameterDTOs = WorkflowParameters
                .Select(x => new WorkflowDefinition_WorkflowParameterDTO(x)).ToList();
            return WorkflowDefinition_WorkflowParameterDTOs;
        }
        [Route(WorkflowDefinitionRoute.SingleListRole), HttpPost]
        public async Task<List<WorkflowDefinition_RoleDTO>> SingleListRole([FromBody] WorkflowDefinition_RoleFilterDTO WorkflowDefinition_RoleFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RoleFilter RoleFilter = new RoleFilter();
            RoleFilter.Skip = 0;
            RoleFilter.Take = 20;
            RoleFilter.OrderBy = RoleOrder.Id;
            RoleFilter.OrderType = OrderType.ASC;
            RoleFilter.Selects = RoleSelect.ALL;
            RoleFilter.Id = WorkflowDefinition_RoleFilterDTO.Id;
            RoleFilter.Code = WorkflowDefinition_RoleFilterDTO.Code;
            RoleFilter.Name = WorkflowDefinition_RoleFilterDTO.Name;
            RoleFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<Role> Roles = await RoleService.List(RoleFilter);
            List<WorkflowDefinition_RoleDTO> WorkflowDefinition_RoleDTOs = Roles
                .Select(x => new WorkflowDefinition_RoleDTO(x)).ToList();
            return WorkflowDefinition_RoleDTOs;
        }

        [Route(WorkflowDefinitionRoute.SingleListStatus), HttpPost]
        public async Task<List<WorkflowDefinition_StatusDTO>> SingleListStatus()
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<WorkflowDefinition_StatusDTO> WorkflowDefinition_StatusDTOs = Statuses
                .Select(x => new WorkflowDefinition_StatusDTO(x)).ToList();
            return WorkflowDefinition_StatusDTOs;
        }

        [Route(WorkflowDefinitionRoute.CountDirection), HttpPost]
        public async Task<long> CountDirection([FromBody] WorkflowDefinition_WorkflowDirectionFilterDTO WorkflowDefinition_WorkflowDirectionFilterDTO)
        {
            WorkflowDirectionFilter WorkflowDirectionFilter = new WorkflowDirectionFilter();
            WorkflowDirectionFilter.FromStepId = WorkflowDefinition_WorkflowDirectionFilterDTO.FromStepId;
            WorkflowDirectionFilter.Id = WorkflowDefinition_WorkflowDirectionFilterDTO.Id;
            WorkflowDirectionFilter.ToStepId = WorkflowDefinition_WorkflowDirectionFilterDTO.ToStepId;
            WorkflowDirectionFilter.UpdatedAt = WorkflowDefinition_WorkflowDirectionFilterDTO.UpdatedAt;
            WorkflowDirectionFilter.WorkflowDefinitionId = WorkflowDefinition_WorkflowDirectionFilterDTO.WorkflowDefinitionId;

            long count = await WorkflowDirectionService.Count(WorkflowDirectionFilter);
            return count;
        }

        [Route(WorkflowDefinitionRoute.ListDirection), HttpPost]
        public async Task<List<WorkflowDefinition_WorkflowDirectionDTO>> ListDirection([FromBody] WorkflowDefinition_WorkflowDirectionFilterDTO WorkflowDefinition_WorkflowDirectionFilterDTO)
        {
            WorkflowDirectionFilter WorkflowDirectionFilter = new WorkflowDirectionFilter();
            WorkflowDirectionFilter.Skip = 0;
            WorkflowDirectionFilter.Take = 20;
            WorkflowDirectionFilter.OrderBy = WorkflowDirectionOrder.Id;
            WorkflowDirectionFilter.OrderType = OrderType.ASC;
            WorkflowDirectionFilter.Selects = WorkflowDirectionSelect.ALL;
            WorkflowDirectionFilter.FromStepId = WorkflowDefinition_WorkflowDirectionFilterDTO.FromStepId;
            WorkflowDirectionFilter.Id = WorkflowDefinition_WorkflowDirectionFilterDTO.Id;
            WorkflowDirectionFilter.ToStepId = WorkflowDefinition_WorkflowDirectionFilterDTO.ToStepId;
            WorkflowDirectionFilter.UpdatedAt = WorkflowDefinition_WorkflowDirectionFilterDTO.UpdatedAt;
            WorkflowDirectionFilter.WorkflowDefinitionId = WorkflowDefinition_WorkflowDirectionFilterDTO.WorkflowDefinitionId;

            List<WorkflowDirection> WorkflowDirections = await WorkflowDirectionService.List(WorkflowDirectionFilter);
            List<WorkflowDefinition_WorkflowDirectionDTO> WorkflowDefinition_WorkflowDirectionDTOs = WorkflowDirections
                .Select(x => new WorkflowDefinition_WorkflowDirectionDTO(x)).ToList();
            return WorkflowDefinition_WorkflowDirectionDTOs;
        }
        [Route(WorkflowDefinitionRoute.FilterListStatus), HttpPost]
        public async Task<List<WorkflowDefinition_StatusDTO>> FilterListStatus()
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<WorkflowDefinition_StatusDTO> WorkflowDefinition_StatusDTOs = Statuses
                .Select(x => new WorkflowDefinition_StatusDTO(x)).ToList();
            return WorkflowDefinition_StatusDTOs;
        }
        [Route(WorkflowDefinitionRoute.SingleListOrganization), HttpPost]
        public async Task<List<WorkflowDefinition_OrganizationDTO>> SingleListOrganization([FromBody] WorkflowDefinition_OrganizationFilterDTO WorkflowDefinition_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = WorkflowDefinition_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = WorkflowDefinition_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = WorkflowDefinition_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = WorkflowDefinition_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = WorkflowDefinition_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = WorkflowDefinition_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = WorkflowDefinition_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = WorkflowDefinition_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = WorkflowDefinition_OrganizationFilterDTO.Email;
            OrganizationFilter.IsDisplay = true;

            if (OrganizationFilter.OrFilter == null) OrganizationFilter.OrFilter = new List<OrganizationFilter>();
            if (CurrentContext.Filters != null)
            {
                foreach (var currentFilter in CurrentContext.Filters)
                {
                    OrganizationFilter subFilter = new OrganizationFilter();
                    OrganizationFilter.OrFilter.Add(subFilter);
                    List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                    foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                    {
                        if (FilterPermissionDefinition.Name == nameof(AppUserFilter.OrganizationId))
                            subFilter.Id = FilterPermissionDefinition.IdFilter;
                    }
                }
            }

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<WorkflowDefinition_OrganizationDTO> WorkflowDefinition_OrganizationDTOs = Organizations
                .Select(x => new WorkflowDefinition_OrganizationDTO(x)).ToList();
            return WorkflowDefinition_OrganizationDTOs;
        }

    }
}

