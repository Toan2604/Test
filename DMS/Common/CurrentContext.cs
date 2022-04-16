using System;
using System.Collections.Generic;
using TrueSight.Common;

namespace DMS.Common
{
    public interface ICurrentContext : IServiceScoped
    {
        long UserId { get; set; }
        Guid UserRowId { get; set; }
        string UserName { get; set; }
        int TimeZone { get; set; }
        string Language { get; set; }
        string Token { get; set; }
        decimal? Latitude { get; set; }
        decimal? Longitude { get; set; }
        long StoreUserId { get; set; }
        List<long> RoleIds { get; set; }
        Dictionary<long, List<FilterPermissionDefinition>> Filters { get; set; }
    }
    public class CurrentContext : ICurrentContext
    {
        public long UserId { get; set; }
        public Guid UserRowId { get; set; }
        public string UserName { get; set; }
        public int TimeZone { get; set; }
        public string Language { get; set; } = "vi";
        public string Token { get; set; }
        public long StoreUserId { get; set; }
        public List<long> RoleIds { get; set; }
        public Dictionary<long, List<FilterPermissionDefinition>> Filters { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
    }
}
