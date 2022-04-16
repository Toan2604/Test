using DMS.ABE;
using DMS.ABE.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Models;
using DMS.ABE.Rpc;
using DMS.ABE.Services.MImage;
using LightBDD.NUnit3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using NUnit.Framework;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using TrueSight.Common;
using DMS.ABE.Handlers.Configuration;

namespace DMS.ABE.Tests
{
    public class BaseTests : FeatureFixture
    {
        public ServiceProvider ServiceProvider;
        public void Init()
        {
            _ = License.EfExtension;
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            IRabbitManager RabbitManager = new MockRabbitManager();
            ILogging Logging = new MockLogging();
            IImageService ImageService = new MockImageService();
            ICurrentContext CurrentContext = new CurrentContext();
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.Tests.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();
            _ = DataEntity.InformationResource;
            _ = DataEntity.WarningResource;
            _ = DataEntity.ErrorResource;
            IHostEnvironment env = new HostingEnvironment();
            env.EnvironmentName = "Tests";
            env.ContentRootPath = System.Environment.CurrentDirectory;
            try
            {
                Startup startup = new Startup(env);
                IServiceCollection serviceCollection = new ServiceCollection();
                serviceCollection.AddSingleton<IConfiguration>(Configuration);
                serviceCollection.AddSingleton<ICurrentContext>(CurrentContext);
                startup.ConfigureServices(serviceCollection);
                var controllers = typeof(Startup).Assembly.ExportedTypes
                    .Where(x => !x.IsAbstract && typeof(ControllerBase).IsAssignableFrom(x)).ToList();
                // By default, the controllers are not loaded so this is necessary
                controllers.ForEach(c => serviceCollection.AddScoped(c));
                serviceCollection.Replace<IRabbitManager, MockRabbitManager>(ServiceLifetime.Scoped);
                serviceCollection.Replace<ILogging, MockLogging>(ServiceLifetime.Scoped);
                serviceCollection.Replace<IImageService, MockImageService>(ServiceLifetime.Scoped);
                ServiceProvider = serviceCollection.BuildServiceProvider();
                Clean();
                InitData();
            }
            catch (Exception ex)
            {
            }
        }
        [TearDown]
        public void Clean()
        {
            DataContext DataContext = ServiceProvider.GetService<DataContext>();
            DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'");
            DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'IF OBJECTPROPERTY(object_id(''?''), ''TableHasIdentity'') = 1 DBCC CHECKIDENT(''?'', RESEED, 0)'");
            DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");
        }
        private void InitData()
        {
            var CurrentContext = ServiceProvider.GetService<ICurrentContext>();
            CurrentContext.Language = "vi";
            CurrentContext.TimeZone = 7;
            CurrentContext.StoreUserId = 118;
            var SetupController = ServiceProvider.GetService<SetupController>();
            SetupController.Init();
        }
        protected MemoryStream ReadFile(string path)
        {
            try
            {
                string host = "http://192.168.20.200";
                WebClient WebClient = new WebClient();
                byte[] array = WebClient.DownloadData(host + path);
                MemoryStream MemoryStream = new MemoryStream(array);
                return MemoryStream;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region insert permission
        private DataContext DataContext;

        protected async Task LoadPermission(string path)
        {
            this.DataContext = ServiceProvider.GetService<DataContext>();
            System.IO.MemoryStream MemoryStream = ReadFile(path);
            using (ExcelPackage excelPackage = new ExcelPackage(MemoryStream))
            {
                ExcelWorksheet wsStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Status)).FirstOrDefault();
                if (wsStatus != null)
                    await Given_Status(wsStatus);
                ExcelWorksheet wsStoreStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreStatus)).FirstOrDefault();
                if (wsStoreStatus != null)
                    await Given_StoreStatus(wsStoreStatus);
                ExcelWorksheet wsStoreScoutingStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreScoutingStatus)).FirstOrDefault();
                if (wsStoreScoutingStatus != null)
                    await Given_StoreScoutingStatus(wsStoreScoutingStatus);
                ExcelWorksheet wsStoreScoutingType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreScoutingType)).FirstOrDefault();
                if (wsStoreScoutingType != null)
                    await Given_StoreScoutingType(wsStoreScoutingType);
                ExcelWorksheet wsColor = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Color)).FirstOrDefault();
                if (wsColor != null)
                    await Given_Color(wsColor);
                ExcelWorksheet wsSex = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Sex)).FirstOrDefault();
                if (wsSex != null)
                    await Given_Sex(wsSex);
                ExcelWorksheet wsOrganization = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Organization)).FirstOrDefault();
                if (wsOrganization != null)
                    await Given_Organization(wsOrganization);
                ExcelWorksheet wsAppUser = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(AppUser)).FirstOrDefault();
                if (wsAppUser != null)
                    await Given_AppUser(wsAppUser);
                ExcelWorksheet wsStoreGrouping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreGrouping)).FirstOrDefault();
                if (wsStoreGrouping != null)
                    await Given_StoreGrouping(wsStoreGrouping);
                ExcelWorksheet wsStoreType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreType)).FirstOrDefault();
                if (wsStoreType != null)
                    await Given_StoreType(wsStoreType);
                ExcelWorksheet wsProvince = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Province)).FirstOrDefault();
                if (wsProvince != null)
                    await Given_Province(wsProvince);
                ExcelWorksheet wsDistrict = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(District)).FirstOrDefault();
                if (wsDistrict != null)
                    await Given_District(wsDistrict);
                ExcelWorksheet wsWard = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Ward)).FirstOrDefault();
                if (wsWard != null)
                    await Given_Ward(wsWard);
                ExcelWorksheet wsStoreScouting = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreScouting)).FirstOrDefault();
                if (wsStoreScouting != null)
                    await Given_StoreScouting(wsStoreScouting);
                ExcelWorksheet wsStore = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Store)).FirstOrDefault();
                if (wsStore != null)
                    await Given_Store(wsStore);
                ExcelWorksheet wsStoreUser = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(StoreUser)).FirstOrDefault();
                if (wsStoreUser != null)
                    await Given_StoreUser(wsStoreUser);
            }
        }
        private List<OrganizationDAO> OrganizationDAOs { get; set; }
        private List<StatusDAO> StatusDAOs { get; set; }
        private List<StoreScoutingDAO> StoreScoutingDAOs { get; set; }
        private List<StoreStatusDAO> StoreStatusDAOs { get; set; }
        private List<AppUserDAO> AppUserDAOs { get; set; }
        private List<StoreDAO> StoreDAOs { get; set; }
        private List<StoreUserDAO> StoreUserDAOs { get; set; }
        private List<SexDAO> SexDAOs { get; set; }
        private List<ColorDAO> ColorDAOs { get; set; }
        private List<StoreGroupingDAO> StoreGroupingDAOs { get; set; }
        private List<StoreTypeDAO> StoreTypeDAOs { get; set; }
        private List<StoreScoutingStatusDAO> StoreScoutingStatusDAOs { get; set; }
        private List<StoreScoutingTypeDAO> StoreScoutingTypeDAOs { get; set; }
        private List<DistrictDAO> DistrictDAOs { get; set; }
        private List<ProvinceDAO> ProvinceDAOs { get; set; }
        private List<WardDAO> WardDAOs { get; set; }

        private async Task Given_AppUser(ExcelWorksheet ExcelWorksheet)
        {
            this.AppUserDAOs = new List<AppUserDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int UsernameColumn = StartColumn + columns.IndexOf("Username");
            int DisplayNameColumn = StartColumn + columns.IndexOf("DisplayName");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int EmailColumn = StartColumn + columns.IndexOf("Email");
            int PhoneColumn = StartColumn + columns.IndexOf("Phone");
            int SexIdColumn = StartColumn + columns.IndexOf("SexId");
            int BirthdayColumn = StartColumn + columns.IndexOf("Birthday");
            int AvatarColumn = StartColumn + columns.IndexOf("Avatar");
            int PositionIdColumn = StartColumn + columns.IndexOf("PositionId");
            int DepartmentColumn = StartColumn + columns.IndexOf("Department");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                AppUserDAO AppUserDAO = new AppUserDAO();
                AppUserDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                AppUserDAO.Username = ExcelWorksheet.Cells[row, UsernameColumn].Value?.ParseString();
                AppUserDAO.DisplayName = ExcelWorksheet.Cells[row, DisplayNameColumn].Value?.ParseString();
                AppUserDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                AppUserDAO.Email = ExcelWorksheet.Cells[row, EmailColumn].Value?.ParseString();
                AppUserDAO.Phone = ExcelWorksheet.Cells[row, PhoneColumn].Value?.ParseString();
                AppUserDAO.SexId = ExcelWorksheet.Cells[row, SexIdColumn].Value?.ParseLong() ?? 0;
                AppUserDAO.Birthday = ExcelWorksheet.Cells[row, BirthdayColumn].Value?.ParseNullDateTime();
                AppUserDAO.Avatar = ExcelWorksheet.Cells[row, AvatarColumn].Value?.ParseString();
                AppUserDAO.PositionId = ExcelWorksheet.Cells[row, PositionIdColumn].Value?.ParseNullLong();
                AppUserDAO.Department = ExcelWorksheet.Cells[row, DepartmentColumn].Value?.ParseString();
                AppUserDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                AppUserDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                AppUserDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                AppUserDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                AppUserDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                AppUserDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                AppUserDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                AppUserDAOs.Add(AppUserDAO);
            }
            await DataContext.AppUser.BulkMergeAsync(AppUserDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_Color(ExcelWorksheet ExcelWorksheet)
        {
            this.ColorDAOs = new List<ColorDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ColorDAO ColorDAO = new ColorDAO();
                ColorDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ColorDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ColorDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ColorDAOs.Add(ColorDAO);
            }
            await DataContext.Color.BulkMergeAsync(ColorDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_District(ExcelWorksheet ExcelWorksheet)
        {
            this.DistrictDAOs = new List<DistrictDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PriorityColumn = StartColumn + columns.IndexOf("Priority");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                DistrictDAO DistrictDAO = new DistrictDAO();
                DistrictDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                DistrictDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                DistrictDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                DistrictDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseNullLong();
                DistrictDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseLong() ?? 0;
                DistrictDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                DistrictDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                DistrictDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                DistrictDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                DistrictDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                DistrictDAOs.Add(DistrictDAO);
            }
            await DataContext.District.BulkMergeAsync(DistrictDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_Organization(ExcelWorksheet ExcelWorksheet)
        {
            this.OrganizationDAOs = new List<OrganizationDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ParentIdColumn = StartColumn + columns.IndexOf("ParentId");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int LevelColumn = StartColumn + columns.IndexOf("Level");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int PhoneColumn = StartColumn + columns.IndexOf("Phone");
            int EmailColumn = StartColumn + columns.IndexOf("Email");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int IsDisplayColumn = StartColumn + columns.IndexOf("IsDisplay");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                OrganizationDAO OrganizationDAO = new OrganizationDAO();
                OrganizationDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                OrganizationDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                OrganizationDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                OrganizationDAO.ParentId = ExcelWorksheet.Cells[row, ParentIdColumn].Value?.ParseNullLong();
                OrganizationDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                OrganizationDAO.Level = ExcelWorksheet.Cells[row, LevelColumn].Value?.ParseLong() ?? 0;
                OrganizationDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                OrganizationDAO.Phone = ExcelWorksheet.Cells[row, PhoneColumn].Value?.ParseString();
                OrganizationDAO.Email = ExcelWorksheet.Cells[row, EmailColumn].Value?.ParseString();
                OrganizationDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                OrganizationDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                OrganizationDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                OrganizationDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                OrganizationDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                OrganizationDAOs.Add(OrganizationDAO);
            }
            await DataContext.Organization.BulkMergeAsync(OrganizationDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_Store(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreDAOs = new List<StoreDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int CodeDraftColumn = StartColumn + columns.IndexOf("CodeDraft");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int UnsignNameColumn = StartColumn + columns.IndexOf("UnsignName");
            int ParentStoreIdColumn = StartColumn + columns.IndexOf("ParentStoreId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int StoreTypeIdColumn = StartColumn + columns.IndexOf("StoreTypeId");
            int TelephoneColumn = StartColumn + columns.IndexOf("Telephone");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
            int WardIdColumn = StartColumn + columns.IndexOf("WardId");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int UnsignAddressColumn = StartColumn + columns.IndexOf("UnsignAddress");
            int DeliveryAddressColumn = StartColumn + columns.IndexOf("DeliveryAddress");
            int LatitudeColumn = StartColumn + columns.IndexOf("Latitude");
            int LongitudeColumn = StartColumn + columns.IndexOf("Longitude");
            int DeliveryLatitudeColumn = StartColumn + columns.IndexOf("DeliveryLatitude");
            int DeliveryLongitudeColumn = StartColumn + columns.IndexOf("DeliveryLongitude");
            int OwnerNameColumn = StartColumn + columns.IndexOf("OwnerName");
            int OwnerPhoneColumn = StartColumn + columns.IndexOf("OwnerPhone");
            int OwnerEmailColumn = StartColumn + columns.IndexOf("OwnerEmail");
            int TaxCodeColumn = StartColumn + columns.IndexOf("TaxCode");
            int LegalEntityColumn = StartColumn + columns.IndexOf("LegalEntity");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int AppUserIdColumn = StartColumn + columns.IndexOf("AppUserId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int StoreScoutingIdColumn = StartColumn + columns.IndexOf("StoreScoutingId");
            int StoreStatusIdColumn = StartColumn + columns.IndexOf("StoreStatusId");
            int IsStoreApprovalDirectSalesOrderColumn = StartColumn + columns.IndexOf("IsStoreApprovalDirectSalesOrder");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int DebtLimitedColumn = StartColumn + columns.IndexOf("DebtLimited");
            int DescriptionColumn = StartColumn + columns.IndexOf("Description");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreDAO StoreDAO = new StoreDAO();
                StoreDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreDAO.CodeDraft = ExcelWorksheet.Cells[row, CodeDraftColumn].Value?.ParseString();
                StoreDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreDAO.UnsignName = ExcelWorksheet.Cells[row, UnsignNameColumn].Value?.ParseString();
                StoreDAO.ParentStoreId = ExcelWorksheet.Cells[row, ParentStoreIdColumn].Value?.ParseNullLong();
                StoreDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.StoreTypeId = ExcelWorksheet.Cells[row, StoreTypeIdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.Telephone = ExcelWorksheet.Cells[row, TelephoneColumn].Value?.ParseString();
                StoreDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                StoreDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseNullLong();
                StoreDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                StoreDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                StoreDAO.UnsignAddress = ExcelWorksheet.Cells[row, UnsignAddressColumn].Value?.ParseString();
                StoreDAO.DeliveryAddress = ExcelWorksheet.Cells[row, DeliveryAddressColumn].Value?.ParseString();
                StoreDAO.Latitude = ExcelWorksheet.Cells[row, LatitudeColumn].Value?.ParseDecimal() ?? 0;
                StoreDAO.Longitude = ExcelWorksheet.Cells[row, LongitudeColumn].Value?.ParseDecimal() ?? 0;
                StoreDAO.DeliveryLatitude = ExcelWorksheet.Cells[row, DeliveryLatitudeColumn].Value?.ParseNullDecimal();
                StoreDAO.DeliveryLongitude = ExcelWorksheet.Cells[row, DeliveryLongitudeColumn].Value?.ParseNullDecimal();
                StoreDAO.OwnerName = ExcelWorksheet.Cells[row, OwnerNameColumn].Value?.ParseString();
                StoreDAO.OwnerPhone = ExcelWorksheet.Cells[row, OwnerPhoneColumn].Value?.ParseString();
                StoreDAO.OwnerEmail = ExcelWorksheet.Cells[row, OwnerEmailColumn].Value?.ParseString();
                StoreDAO.TaxCode = ExcelWorksheet.Cells[row, TaxCodeColumn].Value?.ParseString();
                StoreDAO.LegalEntity = ExcelWorksheet.Cells[row, LegalEntityColumn].Value?.ParseString();
                StoreDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.AppUserId = ExcelWorksheet.Cells[row, AppUserIdColumn].Value?.ParseNullLong();
                StoreDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.StoreScoutingId = ExcelWorksheet.Cells[row, StoreScoutingIdColumn].Value?.ParseNullLong();
                StoreDAO.StoreStatusId = ExcelWorksheet.Cells[row, StoreStatusIdColumn].Value?.ParseLong() ?? 0;
                StoreDAO.DebtLimited = ExcelWorksheet.Cells[row, DebtLimitedColumn].Value?.ParseDecimal() ?? 0;
                StoreDAO.Description = ExcelWorksheet.Cells[row, DescriptionColumn].Value?.ParseString();
                StoreDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreDAOs.Add(StoreDAO);
            }
            await DataContext.Store.BulkMergeAsync(StoreDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_Province(ExcelWorksheet ExcelWorksheet)
        {
            this.ProvinceDAOs = new List<ProvinceDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PriorityColumn = StartColumn + columns.IndexOf("Priority");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                ProvinceDAO ProvinceDAO = new ProvinceDAO();
                ProvinceDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                ProvinceDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                ProvinceDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                ProvinceDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseNullLong();
                ProvinceDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                ProvinceDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                ProvinceDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProvinceDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                ProvinceDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                ProvinceDAOs.Add(ProvinceDAO);
            }
            await DataContext.Province.BulkMergeAsync(ProvinceDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_Sex(ExcelWorksheet ExcelWorksheet)
        {
            this.SexDAOs = new List<SexDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                SexDAO SexDAO = new SexDAO();
                SexDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                SexDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                SexDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                SexDAOs.Add(SexDAO);
            }
            await DataContext.Sex.BulkMergeAsync(SexDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_Status(ExcelWorksheet ExcelWorksheet)
        {
            this.StatusDAOs = new List<StatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StatusDAO StatusDAO = new StatusDAO();
                StatusDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StatusDAOs.Add(StatusDAO);
            }
            await DataContext.Status.BulkMergeAsync(StatusDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_StoreScouting(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreScoutingDAOs = new List<StoreScoutingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int OwnerPhoneColumn = StartColumn + columns.IndexOf("OwnerPhone");
            int ProvinceIdColumn = StartColumn + columns.IndexOf("ProvinceId");
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
            int WardIdColumn = StartColumn + columns.IndexOf("WardId");
            int AddressColumn = StartColumn + columns.IndexOf("Address");
            int LatitudeColumn = StartColumn + columns.IndexOf("Latitude");
            int LongitudeColumn = StartColumn + columns.IndexOf("Longitude");
            int CreatorIdColumn = StartColumn + columns.IndexOf("CreatorId");
            int OrganizationIdColumn = StartColumn + columns.IndexOf("OrganizationId");
            int StoreScoutingStatusIdColumn = StartColumn + columns.IndexOf("StoreScoutingStatusId");
            int StoreScoutingTypeIdColumn = StartColumn + columns.IndexOf("StoreScoutingTypeId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreScoutingDAO StoreScoutingDAO = new StoreScoutingDAO();
                StoreScoutingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreScoutingDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreScoutingDAO.OwnerPhone = ExcelWorksheet.Cells[row, OwnerPhoneColumn].Value?.ParseString();
                StoreScoutingDAO.ProvinceId = ExcelWorksheet.Cells[row, ProvinceIdColumn].Value?.ParseNullLong();
                StoreScoutingDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseNullLong();
                StoreScoutingDAO.WardId = ExcelWorksheet.Cells[row, WardIdColumn].Value?.ParseNullLong();
                StoreScoutingDAO.Address = ExcelWorksheet.Cells[row, AddressColumn].Value?.ParseString();
                StoreScoutingDAO.Latitude = ExcelWorksheet.Cells[row, LatitudeColumn].Value?.ParseDecimal() ?? 0;
                StoreScoutingDAO.Longitude = ExcelWorksheet.Cells[row, LongitudeColumn].Value?.ParseDecimal() ?? 0;
                StoreScoutingDAO.CreatorId = ExcelWorksheet.Cells[row, CreatorIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingDAO.OrganizationId = ExcelWorksheet.Cells[row, OrganizationIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingDAO.StoreScoutingStatusId = ExcelWorksheet.Cells[row, StoreScoutingStatusIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingDAO.StoreScoutingTypeId = ExcelWorksheet.Cells[row, StoreScoutingTypeIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreScoutingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreScoutingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreScoutingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreScoutingDAOs.Add(StoreScoutingDAO);
            }
            var aaaa = StoreScoutingDAOs.Select(x => x.Id).ToList();
            await DataContext.StoreScouting.BulkMergeAsync(StoreScoutingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_StoreStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreStatusDAOs = new List<StoreStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreStatusDAO StoreStatusDAO = new StoreStatusDAO();
                StoreStatusDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreStatusDAOs.Add(StoreStatusDAO);
            }
            await DataContext.StoreStatus.BulkMergeAsync(StoreStatusDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_StoreType(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreTypeDAOs = new List<StoreTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ColorIdColumn = StartColumn + columns.IndexOf("ColorId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreTypeDAO StoreTypeDAO = new StoreTypeDAO();
                StoreTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreTypeDAO.ColorId = ExcelWorksheet.Cells[row, ColorIdColumn].Value?.ParseNullLong();
                StoreTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                StoreTypeDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreTypeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreTypeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreTypeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreTypeDAOs.Add(StoreTypeDAO);
            }
            await DataContext.StoreType.BulkMergeAsync(StoreTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_Ward(ExcelWorksheet ExcelWorksheet)
        {
            this.WardDAOs = new List<WardDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int PriorityColumn = StartColumn + columns.IndexOf("Priority");
            int DistrictIdColumn = StartColumn + columns.IndexOf("DistrictId");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                WardDAO WardDAO = new WardDAO();
                WardDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                WardDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                WardDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                WardDAO.Priority = ExcelWorksheet.Cells[row, PriorityColumn].Value?.ParseNullLong();
                WardDAO.DistrictId = ExcelWorksheet.Cells[row, DistrictIdColumn].Value?.ParseLong() ?? 0;
                WardDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                WardDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                WardDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                WardDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                WardDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                WardDAOs.Add(WardDAO);
            }
            await DataContext.Ward.BulkMergeAsync(WardDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_StoreGrouping(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreGroupingDAOs = new List<StoreGroupingDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int ParentIdColumn = StartColumn + columns.IndexOf("ParentId");
            int PathColumn = StartColumn + columns.IndexOf("Path");
            int LevelColumn = StartColumn + columns.IndexOf("Level");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreGroupingDAO StoreGroupingDAO = new StoreGroupingDAO();
                StoreGroupingDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreGroupingDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreGroupingDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreGroupingDAO.ParentId = ExcelWorksheet.Cells[row, ParentIdColumn].Value?.ParseNullLong();
                StoreGroupingDAO.Path = ExcelWorksheet.Cells[row, PathColumn].Value?.ParseString();
                StoreGroupingDAO.Level = ExcelWorksheet.Cells[row, LevelColumn].Value?.ParseLong() ?? 0;
                StoreGroupingDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                StoreGroupingDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreGroupingDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreGroupingDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreGroupingDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreGroupingDAOs.Add(StoreGroupingDAO);
            }
            await DataContext.StoreGrouping.BulkMergeAsync(StoreGroupingDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_StoreScoutingType(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreScoutingTypeDAOs = new List<StoreScoutingTypeDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreScoutingTypeDAO StoreScoutingTypeDAO = new StoreScoutingTypeDAO();
                StoreScoutingTypeDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingTypeDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreScoutingTypeDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreScoutingTypeDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingTypeDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreScoutingTypeDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreScoutingTypeDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreScoutingTypeDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreScoutingTypeDAOs.Add(StoreScoutingTypeDAO);
            }
            await DataContext.StoreScoutingType.BulkMergeAsync(StoreScoutingTypeDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_StoreScoutingStatus(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreScoutingStatusDAOs = new List<StoreScoutingStatusDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int CodeColumn = StartColumn + columns.IndexOf("Code");
            int NameColumn = StartColumn + columns.IndexOf("Name");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreScoutingStatusDAO StoreScoutingStatusDAO = new StoreScoutingStatusDAO();
                StoreScoutingStatusDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreScoutingStatusDAO.Code = ExcelWorksheet.Cells[row, CodeColumn].Value?.ParseString();
                StoreScoutingStatusDAO.Name = ExcelWorksheet.Cells[row, NameColumn].Value?.ParseString();
                StoreScoutingStatusDAOs.Add(StoreScoutingStatusDAO);
            }
            await DataContext.StoreScoutingStatus.BulkMergeAsync(StoreScoutingStatusDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        private async Task Given_StoreUser(ExcelWorksheet ExcelWorksheet)
        {
            this.StoreUserDAOs = new List<StoreUserDAO>();
            int StartColumn = 1;
            int StartRow = 1;
            List<string> columns = new List<string>();
            for (int column = StartColumn; column <= ExcelWorksheet.Dimension.End.Column; column++)
            {
                string columnName = ExcelWorksheet.Cells[StartRow, column].Value?.ToString() ?? "";
                columns.Add(columnName);
            }
            int IdColumn = StartColumn + columns.IndexOf("Id");
            int StoreIdColumn = StartColumn + columns.IndexOf("StoreId");
            int UsernameColumn = StartColumn + columns.IndexOf("Username");
            int DisplayNameColumn = StartColumn + columns.IndexOf("DisplayName");
            int PasswordColumn = StartColumn + columns.IndexOf("Password");
            int OtpCodeColumn = StartColumn + columns.IndexOf("OtpCode");
            int OtpExpiredColumn = StartColumn + columns.IndexOf("OtpExpired");
            int StatusIdColumn = StartColumn + columns.IndexOf("StatusId");
            int CreatedAtColumn = StartColumn + columns.IndexOf("CreatedAt");
            int UpdatedAtColumn = StartColumn + columns.IndexOf("UpdatedAt");
            int DeletedAtColumn = StartColumn + columns.IndexOf("DeletedAt");
            int RowIdColumn = StartColumn + columns.IndexOf("RowId");
            int UsedColumn = StartColumn + columns.IndexOf("Used");
            for (int row = StartRow + 1; row <= ExcelWorksheet.Dimension.End.Row; row++)
            {
                if (ExcelWorksheet.Cells[row, StartColumn].Value == null)
                    continue;
                if (ExcelWorksheet.Cells[row, StartColumn].Value.ToString() == "END")
                    break;
                StoreUserDAO StoreUserDAO = new StoreUserDAO();
                StoreUserDAO.Id = ExcelWorksheet.Cells[row, IdColumn].Value?.ParseLong() ?? 0;
                StoreUserDAO.StoreId = ExcelWorksheet.Cells[row, StoreIdColumn].Value?.ParseLong() ?? 0;
                StoreUserDAO.Username = ExcelWorksheet.Cells[row, UsernameColumn].Value?.ParseString();
                StoreUserDAO.DisplayName = ExcelWorksheet.Cells[row, DisplayNameColumn].Value?.ParseString();
                StoreUserDAO.Password = ExcelWorksheet.Cells[row, PasswordColumn].Value?.ParseString();
                StoreUserDAO.OtpCode = ExcelWorksheet.Cells[row, OtpCodeColumn].Value?.ParseString();
                StoreUserDAO.OtpExpired = ExcelWorksheet.Cells[row, OtpExpiredColumn].Value?.ParseNullDateTime();
                StoreUserDAO.StatusId = ExcelWorksheet.Cells[row, StatusIdColumn].Value?.ParseLong() ?? 0;
                StoreUserDAO.CreatedAt = ExcelWorksheet.Cells[row, CreatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreUserDAO.UpdatedAt = ExcelWorksheet.Cells[row, UpdatedAtColumn].Value?.ParseDateTime() ?? DateTime.Now;
                StoreUserDAO.DeletedAt = ExcelWorksheet.Cells[row, DeletedAtColumn].Value?.ParseNullDateTime();
                StoreUserDAO.RowId = ExcelWorksheet.Cells[row, RowIdColumn].Value?.ParseGuid() ?? Guid.Empty;
                StoreUserDAO.Used = ExcelWorksheet.Cells[row, UsedColumn].Value?.ParseBool() ?? false;
                StoreUserDAOs.Add(StoreUserDAO);
            }
            await DataContext.StoreUser.BulkMergeAsync(StoreUserDAOs, options =>
            {
                options.MergeKeepIdentity = true;
                options.ColumnPrimaryKeyExpression = c => c.Id;
            });
        }
        #endregion
        protected async Task<ICurrentContext> GetCurrentContext(long StoreUserId)
        {
            DataContext = ServiceProvider.GetService<DataContext>();
            ICurrentContext CurrentContext = ServiceProvider.GetService<ICurrentContext>();
            CurrentContext.StoreUserId = StoreUserId;
            CurrentContext.TimeZone = 7;
            CurrentContext.Language = "vi";
            return CurrentContext;
        }
    }
    public class MockRabbitManager : IRabbitManager
    {
        public void PublishList<T>(List<T> message, string routeKey) where T : DataEntity
        {
        }
        public void PublishSingle<T>(T message, string routeKey) where T : DataEntity
        {
        }
    }
    public class MockLogging : ILogging
    {
        public void CreateAuditLog(object newData, object oldData, string className, [CallerMemberName] string methodName = "")
        {
        }
        public void CreateSystemLog(Exception ex, string className, [CallerMemberName] string methodName = "")
        {
        }
    }
    public class MockImageService : IImageService
    {
        public MockImageService()
        {
        }
        public Task<int> Count(ImageFilter ImageFilter)
        {
            throw new NotImplementedException();
        }
        public async Task<Image> Create(Image Image, string path, string thumbnailPath, int width, int height)
        {
            return null;
        }
        public async Task<Image> Create(Image Image, string path)
        {
            return null;
        }
        public async Task<Image> Create(Image Image)
        {
            return null;
        }
        public async Task<Image> Delete(Image Image)
        {
            return null;
        }
        public async Task<Image> Get(long Id)
        {
            return null;
        }
        public async Task<List<Image>> Import(List<Image> Images)
        {
            return null;
        }
        public async Task<List<Image>> List(ImageFilter ImageFilter)
        {
            return null;
        }
        public async Task<ImageFilter> ToFilter(ImageFilter ImageFilter)
        {
            return null;
        }
        public async Task<Image> Update(Image Image)
        {
            return null;
        }
        public async Task<List<Image>> BulkDelete(List<Image> Images)
        {
            return null;
        }
    }
}
