using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentSystem.Models
{
    public class Attendances
    {
        public int Id { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string GroupName { get; set; }
        public bool IsJoin { get; set; }
        public DateTime Tarix { get; set; }
    }
}