namespace Model.DB2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NHANVIEN")]
    public partial class NHANVIEN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NHANVIEN()
        {
            THANH_PHAN_THAM_DU = new HashSet<THANH_PHAN_THAM_DU>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_NHAN_VIEN { get; set; }

        [Required]
        [StringLength(30)]
        public string MA_NHAN_VIEN { get; set; }

        [Required]
        [StringLength(50)]
        public string HO_TEN { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? SDT { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NGAY_SINH { get; set; }

        [StringLength(100)]
        public string DIA_CHI { get; set; }

        [StringLength(20)]
        public string USERNAME { get; set; }

        [StringLength(10)]
        public string PASSWORD { get; set; }

        public decimal? ID_DON_VI { get; set; }

        public decimal? ID_VAI_TRO { get; set; }

        public virtual DONVI DONVI { get; set; }

        public virtual VAITRO VAITRO { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THANH_PHAN_THAM_DU> THANH_PHAN_THAM_DU { get; set; }
    }
}
