using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using Krypton.Toolkit;
using System.Data.SqlClient;
using New_Availment_System.Properties;

namespace New_Availment_System
{
    public partial class frmMain : KryptonForm
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (GlobalVar.SLevel == "NA")
            {
                menuStrip1.Visible = false;
            }
            timer1.Start();
        }

        // FUNCTIONS

        void Insert1()
        {
            {
                using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
                {
                    string strQry = "INSERT INTO tblAvailment (EmpCode, LName, FName, MName, Department, Status, AvailDate, Amount) VALUES (@EmpCode, @LName, @FName, @MName, @Department, @Status, @AvailDate, @Amount)";
                    SqlCommand sqlCmd = new SqlCommand(strQry, sqlConnection);
                    sqlConnection.Open();

                    sqlCmd.Parameters.Add("@EmpCode", SqlDbType.NVarChar).Value = txtEmpCode.Text;
                    sqlCmd.Parameters.Add("@LName", SqlDbType.NVarChar).Value = txtLastname.Text;
                    sqlCmd.Parameters.Add("@FName", SqlDbType.NVarChar).Value = txtFirstname.Text;
                    sqlCmd.Parameters.Add("@MName", SqlDbType.NVarChar).Value = txtMiddlename.Text;
                    sqlCmd.Parameters.Add("@Department", SqlDbType.NVarChar).Value = txtDepartment.Text;
                    sqlCmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = txtStatus.Text;
                    sqlCmd.Parameters.Add("@AvailDate", SqlDbType.NVarChar).Value = DateTime.Now.ToString("M/d/yyyy");
                    sqlCmd.Parameters.Add("@Amount", SqlDbType.NVarChar).Value = lblAmount.Text;


                    sqlCmd.ExecuteNonQuery();
                    sqlConnection.Close();

                    label9.Text = "Successfully availed the amount of Php. " + lblAmount.Text;
                    label9.Visible = true;
                    pictureBox1.BackgroundImage = Properties.Resources.BARCODEG;
                    pictureBox2.Image = Properties.Resources.Untitled_design__1_;
                    pictureBox2.Visible = true;
                    txtBarcode.Text = string.Empty;
                    label9.ForeColor = Color.Green;
                    txtBarcode.Focus();
                }

            }
        }

        string ACode = string.Empty;
        string ADate = string.Empty;
        void pointChecked()
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {

                SqlCommand sqlCmd = new SqlCommand("select * from [tblAvailment] where AvailDate ='" + DateTime.Now.ToString("M/d/yyyy") + "' and EmpCode = '" + txtBarcode.Text + "'", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    ADate = (sqlReader["AvailDate"].ToString());
                    ACode = (sqlReader["EmpCode"].ToString());
                }


                if (ADate == DateTime.Now.ToString("M/d/yyyy"))
                {
                    if (txtBarcode.Text == ACode)
                    {
                        label9.Text = "Employee Subsidy Already Availed!!!";
                        pictureBox1.BackgroundImage = Properties.Resources.BARCODER;
                        pictureBox2.Visible = true;
                        pictureBox2.Image = Properties.Resources.Untitled_design__2_;
                        label9.ForeColor = Color.Red;
                        label9.Visible = true;
                        txtBarcode.Text = String.Empty;
                        txtBarcode.Focus();
                    }
                    else
                    {
                        Insert1();
                    }
                }
                else
                {
                    Insert1();
                }
            }
        }

        void PIDCheck()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                SqlCommand command = new SqlCommand("select * from [tblEmployee] where Barcode ='" + txtBarcode.Text + "' ", connection);
                connection.Open();

                SqlDataReader read = command.ExecuteReader();

                while (read.Read())
                {
                    txtEmpCode.Text = (read["EmpCode"].ToString());
                    txtLastname.Text = (read["LName"].ToString());
                    txtFirstname.Text = (read["FName"].ToString());
                    txtMiddlename.Text = (read["MName"].ToString());
                    txtDepartment.Text = (read["Department"].ToString());
                    txtStatus.Text = (read["Status"].ToString());
                    lblAmount.Text = (read["Subsidy"].ToString());
                }
            }
        }

        void PIDexist()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                connection.Open();
                bool exists = false;
                SqlCommand command = new SqlCommand("select count(*) from [tblEmployee] where Barcode ='" + txtBarcode.Text + "' ", connection);

                {
                    exists = (int)command.ExecuteScalar() > 0;
                }

                if (exists)
                {

                    PIDCheck();
                    pointChecked();

                }
                else
                {
                    label9.Text = "NO SUCH DATA IN DATABASE!!";
                    pictureBox1.BackgroundImage = Properties.Resources.BARCODE1;
                    pictureBox2.Visible = false;
                    label9.ForeColor = Color.Red;
                    txtFirstname.Text = "------";
                    txtMiddlename.Text = "------";
                    txtLastname.Text = "------";
                    txtDepartment.Text = "------";
                    txtEmpCode.Text = "------";
                    txtStatus.Text = "------";
                    lblAmount.Text = "------";
                }
                txtBarcode.Focus();
                txtBarcode.Text = string.Empty;
            }
        }

        // EVENTS
        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTime.Text = DateTime.Now.ToString("hh:mm tt");
            lblDate.Text = DateTime.Now.ToString("M/dd/yyyy");
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (String.IsNullOrWhiteSpace(txtBarcode.Text) || String.IsNullOrEmpty(txtBarcode.Text))
                {
                    MessageBox.Show("Error!", "PID Barcode Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtBarcode.Text = String.Empty;

                }
                else
                {
                    PIDexist();
                }
            }
        }

        private void generalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmGeneral frm1 = new frmGeneral();
            frm1.Show();
        }

        private void gToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmGForm frm1 = new frmGForm();
            frm1.Show();
        }

        private void saveRecordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewSaveRecords frm1 = new NewSaveRecords();
            frm1.Show();
        }

        private void unswipedReportFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReport frm1 = new frmReport();
            frm1.Show();
        }

        private void unswipedTotalReportFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmURF frm1 = new frmURF();
            frm1.Show();
        }

        private void employeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EmployeeCRUD frm1 = new EmployeeCRUD();
            frm1.Show();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserS frm1 = new frmUserS();
            frm1.Show();
        }

        private void logOffToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Logout this User?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                frmLogIn frm1 = new frmLogIn();
                frm1.Show();
                this.Close();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Close this User?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.Close();
            }
        }
    }
}
