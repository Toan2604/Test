using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MPosition;
using DMS.Services.MRole;
using DMS.Services.MSex;
using DMS.Services.MStatus;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thinktecture;
using Thinktecture.EntityFrameworkCore.TempTables;
using TrueSight.Common;

namespace DMS.Rpc.app_user
{
    public partial class AppUserController : RpcController
    {
        private IOrganizationService OrganizationService;
        private IPositionService PositionService;
        private ISexService SexService;
        private IStatusService StatusService;
        private IRoleService RoleService;
        private IAppUserService AppUserService;
        private IStoreService StoreService;
        private IStoreGroupingService StoreGroupingService;
        private IStoreTypeService StoreTypeService;
        private ICurrentContext CurrentContext;
        private DataContext DataContext;
        public AppUserController(
            IOrganizationService OrganizationService,
            IPositionService PositionService,
            ISexService SexService,
            IStatusService StatusService,
            IRoleService RoleService,
            IAppUserService AppUserService,
            IStoreService StoreService,
            IStoreGroupingService StoreGroupingService,
            IStoreTypeService StoreTypeService,
            ICurrentContext CurrentContext,
            DataContext DataContext
        )
        {
            this.OrganizationService = OrganizationService;
            this.PositionService = PositionService;
            this.SexService = SexService;
            this.StatusService = StatusService;
            this.RoleService = RoleService;
            this.AppUserService = AppUserService;
            this.StoreService = StoreService;
            this.StoreGroupingService = StoreGroupingService;
            this.StoreTypeService = StoreTypeService;
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
        }

        [Route(AppUserRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] AppUser_AppUserFilterDTO AppUser_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = ConvertFilterDTOToFilterEntity(AppUser_AppUserFilterDTO);
            AppUserFilter = AppUserService.ToFilter(AppUserFilter);
            int count = await AppUserService.Count(AppUserFilter);
            return count;
        }

        [Route(AppUserRoute.List), HttpPost]
        public async Task<ActionResult<List<AppUser_AppUserDTO>>> List([FromBody] AppUser_AppUserFilterDTO AppUser_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = ConvertFilterDTOToFilterEntity(AppUser_AppUserFilterDTO);
            AppUserFilter = AppUserService.ToFilter(AppUserFilter);
            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<AppUser_AppUserDTO> AppUser_AppUserDTOs = AppUsers
                .Select(c => new AppUser_AppUserDTO(c)).ToList();
            return AppUser_AppUserDTOs;
        }

        [Route(AppUserRoute.Get), HttpPost]
        public async Task<ActionResult<AppUser_AppUserDTO>> Get([FromBody] AppUser_AppUserDTO AppUser_AppUserDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(AppUser_AppUserDTO.Id))
                return Forbid();

            AppUser AppUser = await AppUserService.Get(AppUser_AppUserDTO.Id);
            AppUser_AppUserDTO = new AppUser_AppUserDTO(AppUser);
            return AppUser_AppUserDTO;
        }

        [Route(AppUserRoute.Update), HttpPost]
        public async Task<ActionResult<AppUser_AppUserDTO>> Update([FromBody] AppUser_AppUserDTO AppUser_AppUserDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(AppUser_AppUserDTO.Id))
                return Forbid();

            AppUser AppUser = ConvertDTOToEntity(AppUser_AppUserDTO);
            AppUser = await AppUserService.Update(AppUser);
            AppUser_AppUserDTO = new AppUser_AppUserDTO(AppUser);
            if (AppUser.IsValidated)
                return AppUser_AppUserDTO;
            else
                return BadRequest(AppUser_AppUserDTO);
        }

        [Route(AppUserRoute.UpdateRole), HttpPost]
        public async Task<ActionResult<AppUser_AppUserDTO>> UpdateRole([FromBody] AppUser_AppUserDTO AppUser_AppUserDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(AppUser_AppUserDTO.Id))
                return Forbid();

            AppUser AppUser = ConvertDTOToEntity(AppUser_AppUserDTO);
            AppUser = await AppUserService.UpdateRole(AppUser);
            AppUser_AppUserDTO = new AppUser_AppUserDTO(AppUser);
            if (AppUser.IsValidated)
                return AppUser_AppUserDTO;
            else
                return BadRequest(AppUser_AppUserDTO);
        }

        [Route(AppUserRoute.ImportStore), HttpPost]
        public async Task<ActionResult<bool>> ImportStore([FromForm] IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            FileInfo FileInfo = new FileInfo(file.FileName);
            if (!FileInfo.Extension.Equals(".xlsx"))
                return BadRequest(new { message = "Định dạng file không hợp lệ" });

            var AppUser = await AppUserService.Get(CurrentContext.UserId);

            AppUserFilter EmployeeFilter = new AppUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName,
                OrganizationId = new IdFilter { Equal = AppUser.OrganizationId }
            };
            List<AppUser> Employees = await AppUserService.List(EmployeeFilter);

            List<Store> Stores = await StoreService.List(new StoreFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreSelect.Id | StoreSelect.Code | StoreSelect.CodeDraft | StoreSelect.Name,
                OrderBy = StoreOrder.Id,
                OrderType = OrderType.ASC,
                StatusId = new IdFilter { Equal = StatusEnum.ACTIVE.Id }
            });

            List<AppUser> AppUsers = new List<AppUser>();
            StringBuilder errorContent = new StringBuilder();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets["Phạm vi đi tuyến"];
                if (worksheet == null)
                    return BadRequest(new { message = "File không đúng biểu mẫu import" });

                int StartColumn = 1;
                int StartRow = 1;
                int SttColumn = 0 + StartColumn;
                int UsernameColumn = 1 + StartColumn;
                int StoreCodeColumn = 3 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    string stt = worksheet.Cells[i + StartRow, SttColumn].Value?.ToString();
                    if (stt != null && stt.ToLower() == "END".ToLower())
                        break;
                    string UsernameValue = worksheet.Cells[i + StartRow, UsernameColumn].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(UsernameValue) && i != worksheet.Dimension.End.Row)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i + 1}: Chưa nhập mã nhân viên");
                    }
                    else if (string.IsNullOrWhiteSpace(UsernameValue) && i == worksheet.Dimension.End.Row)
                        break;

                    string StoreCodeValue = worksheet.Cells[i + StartRow, StoreCodeColumn].Value?.ToString();
                    if (string.IsNullOrWhiteSpace(StoreCodeValue) && i != worksheet.Dimension.End.Row)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i + 1}: Chưa nhập mã đại lý");
                        continue;
                    }
                    AppUser appUser;
                    appUser = Employees.Where(x => x.Username == UsernameValue.Trim()).FirstOrDefault();
                    if (appUser == null)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i + 1}: Nhân viên không tồn tại");
                        continue;
                    }

                    var Store = Stores.Where(x => x.Code == StoreCodeValue).FirstOrDefault();
                    if (Store == null || Store.DeletedAt.HasValue)
                    {
                        errorContent.AppendLine($"Lỗi dòng thứ {i + 1}: Đại lý không tồn tại hoặc đã dừng hoạt động");
                        continue;
                    }
                    var Employee = AppUsers.Where(x => x.Username == appUser.Username).FirstOrDefault();
                    if (Employee == null)
                    {
                        Employee = new AppUser();
                        AppUsers.Add(Employee);
                        Employee.Id = appUser.Id;
                        Employee.Username = appUser.Username;
                        Employee.AppUserStoreMappings = new List<AppUserStoreMapping>();
                        Employee.AppUserStoreMappings.Add(new AppUserStoreMapping
                        {
                            AppUserId = appUser.Id,
                            StoreId = Store.Id
                        });
                    }
                    else
                    {
                        Employee.AppUserStoreMappings.Add(new AppUserStoreMapping
                        {
                            AppUserId = appUser.Id,
                            StoreId = Store.Id
                        });
                    }
                }
                if (errorContent.Length > 0)
                    return BadRequest(errorContent.ToString());
            }

            return await AppUserService.ImportERouteScope(AppUsers);
        }

        [Route(AppUserRoute.ExportStore), HttpPost]
        public async Task<ActionResult> ExportStore([FromBody] AppUser_AppUserFilterDTO AppUser_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = int.MaxValue;
            AppUserFilter.OrderBy = AppUserOrder.Username;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;
            AppUserFilter.Username = AppUser_AppUserFilterDTO.Username;
            AppUserFilter.StatusId = AppUser_AppUserFilterDTO.StatusId;
            AppUserFilter.DisplayName = AppUser_AppUserFilterDTO.DisplayName;
            AppUserFilter.Phone = AppUser_AppUserFilterDTO.Phone;
            AppUserFilter.Email = AppUser_AppUserFilterDTO.Email;
            AppUserFilter.OrganizationId = AppUser_AppUserFilterDTO.OrganizationId;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            var AppUserIds = AppUsers.Select(x => x.Id).ToList();

            ITempTableQuery<TempTable<long>> tempTableQuery = await DataContext
                       .BulkInsertValuesIntoTempTableAsync<long>(AppUserIds);

            var query = from asm in DataContext.AppUserStoreMapping
                        join a in DataContext.AppUser on asm.AppUserId equals a.Id
                        join s in DataContext.Store on asm.StoreId equals s.Id
                        where s.DeletedAt == null
                        select new AppUserStoreMapping
                        {
                            AppUserId = asm.AppUserId,
                            StoreId = asm.StoreId,
                            AppUser = new AppUser
                            {
                                Id = a.Id,
                                Username = a.Username,
                                DisplayName = a.DisplayName
                            },
                            Store = new Store
                            {
                                Id = s.Id,
                                CodeDraft = s.CodeDraft,
                                Code = s.Code,
                                Name = s.Name,
                                Address = s.Address,
                                District = s.District == null ? null : new District
                                {
                                    Id = s.District.Id,
                                    Code = s.District.Code,
                                    Name = s.District.Name
                                },
                                Province = s.Province == null ? null : new Province
                                {
                                    Id = s.Province.Id,
                                    Code = s.Province.Code,
                                    Name = s.Province.Name
                                },
                                Ward = s.Ward == null ? null : new Ward
                                {
                                    Id = s.Ward.Id,
                                    Code = s.Ward.Code,
                                    Name = s.Ward.Name
                                },
                                StoreStatus = s.StoreStatus == null ? null : new StoreStatus
                                {
                                    Id = s.StoreStatus.Id,
                                    Code = s.StoreStatus.Code,
                                    Name = s.StoreStatus.Name
                                },
                            }
                        };

            var AppUserStoreMappings = await query.ToListWithNoLockAsync();

            List<AppUser_AppUserDTO> AppUser_AppUserDTOs = new List<AppUser_AppUserDTO>();
            long STT = 1;
            foreach (var AppUser in AppUsers)
            {
                if (AppUserStoreMappings.Where(x => x.AppUserId == AppUser.Id).FirstOrDefault() != null)
                {
                    AppUser_AppUserDTO AppUser_AppUserDTO = new AppUser_AppUserDTO(AppUser);
                    AppUser_AppUserDTO.AppUserStoreMappings = AppUserStoreMappings.Where(x => x.AppUserId == AppUser.Id).Select(x => new AppUser_AppUserStoreMappingDTO
                    {
                        STT = STT++,
                        AppUserId = x.AppUserId,
                        StoreId = x.StoreId,
                        AppUser = x.AppUser == null ? null : new AppUser_AppUserDTO
                        {
                            Id = x.AppUser.Id,
                            Username = x.AppUser.Username,
                            DisplayName = x.AppUser.DisplayName
                        },
                        Store = x.Store == null ? null : new AppUser_StoreDTO
                        {
                            Id = x.Store.Id,
                            CodeDraft = x.Store.CodeDraft,
                            Code = x.Store.Code,
                            Name = x.Store.Name,
                            Address = x.Store.Address,
                            Province = x.Store.Province == null ? null : new AppUser_ProvinceDTO
                            {
                                Id = x.Store.Province.Id,
                                Code = x.Store.Province.Code,
                                Name = x.Store.Province.Name,
                            },
                            District = x.Store.District == null ? null : new AppUser_DistrictDTO
                            {
                                Id = x.Store.District.Id,
                                Code = x.Store.District.Code,
                                Name = x.Store.District.Name,
                            },
                            Ward = x.Store.Ward == null ? null : new AppUser_WardDTO
                            {
                                Id = x.Store.Ward.Id,
                                Code = x.Store.Ward.Code,
                                Name = x.Store.Ward.Name,
                            },
                            StoreStatus = x.Store.StoreStatus == null ? null : new AppUser_StoreStatusDTO
                            {
                                Id = x.Store.StoreStatus.Id,
                                Code = x.Store.StoreStatus.Code,
                                Name = x.Store.StoreStatus.Name,
                            }
                        },
                    }).ToList();
                    AppUser_AppUserDTOs.Add(AppUser_AppUserDTO);
                }

            }

            string path = "Templates/Template_AppUser_Store.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.AppUsers = AppUser_AppUserDTOs;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", $"AppUser_PhamViDiTuyen.xlsx");
        }

        [Route(AppUserRoute.ExportTemplateStore), HttpPost]
        public async Task<ActionResult> ExportTemplateStore()
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = int.MaxValue;
            AppUserFilter.OrderBy = AppUserOrder.Username;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.Id | AppUserSelect.Username | AppUserSelect.DisplayName;

            if (AppUserFilter.Id == null) AppUserFilter.Id = new IdFilter();
            AppUserFilter.Id.In = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<AppUser_AppUserDTO> AppUser_AppUserDTOs = new List<AppUser_AppUserDTO>();
            long STT = 1;
            foreach (var AppUser in AppUsers)
            {
                AppUser_AppUserDTO AppUser_AppUserDTO = new AppUser_AppUserDTO(AppUser);
                AppUser_AppUserDTO.STT = STT++;
                AppUser_AppUserDTOs.Add(AppUser_AppUserDTO);
            }

            string path = "Templates/AppUser_Store.xlsx";
            byte[] arr = System.IO.File.ReadAllBytes(path);
            MemoryStream input = new MemoryStream(arr);
            MemoryStream output = new MemoryStream();
            dynamic Data = new ExpandoObject();
            Data.AppUsers = AppUser_AppUserDTOs;
            using (var document = StaticParams.DocumentFactory.Open(input, output, "xlsx"))
            {
                document.Process(Data);
            };
            return File(output.ToArray(), "application/octet-stream", "Template_AppUser_Store.xlsx");
        }

        [Route(AppUserRoute.Export), HttpPost]
        public async Task<FileResult> Export([FromBody] AppUser_AppUserFilterDTO AppUser_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            var AppUserFilter = ConvertFilterDTOToFilterEntity(AppUser_AppUserFilterDTO);
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = int.MaxValue;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter = AppUserService.ToFilter(AppUserFilter);

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Organization> Organizations = await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.Code | OrganizationSelect.Name,
                OrderType = OrderType.ASC,
                OrderBy = OrganizationOrder.Id,
            });
            List<Sex> Sexes = await SexService.List(new SexFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = SexSelect.ALL
            });
            List<Role> Roles = await RoleService.List(new RoleFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = RoleSelect.Code | RoleSelect.Name
            });
            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region Appuser sheet
                var AppUserHeaders = new List<string>()
                {
                     "STT", "Mã nhân viên","Tên nhân viên","Địa chỉ","Điện thọi","Email","Giới tính","Chức vụ","Phòng ban","Bộ phận quản lý"
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
                        appUser.Position,
                        appUser.Department,
                        appUser.Organization.Code
                    });
                }
                excel.GenerateWorksheet("AppUser", AppUserHeaders, data);
                #endregion

                #region Org sheet
                data.Clear();
                var OrganizationHeader = new List<string>()
                {
                    "Mã",
                    "Tên"
                };
                foreach (var Organization in Organizations)
                {
                    data.Add(new object[]
                    {
                        Organization.Code,
                        Organization.Name,
                    });
                }
                excel.GenerateWorksheet("Organization", OrganizationHeader, data);
                #endregion

                #region Sex sheet
                data.Clear();
                var SexHeader = new List<string>()
                {
                        "Mã",
                        "Tên"
                };
                foreach (var Sex in Sexes)
                {
                    data.Add(new object[]
                    {
                        Sex.Code,
                        Sex.Name,
                    });
                }
                excel.GenerateWorksheet("Sex", SexHeader, data);
                #endregion

                #region Role sheet
                data.Clear();
                var RoleHeader = new List<string>()
                {
                        "Mã",
                        "Tên"
                };
                foreach (var Role in Roles)
                {
                    data.Add(new object[]
                    {
                        Role.Code,
                        Role.Name,
                    });
                }
                excel.GenerateWorksheet("Role", RoleHeader, data);
                #endregion
                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "AppUser.xlsx");
        }

        private async Task<bool> HasPermission(long Id)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter = AppUserService.ToFilter(AppUserFilter);
            if (Id == 0)
            {

            }
            else
            {
                AppUserFilter.Id = new IdFilter { Equal = Id };
                int count = await AppUserService.Count(AppUserFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private AppUser ConvertDTOToEntity(AppUser_AppUserDTO AppUser_AppUserDTO)
        {
            AppUser_AppUserDTO.TrimString();
            AppUser AppUser = new AppUser();
            AppUser.Id = AppUser_AppUserDTO.Id;
            AppUser.Username = AppUser_AppUserDTO.Username;
            AppUser.DisplayName = AppUser_AppUserDTO.DisplayName;
            AppUser.Address = AppUser_AppUserDTO.Address;
            AppUser.Avatar = AppUser_AppUserDTO.Avatar;
            AppUser.Birthday = AppUser_AppUserDTO.Birthday;
            AppUser.Email = AppUser_AppUserDTO.Email;
            AppUser.Phone = AppUser_AppUserDTO.Phone;
            AppUser.PositionId = AppUser_AppUserDTO.PositionId;
            AppUser.Department = AppUser_AppUserDTO.Department;
            AppUser.OrganizationId = AppUser_AppUserDTO.OrganizationId;
            AppUser.ProvinceId = AppUser_AppUserDTO.ProvinceId;
            AppUser.SexId = AppUser_AppUserDTO.SexId;
            AppUser.StatusId = AppUser_AppUserDTO.StatusId;
            AppUser.Organization = AppUser_AppUserDTO.Organization == null ? null : new Organization
            {
                Id = AppUser_AppUserDTO.Organization.Id,
                Code = AppUser_AppUserDTO.Organization.Code,
                Name = AppUser_AppUserDTO.Organization.Name,
                ParentId = AppUser_AppUserDTO.Organization.ParentId,
                Path = AppUser_AppUserDTO.Organization.Path,
                Level = AppUser_AppUserDTO.Organization.Level,
                StatusId = AppUser_AppUserDTO.Organization.StatusId,
                Phone = AppUser_AppUserDTO.Organization.Phone,
                Address = AppUser_AppUserDTO.Organization.Address,
                Email = AppUser_AppUserDTO.Organization.Email,
            };
            AppUser.Province = AppUser_AppUserDTO.Province == null ? null : new Province
            {
                Id = AppUser_AppUserDTO.Province.Id,
                Code = AppUser_AppUserDTO.Province.Code,
                Name = AppUser_AppUserDTO.Province.Name,
                Priority = AppUser_AppUserDTO.Province.Priority,
                StatusId = AppUser_AppUserDTO.Province.StatusId,
            };
            AppUser.Sex = AppUser_AppUserDTO.Sex == null ? null : new Sex
            {
                Id = AppUser_AppUserDTO.Sex.Id,
                Code = AppUser_AppUserDTO.Sex.Code,
                Name = AppUser_AppUserDTO.Sex.Name,
            };
            AppUser.Status = AppUser_AppUserDTO.Status == null ? null : new Status
            {
                Id = AppUser_AppUserDTO.Status.Id,
                Code = AppUser_AppUserDTO.Status.Code,
                Name = AppUser_AppUserDTO.Status.Name,
            };
            AppUser.AppUserRoleMappings = AppUser_AppUserDTO.AppUserRoleMappings?
                .Select(x => new AppUserRoleMapping
                {
                    RoleId = x.RoleId,
                    Role = x.Role == null ? null : new Role
                    {
                        Id = x.Role.Id,
                        Code = x.Role.Code,
                        Name = x.Role.Name,
                        StatusId = x.Role.StatusId,
                    },
                }).ToList();
            AppUser.AppUserStoreMappings = AppUser_AppUserDTO.AppUserStoreMappings?
                .Select(x => new AppUserStoreMapping
                {
                    StoreId = x.StoreId,
                    Store = x.Store == null ? null : new Store
                    {
                        Id = x.Store.Id,
                        Code = x.Store.Code,
                        CodeDraft = x.Store.CodeDraft,
                        Name = x.Store.Name,
                        StatusId = x.Store.StatusId,
                    },
                }).ToList();
            AppUser.BaseLanguage = CurrentContext.Language;
            return AppUser;
        }

        private AppUserFilter ConvertFilterDTOToFilterEntity(AppUser_AppUserFilterDTO AppUser_AppUserFilterDTO)
        {
            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Skip = AppUser_AppUserFilterDTO.Skip;
            AppUserFilter.Take = AppUser_AppUserFilterDTO.Take;
            AppUserFilter.OrderBy = AppUser_AppUserFilterDTO.OrderBy;
            AppUserFilter.OrderType = AppUser_AppUserFilterDTO.OrderType;

            AppUserFilter.Id = AppUser_AppUserFilterDTO.Id;
            AppUserFilter.Username = AppUser_AppUserFilterDTO.Username;
            AppUserFilter.Password = AppUser_AppUserFilterDTO.Password;
            AppUserFilter.DisplayName = AppUser_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = AppUser_AppUserFilterDTO.Address;
            AppUserFilter.Email = AppUser_AppUserFilterDTO.Email;
            AppUserFilter.Phone = AppUser_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = AppUser_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = AppUser_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = AppUser_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = AppUser_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = AppUser_AppUserFilterDTO.StatusId;
            AppUserFilter.ProvinceId = AppUser_AppUserFilterDTO.ProvinceId;
            AppUserFilter.UpdatedAt = AppUser_AppUserFilterDTO.UpdatedAt;
            return AppUserFilter;
        }
    }
}

