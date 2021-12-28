namespace Model.DB2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LICHLAMVIEC")]
    public partial class LICHLAMVIEC
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LICHLAMVIEC()
        {
            THANH_PHAN_THAM_DU = new HashSet<THANH_PHAN_THAM_DU>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_LICH_LAM_VIEC { get; set; }

        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{dd/MM/yyyy}")]
        //[Column(TypeName = "date")]
        public DateTime NGAY_BAT_DAU { get; set; }

        [Required]
        public TimeSpan GIO_BAT_DAU { get; set; }

        [Column(TypeName = "date")]
        public DateTime NGAY_KET_THUC { get; set; }

        [Required]
        public TimeSpan GIO_KET_THUC { get; set; }

        [Required]
        [StringLength(200)]
        public string NOI_DUNG { get; set; }

        [StringLength(300)]
        public string THANH_PHAN { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<THANH_PHAN_THAM_DU> THANH_PHAN_THAM_DU { get; set; }
    }
}
