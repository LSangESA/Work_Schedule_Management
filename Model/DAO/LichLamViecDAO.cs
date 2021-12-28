using Model.DB2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class LichLamViecDAO
    {
        public QLVanBan db = null;

        public LichLamViecDAO()
        {
            db = new QLVanBan();
        }

        public string InsertLLV(LICHLAMVIEC llv)
        {
            // Xu ly su kien khong nhap ngay ket thuc
            if (llv.NGAY_KET_THUC.ToString() == "01/01/0001 12:00:00 SA")
            {
                llv.NGAY_KET_THUC = llv.NGAY_BAT_DAU;
            }

            // Tra ve loi ngay bat dau lon hon ngay ket thuc
            if (llv.NGAY_BAT_DAU > llv.NGAY_KET_THUC)
            {
                return "errorNgay";
            }

            // Tra ve loi gio bat dau lon hon gio ket thuc khi lich lam viec co hieu luc trong cung ngay
            if (llv.NGAY_BAT_DAU == llv.NGAY_KET_THUC)
            {
                if (llv.GIO_BAT_DAU > llv.GIO_KET_THUC)
                {
                    return "errorGio";
                }
            }

            // Tra ve loi lich lam viec duoc lap sau ngay hienb tai
            if (llv.NGAY_BAT_DAU < DateTime.Now || llv.NGAY_KET_THUC < DateTime.Now)
            {
                return "errorNgay2";
            }

            // Them lich lam viec vao CSDL
            db.LICHLAMVIEC.Add(llv);
            db.SaveChanges();

            return llv.ID_LICH_LAM_VIEC.ToString();
        }

        public bool InsertTPTD(decimal idLLV, string[] slNhanVien, string[] slDonVi)
        {
            List<string> listNVDonVi = new List<string>();
            if (slDonVi != null && slNhanVien != null)
            {
                // Lay ra danh sach nhan vien
                foreach (var itemDV in slDonVi.ToList())
                {
                    decimal idDV = Convert.ToDecimal(itemDV);
                    var getNVofDV = db.NHANVIEN.Where(x => x.ID_DON_VI == idDV).Select(x => x.ID_NHAN_VIEN).ToList();
                    foreach (var itemNV in getNVofDV.ToList())
                    {
                        foreach (var itemNV2 in slNhanVien.ToList())
                        {
                            if (itemNV2 != itemNV.ToString())
                            {
                                listNVDonVi.Add(itemNV.ToString());
                            }
                        }
                    }
                }

                foreach (var item1 in slNhanVien.ToList())
                {
                    foreach (var item2 in listNVDonVi.ToList())
                    {
                        if (item1 == item2)
                        {
                            listNVDonVi.Remove(item2);
                        }
                    }
                }

                foreach (var itemNVDV in listNVDonVi.Distinct())
                {
                    THANH_PHAN_THAM_DU tpdl = new THANH_PHAN_THAM_DU();
                    tpdl.ID_LICH_LAM_VIEC = idLLV;
                    tpdl.ID_NHAN_VIEN = Convert.ToDecimal(itemNVDV);
                    db.THANH_PHAN_THAM_DU.Add(tpdl);
                }

                foreach (var itemNV in slNhanVien)
                {
                    THANH_PHAN_THAM_DU tpdl = new THANH_PHAN_THAM_DU();
                    tpdl.ID_LICH_LAM_VIEC = idLLV;
                    tpdl.ID_NHAN_VIEN = Convert.ToDecimal(itemNV);
                    db.THANH_PHAN_THAM_DU.Add(tpdl);
                }
                db.SaveChanges();
                return true;
            }

            if (slDonVi != null)
            {
                foreach (var itemDV in slDonVi)
                {
                    decimal idDV = Convert.ToDecimal(itemDV);
                    var getNVofDV = db.NHANVIEN.Where(x => x.ID_DON_VI == idDV).ToList();
                    foreach (var itemNV in getNVofDV)
                    {
                        THANH_PHAN_THAM_DU tpdl = new THANH_PHAN_THAM_DU();
                        tpdl.ID_LICH_LAM_VIEC = idLLV;
                        tpdl.ID_NHAN_VIEN = Convert.ToDecimal(itemNV.ID_NHAN_VIEN);
                        db.THANH_PHAN_THAM_DU.Add(tpdl);
                    }
                }
                db.SaveChanges();
                return true;
            }

            if (slNhanVien != null)
            {
                foreach (var item in slNhanVien)
                {
                    THANH_PHAN_THAM_DU tpdl = new THANH_PHAN_THAM_DU();
                    tpdl.ID_NHAN_VIEN = Convert.ToDecimal(item);
                    tpdl.ID_LICH_LAM_VIEC = idLLV;

                    db.THANH_PHAN_THAM_DU.Add(tpdl);
                }
                db.SaveChanges();
                return true;
            }

            return false;
        }

        public bool DeleteLLV(decimal idLLV)
        {
            var getLLV = db.LICHLAMVIEC.Where(a => a.ID_LICH_LAM_VIEC == idLLV).FirstOrDefault();
            if (getLLV != null)
            {
                var getTPTD = db.THANH_PHAN_THAM_DU.Where(x => x.ID_LICH_LAM_VIEC == getLLV.ID_LICH_LAM_VIEC).ToList();
                foreach (var item in getTPTD)
                {
                    db.THANH_PHAN_THAM_DU.Remove(item);
                    db.SaveChanges();
                }
                db.LICHLAMVIEC.Remove(getLLV);
                db.SaveChanges();
            }
            return true;
        }

        public bool DeleteTPTD(decimal idLLV)
        {
            var listTPTD = db.THANH_PHAN_THAM_DU.Where(x => x.ID_LICH_LAM_VIEC == idLLV).ToList();
            foreach (var item in listTPTD)
            {
                db.THANH_PHAN_THAM_DU.Remove(item);
                db.SaveChanges();
            }
            return true;
        }

        public string UpdateLLV(LICHLAMVIEC llv)
        {
            var getLLV = db.LICHLAMVIEC.Where(x=>x.ID_LICH_LAM_VIEC == llv.ID_LICH_LAM_VIEC).SingleOrDefault();
            if(getLLV != null)
            {
                getLLV.GIO_BAT_DAU = llv.GIO_BAT_DAU;
                getLLV.NGAY_BAT_DAU = llv.NGAY_BAT_DAU;
                getLLV.GIO_KET_THUC = llv.GIO_KET_THUC;
                getLLV.NGAY_KET_THUC = llv.NGAY_KET_THUC;
                getLLV.NOI_DUNG = llv.NOI_DUNG;
                getLLV.THANH_PHAN = llv.THANH_PHAN;

                if (getLLV.NGAY_KET_THUC.ToString() == "01/01/0001 12:00:00 SA")
                {
                    getLLV.NGAY_KET_THUC = getLLV.NGAY_BAT_DAU;
                }

                if (getLLV.NGAY_BAT_DAU > getLLV.NGAY_KET_THUC)
                {
                    return "errorNgay";
                }

                if (getLLV.NGAY_BAT_DAU == getLLV.NGAY_KET_THUC)
                {
                    if (getLLV.GIO_BAT_DAU > getLLV.GIO_KET_THUC)
                    {
                        return "errorGio";
                    }
                }

                db.SaveChanges();
            }
                
            return getLLV.ID_LICH_LAM_VIEC.ToString();
        }
    }
}
