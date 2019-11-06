using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentSystem.Models
{
    public class ExamUserResult
    {
        public int Id { get; set; }
        public string QrupAdi { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public int DuzgunSualSayi { get; set; }
        public decimal Faiz { get; set; }
        public string ImtahanAdi { get; set; }
        public string Kateqoriya { get; set; }
        public int CemiSualSayi { get; set; }
        public DateTime Tarix { get; set; }

        public string Sinif { get; set; }

    }

    public class Exam
    {
        public int Id { get; set; }
        public string ImtahanAdi { get; set; }
        public DateTime Tarix { get; set; }
        public string Kateqoriya { get; set; }
        public int CemiSualSayi { get; set; }
    }

}