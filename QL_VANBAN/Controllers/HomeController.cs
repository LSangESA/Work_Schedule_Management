using Model.DAO;
using Model.DB2;
using QL_VANBAN.Common;
using QL_VANBAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace QL_VANBAN.Controllers
{
    public class HomeController : BaseController
    {
        public static string slDVView { get; set; }
        [HttpGet]
        public ActionResult Index(string selectList)
        {
            using (QLVanBan db = new QLVanBan())
            {
                var nhanvien = db.NHANVIEN.ToList();
                var donvi = db.DONVI.ToList();

                LAP_LICH myModel = new LAP_LICH();

                List<NHAN_VIEN> listNhanVien = new List<NHAN_VIEN>();
                foreach(var item in nhanvien.OrderBy(x=>x.ID_DON_VI).ToList())
                {
                    NHAN_VIEN nhanVien = new NHAN_VIEN();

                    nhanVien.ID_NHAN_VIEN = item.ID_NHAN_VIEN;
                    nhanVien.HO_TEN = item.HO_TEN + " (" + item.DONVI.TEN_DON_VI + ")";

                    listNhanVien.Add(nhanVien);
                }
                
                ViewBag.VBNhanVien = new SelectList(listNhanVien, "ID_NHAN_VIEN", "HO_TEN");
                ViewBag.AddNV = new SelectList(db.NHANVIEN.Where(x => x.ID_NHAN_VIEN == -1).ToList(), "ID_NHAN_VIEN", "HO_TEN");
                ViewBag.VBDonVi = new SelectList(donvi, "ID_DON_VI", "TEN_DON_VI");
                ViewBag.AddDV = new SelectList(db.DONVI.Where(x => x.ID_DON_VI == -1).ToList(), "ID_DON_VI", "TEN_DON_VI");
                ViewBag.slFind = new SelectList(db.DONVI.ToList(), "ID_DON_VI", "TEN_DON_VI");

                if (!string.IsNullOrEmpty(selectList))
                {
                    slDVView = selectList;
                }
                else
                {
                    slDVView = null;
                }
                return View(myModel);
            }
        }

        [HttpPost]
        public ActionResult Index(LAP_LICH llv, string[] slNhanVien, string[] slDonVi)
        {
            // Them moi doi tuong lich lam viec
            var resultLLV = new LichLamViecDAO().InsertLLV(llv.lichlamviec);

            if (!string.IsNullOrEmpty(resultLLV))
            {
                // Xy ly loi
                switch (resultLLV)
                {
                    case "errorNgay":
                        SetAlert("Thêm mới thất bại. Ngày bắt đầu phải nhỏ hơn ngày kết thúc", "danger");
                        return RedirectToAction("Index");

                    case "errorGio":
                        SetAlert("Thêm mới thất bại. Giờ làm việc trong ngày không hợp lệ", "danger");
                        return RedirectToAction("Index");

                    case "errorNgay2":
                        SetAlert("Thêm mới thất bại. Ngày làm việc không hợp lệ", "danger");
                        return RedirectToAction("Index");
                }

                // Them moi thanh phan tham du neu co
                decimal idLLV = Convert.ToDecimal(resultLLV);
                var resultTPTD = new LichLamViecDAO().InsertTPTD(idLLV, slNhanVien, slDonVi);

                if(resultTPTD)
                {
                    SetAlert("Thêm thành công lịch làm việc", "success");
                    return RedirectToAction("Index");
                }
            }
            SetAlert("Thêm thành công lịch làm việc", "success");
            return RedirectToAction("Index");
                
        }

        public JsonResult GetEvents()
        {
            var events = new object();
            using (QLVanBan db = new QLVanBan())
            {
                List<LICH_LAM_VIEC> llvList = new List<LICH_LAM_VIEC>();

                if (!string.IsNullOrEmpty(slDVView))
                {
                    decimal slDV = Convert.ToDecimal(slDVView);
                    var getLich = db.THANH_PHAN_THAM_DU
                        .Where(x => x.NHANVIEN.DONVI.ID_DON_VI == slDV)
                        .GroupBy(x => new
                        {
                            x.LICHLAMVIEC.ID_LICH_LAM_VIEC,
                            x.LICHLAMVIEC.NGAY_BAT_DAU,
                            x.LICHLAMVIEC.GIO_BAT_DAU,
                            x.LICHLAMVIEC.NGAY_KET_THUC,
                            x.LICHLAMVIEC.GIO_KET_THUC,
                            x.LICHLAMVIEC.NOI_DUNG,
                            x.LICHLAMVIEC.THANH_PHAN
                        })
                        .Select(x => new
                        {
                            x.Key.ID_LICH_LAM_VIEC,
                            x.Key.NGAY_BAT_DAU,
                            x.Key.GIO_BAT_DAU,
                            x.Key.NGAY_KET_THUC,
                            x.Key.GIO_KET_THUC,
                            x.Key.NOI_DUNG,
                            x.Key.THANH_PHAN
                        }).OrderBy(x => x.GIO_KET_THUC).ToList();

                    foreach (var item in getLich)
                    {
                        LICH_LAM_VIEC llv = new LICH_LAM_VIEC();
                        llv.ID_LICH_LAM_VIEC = item.ID_LICH_LAM_VIEC;
                        llv.GIO_BAT_DAU = item.GIO_BAT_DAU;
                        llv.NGAY_BAT_DAU = item.NGAY_BAT_DAU;
                        llv.GIO_KET_THUC = item.GIO_KET_THUC;
                        llv.NGAY_KET_THUC = item.NGAY_KET_THUC;
                        llv.NOI_DUNG = item.NOI_DUNG;
                        llv.THANH_PHAN = item.THANH_PHAN;

                        // Lấy ra ds đơn vị thuộc lịch làm việc
                        List<NHANVIEN_DONVI> listCalendarDV = new List<NHANVIEN_DONVI>();
                        var listNV = db.THANH_PHAN_THAM_DU.Where(x => x.LICHLAMVIEC.ID_LICH_LAM_VIEC == item.ID_LICH_LAM_VIEC).ToList();
                        foreach (var itemNV in listNV)
                        {
                            NHANVIEN_DONVI dv = new NHANVIEN_DONVI();
                            dv.ID_DV = itemNV.NHANVIEN.ID_DON_VI;
                            dv.TEN_DV = itemNV.NHANVIEN.DONVI.TEN_DON_VI;
                            listCalendarDV.Add(dv);
                        }

                        // Lấy ra những nhân viên thuộc đơn vị của lịch và đưa ra view
                        var listDVGroup = listCalendarDV.GroupBy(x => new { x.ID_DV, x.TEN_DV }).Select(x => new { x.Key.ID_DV, x.Key.TEN_DV }).ToList();
                        for (int i = 0; i < listDVGroup.Count(); i++)
                        {
                            llv.NHAN_VIEN += listDVGroup[i].TEN_DV.ToString() + " (";
                            decimal? idDV = listDVGroup[i].ID_DV;
                            var getNV_TPTD = db.THANH_PHAN_THAM_DU.Where(x => x.NHANVIEN.DONVI.ID_DON_VI == idDV && x.ID_LICH_LAM_VIEC == item.ID_LICH_LAM_VIEC).ToList();
                            for (int j = 0; j < getNV_TPTD.Count(); j++)
                            {
                                if ((j == getNV_TPTD.Count() - 1) && (i == listDVGroup.Count() - 1))
                                {
                                    llv.NHAN_VIEN += getNV_TPTD[j].NHANVIEN.HO_TEN.ToString() + ").";
                                }
                                else if (j == getNV_TPTD.Count() - 1)
                                {
                                    llv.NHAN_VIEN += getNV_TPTD[j].NHANVIEN.HO_TEN.ToString() + ") - ";
                                }
                                else
                                {
                                    llv.NHAN_VIEN += getNV_TPTD[j].NHANVIEN.HO_TEN.ToString() + ", ";
                                }
                            }
                        }
                        llvList.Add(llv);

                    }
                }
                else
                {
                    var getLich = db.LICHLAMVIEC
                        .GroupBy(x => new
                        {
                            x.ID_LICH_LAM_VIEC,
                            x.NGAY_BAT_DAU,
                            x.GIO_BAT_DAU,
                            x.NGAY_KET_THUC,
                            x.GIO_KET_THUC,
                            x.NOI_DUNG,
                            x.THANH_PHAN
                        })
                        .Select(x => new
                        {
                            x.Key.ID_LICH_LAM_VIEC,
                            x.Key.NGAY_BAT_DAU,
                            x.Key.GIO_BAT_DAU,
                            x.Key.NGAY_KET_THUC,
                            x.Key.GIO_KET_THUC,
                            x.Key.NOI_DUNG,
                            x.Key.THANH_PHAN
                        }).OrderBy(x => x.GIO_KET_THUC).ToList();

                    foreach (var item in getLich)
                    {
                        LICH_LAM_VIEC llv = new LICH_LAM_VIEC();
                        llv.ID_LICH_LAM_VIEC = item.ID_LICH_LAM_VIEC;
                        llv.GIO_BAT_DAU = item.GIO_BAT_DAU;
                        llv.NGAY_BAT_DAU = item.NGAY_BAT_DAU;
                        llv.GIO_KET_THUC = item.GIO_KET_THUC;
                        llv.NGAY_KET_THUC = item.NGAY_KET_THUC;
                        llv.NOI_DUNG = item.NOI_DUNG;
                        llv.THANH_PHAN = item.THANH_PHAN;

                        // Lấy ra ds đơn vị thuộc lịch làm việc
                        List<NHANVIEN_DONVI> listCalendarDV = new List<NHANVIEN_DONVI>();
                        var listNV = db.THANH_PHAN_THAM_DU.Where(x => x.LICHLAMVIEC.ID_LICH_LAM_VIEC == item.ID_LICH_LAM_VIEC).ToList();
                        foreach (var itemNV in listNV)
                        {
                            NHANVIEN_DONVI dv = new NHANVIEN_DONVI();
                            dv.ID_DV = itemNV.NHANVIEN.ID_DON_VI;
                            dv.TEN_DV = itemNV.NHANVIEN.DONVI.TEN_DON_VI;
                            listCalendarDV.Add(dv);
                        }

                        // Lấy ra những nhân viên thuộc đơn vị của lịch và đưa ra view
                        var listDVGroup = listCalendarDV.GroupBy(x => new { x.ID_DV, x.TEN_DV }).Select(x => new { x.Key.ID_DV, x.Key.TEN_DV }).ToList();
                        for (int i = 0; i < listDVGroup.Count(); i++)
                        {
                            llv.NHAN_VIEN += listDVGroup[i].TEN_DV.ToString() + " (";
                            decimal? idDV = listDVGroup[i].ID_DV;
                            var getNV_TPTD = db.THANH_PHAN_THAM_DU.Where(x => x.NHANVIEN.DONVI.ID_DON_VI == idDV && x.ID_LICH_LAM_VIEC == item.ID_LICH_LAM_VIEC).ToList();
                            for (int j = 0; j < getNV_TPTD.Count(); j++)
                            {
                                if ((j == getNV_TPTD.Count() - 1) && (i == listDVGroup.Count() - 1))
                                {
                                    llv.NHAN_VIEN += getNV_TPTD[j].NHANVIEN.HO_TEN.ToString() + ").";
                                }
                                else if (j == getNV_TPTD.Count() - 1)
                                {
                                    llv.NHAN_VIEN += getNV_TPTD[j].NHANVIEN.HO_TEN.ToString() + ") - ";
                                }
                                else
                                {
                                    llv.NHAN_VIEN += getNV_TPTD[j].NHANVIEN.HO_TEN.ToString() + ", ";
                                }
                            }
                        }
                        llvList.Add(llv);
                    }
                }

                List<LICH_LAM_VIEC> listDSLICH = new List<LICH_LAM_VIEC>(llvList);
                events = listDSLICH;
                foreach (var item in llvList.ToList())
                {
                    llvList.Remove(item);
                }
            }
            return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;

            var result = new LichLamViecDAO().DeleteLLV(eventID);

            if(result)
            {
                status = result;
            }

            return new JsonResult { Data = new { status = status } };
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            using (QLVanBan db = new QLVanBan())
            {
                decimal idLich = Convert.ToDecimal(id);
                var lich = db.LICHLAMVIEC.Where(x => x.ID_LICH_LAM_VIEC == idLich).SingleOrDefault();
                DateTime dateNow = DateTime.Now;
                if (lich.NGAY_KET_THUC < dateNow)
                {
                    return RedirectToAction("Index");
                }
                var model = new Model.DB2.LICHLAMVIEC();
                model.ID_LICH_LAM_VIEC = lich.ID_LICH_LAM_VIEC;
                model.NGAY_BAT_DAU = lich.NGAY_BAT_DAU;
                model.NGAY_KET_THUC = lich.NGAY_KET_THUC;
                model.GIO_BAT_DAU = lich.GIO_BAT_DAU;
                model.GIO_KET_THUC = lich.GIO_KET_THUC;
                model.NOI_DUNG = lich.NOI_DUNG;
                model.THANH_PHAN = lich.THANH_PHAN;

                List<decimal> dsNV = new List<decimal>();
                List<decimal> dsDV = new List<decimal>();

                // Lấy ra danh sách đơn vị đã được chọn
                var getListTPTD = db.THANH_PHAN_THAM_DU.Where(x => x.ID_LICH_LAM_VIEC == idLich).ToList();
                var getListDV = db.DONVI.ToList();
                foreach (var item in getListDV)
                {
                    var getListNV = db.NHANVIEN.Where(x => x.ID_DON_VI == item.ID_DON_VI).ToList();
                    int countNVDV = getListNV.Count();
                    int count = 0;
                    foreach (var item2 in getListNV)
                    {
                        foreach (var item3 in getListTPTD)
                        {
                            if (item3.ID_NHAN_VIEN == item2.ID_NHAN_VIEN)
                            {
                                count++;
                                if (count == countNVDV)
                                {
                                    dsDV.Add(item.ID_DON_VI);
                                }
                            }
                        }
                    }
                }

                // Lấy ra danh sách nhân viên đã chọn và xử lý việc trừng lập với ds đơn vị
                dsNV = db.THANH_PHAN_THAM_DU.Where(x => x.ID_LICH_LAM_VIEC == idLich).Select(x => x.ID_NHAN_VIEN).ToList();
                foreach (var item in dsDV.ToList())
                {
                    var getListNVDV = db.NHANVIEN.Where(x => x.ID_DON_VI == item).ToList();
                    foreach (var item2 in getListNVDV)
                    {
                        foreach (var item3 in getListTPTD.ToList())
                        {
                            if (item3.ID_NHAN_VIEN == item2.ID_NHAN_VIEN)
                            {
                                dsNV.Remove(item3.ID_NHAN_VIEN);
                            }
                        }
                    }
                }

                // Đưa dữ liệu ds nv đã chọn ra view
                var listNhanVienC = new List<NHAN_VIEN>();
                foreach (var item in dsNV)
                {
                    var list = db.NHANVIEN.Where(x => x.ID_NHAN_VIEN == item).SingleOrDefault();
                    NHAN_VIEN nhanVien = new NHAN_VIEN();
                    nhanVien.ID_NHAN_VIEN = list.ID_NHAN_VIEN;
                    nhanVien.HO_TEN = list.HO_TEN + " (" + list.DONVI.TEN_DON_VI + ")";
                    listNhanVienC.Add(nhanVien);
                }
                ViewBag.ListCheckNV = new SelectList(listNhanVienC, "ID_NHAN_VIEN", "HO_TEN");

                // Đưa dữ liệu ds đơn vị đã chọn ra view
                var listDonViC = new List<DONVI>();
                foreach (var item in dsDV)
                {
                    var list = db.DONVI.Where(x => x.ID_DON_VI == item).SingleOrDefault();
                    listDonViC.Add(list);
                }
                ViewBag.ListCheckDonVi = new SelectList(listDonViC, "ID_DON_VI", "TEN_DON_VI");

                // Đưa danh sach du lieu nhan vien chua duoc chon ra view
                var listNhanVien = db.NHANVIEN.ToList();
                foreach (var item in listNhanVienC.ToList())
                {
                    foreach (var item2 in listNhanVien.ToList())
                    {
                        if (item2.ID_NHAN_VIEN == item.ID_NHAN_VIEN)
                        {
                            listNhanVien.Remove(item2);
                        }
                    }
                }

                List<NHAN_VIEN> dsNhanVien = new List<NHAN_VIEN>();
                foreach(var item in listNhanVien.GroupBy(x => new { x.ID_NHAN_VIEN, x.HO_TEN }).Select(x => new { x.Key.ID_NHAN_VIEN, x.Key.HO_TEN }))
                {
                    var list = db.NHANVIEN.Where(x => x.ID_NHAN_VIEN == item.ID_NHAN_VIEN).SingleOrDefault();
                    NHAN_VIEN nhanVien = new NHAN_VIEN();
                    nhanVien.ID_NHAN_VIEN = list.ID_NHAN_VIEN;
                    nhanVien.HO_TEN = list.HO_TEN + " (" + list.DONVI.TEN_DON_VI + ")";
                    nhanVien.ID_DON_VI = list.ID_DON_VI;
                    dsNhanVien.Add(nhanVien);
                }
                ViewBag.ListNhanVien = new SelectList(dsNhanVien.OrderBy(x=>x.ID_DON_VI).ToList(), "ID_NHAN_VIEN", "HO_TEN");

                // Đưa danh sach du lieu don vi chua duoc chon ra view
                var listDonVi = db.DONVI.ToList();
                foreach (var item in listDonViC.ToList())
                {
                    foreach (var item2 in listDonVi.ToList())
                    {
                        if (item2 == item)
                        {
                            listDonVi.Remove(item2);
                        }
                    }
                }
                ViewBag.ListDonVi = new SelectList(listDonVi.GroupBy(x => new { x.ID_DON_VI, x.TEN_DON_VI }).Select(x => new { x.Key.ID_DON_VI, x.Key.TEN_DON_VI }), "ID_DON_VI", "TEN_DON_VI");
                
                return View(model);
            }

        }


        [HttpPost]
        public ActionResult Edit(Model.DB2.LICHLAMVIEC llv, string[] slNhanVien, string[] slDonVi)
        {
            // 1. Tìm lịch đã chọn
            // 1. Xóa hết toàn bộ thành phần tham dự lịch được chọn
            // 2. Thực hiện lại thêm mới như create
            
            var resultEditLLV = new LichLamViecDAO().UpdateLLV(llv);

            if(!string.IsNullOrEmpty(resultEditLLV))
            {
                switch (resultEditLLV)
                {
                    case "errorNgay":
                        SetAlert("Cập nhật thất bại. Ngày bắt đầu phải nhỏ hơn ngày kết thúc", "warning");
                        return RedirectToAction("Edit", "Home", new { id = llv.ID_LICH_LAM_VIEC.ToString() });

                    case "errorGio":
                        SetAlert("Cập nhật thất bại. Giờ làm việc trong ngày không hợp lệ", "warning");
                        return RedirectToAction("Edit", "Home", new { id = llv.ID_LICH_LAM_VIEC.ToString() });
                        
                }

                decimal idLLV = Convert.ToDecimal(resultEditLLV);

                // Xoa toan bo thanh phan tham du lich duoc chon
                var resultDelete = new LichLamViecDAO().DeleteTPTD(idLLV);

                if(resultDelete)
                {
                    // Them moi lai thanh phan tham du neu co
                    var resultTPTD = new LichLamViecDAO().InsertTPTD(idLLV, slNhanVien, slDonVi);
                    if (resultTPTD)
                    {
                        SetAlert("Cập nhật thành công.", "success");
                        return RedirectToAction("Index", "Home");
                    }
                }
                
                SetAlert("Cập nhật thành công.", "success");
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
    }
}