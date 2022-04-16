using System;
using TrueSight.Common;

namespace DMS.Entities.Report
{
    public class POSMTransaction : DataEntity, IEquatable<POSMTransaction>
    {
        public long Id { get; set; }
        public long? ShowingOrderId { get; set; }
        public long? ShowingOrderWithDrawId { get; set; }
        public long OrganizationId { get; set; }
        public long AppUserId { get; set; }
        public long StoreId { get; set; }
        public long ShowingItemId { get; set; }
        public long Quantity { get; set; }
        public decimal SalePrice { get; set; }
        public long UnitOfMeasureId { get; set; }
        public long TransactionTypeId { get; set; }
        public DateTime Date { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public decimal Amount { get; set; }

        public bool Equals(POSMTransaction other)
        {
            return other != null && Id == other.Id;
        }
    }
}
