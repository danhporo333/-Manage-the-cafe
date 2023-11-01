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
using System.Data.Entity;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Remoting.Contexts;

namespace QL_Quán_Cafe
{
    public partial class fAdmin : Form
    {
        private Model1 context;
        public fAdmin()
        {
            InitializeComponent();
            context = new Model1();
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {
            //dtgvAccount.Columns[1].Visible = false;//dùng để ẩn cột mà mình muốn
            Model1 context = new Model1();
            var account = context.Account.ToList();
            var foodcategory = context.FoodCategory.ToList();
            var food = context.Food.ToList();
            var tf = context.TableFood.ToList();
            
            dtgvAccount.DataSource = account;
            dtgvCategory.DataSource = foodcategory;
            dtgvCategory.Columns[2].Visible = false;
            
            //đổ dữ liệu từ bảng FoodCategory vào combobox
            cbFoodCategory.DataSource = foodcategory;
            cbFoodCategory.DisplayMember = "name";
            cbFoodCategory.ValueMember = "id";
            
            //đổ dữ liệu và datagirdview
            dtgvFood.Rows.Clear();
            foreach(var p in food)
            {
                int newRow = dtgvFood.Rows.Add();
                dtgvFood.Rows[newRow].Cells[0].Value = p.id;
                dtgvFood.Rows[newRow].Cells[1].Value = p.name;
                dtgvFood.Rows[newRow].Cells[2].Value = p.FoodCategory.id + " : " + p.FoodCategory.name;
                dtgvFood.Rows[newRow].Cells[3].Value = p.price;
            }
        }

        private void btnxembill_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime startDate = dtpkBatDau.Value.Date;
                DateTime endDate = dtpketthuc.Value.Date;

                using (Model1 context = new Model1())
                {
                    // Query the Bill table based on the selected date range
                    var bills = context.Bill.Where(b => b.DateCheckOut >= startDate && b.DateCheckOut <= endDate && b.status == 1).ToList();

                    // Display the results in the DataGridView
                    dtgvBilll.DataSource = bills;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                Food f = new Food();
                f.id = int.Parse(txbFoodID.Text);
                f.name = txbFoodName.Text;
                f.idCategory = int.Parse(cbFoodCategory.SelectedValue.ToString());
                f.price = float.Parse(FoodPrice.Text);
                context.Food.Add(f);
                MessageBox.Show("thêm món thành công");
                context.SaveChanges();
                fAdmin_Load(sender, e);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteFood_Click(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                int temp = int.Parse(txbFoodID.Text);
                Food find = context.Food.FirstOrDefault(p => p.id == temp);
                if (find != null)
                {
                    context.Food.Remove(find);
                    context.SaveChanges();
                    MessageBox.Show("xóa thành công");
                    fAdmin_Load(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dtgvFood_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string ids = dtgvFood.Rows[e.RowIndex].Cells[0].Value.ToString();
            Model1 context = new Model1();
            Food find = context.Food.FirstOrDefault(p => p.id.ToString() == ids);
            if(find != null)
            {
                txbFoodID.Text = ids;
                txbFoodName.Text = find.name;
                cbFoodCategory.SelectedValue = find.FoodCategory.id;
                FoodPrice.Text = find.price.ToString();
            }
        }

        private void btnEditFood_Click(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                int temp = int.Parse(txbFoodID.Text);
                Food find = context.Food.FirstOrDefault(p => p.id == temp);
                if (find != null)
                {
                    find.name = txbFoodName.Text;
                    find.price = float.Parse(FoodPrice.Text);
                    find.idCategory = int.Parse(cbFoodCategory.SelectedValue.ToString());

                    MessageBox.Show("cập nhật thành công");
                    context.SaveChanges();
                    fAdmin_Load(sender, e);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSearchFood_Click(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                string foodNameToSearch = txbSearchFoodName.Text;

                // Tìm danh sách các món ăn trùng với tên
                List<Food> matchingFoods = context.Food.Where(p => p.name == foodNameToSearch).ToList();

                // Gán danh sách món ăn vào DataSource của DataGridView
                dtgvFood.Rows.Clear();
                foreach (var p in matchingFoods)
                {
                    int newRow = dtgvFood.Rows.Add();
                    dtgvFood.Rows[newRow].Cells[0].Value = p.id;
                    dtgvFood.Rows[newRow].Cells[1].Value = p.name;
                    dtgvFood.Rows[newRow].Cells[2].Value = p.FoodCategory.id + " : " + p.FoodCategory.name;
                    dtgvFood.Rows[newRow].Cells[3].Value = p.price;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //chuyển sang phần danh mục 
        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                FoodCategory f = new FoodCategory();
                f.name = textBox1.Text;
                context.FoodCategory.Add(f);
                MessageBox.Show("thêm thành công");
                context.SaveChanges();
                fAdmin_Load(sender, e);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            try
            {
                Model1 context = new Model1();
                FoodCategory find = context.FoodCategory.FirstOrDefault(p => p.name == textBox1.Text);
                if (find != null)
                {
                    context.FoodCategory.Remove(find);
                    context.SaveChanges();
                    MessageBox.Show("xóa thành công");
                    fAdmin_Load(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnEditCategory_Click(object sender, EventArgs e)
        {
            try
            {

                using (Model1 context = new Model1())
                {
                    // Giả định bạn đang tìm một FoodCategory dựa trên mã của bạn
                    var find = context.FoodCategory.FirstOrDefault(p => p.id.ToString() == txbCategoryID.Text);

                    if (find != null)
                    { 
                        find.name = textBox1.Text;

                        context.SaveChanges();
                        MessageBox.Show("Cập nhật thành công!");
                        fAdmin_Load(sender, e);
                    }
                }
            }
            catch (Exception ex)
            { 
                MessageBox.Show(ex.Message);
            }
        }


        private void dtgvCategory_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            { 
                string ids = dtgvCategory.Rows[e.RowIndex].Cells[0].Value.ToString();

                using (Model1 context = new Model1())
                {
                    // Đảm bảo rằng tên biến phản ánh đúng loại đối tượng
                    var findCategory = context.FoodCategory.FirstOrDefault(p => p.id.ToString() == ids);

                    if (findCategory != null)
                    {
                        txbCategoryID.Text = ids;
                        textBox1.Text = findCategory.name;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi. Vui lòng thử lại.");
            }
        }
    }
}
