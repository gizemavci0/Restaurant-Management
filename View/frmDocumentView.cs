﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Restaurant_Management.Model;

namespace Restaurant_Management.View
{
    public partial class frmDocumentView : Form
    {
        public frmDocumentView()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmDocument frm = new frmDocument();
            frm.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmDocumentList frm = new frmDocumentList();
            frm.Show();
        }
    }
}
