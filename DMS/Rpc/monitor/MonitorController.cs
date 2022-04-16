using DMS.Enums;
using DMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Rpc.monitor
{

    public class MonitorController : RpcController
    {
        protected int CountPlan(DateTime Date, long SalesEmployeeId, long OrganizationId, List<ERouteContentDAO> ERouteContentDAOs, List<AppUserStoreMappingDAO> AppUserStoreMappingDAOs, List<StoreDAO> Stores)
        {
            List<long> ListPlan = new List<long>();

            ERouteContentDAOs = ERouteContentDAOs.Where(ec => ec.ERoute.SaleEmployeeId == SalesEmployeeId &&
               ec.ERoute.RealStartDate <= Date &&
               (ec.ERoute.EndDate == null || ec.ERoute.EndDate.Value >= Date))
                .ToList();

            List<long> StoreInScopeIds = AppUserStoreMappingDAOs.Where(x => x.AppUserId == SalesEmployeeId)
                .Select(x => x.StoreId).ToList();
            Stores = Stores.Where(x => x.OrganizationId == OrganizationId && x.StatusId == StatusEnum.ACTIVE.Id).ToList();
            var StoreInOrgsIds = new List<long>();
            if (StoreInScopeIds != null && StoreInScopeIds.Count > 0)
            {
                StoreInOrgsIds = Stores.Where(x => x.StoreStatusId == StoreStatusEnum.DRAFT.Id).Select(x => x.Id).ToList();
                StoreInScopeIds.AddRange(StoreInOrgsIds);
            }
            else
                StoreInScopeIds = Stores.Select(x => x.Id).ToList();

            foreach (var ERouteContent in ERouteContentDAOs)
            {
                ERouteContent.RealStartDate = ERouteContent.ERoute.RealStartDate;
            }
            ERouteContentDAOs = ERouteContentDAOs.Distinct().ToList();
            foreach (var ERouteContent in ERouteContentDAOs)
            {
                var index = (Date - ERouteContent.ERoute.RealStartDate).Days % 28;
                if (index >= 0 && ERouteContent.ERouteContentDays.Count > index)
                {
                    if (ERouteContent.ERouteContentDays.ElementAt(index).Planned == true)
                        ListPlan.Add(ERouteContent.StoreId);
                }
            }
            ListPlan = ListPlan.Intersect(StoreInScopeIds).Distinct().ToList();
            return ListPlan.Count();
        }
    }
}
