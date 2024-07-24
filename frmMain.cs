using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management.View;

namespace Restaurant_Management
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }
        
        //normalde staticti ama CenterPanel degişkenine ulasılmadıgından static i kaldırdım
        public void AddControls(Form f)
        {
            CenterPanel.Controls.Clear();
            f.Dock = DockStyle.Fill; 
            f.TopLevel = false;
            CenterPanel.Controls.Add(f);
            f.Show();
        }


        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            lblUser.Text = MainClass.USER;
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            AddControls(new frmHome());
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            AddControls(new frmPOS());
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            AddControls(new frmSetting());
        }

        private void btnKitchen_Click(object sender, EventArgs e)
        {
            AddControls(new frmKitchen());
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            AddControls(new frmStaff());
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            AddControls(new frmTable());
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            AddControls(new frmProduct());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            AddControls(new frmCategoryView());
        }
    }
}
