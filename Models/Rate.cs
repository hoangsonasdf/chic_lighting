using System;
using System.Collections.Generic;

namespace chic_lighting.Models
{
    public partial class Rate
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? ProductId { get; set; }
        public short? Star { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual Product? Product { get; set; }
        public virtual User? User { get; set; }
    }
}
