using DMS.Common;
using DMS.Entities;
using DMS.Services.MAppUser;
using DMS.Services.MBanner;
using DMS.Services.MImage;
using DMS.Services.MOrganization;
using DMS.Services.MStatus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc.banner
{
    public class BannerController : RpcController
    {
        private IAppUserService AppUserService;
        private IImageService ImageService;
        private IOrganizationService OrganizationService;
        private IStatusService StatusService;
        private IBannerService BannerService;
        private ICurrentContext CurrentContext;
        public BannerController(
            IAppUserService AppUserService,
            IImageService ImageService,
            IOrganizationService OrganizationService,
            IStatusService StatusService,
            IBannerService BannerService,
            ICurrentContext CurrentContext
        )
        {
            this.AppUserService = AppUserService;
            this.ImageService = ImageService;
            this.OrganizationService = OrganizationService;
            this.StatusService = StatusService;
            this.BannerService = BannerService;
            this.CurrentContext = CurrentContext;
        }

        [Route(BannerRoute.Count), HttpPost]
        public async Task<ActionResult<int>> Count([FromBody] Banner_BannerFilterDTO Banner_BannerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            BannerFilter BannerFilter = ConvertFilterDTOToFilterEntity(Banner_BannerFilterDTO);
            BannerFilter = BannerService.ToFilter(BannerFilter);
            int count = await BannerService.Count(BannerFilter);
            return count;
        }

        [Route(BannerRoute.List), HttpPost]
        public async Task<ActionResult<List<Banner_BannerDTO>>> List([FromBody] Banner_BannerFilterDTO Banner_BannerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            BannerFilter BannerFilter = ConvertFilterDTOToFilterEntity(Banner_BannerFilterDTO);
            BannerFilter = BannerService.ToFilter(BannerFilter);
            List<Banner> Banners = await BannerService.List(BannerFilter);
            List<Banner_BannerDTO> Banner_BannerDTOs = Banners
                .Select(c => new Banner_BannerDTO(c)).ToList();
            return Banner_BannerDTOs;
        }

        [Route(BannerRoute.Get), HttpPost]
        public async Task<ActionResult<Banner_BannerDTO>> Get([FromBody] Banner_BannerDTO Banner_BannerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Banner_BannerDTO.Id))
                return Forbid();

            Banner Banner = await BannerService.Get(Banner_BannerDTO.Id);
            return new Banner_BannerDTO(Banner);
        }

        [Route(BannerRoute.Create), HttpPost]
        public async Task<ActionResult<Banner_BannerDTO>> Create([FromBody] Banner_BannerDTO Banner_BannerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Banner_BannerDTO.Id))
                return Forbid();

            Banner Banner = ConvertDTOToEntity(Banner_BannerDTO);
            Banner = await BannerService.Create(Banner);
            Banner_BannerDTO = new Banner_BannerDTO(Banner);
            if (Banner.IsValidated)
                return Banner_BannerDTO;
            else
                return BadRequest(Banner_BannerDTO);
        }

        [Route(BannerRoute.Update), HttpPost]
        public async Task<ActionResult<Banner_BannerDTO>> Update([FromBody] Banner_BannerDTO Banner_BannerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Banner_BannerDTO.Id))
                return Forbid();

            Banner Banner = ConvertDTOToEntity(Banner_BannerDTO);
            Banner = await BannerService.Update(Banner);
            Banner_BannerDTO = new Banner_BannerDTO(Banner);
            if (Banner.IsValidated)
                return Banner_BannerDTO;
            else
                return BadRequest(Banner_BannerDTO);
        }

        [Route(BannerRoute.Delete), HttpPost]
        public async Task<ActionResult<Banner_BannerDTO>> Delete([FromBody] Banner_BannerDTO Banner_BannerDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            if (!await HasPermission(Banner_BannerDTO.Id))
                return Forbid();

            Banner Banner = ConvertDTOToEntity(Banner_BannerDTO);
            Banner = await BannerService.Delete(Banner);
            Banner_BannerDTO = new Banner_BannerDTO(Banner);
            if (Banner.IsValidated)
                return Banner_BannerDTO;
            else
                return BadRequest(Banner_BannerDTO);
        }

        [Route(BannerRoute.BulkDelete), HttpPost]
        public async Task<ActionResult<bool>> BulkDelete([FromBody] List<long> Ids)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            BannerFilter BannerFilter = new BannerFilter();
            BannerFilter = BannerService.ToFilter(BannerFilter);
            BannerFilter.Id = new IdFilter { In = Ids };
            BannerFilter.Selects = BannerSelect.Id;
            BannerFilter.Skip = 0;
            BannerFilter.Take = int.MaxValue;

            List<Banner> Banners = await BannerService.List(BannerFilter);
            Banners = await BannerService.BulkDelete(Banners);
            if (Banners.Any(x => !x.IsValidated))
                return BadRequest(Banners.Where(x => !x.IsValidated));
            return true;
        }

        [Route(BannerRoute.Import), HttpPost]
        public async Task<ActionResult> Import(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            AppUserFilter CreatorFilter = new AppUserFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.ALL
            };
            List<AppUser> Creators = await AppUserService.List(CreatorFilter);
            ImageFilter ImageFilter = new ImageFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = ImageSelect.ALL
            };
            List<Image> Images = await ImageService.List(ImageFilter);
            StatusFilter StatusFilter = new StatusFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = StatusSelect.ALL
            };
            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<Banner> Banners = new List<Banner>();
            using (ExcelPackage excelPackage = new ExcelPackage(file.OpenReadStream()))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets.FirstOrDefault();
                if (worksheet == null)
                    return Ok(Banners);
                int StartColumn = 1;
                int StartRow = 1;
                int IdColumn = 0 + StartColumn;
                int CodeColumn = 1 + StartColumn;
                int TitleColumn = 2 + StartColumn;
                int PriorityColumn = 3 + StartColumn;
                int ContentColumn = 4 + StartColumn;
                int CreatorIdColumn = 5 + StartColumn;
                int ImageIdColumn = 6 + StartColumn;
                int StatusIdColumn = 7 + StartColumn;

                for (int i = StartRow; i <= worksheet.Dimension.End.Row; i++)
                {
                    if (string.IsNullOrEmpty(worksheet.Cells[i + StartRow, StartColumn].Value?.ToString()))
                        break;
                    string IdValue = worksheet.Cells[i + StartRow, IdColumn].Value?.ToString();
                    string CodeValue = worksheet.Cells[i + StartRow, CodeColumn].Value?.ToString();
                    string TitleValue = worksheet.Cells[i + StartRow, TitleColumn].Value?.ToString();
                    string PriorityValue = worksheet.Cells[i + StartRow, PriorityColumn].Value?.ToString();
                    string ContentValue = worksheet.Cells[i + StartRow, ContentColumn].Value?.ToString();
                    string CreatorIdValue = worksheet.Cells[i + StartRow, CreatorIdColumn].Value?.ToString();
                    string ImageIdValue = worksheet.Cells[i + StartRow, ImageIdColumn].Value?.ToString();
                    string StatusIdValue = worksheet.Cells[i + StartRow, StatusIdColumn].Value?.ToString();

                    Banner Banner = new Banner();
                    Banner.Code = CodeValue;
                    Banner.Title = TitleValue;
                    Banner.Priority = long.TryParse(PriorityValue, out long Priority) ? Priority : 0;
                    Banner.Content = ContentValue;
                    AppUser Creator = Creators.Where(x => x.Id.ToString() == CreatorIdValue).FirstOrDefault();
                    Banner.CreatorId = Creator == null ? 0 : Creator.Id;
                    Banner.Creator = Creator;
                    Image Image = Images.Where(x => x.Id.ToString() == ImageIdValue).FirstOrDefault();
                    Banner.ImageId = Image == null ? 0 : Image.Id;
                    Banner.Image = Image;
                    Status Status = Statuses.Where(x => x.Id.ToString() == StatusIdValue).FirstOrDefault();
                    Banner.StatusId = Status == null ? 0 : Status.Id;
                    Banner.Status = Status;

                    Banners.Add(Banner);
                }
            }
            Banners = await BannerService.Import(Banners);
            if (Banners.All(x => x.IsValidated))
                return Ok(true);
            else
            {
                List<string> Errors = new List<string>();
                for (int i = 0; i < Banners.Count; i++)
                {
                    Banner Banner = Banners[i];
                    if (!Banner.IsValidated)
                    {
                        string Error = $"Dòng {i + 2} có lỗi:";
                        if (Banner.Errors.ContainsKey(nameof(Banner.Id)))
                            Error += Banner.Errors[nameof(Banner.Id)];
                        if (Banner.Errors.ContainsKey(nameof(Banner.Code)))
                            Error += Banner.Errors[nameof(Banner.Code)];
                        if (Banner.Errors.ContainsKey(nameof(Banner.Title)))
                            Error += Banner.Errors[nameof(Banner.Title)];
                        if (Banner.Errors.ContainsKey(nameof(Banner.Priority)))
                            Error += Banner.Errors[nameof(Banner.Priority)];
                        if (Banner.Errors.ContainsKey(nameof(Banner.Content)))
                            Error += Banner.Errors[nameof(Banner.Content)];
                        if (Banner.Errors.ContainsKey(nameof(Banner.CreatorId)))
                            Error += Banner.Errors[nameof(Banner.CreatorId)];
                        if (Banner.Errors.ContainsKey(nameof(Banner.ImageId)))
                            Error += Banner.Errors[nameof(Banner.ImageId)];
                        if (Banner.Errors.ContainsKey(nameof(Banner.StatusId)))
                            Error += Banner.Errors[nameof(Banner.StatusId)];
                        Errors.Add(Error);
                    }
                }
                return BadRequest(Errors);
            }
        }

        [Route(BannerRoute.Export), HttpPost]
        public async Task<FileResult> Export([FromBody] Banner_BannerFilterDTO Banner_BannerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region Banner
                var BannerFilter = ConvertFilterDTOToFilterEntity(Banner_BannerFilterDTO);
                BannerFilter.Skip = 0;
                BannerFilter.Take = int.MaxValue;
                BannerFilter = BannerService.ToFilter(BannerFilter);
                List<Banner> Banners = await BannerService.List(BannerFilter);

                var BannerHeaders = new List<string>()
                {
                        "Id",
                        "Code",
                        "Title",
                        "Priority",
                        "Content",
                        "CreatorId",
                        "ImageId",
                        "StatusId",
                };
                List<object[]> BannerData = new List<object[]>();
                for (int i = 0; i < Banners.Count; i++)
                {
                    var Banner = Banners[i];
                    BannerData.Add(new Object[]
                    {
                        Banner.Id,
                        Banner.Code,
                        Banner.Title,
                        Banner.Priority,
                        Banner.Content,
                        Banner.CreatorId,
                        Banner.ImageId,
                        Banner.StatusId,
                    });
                }
                excel.GenerateWorksheet("Banner", BannerHeaders, BannerData);
                #endregion

                #region AppUser
                var AppUserFilter = new AppUserFilter();
                AppUserFilter.Selects = AppUserSelect.ALL;
                AppUserFilter.OrderBy = AppUserOrder.Id;
                AppUserFilter.OrderType = OrderType.ASC;
                AppUserFilter.Skip = 0;
                AppUserFilter.Take = int.MaxValue;
                List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);

                var AppUserHeaders = new List<string>()
                {
                        "Id",
                        "Username",
                        "Password",
                        "DisplayName",
                        "Address",
                        "Email",
                        "Phone",
                        "Position",
                        "Department",
                        "OrganizationId",
                        "SexId",
                        "StatusId",
                        "Avatar",
                        "Birthday",
                        "ProvinceId",
                };
                List<object[]> AppUserData = new List<object[]>();
                for (int i = 0; i < AppUsers.Count; i++)
                {
                    var AppUser = AppUsers[i];
                    AppUserData.Add(new Object[]
                    {
                        AppUser.Id,
                        AppUser.Username,
                        AppUser.DisplayName,
                        AppUser.Address,
                        AppUser.Email,
                        AppUser.Phone,
                        AppUser.Position,
                        AppUser.Department,
                        AppUser.OrganizationId,
                        AppUser.SexId,
                        AppUser.StatusId,
                        AppUser.Avatar,
                        AppUser.Birthday,
                        AppUser.ProvinceId,
                    });
                }
                excel.GenerateWorksheet("AppUser", AppUserHeaders, AppUserData);
                #endregion
                #region Image
                var ImageFilter = new ImageFilter();
                ImageFilter.Selects = ImageSelect.ALL;
                ImageFilter.OrderBy = ImageOrder.Id;
                ImageFilter.OrderType = OrderType.ASC;
                ImageFilter.Skip = 0;
                ImageFilter.Take = int.MaxValue;
                List<Image> Images = await ImageService.List(ImageFilter);

                var ImageHeaders = new List<string>()
                {
                        "Id",
                        "Name",
                        "Url",
                };
                List<object[]> ImageData = new List<object[]>();
                for (int i = 0; i < Images.Count; i++)
                {
                    var Image = Images[i];
                    ImageData.Add(new Object[]
                    {
                        Image.Id,
                        Image.Name,
                        Image.Url,
                    });
                }
                excel.GenerateWorksheet("Image", ImageHeaders, ImageData);
                #endregion
                #region Status
                var StatusFilter = new StatusFilter();
                StatusFilter.Selects = StatusSelect.ALL;
                StatusFilter.OrderBy = StatusOrder.Id;
                StatusFilter.OrderType = OrderType.ASC;
                StatusFilter.Skip = 0;
                StatusFilter.Take = int.MaxValue;
                List<Status> Statuses = await StatusService.List(StatusFilter);

                var StatusHeaders = new List<string>()
                {
                        "Id",
                        "Code",
                        "Name",
                };
                List<object[]> StatusData = new List<object[]>();
                for (int i = 0; i < Statuses.Count; i++)
                {
                    var Status = Statuses[i];
                    StatusData.Add(new Object[]
                    {
                        Status.Id,
                        Status.Code,
                        Status.Name,
                    });
                }
                excel.GenerateWorksheet("Status", StatusHeaders, StatusData);
                #endregion
                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "Banner.xlsx");
        }

        [Route(BannerRoute.ExportTemplate), HttpPost]
        public async Task<FileResult> ExportTemplate([FromBody] Banner_BannerFilterDTO Banner_BannerFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            MemoryStream memoryStream = new MemoryStream();
            using (ExcelPackage excel = new ExcelPackage(memoryStream))
            {
                #region Banner
                var BannerHeaders = new List<string>()
                {
                        "Id",
                        "Code",
                        "Title",
                        "Priority",
                        "Content",
                        "CreatorId",
                        "ImageId",
                        "StatusId",
                };
                List<object[]> BannerData = new List<object[]>();
                excel.GenerateWorksheet("Banner", BannerHeaders, BannerData);
                #endregion

                #region AppUser
                var AppUserFilter = new AppUserFilter();
                AppUserFilter.Selects = AppUserSelect.ALL;
                AppUserFilter.OrderBy = AppUserOrder.Id;
                AppUserFilter.OrderType = OrderType.ASC;
                AppUserFilter.Skip = 0;
                AppUserFilter.Take = int.MaxValue;
                List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);

                var AppUserHeaders = new List<string>()
                {
                        "Id",
                        "Username",
                        "Password",
                        "DisplayName",
                        "Address",
                        "Email",
                        "Phone",
                        "Position",
                        "Department",
                        "OrganizationId",
                        "SexId",
                        "StatusId",
                        "Avatar",
                        "Birthday",
                        "ProvinceId",
                };
                List<object[]> AppUserData = new List<object[]>();
                for (int i = 0; i < AppUsers.Count; i++)
                {
                    var AppUser = AppUsers[i];
                    AppUserData.Add(new Object[]
                    {
                        AppUser.Id,
                        AppUser.Username,
                        AppUser.DisplayName,
                        AppUser.Address,
                        AppUser.Email,
                        AppUser.Phone,
                        AppUser.Position,
                        AppUser.Department,
                        AppUser.OrganizationId,
                        AppUser.SexId,
                        AppUser.StatusId,
                        AppUser.Avatar,
                        AppUser.Birthday,
                        AppUser.ProvinceId,
                    });
                }
                excel.GenerateWorksheet("AppUser", AppUserHeaders, AppUserData);
                #endregion
                #region Image
                var ImageFilter = new ImageFilter();
                ImageFilter.Selects = ImageSelect.ALL;
                ImageFilter.OrderBy = ImageOrder.Id;
                ImageFilter.OrderType = OrderType.ASC;
                ImageFilter.Skip = 0;
                ImageFilter.Take = int.MaxValue;
                List<Image> Images = await ImageService.List(ImageFilter);

                var ImageHeaders = new List<string>()
                {
                        "Id",
                        "Name",
                        "Url",
                };
                List<object[]> ImageData = new List<object[]>();
                for (int i = 0; i < Images.Count; i++)
                {
                    var Image = Images[i];
                    ImageData.Add(new Object[]
                    {
                        Image.Id,
                        Image.Name,
                        Image.Url,
                    });
                }
                excel.GenerateWorksheet("Image", ImageHeaders, ImageData);
                #endregion
                #region Status
                var StatusFilter = new StatusFilter();
                StatusFilter.Selects = StatusSelect.ALL;
                StatusFilter.OrderBy = StatusOrder.Id;
                StatusFilter.OrderType = OrderType.ASC;
                StatusFilter.Skip = 0;
                StatusFilter.Take = int.MaxValue;
                List<Status> Statuses = await StatusService.List(StatusFilter);

                var StatusHeaders = new List<string>()
                {
                        "Id",
                        "Code",
                        "Name",
                };
                List<object[]> StatusData = new List<object[]>();
                for (int i = 0; i < Statuses.Count; i++)
                {
                    var Status = Statuses[i];
                    StatusData.Add(new Object[]
                    {
                        Status.Id,
                        Status.Code,
                        Status.Name,
                    });
                }
                excel.GenerateWorksheet("Status", StatusHeaders, StatusData);
                #endregion
                excel.Save();
            }
            return File(memoryStream.ToArray(), "application/octet-stream", "Banner.xlsx");
        }

        [HttpPost]
        [Route(BannerRoute.SaveImage)]
        public async Task<ActionResult<Banner_ImageDTO>> SaveImage(IFormFile file)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);
            MemoryStream memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            Image Image = new Image
            {
                Name = file.FileName,
                Content = memoryStream.ToArray()
            };
            Image = await BannerService.SaveImage(Image);
            if (Image == null)
                return BadRequest();
            Banner_ImageDTO Banner_ImageDTO = new Banner_ImageDTO
            {
                Id = Image.Id,
                Name = Image.Name,
                Url = Image.Url,
            };
            return Ok(Banner_ImageDTO);
        }

        private async Task<bool> HasPermission(long Id)
        {
            BannerFilter BannerFilter = new BannerFilter();
            BannerFilter = BannerService.ToFilter(BannerFilter);
            if (Id == 0)
            {

            }
            else
            {
                BannerFilter.Id = new IdFilter { Equal = Id };
                int count = await BannerService.Count(BannerFilter);
                if (count == 0)
                    return false;
            }
            return true;
        }

        private Banner ConvertDTOToEntity(Banner_BannerDTO Banner_BannerDTO)
        {
            Banner_BannerDTO.TrimString();
            Banner Banner = new Banner();
            Banner.Id = Banner_BannerDTO.Id;
            Banner.Code = Banner_BannerDTO.Code;
            Banner.Title = Banner_BannerDTO.Title;
            Banner.Priority = Banner_BannerDTO.Priority;
            Banner.Content = Banner_BannerDTO.Content;
            Banner.CreatorId = Banner_BannerDTO.CreatorId;
            Banner.OrganizationId = Banner_BannerDTO.OrganizationId;
            Banner.ImageId = Banner_BannerDTO.ImageId;
            Banner.StatusId = Banner_BannerDTO.StatusId;
            Banner.Creator = Banner_BannerDTO.Creator == null ? null : new AppUser
            {
                Id = Banner_BannerDTO.Creator.Id,
                Username = Banner_BannerDTO.Creator.Username,
                DisplayName = Banner_BannerDTO.Creator.DisplayName,
                Address = Banner_BannerDTO.Creator.Address,
                Email = Banner_BannerDTO.Creator.Email,
                Phone = Banner_BannerDTO.Creator.Phone,
                PositionId = Banner_BannerDTO.Creator.PositionId,
                Department = Banner_BannerDTO.Creator.Department,
                OrganizationId = Banner_BannerDTO.Creator.OrganizationId,
                SexId = Banner_BannerDTO.Creator.SexId,
                StatusId = Banner_BannerDTO.Creator.StatusId,
                Birthday = Banner_BannerDTO.Creator.Birthday,
                ProvinceId = Banner_BannerDTO.Creator.ProvinceId,
            };
            Banner.Organization = Banner_BannerDTO.Organization == null ? null : new Organization
            {
                Id = Banner_BannerDTO.Organization.Id,
                Code = Banner_BannerDTO.Organization.Code,
                Name = Banner_BannerDTO.Organization.Name,
                ParentId = Banner_BannerDTO.Organization.ParentId,
                Path = Banner_BannerDTO.Organization.Path,
                Level = Banner_BannerDTO.Organization.Level,
                StatusId = Banner_BannerDTO.Organization.StatusId,
                Phone = Banner_BannerDTO.Organization.Phone,
                Address = Banner_BannerDTO.Organization.Address,
                Email = Banner_BannerDTO.Organization.Email,
            };
            Banner.Image = Banner_BannerDTO.Images == null ? null : Banner_BannerDTO.Images.Select(x => new Image
            {
                Id = x.Id,
                Name = x.Name,
                Url = x.Url,
            }).FirstOrDefault();
            Banner.Status = Banner_BannerDTO.Status == null ? null : new Status
            {
                Id = Banner_BannerDTO.Status.Id,
                Code = Banner_BannerDTO.Status.Code,
                Name = Banner_BannerDTO.Status.Name,
            };
            Banner.BaseLanguage = CurrentContext.Language;
            return Banner;
        }

        private BannerFilter ConvertFilterDTOToFilterEntity(Banner_BannerFilterDTO Banner_BannerFilterDTO)
        {
            BannerFilter BannerFilter = new BannerFilter();
            BannerFilter.Selects = BannerSelect.ALL;
            BannerFilter.Skip = Banner_BannerFilterDTO.Skip;
            BannerFilter.Take = Banner_BannerFilterDTO.Take;
            BannerFilter.OrderBy = Banner_BannerFilterDTO.OrderBy;
            BannerFilter.OrderType = Banner_BannerFilterDTO.OrderType;

            BannerFilter.Id = Banner_BannerFilterDTO.Id;
            BannerFilter.Code = Banner_BannerFilterDTO.Code;
            BannerFilter.Title = Banner_BannerFilterDTO.Title;
            BannerFilter.Priority = Banner_BannerFilterDTO.Priority;
            BannerFilter.Content = Banner_BannerFilterDTO.Content;
            BannerFilter.CreatorId = Banner_BannerFilterDTO.CreatorId;
            BannerFilter.OrganizationId = Banner_BannerFilterDTO.OrganizationId;
            BannerFilter.ImageId = Banner_BannerFilterDTO.ImageId;
            BannerFilter.StatusId = Banner_BannerFilterDTO.StatusId;
            BannerFilter.CreatedAt = Banner_BannerFilterDTO.CreatedAt;
            BannerFilter.UpdatedAt = Banner_BannerFilterDTO.UpdatedAt;
            return BannerFilter;
        }

        [Route(BannerRoute.FilterListAppUser), HttpPost]
        public async Task<List<Banner_AppUserDTO>> FilterListAppUser([FromBody] Banner_AppUserFilterDTO Banner_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = Banner_AppUserFilterDTO.Id;
            AppUserFilter.Username = Banner_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = Banner_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = Banner_AppUserFilterDTO.Address;
            AppUserFilter.Email = Banner_AppUserFilterDTO.Email;
            AppUserFilter.Phone = Banner_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = Banner_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = Banner_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = Banner_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = Banner_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = Banner_AppUserFilterDTO.StatusId;
            AppUserFilter.Birthday = Banner_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = Banner_AppUserFilterDTO.ProvinceId;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Banner_AppUserDTO> Banner_AppUserDTOs = AppUsers
                .Select(x => new Banner_AppUserDTO(x)).ToList();
            return Banner_AppUserDTOs;
        }

        [Route(BannerRoute.FilterListOrganization), HttpPost]
        public async Task<List<Banner_OrganizationDTO>> FilterListOrganization([FromBody] Banner_OrganizationFilterDTO Banner_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = Banner_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = Banner_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = Banner_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = Banner_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = Banner_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = Banner_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = Banner_OrganizationFilterDTO.StatusId;
            OrganizationFilter.Phone = Banner_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = Banner_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = Banner_OrganizationFilterDTO.Email;
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
                        if (FilterPermissionDefinition.Name == nameof(BannerFilter.OrganizationId))
                            subFilter.Id = FilterPermissionDefinition.IdFilter;
                    }
                }
            }

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Banner_OrganizationDTO> Banner_OrganizationDTOs = Organizations
                .Select(x => new Banner_OrganizationDTO(x)).ToList();
            return Banner_OrganizationDTOs;
        }

        [Route(BannerRoute.FilterListStatus), HttpPost]
        public async Task<List<Banner_StatusDTO>> FilterListStatus([FromBody] Banner_StatusFilterDTO Banner_StatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<Banner_StatusDTO> Banner_StatusDTOs = Statuses
                .Select(x => new Banner_StatusDTO(x)).ToList();
            return Banner_StatusDTOs;
        }

        [Route(BannerRoute.SingleListAppUser), HttpPost]
        public async Task<List<Banner_AppUserDTO>> SingleListAppUser([FromBody] Banner_AppUserFilterDTO Banner_AppUserFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            AppUserFilter AppUserFilter = new AppUserFilter();
            AppUserFilter.Skip = 0;
            AppUserFilter.Take = 20;
            AppUserFilter.OrderBy = AppUserOrder.Id;
            AppUserFilter.OrderType = OrderType.ASC;
            AppUserFilter.Selects = AppUserSelect.ALL;
            AppUserFilter.Id = Banner_AppUserFilterDTO.Id;
            AppUserFilter.Username = Banner_AppUserFilterDTO.Username;
            AppUserFilter.DisplayName = Banner_AppUserFilterDTO.DisplayName;
            AppUserFilter.Address = Banner_AppUserFilterDTO.Address;
            AppUserFilter.Email = Banner_AppUserFilterDTO.Email;
            AppUserFilter.Phone = Banner_AppUserFilterDTO.Phone;
            AppUserFilter.PositionId = Banner_AppUserFilterDTO.PositionId;
            AppUserFilter.Department = Banner_AppUserFilterDTO.Department;
            AppUserFilter.OrganizationId = Banner_AppUserFilterDTO.OrganizationId;
            AppUserFilter.SexId = Banner_AppUserFilterDTO.SexId;
            AppUserFilter.StatusId = Banner_AppUserFilterDTO.StatusId;
            AppUserFilter.Birthday = Banner_AppUserFilterDTO.Birthday;
            AppUserFilter.ProvinceId = Banner_AppUserFilterDTO.ProvinceId;

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<Banner_AppUserDTO> Banner_AppUserDTOs = AppUsers
                .Select(x => new Banner_AppUserDTO(x)).ToList();
            return Banner_AppUserDTOs;
        }

        [Route(BannerRoute.SingleListOrganization), HttpPost]
        public async Task<List<Banner_OrganizationDTO>> SingleListOrganization([FromBody] Banner_OrganizationFilterDTO Banner_OrganizationFilterDTO)
        {
            OrganizationFilter OrganizationFilter = new OrganizationFilter();
            OrganizationFilter.Skip = 0;
            OrganizationFilter.Take = 99999;
            OrganizationFilter.OrderBy = OrganizationOrder.Id;
            OrganizationFilter.OrderType = OrderType.ASC;
            OrganizationFilter.Selects = OrganizationSelect.ALL;
            OrganizationFilter.Id = Banner_OrganizationFilterDTO.Id;
            OrganizationFilter.Code = Banner_OrganizationFilterDTO.Code;
            OrganizationFilter.Name = Banner_OrganizationFilterDTO.Name;
            OrganizationFilter.ParentId = Banner_OrganizationFilterDTO.ParentId;
            OrganizationFilter.Path = Banner_OrganizationFilterDTO.Path;
            OrganizationFilter.Level = Banner_OrganizationFilterDTO.Level;
            OrganizationFilter.StatusId = new IdFilter { Equal = Enums.StatusEnum.ACTIVE.Id };
            OrganizationFilter.Phone = Banner_OrganizationFilterDTO.Phone;
            OrganizationFilter.Address = Banner_OrganizationFilterDTO.Address;
            OrganizationFilter.Email = Banner_OrganizationFilterDTO.Email;
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
                        if (FilterPermissionDefinition.Name == nameof(BannerFilter.OrganizationId))
                            subFilter.Id = FilterPermissionDefinition.IdFilter;
                    }
                }
            }

            List<Organization> Organizations = await OrganizationService.List(OrganizationFilter);
            List<Banner_OrganizationDTO> Banner_OrganizationDTOs = Organizations
                .Select(x => new Banner_OrganizationDTO(x)).ToList();
            return Banner_OrganizationDTOs;
        }

        [Route(BannerRoute.SingleListStatus), HttpPost]
        public async Task<List<Banner_StatusDTO>> SingleListStatus([FromBody] Banner_StatusFilterDTO Banner_StatusFilterDTO)
        {
            if (!ModelState.IsValid)
                throw new BindException(ModelState);

            StatusFilter StatusFilter = new StatusFilter();
            StatusFilter.Skip = 0;
            StatusFilter.Take = int.MaxValue;
            StatusFilter.Take = 20;
            StatusFilter.OrderBy = StatusOrder.Id;
            StatusFilter.OrderType = OrderType.ASC;
            StatusFilter.Selects = StatusSelect.ALL;

            List<Status> Statuses = await StatusService.List(StatusFilter);
            List<Banner_StatusDTO> Banner_StatusDTOs = Statuses
                .Select(x => new Banner_StatusDTO(x)).ToList();
            return Banner_StatusDTOs;
        }

    }
}

