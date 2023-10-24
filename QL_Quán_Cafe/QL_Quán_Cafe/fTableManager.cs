using QL_Quán_Cafe.database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.LinkLabel;

namespace QL_Quán_Cafe
{
    public partial class fTableManager : Form
    {
        public fTableManager()
        {
            InitializeComponent();
            
        }

        private void đăngXuấtToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void thôngTinCáNhânToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile();
            f.ShowDialog();
        }

        private void adminToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            fAdmin a = new fAdmin();
            a.ShowDialog();
        }

        private void fTableManager_Load(object sender, EventArgs e)
        {
            Model1 db = new Model1();
            var account = db.TableFood.ToList();
            using (var context = new Model1())
            {
                var tables = context.TableFood.ToList();

                foreach (var tableName in tables)
                {
                    var button = new Button();
                    button.Text = tableName.name + "\n" + tableName.status;
                    button.Size = new Size(80, 80); // Chiều rộng và chiều cao của nút
                    flowLayoutPanel1.Controls.Add(button);
                    button.Click += button_Click;
                    switch (tableName.status)
                    {
                        case "Trống":
                            button.BackColor = Color.Aqua;
                            break;
                        default:
                            button.BackColor = Color.LightPink;
                            break;
                    }
                }
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;

            if (clickedButton != null)
            {
                string buttonInfo = clickedButton.Text;
                string tableName = buttonInfo.Split('\n')[0]; // Lấy tên bàn từ Text của button

                using (Model1 context = new Model1())
                {
                    // Sử dụng LINQ để truy vấn dữ liệu từ các bảng BillInfo, Bill, và Food
                    var query = from billInfo in context.BillInfo
                                join bill in context.Bill on billInfo.idBill equals bill.id
                                join food in context.Food on billInfo.idFood equals food.id
                                where bill.idTable == 1
                                select new
                                {
                                    FoodName = food.name,
                                    Count = billInfo.count,
                                    Price = food.price,
                                    TotalPrice = food.price * billInfo.count
                                };

                    // Xóa tất cả các mục hiện tại trong ListView
                    listView1.Items.Clear();

                    // Hiển thị thông tin từ truy vấn trong ListView
                    foreach (var result in query)
                    {
                        var item = new ListViewItem(result.FoodName);
                        item.SubItems.Add(result.Count.ToString());
                        item.SubItems.Add(result.Price.ToString());
                        item.SubItems.Add(result.TotalPrice.ToString());

                        listView1.Items.Add(item);
                    }
                }
            }
        }
    }
}



