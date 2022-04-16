using DMS.Common;
using DMS.DWModels;
using DMS.Entities;
using DMS.Enums;
using DMS.Helpers;
using DMS.Models;
using DMS.Repositories;
using DMS.Services.MAppUser;
using DMS.Services.MOrganization;
using DMS.Services.MProductGrouping;
using DMS.Services.MProductType;
using DMS.Services.MStore;
using DMS.Services.MStoreGrouping;
using DMS.Services.MStoreScouting;
using DMS.Services.MStoreType;
using DMS.Services.MWarehouse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TrueSight.Common;

namespace DMS.Rpc
{
    public class Root
    {
        protected const string Module = "dms";
        protected const string Rpc = "rpc/";
        protected const string Rest = "rest/";
        protected const string WebABEModule = "order-hub-external";
        protected const string MobileABEModule = "dms-abe";
    }
    [Authorize]
    [Authorize(Policy = "Permission")]
    public class RpcController : ControllerBase
    {
        protected async Task<List<long>> FilterOrganization(IOrganizationService OrganizationService, ICurrentContext CurrentContext)
        {
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return new List<long>();
            List<Organization> Organizations = await OrganizationService.List(new OrganizationFilter
            {
                Skip = 0,
                Take = int.MaxValue,
                Selects = OrganizationSelect.ALL,
                OrderBy = OrganizationOrder.Id,
                OrderType = OrderType.ASC
            });

            List<long> In = null;
            List<long> NotIn = null;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "OrganizationId")
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal != null)
                        {
                            if (In == null) In = new List<long>();
                            In.Add(FilterPermissionDefinition.IdFilter.Equal.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.In != null)
                        {
                            if (In == null) In = new List<long>();
                            In.AddRange(FilterPermissionDefinition.IdFilter.In);
                        }

                        if (FilterPermissionDefinition.IdFilter.NotEqual != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.Add(FilterPermissionDefinition.IdFilter.NotEqual.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.NotIn != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.AddRange(FilterPermissionDefinition.IdFilter.NotIn);
                        }
                    }
                }
            }

            if (In != null)
            {
                List<string> InPaths = Organizations.Where(o => In.Count == 0 || In.Contains(o.Id)).Select(o => o.Path).ToList();
                Organizations = Organizations.Where(o => InPaths.Any(p => o.Path.StartsWith(p))).ToList();
            }
            if (NotIn != null)
            {
                List<string> NotInPaths = Organizations.Where(o => NotIn.Count == 0 || NotIn.Contains(o.Id)).Select(o => o.Path).ToList();
                Organizations = Organizations.Where(o => !NotInPaths.Any(p => o.Path.StartsWith(p))).ToList();
            }

            List<long> organizationIds = Organizations.Select(o => o.Id).ToList();

            return organizationIds;
        }
        protected async Task<List<long>> FilterAppUser(IAppUserService AppUserService, IOrganizationService OrganizationService, ICurrentContext CurrentContext)
        {
            List<long> organizationIds = await FilterOrganization(OrganizationService, CurrentContext);

            List<long> In = null;
            List<long> NotIn = null;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "AppUserId")
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal != null)
                        {
                            if (In == null) In = new List<long>();
                            In.Add(FilterPermissionDefinition.IdFilter.Equal.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.In != null)
                        {
                            if (In == null) In = new List<long>();
                            In.AddRange(FilterPermissionDefinition.IdFilter.In);
                        }

                        if (FilterPermissionDefinition.IdFilter.NotEqual != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.Add(FilterPermissionDefinition.IdFilter.NotEqual.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.NotIn != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.AddRange(FilterPermissionDefinition.IdFilter.NotIn);
                        }
                    }
                    if (FilterPermissionDefinition.Name == nameof(CurrentContext.UserId) && FilterPermissionDefinition.IdFilter != null)
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.IS.Id)
                        {
                            if (In == null) In = new List<long>();
                            In.Add(CurrentContext.UserId);
                        }
                        if (FilterPermissionDefinition.IdFilter.Equal.HasValue && FilterPermissionDefinition.IdFilter.Equal.Value == CurrentUserEnum.ISNT.Id)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.Add(CurrentContext.UserId);
                        }
                    }
                }
            }

            AppUserFilter AppUserFilter = new AppUserFilter
            {
                Id = new IdFilter { In = In, NotIn = NotIn, },
                OrganizationId = new IdFilter { In = organizationIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id,
                OrderBy = AppUserOrder.DisplayName,
                OrderType = OrderType.ASC,
            };

            List<AppUser> AppUsers = await AppUserService.List(AppUserFilter);
            List<long> AppUserIds = AppUsers.Select(a => a.Id).ToList();

            return AppUserIds;
        }

        protected async Task<List<long>> FilterStore(IStoreService StoreService, IAppUserService AppUserService, IOrganizationService OrganizationService, ICurrentContext CurrentContext)
        {
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return new List<long>();
            List<long> organizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<long> appUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            List<long> In = null;
            List<long> NotIn = null;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "StoreId")
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal != null)
                        {
                            if (In == null) In = new List<long>();
                            In.Add(FilterPermissionDefinition.IdFilter.Equal.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.In != null)
                        {
                            if (In == null) In = new List<long>();
                            In.AddRange(FilterPermissionDefinition.IdFilter.In);
                        }

                        if (FilterPermissionDefinition.IdFilter.NotEqual != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.Add(FilterPermissionDefinition.IdFilter.NotEqual.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.NotIn != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.AddRange(FilterPermissionDefinition.IdFilter.NotIn);
                        }
                    }
                }
            }

            int AppUserInDbCount = await AppUserService.Count(new AppUserFilter
            {
                OrganizationId = new IdFilter { In = organizationIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = AppUserSelect.Id,
                OrderBy = AppUserOrder.DisplayName,
                OrderType = OrderType.ASC,
            });
            IdFilter AppUserIdFilter = new IdFilter();
            if (appUserIds.Count < AppUserInDbCount) //khi tập quyền appuser < tổng số lượng appuser => đã được gán phân quyền dữ liệu theo appuser
            {
                AppUserIdFilter.In = appUserIds;
            }
            List<long> StoreIds = await StoreService.ListToIds(new StoreFilter
            {
                Id = new IdFilter { In = In, NotIn = NotIn },
                OrganizationId = new IdFilter { In = organizationIds },
                AppUserId = AppUserIdFilter,
                Skip = 0,
                Take = 1000000,
                Selects = StoreSelect.Id,
                //NoCache = true,
            });
            return StoreIds;
        }

        protected async Task<List<long>> FilterStoreScouting(IStoreScoutingService StoreScoutingService, IOrganizationService OrganizationService, ICurrentContext CurrentContext)
        {
            if (CurrentContext.Filters == null || CurrentContext.Filters.Count == 0) return new List<long>();
            List<long> organizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<long> In = null;
            List<long> NotIn = null;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "StoreScoutingId")
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal != null)
                        {
                            if (In == null) In = new List<long>();
                            In.Add(FilterPermissionDefinition.IdFilter.Equal.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.In != null)
                        {
                            if (In == null) In = new List<long>();
                            In.AddRange(FilterPermissionDefinition.IdFilter.In);
                        }

                        if (FilterPermissionDefinition.IdFilter.NotEqual != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.Add(FilterPermissionDefinition.IdFilter.NotEqual.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.NotIn != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.AddRange(FilterPermissionDefinition.IdFilter.NotIn);
                        }
                    }
                }
            }

            List<StoreScouting> StoreScoutings = await StoreScoutingService.List(new StoreScoutingFilter
            {
                Id = new IdFilter { In = In, NotIn = NotIn },
                OrganizationId = new IdFilter { In = organizationIds },
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreScoutingSelect.Id,
            });
            List<long> StoreScoutingIds = StoreScoutings.Select(a => a.Id).ToList();

            return StoreScoutingIds;
        }

        protected async Task<List<long>> FilterStoreGrouping(IStoreGroupingService StoreGroupingService, ICurrentContext CurrentContext)
        {
            List<long> In = null;
            List<long> NotIn = null;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "StoreGroupingId")
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal != null)
                        {
                            if (In == null) In = new List<long>();
                            In.Add(FilterPermissionDefinition.IdFilter.Equal.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.In != null)
                        {
                            if (In == null) In = new List<long>();
                            In.AddRange(FilterPermissionDefinition.IdFilter.In);
                        }

                        if (FilterPermissionDefinition.IdFilter.NotEqual != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.Add(FilterPermissionDefinition.IdFilter.NotEqual.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.NotIn != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.AddRange(FilterPermissionDefinition.IdFilter.NotIn);
                        }
                    }
                }
            }

            List<StoreGrouping> StoreGroupings = await StoreGroupingService.List(new StoreGroupingFilter
            {
                Id = new IdFilter { In = In, NotIn = NotIn },
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreGroupingSelect.Id,
            });
            List<long> StoreGroupingIds = StoreGroupings.Select(a => a.Id).ToList();

            return StoreGroupingIds;
        }

        protected async Task<List<long>> FilterStoreType(IStoreTypeService StoreTypeService, ICurrentContext CurrentContext)
        {
            List<long> In = null;
            List<long> NotIn = null;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "StoreTypeId")
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal != null)
                        {
                            if (In == null) In = new List<long>();
                            In.Add(FilterPermissionDefinition.IdFilter.Equal.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.In != null)
                        {
                            if (In == null) In = new List<long>();
                            In.AddRange(FilterPermissionDefinition.IdFilter.In);
                        }

                        if (FilterPermissionDefinition.IdFilter.NotEqual != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.Add(FilterPermissionDefinition.IdFilter.NotEqual.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.NotIn != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.AddRange(FilterPermissionDefinition.IdFilter.NotIn);
                        }
                    }
                }
            }

            List<StoreType> StoreTypes = await StoreTypeService.List(new StoreTypeFilter
            {
                Id = new IdFilter { In = In, NotIn = NotIn },
                Skip = 0,
                Take = int.MaxValue,
                Selects = StoreTypeSelect.Id,
            });
            List<long> StoreTypeIds = StoreTypes.Select(a => a.Id).ToList();

            return StoreTypeIds;
        }

        protected async Task<List<long>> FilterProductGrouping(IProductGroupingService ProductGroupingService, ICurrentContext CurrentContext)
        {
            List<long> In = null;
            List<long> NotIn = null;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "ProductGroupingId")
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal != null)
                        {
                            if (In == null) In = new List<long>();
                            In.Add(FilterPermissionDefinition.IdFilter.Equal.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.In != null)
                        {
                            if (In == null) In = new List<long>();
                            In.AddRange(FilterPermissionDefinition.IdFilter.In);
                        }

                        if (FilterPermissionDefinition.IdFilter.NotEqual != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.Add(FilterPermissionDefinition.IdFilter.NotEqual.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.NotIn != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.AddRange(FilterPermissionDefinition.IdFilter.NotIn);
                        }
                    }
                }
            }

            List<ProductGrouping> ProductGroupings = await ProductGroupingService.List(new ProductGroupingFilter
            {
                Id = new IdFilter { In = In, NotIn = NotIn },
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductGroupingSelect.Id,
            });
            List<long> ProductGroupingIds = ProductGroupings.Select(a => a.Id).ToList();

            return ProductGroupingIds;
        }

        protected async Task<List<long>> FilterProductType(IProductTypeService ProductTypeService, ICurrentContext CurrentContext)
        {
            List<long> In = null;
            List<long> NotIn = null;
            foreach (var currentFilter in CurrentContext.Filters)
            {
                List<FilterPermissionDefinition> FilterPermissionDefinitions = currentFilter.Value;
                foreach (FilterPermissionDefinition FilterPermissionDefinition in FilterPermissionDefinitions)
                {
                    if (FilterPermissionDefinition.Name == "ProductTypeId")
                    {
                        if (FilterPermissionDefinition.IdFilter.Equal != null)
                        {
                            if (In == null) In = new List<long>();
                            In.Add(FilterPermissionDefinition.IdFilter.Equal.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.In != null)
                        {
                            if (In == null) In = new List<long>();
                            In.AddRange(FilterPermissionDefinition.IdFilter.In);
                        }

                        if (FilterPermissionDefinition.IdFilter.NotEqual != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.Add(FilterPermissionDefinition.IdFilter.NotEqual.Value);
                        }
                        if (FilterPermissionDefinition.IdFilter.NotIn != null)
                        {
                            if (NotIn == null) NotIn = new List<long>();
                            NotIn.AddRange(FilterPermissionDefinition.IdFilter.NotIn);
                        }
                    }
                }
            }

            List<ProductType> ProductTypes = await ProductTypeService.List(new ProductTypeFilter
            {
                Id = new IdFilter { In = In, NotIn = NotIn },
                Skip = 0,
                Take = int.MaxValue,
                Selects = ProductTypeSelect.Id,
            });
            List<long> ProductTypeIds = ProductTypes.Select(a => a.Id).ToList();

            return ProductTypeIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="OrganizationId"></param>
        /// <param name="AppUserService"></param>
        /// <param name="OrganizationService"></param>
        /// <param name="CurrentContext"></param>
        /// <param name="DataContext"></param>
        /// <returns> AppUserIds, OrganizationIds </returns>
        protected async Task<(List<long>, List<long>)> FilterOrganizationAndUser(IdFilter OrganizationId, IAppUserService AppUserService, IOrganizationService OrganizationService, ICurrentContext CurrentContext, DataContext DataContext)
        {
            List<long> OrganizationIds = await FilterOrganization(OrganizationService, CurrentContext);
            List<OrganizationDAO> OrganizationDAOs = await DataContext.Organization.Where(o => o.DeletedAt == null && (OrganizationIds.Count == 0 || OrganizationIds.Contains(o.Id))).ToListWithNoLockAsync();
            OrganizationDAO OrganizationDAO = null;
            if (OrganizationId?.Equal != null)
            {
                OrganizationDAO = await DataContext.Organization.Where(o => o.Id == OrganizationId.Equal.Value).FirstOrDefaultWithNoLockAsync();
                OrganizationDAOs = OrganizationDAOs.Where(o => o.Path.StartsWith(OrganizationDAO.Path)).ToList();
            }
            OrganizationIds = OrganizationDAOs.Select(o => o.Id).ToList();
            List<long> AppUserIds = await FilterAppUser(AppUserService, OrganizationService, CurrentContext);
            return (AppUserIds, OrganizationIds);
        }

        protected DateTime LocalStartDay(ICurrentContext CurrentContext)
        {
            DateTime Start = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
            return Start;
        }

        protected DateTime LocalEndDay(ICurrentContext CurrentContext)
        {
            DateTime End = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone).AddDays(1).AddSeconds(-1);
            return End;
        }
    }

    [Authorize]
    [Authorize(Policy = "Simple")]
    public class SimpleController : ControllerBase
    {
        protected DateTime LocalStartDay(ICurrentContext CurrentContext)
        {
            DateTime Start = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone);
            return Start;
        }

        protected DateTime LocalEndDay(ICurrentContext CurrentContext)
        {
            DateTime End = StaticParams.DateTimeNow.AddHours(CurrentContext.TimeZone).Date.AddHours(0 - CurrentContext.TimeZone).AddDays(1).AddSeconds(-1);
            return End;
        }
    }

    public class PermissionRequirement : IAuthorizationRequirement
    {
        public PermissionRequirement()
        {
        }
    }
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        private ICurrentContext CurrentContext;
        
        private readonly IHttpContextAccessor httpContextAccessor;
        private IUOW UOW;
        private DataContext DataContext;
        public PermissionHandler(
            ICurrentContext CurrentContext,
            IUOW UOW,
            DataContext DataContext,
            IHttpContextAccessor httpContextAccessor)
        {
            this.CurrentContext = CurrentContext;
            this.UOW = UOW;
            this.DataContext = DataContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var types = context.User.Claims.Select(c => c.Type).ToList();
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                context.Fail();
                return;
            }
            long UserId = long.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value, out long u) ? u : 0;
            Guid UserRowId = Guid.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.PrimarySid).Value, out Guid rowId) ? rowId : Guid.Empty;
            string UserName = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
            var HttpContext = httpContextAccessor.HttpContext;
            string url = HttpContext.Request.Path.Value.ToLower().Substring(1);
            string TimeZone = HttpContext.Request.Headers["X-TimeZone"];
            string Language = HttpContext.Request.Headers["X-Language"];
            CurrentContext.Token = HttpContext.Request.Cookies["Token"];
            CurrentContext.UserId = UserId;
            CurrentContext.UserRowId = UserRowId;
            CurrentContext.TimeZone = int.TryParse(TimeZone, out int t) ? t : 0;
            CurrentContext.Language = Language ?? "vi";
            context.Succeed(requirement);

            List<CurrentPermission> CurrentPermissions = await UOW.PermissionRepository.ListByUserAndPath(UserId, url);

            if (CurrentPermissions.Count == 0)
            {
                context.Fail();
                return;
            }
            CurrentContext.RoleIds = CurrentPermissions.Select(p => p.RoleId).Distinct().ToList();
            CurrentContext.Filters = new Dictionary<long, List<FilterPermissionDefinition>>();
            foreach (CurrentPermission CurrentPermission in CurrentPermissions)
            {
                List<FilterPermissionDefinition> FilterPermissionDefinitions = new List<FilterPermissionDefinition>();
                CurrentContext.Filters.Add(CurrentPermission.Id, FilterPermissionDefinitions);
                foreach (CurrentPermissionContent CurrentPermissionContent in CurrentPermission.CurrentPermissionContents)
                {
                    FilterPermissionDefinition FilterPermissionDefinition = FilterPermissionDefinitions.Where(f => f.Name == CurrentPermissionContent.FieldName).FirstOrDefault();
                    if (FilterPermissionDefinition == null)
                    {
                        FilterPermissionDefinition = new FilterPermissionDefinition(CurrentPermissionContent.FieldName);
                        FilterPermissionDefinitions.Add(FilterPermissionDefinition);
                    }
                    FilterPermissionDefinition.SetValue(CurrentPermissionContent.FieldTypeId, CurrentPermissionContent.PermissionOperatorId, CurrentPermissionContent.Value);
                }
            }
            context.Succeed(requirement);
        }
    }

    public class SimpleRequirement : IAuthorizationRequirement
    {
        public SimpleRequirement()
        {
        }
    }
    public class SimpleHandler : AuthorizationHandler<SimpleRequirement>
    {
        private ICurrentContext CurrentContext;
        private IUOW UOW;
        private readonly IHttpContextAccessor httpContextAccessor;
        public SimpleHandler(ICurrentContext CurrentContext, IUOW UOW, IHttpContextAccessor httpContextAccessor)
        {
            this.CurrentContext = CurrentContext;
            this.UOW = UOW;
            this.httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, SimpleRequirement requirement)
        {
            var types = context.User.Claims.Select(c => c.Type).ToList();
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.NameIdentifier))
            {
                context.Fail();
                return;
            }
            long UserId = long.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value, out long u) ? u : 0;
            Guid UserRowId = Guid.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.PrimarySid).Value, out Guid rowId) ? rowId : Guid.Empty;
            string UserName = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
            var HttpContext = httpContextAccessor.HttpContext;
            string url = HttpContext.Request.Path.Value.ToLower().Substring(1);
            string TimeZone = HttpContext.Request.Headers["X-TimeZone"];
            string Language = HttpContext.Request.Headers["X-Language"];
            string Latitude = HttpContext.Request.Headers["X-Latitude"];
            string Longitude = HttpContext.Request.Headers["X-Longitude"];
            CurrentContext.Token = HttpContext.Request.Cookies["Token"];
            CurrentContext.UserId = UserId;
            CurrentContext.UserRowId = UserRowId;
            CurrentContext.TimeZone = int.TryParse(TimeZone, out int t) ? t : 0;
            CurrentContext.Language = Language ?? "vi";
            if (decimal.TryParse(Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal lat))
                CurrentContext.Latitude = lat;
            if (decimal.TryParse(Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal lon))
                CurrentContext.Longitude = lon;
            CurrentContext.RoleIds = await UOW.RoleRepository.ListIds(UserId);
            context.Succeed(requirement);
        }
    }
}
