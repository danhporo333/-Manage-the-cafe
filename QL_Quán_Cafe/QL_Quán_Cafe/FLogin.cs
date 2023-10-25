using QL_Quán_Cafe.database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_Quán_Cafe
{
    public partial class FLogin : Form
    {
        public FLogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                Model1 db = new Model1();
                var account = db.Account.ToList();
                string username = txtbUserName.Text;
                string password = txtPassword.Text;

                using (var context = new Model1())
                {
                    var user = context.Account.Where(u => u.UserName == username && u.PassWord == password).FirstOrDefault();

                    if (user != null)
                    {
                        // Ẩn form đăng nhập
                        this.Hide();
                        fTableManager f = new fTableManager();
                        f.Show();
                    }
                    else
                    {
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu không hợp lệ. Vui lòng thử lại.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FLogin_Load(object sender, EventArgs e)
        {

        }
    }
}
