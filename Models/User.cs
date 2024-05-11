using System;
using System.Collections.Generic;

namespace chic_lighting.Models
{
    public partial class User
    {
        public User()
        {
            Carts = new HashSet<Cart>();
            Orders = new HashSet<Order>();
            Rates = new HashSet<Rate>();
        }

        public long Id { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public long? RoleId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string? Address { get; set; }
        public string? VerifyCode { get; set; }
        public DateTime? VerifyAt { get; set; }
        public bool? IsActive { get; set; }
        public DateTime? CreateAt { get; set; }

        public virtual Role? Role { get; set; }
        public virtual ICollection<Cart> Carts { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Rate> Rates { get; set; }
    }
}
