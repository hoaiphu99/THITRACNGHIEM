using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace THITRACNGHIEM
{
    public partial class frmDangNhap : Form
    {
        public frmMain f;
        public frmDangNhap()
        {
            InitializeComponent();
        }

        private void cOSOBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.cOSOBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.dS);

        }

        private void frmDangNhap_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dS_DSPHANMANH.v_DS_PHANMANH' table. You can move, or remove it, as needed.
            this.v_DS_PHANMANHTableAdapter.Fill(this.dS_DSPHANMANH.V_DS_PHANMANH);
            /*// TODO: This line of code loads data into the 'dS.COSO' table. You can move, or remove it, as needed.
            this.cOSOTableAdapter.Fill(this.dS.COSO);
            //cmbCoSo.SelectedIndex = 1; cmbCoSo.SelectedIndex = 0;*/
            string chuoiketnoi = "Data Source=HOAIPHU-PC;Initial Catalog=TN_CSDLPT;Integrated Security=True";
            Program.conn.ConnectionString = chuoiketnoi;
            Program.conn.Open();
            DataTable dt = new DataTable();
            dt = Program.ExecSqlDataTable("SELECT * FROM V_DS_PHANMANH");
            Program.bds_dspm.DataSource = dt;
            cmbCoSo.DataSource = dt;
            cmbCoSo.DisplayMember = "TEN_COSO";
            cmbCoSo.ValueMember = "TEN_SERVER";
            cmbCoSo.SelectedIndex = 0;
        }

        private void btnDangNhap_Click_1(object sender, EventArgs e)
        {
            if (txtLogin.Text.Trim() == "" || txtPassword.Text.Trim() == "")
            {
                MessageBox.Show("Login name và mật mã không được trống", "", MessageBoxButtons.OK);
                return;
            }
            Program.mlogin = txtLogin.Text; 
            Program.password = txtPassword.Text;
            if (Program.KetNoi() == 0) return;

            Program.mCoso = cmbCoSo.SelectedIndex;

            Program.mloginDN = Program.mlogin;
            Program.passwordDN = Program.password;
            string strLenh = "EXEC SP_THONGTINDANGNHAP '" + Program.mlogin + "'";

            Program.myReader = Program.ExecSqlDataReader(strLenh);
            if (Program.myReader == null) return;
            Program.myReader.Read();

            Program.username = Program.myReader.GetString(0);     // Lay user name
            if (Convert.IsDBNull(Program.username))
            {
                MessageBox.Show("Login bạn nhập không có quyền truy cập dữ liệu\n Bạn xem lại username, password", "", MessageBoxButtons.OK);
                return;
            }
            Program.mHoten = Program.myReader.GetString(1);
            Program.mGroup = Program.myReader.GetString(2);
            Program.myReader.Close();
            Program.conn.Close();
            //Program.frmMain.MANV.Text = "";
            MessageBox.Show("Nhân viên - Nhóm : " + Program.mHoten + " - " + Program.mGroup, "", MessageBoxButtons.OK);
            f.hienThiMenu();
        }

        private void cmbCoSo_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            try
            {
                Program.servername = cmbCoSo.SelectedValue.ToString();
            }
            catch (Exception)
            {
            };
        }
    }
}
