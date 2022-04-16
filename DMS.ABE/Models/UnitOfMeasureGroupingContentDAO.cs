using System;
using System.Collections.Generic;

namespace DMS.ABE.Models
{
    public partial class UnitOfMeasureGroupingContentDAO
    {
        public long Id { get; set; }
        public long UnitOfMeasureGroupingId { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long? Factor { get; set; }
        public Guid RowId { get; set; }

        public virtual UnitOfMeasureDAO UnitOfMeasure { get; set; }
        public virtual UnitOfMeasureGroupingDAO UnitOfMeasureGrouping { get; set; }
    }
}
