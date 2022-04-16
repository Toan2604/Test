using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Helpers;
using DMS.Models;
using DMS.Rpc;
using DMS.Services.MImage;
using DMS.Handlers.Configuration;
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
using Newtonsoft.Json;
using System.Data.SqlClient;
using Thinktecture;

namespace DMS.Tests
{
    public partial class BaseTests : FeatureFixture
    {
        public const string DATA_DMS_EXCEL_PATH = "/files/DMS/20211012/Database.xlsx";
        public const string DW_DMS_EXCEL_PATH = "/files/DMS/20211012/Datawarehouse.xlsx";
        public ServiceProvider ServiceProvider;
        protected DataContext DataContext;
        protected DWContext DWContext;
        public void Init(string DatabaseName = "DMS", string DWName = "DW_DMS")
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

                string DataContextConnectionString = $@"data source=localhost;initial catalog={DatabaseName};persist security info=True;Trusted_Connection=True;multipleactiveresultsets=True;";
                string DWContextConnectionString = $@"data source=localhost;initial catalog={DWName};persist security info=True;Trusted_Connection=True;multipleactiveresultsets=True;";
                
                ServiceDescriptor descriptor = serviceCollection.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<DataContext>));
                if (descriptor != null)
                {
                    serviceCollection.Remove(descriptor);
                }
                descriptor = serviceCollection.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<DWContext>));
                if (descriptor != null)
                {
                    serviceCollection.Remove(descriptor);
                }

                serviceCollection.AddDbContext<DataContext>(options =>
                {
                    options.UseSqlServer(DataContextConnectionString, sqlOptions =>
                    {
                        sqlOptions.AddTempTableSupport();
                    });
                    options.AddInterceptors(new HintCommandInterceptor());
                });
                serviceCollection.AddDbContext<DWContext>(options =>
                {
                    options.UseSqlServer(DWContextConnectionString, sqlOptions =>
                    {
                        sqlOptions.AddTempTableSupport();
                    });
                    options.AddInterceptors(new HintCommandInterceptor());
                });

                serviceCollection.Replace<DataContext, DataContext>(ServiceLifetime.Scoped);
                serviceCollection.Replace<IRabbitManager, MockRabbitManager>(ServiceLifetime.Scoped);
                serviceCollection.Replace<ILogging, MockLogging>(ServiceLifetime.Scoped);
                serviceCollection.Replace<IImageService, MockImageService>(ServiceLifetime.Scoped);
                ServiceProvider = serviceCollection.BuildServiceProvider();
                DWContext = ServiceProvider.GetService<DWContext>();
                DataContext = ServiceProvider.GetService<DataContext>();
                //Clean();
                InitData();
            }
            catch (Exception ex)
            {
            }
        }
        [TearDown]
        public void Clean()
        {
            DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'");
            DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'IF OBJECTPROPERTY(object_id(''?''), ''TableHasIdentity'') = 1 DBCC CHECKIDENT(''?'', RESEED, 0)'");
            DataContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");
            DWContext DWContext = ServiceProvider.GetService<DWContext>();
            DWContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL'");
            DWContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'SET QUOTED_IDENTIFIER ON; DELETE FROM ?'");
            DWContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'IF OBJECTPROPERTY(object_id(''?''), ''TableHasIdentity'') = 1 DBCC CHECKIDENT(''?'', RESEED, 0)'");
            DWContext.Database.ExecuteSqlRaw("EXEC sp_MSForEachTable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL'");
        }
        private void InitData()
        {
            var CurrentContext = ServiceProvider.GetService<ICurrentContext>();
            CurrentContext.Language = "vi";
            CurrentContext.TimeZone = 7;
            CurrentContext.UserId = 2;
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
        protected T ReadFileFromJson<T>(string path)
        {
            MemoryStream MemoryStream = ReadFile(path);
            StreamReader reader = new StreamReader(MemoryStream);
            string Payload = reader.ReadToEnd();
            return JsonConvert.DeserializeObject<T>(Payload);
        }
        protected async Task LoadPermission(string path)
        {
            this.DataContext = ServiceProvider.GetService<DataContext>();
            System.IO.MemoryStream MemoryStream = ReadFile(path);
            using (ExcelPackage excelPackage = new ExcelPackage(MemoryStream))
            {
                ExcelWorksheet wsStatus = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Status)).FirstOrDefault();
                if (wsStatus != null)
                    await Given_Status(wsStatus);
                ExcelWorksheet wsSex = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Sex)).FirstOrDefault();
                if (wsSex != null)
                    await Given_Sex(wsSex);
                ExcelWorksheet wsOrganization = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Organization)).FirstOrDefault();
                if (wsOrganization != null)
                    await Given_Organization(wsOrganization);
                ExcelWorksheet wsProvince = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Province)).FirstOrDefault();
                if (wsProvince != null)
                    await Given_Province(wsProvince);
                ExcelWorksheet wsDistrict = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(District)).FirstOrDefault();
                if (wsDistrict != null)
                    await Given_District(wsDistrict);
                ExcelWorksheet wsAppUser = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(AppUser)).FirstOrDefault();
                if (wsAppUser != null)
                    await Given_AppUser(wsAppUser);
                ExcelWorksheet wsFieldType = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(FieldType)).FirstOrDefault();
                if (wsFieldType != null)
                    await Given_FieldType(wsFieldType);
                ExcelWorksheet wsRole = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Role)).FirstOrDefault();
                if (wsRole != null)
                    await Given_Role(wsRole);
                ExcelWorksheet wsMenu = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Menu)).FirstOrDefault();
                if (wsMenu != null)
                    await Given_Menu(wsMenu);
                ExcelWorksheet wsField = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Field)).FirstOrDefault();
                if (wsField != null)
                    await Given_Field(wsField);
                ExcelWorksheet wsAction = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Entities.Action)).FirstOrDefault();
                if (wsAction != null)
                    await Given_Action(wsAction);
                ExcelWorksheet wsPage = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Page)).FirstOrDefault();
                if (wsPage != null)
                    await Given_Page(wsPage);
                ExcelWorksheet wsPermission = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(Permission)).FirstOrDefault();
                if (wsPermission != null)
                    await Given_Permission(wsPermission);
                ExcelWorksheet wsPermissonOperator = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(PermissonOperator)).FirstOrDefault();
                if (wsPermissonOperator != null)
                    await Given_PermissionOperator(wsPermissonOperator);
                ExcelWorksheet wsAppUserRoleMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(AppUserRoleMapping)).FirstOrDefault();
                if (wsAppUserRoleMapping != null)
                    await Given_AppUserRoleMapping(wsAppUserRoleMapping);
                ExcelWorksheet wsPermissonContent = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(PermissonContent)).FirstOrDefault();
                if (wsPermissonContent != null)
                    await Given_PermissionContent(wsPermissonContent);
                ExcelWorksheet wsActionPageMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(ActionPageMapping)).FirstOrDefault();
                if (wsActionPageMapping != null)
                    await Given_ActionPageMapping(wsActionPageMapping);
                ExcelWorksheet wsPermissionActionMapping = excelPackage.Workbook.Worksheets.Where(x => x.Name == nameof(PermissionActionMapping)).FirstOrDefault();
                if (wsPermissionActionMapping != null)
                    await Given_PermissionActionMapping(wsPermissionActionMapping);
            }
        }

        protected async Task<ICurrentContext> GetCurrentContext(long UserId, string Path)
        {
            DataContext = ServiceProvider.GetService<DataContext>();
            ICurrentContext CurrentContext = ServiceProvider.GetService<ICurrentContext>();
            CurrentContext.UserId = UserId;
            CurrentContext.UserName = UserId.ToString();
            CurrentContext.TimeZone = 7;
            CurrentContext.Language = "vi";
            //StaticParams.DateTimeNow = new DateTime(2021, 07, 28, 16, 00, 00); //set date để test các trường hợp filter time
            List<long> permissionIds = await (from aurm in DataContext.AppUserRoleMapping.Where(x => x.AppUserId == UserId)
                                              join r in DataContext.Role on aurm.RoleId equals r.Id
                                              join per in DataContext.Permission on aurm.RoleId equals per.RoleId
                                              join pam in DataContext.PermissionActionMapping on per.Id equals pam.PermissionId
                                              join apm in DataContext.ActionPageMapping on pam.ActionId equals apm.ActionId
                                              join page in DataContext.Page on apm.PageId equals page.Id
                                              where r.StatusId == StatusEnum.ACTIVE.Id && per.StatusId == StatusEnum.ACTIVE.Id &&
                                              page.Path == Path
                                              select per.Id
                                             ).Distinct().ToListAsync();
            IdFilter IdFilter = new IdFilter { In = permissionIds };
            List<PermissionDAO> PermissionDAOs = await DataContext.Permission.AsNoTracking()
               .Include(p => p.PermissionContents).ThenInclude(pf => pf.Field)
               .Where(p => p.Id, IdFilter)
               .ToListAsync();
            CurrentContext.RoleIds = PermissionDAOs.Select(p => p.RoleId).Distinct().ToList();
            CurrentContext.Filters = new Dictionary<long, List<FilterPermissionDefinition>>();
            foreach (PermissionDAO PermissionDAO in PermissionDAOs)
            {
                List<FilterPermissionDefinition> FilterPermissionDefinitions = new List<FilterPermissionDefinition>();
                CurrentContext.Filters.Add(PermissionDAO.Id, FilterPermissionDefinitions);
                foreach (PermissionContentDAO PermissionContentDAO in PermissionDAO.PermissionContents)
                {
                    FilterPermissionDefinition FilterPermissionDefinition = FilterPermissionDefinitions.Where(f => f.Name == PermissionContentDAO.Field.Name).FirstOrDefault();
                    if (FilterPermissionDefinition == null)
                    {
                        FilterPermissionDefinition = new FilterPermissionDefinition(PermissionContentDAO.Field.Name);
                        FilterPermissionDefinitions.Add(FilterPermissionDefinition);
                    }
                    FilterPermissionDefinition.SetValue(PermissionContentDAO.Field.FieldTypeId, PermissionContentDAO.PermissionOperatorId, PermissionContentDAO.Value);
                }
            }
            return CurrentContext;
        }

        protected void InitDatabase(string DatabaseName, string ScriptPath)
        {
            string SnapshotName = $"{DatabaseName}_Snapshot";
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = "data source=localhost;initial catalog=master;persist security info=True;Trusted_Connection=True;multipleactiveresultsets=True;";
                conn.Open();

                // get snapshot
                SqlCommand cmd = new SqlCommand($@"Use master;SELECT* FROM sys.databases WHERE NAME= '{SnapshotName}'", conn);
                var result = cmd.ExecuteReader().Read();

                if (result) // if exists Snapshot, delete other snapshots then restore database from snapshot of that database
                {
                    cmd = new SqlCommand($@"Use master; IF EXISTS(SELECT* FROM sys.databases WHERE NAME= '{SnapshotName}') 
                    RESTORE DATABASE {DatabaseName} from DATABASE_SNAPSHOT = '{SnapshotName}'", conn);
                    cmd.ExecuteNonQuery();

                }
                else // if snapshot has been existed, create new database and structure by script
                {
                    cmd = new SqlCommand($@"Use master; IF EXISTS (SELECT * FROM sys.databases WHERE name = '{DatabaseName}') DROP DATABASE {DatabaseName};", conn);
                    cmd.ExecuteNonQuery();
                    MemoryStream MemoryStream = ReadFile(ScriptPath);
                    StreamReader reader = new StreamReader(MemoryStream);
                    string InitDMSQuery = reader.ReadToEnd();
                    if(DatabaseName.StartsWith("DMS")) InitDMSQuery = InitDMSQuery.Replace("DMS", DatabaseName);
                    if(DatabaseName.StartsWith("DW")) InitDMSQuery = InitDMSQuery.Replace("DW_DMS", DatabaseName);
                    var SubInitDMSQuery = InitDMSQuery.Split("\r\nGO\r\n").ToList();
                    // Vì các câu lệnh trước phải được thực hiện rồi thì mới thực hiện câu lệnh sau được nên phải tách từng phần để execute
                    for (int i = 0; i < SubInitDMSQuery.Count - 1; i++)
                    {
                        cmd = new SqlCommand(SubInitDMSQuery[i], conn);
                        cmd.ExecuteNonQuery();
                    }
                }

                conn.Close();
            }
        }


    }

}
