using Model.DB2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_VANBAN.Models
{
    public class LAP_LICH
    {
        public List<DONVI> donvi { get; set; }
        public List<NHANVIEN> nhanvien { get; set; }
        public LICHLAMVIEC lichlamviec { get; set; }
    }
}