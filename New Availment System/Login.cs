using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Krypton.Toolkit;
using System.Data.SqlClient;

namespace New_Availment_System
{
    public partial class frmLogIn : KryptonForm
    {
        public frmLogIn()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void login_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection("server=SVPH_DBSERVER4;database=TSPHAvailment;Integrated Security = true");
                SqlCommand com = new SqlCommand("SELECT * FROM tblSecurity WHERE Username=@user and Password=@pass", con);
                con.Open();
                string strQry = ("Select * From tblSecurity where Username ='" + txtUserName.Text + "' and Password = '" + txtPassword.Text + "'");
                SqlCommand scmd = new SqlCommand(strQry, con);
                using (var reader = scmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        GlobalVar.FullName = reader["EmpName"].ToString();
                        GlobalVar.Dept = reader["Department"].ToString();
                        GlobalVar.SLevel = reader["SecurityLevel"].ToString();

                    }
                }


                com.Parameters.AddWithValue("@user", txtUserName.Text);
                com.Parameters.AddWithValue("@pass", txtPassword.Text);

                SqlDataReader Dr = com.ExecuteReader();
                if (Dr.HasRows == true)
                {

                    frmMain ss = new frmMain();
                    ss.Show();
                    this.Hide();
                }

                else
                {
                    MessageBox.Show("Please check your Username & Password", "UserPass Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void loginwopass_Click(object sender, EventArgs e)
        {
            GlobalVar.SLevel = "NA";
            frmMain ss = new frmMain();
            ss.Show();
            this.Hide();
        }
    }
}
