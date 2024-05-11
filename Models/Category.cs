using System;
using System.Collections.Generic;

namespace chic_lighting.Models
{
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public long Id { get; set; }
        public string? CategoryName { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public string? ModifyBy { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
