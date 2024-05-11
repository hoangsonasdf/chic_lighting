using System;
using System.Collections.Generic;

namespace chic_lighting.Models
{
    public partial class ProductStatus
    {
        public ProductStatus()
        {
            Products = new HashSet<Product>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
