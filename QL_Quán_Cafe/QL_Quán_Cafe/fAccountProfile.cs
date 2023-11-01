using QL_Quán_Cafe.database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QL_Quán_Cafe
{
    public partial class fAccountProfile : Form
    {
        public fAccountProfile()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fAccountProfile_Load(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string displayName = txbDisplayname.Text;
            string password = txbPassword.Text;
            string newPassword = txbNewPassWord.Text;
            string reEnterPassword = txbReEnterPass.Text;

            // Kiểm tra mật khẩu hiện tại có đúng không
            if (CheckPassword(username, password))
            {
                if (newPassword == reEnterPassword)
                {
                    UpdatePassword(username, newPassword);
                    MessageBox.Show("Mật khẩu đã được cập nhật!");
                }
                else
                {
                    MessageBox.Show("Mật khẩu nhập lại không khớp!");
                }
            }
            else
            {
                MessageBox.Show("Mật khẩu hiện tại không chính xác!");
            }
        }

        private bool CheckPassword(string username, string password)
        {
            using (var context = new Model1())
            {
                var user = context.Account.SingleOrDefault(u => u.UserName == username);

                if (user != null)
                {
                    return user.PassWord == password; // so sánh mật khẩu từ database
                }

                return false; // trả về false nếu tên đăng nhập không tồn tại
            }
        }

        private void UpdatePassword(string username, string newPassword)
        {
            using (var context = new Model1())
            {
                var user = context.Account.SingleOrDefault(u => u.UserName == username);

                if (user != null)
                {
                    user.PassWord = newPassword; // cập nhật mật khẩu
                    context.SaveChanges(); // lưu thay đổi
                }
            }
        }
    }
}
