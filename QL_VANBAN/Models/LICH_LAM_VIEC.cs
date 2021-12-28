using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace QL_VANBAN.Models
{
    public class LICH_LAM_VIEC
    {
        public decimal ID_LICH_LAM_VIEC { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        public DateTime NGAY_BAT_DAU { get; set; }
        
        public TimeSpan GIO_BAT_DAU { get; set; }
        
        public DateTime NGAY_KET_THUC { get; set; }
        
        public TimeSpan GIO_KET_THUC { get; set; }
        
        public string NOI_DUNG { get; set; }
        
        public string THANH_PHAN { get; set; }
        
        public string NHAN_VIEN { get; set; }
        public string SessionName { get; set; }

    }
}