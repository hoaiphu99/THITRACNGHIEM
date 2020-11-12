using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THITRACNGHIEM
{
    public partial class frmSinhVien : Form
    {
        string maCS = "";
        string maLop = "";
        int vitri;
        BindingSource bdsLop = new BindingSource();
        public frmSinhVien()
        {
            InitializeComponent();
        }

        private void sINHVIENBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.bdsSinhVien.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void frmSinhVien_Load(object sender, EventArgs e)
        {
            dS.EnforceConstraints = false;
            

            // TODO: This line of code loads data into the 'dS.SINHVIEN' table. You can move, or remove it, as needed.
            this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
            this.sINHVIENTableAdapter.Fill(this.dS.SINHVIEN);

            // TODO: This line of code loads data into the 'dS.BANGDIEM' table. You can move, or remove it, as needed.
            this.bANGDIEMTableAdapter.Connection.ConnectionString = Program.connstr;
            this.bANGDIEMTableAdapter.Fill(this.dS.BANGDIEM);

            DataTable dt = new DataTable();
            dt = Program.ExecSqlDataTable("SELECT MALOP, TENLOP FROM LOP");
            bdsLop.DataSource = dt;
            cmbMaLop.DataSource = bdsLop;
            cmbMaLop.DisplayMember = "MALOP";
            cmbMaLop.ValueMember = "MALOP";
            cmbMaLop.SelectedIndex = 0;


            cmbCoSo.DataSource = Program.bds_dspm;
            cmbCoSo.DisplayMember = "TENCN";
            cmbCoSo.ValueMember = "TEN_SERVER";
            cmbCoSo.SelectedIndex = Program.mCoso;
            if (Program.mGroup == "TRUONG")
                cmbCoSo.Enabled = true;
            else
                cmbCoSo.Enabled = false;
            groupControl2.Enabled = false;
            btnGhi.Enabled = btnPhuchoi.Enabled = false;
            if (bdsSinhVien.Count == 0)
                btnXoa.Enabled = false;
            btnHuy.Enabled = false;
        }

        private void btnThem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            
            vitri = bdsSinhVien.Position;
            groupControl2.Enabled = true;
            bdsSinhVien.AddNew();
            dtpNgaySinh.EditValue = "";

            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = false;
            btnGhi.Enabled = btnPhuchoi.Enabled = btnHuy.Enabled = true;
            gcSinhVien.Enabled = false;
        }

        private void cmbCoSo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbCoSo.SelectedValue.ToString() == "System.Data.DataRowView")
                return;
            Program.servername = cmbCoSo.SelectedValue.ToString();
            if (cmbCoSo.SelectedIndex != Program.mCoso)
            {
                Program.mlogin = Program.remotelogin;
                Program.password = Program.remotepassword;
            }
            else
            {
                Program.mlogin = Program.mloginDN;
                Program.password = Program.passwordDN;
            }
            if (Program.KetNoi() == 0)
                MessageBox.Show("Lỗi kết nối tới cơ sở mới!", "Lỗi", MessageBoxButtons.OK);
            else
            {

                this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                this.sINHVIENTableAdapter.Fill(this.dS.SINHVIEN);

                
            }
        }

        private void btnHuy_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (gcSinhVien.Enabled == false)
                bdsSinhVien.RemoveCurrent();
            groupControl2.Enabled = false;
            btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = true;
            btnGhi.Enabled = btnPhuchoi.Enabled = btnHuy.Enabled = false;
            gcSinhVien.Enabled = true;
        }

        private void cmbMaLop_SelectedIndexChanged(object sender, EventArgs e)
        {
            maLop = cmbMaLop.SelectedValue.ToString();
            
        }

        private void btnGhi_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if(txtMaSV.Text.Trim().Length == 0)
            {
                MessageBox.Show("Mã sinh viên không được trống!", "Lỗi", MessageBoxButtons.OK);
                txtMaSV.Focus();
                return;
            }
            if (txtHo.Text.Trim().Length == 0)
            {
                MessageBox.Show("Họ sinh viên không được trống!", "Lỗi", MessageBoxButtons.OK);
                txtHo.Focus();
                return;
            }
            if (txtTen.Text.Trim().Length == 0)
            {
                MessageBox.Show("Tên sinh viên không được trống!", "Lỗi", MessageBoxButtons.OK);
                txtTen.Focus();
                return;
            }
            if (dtpNgaySinh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Ngày sinh không được trống!", "Lỗi", MessageBoxButtons.OK);
                dtpNgaySinh.Focus();
                return;
            }
            try
            {
                bdsSinhVien.EndEdit();
                bdsSinhVien.ResetCurrentItem();
                this.sINHVIENTableAdapter.Update(this.dS.SINHVIEN);

                groupControl2.Enabled = false;
                btnThem.Enabled = btnSua.Enabled = btnXoa.Enabled = true;
                btnGhi.Enabled = btnPhuchoi.Enabled = btnHuy.Enabled = false;
                gcSinhVien.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi ghi nhân viên\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);
            }
        }

        private void btnXoa_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Int32 maSV = 0;
            if(Program.mGroup == "TRUONG")
            {
                MessageBox.Show("Bạn không được phép xóa!", "Thông báo", MessageBoxButtons.OK);
                return;
            }
            if (MessageBox.Show("Bạn có thật sự muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                try
                {
                    maSV = int.Parse(((DataRowView)bdsSinhVien[bdsSinhVien.Position])["MASV"].ToString());
                    bdsSinhVien.RemoveCurrent();
                    this.sINHVIENTableAdapter.Connection.ConnectionString = Program.connstr;
                    this.sINHVIENTableAdapter.Update(this.dS.SINHVIEN);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi xóa sinh viên. Hãy xóa lại!\n" + ex.Message, "Lỗi", MessageBoxButtons.OK);
                    this.sINHVIENTableAdapter.Fill(this.dS.SINHVIEN);
                    bdsSinhVien.Position = bdsSinhVien.Find("MASV", maSV);
                    return;
                }
            }
            if (bdsSinhVien.Count == 0)
                btnXoa.Enabled = false;
        }
    }
}