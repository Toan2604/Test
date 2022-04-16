using DMS.Rpc.store_balance;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DMS.Models;
using LightBDD.NUnit3;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using System.Collections.Generic;

namespace DMS.Tests.Rpc.store_balance
{
    [TestFixture]
    [FeatureDescription(@"")] 
    [Label("Story-1")]
    public partial class StoreBalanceControllerFeature : BaseTests
    {
        StoreBalanceController StoreBalanceController;
        StoreBalance_StoreBalanceDTO StoreBalance_StoreBalanceDTO;
        List<StoreBalance_StoreBalanceDTO> StoreBalance_StoreBalanceDTOs;
        StoreBalance_StoreBalanceFilterDTO StoreBalance_StoreBalanceFilterDTO;
        List<long> StoreBalanceIds;
        DataContext DataContext;
        [SetUp]
        public async Task Setup()
        {
            Init();
            DataContext = ServiceProvider.GetService<DataContext>();
            StoreBalanceController = ServiceProvider.GetService<StoreBalanceController>();
        }

        [Scenario]
        [Label("Tạo mới thành công công nợ")]
        public async Task Create_Success() 
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_CreateSuccess(),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                );
        }
        [Scenario]
        [Label("Tạo mới công nợ bị trùng, công nợ bị âm")]
        public async Task Create_Failed()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_CreateFailed(),
                _ => When_UserCreate(),
                _ => Then_Create_Failed()
                );
        }
        [Scenario]
        [Label("Tạo mới công nợ thiếu các trường bắt buộc")]
        public async Task Create_MissingRequiredField_Failed()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_MissingRequiredField_CreateFailed(),
                _ => When_UserCreate(),
                _ => Then_Create_MissingRequiredField_Failed()
                );
        }

        [Scenario]
        [Label("Sửa thành công công nợ")]
        public async Task Update_Success() 
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_UpdateSuccess(),
                _ => When_UserUpdate(),
                _ => Then_Update_Success()
                );
        }
        [Scenario]
        [Label("Sửa các tiêu chí không được sửa của công nợ")]
        public async Task Update_Failed()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_UpdateFailed(),
                _ => When_UserUpdate(),
                _ => Then_Update_Failed()
                );
        }
        [Scenario]
        [Label("Sửa dư nợ, dư có về số âm")]
        public async Task Update_NegativeAmount_Failed()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_NegativeAmount_UpdateFailed(),
                _ => When_UserUpdate(),
                _ => Then_Update_NegativeAmount_Failed()
                );
        }
        [Scenario]
        [Label("Sửa dư nợ, dư có về 0")]
        public async Task Update_AmountEqual0_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_AmountEqual0_UpdateSuccess(),
                _ => When_UserUpdate(),
                _ => Then_Update_Success()
                );
        }
        [Scenario]
        [Label("Để trống dư nợ, dư có")]
        public async Task Update_BlankAmount_Failed()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_BlankAmount_UpdateFailed(),
                _ => When_UserUpdate(),
                _ => Then_Update_Success()
                );
        }
        [Scenario]
        [Label("Tự động làm tròn dư nợ, dư có")]
        public async Task Update_RoundAmount_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_RoundAmount_UpdateSuccess(),
                _ => When_UserUpdate(),
                _ => Then_Update_RoundAmount_Success()
                );
        }
        [Scenario]
        [Label("Xóa thành công công nợ ")]
        public async Task Delete_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_DeleteSuccess(),
                _ => When_UserDelete(),
                _ => Then_Delete_Success()
                );
        }

        [Scenario]
        [Label("Xóa nhiều công nợ")]
        public async Task BulkDelete_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_BulkDeleteSuccess(),
                _ => When_UserBulkDelete(),
                _ => Then_BulkDelete_Success()
                );
        }
        [Scenario]
        [Label("Tìm kiếm công nợ theo tên đại lý")]
        public async Task SearchByStoreName_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_FilterByStoreName(),
                _ => When_UserSearch(),
                _ => Then_SearchByStoreName_Success()
                );
        }
        [Scenario]
        [Label("Tìm kiếm công nợ theo mã đại lý")]
        public async Task SearchByStoreCode_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_FilterByStoreCode(),
                _ => When_UserSearch(),
                _ => Then_SearchByStoreCode_Success()
                );
        }
        [Scenario]
        [Label("Tìm kiếm công nợ theo đơn vị tổ chức")]
        public async Task SearchByOrganization_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_FilterByOrganization(),
                _ => When_UserSearch(),
                _ => Then_SearchByOrganization_Success()
                );
        }
        [Scenario]
        [Label("Tìm kiếm công nợ theo nhiều trường: tên, mã, mã tự nhập đại lý")]
        public async Task SearchMultiFilter_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_MultiFilter(),
                _ => When_UserSearch(),
                _ => Then_SearchMultiFilter_Success()
                );
        }
        [Scenario]
        [Label("Xuất excel danh sách công nợ")]
        public async Task ExportExcel_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_Filter(),
                _ => When_UserExport(),
                _ => Then_Update_Success()
                );
        }
        [Scenario]
        [Label("Xuất excel danh sách công nợ theo điều kiện lọc: tên, mã đại lý")]
        public async Task ExportExcel_WithMultiFilter_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput_MultiFilter(),
                _ => When_UserExport(),
                _ => Then_Update_Success()
                );
        }
        [Scenario]
        [Label("Tải mẫu excel thành công")]
        public async Task ExportExcelTemplate_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserExportTemplate(),
                _ => Then_Update_Success()
                );
        }
        [Scenario]
        [Label("Import excel thành công")]
        public async Task ImportExcelTemplate_Success()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                //_ => When_UserInput_Filter(),
                _ => When_UserUpdate(),
                _ => Then_Update_Success()
                );
        }
        [Scenario]
        [Label("Import excel thất bại")]
        public async Task ImportExcelTemplate_Failed()
        {
            string path = "/files/DMS/20210906/Database.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                //_ => When_UserInput_Filter(),
                _ => When_UserUpdate(),
                _ => Then_Update_Success()
                );
        }
    }
}
