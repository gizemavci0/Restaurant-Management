using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management.Model;
using Restaurant_Management.View;

namespace Restaurant_Management
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        static frmMain _obj;

        public static frmMain Instance
        {
            get { if( _obj == null ) { _obj = new frmMain(); } return _obj; }
        }
        
        public void AddControls(Form f)
        {
            CenterPanel.Controls.Clear(); //yeni panel gelmeden once paneli temizleme islemi
            f.Dock = DockStyle.Fill; //yeni gelen formun panele gore boyut olarak ayarlanmasını saglar
            f.TopLevel = false; //bu islem formun panel ıcınde acılmasına ızın verdırır
            CenterPanel.Controls.Add(f); //yenı form panele eklenır
            f.Show(); //form gosterıme hazır
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        //form calıstıgında kullanıcı adını ekrana gırmeyı saglar.
        private void frmMain_Load(object sender, EventArgs e)
        {
            lblUser.Text = MainClass.USER;
            _obj = this;
        }

        //asağıdakı kısımda butonlara basıldıgında olabılecek ıhtımallerı ve acılacak formlar ayarlar
        private void btnHome_Click(object sender, EventArgs e)
        {
            AddControls(new frmHome());
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            frmPOS frm = new frmPOS();
            frm.Show();
        }

        private void btnKitchen_Click(object sender, EventArgs e)
        {
            AddControls(new frmKitchenView());
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            AddControls(new frmStaffView());
        }

        private void btnTable_Click(object sender, EventArgs e)
        {
            AddControls(new frmTableView());
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            AddControls(new frmProductView());
        }

        private void btnCategory_Click(object sender, EventArgs e)
        {
            AddControls(new frmCategoryView());
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AddControls(new frmDocumentView());
        }
    }
}
