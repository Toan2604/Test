using System.Collections.Generic;

namespace DMS.Rpc.mobile.general_mobile
{
    public class GeneralMobileRoute : Root
    {
        public const string Master = Module + "/general-mobile/store-checking-master";
        public const string Detail = Module + "/general-mobile/store-checking-detail";
        private const string Default = Rpc + Module + "/general-mobile";
        public const string CountStoreChecking = Default + "/count-store-checking";
        public const string ListStoreChecking = Default + "/list-store-checking";
        public const string GetStoreChecking = Default + "/get-store-checking";
        public const string UpdateStoreChecking = Default + "/update-store-checking";
        public const string UpdateStoreCheckingImage = Default + "/update-store-checking-image";
        public const string CheckIn = Default + "/check-in";
        public const string CheckOut = Default + "/check-out";
        public const string GetConfiguration = Default + "/get-configuration";


        // mobile route custom
        public const string GetItemByVariation = Default + "/get-item-by-variation";
        public const string ListErpApprovalState = Default + "/list-erp-approval-state";

        public const string CreateProblem = Default + "/create-problem";
        public const string SaveImage = Default + "/save-image";
        public const string SaveImage64 = Default + "/save-image-64";
        public const string UpdateAlbum = Default + "/update-album";
        public const string GetNotification = Default + "/get-notification";
        public const string UpdateGPS = Default + "/update-gps";
        public const string PrintIndirectOrder = Default + "/print-indirect-order";
        public const string PrintDirectOrder = Default + "/print-direct-order";

        public const string StoreReport = Default + "/store-report";
        public const string StoreStatistic = Default + "/store-statistic";

        public const string SingleListAlbum = Default + "/single-list-album";
        public const string SingleListAppUser = Default + "/single-list-app-user";
        public const string SingleListEroute = Default + "/single-list-e-route";
        public const string SingleListStore = Default + "/single-list-store";
        public const string SingleListStoreGrouping = Default + "/single-list-store-grouping";
        public const string SingleListStoreStatus = Default + "/single-list-store-status";
        public const string SingleListStoreType = Default + "/single-list-store-type";
        public const string SingleListTaxType = Default + "/single-list-tax-type";
        public const string SingleListUnitOfMeasure = Default + "/single-list-unit-of-measure";
        public const string SingleListProblemType = Default + "/single-list-problem-type";
        public const string SingleListStoreScoutingType = Default + "/single-list-store-scouting-type";
        public const string SingleListProvince = Default + "/single-list-province";
        public const string SingleListDistrict = Default + "/single-list-district";
        public const string SingleListWard = Default + "/single-list-ward";
        public const string SingleListTime = Default + "/single-list-time";
        public const string SingleListSalesOrderType = Default + "/single-list-sales-order-type";
        public const string SingleListEstimatedRevenue = Default + "/single-list-estimated-revenue";

        public const string SingleListBrand = Default + "/single-list-brand";
        public const string SingleListColor = Default + "/single-list-color";
        public const string SingleListSupplier = Default + "/single-list-supplier";
        public const string SingleListProductGrouping = Default + "/single-list-product-grouping";
        public const string SingleListStoreCheckingStatus = Default + "/single-list-store-checking-status";
        public const string SingleListStoreDraftType = Default + "/single-list-store-draft-type";
        public const string SingleListCategory = Default + "/single-list-category";

        public const string FilterListTime = Default + "/filter-list-time";
        public const string FilterListDay = Default + "/filter-list-day";

        public const string CountBanner = Default + "/count-banner";
        public const string ListBanner = Default + "/list-banner";
        public const string GetBanner = Default + "/get-banner";
        public const string CountItem = Default + "/count-item";
        public const string ListItem = Default + "/list-item";
        public const string CountProduct = Default + "/count-product";
        public const string ListProduct = Default + "/list-product";
        public const string ListItemDirectOrder = Default + "/list-item-direct-order";
        public const string CountProductDirectOrder = Default + "/count-product-direct-order";
        public const string ListProductDirectOrder = Default + "/list-product-direct-order";
        public const string GetItem = Default + "/get-item";
        public const string GetProduct = Default + "/get-product";
        public const string CountStore = Default + "/count-store";
        public const string ListStore = Default + "/list-store";
        public const string CountBuyerStore = Default + "/count-buyer-store";
        public const string ListBuyerStore = Default + "/list-buyer-store";
        public const string GetStore = Default + "/get-store";
        public const string ListStoreBalance = Default + "/list-store-balance";
        public const string CreateStore = Default + "/create-store";
        public const string UpdateStore = Default + "/update-store";
        public const string UpdateDraftStore = Default + "/update-draft-store";
        public const string CountStorePlanned = Default + "/count-store-planned";
        public const string ListStorePlanned = Default + "/list-store-planned";
        public const string CountStoreUnPlanned = Default + "/count-store-unplanned";
        public const string ListStoreUnPlanned = Default + "/list-store-unplanned";
        public const string CountStoreInScope = Default + "/count-store-in-scope";
        public const string ListStoreInScope = Default + "/list-store-in-scope";
        public const string CountProblem = Default + "/count-problem";
        public const string ListProblem = Default + "/list-problem";
        public const string GetProblem = Default + "/get-problem";
        public const string CountSurvey = Default + "/count-survey";
        public const string ListSurvey = Default + "/list-survey";
        public const string GetSurveyForm = Default + "/get-survey-form";
        public const string SaveSurveyForm = Default + "/save-survey-form";

        public const string CountStoreScouting = Default + "/count-store-scouting";
        public const string ListStoreScouting = Default + "/list-store-scouting";
        public const string GetStoreScouting = Default + "/get-store-scouting";
        public const string CreateStoreScouting = Default + "/create-store-scouting";
        public const string UpdateStoreScouting = Default + "/update-store-scouting";
        public const string DeleteStoreScouting = Default + "/delete-store-scouting";

        public const string ListRewardHistory = Default + "/list-reward";
        public const string CountRewardHistory = Default + "/count-reward";
        public const string GetRewardHistory = Default + "/get-reward";
        public const string CreateRewardHistory = Default + "/create-reward";
        public const string LuckyDraw = Default + "/lucky-draw";

        public const string CountProductGrouping = Default + "/count-product-grouping";
        public const string ListProductGrouping = Default + "/list-product-grouping";
        public const string CountBrand = Default + "/count-brand";
        public const string ListBrand = Default + "/list-brand";
        public const string ListRequestState = Default + "/list-request-state";
        public const string ListGeneralApprovalState = Default + "/list-general-approval-state";
        public const string CountShowingCategory = Default + "/count-showing-category";
        public const string ListShowingCategory = Default + "/list-showing-category";
        public const string ListUploadedImage = Default + "/list-uploaded-image";

        // appUsers for chatting
        public const string ListAppUser = Default + "/list-app-user";
        public const string CountAppUser = Default + "/count-app-user";

        // resize Image
        public const string GetCroppedImage = Default + "/get/{FileName}";
        public const string CroppedImage = Default + "/crop/{FileName}";
        public const string ResizeImage = Default + "/resize/{FileName}";

        // list path
        public const string ListPath = Default + "/list-path";
        public const string CreateSystemLog = Default + "/create-system-log";

        public const string CountLuckyDraw = Default + "/count-lucky-draw";
        public const string ListLuckyDraw = Default + "/list-lucky-draw";
        public const string GetLuckyDraw = Default + "/get-lucky-draw";
        public const string Draw = Default + "/draw";
        public const string RegistrationLuckyDraw = Default + "/registration-lucky-draw";
        public const string SendLuckyDraw = Default + "/send-lucky-draw";
        public const string ListLuckyDrawStore = Default + "/list-lucky-draw-store";
        public const string CountLuckyDrawStore = Default + "/count-lucky-draw-store";
        public const string ListLuckyDrawHistory = Default + "/lucky-draw-history";
        public const string CountLuckyDrawHistory = Default + "/count-lucky-draw-history";
        public const string ListTurnNotCompletedByEmployee = Default + "/list-turn-not-completed-by-employee";
        public const string CountTurnNotCompletedByEmployee = Default + "/count-turn-not-completed-by-employee";
        public const string CountTurnNotCompletedByStore = Default + "/count-turn-not-completed-by-store";

        public static Dictionary<string, long> Filters = new Dictionary<string, long>
        {
        };

        public static Dictionary<string, IEnumerable<string>> Action = new Dictionary<string, IEnumerable<string>>
        {
            { "Tìm kiếm", new List<string> {
                Master, CountStoreChecking, ListStoreChecking, GetStoreChecking, ListPath, CreateSystemLog
                } },
            { "Checkin", new List<string> {
                Master, CountStoreChecking, ListStoreChecking, GetStoreChecking, CountProductGrouping, ListProductGrouping, CountBrand, ListBrand, CountProduct, ListProduct, CountProductDirectOrder, ListProductDirectOrder,
                Detail, CheckIn,  UpdateStoreChecking, UpdateStoreCheckingImage, CheckOut, PrintIndirectOrder, StoreReport, StoreStatistic, PrintDirectOrder,
                CreateProblem, SaveImage, GetSurveyForm, SaveSurveyForm, GetProduct,
                CountItem, ListItem, ListItemDirectOrder, CountStorePlanned, ListStorePlanned, CountStoreUnPlanned, ListStoreUnPlanned, CountStoreInScope, ListStoreInScope, CountProblem, ListProblem, CountSurvey, ListSurvey, CountStoreScouting, ListStoreScouting,
                SingleListAlbum, SingleListAppUser, SingleListStore, SingleListStoreStatus, SingleListTaxType, SingleListUnitOfMeasure, SingleListProblemType, SingleListStoreScoutingType, SingleListProvince, SingleListDistrict, SingleListWard, SingleListStoreDraftType,
                SingleListSalesOrderType, SingleListCategory, ListRequestState, ListGeneralApprovalState, ListStoreBalance, CountShowingCategory, ListShowingCategory,
                CountLuckyDraw, ListLuckyDraw, GetLuckyDraw, Draw, RegistrationLuckyDraw, SendLuckyDraw, ListLuckyDrawStore, ListLuckyDrawHistory, CountLuckyDrawStore, CountLuckyDrawHistory,
                ListPath, SingleListEstimatedRevenue, FilterListTime, ListUploadedImage
            } },
            { "Quay thưởng", new List<string>{
                Master, CountStoreChecking, ListStoreChecking, GetStoreChecking,
                Detail, ListRewardHistory,  CountRewardHistory, GetRewardHistory, LuckyDraw, CreateRewardHistory,
                CountItem, ListItem, CountStorePlanned, ListStorePlanned, CountStoreUnPlanned, ListStoreUnPlanned, CountStoreInScope, ListStoreInScope, CountProblem, ListProblem, CountSurvey, ListSurvey, CountStoreScouting, ListStoreScouting,
                SingleListAlbum, SingleListAppUser, SingleListStore, SingleListStoreStatus, SingleListTaxType, SingleListUnitOfMeasure, SingleListProblemType, SingleListStoreScoutingType, SingleListProvince, SingleListDistrict, SingleListWard, SingleListStoreDraftType,
                SingleListSalesOrderType, SingleListCategory, ListStoreBalance,
                CountLuckyDraw, ListLuckyDraw, GetLuckyDraw, Draw, RegistrationLuckyDraw, SendLuckyDraw, ListLuckyDrawStore, ListLuckyDrawHistory, CountLuckyDrawStore, CountLuckyDrawHistory,
                ListPath, SingleListEstimatedRevenue, ListTurnNotCompletedByEmployee, CountTurnNotCompletedByEmployee, CountTurnNotCompletedByStore
            } },
            {"Trò chuyện", new List<string>{
                ListPath,
                ListAppUser, CountAppUser
            } }
        };
    }
}
