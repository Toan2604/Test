using DMS.Entities;
using System.Collections.Generic;
using System.Linq;
using TrueSight.Common;

namespace DMS.Services
{
    public class BaseService
    {
        protected List<long> FilterOrganization(List<Organization> Organizations, IdFilter IdFilter)
        {
            List<long> In = null;
            List<long> NotIn = null;
            if (IdFilter.Equal != null)
            {
                if (In == null) In = new List<long>();
                In.Add(IdFilter.Equal.Value);
            }
            if (IdFilter.In != null)
            {
                if (In == null) In = new List<long>();
                In.AddRange(IdFilter.In);
            }

            if (IdFilter.NotEqual != null)
            {
                if (NotIn == null) NotIn = new List<long>();
                NotIn.Add(IdFilter.NotEqual.Value);
            }
            if (IdFilter.NotIn != null)
            {
                if (NotIn == null) NotIn = new List<long>();
                NotIn.AddRange(IdFilter.NotIn);
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
            return Organizations.Select(o => o.Id).ToList();
        }
    }
}
