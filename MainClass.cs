using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using System.Windows.Forms;
using System.Web.ModelBinding;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Channels;
using System.Drawing;

namespace Restaurant_Management
{
    internal class MainClass
    {
        //sql bağlantısı saglandı

        //public static readonly string con_string = "Data Source =localhost; Initial Catalog=Restaruant-Management;Persist Security Info-True;";
        public static readonly string con_string = "Data Source=localhost;Initial Catalog=Restaruant-Management;Integrated Security=True";

        //con_string degiskeni ile baglantı sorulur
        public static SqlConnection con = new SqlConnection(con_string);


        //bu kısımda kullanıcının gırdıgı kullanıcı adı ve sıfresı kontrol edılır
        public static bool IsValidUser (string user, string password)
        {
            bool isValid = false;

            string qry = @"Select * from users where username ='"+user+"' and userpass = '"+password+"'";
            SqlCommand cmd = new SqlCommand(qry, con);

            DataTable dt = new DataTable(); //verileri tutmak icin DataTable nesnesi olsuturuldu
            SqlDataAdapter da = new SqlDataAdapter(cmd); //veri almak için nesne olusturuldu
            da.Fill(dt);

            if(dt.Rows.Count > 0 )
            {
                //eger tablo dolmussa yanı degerler dogru ıse true olarak ayarlanır
                isValid = true;
                USER = dt.Rows[0]["uName"].ToString();
            }

            return isValid;
        }

        public static string user;

        public static string USER
        {
            get { return user; }
            private set { user = value; }
        }

        //SQL komutlatı calıstırma 
        public static int SQl(string qry, Hashtable ht)
        {
            int res = 0;

            try
            {
                SqlCommand cmd = new SqlCommand(qry,con);
                cmd.CommandType = CommandType.Text;

                foreach (DictionaryEntry item in ht)
                {
                    cmd.Parameters.AddWithValue(item.Key.ToString(), item.Value);
                }
                if(con.State == ConnectionState.Closed) { con.Open(); }
                res = cmd.ExecuteNonQuery();
                if (con.State == ConnectionState.Open) { con.Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
            }

            return res;

        }

        //Veri yukleme fonksıyonu
        public static void LoadData(string qry, DataGridView gv, ListBox lb)
        {
            gv.CellFormatting += new DataGridViewCellFormattingEventHandler(gv_CellFormatting);

            try
            {
                SqlCommand cmd = new SqlCommand(qry,con);
                cmd.CommandType = CommandType.Text; //komut turumuz: metın 
                SqlDataAdapter da = new SqlDataAdapter(cmd); //sql komutu ıle verı almak için olusturulur
                DataTable dt = new DataTable(); //verileri tutmak ıcın datatable nesnesi olusturuldu
                da.Fill(dt); //veri alma işlemi


                //listbox içerisindeki her bir oge icin dongu ıslemı baslar
                for(int i = 0; i < lb.Items.Count ; i++)
                {
                    string colName1 = ((DataGridViewColumn)lb.Items[i]).Name;
                    gv.Columns[colName1].DataPropertyName = dt.Columns[i].ToString();
                }
                gv.DataSource = dt;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                con.Close();
                
            }
        }

        //Hücre Biçimlendirme fonksiyonu
        public static void gv_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Guna.UI2.WinForms.Guna2DataGridView gv = (Guna.UI2.WinForms.Guna2DataGridView)sender;
            int count = 0; //satır sayısını tutmak ıcın bır degısken

            foreach(DataGridViewRow row in gv.Rows)
            {
                count++;
                row.Cells[0].Value = count;
            }
        }

        //Arka Planı bulanıklaştırma fonksiyonu
        public static void BlurBackground(Form Model)
        {
            Form Background = new Form();
            using (Model)
            {
                Background.StartPosition = FormStartPosition.Manual;
                Background.FormBorderStyle = FormBorderStyle.None;
                Background.Opacity = 0.5d;
                Background.BackColor = Color.Black;
                Background.Size = frmMain.Instance.Size;
                Background.Location = frmMain.Instance.Location;
                Background.ShowInTaskbar = false;
                Background.Show();
                Model.Owner = Background;
                Model.ShowDialog(Background);
                Background.Dispose();

            }
        }

        //combox fonksiyonu
        public static void CBFill (string qry, ComboBox cb)
        {
            SqlCommand cmd = new SqlCommand(qry, con);
            cmd.CommandType = CommandType.Text;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            cb.DisplayMember = "name";
            cb.ValueMember = "id";
            cb.DataSource = dt;
            cb.SelectedIndex = -1;
        }
    }
}
