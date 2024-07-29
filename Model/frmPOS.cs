    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.Data.SqlClient;
    using System.Drawing;
    using System.IO;
    using System.Linq;
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

            public int MainID = 0;
            public string OrderType;

            private void btnExit_Click(object sender, EventArgs e)
            {
                this.Close();
            }

            private void frmPOS_Load(object sender, EventArgs e)
            {
                guna2DataGridView1.BorderStyle = BorderStyle.FixedSingle;
                AddCategory();

                ProductPanel.Controls.Clear();
                LoadProducts();
            }

            public void b_Click(object sender, EventArgs e)
            {
                Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)sender;
                if (b.Text == "All Categories")
                {
                    txtSearch.Text = "1";
                    txtSearch.Text = "";
                    return;
                }
                foreach (var item in ProductPanel.Controls)
                {
                    var pro = (ucProduct)item;
                    pro.Visible = pro.PCategory.ToLower().Contains(b.Text.Trim().ToLower());
                
                }
            }

            // sol panele buton ekleme
            private void AddCategory()
            {
                string qry = "Select * from Category";
                using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    CategoryPanel.Controls.Clear();

                    Guna.UI2.WinForms.Guna2Button b2 = new Guna.UI2.WinForms.Guna2Button();
                    b2.FillColor = Color.FromArgb(50, 55, 89);
                    b2.Size = new Size(180, 45);
                    b2.ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton;
                    b2.Text = "All Categories";
                    b2.Click += new EventHandler(b_Click);
                    CategoryPanel.Controls.Add(b2);


                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            Guna.UI2.WinForms.Guna2Button b = new Guna.UI2.WinForms.Guna2Button
                            {
                                FillColor = Color.FromArgb(50, 55, 89),
                                Size = new Size(180, 45),
                                ButtonMode = Guna.UI2.WinForms.Enums.ButtonMode.RadioButton,
                                Text = row["catName"].ToString()
                            };

                            b.Click += new EventHandler(b_Click);

                            CategoryPanel.Controls.Add(b);
                        }
                    }
                }
            }

           

            // ortadaki panele ürün ekleme
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
                    foreach (DataGridViewRow item in guna2DataGridView1.Rows)
                    {
                        if (Convert.ToInt32(item.Cells["dgvid"].Value) == wdg.id)
                        {
                            item.Cells["dgvQty"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) + 1;
                            item.Cells["dgvAmount"].Value = int.Parse(item.Cells["dgvQty"].Value.ToString()) *
                                                            double.Parse(item.Cells["dgvPrice"].Value.ToString());
                            GetTotal();
                            return;
                        }
                    }
                    guna2DataGridView1.Rows.Add(new object[] { 0, wdg.id, wdg.PName, 1, wdg.PPrice, wdg.PPrice });
                    GetTotal();
                };
            }


            private void LoadProducts()
            {
            
                string qry = "Select * from Product inner join category on catID = CategoryID";
            
                    using (SqlCommand cmd = new SqlCommand(qry, MainClass.con))
                    {
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
                                AddItems(productId, item["pName"].ToString(), item["catName"].ToString(), item["pPrice"].ToString(), Image.FromStream(new MemoryStream(imagearray)));
                            }
                            else
                            {
                                // Geçersiz id durumunda uyarı verildi
                                MessageBox.Show($"Ürün ID'si geçersiz: {item["pID"]}");
                            }
                        }
                    }
                
            }

            private void txtSearch_TextChanged(object sender, EventArgs e)
            {
                foreach (var item in CategoryPanel.Controls)
                {
                    if (item is Guna.UI2.WinForms.Guna2Button)
                    {
                        Guna.UI2.WinForms.Guna2Button b = (Guna.UI2.WinForms.Guna2Button)item;
                        b.Checked = false;
                    }
                }
                foreach (var item in ProductPanel.Controls)
                {
                    /*if (item is ucProduct pro)
                    {
                        pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
                    
                    }*/
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

            public void GetTotal()
            {
                double tot = 0;
                lblTotal.Text = "";
                foreach(DataGridViewRow item in guna2DataGridView1.Rows)
                {
                    tot += double.Parse(item.Cells["dgvAmount"].Value.ToString());
                }
            lblTotal.Text = tot.ToString("N2");
            }


            private void btnNew_Click(object sender, EventArgs e)
            {
                lblTable.Text = "";
                lblWaiter.Text = "";
                lblWaiter.Visible = false;
                lblTable.Visible = false;
                lblTotal.Text = "0.00";
                guna2DataGridView1.Rows.Clear();
                MainID = 0;
            }

            private void btnDelivery_Click(object sender, EventArgs e)
            {
                lblTable.Text = "";
                lblWaiter.Text = "";
                lblWaiter.Visible = false;
                lblTable.Visible = false;
                OrderType = "Delivery";
            }

            private void btnTake_Click(object sender, EventArgs e)
            {
                lblTable.Text = "";
                lblWaiter.Text = "";
                lblWaiter.Visible = false;
                lblTable.Visible = false;
                OrderType = "Take Away";
            }

            private void btnDin_Click(object sender, EventArgs e)
            {
                frmTableSelect frm = new frmTableSelect();
                MainClass.BlurBackground(frm);
                if (frm.TableName != "")
                {
                    lblTable.Text = frm.TableName;
                    lblTable.Visible = true;
                }
                else
                {
                    lblTable.Text = "";
                    lblTable.Visible = false;
                }
                

                frmWaiterSelect frm2 = new frmWaiterSelect();
                MainClass.BlurBackground(frm2);

                if (frm2.waiterName != "")
                {
                    lblWaiter.Text = frm2.waiterName;
                    lblWaiter.Visible = true;
                }
                else
                {
                    lblWaiter.Text = "";
                    lblWaiter.Visible = false;
                }
            }

            private void btnKOT_Click(object sender, EventArgs e)
            {
                
            }
        }
    }
