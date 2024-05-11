using System;
using System.Collections.Generic;

namespace chic_lighting.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Transactions = new HashSet<Transaction>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
