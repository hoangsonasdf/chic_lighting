using System;
using System.Collections.Generic;

namespace chic_lighting.Models
{
    public partial class Feedback
    {
        public long Id { get; set; }
        public string? Comment { get; set; }
        public short? Rate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Email { get; set; }
        public string? Name { get; set; }
    }
}
