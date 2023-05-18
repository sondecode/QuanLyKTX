using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace QuanLyKTX
{
    public partial class QuanLySV : Form
    {
        public QuanLySV(string masv)
        {
            InitializeComponent();
            txtMasv.Text = masv;
        }
        void hienthi()
        {
            using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
            {
                c.Open();
                DataTable dt = new DataTable();
                using (SQLiteDataAdapter da = new SQLiteDataAdapter("select masv,ten,sdt,case sex when 1 then 'Nam' else 'Nu' end as gioitinh, maphong from sinhvien", c))
                {
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
                c.Close();
            }
        }
        void hiencbb()
        {
            using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
            {
                c.Open();
                string sql = "select maphong from phong";
                using (SQLiteCommand cm = new SQLiteCommand(sql, c))
                {
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cbbMaphong.DataSource = dt;
                    cbbMaphong.DisplayMember = "maphong";
                    cbbMaphong.ValueMember = "maphong";
                }
                c.Close();
            }
        }
        private void QuanLySV_Load(object sender, EventArgs e)
        {
            hienthi();
            hiencbb();
            int rowIndex = -1;
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                if (row.Cells["masv"].Value.ToString().Equals(txtMasv.Text))
                {
                    rowIndex = row.Index;
                    break;
                }
            }
            txtMasv.Text = dataGridView1.Rows[rowIndex].Cells["masv"].Value.ToString();
            cbbMaphong.SelectedValue = dataGridView1.Rows[rowIndex].Cells["maphong"].Value.ToString();
            txtSdt.Text = dataGridView1.Rows[rowIndex].Cells["sdt"].Value.ToString();
            txtTen.Text = dataGridView1.Rows[rowIndex].Cells["Ten"].Value.ToString();
            string gioitinh = dataGridView1.Rows[rowIndex].Cells["gioitinh"].Value.ToString();
            if (gioitinh == "Nam")
            {
                rdNam.Checked = true;
            }
            else rdNu.Checked = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            txtMasv.Text = dataGridView1.Rows[index].Cells["masv"].Value.ToString();
            cbbMaphong.SelectedValue = dataGridView1.Rows[index].Cells["maphong"].Value.ToString();
            txtSdt.Text = dataGridView1.Rows[index].Cells["sdt"].Value.ToString();
            txtTen.Text = dataGridView1.Rows[index].Cells["Ten"].Value.ToString();
            string gioitinh = dataGridView1.Rows[index].Cells["gioitinh"].Value.ToString();
            if (gioitinh == "Nam")
            {
                rdNam.Checked = true;
            }
            else rdNu.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtMasv.Text != "")
            {
                using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
                {
                c.Open();
                string sql = "select * from sinhvien where masv=@masv";
                using (SQLiteCommand cm = new SQLiteCommand(sql, c))
                {
                    cm.Parameters.AddWithValue("@masv", txtMasv.Text);
                    using (SQLiteDataReader reader = cm.ExecuteReader())
                    {
                        if (reader.Read()==false)
                        {
                                string sqlthem = "insert into sinhvien values(@masv,@ten,@sdt,@gioitinh,@maphong)";
                                using (SQLiteCommand cmthem = new SQLiteCommand(sqlthem, c))
                                {
                                    if (rdNam.Checked == true)
                                    {
                                        cmthem.Parameters.AddWithValue("@gioitinh", 1);
                                    }
                                    else cmthem.Parameters.AddWithValue("@gioitinh", 0);
                                    cmthem.Parameters.AddWithValue("@masv", txtMasv.Text);
                                    cmthem.Parameters.AddWithValue("@ten", txtTen.Text);
                                    cmthem.Parameters.AddWithValue("@sdt", txtSdt.Text);
                                    cmthem.Parameters.AddWithValue("@maphong", cbbMaphong.SelectedValue);
                                    cmthem.ExecuteNonQuery();
                                    string sqlsua = "update phong set sosv=sosv+1 where maphong=@maphong";
                                    using (SQLiteCommand cmsua = new SQLiteCommand(sqlsua, c))
                                    {

                                        cmsua.Parameters.AddWithValue("@maphong", cbbMaphong.SelectedValue);
                                        cmsua.ExecuteNonQuery();
                                    }
                                }
                                c.Close();
                                hienthi();
                            }
                        else MessageBox.Show("Mã sinh viên bị trùng!");
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtMasv.Text != "")
            {
                using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
                {
                    c.Open();
                    //sửa số lượng sinh viên của phòng
                    string sqlSelect = "select * from sinhvien where masv='" + txtMasv.Text + "'";
                    using (SQLiteCommand cm = new SQLiteCommand(sqlSelect, c))
                    {
                        using (SQLiteDataReader reader = cm.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                //neu thay doi phong khac
                                if (reader["maphong"].ToString() != cbbMaphong.SelectedValue.ToString())
                                {
                                    //tang 1sv cho phong moi
                                    string sqlsua = "update phong set sosv=sosv+1 where maphong=@maphong";
                                    using (SQLiteCommand cmsua = new SQLiteCommand(sqlsua, c))
                                    {

                                        cmsua.Parameters.AddWithValue("@maphong", cbbMaphong.SelectedValue);
                                        cmsua.ExecuteNonQuery();
                                    }
                                    //giam 1sv cho phong cu
                                    string sqlsua2 = "update phong set sosv=sosv-1 where maphong=@maphong";
                                    using (SQLiteCommand cmsua = new SQLiteCommand(sqlsua2, c))
                                    {

                                        cmsua.Parameters.AddWithValue("@maphong", reader["maphong"].ToString());
                                        cmsua.ExecuteNonQuery();
                                    }
                                }
                                reader.Close();//dong read
                                //sua thong tin
                                string sqlthem = "update sinhvien set ten=@ten, sdt=@sdt, sex=@gioitinh, maphong=@maphong where masv=@masv";
                                using (SQLiteCommand cmthem = new SQLiteCommand(sqlthem, c))
                                {
                                    if (rdNam.Checked == true)
                                    {
                                        cmthem.Parameters.AddWithValue("@gioitinh", 1);
                                    }
                                    else cmthem.Parameters.AddWithValue("@gioitinh", 0);
                                    cmthem.Parameters.AddWithValue("@masv", txtMasv.Text);
                                    cmthem.Parameters.AddWithValue("@ten", txtTen.Text);
                                    cmthem.Parameters.AddWithValue("@sdt", txtSdt.Text);
                                    cmthem.Parameters.AddWithValue("@maphong", cbbMaphong.SelectedValue);
                                    cmthem.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    c.Close();
                }
                hienthi();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtMasv.Text != "")
            {
                using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
                {
                    c.Open();
                    string sqlxoa = "delete from sinhvien where masv=@masv";
                    using (SQLiteCommand cmxoa = new SQLiteCommand(sqlxoa, c))
                    {
                        cmxoa.Parameters.AddWithValue("@masv", txtMasv.Text);
                        cmxoa.ExecuteNonQuery();
                        string sqlsua = "update phong set sosv=sosv-1 where maphong=@maphong";
                        using (SQLiteCommand cmsua = new SQLiteCommand(sqlsua, c))
                        {

                            cmsua.Parameters.AddWithValue("@maphong", cbbMaphong.SelectedValue);
                            cmsua.ExecuteNonQuery();
                        }
                    }
                    c.Close();
                }
                hienthi();
            }
        }
    }
}
