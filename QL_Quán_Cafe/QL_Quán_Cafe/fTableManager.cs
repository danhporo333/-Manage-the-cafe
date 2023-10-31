using QL_Quán_Cafe.database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
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
        private int selectedTableId = -1;
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
            this.Hide();
            fAdmin a = new fAdmin();
            a.ShowDialog();
        }

        private void fTableManager_Load(object sender, EventArgs e)
        {
            try
            {
                Model1 db = new Model1();
                comboBox1.DataSource = db.FoodCategory.ToList();
                comboBox1.DisplayMember = "name";
                comboBox1.ValueMember = "id";

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
                        var hasUnpaidBill = context.Bill.Any(b => b.idTable == tableName.id && b.status == 0);
                        if (hasUnpaidBill || tableName.status != "Trống")
                        {
                            button.BackColor = Color.LightPink;
                        }
                        else
                        {
                            button.BackColor = Color.Aqua;
                        }
                    }
                }

                //đổ dữ liệu từ bảng tableFood vào cbSwitchTable
                var d = db.TableFood.ToList();
                cbSwitchTable.DataSource = db.TableFood.ToList();
                cbSwitchTable.DisplayMember = "name";
                cbSwitchTable.ValueMember = "id";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            double totalprice = 0;
            if (clickedButton != null)
            {
                
                string buttonInfo = clickedButton.Text;
                string tableName = buttonInfo.Split('\n')[0]; // Lấy tên bàn từ Text của button

                using (Model1 context = new Model1())
                {
                    var table = context.TableFood.FirstOrDefault(p => p.name == tableName);
                    if (table != null)
                    {
                        selectedTableId = table.id;
                    }

                        // Sử dụng LINQ để truy vấn dữ liệu từ các bảng BillInfo, Bill, và Food 
                        var query = from billInfo in context.BillInfo
                                    join bill in context.Bill on billInfo.idBill equals bill.id
                                    join food in context.Food on billInfo.idFood equals food.id
                                    where bill.status == 0 && bill.idTable == context.TableFood.FirstOrDefault(p => p.name == tableName && p.status == "Có Người").id
                                    select new
                                    {
                                        FoodName = food.name,
                                        Count = billInfo.count,
                                        Price = food.price,
                                        TotalPrice = food.price * billInfo.count
                                    };

                        // Xóa tất cả các mục hiện tại trong ListView
                        listView1.Items.Clear();

                        // Hiển thị thông tin từ truy vấn trong ListView và tính tổng tiền
                        foreach (var p in query)
                        {
                            var item = new ListViewItem(p.FoodName);
                            item.SubItems.Add(p.Count.ToString());
                            item.SubItems.Add(p.Price.ToString());
                            item.SubItems.Add(p.TotalPrice.ToString());
                            totalprice += p.TotalPrice; // Tính tổng tiền

                            listView1.Items.Add(item);
                        }
                    
                    Tong.Text = totalprice.ToString("c");
                }
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy giá trị được chọn trong comboBox1
            if (comboBox1.SelectedValue != null)
            {
                if (int.TryParse(comboBox1.SelectedValue.ToString(), out int selectedCategoryId))
                {
                    // Truy vấn danh sách các món ăn có categoryId tương ứng
                    using (Model1 db = new Model1())
                    {
                        var foodList = db.Food.Where(p => p.idCategory == selectedCategoryId).ToList();
                        // Đặt danh sách món ăn cho comboBox2
                        comboBox2.DataSource = foodList;
                        comboBox2.DisplayMember = "name";
                        comboBox2.ValueMember = "id";
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            ListViewItem existingItem = listView1.Items.Cast<ListViewItem>().FirstOrDefault(item => item.SubItems[0].Text == comboBox2.Text);
            if (existingItem == null)
            {
                string selectedFoodName = comboBox2.Text; // Tên món ăn từ ComboBox
                int selectedQuantity = (int)soLuong.Value; // Số lượng từ NumericUpDown
                using (Model1 context = new Model1())
                {
                    var food = context.Food.FirstOrDefault(f => f.name == selectedFoodName);
                    if (food != null)
                    {
                        // Lấy giá tiền từ cơ sở dữ liệu SQL
                        decimal selectedPrice = (decimal)food.price;
                        decimal totalPrice = selectedQuantity * selectedPrice;

                        // Tạo một mục mới cho ListView
                        ListViewItem newItem = new ListViewItem(selectedFoodName);
                        newItem.SubItems.Add(selectedQuantity.ToString()); // Số lượng
                        newItem.SubItems.Add(selectedPrice.ToString()); // Giá tiền
                        newItem.SubItems.Add(totalPrice.ToString()); // Thành tiền

                        listView1.Items.Add(newItem);

                    }
                }
            }
            else
            {
                // Món ăn đã tồn tại trong ListView
                // Tăng số lượng lên
                int currentQuantity = int.Parse(existingItem.SubItems[1].Text);
                int newQuantity = currentQuantity + (int)soLuong.Value;
                existingItem.SubItems[1].Text = newQuantity.ToString();

                // Cập nhật thành tiền
                decimal unitPrice = decimal.Parse(existingItem.SubItems[2].Text);
                decimal newTotalPrice = newQuantity * unitPrice;
                existingItem.SubItems[3].Text = newTotalPrice.ToString();
            }
            decimal totalAmount = 0;
            foreach (ListViewItem item in listView1.Items)
            {
                decimal totalPrice = decimal.Parse(item.SubItems[3].Text);
                totalAmount += totalPrice;
            }
            Tong.Text = totalAmount.ToString("c");

            // chuyển trạng thái khi thêm món mà không cần phải khởi động lại chương trình
            using (Model1 context = new Model1())
            {
                var table = context.TableFood.FirstOrDefault(t => t.id == selectedTableId);

                if (table != null)
                {
                    // Tạo hóa đơn mới nếu bàn chưa có hóa đơn chưa thanh toán
                    var existingBill = context.Bill.FirstOrDefault(b => b.idTable == selectedTableId && b.status == 0);
                    if (existingBill == null)
                    {
                        var newBill = new Bill
                        {
                            DateCheckIn = DateTime.Now,
                            idTable = table.id,
                            status = 0 // 0: Chưa thanh toán
                        };

                        context.Bill.Add(newBill);
                        context.SaveChanges();
                        existingBill = newBill;
                    }

                    // Thêm các mục chi tiết hóa đơn (BillInfo)
                    foreach (ListViewItem item in listView1.Items)
                    {
                        string foodName = item.SubItems[0].Text;
                        int quantity = int.Parse(item.SubItems[1].Text);

                        var food = context.Food.FirstOrDefault(f => f.name == foodName);
                        if (food != null)
                        {
                            var billInfo = new BillInfo
                            {
                                idBill = existingBill.id,
                                idFood = food.id,
                                count = quantity
                            };
                            context.BillInfo.Add(billInfo);
                        }
                    }
                    context.SaveChanges();

                    // Cập nhật trạng thái của bàn thành "Có Người"
                    table.status = "Có Người";
                    context.SaveChanges();

                    // Cập nhật màu sắc của nút bàn thành LightPink
                    var button = flowLayoutPanel1.Controls.OfType<Button>().FirstOrDefault(b => b.Text.StartsWith(table.name));
                    if (button != null)
                    {
                        button.BackColor = Color.LightPink;
                        button.Text = table.name + "\n" + "Có Người";
                    }

                    // Làm mới danh sách món ăn và tổng tiền
                    listView1.Items.Clear();
                    Tong.Text = "$0.00";
                    selectedTableId = -1;

                    MessageBox.Show("Đã thêm món và tạo/cập nhật hóa đơn thành công!");
                }
            }
        }

        private void btnThanhtoan_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count == 0)
            {
                MessageBox.Show("Không có món ăn để thanh toán!");
                return;
            }

            using (Model1 context = new Model1())
            {
                if (selectedTableId == -1)
                {
                    MessageBox.Show("Vui lòng chọn một bàn để thanh toán!");
                    return;
                }

                // Lấy thông tin bàn đang được chọn dựa trên selectedTableId
                var table = context.TableFood.FirstOrDefault(t => t.id == selectedTableId);
                if (table == null)
                {
                    MessageBox.Show("Không tìm thấy thông tin bàn!");
                    return;
                }

                var bill = context.Bill.FirstOrDefault(b => b.idTable == table.id && b.status == 0);
                if (bill == null)
                {
                    MessageBox.Show("Không tìm thấy hóa đơn của bàn này!");
                    return;
                }

                // Cập nhật trạng thái và tổng tiền cho bill
                bill.status = 1;

                // Lưu thay đổi vào cơ sở dữ liệu
                context.SaveChanges();

                // Cập nhật trạng thái của bàn là "Trống"
                table.status = "Trống";
                context.SaveChanges();

                // Cập nhật màu sắc của nút bàn sau khi thanh toán
                var button = flowLayoutPanel1.Controls.OfType<Button>().FirstOrDefault(b => b.Text.StartsWith(table.name));
                if (button != null)
                {
                    button.BackColor = Color.Aqua;
                    button.Text = table.name + "\n" + "Trống";
                }

                // Làm mới danh sách món ăn và tổng tiền
                listView1.Items.Clear();
                Tong.Text = "0.00đ";
                selectedTableId = -1;

                MessageBox.Show("Thanh toán thành công!");
            }
        }

    
        private void button2_Click(object sender, EventArgs e)
        {
            if (selectedTableId < 0)
            {
                MessageBox.Show("Vui lòng chọn bàn muốn chuyển!");
                return;
            }

            int newTableId = (int)cbSwitchTable.SelectedValue;
            if (newTableId == selectedTableId)
            {
                MessageBox.Show("Vui lòng chọn một bàn khác để chuyển!");
                return;
            }

            using (Model1 db = new Model1())
            {
                TableFood oldTable = db.TableFood.FirstOrDefault(t => t.id == selectedTableId);
                TableFood newTable = db.TableFood.FirstOrDefault(t => t.id == newTableId);

                if (oldTable == null || newTable == null)
                {
                    MessageBox.Show("Lỗi khi truy xuất dữ liệu bàn!");
                    return;
                }

                if (newTable.status == "Có Người")
                {
                    MessageBox.Show("Bàn bạn muốn chuyển đến đã có người!");
                    return;
                }

                // Chuyển hóa đơn từ bàn cũ sang bàn mới
                Bill billToMove = db.Bill.FirstOrDefault(b => b.idTable == oldTable.id && b.status == 0);
                if (billToMove != null)
                {
                    billToMove.idTable = newTable.id;
                    newTable.status = "Có Người";
                    oldTable.status = "Trống";

                    db.SaveChanges();

                    MessageBox.Show("Đã chuyển bàn thành công!");

                    // Cập nhật hiển thị trên giao diện
                    Button oldTableButton = flowLayoutPanel1.Controls.OfType<Button>().FirstOrDefault(b => b.Text.StartsWith(oldTable.name));
                    if (oldTableButton != null)
                    {
                        oldTableButton.BackColor = Color.Aqua;
                        oldTableButton.Text = oldTable.name + "\n" + "Trống";
                    }

                    Button newTableButton = flowLayoutPanel1.Controls.OfType<Button>().FirstOrDefault(b => b.Text.StartsWith(newTable.name));
                    if (newTableButton != null)
                    {
                        newTableButton.BackColor = Color.LightPink;
                        newTableButton.Text = newTable.name + "\n" + "Có Người";
                    }
                }
                else
                {
                    MessageBox.Show("Không tìm thấy hóa đơn của bàn này!");
                }
            }
        }

        private void btnDiscount_Click(object sender, EventArgs e)
        {
            decimal discountPercentage = numericUpDown2.Value;

            // Tính tiền giảm giá
            decimal currentTotal = decimal.Parse(Tong.Text, NumberStyles.Currency);
            decimal discountAmount = (currentTotal * discountPercentage) / 100;
            decimal newTotal = currentTotal - discountAmount;

            // Hiển thị hộp thoại xác nhận
            string tableName = ""; // Bạn cần lấy tên bàn hiện tại ở đây
            DialogResult result = MessageBox.Show($"Bạn có chắc muốn thanh toán hóa đơn cho bàn {tableName} với tổng tiền: {newTotal.ToString("c")}?", "Xác nhận thanh toán", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                // Lưu giảm giá vào cơ sở dữ liệu 
                using (Model1 context = new Model1())
                {
                    // Tìm hóa đơn chưa thanh toán của bàn hiện tại
                    var billToUpdate = context.Bill.FirstOrDefault(b => b.idTable == selectedTableId && b.status == 0);

                    if (billToUpdate != null)
                    {
                        // Cập nhật giảm giá
                        billToUpdate.discount = (int)discountPercentage;

                        // Lưu thay đổi vào cơ sở dữ liệu
                        context.SaveChanges();
                    }
                }
                Tong.Text = newTotal.ToString("c"); // Cập nhật lại tổng tiền sau khi giảm giá
            }

        }
    }
}