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
    public partial class QuanLyPhong : Form
    {
        public QuanLyPhong()
        {
            InitializeComponent();
        }
        private void listBox1_DoubleClick(object sender, MouseEventArgs e)
        {
            int index = this.listBox1.IndexFromPoint(e.Location);
            if (index != System.Windows.Forms.ListBox.NoMatches)
            {
                QuanLySV frmSV = new QuanLySV(listBox1.SelectedValue.ToString());
                frmSV.Show();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtMaphong.Text != "")
            {
                QuanLyDN frmDN = new QuanLyDN(txtMaphong.Text, txtNo.Text);
                frmDN.Show();
            }
        }
        void hienthi()
        {
            using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
            {
                c.Open();
                DataTable dt = new DataTable();
                using (SQLiteDataAdapter da = new SQLiteDataAdapter("select * from phong", c))
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
                string sql = "select * from loaiphong";
                using (SQLiteCommand cm = new SQLiteCommand(sql, c))
                {
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    cbbLoai.DataSource = dt;
                    cbbLoai.DisplayMember = "tenloai";
                    cbbLoai.ValueMember = "maloai";
                }
                c.Close();
            }
        }
        void hienlist()
        {
            using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
            {
                c.Open();
                string sql = "select * from sinhvien where maphong=@maphong";
                using (SQLiteCommand cm = new SQLiteCommand(sql, c))
                {
                    cm.Parameters.AddWithValue("@maphong", txtMaphong.Text);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    listBox1.DataSource = dt;
                    listBox1.DisplayMember = "ten";
                    listBox1.ValueMember = "masv";
                }
                c.Close();
            }
        }
        private void QuanLyPhong_Load(object sender, EventArgs e)
        {
            hienthi();
            hiencbb();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int index = dataGridView1.CurrentCell.RowIndex;
            txtMaphong.Text = dataGridView1.Rows[index].Cells["maphong"].Value.ToString();
            cbbLoai.SelectedValue = dataGridView1.Rows[index].Cells["maloai"].Value.ToString();
            numSuc.Text = dataGridView1.Rows[index].Cells["succhua"].Value.ToString();
            txtSoSV.Text = dataGridView1.Rows[index].Cells["sosv"].Value.ToString();
            txtNo.Text = dataGridView1.Rows[index].Cells["notien"].Value.ToString();
            hienlist();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtMaphong.Text != "")
            {
                using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
                {
                    c.Open();
                    string sql = "select * from phong where maphong=@maphong";
                    using (SQLiteCommand cm = new SQLiteCommand(sql, c))
                    {
                        cm.Parameters.AddWithValue("@maphong", txtMaphong.Text);
                        using (SQLiteDataReader reader = cm.ExecuteReader())
                        {
                            if (reader.Read() == false)
                            {
                                string sqlthem = "insert into phong values(@maphong,@maloai,@succhua,0,0)";
                                using (SQLiteCommand cmthem = new SQLiteCommand(sqlthem, c))
                                {
                                    cmthem.Parameters.AddWithValue("@maphong", txtMaphong.Text);
                                    cmthem.Parameters.AddWithValue("@maloai", cbbLoai.SelectedValue);
                                    cmthem.Parameters.AddWithValue("@succhua", numSuc.Value);
                                    cmthem.ExecuteNonQuery();
                                }
                                c.Close();
                                hienthi();
                            }
                            else MessageBox.Show("Mã phòng bị trùng!");
                        }
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (txtMaphong.Text != "")
            {
                using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
                {
                    c.Open();
                    string sqlsua = "update phong set maloai=@maloai, succhua=@succhua where maphong=@maphong";
                    using (SQLiteCommand cmsua = new SQLiteCommand(sqlsua, c))
                    {
                        cmsua.Parameters.AddWithValue("@maphong", txtMaphong.Text);
                        cmsua.Parameters.AddWithValue("@maloai", cbbLoai.SelectedValue);
                        cmsua.Parameters.AddWithValue("@succhua", numSuc.Value);
                        cmsua.ExecuteNonQuery();
                    }
                    c.Close();
                }
                hienthi();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtMaphong.Text != "")
            {
                using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
                {
                    c.Open();
                    string sqlsua = "delete from phong where maphong=@maphong";
                    using (SQLiteCommand cmsua = new SQLiteCommand(sqlsua, c))
                    {
                        cmsua.Parameters.AddWithValue("@maphong", txtMaphong.Text);
                        cmsua.ExecuteNonQuery();
                    }
                    c.Close();
                }
                hienthi();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            hienthi();
        }

        private void đơnGiáToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SuaDonGia frmSua = new SuaDonGia();
            frmSua.ShowDialog();
        }

        private void quảnLýĐiệnNướcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtMaphong.Text != "")
            {
                QuanLyDN frmDN = new QuanLyDN(txtMaphong.Text, txtNo.Text);
                frmDN.Show();
            }
        }

        private void quảnLýSinhViênToolStripMenuItem_Click(object sender, EventArgs e)
        {
            QuanLySV frmSV = new QuanLySV("1");
            frmSV.Show();
        }
    }
}
