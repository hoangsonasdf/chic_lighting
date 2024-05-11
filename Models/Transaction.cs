using System;
using System.Collections.Generic;

namespace chic_lighting.Models
{
    public partial class Transaction
    {
        public long Id { get; set; }
        public long? OrderId { get; set; }
        public long? PaymentId { get; set; }
        public double? Total { get; set; }

        public virtual Order? Order { get; set; }
        public virtual Payment? Payment { get; set; }
    }
}
