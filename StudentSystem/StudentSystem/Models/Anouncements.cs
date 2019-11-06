using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentSystem.Models
{
    public class Anouncements
    {
        public int Id { get; set; }
        public string AnounceName { get; set; }
        public DateTime AnounceDate { get; set; }
        public string AnounceDescription { get; set; }
        public string ShareWith { get; set; }
    }
}