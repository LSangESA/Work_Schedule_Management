using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QL_VANBAN.Models
{
    public class NHAN_VIEN
    {
        public decimal ID_NHAN_VIEN { get; set; }
        public string HO_TEN { get; set; }
        public decimal? ID_DON_VI { get; set; }
    }
}