﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Restaurant_Management
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if(MainClass.IsValidUser(txtUser.Text, txtPass.Text) == false)
            {
                //kullanıcı adı veya sıfre bılgısı yanlıs girilme durumunda hata mesajı doner
                guna2MessageDialog1.Show("invalid username or password");
                return;
            }
            else
            {
                this.Hide(); // frmLogin ekranini gizler. <Başka işleme geçmek için>
                frmMain frm = new frmMain(); //bir sonraki form olan ana ekran formun nesnei olusturulu 
                frm.Show(); // yeni form gosterildi
            }
        }
    }
}

//