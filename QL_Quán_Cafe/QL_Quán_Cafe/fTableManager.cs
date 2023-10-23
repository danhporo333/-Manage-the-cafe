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
    }
}
