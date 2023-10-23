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
    public partial class fAdmin : Form
    {
        public fAdmin()
        {
            InitializeComponent();
        }

        private void fAdmin_Load(object sender, EventArgs e)
        {
            Model1 context = new Model1();
            var account = context.Account.ToList();
            
            dtgvAccount.DataSource = account;
            //dtgvAccount.Columns[1].Visible = false;//dùng để ẩn cột mà mình muốn
            
        }
    }
}
