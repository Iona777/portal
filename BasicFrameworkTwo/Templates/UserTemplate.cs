using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using FileHelpers;

namespace BasicFrameworkTwo.Templates
{
    [DelimitedRecord(",")]
    public class UserTemplate
    {
        public string UserKey { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string AccountNum { get; set; }
        public string SiteRefNum { get; set; }
        public string Lastname { get; set; }
        public string NewPassword { get; set; }
        public string GroupAccountNum { get; set; }
        public string SiteRefNumElectricity { get; set; }
        public string BasketIds { get; set; }
    }
}