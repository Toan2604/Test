using DMS.Rpc.lucky_draw;
using NUnit.Framework;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using DMS.Models;
using LightBDD.NUnit3;
using LightBDD.Framework;
using LightBDD.Framework.Scenarios;
using System.Collections.Generic;
using DMS.Rpc.mobile.general_mobile;

namespace DMS.Tests.Rpc.general_mobile
{
    [TestFixture]
    [FeatureDescription(@"")] 
    [Label("Story-1")]
    public partial class LuckyDrawControllerFeature : BaseTests
    {
        private const string DATA_DMS_PATH = "/files/DMS/bachlx/DMS_1121.xlsx";
        private const string DW_DMS_PATH = "/files/DMS/bachlx/DW_DMS_1121.xlsx";
        public const string INIT_DMS_PATH = "/files/DMS/DMS_Script_1121.txt";
        public const string INIT_DWDMS_PATH = "/files/DMS/DW_DMS_Script_1121.txt";

        MobileController MobileController;
        GeneralMobile_LuckyDrawDTO GeneralMobile_LuckyDrawDTO;
        List<GeneralMobile_LuckyDrawDTO> GeneralMobile_LuckyDrawDTOs;
        GeneralMobile_LuckyDrawFilterDTO GeneralMobile_LuckyDrawFilterDTO;

        [SetUp]
        public async Task Setup()
        {
            string DatabaseName = DATA_DMS_PATH.Split('/')[DATA_DMS_PATH.Split('/').Length - 1].Split('.')[0];
            string DWName = DW_DMS_PATH.Split('/')[DATA_DMS_PATH.Split('/').Length - 1].Split('.')[0];

            InitDatabase(DatabaseName, INIT_DMS_PATH);
            InitDatabase(DWName, INIT_DWDMS_PATH);

            Init(DatabaseName, DWName);

            await LoadExcel(DATA_DMS_PATH, DatabaseName);
            await LoadDWExcel(DW_DMS_PATH, DWName);

            DataContext = ServiceProvider.GetService<DataContext>();
            MobileController = ServiceProvider.GetService<MobileController>();
        }
        [Scenario]
        [Label("Danh sách chương trình hiện có không filter")]
        public async Task ListLuckyDraw_NoFilter() 
        {
            await Runner.RunScenarioAsync(
                _ => When_UserInputNoFilter(),
                _ => Get_ListLuckyDraw(),
                _ => Then_SearchNoFilter_Success()
                );
        }
        [Scenario]
        [Label("Danh sách đại lý")]
        public async Task ListLuckyDrawStire_NoFillter()
        {
            await Runner.RunScenarioAsync(
                _ => When_UserInputNoFilter(),
                _ => Get_ListLuckyDraw(),
                _ => Then_SearchNoFilter_Success()
                );
        }
        [Scenario]
        [Label("Tìm kiếm đại lý theo tên")]
        public async Task ListLuckyDrawStire_FillterByName()
        {
            await Runner.RunScenarioAsync(
                _ => When_UserInputNoFilter(),
                _ => Get_ListLuckyDraw(),
                _ => Then_SearchNoFilter_Success()
                );
        }
        [Scenario]
        [Label("Danh sách chương trình đã hết hạn: inactive / hết giải / hết hạn")]
        public async Task ListHistory_NoFilter()
        {
            string payload_path = "rpc/lucky-draw/payload/create.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload_path)
                //_ => When_UserCreate(),
                //_ => Then_Create_Success()
                );
        }
        [Scenario]
        [Label("Quay thưởng với chương trình hợp lệ")]
        public async Task Draw_Success()
        {
            string payload_path = "rpc/lucky-draw/payload/create.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload_path)
                //_ => When_UserCreate(),
                //_ => Then_Create_Success()
                );
        }
        [Scenario]
        [Label("Quay thưởng với chương trình không thuộc phạm vi")]
        public async Task Draw_NotInScoped()
        {
            string payload_path = "rpc/lucky-draw/payload/create.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload_path)
                //_ => When_UserCreate(),
                //_ => Then_Create_Success()
                );
        }
        [Scenario]
        [Label("Quay thưởng với chương trình hết giải")]
        public async Task Draw_PrizeOver()
        {
            string payload_path = "rpc/lucky-draw/payload/create.json";
            await Runner.RunScenarioAsync(
                _ => When_UserInput(payload_path)
                //_ => When_UserCreate(),
                //_ => Then_Create_Success()
                );
        }
    }
}
