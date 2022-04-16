using DMS.Common;
using DMS.Entities;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MStatus;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.organization
{
    public class OrganizationController : RpcController
    {
        private IStatusService StatusService;
        private IAppUserService AppUserService;
        private IOrganizationService OrganizationService;
        private ICurrentContext CurrentContext;
        public OrganizationController(
            IStatusService StatusService,
            IAppUserService AppUserService,
            IOrganizationService OrganizationService,
            ICurrentContext CurrentContext
        )
        {
            this.StatusService = StatusService;
            this.AppUserService = AppUserService;
            this.OrganizationService = OrganizationService;
            this.CurrentContext = CurrentContext;
        }

        [Route(OrganizationRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Organization_OrganizationFilterDTO Organization_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = ConvertFilterDTOToFilterEntity(Organization_OrganizationFilterDTO);
            OrganizationFilter = OrganizationService.ToFilter(OrganizationFilter);
            int count = await OrganizationService.Count(OrganizationFilter);
            return count;
        }

        [Route(OrganizationRoute.List), HttpPost]
        public async Task<ActionResult<List<Organization_OrganizationDTO>>> List([FromBody] Organization_OrganizationFilterDTO Organization_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = ConvertFilterDTOToFilterEntity(Organization_OrganizationFilterDTO);
            OrganizationFilter = OrganizationService.ToFilter(OrganizationFilter);
            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Organization_OrganizationDTO> Organization_OrganizationDTOs = Organizations
                .Select(c => new Organization_OrganizationDTO(c)).ToList();
            return Organization_OrganizationDTOs;
        }

        [Route(OrganizationRoute.Get), HttpPost]
        public async Task<ActionResult<Organization_OrganizationDTO>> Get([FromBody] Organization_OrganizationDTO Organization_OrganizationDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Organization_OrganizationDTO.Id))
                return Forbid();

            Organization Organization = await OrganizationService.Get(Organization_OrganizationDTO.Id);
            return new Organization_OrganizationDTO(Organization);
        }

        [Route(OrganizationRoute.UpdateIsDisplay), HttpPost]
        public async Task<ActionResult<Organization_OrganizationDTO>> UpdateIsDisplay([FromBody] Organization_OrganizationDTO Organization_OrganizationDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Organization_OrganizationDTO.Id))
                return Forbid();

            Organization Organization = ConvertDTOToEntity(Organization_OrganizationDTO);
            Organization = await OrganizationService.UpdateIsDisplay(Organization);
            Organization_OrganizationDTO = new Organization_OrganizationDTO(Organization);
            if (Organization.IsValidated)
                return Organization_OrganizationDTO;
            else
                return BadRequest(Organization_OrganizationDTO);
        }

        [Route(OrganizationRoute.Export), HttpPost]
        public async Task<ActionResult> Export([FromBody] Organization_OrganizationFilterDTO Organization_OrganizationFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            OrganizationFilter OrganizationFilter = ConvertFilterDTOToFilterEntity(Organization_OrganizationFilterDTO);
            OrganizationFilter = OrganizationService.ToFilter(OrganizationFilter);

            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = int.MaxValue;
            OrganizationFilter = OrganizationService.ToFilter(OrganizationFilter);

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                var OrganizationHeaders = new List<string>()
                {
                     "Mã tổ chức","Tên tổ chức","Mã tổ chức"
                };
                List<object[]> data = new List<object[]>();
                for (int i = 0; i < Organizations.Count; i++)
                {
                    var Organization = Organizations[i];
                    data.Add(new Object[]
                    {
                        Organization.Name,
                        Organization.Code,
                        Organization.Parent?.Code ?? "",
                    });
                }
                excel.GenerateWorksheet("Organization", OrganizationHeaders, data);
                excel.Save();
            }

            return File(memoryStream.ToArray(), "application/octet-stream", "Organization.xlsx");

        }

        [Route(OrganizationRoute.ExportAppUser), HttpPost]
        public async Task<FileResult> ExportAppUser([FromBody] Organization_AppUserFilterDTO Organization_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = int.MaxValue;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.Username | AppUserSelect.Address | AppUserSelect.Phone | AppUserSelect.Sex
                | AppUserSelect.Birthday | AppUserSelect.Organization | AppUserSelect.Status;
            AppUserFilter.OrganizationId = Organization_AppUserFilterDTO.OrganizationId;
            AppUserFilter = AppUserService.ToFilter(AppUserFilter);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                var AppUserHeaders = new List<string>()
                {
                     "STT", "Tên đăng nhập","Tên hiển thị","Địa chỉ","Điện thoại","Email","Giới tính","Ngày sinh","Đơn vị quản lý", "Trạng thái"
                };
                List<object[]> data = new List<object[]>();
                for (int i = 0; i < AppUsers.Count; i++)
                {
                    var appUser = AppUsers[i];
                    data.Add(new Object[]
                    {
                        i+1,
                        appUser.Username,
                        appUser.DisplayName,
                        appUser.Address,
                        appUser.Phone,
                        appUser.Email,
                        appUser.Sex.Name,
                        appUser.Birthday?.ToString("dd-MM-yyyy"),
                        appUser.Organization.Code,
                        appUser.Status.Name,
                    });
                }
                excel.GenerateWorksheet("Tài khoản", AppUserHeaders, data);
                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "Organization_AppUser" + ".xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter = OrganizationService.ToFilter(OrganizationFilter);
            if (Id == 0)
            {

            }
            else
            {
                OrganizationFilter.Id = new IdFilter { Equal = Id };
                int count = await OrganizationService.Count(OrganizationFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private Organization ConvertDTOToEntity(Organization_OrganizationDTO Organization_OrganizationDTO)
        {
            Organization_OrganizationDTO.TrimString();
            Organization Organization = new Organization();
            Organization.Id = Organization_OrganizationDTO.Id;
            Organization.Code = Organization_OrganizationDTO.Code;
            Organization.Name = Organization_OrganizationDTO.Name;
            Organization.ParentId = Organization_OrganizationDTO.ParentId;
            Organization.Path = Organization_OrganizationDTO.Path;
            Organization.Level = Organization_OrganizationDTO.Level;
            Organization.StatusId = Organization_OrganizationDTO.StatusId;
            Organization.Phone = Organization_OrganizationDTO.Phone;
            Organization.Address = Organization_OrganizationDTO.Address;
            Organization.Email = Organization_OrganizationDTO.Email;
            Organization.IsDisplay = Organization_OrganizationDTO.IsDisplay;
            Organization.Parent = Organization_OrganizationDTO.Parent == null ? null : new Organization
            {
                Id = Organization_OrganizationDTO.Parent.Id,
                Code = Organization_OrganizationDTO.Parent.Code,
                Name = Organization_OrganizationDTO.Parent.Name,
                ParentId = Organization_OrganizationDTO.Parent.ParentId,
                Path = Organization_OrganizationDTO.Parent.Path,
                Level = Organization_OrganizationDTO.Parent.Level,
                StatusId = Organization_OrganizationDTO.Parent.StatusId,
                Phone = Organization_OrganizationDTO.Parent.Phone,
                Address = Organization_OrganizationDTO.Parent.Address,
                Email = Organization_OrganizationDTO.Parent.Email,
            };
            Organization.Status = Organization_OrganizationDTO.Status == null ? null : new Status
            {
                Id = Organization_OrganizationDTO.Status.Id,
                Code = Organization_OrganizationDTO.Status.Code,
                Name = Organization_OrganizationDTO.Status.Name,
            };
            Organization.AppUsers = Organization_OrganizationDTO.AppUsers?
                .Select(x => new AppUser
                {
                    Id = x.Id,
                    Username = x.Username,
                    DisplayName = x.DisplayName,
                    Address = x.Address,
                    Email = x.Email,
                    Phone = x.Phone,
                    Department = x.Department,
                    SexId = x.SexId,
                    StatusId = x.StatusId,
                    Status = x.Status == null ? null : new Status
                    {
                        Id = x.Status.Id,
                        Code = x.Status.Code,
                        Name = x.Status.Name,
                    },
                }).ToList();
            Organization.BaseLanguage = CurrentContext.Language;
            return Organization;
        }

        private OrganizationFilter ConvertFilterDTOToFilterEntity(Organization_OrganizationFilterDTO Organization_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;

            OrganizationFilter.Id = Organization_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = Organization_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = Organization_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = Organization_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = Organization_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = Organization_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = Organization_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = Organization_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = Organization_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = Organization_OrganizationFilterDTO.Email;
            OrganizationFilter.UpdatedAt = Organization_OrganizationFilterDTO.UpdatedAt;
            return OrganizationFilter;
        }

        [Route(OrganizationRoute.FilterListOrganization), HttpPost]
        public async Task<List<Organization_OrganizationDTO>> FilterListOrganization([FromBody] Organization_OrganizationFilterDTO Organization_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = Organization_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = Organization_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = Organization_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = Organization_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = Organization_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = Organization_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = null;
            OrganizationFilter.IsDisplay = true;

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Organization_OrganizationDTO> Organization_OrganizationDTOs = Organizations
                .Select(x => new Organization_OrganizationDTO(x)).ToList();
            return Organization_OrganizationDTOs;
        }
        [Route(OrganizationRoute.FilterListStatus), HttpPost]
        public async Task<List<Organization_StatusDTO>> FilterListStatus([FromBody] Organization_StatusFilterDTO Organization_StatusFilterDTO)
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;
            StatusFilter.Id = Organization_StatusFilterDTO.Id;
            StatusFilter.Code = Organization_StatusFilterDTO.Code;
            StatusFilter.Name = Organization_StatusFilterDTO.Name;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<Organization_StatusDTO> Organization_StatusDTOs = Statuses
                .Select(x => new Organization_StatusDTO(x)).ToList();
            return Organization_StatusDTOs;
        }
        [Route(OrganizationRoute.FilterListAppUser), HttpPost]
        public async Task<List<Organization_AppUserDTO>> FilterListAppUser([FromBody] Organization_AppUserFilterDTO Organization_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = Organization_AppUserFilterDTO.Id;
            AppUserFilter.Username = Organization_AppUserFilterDTO.Username;
            AppUserFilter.Password = Organization_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = Organization_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = Organization_AppUserFilterDTO.Address;
            AppUserFilter.Email = Organization_AppUserFilterDTO.Email;
            AppUserFilter.Phone = Organization_AppUserFilterDTO.Phone;
            AppUserFilter.Department = Organization_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = Organization_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = Organization_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = null;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Organization_AppUserDTO> Organization_AppUserDTOs = AppUsers
                .Select(x => new Organization_AppUserDTO(x)).ToList();
            return Organization_AppUserDTOs;
        }

        [Route(OrganizationRoute.SingleListOrganization), HttpPost]
        public async Task<List<Organization_OrganizationDTO>> SingleListOrganization([FromBody] Organization_OrganizationFilterDTO Organization_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = Organization_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = Organization_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = Organization_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = Organization_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = Organization_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = Organization_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            OrganizationFilter.IsDisplay = true;

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Organization_OrganizationDTO> Organization_OrganizationDTOs = Organizations
                .Select(x => new Organization_OrganizationDTO(x)).ToList();
            return Organization_OrganizationDTOs;
        }
        [Route(OrganizationRoute.SingleListStatus), HttpPost]
        public async Task<List<Organization_StatusDTO>> SingleListStatus([FromBody] Organization_StatusFilterDTO Organization_StatusFilterDTO)
        {
            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;
            StatusFilter.Id = Organization_StatusFilterDTO.Id;
            StatusFilter.Code = Organization_StatusFilterDTO.Code;
            StatusFilter.Name = Organization_StatusFilterDTO.Name;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<Organization_StatusDTO> Organization_StatusDTOs = Statuses
                .Select(x => new Organization_StatusDTO(x)).ToList();
            return Organization_StatusDTOs;
        }
        [Route(OrganizationRoute.SingleListAppUser), HttpPost]
        public async Task<List<Organization_AppUserDTO>> SingleListAppUser([FromBody] Organization_AppUserFilterDTO Organization_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = Organization_AppUserFilterDTO.Id;
            AppUserFilter.Username = Organization_AppUserFilterDTO.Username;
            AppUserFilter.Password = Organization_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = Organization_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = Organization_AppUserFilterDTO.Address;
            AppUserFilter.Email = Organization_AppUserFilterDTO.Email;
            AppUserFilter.Phone = Organization_AppUserFilterDTO.Phone;
            AppUserFilter.Department = Organization_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = Organization_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = Organization_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Organization_AppUserDTO> Organization_AppUserDTOs = AppUsers
                .Select(x => new Organization_AppUserDTO(x)).ToList();
            return Organization_AppUserDTOs;
        }
    }
}

