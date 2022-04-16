using System;
using System.Collections.Generic;

namespace DMS.Models
{
    /// <summary>
    /// B&#7843;ng chi ti&#7871;t c&#7911;a m&#7897;t y&#234;u c&#7847;u thay &#273;&#7893;i tuy&#7871;n
    /// </summary>
    public partial class ERouteChangeRequestContentDAO
    {
        public long Id { get; set; }
        public long ERouteChangeRequestId { get; set; }
        public long StoreId { get; set; }
        public long? OrderNumber { get; set; }
        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }
        public bool Week1 { get; set; }
        public bool Week2 { get; set; }
        public bool Week3 { get; set; }
        public bool Week4 { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ERouteChangeRequestDAO ERouteChangeRequest { get; set; }
        public virtual StoreDAO Store { get; set; }
    }
}
