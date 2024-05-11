using System;
using System.Collections.Generic;

namespace chic_lighting.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            Transactions = new HashSet<Transaction>();
        }

        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? OrderStatusId { get; set; }
        public DateTime OrderDate { get; set; }
        public string? Notes { get; set; }
        public string? Firstname { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }

        public virtual OrderStatus? OrderStatus { get; set; }
        public virtual User? User { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
