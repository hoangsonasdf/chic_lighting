using System;
using System.Collections.Generic;

namespace chic_lighting.Models
{
    public partial class Product
    {
        public Product()
        {
            CartItems = new HashSet<CartItem>();
            OrderDetails = new HashSet<OrderDetail>();
            Rates = new HashSet<Rate>();
        }

        public long Id { get; set; }
        public string? ProductName { get; set; }
        public string? Title { get; set; }
        public double? Price { get; set; }
        public long? CategoryId { get; set; }
        public string? Description { get; set; }
        public int? Quantity { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public string? ModifyBy { get; set; }
        public string? Image { get; set; }
        public long? ProductStatusId { get; set; }
        public double? Saleprice { get; set; }

        public virtual Category? Category { get; set; }
        public virtual ProductStatus? ProductStatus { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}
