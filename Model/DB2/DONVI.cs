namespace Model.DB2
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DONVI")]
    public partial class DONVI
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DONVI()
        {
            NHANVIEN = new HashSet<NHANVIEN>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public decimal ID_DON_VI { get; set; }

        [Required]
        [StringLength(10)]
        public string MA_DON_VI { get; set; }

        [Required]
        [StringLength(30)]
        public string TEN_DON_VI { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NHANVIEN> NHANVIEN { get; set; }
    }
}
