using DMS.Common;
using DMS.Entities;
using DMS.Enums;
using DMS.Handlers;
using DMS.Models;
using DMS.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Thinktecture;
using TrueSight.Common;
using DMS.Handlers.Configuration;
using Action = DMS.Entities.Action;
using DMS.DWModels;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using DMS.Helpers;
using MessagePack.Resolvers;
using MessagePack;
using System.Diagnostics;
using System.Text.Json;
using System.Dynamic;

namespace DMS.Rpc
{
    public class SetupController : ControllerBase
    {
        private ConnectionMultiplexer RedisConnection;
        private DataContext DataContext;
        private DWContext DWContext;
        private IConfiguration Configuration;
        private IRedisStore RedisStore;
        private IRabbitManager RabbitManager;
        private IUOW UOW;
        private string Hostname;
        private int Port;
        public SetupController(
            DataContext DataContext,
            DWContext DWContext,
            IConfiguration Configuration,
            IRedisStore RedisStore,
            IRabbitManager RabbitManager,
            IUOW UOW
            )
        {
            this.DataContext = DataContext;
            this.DWContext = DWContext;
            this.RabbitManager = RabbitManager;
            this.UOW = UOW;
            this.RedisStore = RedisStore;
        }

        [HttpGet, Route("rpc/dms/setup/store")]
        public ActionResult SetupStore()
        {
            int count = DataContext.Store.Count();
            for (int i = 0; i < count; i = i + 1000)
            {
                int take = count - i > 1000 ? 1000 : count - i;
                List<StoreDAO> StoreDAOs = DataContext.Store
                    .Include(x => x.Organization)
                    .Include(x => x.StoreType)
                    .Skip(i).Take(take)
                    .ToList();
                foreach (StoreDAO StoreDAO in StoreDAOs)
                {
                    StoreDAO.Code = $"{StoreDAO.Organization.Code}.{StoreDAO.StoreType.Code}.{(10000000 + StoreDAO.Id).ToString().Substring(1)}";
                }
                DataContext.Store.BulkMerge(StoreDAOs);
            }
            return Ok();
        }

        #region publish Data
        [HttpGet, Route("rpc/dms/setup/publish-data")]
        public async Task<ActionResult> PublishData()
        {
            await PublishStoreType();
            await PublishStoreGrouping();
            await PublishStoreUser();
            await StoreScoutingPublish();
            await PublishStore();
            await DirectSalesOrderPublish();
            await IndirectSalesOrderPublish();
            await NewItemPublish();
            await ProblemPublish();
            await ProblemTypePublish();
            await ShowingOrderPublish();
            await ShowingOrderWithDrawPublish();
            await ShowingItemPublish();
            await StoreCheckingPublish();
            await StoreScoutingTypePublish();
            await StoreStatusHistoryPublish();
            await StoreUncheckingPublish();
            await PublishKpiGeneral();
            await PublishAlbum();
            await PublishStoreBalance();
            await PublishWarehouse();
            return Ok();
        }

        #region DirectSalesOrder
        [HttpGet, Route("rpc/dms/setup/publish-direct-sales-order")]
        public async Task DirectSalesOrderPublish()
        {
            List<long> DirectSalesOrderIds = await DataContext.DirectSalesOrder.Select(x => x.Id).ToListWithNoLockAsync();
            var count = DirectSalesOrderIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = DirectSalesOrderIds.Skip(skip).Take(take).ToList();
                var DirectSalesOrders = await UOW.DirectSalesOrderRepository.List(Batch);
                RabbitManager.PublishList(DirectSalesOrders, RoutingKeyEnum.DirectSalesOrderSync.Code);
            }
        }
        #endregion

        #region NewProduct
        public async Task NewItemPublish()
        {
            List<long> NewItemIds = await DataContext.Item.Select(x => x.Id).ToListWithNoLockAsync();
            var count = NewItemIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                ItemFilter ItemFilter = new ItemFilter
                {
                    Take = take,
                    Skip = skip,
                    Selects = ItemSelect.ALL
                };
                var NewItems = await UOW.ItemRepository.List(ItemFilter);
                RabbitManager.PublishList(NewItems, RoutingKeyEnum.NewItemHandlerSync.Code);
            }

        }
        #endregion

        #region IndirectSalesOrder
        [HttpGet, Route("rpc/dms/setup/publish-indirect-sales-order")]
        public async Task IndirectSalesOrderPublish()
        {
            List<long> Fact_IndirectSalesOrderIds = new List<long>();
                //await DWContext.Fact_IndirectSalesOrder.Select(x => x.IndirectSalesOrderId).ToListWithNoLockAsync();
            List<long> IndirectSalesOrderIds = await DataContext.IndirectSalesOrder.Where(x => x.Id, new IdFilter { NotIn = Fact_IndirectSalesOrderIds }).Select(x => x.Id).ToListWithNoLockAsync();
            IndirectSalesOrderIds = IndirectSalesOrderIds.OrderBy(x => x).ToList();
            var count = IndirectSalesOrderIds.Count();
            var BatchCounter = (count / 1000) + 1;
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = IndirectSalesOrderIds.Skip(skip).Take(take).ToList();
                var IndirectSalesOrders = await UOW.IndirectSalesOrderRepository.List(Batch);
                RabbitManager.PublishList(IndirectSalesOrders, RoutingKeyEnum.IndirectSalesOrderSync.Code);
            }
        }
        #endregion

        #region Problem
        public async Task ProblemPublish()
        {
            List<long> ProblemIds = await DataContext.Problem.Select(x => x.Id).ToListWithNoLockAsync();
            var count = ProblemIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = ProblemIds.Skip(skip).Take(take).ToList();
                var Problems = await UOW.ProblemRepository.List(Batch);
                RabbitManager.PublishList(Problems, RoutingKeyEnum.ProblemSync.Code);
            }
        }
        #endregion

        public async Task ProblemTypePublish()
        {
            List<long> ProblemTypeIds = await DataContext.ProblemType.Select(x => x.Id).ToListWithNoLockAsync();
            var ProblemTypes = await UOW.ProblemTypeRepository.List(ProblemTypeIds);
            RabbitManager.PublishList(ProblemTypes, RoutingKeyEnum.ProblemTypeSync.Code);
        }

        #region Store
        [HttpGet, Route("rpc/dms/setup/publish-store")]
        public async Task PublishStore()
        {
            List<long> StoreIds = await DataContext.Store.Select(x => x.Id).ToListWithNoLockAsync();
            StoreIds = StoreIds.OrderBy(x => x).ToList();
            var count = StoreIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = StoreIds.Skip(skip).Take(take).ToList();
                var Stores = await UOW.StoreRepository.List(Batch);
                RabbitManager.PublishList(Stores, RoutingKeyEnum.StoreSync.Code);
            }
        }
        #endregion

        #region KpiGeneral
        [HttpGet, Route("rpc/dms/setup/publish-kpi-general")]
        public async Task PublishKpiGeneral()
        {
            List<long> KpiGeneralIds = await DataContext.KpiGeneral.Select(x => x.Id).ToListWithNoLockAsync();
            var count = KpiGeneralIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = KpiGeneralIds.Skip(skip).Take(take).ToList();
                var KpiGenerals = await UOW.KpiGeneralRepository.List(Batch);
                RabbitManager.PublishList(KpiGenerals, RoutingKeyEnum.KpiGeneralSync.Code);
            }
        }
        #endregion

        #region Album
        [HttpGet, Route("rpc/dms/setup/publish-album")]
        public async Task PublishAlbum()
        {
            List<long> AlbumIds = await DataContext.Album.Select(x => x.Id).ToListWithNoLockAsync();
            var count = AlbumIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = AlbumIds.Skip(skip).Take(take).ToList();
                var Albums = await UOW.AlbumRepository.List(Batch);
                RabbitManager.PublishList(Albums, RoutingKeyEnum.AlbumSync.Code);
            }
        }
        #endregion

        #region StoreUser
        [HttpGet, Route("rpc/dms/setup/publish-store-user")]
        public async Task PublishStoreUser()
        {
            List<long> StoreUserIds = await DataContext.StoreUser.Select(x => x.Id).ToListWithNoLockAsync();
            var count = StoreUserIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = StoreUserIds.Skip(skip).Take(take).ToList();
                var StoreUsers = await UOW.StoreUserRepository.List(Batch);
                RabbitManager.PublishList(StoreUsers, RoutingKeyEnum.StoreUserSync.Code);
            }
        }
        #endregion

        #region StoreGrouping
        public async Task PublishStoreGrouping()
        {
            List<long> StoreGroupingIds = await DataContext.StoreGrouping.Select(x => x.Id).ToListWithNoLockAsync();
            var StoreGroupinges = await UOW.StoreGroupingRepository.List(StoreGroupingIds);
            RabbitManager.PublishList(StoreGroupinges, RoutingKeyEnum.StoreGroupingSync.Code);
        }
        #endregion

        #region StoreType
        public async Task PublishStoreType()
        {
            List<long> StoreTypeIds = await DataContext.StoreType.Select(x => x.Id).ToListWithNoLockAsync();
            var StoreTypes = await UOW.StoreTypeRepository.List(StoreTypeIds);
            RabbitManager.PublishList(StoreTypes, RoutingKeyEnum.StoreTypeSync.Code);
        }
        #endregion

        #region StoreBalance
        public async Task PublishStoreBalance()
        {
            List<long> StoreBalanceIds = await DataContext.StoreBalance.Select(x => x.Id).ToListWithNoLockAsync();
            var StoreBalances = await UOW.StoreBalanceRepository.List(StoreBalanceIds);
            RabbitManager.PublishList(StoreBalances, RoutingKeyEnum.StoreBalanceSync.Code);
        }
        #endregion
        #region Warehouse
        [HttpGet, Route("rpc/dms/setup/publish-warehouse")]
        public async Task PublishWarehouse()
        {
            List<long> WarehouseIds = await DataContext.Warehouse.Select(x => x.Id).ToListWithNoLockAsync();
            var Warehouses = await UOW.WarehouseRepository.List(WarehouseIds);
            RabbitManager.PublishList(Warehouses, RoutingKeyEnum.WarehouseSync.Code);
        }
        #endregion
        public async Task ShowingItemPublish()
        {
            List<long> ShowingItemIds = await DataContext.ShowingItem.Select(x => x.Id).ToListWithNoLockAsync();
            var ShowingItems = await UOW.ShowingItemRepository.List(ShowingItemIds);
            RabbitManager.PublishList(ShowingItems, RoutingKeyEnum.ShowingItemSync.Code);
        }

        public async Task ShowingOrderPublish()
        {
            List<long> ShowingOrderIds = await DataContext.ShowingOrder.Select(x => x.Id).ToListWithNoLockAsync();
            var count = ShowingOrderIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = ShowingOrderIds.Skip(skip).Take(take).ToList();
                var ShowingOrders = await UOW.ShowingOrderRepository.List(Batch);
                RabbitManager.PublishList(ShowingOrders, RoutingKeyEnum.ShowingOrderSync.Code);
            }
        }

        public async Task ShowingOrderWithDrawPublish()
        {
            List<long> ShowingOrderWithDrawIds = await DataContext.ShowingOrderWithDraw.Select(x => x.Id).ToListWithNoLockAsync();
            var count = ShowingOrderWithDrawIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = ShowingOrderWithDrawIds.Skip(skip).Take(take).ToList();
                var ShowingOrderWithDraws = await UOW.ShowingOrderWithDrawRepository.List(Batch);
                RabbitManager.PublishList(ShowingOrderWithDraws, RoutingKeyEnum.ShowingOrderWithDrawSync.Code);
            }
        }
        [HttpGet, Route("rpc/dms/setup/publish-store-checking")]
        public async Task StoreCheckingPublish()
        {
            List<long> StoreCheckingIds = await DataContext.StoreChecking.Select(x => x.Id).ToListWithNoLockAsync();
            var count = StoreCheckingIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = StoreCheckingIds.Skip(skip).Take(take).ToList();
                var StoreCheckings = await UOW.StoreCheckingRepository.List(Batch);
                RabbitManager.PublishList(StoreCheckings, RoutingKeyEnum.StoreCheckingSync.Code);
            }
        }

        public async Task StoreScoutingPublish()
        {
            List<long> StoreScoutingIds = await DataContext.StoreScouting.Select(x => x.Id).ToListWithNoLockAsync();
            var count = StoreScoutingIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = StoreScoutingIds.Skip(skip).Take(take).ToList();
                var StoreScoutings = await UOW.StoreScoutingRepository.List(Batch);
                RabbitManager.PublishList(StoreScoutings, RoutingKeyEnum.StoreScoutingSync.Code);
            }
        }

        public async Task StoreScoutingTypePublish()
        {
            List<long> StoreScoutingTypeIds = await DataContext.StoreScoutingType.Select(x => x.Id).ToListWithNoLockAsync();
            var StoreScoutingTypes = await UOW.StoreScoutingTypeRepository.List(StoreScoutingTypeIds);
            RabbitManager.PublishList(StoreScoutingTypes, RoutingKeyEnum.StoreScoutingTypeSync.Code);
        }

        public async Task StoreStatusHistoryPublish()
        {
            List<long> StoreStatusHistoryIds = await DataContext.StoreStatusHistory.Select(x => x.Id).ToListWithNoLockAsync();
            var count = StoreStatusHistoryIds.Count();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                var Batch = StoreStatusHistoryIds.Skip(skip).Take(take).ToList();
                var StoreStatusHistories = await UOW.StoreStatusHistoryRepository.List(Batch);
                RabbitManager.PublishList(StoreStatusHistories, RoutingKeyEnum.StoreStatusHistorySync.Code);
            }
        }

        public async Task StoreUncheckingPublish()
        {
            var count = await DataContext.StoreUnchecking.CountWithNoLockAsync();
            for (int i = 0; i < count; i += 1000)
            {
                int skip = i;
                int take = 1000;
                List<StoreUnchecking> Batch = await DataContext.StoreUnchecking.Select(x => new StoreUnchecking
                {
                    Id = x.Id,
                    AppUserId = x.AppUserId,
                    OrganizationId = x.OrganizationId,
                    StoreId = x.StoreId,
                    Date = x.Date,
                }).Skip(skip).Take(take).ToListWithNoLockAsync();
                RabbitManager.PublishList(Batch, RoutingKeyEnum.StoreUncheckingSync.Code);
            }
        }

        #endregion

        [HttpGet, Route("rpc/dms/setup/init")]
        public async Task<ActionResult> Init()
        {
            InitEnum();
            await InitRoute();
            InitAdmin();
            return Ok();
        }

        #region permission
        private async Task<ActionResult> InitRoute()
        {
            List<Type> routeTypes = typeof(SetupController).Assembly.GetTypes()
               .Where(x => typeof(Root).IsAssignableFrom(x) && x.IsClass && x.Name != "Root")
               .ToList();

            InitMenu(routeTypes);
            InitPage(routeTypes);
            InitField(routeTypes);
            InitAction(routeTypes);

            DataContext.ActionPageMapping.Where(ap => ap.Action.IsDeleted || ap.Page.IsDeleted || ap.Action.Menu.IsDeleted).DeleteFromQuery();
            DataContext.PermissionActionMapping.Where(ap => ap.Action.IsDeleted || ap.Action.Menu.IsDeleted || ap.Permission.Menu.IsDeleted).DeleteFromQuery();
            DataContext.Action.Where(p => p.IsDeleted || p.Menu.IsDeleted).DeleteFromQuery();
            DataContext.Page.Where(p => p.IsDeleted).DeleteFromQuery();
            DataContext.PermissionContent.Where(f => f.Field.IsDeleted == true || f.Field.Menu.IsDeleted).DeleteFromQuery();
            DataContext.Field.Where(pf => pf.IsDeleted || pf.Menu.IsDeleted).DeleteFromQuery();
            DataContext.Permission.Where(p => p.Menu.IsDeleted).DeleteFromQuery();
            DataContext.Menu.Where(v => v.IsDeleted).DeleteFromQuery();
            await RemoveFromCache(nameof(RoleRepository));
            await RemoveFromCache(nameof(PermissionRepository));
            return Ok();
        }
        private async Task RemoveFromCache(string key)
        {
            try
            {
                key = $"{StaticParams.ModuleName}.{key}";
                IServer Server = await RedisStore.GetServer();
                var RedisKeys = Server.Keys(0, $"{key}*");
                foreach (var k in RedisKeys)
                {
                    try
                    {
                        IDatabase Database = await RedisStore.GetDatabase();
                        await Database.KeyDeleteAsync(k, CommandFlags.FireAndForget);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("RemoveFromCache:" + key + ex.Message);
                    }
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine("RemoveFromCache:" + ex2.Message);
            }
        }

        private ActionResult InitAdmin()
        {
            RoleDAO Admin = DataContext.Role
               .Where(r => r.Name == "ADMIN")
               .FirstOrDefault();
            if (Admin == null)
            {
                Admin = new RoleDAO
                {
                    Name = "ADMIN",
                    Code = "ADMIN",
                    StatusId = StatusEnum.ACTIVE.Id,
                };
                DataContext.Role.Add(Admin);
                DataContext.SaveChanges();
            }

            AppUserDAO AppUser = DataContext.AppUser
                .Where(au => au.Username.ToLower() == "Administrator".ToLower())
                .FirstOrDefault();
            if (AppUser == null)
            {
                return Ok();
            }

            AppUserRoleMappingDAO AppUserRoleMappingDAO = DataContext.AppUserRoleMapping
                .Where(ur => ur.RoleId == Admin.Id && ur.AppUserId == AppUser.Id)
                .FirstOrDefault();
            if (AppUserRoleMappingDAO == null)
            {
                AppUserRoleMappingDAO = new AppUserRoleMappingDAO
                {
                    AppUserId = AppUser.Id,
                    RoleId = Admin.Id,
                };
                DataContext.AppUserRoleMapping.Add(AppUserRoleMappingDAO);
                DataContext.SaveChanges();
            }

            List<MenuDAO> Menus = DataContext.Menu.AsNoTracking()
                .Include(v => v.Actions)
                .ToList();
            List<PermissionDAO> permissions = DataContext.Permission.AsNoTracking()
                .Include(p => p.PermissionActionMappings)
                .ToList();
            foreach (MenuDAO Menu in Menus)
            {
                PermissionDAO permission = permissions
                    .Where(p => p.MenuId == Menu.Id && p.RoleId == Admin.Id)
                    .FirstOrDefault();
                if (permission == null)
                {
                    permission = new PermissionDAO
                    {
                        Code = Admin + "_" + Menu.Name,
                        Name = Admin + "_" + Menu.Name,
                        MenuId = Menu.Id,
                        RoleId = Admin.Id,
                        StatusId = StatusEnum.ACTIVE.Id,
                        PermissionActionMappings = new List<PermissionActionMappingDAO>(),
                    };
                    permissions.Add(permission);
                }
                else
                {
                    permission.StatusId = StatusEnum.ACTIVE.Id;
                    if (permission.PermissionActionMappings == null)
                        permission.PermissionActionMappings = new List<PermissionActionMappingDAO>();
                }
                foreach (ActionDAO action in Menu.Actions)
                {
                    PermissionActionMappingDAO PermissionActionMappingDAO = permission.PermissionActionMappings
                        .Where(ppm => ppm.ActionId == action.Id).FirstOrDefault();
                    if (PermissionActionMappingDAO == null)
                    {
                        PermissionActionMappingDAO = new PermissionActionMappingDAO
                        {
                            ActionId = action.Id
                        };
                        permission.PermissionActionMappings.Add(PermissionActionMappingDAO);
                    }
                }

            }
            DataContext.Permission.BulkMerge(permissions);
            permissions.ForEach(p =>
            {
                foreach (var action in p.PermissionActionMappings)
                {
                    action.PermissionId = p.Id;
                }
            });

            List<PermissionActionMappingDAO> PermissionActionMappingDAOs = permissions
                .SelectMany(p => p.PermissionActionMappings).ToList();
            DataContext.PermissionContent.Where(pf => pf.Permission.RoleId == Admin.Id).DeleteFromQuery();
            DataContext.PermissionActionMapping.Where(pf => pf.Permission.RoleId == Admin.Id).DeleteFromQuery();
            DataContext.PermissionActionMapping.BulkMerge(PermissionActionMappingDAOs);
            return Ok();
        }

        private void InitMenu(List<Type> routeTypes)
        {
            List<MenuDAO> Menus = DataContext.Menu.AsNoTracking().ToList();
            Menus.ForEach(m => m.IsDeleted = true);
            foreach (Type type in routeTypes)
            {
                MenuDAO Menu = Menus.Where(m => m.Code == type.Name && m.Name != "Root").FirstOrDefault();
                var DisplayName = type.GetCustomAttributes(typeof(DisplayNameAttribute), true)
               .Select(x => ((DisplayNameAttribute)x).DisplayName)
               .DefaultIfEmpty(type.Name)
               .FirstOrDefault();

                if (Menu == null)
                {
                    Menu = new MenuDAO
                    {
                        Code = type.Name,
                        Name = DisplayName,
                        IsDeleted = false,
                    };
                    Menus.Add(Menu);
                }
                else
                {
                    Menu.Name = DisplayName;
                    Menu.IsDeleted = false;
                }
            }
            DataContext.BulkMerge(Menus);
        }

        private void InitPage(List<Type> routeTypes)
        {
            List<PageDAO> pages = DataContext.Page.AsNoTracking().OrderBy(p => p.Path).ToList();
            pages.ForEach(p => p.IsDeleted = true);
            foreach (Type type in routeTypes)
            {
                var values = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue())
                .ToList();
                foreach (string value in values)
                {
                    PageDAO page = pages.Where(p => p.Path == value).FirstOrDefault();
                    if (page == null)
                    {
                        page = new PageDAO
                        {
                            Name = value,
                            Path = value,
                            IsDeleted = false,
                        };
                        pages.Add(page);
                    }
                    else
                    {
                        page.IsDeleted = false;
                    }
                }
            }
            DataContext.BulkMerge(pages);
        }

        private void InitField(List<Type> routeTypes)
        {
            List<MenuDAO> Menus = DataContext.Menu.AsNoTracking().ToList();
            List<FieldDAO> fields = DataContext.Field.AsNoTracking().ToList();
            fields.ForEach(p => p.IsDeleted = true);
            foreach (Type type in routeTypes)
            {
                MenuDAO Menu = Menus.Where(m => m.Code == type.Name).FirstOrDefault();
                var value = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => !fi.IsInitOnly && fi.FieldType == typeof(Dictionary<string, long>))
                .Select(x => (Dictionary<string, long>)x.GetValue(x))
                .FirstOrDefault();
                if (value == null)
                    continue;
                foreach (var pair in value)
                {
                    FieldDAO field = fields
                        .Where(p => p.MenuId == Menu.Id && p.Name == pair.Key)
                        .FirstOrDefault();
                    if (field == null)
                    {
                        field = new FieldDAO
                        {
                            MenuId = Menu.Id,
                            Name = pair.Key,
                            FieldTypeId = pair.Value,
                            IsDeleted = false,
                        };
                        fields.Add(field);
                    }
                    else
                    {
                        field.FieldTypeId = pair.Value;
                        field.IsDeleted = false;
                    }
                }
            }
            DataContext.BulkMerge(fields);
        }
        private void InitAction(List<Type> routeTypes)
        {
            List<MenuDAO> Menus = DataContext.Menu.AsNoTracking().ToList();
            List<ActionDAO> actions = DataContext.Action.AsNoTracking().ToList();
            actions.ForEach(p => p.IsDeleted = true);
            foreach (Type type in routeTypes)
            {
                MenuDAO Menu = Menus.Where(m => m.Code == type.Name).FirstOrDefault();
                var value = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
               .Where(fi => !fi.IsInitOnly && fi.FieldType == typeof(Dictionary<string, IEnumerable<string>>))
               .Select(x => (Dictionary<string, IEnumerable<string>>)x.GetValue(x))
               .FirstOrDefault();
                if (value == null)
                    continue;
                foreach (var pair in value)
                {
                    ActionDAO action = actions
                        .Where(p => p.MenuId == Menu.Id && p.Name == pair.Key)
                        .FirstOrDefault();
                    if (action == null)
                    {
                        action = new ActionDAO
                        {
                            MenuId = Menu.Id,
                            Name = pair.Key,
                            IsDeleted = false,
                        };
                        actions.Add(action);
                    }
                    else
                    {
                        action.IsDeleted = false;
                    }
                }
            }
            DataContext.BulkMerge(actions);
            List<Action> ListAction = actions.Select(x => new Action
            {
                Id = x.Id,
                Name = x.Name,
                MenuId = x.MenuId,
                IsDeleted = x.IsDeleted,
            }).ToList();
            RabbitManager.PublishList(ListAction, RoutingKeyEnum.ActionSync.Code);

            actions = DataContext.Action.Where(a => a.IsDeleted == false).AsNoTracking().ToList();
            List<PageDAO> PageDAOs = DataContext.Page.AsNoTracking().ToList();
            List<ActionPageMappingDAO> ActionPageMappingDAOs = new List<ActionPageMappingDAO>();
            foreach (Type type in routeTypes)
            {
                MenuDAO Menu = Menus.Where(m => m.Code == type.Name).FirstOrDefault();
                var value = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
               .Where(fi => !fi.IsInitOnly && fi.FieldType == typeof(Dictionary<string, IEnumerable<string>>))
               .Select(x => (Dictionary<string, IEnumerable<string>>)x.GetValue(x))
               .FirstOrDefault();
                if (value == null)
                    continue;

                foreach (var pair in value)
                {
                    ActionDAO action = actions
                        .Where(p => p.MenuId == Menu.Id && p.Name == pair.Key)
                        .FirstOrDefault();
                    if (action == null)
                        continue;
                    IEnumerable<string> pages = pair.Value;
                    foreach (string page in pages)
                    {
                        PageDAO PageDAO = PageDAOs.Where(p => p.Path == page).FirstOrDefault();
                        if (PageDAO != null)
                        {
                            if (!ActionPageMappingDAOs.Any(ap => ap.ActionId == action.Id && ap.PageId == PageDAO.Id))
                            {
                                ActionPageMappingDAOs.Add(new ActionPageMappingDAO
                                {
                                    ActionId = action.Id,
                                    PageId = PageDAO.Id
                                });
                            }
                        }
                    }
                }
            }
            ActionPageMappingDAOs = ActionPageMappingDAOs.Distinct().ToList();
            DataContext.ActionPageMapping.DeleteFromQuery();
            DataContext.BulkInsert(ActionPageMappingDAOs);
        }
        #endregion

        #region Test BuildPermission 
        [HttpGet(), Route("rpc/dms/setup/publish-role")]
        public async Task<ActionResult> SendRole()
        {
            Site Site = await BuildRole();
            RabbitManager.PublishSingle(Site, RoutingKeyEnum.RoleSend.Code);
            return Ok();
        }
        private async Task<Site> BuildRole()
        {
            Site Site = new Site
            {
                Code = "/dms/",
                Name = "DMS",
                IsDisplay = true
            };

            var RoleDAOs = DataContext.Role.AsNoTracking();
            var PermissionDAOs = DataContext.Permission.AsNoTracking();
            var AppUserRoleMappingDAOs = DataContext.AppUserRoleMapping.AsNoTracking();
            var PermissionActionMappingDAOs = DataContext.PermissionActionMapping.AsNoTracking();
            var PermissionContentDAOs = DataContext.PermissionContent.AsNoTracking();

            List<Entities.Role> Roles = new List<Entities.Role>();

            //Build Role 
            foreach (var RoleDAO in RoleDAOs)
            {
                Entities.Role Role = new Entities.Role()
                {
                    Id = RoleDAO.Id,
                    Code = RoleDAO.Code,
                    Name = RoleDAO.Name,
                    StatusId = RoleDAO.StatusId,
                    Used = RoleDAO.Used,
                    Status = RoleDAO.Status == null ? null : new Status
                    {
                        Id = RoleDAO.Status.Id,
                        Code = RoleDAO.Status.Code,
                        Name = RoleDAO.Status.Name,
                    },
                };

                //Build AppUserRoleMapping
                List<AppUserRoleMapping> AppUserRoleMappings = AppUserRoleMappingDAOs.Where(x => x.RoleId == RoleDAO.Id)
                    .Select(ar => new AppUserRoleMapping
                    {
                        AppUserId = ar.AppUserId,
                    }).ToList();
                Role.AppUserRoleMappings = AppUserRoleMappings;

                //Build Permission
                List<Permission> Permissions = PermissionDAOs.Where(x => x.RoleId == RoleDAO.Id)
                    .Select(p => new Permission()
                    {
                        Id = p.Id,
                        Code = p.Code,
                        Name = p.Name,
                        StatusId = p.StatusId,

                        //Get MenuId
                        Menu = new Menu()
                        {
                            Id = p.Menu.Id,
                            Code = p.Menu.Code,
                            Name = p.Menu.Name,
                        },

                        // Build PermissionActionMapping
                        PermissionActionMappings = PermissionActionMappingDAOs.Where(pa => pa.PermissionId == p.Id)
                            .Select(pam => new PermissionActionMapping()
                            {
                                Action = new Action()
                                {
                                    Name = pam.Action.Name
                                }
                            }).ToList(),

                        //Build PermissionContent
                        PermissionContents = PermissionContentDAOs.Where(pc => pc.PermissionId == p.Id)
                            .Select(pec => new PermissionContent()
                            {
                                Id = pec.Id,
                                PermissionOperatorId = pec.PermissionOperatorId,
                                Value = pec.Value,
                                Field = new Field()
                                {
                                    Name = pec.Field.Name
                                },
                            }).ToList(),
                    }).ToList();
                Role.Permissions = Permissions;
                Roles.Add(Role);
            }
            Site.Roles = Roles;
            return Site;
        }
        #endregion

        #region Test BuilMenu
        [HttpGet(), Route("rpc/dms/setup/publish-menu")]
        public async Task<ActionResult> MenuPublish()
        {
            Site Site = await BuildMenu();
            RabbitManager.PublishSingle(Site, RoutingKeyEnum.MenuSend.Code);
            return Ok();
        }

        private async Task<Site> BuildMenu()
        {
            Site Site = new Site
            {
                Code = "/dms/",
                Name = "DMS",
                IsDisplay = true
            };
            List<Type> routeTypes = typeof(SetupController).Assembly.GetTypes()
               .Where(x => typeof(Root).IsAssignableFrom(x) && x.IsClass && x.Name != "Root")
               .ToList();

            List<Menu> Menus = new List<Menu>();

            //Build Menu
            foreach (Type type in routeTypes)
            {
                var DisplayName = type.GetCustomAttributes(typeof(DisplayNameAttribute), true)
               .Select(x => ((DisplayNameAttribute)x).DisplayName)
               .DefaultIfEmpty(type.Name)
               .FirstOrDefault();
                Menu Menu = new Menu
                {
                    Code = type.Name,
                    Name = DisplayName,
                    IsDeleted = false,
                };

                //Build Page
                Menu.Pages = new List<Page>();
                var valuePage = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(string))
                .Select(x => (string)x.GetRawConstantValue())
                .ToList();
                foreach (string value in valuePage)
                {
                    Page page = new Page
                    {
                        Name = value,
                        Path = value,
                        IsDeleted = false,
                    };
                    Menu.Pages.Add(page);
                }

                //Build field
                Menu.Fields = new List<Field>();
                var valueField = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => !fi.IsInitOnly && fi.FieldType == typeof(Dictionary<string, long>))
                .Select(x => (Dictionary<string, long>)x.GetValue(x))
                .FirstOrDefault();
                if (valueField != null)
                {
                    foreach (var pair in valueField)
                    {
                        Field field = new Field
                        {
                            Name = pair.Key,
                            FieldTypeId = pair.Value,
                            IsDeleted = false,
                        };
                        Menu.Fields.Add(field);
                    }
                }

                //Build Action and ActionPageMapping
                Menu.Actions = new List<Action>();
                var valueAction = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
               .Where(fi => !fi.IsInitOnly && fi.FieldType == typeof(Dictionary<string, IEnumerable<string>>))
               .Select(x => (Dictionary<string, IEnumerable<string>>)x.GetValue(x))
               .FirstOrDefault();
                if (valueAction != null)
                {
                    foreach (var pair in valueAction)
                    {
                        Action action = new Action
                        {
                            Name = pair.Key,
                            IsDeleted = false,
                        };
                        action.Pages = new List<Page>();
                        IEnumerable<string> pages = pair.Value;
                        foreach (string page in pages)
                        {
                            Page Page = Menu.Pages.Where(p => p.Path == page).FirstOrDefault();
                            if (Page != null)
                            {
                                //ActionPageMapping
                                action.Pages.Add(new Page
                                {
                                    Name = Page.Name,
                                    Path = Page.Path,
                                });
                            }
                        }
                        Menu.Actions.Add(action);
                    }
                }

                Menus.Add(Menu);
            }

            Site.Menus = Menus;
            return Site;
        }
        #endregion

        #region enum
        private ActionResult InitEnum()
        {
            InitStoreStatusEnum();
            InitStoreStatusHistoryTypeEnum();
            InitPriceListTypeEnum();
            InitSalesOrderTypeEnum();
            InitEditedPriceStatusEnum();
            InitProblemStatusEnum();
            InitERouteTypeEnum();
            InitNotificationStatusEnum();
            InitSurveyEnum();
            InitKpiEnum();
            InitPermissionEnum();
            InitStoreScoutingStatusEnum();
            InitSystemConfigurationEnum();
            InitWorkflowEnum();
            InitPromotionTypeEnum();
            InitPromotionProductAppliedTypeEnum();
            InitPromotionPolicyEnum();
            InitPromotionDiscountTypeEnum();
            InitRewardStatusEnum();
            InitTransactionTypeEnum();
            InitStoreApprovalStateEnum();
            InitErpApprovalStateEnum();
            InitDirectSalesOrderSourceTypeEnum();
            InitExportTemplateEnum();
            InitEstimatedRevenueEnum();
            InitConversationAttachmentTypeEnum();

            return Ok();
        }
        public void InitConversationAttachmentTypeEnum()
        {
            List<AttachmentTypeDAO> ConversationAttachmentTypeEnumList = AttachmentTypeEnum.AttachmentTypeEnumList.Select(item => new AttachmentTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.AttachmentType.BulkSynchronize(ConversationAttachmentTypeEnumList);
        }
        private void InitEstimatedRevenueEnum()
        {
            List<EstimatedRevenueDAO> EstimatedRevenueDAOs = EstimatedRevenueEnum.EstimatedRevenueEnumList.Select(x => new EstimatedRevenueDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToList();
            DataContext.EstimatedRevenue.BulkSynchronize(EstimatedRevenueDAOs);
        }

        private void InitStoreStatusHistoryTypeEnum()
        {
            List<StoreStatusHistoryTypeDAO> StoreStatusHistoryTypeDAOs = StoreStatusHistoryTypeEnum.StoreStatusHistoryTypeEnumList.Select(x => new StoreStatusHistoryTypeDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToList();
            DataContext.StoreStatusHistoryType.BulkSynchronize(StoreStatusHistoryTypeDAOs);
        }

        private void InitStoreStatusEnum()
        {
            List<StoreStatusDAO> StoreStatusDAOs = StoreStatusEnum.StoreStatusEnumList.Select(x => new StoreStatusDAO
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToList();
            DataContext.StoreStatus.BulkSynchronize(StoreStatusDAOs);
            List<StoreStatus> StoreStatuses = StoreStatusDAOs.Select(x => new StoreStatus
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToList();

            RabbitManager.PublishList(StoreStatuses, RoutingKeyEnum.StoreStatusSync.Code);
        }

        private void InitERouteTypeEnum()
        {
            List<ERouteTypeDAO> ERouteTypeEnumList = ERouteTypeEnum.ERouteTypeEnumList.Select(item => new ERouteTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.ERouteType.BulkSynchronize(ERouteTypeEnumList);
        }

        private void InitRewardStatusEnum()
        {
            List<RewardStatusDAO> RewardStatusEnumList = RewardStatusEnum.RewardStatusEnumList.Select(item => new RewardStatusDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.RewardStatus.BulkSynchronize(RewardStatusEnumList);
        }

        private void InitTransactionTypeEnum()
        {
            List<TransactionTypeDAO> TransactionTypeEnumList = TransactionTypeEnum.TransactionTypeEnumList.Select(item => new TransactionTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.TransactionType.BulkSynchronize(TransactionTypeEnumList);
        }

        private void InitStoreScoutingStatusEnum()
        {
            List<StoreScoutingStatusDAO> StoreScoutingStatusEnumList = StoreScoutingStatusEnum.StoreScoutingStatusEnumList.Select(item => new StoreScoutingStatusDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.StoreScoutingStatus.BulkSynchronize(StoreScoutingStatusEnumList);
        }

        private void InitNotificationStatusEnum()
        {
            List<NotificationStatusDAO> NotificationStatusEnumList = NotificationStatusEnum.NotificationStatusEnumList.Select(item => new NotificationStatusDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.NotificationStatus.BulkSynchronize(NotificationStatusEnumList);
        }

        private void InitKpiEnum()
        {
            List<KpiCriteriaGeneralDAO> KpiCriteriaGeneralDAOs = DataContext.KpiCriteriaGeneral.ToList();
            foreach (GenericEnum KpiCriteriaGeneralEnum in KpiCriteriaGeneralEnum.KpiCriteriaGeneralEnumList)
            {
                KpiCriteriaGeneralDAO KpiCriteriaGeneralDAO = KpiCriteriaGeneralDAOs
                    .Where(x => x.Id == KpiCriteriaGeneralEnum.Id)
                    .FirstOrDefault();
                if (KpiCriteriaGeneralDAO == null)
                {
                    KpiCriteriaGeneralDAO = new KpiCriteriaGeneralDAO();
                    KpiCriteriaGeneralDAO.Id = KpiCriteriaGeneralEnum.Id;
                    KpiCriteriaGeneralDAO.StatusId = StatusEnum.ACTIVE.Id;
                    KpiCriteriaGeneralDAOs.Add(KpiCriteriaGeneralDAO);
                }
                KpiCriteriaGeneralDAO.Code = KpiCriteriaGeneralEnum.Code;
                KpiCriteriaGeneralDAO.Name = KpiCriteriaGeneralEnum.Name;
            }
            DataContext.KpiCriteriaGeneral.BulkSynchronize(KpiCriteriaGeneralDAOs);
            List<KpiCriteriaGeneral> KpiCriteriaGenerals = KpiCriteriaGeneralDAOs.Select(x => new KpiCriteriaGeneral
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToList();
            RabbitManager.PublishList(KpiCriteriaGenerals, RoutingKeyEnum.KpiCriteriaGeneralSync.Code);

            List<KpiCriteriaItemDAO> KpiCriteriaItemDAOs = DataContext.KpiCriteriaItem.ToList();
            foreach (var KpiCriteriaItemEnum in KpiCriteriaItemEnum.KpiCriteriaItemEnumList)
            {
                KpiCriteriaItemDAO KpiCriteriaItemDAO = KpiCriteriaItemDAOs
                    .Where(x => x.Id == KpiCriteriaItemEnum.Id)
                    .FirstOrDefault();
                if (KpiCriteriaItemDAO == null)
                {
                    KpiCriteriaItemDAO = new KpiCriteriaItemDAO();
                    KpiCriteriaItemDAO.Id = KpiCriteriaItemEnum.Id;
                    KpiCriteriaItemDAO.StatusId = StatusEnum.ACTIVE.Id;
                    KpiCriteriaItemDAOs.Add(KpiCriteriaItemDAO);
                }
                KpiCriteriaItemDAO.Code = KpiCriteriaItemEnum.Code;
                KpiCriteriaItemDAO.Name = KpiCriteriaItemEnum.Name;
            }
            DataContext.KpiCriteriaItem.BulkSynchronize(KpiCriteriaItemDAOs);
            List<KpiCriteriaItem> KpiCriteriaItems = KpiCriteriaItemDAOs.Select(x => new KpiCriteriaItem
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name,
            }).ToList();
            //RabbitManager.PublishList(KpiCriteriaItems, RoutingKeyEnum.KpiCriteriaItemSync.Code);

            List<KpiProductGroupingCriteriaDAO> KpiProductGroupingCriteriaDAOs = DataContext.KpiProductGroupingCriteria.ToList();
            foreach (var KpiProductGroupingCriteriaEnum in KpiProductGroupingCriteriaEnum.KpiProductGroupingCriteriaEnumList)
            {
                KpiProductGroupingCriteriaDAO KpiProductGroupingCriteriaDAO = KpiProductGroupingCriteriaDAOs
                    .Where(x => x.Id == KpiProductGroupingCriteriaEnum.Id)
                    .FirstOrDefault();
                if (KpiProductGroupingCriteriaDAO == null)
                {
                    KpiProductGroupingCriteriaDAO = new KpiProductGroupingCriteriaDAO();
                    KpiProductGroupingCriteriaDAO.Id = KpiProductGroupingCriteriaEnum.Id;
                    KpiProductGroupingCriteriaDAO.StatusId = StatusEnum.ACTIVE.Id;
                    KpiProductGroupingCriteriaDAOs.Add(KpiProductGroupingCriteriaDAO);
                }
                KpiProductGroupingCriteriaDAO.Code = KpiProductGroupingCriteriaEnum.Code;
                KpiProductGroupingCriteriaDAO.Name = KpiProductGroupingCriteriaEnum.Name;
            }
            DataContext.BulkSynchronize(KpiProductGroupingCriteriaDAOs);
            List<KpiProductGroupingCriteria> KpiProductGroupingCriterias = KpiProductGroupingCriteriaDAOs.Select(x => new KpiProductGroupingCriteria
            {
                Id = x.Id,
                Code = x.Code,
                Name = x.Name
            }).ToList();
            //RabbitManager.PublishList(KpiProductGroupingCriterias, RoutingKeyEnum.KpiProductGroupingCriteriaSync.Code);

            List<KpiProductGroupingTypeDAO> KpiProductGroupingTypeDAOs = KpiProductGroupingTypeEnum.KpiProductGroupingTypeEnumList.Select(item => new KpiProductGroupingTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.KpiProductGroupingType.BulkSynchronize(KpiProductGroupingTypeDAOs);

            List<KpiPeriodDAO> KpiPeriodDAOs = KpiPeriodEnum.KpiPeriodEnumList.Select(item => new KpiPeriodDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.KpiPeriod.BulkSynchronize(KpiPeriodDAOs);

            List<KpiYearDAO> KpiYearDAOs = KpiYearEnum.KpiYearEnumList.Select(item => new KpiYearDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.KpiYear.BulkSynchronize(KpiYearDAOs);
        }

        private void InitEditedPriceStatusEnum()
        {
            List<EditedPriceStatusDAO> EditedPriceStatusEnumList = EditedPriceStatusEnum.EditedPriceStatusEnumList.Select(item => new EditedPriceStatusDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.EditedPriceStatus.BulkSynchronize(EditedPriceStatusEnumList);
        }

        private void InitPriceListTypeEnum()
        {
            List<PriceListTypeDAO> PriceListTypeEnumList = PriceListTypeEnum.PriceListTypeEnumList.Select(item => new PriceListTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.PriceListType.BulkSynchronize(PriceListTypeEnumList);
        }

        private void InitPromotionTypeEnum()
        {
            List<PromotionTypeDAO> PromotionTypeEnumList = PromotionTypeEnum.PromotionTypeEnumList.Select(item => new PromotionTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.PromotionType.BulkSynchronize(PromotionTypeEnumList);
        }

        private void InitPromotionProductAppliedTypeEnum()
        {
            List<PromotionProductAppliedTypeDAO> PromotionProductAppliedTypeDAOs = PromotionProductAppliedTypeEnum.PromotionProductAppliedTypeEnumList.Select(item => new PromotionProductAppliedTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.PromotionProductAppliedType.BulkSynchronize(PromotionProductAppliedTypeDAOs);
        }

        private void InitPromotionPolicyEnum()
        {
            List<PromotionPolicyDAO> PromotionPolicyEnumList = PromotionPolicyEnum.PromotionPolicyEnumList.Select(item => new PromotionPolicyDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.PromotionPolicy.BulkSynchronize(PromotionPolicyEnumList);
        }

        private void InitPromotionDiscountTypeEnum()
        {
            List<PromotionDiscountTypeDAO> PromotionDiscountTypeEnumList = PromotionDiscountTypeEnum.PromotionDiscountTypeEnumList.Select(item => new PromotionDiscountTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.PromotionDiscountType.BulkSynchronize(PromotionDiscountTypeEnumList);
        }

        private void InitSalesOrderTypeEnum()
        {
            List<SalesOrderTypeDAO> SalesOrderTypeEnumList = SalesOrderTypeEnum.SalesOrderTypeEnumList.Select(item => new SalesOrderTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.SalesOrderType.BulkSynchronize(SalesOrderTypeEnumList);
        }

        private void InitSurveyEnum()
        {
            List<SurveyQuestionTypeDAO> SurveyQuestionTypeEnumList = SurveyQuestionTypeEnum.SurveyQuestionTypeEnumList.Select(item => new SurveyQuestionTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.SurveyQuestionType.BulkSynchronize(SurveyQuestionTypeEnumList);

            List<SurveyOptionTypeDAO> SurveyOptionTypeEnumList = SurveyOptionTypeEnum.SurveyOptionTypeEnumList.Select(item => new SurveyOptionTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.SurveyOptionType.BulkSynchronize(SurveyOptionTypeEnumList);

            List<SurveyRespondentTypeDAO> SurveyRespondentTypeEnumList = SurveyRespondentTypeEnum.SurveyRespondentTypeEnumList.Select(item => new SurveyRespondentTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.SurveyRespondentType.BulkSynchronize(SurveyRespondentTypeEnumList);
        }

        private void InitProblemStatusEnum()
        {
            List<ProblemStatusDAO> ProblemStatusEnumList = ProblemStatusEnum.ProblemStatusEnumList.Select(item => new ProblemStatusDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.ProblemStatus.BulkSynchronize(ProblemStatusEnumList);
        }

        private void InitPermissionEnum()
        {
            List<FieldTypeDAO> FieldTypeDAOs = FieldTypeEnum.List.Select(item => new FieldTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.FieldType.BulkSynchronize(FieldTypeDAOs);
            List<PermissionOperatorDAO> PermissionOperatorDAOs = new List<PermissionOperatorDAO>();
            List<PermissionOperatorDAO> ID = PermissionOperatorEnum.PermissionOperatorEnumForID.Select(item => new PermissionOperatorDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                FieldTypeId = FieldTypeEnum.ID.Id,
            }).ToList();
            PermissionOperatorDAOs.AddRange(ID);
            List<PermissionOperatorDAO> STRING = PermissionOperatorEnum.PermissionOperatorEnumForSTRING.Select(item => new PermissionOperatorDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                FieldTypeId = FieldTypeEnum.STRING.Id,
            }).ToList();
            PermissionOperatorDAOs.AddRange(STRING);

            List<PermissionOperatorDAO> LONG = PermissionOperatorEnum.PermissionOperatorEnumForLONG.Select(item => new PermissionOperatorDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                FieldTypeId = FieldTypeEnum.LONG.Id,
            }).ToList();
            PermissionOperatorDAOs.AddRange(LONG);

            List<PermissionOperatorDAO> DECIMAL = PermissionOperatorEnum.PermissionOperatorEnumForDECIMAL.Select(item => new PermissionOperatorDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                FieldTypeId = FieldTypeEnum.DECIMAL.Id,
            }).ToList();
            PermissionOperatorDAOs.AddRange(DECIMAL);

            List<PermissionOperatorDAO> DATE = PermissionOperatorEnum.PermissionOperatorEnumForDATE.Select(item => new PermissionOperatorDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                FieldTypeId = FieldTypeEnum.DATE.Id,
            }).ToList();
            PermissionOperatorDAOs.AddRange(DATE);

            DataContext.PermissionOperator.BulkSynchronize(PermissionOperatorDAOs);
        }

        private void InitSystemConfigurationEnum()
        {
            List<SystemConfigurationDAO> SystemConfigurationDAOs = DataContext.SystemConfiguration.ToList();
            foreach (GenericEnum item in SystemConfigurationEnum.SystemConfigurationEnumList)
            {
                SystemConfigurationDAO SystemConfigurationDAO = SystemConfigurationDAOs.Where(sc => sc.Id == item.Id).FirstOrDefault();
                if (SystemConfigurationDAO == null)
                {
                    SystemConfigurationDAO = new SystemConfigurationDAO();
                    SystemConfigurationDAO.Id = item.Id;
                    SystemConfigurationDAO.Code = item.Code;
                    SystemConfigurationDAO.Name = item.Name;
                    SystemConfigurationDAO.Value = null;
                    SystemConfigurationDAOs.Add(SystemConfigurationDAO);
                }
            }
            DataContext.SystemConfiguration.BulkSynchronize(SystemConfigurationDAOs);
        }

        private void InitWorkflowEnum()
        {
            List<WorkflowParameterTypeDAO> WorkflowParameterTypeDAOs = WorkflowParameterTypeEnum.List.Select(item => new WorkflowParameterTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.WorkflowParameterType.BulkSynchronize(WorkflowParameterTypeDAOs);

            List<WorkflowOperatorDAO> WorkflowOperatorDAOs = new List<WorkflowOperatorDAO>();
            List<WorkflowOperatorDAO> ID = WorkflowOperatorEnum.WorkflowOperatorEnumForID.Select(item => new WorkflowOperatorDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                WorkflowParameterTypeId = WorkflowParameterTypeEnum.ID.Id,
            }).ToList();
            WorkflowOperatorDAOs.AddRange(ID);

            List<WorkflowOperatorDAO> STRING = WorkflowOperatorEnum.WorkflowOperatorEnumForSTRING.Select(item => new WorkflowOperatorDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                WorkflowParameterTypeId = FieldTypeEnum.STRING.Id,
            }).ToList();
            WorkflowOperatorDAOs.AddRange(STRING);

            List<WorkflowOperatorDAO> LONG = WorkflowOperatorEnum.WorkflowOperatorEnumForLONG.Select(item => new WorkflowOperatorDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                WorkflowParameterTypeId = FieldTypeEnum.LONG.Id,
            }).ToList();
            WorkflowOperatorDAOs.AddRange(LONG);

            List<WorkflowOperatorDAO> DECIMAL = WorkflowOperatorEnum.WorkflowOperatorEnumForDECIMAL.Select(item => new WorkflowOperatorDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                WorkflowParameterTypeId = FieldTypeEnum.DECIMAL.Id,
            }).ToList();
            WorkflowOperatorDAOs.AddRange(DECIMAL);

            List<WorkflowOperatorDAO> DATE = WorkflowOperatorEnum.WorkflowOperatorEnumForDATE.Select(item => new WorkflowOperatorDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                WorkflowParameterTypeId = FieldTypeEnum.DATE.Id,
            }).ToList();
            WorkflowOperatorDAOs.AddRange(DATE);

            DataContext.WorkflowOperator.BulkSynchronize(WorkflowOperatorDAOs);


            List<WorkflowTypeDAO> WorkflowTypeEnumList = WorkflowTypeEnum.WorkflowTypeEnumList.Select(item => new WorkflowTypeDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.WorkflowType.BulkSynchronize(WorkflowTypeEnumList);

            List<WorkflowParameterDAO> WorkflowParameterDAOs = new List<WorkflowParameterDAO>();

            List<WorkflowParameterDAO> EROUTE_PARAMETER = WorkflowParameterEnum.ERouteEnumList.Select(item => new WorkflowParameterDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                WorkflowParameterTypeId = long.Parse(item.Value),
                WorkflowTypeId = WorkflowTypeEnum.EROUTE.Id,
            }).ToList();
            WorkflowParameterDAOs.AddRange(EROUTE_PARAMETER);

            List<WorkflowParameterDAO> INDIRECT_SALES_ORDER_PARAMETER = WorkflowParameterEnum.IndirectSalesOrderEnumList.Select(item => new WorkflowParameterDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                WorkflowParameterTypeId = long.Parse(item.Value),
                WorkflowTypeId = WorkflowTypeEnum.INDIRECT_SALES_ORDER.Id,
            }).ToList();
            WorkflowParameterDAOs.AddRange(INDIRECT_SALES_ORDER_PARAMETER);

            List<WorkflowParameterDAO> DIRECT_SALES_ORDER_PARAMETER = WorkflowParameterEnum.DirectSalesOrderEnumList.Select(item => new WorkflowParameterDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                WorkflowParameterTypeId = long.Parse(item.Value),
                WorkflowTypeId = WorkflowTypeEnum.DIRECT_SALES_ORDER.Id,
            }).ToList();
            WorkflowParameterDAOs.AddRange(DIRECT_SALES_ORDER_PARAMETER);

            List<WorkflowParameterDAO> PRICE_LIST_PARAMETER = WorkflowParameterEnum.PriceListEnumList.Select(item => new WorkflowParameterDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
                WorkflowParameterTypeId = long.Parse(item.Value),
                WorkflowTypeId = WorkflowTypeEnum.PRICE_LIST.Id,
            }).ToList();
            WorkflowParameterDAOs.AddRange(PRICE_LIST_PARAMETER);

            DataContext.WorkflowParameter.BulkMerge(WorkflowParameterDAOs);


            List<WorkflowStateDAO> WorkflowStateEnumList = WorkflowStateEnum.WorkflowStateEnumList.Select(item => new WorkflowStateDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.WorkflowState.BulkSynchronize(WorkflowStateEnumList);
            List<RequestStateDAO> RequestStateEnumList = RequestStateEnum.RequestStateEnumList.Select(item => new RequestStateDAO
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Name,
            }).ToList();
            DataContext.RequestState.BulkSynchronize(RequestStateEnumList);
        }

        private void InitStoreApprovalStateEnum()
        {
            List<StoreApprovalStateDAO> StoreApprovalStateDAOs = DataContext.StoreApprovalState.ToList();
            foreach (GenericEnum item in StoreApprovalStateEnum.StoreApprovalStateEnumList)
            {
                StoreApprovalStateDAO StoreApprovalStateDAO = StoreApprovalStateDAOs.Where(sc => sc.Id == item.Id).FirstOrDefault();
                if (StoreApprovalStateDAO == null)
                {
                    StoreApprovalStateDAO = new StoreApprovalStateDAO();
                    StoreApprovalStateDAO.Id = item.Id;
                    StoreApprovalStateDAO.Code = item.Code;
                    StoreApprovalStateDAO.Name = item.Name;
                    StoreApprovalStateDAOs.Add(StoreApprovalStateDAO);
                }
            }
            DataContext.StoreApprovalState.BulkSynchronize(StoreApprovalStateDAOs);
        }

        private void InitErpApprovalStateEnum()
        {
            List<ErpApprovalStateDAO> ErpApprovalStateDAOs = DataContext.ErpApprovalState.ToList();
            foreach (GenericEnum item in ErpApprovalStateEnum.ErpApprovalStateEnumList)
            {
                ErpApprovalStateDAO ErpApprovalStateDAO = ErpApprovalStateDAOs.Where(sc => sc.Id == item.Id).FirstOrDefault();
                if (ErpApprovalStateDAO == null)
                {
                    ErpApprovalStateDAO = new ErpApprovalStateDAO();
                    ErpApprovalStateDAO.Id = item.Id;
                    ErpApprovalStateDAO.Code = item.Code;
                    ErpApprovalStateDAO.Name = item.Name;
                    ErpApprovalStateDAOs.Add(ErpApprovalStateDAO);
                }
            }
            DataContext.ErpApprovalState.BulkSynchronize(ErpApprovalStateDAOs);
        }

        private void InitDirectSalesOrderSourceTypeEnum()
        {
            List<DirectSalesOrderSourceTypeDAO> DirectSalesOrderSourceTypeDAOs = DataContext.DirectSalesOrderSourceType.ToList();
            foreach (GenericEnum item in DirectSalesOrderSourceTypeEnum.DirectSalesOrderSourceTypeEnumList)
            {
                DirectSalesOrderSourceTypeDAO DirectSalesOrderSourceTypeDAO = DirectSalesOrderSourceTypeDAOs.Where(sc => sc.Id == item.Id).FirstOrDefault();
                if (DirectSalesOrderSourceTypeDAO == null)
                {
                    DirectSalesOrderSourceTypeDAO = new DirectSalesOrderSourceTypeDAO();
                    DirectSalesOrderSourceTypeDAO.Id = item.Id;
                    DirectSalesOrderSourceTypeDAO.Code = item.Code;
                    DirectSalesOrderSourceTypeDAO.Name = item.Name;
                    DirectSalesOrderSourceTypeDAOs.Add(DirectSalesOrderSourceTypeDAO);
                }
            }
            DataContext.DirectSalesOrderSourceType.BulkSynchronize(DirectSalesOrderSourceTypeDAOs);
        }

        private void InitExportTemplateEnum()
        {
            List<ExportTemplateDAO> ExportTemplateDAOs = DataContext.ExportTemplate.ToList();
            foreach (GenericEnum item in ExportTemplateEnum.ExportTemplateEnumList)
            {
                ExportTemplateDAO ExportTemplateDAO = ExportTemplateDAOs.Where(sc => sc.Id == item.Id).FirstOrDefault();
                if (ExportTemplateDAO == null)
                {
                    ExportTemplateDAO = new ExportTemplateDAO();
                    ExportTemplateDAO.Id = item.Id;
                    ExportTemplateDAO.Code = item.Code;
                    ExportTemplateDAO.Name = item.Name;
                    ExportTemplateDAOs.Add(ExportTemplateDAO);
                }
            }
            DataContext.ExportTemplate.BulkSynchronize(ExportTemplateDAOs);
        }
        #endregion
        #region Exxport All Database
        [Route("rpc/dms/setup/export-database"), HttpPost]
        public ActionResult ExportDatabase(string source, string catalog)
        {

            string conString = $"data source={source};initial catalog={catalog};persist security info=True;user id=SA;password=123@123a;multipleactiveresultsets=True;";

            var excelBytes = GetDatabaseList(conString);

            return File(excelBytes, "application/octet-stream", "Database.xlsx");
        }
        public byte[] GetDatabaseList(string conString)
        {
            using SqlConnection con = new SqlConnection(conString);
            con.Open();
            MemoryStream memoryStream = new MemoryStream();
            ExcelPackage excel = new ExcelPackage(memoryStream);

            List<string> TableNames = GetTableNames(con);
            foreach (var tableName in TableNames)
            {
                if (tableName == "[STAFF].[ERouteContentDay]") continue;
                var listName = tableName.Split('.').ToList();
                var filter = listName[1].Where(x => Char.IsLetter(x));
                if (filter.Count() > 31)
                {
                    filter = filter.Where(x => x < 91);
                }
                var sheetName = new string(filter.ToArray());
                var workSheet = excel.Workbook.Worksheets.Add(sheetName);
                workSheet.TabColor = System.Drawing.Color.Black;
                workSheet.DefaultRowHeight = 12;
                int recordIndex = 1;
                var ColumnNames = GetColumnNames(con, tableName);
                using SqlCommand cmd = new SqlCommand("SELECT * from " + tableName, con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    int i = 1;
                    foreach (var columnName in ColumnNames)
                    {
                        if (recordIndex == 1)
                        {
                            workSheet.Cells[recordIndex, i].Value = columnName;
                            workSheet.Cells[recordIndex + 1, i].Value = rdr[columnName].ToString();
                        }
                        else
                        {
                            workSheet.Cells[recordIndex, i].Value = rdr[columnName].ToString();
                        }
                        i++;
                    }
                    if (recordIndex == 1)
                    {
                        recordIndex += 2;
                    }
                    else
                    {
                        recordIndex++;
                    }

                }
            }
            excel.Save();
            return memoryStream.ToArray();
        }
        public static List<string> GetTableNames(SqlConnection connection)
        {
            using SqlCommand cmd = new SqlCommand(@"SELECT t.name AS tname, s.name AS sname FROM sys.Schemas s JOIN sys.tables t ON s.schema_id = t.schema_id ORDER BY t.name", connection);
            SqlDataReader rdr = cmd.ExecuteReader();
            List<string> TableNames = new List<string>();
            while (rdr.Read())
            {
                string tableName = "[" + rdr.GetString(1) + "]" + ".[" + rdr.GetString(0) + "]";
                Console.WriteLine(tableName);
                TableNames.Add(tableName);
            }
            return TableNames;
        }
        private static List<string> GetColumnNames(SqlConnection sqlCon, string tableName)
        {
            var result = new List<string>();
            using SqlCommand sqlCmd = new SqlCommand(@"select * from " + tableName + " where 1=0", sqlCon);
            var sqlDR = sqlCmd.ExecuteReader();
            var dataTable = sqlDR.GetSchemaTable();
            foreach (DataRow row in dataTable.Rows) result.Add(row.Field<string>("ColumnName"));
            return result;
        }
        #endregion
        [Route("rpc/dms/setup/product-calculator"), HttpGet]
        public ActionResult UpdateProductOrderSaleCounter()
        {
            var Product_query = DataContext.Product.AsNoTracking();
            Product_query = Product_query.Where(x => x.DeletedAt == null);
            var Products = Product_query.Select(x => new Product { Id = x.Id }).ToList();
            RabbitManager.PublishList(Products, RoutingKeyEnum.ProductCal.Code);
            return Ok();
        }



        [Route("rpc/dms/setup/test"), HttpPost]
        public async Task<ActionResult<List<dynamic>>>  Test()
        {
            List <Warehouse> Warehouses = await UOW.WarehouseRepository.List(new WarehouseFilter
            {
                Take = int.MaxValue,
                Skip = 0,
                Selects = WarehouseSelect.Id
            });
            List<long> Ids = Warehouses.Select(x => x.Id).ToList();
            Warehouses = await UOW.WarehouseRepository.List(Ids);

            var resolver = CompositeResolver.Create(
                StandardResolver.Instance,
                DynamicGenericResolver.Instance,
                ContractlessStandardResolver.Instance
            );
            var pack_options = MessagePackSerializerOptions.Standard.WithResolver(resolver);


            var json_options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                WriteIndented = false
            };

            Stopwatch Stopwatch = new Stopwatch();
            //Serialize with MessagePack
            Stopwatch.Start();
            byte[] data1 = MessagePackSerializer.Serialize(Warehouses, pack_options);
            var ser_time1 = Stopwatch.ElapsedMilliseconds;
            Stopwatch.Stop();
            var size1 = data1.Length;
            //Deserialize with MessagePack
            Stopwatch.Start();
            List<Warehouse> result1 = MessagePackSerializer.Deserialize<List<Warehouse>>(data1, pack_options);
            var des_time1 = Stopwatch.ElapsedMilliseconds;
            Stopwatch.Stop();


            //Serializser_time1e with JsonSerializer
            Stopwatch.Start();
            string data2 = JsonSerializer.Serialize(Warehouses, json_options);
            var ser_time2 = Stopwatch.ElapsedMilliseconds;
            Stopwatch.Stop();
            var size2 = data2.Length;
            //Deserialize with JsonSerializer
            Stopwatch.Start();
            List<Warehouse> result2 = JsonSerializer.Deserialize<List<Warehouse>>(data2, json_options);
            var des_time2 = Stopwatch.ElapsedMilliseconds;
            Stopwatch.Stop();

            dynamic messagepack = new ExpandoObject();
            messagepack.Pack_Serial_Time = ser_time1;
            messagepack.Pack_Deserial_Time = des_time1;
            messagepack.Pack_Size = size1;

            dynamic jsonserializer = new ExpandoObject();
            jsonserializer.Json_Serial_Time = ser_time2;
            jsonserializer.Json_Deserial_Time = des_time2;
            jsonserializer.Json_Size = size2;

            List<dynamic> rs = new List<dynamic>();
            rs.Add(messagepack);
            rs.Add(jsonserializer);

            return rs;
        }
    }
}