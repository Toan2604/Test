using DMS.Rpc.lucky_draw;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DMS.Models;
using LightBDD.NUnit3;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using System.Collections.Generic;

namespace DMS.Tests.Rpc.lucky_draw
{
    [TestFixture]
    [FeatureDescription(@"")] 
    [Label("Story-1")]
    public partial class LuckyDrawControllerFeature : BaseTests
    {
        LuckyDrawController LuckyDrawController;
        LuckyDraw_LuckyDrawDTO LuckyDraw_LuckyDrawDTO;
        List<LuckyDraw_LuckyDrawDTO> LuckyDraw_LuckyDrawDTOs;
        LuckyDraw_LuckyDrawFilterDTO LuckyDraw_LuckyDrawFilterDTO;
        DataContext DataContext;
        [SetUp]
        public async Task Setup()
        {
            Init();
            DataContext = ServiceProvider.GetService<DataContext>();
            LuckyDrawController = ServiceProvider.GetService<LuckyDrawController>();
        }
        [Scenario]
        [Label("Tạo mới chương trình quay thưởng")]
        public async Task LoadExcel_Create_Success() 
        {
            string payload_path = "rpc/lucky-draw/payload/create.json";
            string path = "/files/DMS/20210830/Database20210830.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload_path),
                _ => When_UserCreate(),
                _ => Then_Create_Success()
                );
        }

        [Scenario]
        [Label("Update chương trình quay thưởng")]
        public async Task LoadExcel_Update_Success() 
        {
            string payload_path = "rpc/lucky-draw/payload/update.json";
            string path = "/files/DMS/20210830/Database20210830.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload_path),
                _ => When_UserUpdate(),
                _ => Then_Update_Success()
                );
        }

        [Scenario]
        [Label("Xóa chương trình quay thưởng")]
        public async Task LoadExcel_Delete_Success() 
        {
            string payload_path = "rpc/lucky-draw/payload/update.json";
            string path = "/files/DMS/20210830/Database20210830.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload_path),
                _ => When_UserDelete(),
                _ => Then_Delete_Success()
                );
        }

        [Scenario]
        [Label("Tìm kiếm chương trình quay thưởng theo tên")]
        public async Task SearchLuckyDraw_ByName()
        {
            string path = "/files/DMS/20210830/Database20210830.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInputFilter(),
                _ => When_UserSearch(),
                _ => Then_Search_Success()
                );
        }
        [Scenario]
        [Label("Tìm kiếm chương trình quay thưởng với nhiều điều kiện")]
        public async Task SearchLuckyDraw_WithManyCondition_Success()
        {
            string path = "/files/DMS/20210830/Database20210830.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInputManyFilter(),
                _ => When_UserSearch(),
                _ => Then_SearchManyFilter_Success()
                );
        }
        [Scenario]
        [Label("Validate khi update chương trình đã quay thưởng các trường không phải là: tên, ngày kết thúc, trạng thái, số lượng giải")]
        public async Task UpdateUsedLuckyDraw_Failed()
        {
            string payload_path = "rpc/lucky-draw/payload/update_used.json";
            string path = "/files/DMS/20210830/Database20210830.xlsx";
            await Runner.RunScenarioAsync(
                _ => LoadPermission(path),
                _ => LoadExcel(path),
                _ => When_UserInput(payload_path),
                _ => When_UserUpdate(),
                _ => Then_Update_Failed()
                );
        }
    }
}
