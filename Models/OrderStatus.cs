using System;
using System.Collections.Generic;

namespace chic_lighting.Models
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Order>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Bootstapicon { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
    }
}
