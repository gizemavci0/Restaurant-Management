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
using System.Xml.Linq;

namespace Restaurant_Management.Model
{
    public partial class frmDocument : Form
    {
        public frmDocument()
        {
            InitializeComponent();
        }

        public string qry = "select MainID,aDate,aTime, TableName, WaiterName,status,orderType,total,received,driverID,CustName,CustPhone from tblMain";


        public int id = 0;
        public bool control = false;

        //combobox a veri cekerek değer ekleme
        private void Add_Value_combobox()
        {
            //table degerlerini bu bloktan aldm
            string qry1 = "SELECT * FROM tables";
            SqlCommand cmd = new SqlCommand(qry1,MainClass.con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            cbTable.Items.Add("");
            
            foreach (DataRow row in dt.Rows)
            {
                cbTable.Items.Add(row["tname"].ToString());
            }

            //waiter degerini bu bloktan aldım
            string qry2 = "SELECT * FROM Staff where sRole = 'Waiter'";
            SqlCommand cmd2 = new SqlCommand(qry2, MainClass.con);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dt2 = new DataTable();
            da2.Fill(dt2);
            cbWaiter.Items.Add("");

            foreach (DataRow row in dt2.Rows)
            {
                cbWaiter.Items.Add(row["sName"].ToString());
            }

        }

        private void frmDocument_Load(object sender, EventArgs e)
        {
            Add_Value_combobox();
            LoadData(qry);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            control = false;
            string qry = Control_Filter();
            LoadData(qry);
        }

        private string Control_Filter()
        {

            string qry = "select MainID,aDate,aTime, TableName, WaiterName,status,orderType,total,received,driverID,CustName,CustPhone from tblMain";
            
            if(cbTable.Text.ToString() != "")
            {
                if (control)
                {
                    qry += " and TableName = '" + cbTable.Text.ToString()+"'";
                }
                else
                {
                    qry += " Where TableName = '" + cbTable.Text.ToString() + "'";

                }
                control = true;
            }
            if (cbStatus.Text.ToString() != "")
            {
                if (control)
                {
                    qry += " and status = '" + cbStatus.Text.ToString() + "'";
                }
                else
                {
                    qry += " Where status = '" + cbStatus.Text.ToString() + "'";

                }
                control = true;
            }

            if (cbOrderType.Text.ToString() != "")
            {
                if (control)
                {
                    qry += " and ordertype = '" + cbOrderType.Text.ToString() + "'";
                }
                else
                {
                    qry += " Where ordertype = '" + cbOrderType.Text.ToString() + "'";

                }
                control = true;
            }

            if (cbWaiter.Text.ToString() != "")
            {
                if (control)
                {
                    qry += " and waiterName = '" + cbWaiter.Text.ToString() + "'";
                }
                else
                {
                    qry += " Where waiterName = '" + cbWaiter.Text.ToString() + "'";

                }
                control = true;
            }

            if (cbTotal.Text.ToString() != "")
            {
                if (control)
                {
                    qry += " and total ";
                }
                else
                {
                    qry += " Where total ";
                    control = true;

                }
                if(cbTotal.Text.ToString() == "Küçüktür")
                {
                    qry += " < '" + txtTotal.Text + "'";
                }
                else if (cbTotal.Text.ToString() == "Küçük Eşittir")
                {
                    qry += " <= '" + txtTotal.Text + "'";
                }
                else if (cbTotal.Text.ToString() == "Eşittir" || cbTotal.Text.ToString() == "")
                {
                    qry += " = '" + txtTotal.Text + "'";
                }
                else if (cbTotal.Text.ToString() == "Büyük Eşittir")
                {
                    qry += " >= '" + txtTotal.Text + "'";
                }
                else
                {
                    qry += " > '" + txtTotal.Text + "'";
                }   

            }

            if (txtName.Text.ToString() != "")
            {
                if (control)
                {
                    qry += " and CustName = '" + txtName.Text.ToString() + "'";
                }
                else
                {
                    qry += " Where CustName = '" + txtName.Text.ToString() + "'";

                }
                control = true;
            }

            if (txtPhone.Text.ToString() != "")
            {
                if (control)
                {
                    qry += " and CustPhone = '" + txtPhone.Text.ToString() + "'";
                }
                else
                {
                    qry += " Where CustPhone = '" + txtPhone.Text.ToString() + "'";
                    control = true;

                }
                
            }

            if (dt1.Value != null && dt2.Value != null)
            {
                // Tarihleri uygun formata dönüştürün
                string startDate = dt1.Value.ToString("yyyy-MM-dd");
                string endDate = dt2.Value.ToString("yyyy-MM-dd");

                if (control)
                {
                    qry += " AND aDate BETWEEN '"+ dt1.Value.ToString("yyyy-MM-dd") + "' AND '"+ dt2.Value.ToString("yyyy-MM-dd")+"'";
                }
                else
                {
                    qry += " WHERE aDate BETWEEN '"+ dt1.Value.ToString("yyyy-MM-dd")+ "' AND '"+ dt2.Value.ToString("yyyy-MM-dd")+"'";
                    control = true;
                }

                // SQL komutunuza parametreleri ekleyin
                SqlCommand command = new SqlCommand(qry, MainClass.con);
                command.Parameters.AddWithValue("@startDate", startDate);
                command.Parameters.AddWithValue("@endDate", endDate);
            }


            return qry;
        }

        private void LoadData(string qry)
        {
            ListBox lb = new ListBox();
            lb.Items.Add(dgvid);
            lb.Items.Add(dgvDate);
            lb.Items.Add(dgvTime);
            lb.Items.Add(dgvtable);
            lb.Items.Add(dgvWaiter);
            lb.Items.Add(dgvStatus);
            lb.Items.Add(dgvType);
            lb.Items.Add(dgvTotal);
            lb.Items.Add(dgvReceived);
            lb.Items.Add(dgvDriver);
            lb.Items.Add(dgvCusName);
            lb.Items.Add(dgvCustPhone);



            MainClass.LoadData(qry, guna2DataGridView1, lb);
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
    }
}
