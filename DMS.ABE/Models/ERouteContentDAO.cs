using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    /// <summary>
    /// B&#7843;ng chi ti&#7871;t c&#7911;a thi&#7871;t l&#7853;p tuy&#7871;n
    /// </summary>
    public partial class ERouteContentDAO
    {
        public ERouteContentDAO()
        {
            ERouteContentDays = new HashSet<ERouteContentDayDAO>();
        }

        public long Id { get; set; }
        public long ERouteId { get; set; }
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
        public Guid RowId { get; set; }

        public virtual ERouteDAO ERoute { get; set; }
        public virtual StoreDAO Store { get; set; }
        public virtual ICollection<ERouteContentDayDAO> ERouteContentDays { get; set; }
    }
}
