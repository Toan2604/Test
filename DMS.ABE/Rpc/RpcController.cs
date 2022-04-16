using DMS.ABE.Common; using TrueSight.Common;
using DMS.ABE.Entities;
using DMS.ABE.Enums;
using DMS.ABE.Helpers;
using DMS.ABE.Models;
using DMS.ABE.Services.MAppUser;
using DMS.ABE.Services.MOrganization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DMS.ABE.Rpc
{
    public class Root
    {
        protected const string Module = "dms-abe";
        protected const string WebModule = "order-hub-external";
        protected const string DMSModule = "dms";
        protected const string Rpc = "rpc/";
        protected const string Rest = "rest/";
    }
    [Authorize]
    [Authorize(Policy = "Permission")]
    public class RpcController : ControllerBase
    {
    }

    [Authorize]
    [Authorize(Policy = "RpcSimple")]
    public class RpcSimpleController : ControllerBase
    {
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
        private DataContext DataContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        public PermissionHandler(ICurrentContext CurrentContext, DataContext DataContext, IHttpContextAccessor httpContextAccessor)
        {
            this.CurrentContext = CurrentContext;
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
            List<long> permissionIds = await (from aurm in DataContext.AppUserRoleMapping.Where(x => x.AppUserId == UserId)
                                              join r in DataContext.Role on aurm.RoleId equals r.Id
                                              join per in DataContext.Permission on aurm.RoleId equals per.RoleId
                                              join pam in DataContext.PermissionActionMapping on per.Id equals pam.PermissionId
                                              join apm in DataContext.ActionPageMapping on pam.ActionId equals apm.ActionId
                                              join page in DataContext.Page on apm.PageId equals page.Id
                                              where r.StatusId == StatusEnum.ACTIVE.Id && per.StatusId == StatusEnum.ACTIVE.Id &&
                                              page.Path == url
                                              select per.Id
                                               ).Distinct().ToListAsync();

            if (permissionIds.Count == 0)
            {
                context.Fail();
                return;
            }
            List<PermissionDAO> PermissionDAOs = await DataContext.Permission.AsNoTracking()
                .Include(p => p.PermissionContents).ThenInclude(pf => pf.Field)
                .Where(p => permissionIds.Contains(p.Id))
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
            context.Succeed(requirement);
        }

    }

    public class RpcSimpleRequirement : IAuthorizationRequirement
    {
        public RpcSimpleRequirement()
        {
        }
    }
    public class RpcSimpleHandler : AuthorizationHandler<RpcSimpleRequirement>
    {
        private ICurrentContext CurrentContext;
        private DataContext DataContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        public RpcSimpleHandler(ICurrentContext CurrentContext, DataContext DataContext, IHttpContextAccessor httpContextAccessor)
        {
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
            this.httpContextAccessor = httpContextAccessor;
        }
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RpcSimpleRequirement requirement)
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
        private DataContext DataContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        public SimpleHandler(ICurrentContext CurrentContext, DataContext DataContext, IHttpContextAccessor httpContextAccessor)
        {
            this.CurrentContext = CurrentContext;
            this.DataContext = DataContext;
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
            long StoreUserId = long.TryParse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value, out long u) ? u : 0;
            string UserName = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
            var HttpContext = httpContextAccessor.HttpContext;
            string url = HttpContext.Request.Path.Value.ToLower().Substring(1);
            string TimeZone = HttpContext.Request.Headers["X-TimeZone"];
            string Language = HttpContext.Request.Headers["X-Language"];
            string Latitude = HttpContext.Request.Headers["X-Latitude"];
            string Longitude = HttpContext.Request.Headers["X-Longitude"];
            CurrentContext.Token = HttpContext.Request.Cookies["Token"];
            CurrentContext.StoreUserId = StoreUserId;
            CurrentContext.TimeZone = int.TryParse(TimeZone, out int t) ? t : 0;
            CurrentContext.Language = Language ?? "vi";
            if (decimal.TryParse(Latitude, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal lat))
                CurrentContext.Latitude = lat;
            if (decimal.TryParse(Longitude, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal lon))
                CurrentContext.Longitude = lon;
            context.Succeed(requirement);
        }

    }
}
