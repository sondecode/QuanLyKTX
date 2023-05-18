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
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            using (SQLiteConnection c = new SQLiteConnection(string.Format(@"Data Source={0}\KTX.db;Version=3;", Application.StartupPath)))
            {
                c.Open();
                string sqlSelect = "select * from taikhoan where username='" + txtUsername.Text + "' and password='" + txtPassword.Text + "'";
                using (SQLiteCommand cm = new SQLiteCommand(sqlSelect, c))
                {
                    using (SQLiteDataReader reader = cm.ExecuteReader())
                    {
                        if (reader.Read() == true)
                        {
                            this.Hide();
                            QuanLyPhong frmP = new QuanLyPhong();
                            frmP.Show();
                        }
                        else
                        {
                            MessageBox.Show("Vui lòng thử lại!");
                            txtPassword.Text = "";
                        }
                    }
                }
                c.Close();
            }
        }
    }
}
