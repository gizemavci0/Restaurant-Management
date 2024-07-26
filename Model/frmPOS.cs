using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Management.Model
{
    public partial class frmPOS : Form
    {
        public frmPOS()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
            frmMain frm = new frmMain();
            frm.AddControls(new frmHome());
        }

        private void frmPOS_Load(object sender, EventArgs e)
        {
            guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
            AddCategory();

            ProductPanel.Controls.Clear();
            LoadProducts();
        }

        public void b_Click(object sender , EventArgs e)
        {
            Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PCategory.ToLower().Contains(b.Text.Trim().ToLower());
            }
        }

        //sol panele buton ekleme
        private void AddCategory()
        {
            string qry = "Select * from Category";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            CategoryPanel.Controls.Clear();

            if(dt.Rows.Count > 0 )
            {
                foreach (DataRow row in dt.Rows)
                {
                    Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button();
                    b.FillColor = Color.FromArgb(50, 55, 89);
                    b.Size = new Size(180, 45);
                    b.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b.Text = row["catName"].ToString();

                    //
                    b.Click += new EventHandler(b_Click);

                    CategoryPanel.Controls.Add(b);
                }
                
            }
        }

        double total;

        //ortadaki panele ürün ekleme
        private void AddItems(int id, string name, string cat, string price, Image pImage)
        {
            var w = new ucProduct()
            {
                PName = name,
                PPrice = price,
                PCategory = cat,
                PImage = pImage,
                id = Convert.ToInt32(id),
            };

            ProductPanel.Controls.Add(w);

            w.onSelect += (ss, ee) =>
            {
                var wdg = (ucProduct)ss;
                total += Convert.ToDouble(w.PPrice);
                lblTotal.Text = total.ToString();
                foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    if (Convert.ToInt32(item.Cells["dgvid"].Value) == wdg.id)
                    {
                        item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) +1;
                        item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                                                        double.Parse(item.Cells["dgvPrice"].Value.ToString());
                        return;
                    }

                }
                guna2DataGridView1.Rows.Add(new object[] { 0, wdg.id, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                
            };
        }
        //id degerini kontrol etmeden donusumden dolayı hata alıyor
        /*
        private void LoadProducts()
        {
            string qry = "Select * from Product inner join category on catID = CategoryID";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach(DataRow item in dt.Rows) 
            {
                Byte[] imagearray = (byte[])item["pImage"];
                byte[] imageByteArray = imagearray;

                AddItems( item["pID"].ToString() , item["pName"].ToString() , item["catName"].ToString() ,
                    item["pPrice"].ToString() , Image.FromStream(new MemoryStream(imageByteArray)) );
            }
        }
        */

        private void LoadProducts()
        {
            string qry = "Select * from Product inner join category on catID = CategoryID";
            SqlCommand cmd = new SqlCommand(qry, MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            foreach (DataRow item in dt.Rows)
            {
                Byte[] imagearray = (byte[])item["pImage"];
                byte[] imageByteArray = imagearray;

                // id değerinin gecerli bir deger olup olmadıgı kontrol edildi
                if (int.TryParse(item["pID"].ToString(), out int productId))
                {
                    AddItems(productId, item["pName"].ToString(), item["catName"].ToString(),
                             item["pPrice"].ToString(), Image.FromStream(new MemoryStream(imageByteArray)));
                }
                else
                {
                    // Geçersiz id durumunda uyarı verildi
                    MessageBox.Show($"Ürün ID'si geçersiz: {item["pID"]}");
                }
            }
        }


        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (ucProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }

        private void guna2DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            int count = 0;

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            lblTable.Text = "";
            lblWaiter.Text = "";
            lblWaiter.Visible = false;
            lblTable.Visible = false;
            lblTotal.Text = "0.00";
            total = 0d;
            guna2DataGridView1.Rows.Clear();
        }
    }
}
