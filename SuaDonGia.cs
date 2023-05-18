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
    public partial class SuaDonGia : Form
    {
        public SuaDonGia()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (txtNuoc.Text != "" && txtDien.Text != "")
            {
                using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
                {
                    c.Open();
                    string sqlsua = "update dongia set sotien=@nuoc where madg=2";
                    using (SQLiteCommand cmsua = new SQLiteCommand(sqlsua, c))
                    {
                        cmsua.Parameters.AddWithValue("@nuoc", txtNuoc.Text);
                        cmsua.ExecuteNonQuery();
                    }
                    string sqlsua2 = "update dongia set sotien=@dien where madg=1";
                    using (SQLiteCommand cmsua = new SQLiteCommand(sqlsua2, c))
                    {
                        cmsua.Parameters.AddWithValue("@dien", txtDien.Text);
                        cmsua.ExecuteNonQuery();
                    }
                    c.Close();
                }
                khoiphuc();
            }
        }
        void khoiphuc()
        {
            using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
            {
                c.Open();
                string sql = "select * from dongia";
                using (SQLiteCommand cm = new SQLiteCommand(sql, c))
                {
                    SQLiteDataAdapter da = new SQLiteDataAdapter(cm);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    txtDien.Text = dt.Rows[0][1].ToString();
                    txtNuoc.Text = dt.Rows[1][1].ToString();
                }
                c.Close();
            }
        }

        private void SuaDonGia_Load(object sender, EventArgs e)
        {
            khoiphuc();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            khoiphuc();
        }
    }
}
