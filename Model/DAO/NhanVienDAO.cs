using Model.DB2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.DAO
{
    public class NhanVienDAO
    {
        public QLVanBan db = null;

        public NhanVienDAO()
        {
            db = new QLVanBan();
        }

        public bool Login(string userName, string passWord)
        {
            if(!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(passWord))
            {
                int checkAccount = db.NHANVIEN.Where(x => x.USERNAME == userName && x.PASSWORD == passWord).Count();

                if(checkAccount > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public NHANVIEN GetNhanVien(string userName)
        {
            return db.NHANVIEN.Where(x => x.USERNAME == userName).SingleOrDefault();
        }

        public bool IsVanThu(string userName)
        {
            if(!string.IsNullOrEmpty(userName))
            {
                int isVT = db.NHANVIEN.Where(x => x.USERNAME == userName && x.ID_VAI_TRO == 4).Count();
                if(isVT > 0)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
