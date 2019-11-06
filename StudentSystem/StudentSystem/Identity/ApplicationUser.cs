using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentSystem.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Parent { get; set; }
        public string Group { get; set; }
        public string Group1 { get; set; }
        public string Sinif { get; set; }
    }
}