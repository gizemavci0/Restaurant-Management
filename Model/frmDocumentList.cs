using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Management.Model
{
    public partial class frmDocumentList : Form
    {
        public frmDocumentList()
        {
            InitializeComponent();
        }

        private void frmDocumentList_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            string qry = "select MainID,aDate,aTime, TableName, WaiterName,status,orderType,total,received,driverID,CustName,CustPhone from tblMain";
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
