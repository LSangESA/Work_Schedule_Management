namespace Model.DB2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class THANH_PHAN_THAM_DU
    {
        [Key]
        [Column(Order = 0)]
        public decimal ID_LICH_LAM_VIEC { get; set; }

        [Key]
        [Column(Order = 1)]
        public decimal ID_NHAN_VIEN { get; set; }

        [StringLength(100)]
        public string MAC_DINH { get; set; }

        public virtual LICHLAMVIEC LICHLAMVIEC { get; set; }

        public virtual NHANVIEN NHANVIEN { get; set; }
    }
}
