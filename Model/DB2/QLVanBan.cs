namespace Model.DB2
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class QLVanBan : DbContext
    {
        public QLVanBan()
            : base("name=QLVanBan")
        {
        }

        public virtual DbSet<DONVI> DONVI { get; set; }
        public virtual DbSet<LICHLAMVIEC> LICHLAMVIEC { get; set; }
        public virtual DbSet<NHANVIEN> NHANVIEN { get; set; }
        public virtual DbSet<THANH_PHAN_THAM_DU> THANH_PHAN_THAM_DU { get; set; }
        public virtual DbSet<VAITRO> VAITRO { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DONVI>()
                .Property(e => e.ID_DON_VI)
                .HasPrecision(18, 0);

            modelBuilder.Entity<DONVI>()
                .Property(e => e.MA_DON_VI)
                .IsUnicode(false);

            modelBuilder.Entity<LICHLAMVIEC>()
                .Property(e => e.ID_LICH_LAM_VIEC)
                .HasPrecision(18, 0);

            modelBuilder.Entity<LICHLAMVIEC>()
                .HasMany(e => e.THANH_PHAN_THAM_DU)
                .WithRequired(e => e.LICHLAMVIEC)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.ID_NHAN_VIEN)
                .HasPrecision(18, 0);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.MA_NHAN_VIEN)
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.SDT)
                .HasPrecision(18, 0);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.USERNAME)
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.PASSWORD)
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.ID_DON_VI)
                .HasPrecision(18, 0);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.ID_VAI_TRO)
                .HasPrecision(18, 0);

            modelBuilder.Entity<NHANVIEN>()
                .HasMany(e => e.THANH_PHAN_THAM_DU)
                .WithRequired(e => e.NHANVIEN)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<THANH_PHAN_THAM_DU>()
                .Property(e => e.ID_LICH_LAM_VIEC)
                .HasPrecision(18, 0);

            modelBuilder.Entity<THANH_PHAN_THAM_DU>()
                .Property(e => e.ID_NHAN_VIEN)
                .HasPrecision(18, 0);

            modelBuilder.Entity<THANH_PHAN_THAM_DU>()
                .Property(e => e.MAC_DINH)
                .IsUnicode(false);

            modelBuilder.Entity<VAITRO>()
                .Property(e => e.ID_VAI_TRO)
                .HasPrecision(18, 0);
        }
    }
}
