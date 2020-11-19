using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THITRACNGHIEM
{
    class PhucHoi
    {
        Stack<string> myStack = new Stack<string>();
        string TruocKhiSua = "";

        // sinh viên
        public void PushStack_ThemSV(string newMaSV)
        {
            myStack.Push("delete dbo.SINHVIEN where MASV = '" + newMaSV + "'");
        }

        public void PushStack_XoaSV(string maSV, string ho, string ten, string ngaySinh, string diaChi, string maLop)
        {
            myStack.Push("insert into [dbo].[SINHVIEN] ( MASV, HO, TEN, NGAYSINH, DIACHI, MALOP ) " +
                "values ('" + maSV + "', N'" + ho + "', N'" + ten + "', '" + ngaySinh + "', N'" + diaChi + "', '" + maLop + "')");
        }

        public void Save_OldSV(string oldMaSV, string oldHo, string oldTen, string oldNgaySinh, string oldDiaChi, string oldMaLop)
        {
            TruocKhiSua = oldMaSV + "/" + oldHo + "/" + oldTen + "/" + oldNgaySinh + "/" + oldDiaChi + "/" + oldMaLop;
        }

        public void PushStack_SuaSV(string newMaSV)
        {
            string[] arr = TruocKhiSua.Split('/');
            myStack.Push("update [dbo].[SINHVIEN] set MASV = '" + arr[0] + "', HO = '" + arr[1] +"', TEN = '" + arr[2] + "', NGAYSINH = '" + arr[3] + "', DIACHI = '" + arr[4] + "', MALOP = '" + arr[5] + "' where MASV = '" + newMaSV + "'");
        }

        // môn học
        public void PushStack_ThemMH(string newMaMH)
        {
            myStack.Push("delete dbo.MONHOC where MAMH = '" + newMaMH + "'");
        }

        public void PushStack_XoaMH(string maMH, string tenMH)
        {
            myStack.Push("insert into [dbo].[MONHOC] ( MAMH, TENMH ) values ('" + maMH + "', N'" + tenMH + "')");
        }

        public void Save_OldMH(string oldMaMH, string oldTenMH)
        {
            TruocKhiSua = oldMaMH + "/" + oldTenMH;
        }

        public void PushStack_SuaMH(string newMaMH)
        {
            string[] arr = TruocKhiSua.Split('/');
            myStack.Push("update[dbo].[MONHOC] set MAMH = '" + arr[0] + "', TENMH = '" + arr[1] + "' where MAMH = '" + newMaMH + "'");
        }

        public string PopStack()
        {
            if (myStack.Count == 0)
            {
                return "Đã phục hồi hết các thao tác, không thể phục hồi được nữa!";
            }
            string sql = myStack.Pop();
            Program.ExecSqlNonQuery(sql);
            return "success";
        }
    }

}
