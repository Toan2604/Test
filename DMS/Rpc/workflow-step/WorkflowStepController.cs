using DMS.Common;
using DMS.Entities;
using DMS.Services.MAppUser;
using DMS.Services.MRole;
using DMS.Services.MStatus;
using DMS.Services.MWorkflow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.workflow_step
{
    public class WorkflowStepController : RpcController
    {
        private IAppUserService AppUserService;
        private IRoleService RoleService;
        private IWorkflowDefinitionService WorkflowDefinitionService;
        private IWorkflowStepService WorkflowStepService;
        private IWorkflowParameterService WorkflowParameterService;
        private ICurrentContext CurrentContext;
        private IStatusService StatusService;
        public WorkflowStepController(
            IAppUserService AppUserService,
            IRoleService RoleService,
            IWorkflowDefinitionService WorkflowDefinitionService,
            IWorkflowStepService WorkflowStepService,
            IWorkflowParameterService WorkflowParameterService,
            IStatusService StatusService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.RoleService = RoleService;
            this.StatusService = StatusService;
            this.WorkflowDefinitionService = WorkflowDefinitionService;
            this.WorkflowStepService = WorkflowStepService;
            this.WorkflowParameterService = WorkflowParameterService;
            this.CurrentContext = CurrentContext;
        }

        [Route(WorkflowStepRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] WorkflowStep_WorkflowStepFilterDTO WorkflowStep_WorkflowStepFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowStepFilter WorkflowStepFilter = ConvertFilterDTOToFilterEntity(WorkflowStep_WorkflowStepFilterDTO);
            WorkflowStepFilter = WorkflowStepService.ToFilter(WorkflowStepFilter);
            int count = await WorkflowStepService.Count(WorkflowStepFilter);
            return count;
        }

        [Route(WorkflowStepRoute.List), HttpPost]
        public async Task<ActionResult<List<WorkflowStep_WorkflowStepDTO>>> List([FromBody] WorkflowStep_WorkflowStepFilterDTO WorkflowStep_WorkflowStepFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowStepFilter WorkflowStepFilter = ConvertFilterDTOToFilterEntity(WorkflowStep_WorkflowStepFilterDTO);
            WorkflowStepFilter = WorkflowStepService.ToFilter(WorkflowStepFilter);
            List<WorkflowStep> WorkflowSteps = await WorkflowStepService.List(WorkflowStepFilter);
            List<WorkflowStep_WorkflowStepDTO> WorkflowStep_WorkflowStepDTOs = WorkflowSteps
                .Select(c => new WorkflowStep_WorkflowStepDTO(c)).ToList();
            return WorkflowStep_WorkflowStepDTOs;
        }

        [Route(WorkflowStepRoute.Get), HttpPost]
        public async Task<ActionResult<WorkflowStep_WorkflowStepDTO>> Get([FromBody] WorkflowStep_WorkflowStepDTO WorkflowStep_WorkflowStepDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkflowStep_WorkflowStepDTO.Id))
                return Forbid();

            WorkflowStep WorkflowStep = await WorkflowStepService.Get(WorkflowStep_WorkflowStepDTO.Id);
            return new WorkflowStep_WorkflowStepDTO(WorkflowStep);
        }

        [Route(WorkflowStepRoute.Create), HttpPost]
        public async Task<ActionResult<WorkflowStep_WorkflowStepDTO>> Create([FromBody] WorkflowStep_WorkflowStepDTO WorkflowStep_WorkflowStepDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkflowStep_WorkflowStepDTO.Id))
                return Forbid();

            WorkflowStep WorkflowStep = ConvertDTOToEntity(WorkflowStep_WorkflowStepDTO);
            WorkflowStep = await WorkflowStepService.Create(WorkflowStep);
            WorkflowStep_WorkflowStepDTO = new WorkflowStep_WorkflowStepDTO(WorkflowStep);
            if (WorkflowStep.IsValidated)
                return WorkflowStep_WorkflowStepDTO;
            else
                return BadRequest(WorkflowStep_WorkflowStepDTO);
        }

        [Route(WorkflowStepRoute.Update), HttpPost]
        public async Task<ActionResult<WorkflowStep_WorkflowStepDTO>> Update([FromBody] WorkflowStep_WorkflowStepDTO WorkflowStep_WorkflowStepDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkflowStep_WorkflowStepDTO.Id))
                return Forbid();

            WorkflowStep WorkflowStep = ConvertDTOToEntity(WorkflowStep_WorkflowStepDTO);
            WorkflowStep = await WorkflowStepService.Update(WorkflowStep);
            WorkflowStep_WorkflowStepDTO = new WorkflowStep_WorkflowStepDTO(WorkflowStep);
            if (WorkflowStep.IsValidated)
                return WorkflowStep_WorkflowStepDTO;
            else
                return BadRequest(WorkflowStep_WorkflowStepDTO);
        }

        [Route(WorkflowStepRoute.Delete), HttpPost]
        public async Task<ActionResult<WorkflowStep_WorkflowStepDTO>> Delete([FromBody] WorkflowStep_WorkflowStepDTO WorkflowStep_WorkflowStepDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(WorkflowStep_WorkflowStepDTO.Id))
                return Forbid();

            WorkflowStep WorkflowStep = ConvertDTOToEntity(WorkflowStep_WorkflowStepDTO);
            WorkflowStep = await WorkflowStepService.Delete(WorkflowStep);
            WorkflowStep_WorkflowStepDTO = new WorkflowStep_WorkflowStepDTO(WorkflowStep);
            if (WorkflowStep.IsValidated)
                return WorkflowStep_WorkflowStepDTO;
            else
                return BadRequest(WorkflowStep_WorkflowStepDTO);
        }

        [Route(WorkflowStepRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowStepFilter WorkflowStepFilter = new WorkflowStepFilter();
            WorkflowStepFilter = WorkflowStepService.ToFilter(WorkflowStepFilter);
            WorkflowStepFilter.Id = new IdFilter { In = Ids };
            WorkflowStepFilter.Selects = WorkflowStepSelect.Id;
            WorkflowStepFilter.Skip = 0;
            WorkflowStepFilter.Take = int.MaxValue;

            List<WorkflowStep> WorkflowSteps = await WorkflowStepService.List(WorkflowStepFilter);
            WorkflowSteps = await WorkflowStepService.BulkDelete(WorkflowSteps);
            if (WorkflowSteps.Any(x => !x.IsValidated))
                return BadRequest(WorkflowSteps.Where(x => !x.IsValidated));
            return true;
        }

        [Route(WorkflowStepRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            RoleFilter RoleFilter = new RoleFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = RoleSelect.ALL
            };
            List<Role> Roles = await RoleService.List(RoleFilter);
            WorkflowDefinitionFilter WorkflowDefinitionFilter = new WorkflowDefinitionFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = WorkflowDefinitionSelect.ALL
            };
            List<WorkflowDefinition> WorkflowDefinitions = await WorkflowDefinitionService.List(WorkflowDefinitionFilter);
            List<WorkflowStep> WorkflowSteps = new List<WorkflowStep>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    return Ok(WorkflowSteps);
                int StartColumn = 1;
                int StartRow = 1;
                int IdColumn = 0 + StartColumn;
                int WorkflowDefinitionIdColumn = 1 + StartColumn;
                int NameColumn = 2 + StartColumn;
                int RoleIdColumn = 3 + StartColumn;
                int SubjectMailForRejectColumn = 4 + StartColumn;
                int BodyMailForRejectColumn = 5 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[i + StartRow, StartColumn].Value?.ToString()))
                        break;
                    string IdValue = worksheet.Cells[i + StartRow, IdColumn].Value?.ToString();
                    string WorkflowDefinitionIdValue = worksheet.Cells[i + StartRow, WorkflowDefinitionIdColumn].Value?.ToString();
                    string NameValue = worksheet.Cells[i + StartRow, NameColumn].Value?.ToString();
                    string RoleIdValue = worksheet.Cells[i + StartRow, RoleIdColumn].Value?.ToString();
                    string SubjectMailForRejectValue = worksheet.Cells[i + StartRow, SubjectMailForRejectColumn].Value?.ToString();
                    string BodyMailForRejectValue = worksheet.Cells[i + StartRow, BodyMailForRejectColumn].Value?.ToString();

                    WorkflowStep WorkflowStep = new WorkflowStep();
                    WorkflowStep.Name = NameValue;
                    WorkflowStep.SubjectMailForReject = SubjectMailForRejectValue;
                    WorkflowStep.BodyMailForReject = BodyMailForRejectValue;
                    Role Role = Roles.Where(x => x.Id.ToString() == RoleIdValue).FirstOrDefault();
                    WorkflowStep.RoleId = Role == null ? 0 : Role.Id;
                    WorkflowStep.Role = Role;
                    WorkflowDefinition WorkflowDefinition = WorkflowDefinitions.Where(x => x.Id.ToString() == WorkflowDefinitionIdValue).FirstOrDefault();
                    WorkflowStep.WorkflowDefinitionId = WorkflowDefinition == null ? 0 : WorkflowDefinition.Id;
                    WorkflowStep.WorkflowDefinition = WorkflowDefinition;

                    WorkflowSteps.Add(WorkflowStep);
                }
            }
            WorkflowSteps = await WorkflowStepService.Import(WorkflowSteps);
            if (WorkflowSteps.All(x => x.IsValidated))
                return Ok(true);
            else
            {
                List<string> Errors = new List<string>();
                for (int i = 0; i < WorkflowSteps.Count; i++)
                {
                    WorkflowStep WorkflowStep = WorkflowSteps[i];
                    if (!WorkflowStep.IsValidated)
                    {
                        string Error = $"Dòng {i + 2} có lỗi:";
                        if (WorkflowStep.Errors.ContainsKey(nameof(WorkflowStep.Id)))
                            Error += WorkflowStep.Errors[nameof(WorkflowStep.Id)];
                        if (WorkflowStep.Errors.ContainsKey(nameof(WorkflowStep.WorkflowDefinitionId)))
                            Error += WorkflowStep.Errors[nameof(WorkflowStep.WorkflowDefinitionId)];
                        if (WorkflowStep.Errors.ContainsKey(nameof(WorkflowStep.Name)))
                            Error += WorkflowStep.Errors[nameof(WorkflowStep.Name)];
                        if (WorkflowStep.Errors.ContainsKey(nameof(WorkflowStep.RoleId)))
                            Error += WorkflowStep.Errors[nameof(WorkflowStep.RoleId)];
                        if (WorkflowStep.Errors.ContainsKey(nameof(WorkflowStep.SubjectMailForReject)))
                            Error += WorkflowStep.Errors[nameof(WorkflowStep.SubjectMailForReject)];
                        if (WorkflowStep.Errors.ContainsKey(nameof(WorkflowStep.BodyMailForReject)))
                            Error += WorkflowStep.Errors[nameof(WorkflowStep.BodyMailForReject)];
                        Errors.Add(Error);
                    }
                }
                return BadRequest(Errors);
            }
        }

        [Route(WorkflowStepRoute.Export), HttpPost]
        public async Task<FileResult> Export([FromBody] WorkflowStep_WorkflowStepFilterDTO WorkflowStep_WorkflowStepFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region WorkflowStep
                var WorkflowStepFilter = ConvertFilterDTOToFilterEntity(WorkflowStep_WorkflowStepFilterDTO);
                WorkflowStepFilter.Skip = 0;
                WorkflowStepFilter.Take = int.MaxValue;
                WorkflowStepFilter = WorkflowStepService.ToFilter(WorkflowStepFilter);
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
            return File(memoryStream.ToArray(), "application/octet-stream", "WorkflowStep.xlsx");
        }

        [Route(WorkflowStepRoute.ExportTemplate), HttpPost]
        public async Task<FileResult> ExportTemplate([FromBody] WorkflowStep_WorkflowStepFilterDTO WorkflowStep_WorkflowStepFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region WorkflowStep
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
                excel.GenerateWorksheet("WorkflowStep", WorkflowStepHeaders, WorkflowStepData);
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
            return File(memoryStream.ToArray(), "application/octet-stream", "WorkflowStep.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            WorkflowStepFilter WorkflowStepFilter = new WorkflowStepFilter();
            WorkflowStepFilter = WorkflowStepService.ToFilter(WorkflowStepFilter);
            if (Id == 0)
            {

            }
            else
            {
                WorkflowStepFilter.Id = new IdFilter { Equal = Id };
                int count = await WorkflowStepService.Count(WorkflowStepFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private WorkflowStep ConvertDTOToEntity(WorkflowStep_WorkflowStepDTO WorkflowStep_WorkflowStepDTO)
        {
            WorkflowStep_WorkflowStepDTO.TrimString();
            WorkflowStep WorkflowStep = new WorkflowStep();
            WorkflowStep.Id = WorkflowStep_WorkflowStepDTO.Id;
            WorkflowStep.WorkflowDefinitionId = WorkflowStep_WorkflowStepDTO.WorkflowDefinitionId;
            WorkflowStep.Code = WorkflowStep_WorkflowStepDTO.Code;
            WorkflowStep.Name = WorkflowStep_WorkflowStepDTO.Name;
            WorkflowStep.RoleId = WorkflowStep_WorkflowStepDTO.RoleId;
            WorkflowStep.StatusId = WorkflowStep_WorkflowStepDTO.StatusId;
            WorkflowStep.SubjectMailForReject = WorkflowStep_WorkflowStepDTO.SubjectMailForReject;
            WorkflowStep.BodyMailForReject = WorkflowStep_WorkflowStepDTO.BodyMailForReject;
            WorkflowStep.Status = WorkflowStep_WorkflowStepDTO.Status == null ? null : new Status
            {
                Id = WorkflowStep_WorkflowStepDTO.Status.Id,
                Code = WorkflowStep_WorkflowStepDTO.Status.Code,
                Name = WorkflowStep_WorkflowStepDTO.Status.Name,
            };
            WorkflowStep.Role = WorkflowStep_WorkflowStepDTO.Role == null ? null : new Role
            {
                Id = WorkflowStep_WorkflowStepDTO.Role.Id,
                Code = WorkflowStep_WorkflowStepDTO.Role.Code,
                Name = WorkflowStep_WorkflowStepDTO.Role.Name,
                StatusId = WorkflowStep_WorkflowStepDTO.Role.StatusId,
            };
            WorkflowStep.WorkflowDefinition = WorkflowStep_WorkflowStepDTO.WorkflowDefinition == null ? null : new WorkflowDefinition
            {
                Id = WorkflowStep_WorkflowStepDTO.WorkflowDefinition.Id,
                Code = WorkflowStep_WorkflowStepDTO.WorkflowDefinition.Code,
                Name = WorkflowStep_WorkflowStepDTO.WorkflowDefinition.Name,
                WorkflowTypeId = WorkflowStep_WorkflowStepDTO.WorkflowDefinition.WorkflowTypeId,
                StartDate = WorkflowStep_WorkflowStepDTO.WorkflowDefinition.StartDate,
                EndDate = WorkflowStep_WorkflowStepDTO.WorkflowDefinition.EndDate,
                StatusId = WorkflowStep_WorkflowStepDTO.WorkflowDefinition.StatusId,
                UpdatedAt = WorkflowStep_WorkflowStepDTO.WorkflowDefinition.UpdatedAt,
            };
            WorkflowStep.BaseLanguage = CurrentContext.Language;
            return WorkflowStep;
        }

        private WorkflowStepFilter ConvertFilterDTOToFilterEntity(WorkflowStep_WorkflowStepFilterDTO WorkflowStep_WorkflowStepFilterDTO)
        {
            WorkflowStepFilter WorkflowStepFilter = new WorkflowStepFilter();
            WorkflowStepFilter.Selects = WorkflowStepSelect.ALL;
            WorkflowStepFilter.Skip = WorkflowStep_WorkflowStepFilterDTO.Skip;
            WorkflowStepFilter.Take = WorkflowStep_WorkflowStepFilterDTO.Take;
            WorkflowStepFilter.OrderBy = WorkflowStep_WorkflowStepFilterDTO.OrderBy;
            WorkflowStepFilter.OrderType = WorkflowStep_WorkflowStepFilterDTO.OrderType;

            WorkflowStepFilter.Id = WorkflowStep_WorkflowStepFilterDTO.Id;
            WorkflowStepFilter.WorkflowDefinitionId = WorkflowStep_WorkflowStepFilterDTO.WorkflowDefinitionId;
            WorkflowStepFilter.Code = WorkflowStep_WorkflowStepFilterDTO.Code;
            WorkflowStepFilter.Name = WorkflowStep_WorkflowStepFilterDTO.Name;
            WorkflowStepFilter.RoleId = WorkflowStep_WorkflowStepFilterDTO.RoleId;
            WorkflowStepFilter.StatusId = WorkflowStep_WorkflowStepFilterDTO.StatusId;
            return WorkflowStepFilter;
        }

        [Route(WorkflowStepRoute.FilterListAppUser), HttpPost]
        public async Task<List<WorkflowStep_AppUserDTO>> FilterListAppUser([FromBody] WorkflowStep_AppUserFilterDTO WorkflowStep_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = WorkflowStep_AppUserFilterDTO.Id;
            AppUserFilter.Username = WorkflowStep_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = WorkflowStep_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = WorkflowStep_AppUserFilterDTO.Address;
            AppUserFilter.Email = WorkflowStep_AppUserFilterDTO.Email;
            AppUserFilter.Phone = WorkflowStep_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = WorkflowStep_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = WorkflowStep_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = WorkflowStep_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = WorkflowStep_AppUserFilterDTO.StatusId;
            AppUserFilter.ProvinceId = WorkflowStep_AppUserFilterDTO.ProvinceId;
            AppUserFilter.SexId = WorkflowStep_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = WorkflowStep_AppUserFilterDTO.Birthday;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<WorkflowStep_AppUserDTO> WorkflowStep_AppUserDTOs = AppUsers
                .Select(x => new WorkflowStep_AppUserDTO(x)).ToList();
            return WorkflowStep_AppUserDTOs;
        }

        [Route(WorkflowStepRoute.FilterListRole), HttpPost]
        public async Task<List<WorkflowStep_RoleDTO>> FilterListRole([FromBody] WorkflowStep_RoleFilterDTO WorkflowStep_RoleFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RoleFilter RoleFilter = new RoleFilter();
            RoleFilter.Skip = 0;
            RoleFilter.Take = 20;
            RoleFilter.OrderBy = RoleOrder.Id;
            RoleFilter.OrderType = OrderType.ASC;
            RoleFilter.Selects = RoleSelect.ALL;
            RoleFilter.Id = WorkflowStep_RoleFilterDTO.Id;
            RoleFilter.Code = WorkflowStep_RoleFilterDTO.Code;
            RoleFilter.Name = WorkflowStep_RoleFilterDTO.Name;
            RoleFilter.StatusId = WorkflowStep_RoleFilterDTO.StatusId;

            if (WorkflowStep_RoleFilterDTO.WorkflowDefinitionId != null && WorkflowStep_RoleFilterDTO.WorkflowDefinitionId.HasValue)
            {
                List<Role> Roles = await WorkflowStepService.ListRole(WorkflowStep_RoleFilterDTO.WorkflowDefinitionId, RoleFilter);
                List<WorkflowStep_RoleDTO> WorkflowStep_RoleDTOs = Roles
                    .Select(x => new WorkflowStep_RoleDTO(x)).ToList();
                return WorkflowStep_RoleDTOs;
            }
            return new List<WorkflowStep_RoleDTO>();
        }

        [Route(WorkflowStepRoute.FilterListWorkflowDefinition), HttpPost]
        public async Task<List<WorkflowStep_WorkflowDefinitionDTO>> FilterListWorkflowDefinition([FromBody] WorkflowStep_WorkflowDefinitionFilterDTO WorkflowStep_WorkflowDefinitionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDefinitionFilter WorkflowDefinitionFilter = new WorkflowDefinitionFilter();
            WorkflowDefinitionFilter.Skip = 0;
            WorkflowDefinitionFilter.Take = 20;
            WorkflowDefinitionFilter.OrderBy = WorkflowDefinitionOrder.Id;
            WorkflowDefinitionFilter.OrderType = OrderType.ASC;
            WorkflowDefinitionFilter.Selects = WorkflowDefinitionSelect.ALL;
            WorkflowDefinitionFilter.Id = WorkflowStep_WorkflowDefinitionFilterDTO.Id;
            WorkflowDefinitionFilter.Code = WorkflowStep_WorkflowDefinitionFilterDTO.Code;
            WorkflowDefinitionFilter.Name = WorkflowStep_WorkflowDefinitionFilterDTO.Name;
            WorkflowDefinitionFilter.WorkflowTypeId = WorkflowStep_WorkflowDefinitionFilterDTO.WorkflowTypeId;
            WorkflowDefinitionFilter.StartDate = WorkflowStep_WorkflowDefinitionFilterDTO.StartDate;
            WorkflowDefinitionFilter.EndDate = WorkflowStep_WorkflowDefinitionFilterDTO.EndDate;
            WorkflowDefinitionFilter.StatusId = WorkflowStep_WorkflowDefinitionFilterDTO.StatusId;
            WorkflowDefinitionFilter.UpdatedAt = WorkflowStep_WorkflowDefinitionFilterDTO.UpdatedAt;

            List<WorkflowDefinition> WorkflowDefinitions = await WorkflowDefinitionService.List(WorkflowDefinitionFilter);
            List<WorkflowStep_WorkflowDefinitionDTO> WorkflowStep_WorkflowDefinitionDTOs = WorkflowDefinitions
                .Select(x => new WorkflowStep_WorkflowDefinitionDTO(x)).ToList();
            return WorkflowStep_WorkflowDefinitionDTOs;
        }
        [Route(WorkflowStepRoute.FilterListStatus), HttpPost]
        public async Task<List<WorkflowStep_StatusDTO>> FilterListStatus()
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<WorkflowStep_StatusDTO> WorkflowStep_StatusDTOs = Statuses
                .Select(x => new WorkflowStep_StatusDTO(x)).ToList();
            return WorkflowStep_StatusDTOs;
        }

        [Route(WorkflowStepRoute.SingleListAppUser), HttpPost]
        public async Task<List<WorkflowStep_AppUserDTO>> SingleListAppUser([FromBody] WorkflowStep_AppUserFilterDTO WorkflowStep_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = WorkflowStep_AppUserFilterDTO.Id;
            AppUserFilter.Username = WorkflowStep_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = WorkflowStep_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = WorkflowStep_AppUserFilterDTO.Address;
            AppUserFilter.Email = WorkflowStep_AppUserFilterDTO.Email;
            AppUserFilter.Phone = WorkflowStep_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = WorkflowStep_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = WorkflowStep_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = WorkflowStep_AppUserFilterDTO.OrganizationId;
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            AppUserFilter.ProvinceId = WorkflowStep_AppUserFilterDTO.ProvinceId;
            AppUserFilter.SexId = WorkflowStep_AppUserFilterDTO.SexId;
            AppUserFilter.Birthday = WorkflowStep_AppUserFilterDTO.Birthday;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<WorkflowStep_AppUserDTO> WorkflowStep_AppUserDTOs = AppUsers
                .Select(x => new WorkflowStep_AppUserDTO(x)).ToList();
            return WorkflowStep_AppUserDTOs;
        }

        [Route(WorkflowStepRoute.SingleListRole), HttpPost]
        public async Task<List<WorkflowStep_RoleDTO>> SingleListRole([FromBody] WorkflowStep_RoleFilterDTO WorkflowStep_RoleFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            RoleFilter RoleFilter = new RoleFilter();
            RoleFilter.Skip = 0;
            RoleFilter.Take = 20;
            RoleFilter.OrderBy = RoleOrder.Id;
            RoleFilter.OrderType = OrderType.ASC;
            RoleFilter.Selects = RoleSelect.ALL;
            RoleFilter.Id = WorkflowStep_RoleFilterDTO.Id;
            RoleFilter.Code = WorkflowStep_RoleFilterDTO.Code;
            RoleFilter.Name = WorkflowStep_RoleFilterDTO.Name;
            RoleFilter.StatusId = WorkflowStep_RoleFilterDTO.StatusId;

            List<Role> Roles = await RoleService.List(RoleFilter);
            List<WorkflowStep_RoleDTO> WorkflowStep_RoleDTOs = Roles
                .Select(x => new WorkflowStep_RoleDTO(x)).ToList();
            return WorkflowStep_RoleDTOs;
        }
        [Route(WorkflowStepRoute.SingleListWorkflowDefinition), HttpPost]
        public async Task<List<WorkflowStep_WorkflowDefinitionDTO>> SingleListWorkflowDefinition([FromBody] WorkflowStep_WorkflowDefinitionFilterDTO WorkflowStep_WorkflowDefinitionFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowDefinitionFilter WorkflowDefinitionFilter = new WorkflowDefinitionFilter();
            WorkflowDefinitionFilter.Skip = 0;
            WorkflowDefinitionFilter.Take = 20;
            WorkflowDefinitionFilter.OrderBy = WorkflowDefinitionOrder.Id;
            WorkflowDefinitionFilter.OrderType = OrderType.ASC;
            WorkflowDefinitionFilter.Selects = WorkflowDefinitionSelect.ALL;
            WorkflowDefinitionFilter.Id = WorkflowStep_WorkflowDefinitionFilterDTO.Id;
            WorkflowDefinitionFilter.Code = WorkflowStep_WorkflowDefinitionFilterDTO.Code;
            WorkflowDefinitionFilter.Name = WorkflowStep_WorkflowDefinitionFilterDTO.Name;
            WorkflowDefinitionFilter.WorkflowTypeId = WorkflowStep_WorkflowDefinitionFilterDTO.WorkflowTypeId;
            WorkflowDefinitionFilter.StartDate = WorkflowStep_WorkflowDefinitionFilterDTO.StartDate;
            WorkflowDefinitionFilter.EndDate = WorkflowStep_WorkflowDefinitionFilterDTO.EndDate;
            //WorkflowDefinitionFilter.StatusId = WorkflowStep_WorkflowDefinitionFilterDTO.StatusId;
            WorkflowDefinitionFilter.UpdatedAt = WorkflowStep_WorkflowDefinitionFilterDTO.UpdatedAt;
            WorkflowDefinitionFilter.Used = false;
            List<WorkflowDefinition> WorkflowDefinitions = await WorkflowDefinitionService.List(WorkflowDefinitionFilter);
            List<WorkflowStep_WorkflowDefinitionDTO> WorkflowStep_WorkflowDefinitionDTOs = WorkflowDefinitions
                .Select(x => new WorkflowStep_WorkflowDefinitionDTO(x)).ToList();
            return WorkflowStep_WorkflowDefinitionDTOs;
        }

        [Route(WorkflowStepRoute.SingleListStatus), HttpPost]
        public async Task<List<WorkflowStep_StatusDTO>> SingleListStatus()
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<WorkflowStep_StatusDTO> WorkflowStep_StatusDTOs = Statuses
                .Select(x => new WorkflowStep_StatusDTO(x)).ToList();
            return WorkflowStep_StatusDTOs;
        }

        [Route(WorkflowStepRoute.SingleListWorkflowParameter), HttpPost]
        public async Task<List<WorkflowStep_WorkflowParameterDTO>> SingleListWorkflowParameter([FromBody] WorkflowStep_WorkflowParameterFilterDTO WorkflowStep_WorkflowParameterFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            WorkflowParameterFilter WorkflowParameterFilter = new WorkflowParameterFilter();
            WorkflowParameterFilter.Skip = 0;
            WorkflowParameterFilter.Take = 20;
            WorkflowParameterFilter.OrderBy = WorkflowParameterOrder.Id;
            WorkflowParameterFilter.OrderType = OrderType.ASC;
            WorkflowParameterFilter.Selects = WorkflowParameterSelect.ALL;
            WorkflowParameterFilter.Id = WorkflowStep_WorkflowParameterFilterDTO.Id;
            WorkflowParameterFilter.Code = WorkflowStep_WorkflowParameterFilterDTO.Code;
            WorkflowParameterFilter.Name = WorkflowStep_WorkflowParameterFilterDTO.Name;
            WorkflowParameterFilter.WorkflowTypeId = WorkflowStep_WorkflowParameterFilterDTO.WorkflowTypeId;
            WorkflowParameterFilter.WorkflowParameterTypeId = WorkflowStep_WorkflowParameterFilterDTO.WorkflowParameterTypeId;

            List<WorkflowParameter> WorkflowParameters = await WorkflowParameterService.List(WorkflowParameterFilter);
            List<WorkflowStep_WorkflowParameterDTO> WorkflowStep_WorkflowParameterDTOs = WorkflowParameters
                .Select(x => new WorkflowStep_WorkflowParameterDTO(x)).ToList();
            return WorkflowStep_WorkflowParameterDTOs;
        }
    }
}

