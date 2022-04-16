using DMS.Rpc.store;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DMS.Models;
using LightBDD.NUnit3;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using Microsoft.AspNetCore.Http;
using DMS.Common;
namespace DMS.Tests.Rpc.store
{
    [TestFixture]
    [FeatureDescription(@"")] 
    [Label("Story-1")]
    public partial class StoreControllerFeature : BaseTests
    {
        StoreController StoreController;
        Store_StoreDTO Store_StoreDTO;
        IFormFile FormFile;
        DataContext DataContext;
        [SetUp]
        public async Task Setup()
        {
            Init();
            DataContext = ServiceProvider.GetService<DataContext>();
            StoreController = ServiceProvider.GetService<StoreController>();
            ICurrentContext CurrentContext = ServiceProvider.GetService<ICurrentContext>();
            CurrentContext.UserId = 2;
        }
        [Scenario]
        [Label("Tạo mới sản phẩm không có rule")]
        public async Task LoadExcel_Create_Success() //scenario name
        {
            string path = "Controllers/store/Store.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadExcel(path),
                _ => When_UserInput(),
                _ => When_UserCreate(),
                _ => Then_Success()
                );
        }
        [Scenario]
        [Label("Tạo mới sản phẩm không có rule")]
        public async Task LoadExcel_Update_Success() //scenario name
        {
            string path = "Controllers/store/Store.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadExcel(path),
                _ => When_UserInput(),
                _ => When_UserUpdate(),
                _ => Then_Success()
                );
        }
        [Scenario]
        [Label("Tạo mới sản phẩm không có rule")]
        public async Task LoadExcel_Delete_Success() //scenario name
        {
            string path = "Controllers/store/Store.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadExcel(path),
                _ => When_UserInput(),
                _ => When_UserDelete(),
                _ => Then_Success()
                );
        }
        [Scenario]
        [Label("")]
        public async Task LoadExcel_Import_Success() //scenario name
        {
            string import_excel = "Rpc/store/ImportStore_20210726.xlsx";
            string path = "Rpc/store/Store.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadExcel(path),
                _ => When_UserImportExcel(import_excel),
                _ => When_UserImport(),
                _ => Then_ImportSuccess(1474)
                );
        }
        [Scenario]
        [Label("")]
        public async Task LoadExcel_ImportDev_Success() //scenario name
        {
            string import_excel = "Rpc/store/ImportStore_Dev.xlsx";
            string path = "Rpc/store/Store_Dev.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadExcel(path),
                _ => When_UserImportExcel(import_excel),
                _ => When_UserImport(),
                _ => Then_ImportSuccess(1)
                );
        }
    }
}
