using System;
using System.Collections.Generic;

namespace DMS.Models
{
    public partial class StoreBalanceDAO
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public long StoreId { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal DebitAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid RowId { get; set; }

        public virtual OrganizationDAO Organization { get; set; }
        public virtual StoreDAO Store { get; set; }
    }
}
