using System;
using System.Collections.Generic;

namespace DMS.Models
{
    /// <summary>
    /// B&#7843;ng &#273;&#7883;nh ngh&#297;a chu k&#236; &#273;i tuy&#7871;n c&#7911;a t&#7915;ng tuy&#7871;n
    /// </summary>
    public partial class ERouteContentDayDAO
    {
        public long Id { get; set; }
        public long ERouteContentId { get; set; }
        public long OrderDay { get; set; }
        public bool Planned { get; set; }

        public virtual ERouteContentDAO ERouteContent { get; set; }
    }
}
