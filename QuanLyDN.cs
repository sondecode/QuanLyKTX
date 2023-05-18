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
    public partial class QuanLyDN : Form
    {
        static string mp;
        public QuanLyDN(string maphong, string no)
        {
            InitializeComponent();
            cbbMaphong.Text = maphong;
            txtNo.Text = no;
            mp = maphong;
        }
        void hienthi()
        {
            using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
            {
                c.Open();
                DataTable dt = new DataTable();
                string sql = "select * from dn where maphong=@maphong";
                using (SQLiteCommand cm = new SQLiteCommand(sql, c))
                {
                    cm.Parameters.AddWithValue("@maphong", cbbMaphong.Text);
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
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
        private void QuanLyDN_Load(object sender, EventArgs e)
        {
            hiencbb();
            cbbMaphong.Text = mp;
            hienthi();
        }
        void lammoi()
        {
            int dgdien;
            int dgnuoc;
            using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
            {
                c.Open();
                string sql = "select * from dongia";
                using (SQLiteCommand cm = new SQLiteCommand(sql, c))
                {
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dgdien = int.Parse(dt.Rows[0][1].ToString());
                    dgnuoc = int.Parse(dt.Rows[1][1].ToString());
                }
                c.Close();
            }
            int tong = int.Parse(txtNo.Text.ToString());
            tong += int.Parse(numDien.Value.ToString()) * dgdien;
            tong += int.Parse(numNuoc.Value.ToString()) * dgnuoc;
            tong += int.Parse(numTienphong.Value.ToString());
            txtNo.Text = tong.ToString();
            if (txtNo.Text != "")
            {
                using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
                {
                    c.Open();
                    string sqlsua = "update phong set notien=@tongno where maphong=@maphong";
                    using (SQLiteCommand cmsua = new SQLiteCommand(sqlsua, c))
                    {
                        cmsua.Parameters.AddWithValue("@maphong", cbbMaphong.Text);
                        cmsua.Parameters.AddWithValue("@tongno", tong);
                        cmsua.ExecuteNonQuery();
                    }
                    c.Close();
                }
                hienthi();
            }


        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cbbMaphong.Text != "")
            {
                using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
                {
                    c.Open();
                    string sqlthem = "insert into dn values(null,@maphong,@ngay,@dien,@nuoc,@tienphong)";
                    using (SQLiteCommand cmthem = new SQLiteCommand(sqlthem, c))
                    {
                        DateTime dateTime = DateTime.UtcNow.Date;
                        cmthem.Parameters.AddWithValue("@maphong", cbbMaphong.Text);
                        cmthem.Parameters.AddWithValue("@ngay", dateTime.ToString("MM/dd/yyyy"));
                        cmthem.Parameters.AddWithValue("@dien", numDien.Value);
                        cmthem.Parameters.AddWithValue("@nuoc", numNuoc.Value);
                        cmthem.Parameters.AddWithValue("@tienphong", numTienphong.Value);
                        cmthem.ExecuteNonQuery();
                    }
                    c.Close();
                }
                hienthi();
                lammoi();
            }
        }

        private void cbbMaphong_SelectedIndexChanged(object sender, EventArgs e)
        {
            hienthi();
            using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
            {
                c.Open();
                string sql = "select maloai,notien from phong where maphong=@maphong";
                using (SQLiteCommand cm = new SQLiteCommand(sql, c))
                {
                    cm.Parameters.AddWithValue("@maphong", cbbMaphong.Text);
                    using (SQLiteDataReader reader = cm.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtNo.Text = reader["notien"].ToString();
                        }
                        reader.Close();
                    }
                }
                c.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (cbbMaphong.Text != "")
            {
                using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
                {
                    c.Open();
                    string sqlsua = "update phong set notien=@tongno where maphong=@maphong";
                    using (SQLiteCommand cmsua = new SQLiteCommand(sqlsua, c))
                    {
                        cmsua.Parameters.AddWithValue("@maphong", cbbMaphong.Text);
                        cmsua.Parameters.AddWithValue("@tongno", txtNo.Text);
                        cmsua.ExecuteNonQuery();
                    }
                    c.Close();
                }
                hienthi();
            }
        }
    }
}
