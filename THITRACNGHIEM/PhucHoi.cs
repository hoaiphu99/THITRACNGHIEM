using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THITRACNGHIEM
{
    class PhucHoi
    {
        private Stack<string> myStack = new Stack<string>();
        private string DataTruocKhiSua = "";

        //-----------MÔN HỌC ------------------------
        public string GetDataTruocKhiSua()
        {
            return this.DataTruocKhiSua;
        }
        public void PushStack_ThemMH(string newMaMH)
        {
            myStack.Push("exec [dbo].[SP_PhucHoiThemMH] '" + newMaMH + "'");
        }
        public void PushStack_XoaMH(string maMH, string tenMH)
        {
            myStack.Push("exec [dbo].[SP_PhucHoiXoaMH] '" + maMH + "', N'" + tenMH + "'");
        }
        public void Save_OldMH(string oldMaMH, string oldTenMH)
        {
            DataTruocKhiSua = oldMaMH + "/" + oldTenMH;
        }

        public void PushStack_SuaMH(string newMaMH, string newTenMH)
        {
            string[] arr = DataTruocKhiSua.Split('/');
            //myStack.Push("update[dbo].[MONHOC] set MAMH = '" + arr[0] + "', TENMH = '" + arr[1] + "' where MAMH = '" + newMaMH + "'");
            myStack.Push("exec[dbo].[SP_PhucHoiSuaMH] '" + newMaMH + "', N'" + newTenMH + "', '" + arr[0] + "', N'" + arr[1] + "'");
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
            DataTruocKhiSua = oldMaSV + "/" + oldHo + "/" + oldTen + "/" + oldNgaySinh + "/" + oldDiaChi + "/" + oldMaLop;
        }

        public void PushStack_SuaSV(string newMaSV)
        {
            string[] arr = DataTruocKhiSua.Split('/');
            myStack.Push("update [dbo].[SINHVIEN] set MASV = '" + arr[0] + "', HO = '" + arr[1] +"', TEN = '" + arr[2] + "', NGAYSINH = '" + arr[3] + "', DIACHI = '" + arr[4] + "', MALOP = '" + arr[5] + "' where MASV = '" + newMaSV + "'");
        }

    }

}
