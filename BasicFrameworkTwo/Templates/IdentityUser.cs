using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicFrameworkTwo.Templates
{
    public class IdentityUser
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public bool ForcePasswordReset { get; set; }
        public DateTime? LastLoggedIn { get; set; }
        public DateTime? PasswordExpireUtc { get; set; }
    }
}
