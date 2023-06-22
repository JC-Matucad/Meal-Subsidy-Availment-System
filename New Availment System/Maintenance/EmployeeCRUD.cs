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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Microsoft.Office.Interop.Excel;
using Application = System.Windows.Forms.Application;

namespace New_Availment_System
{
    public partial class EmployeeCRUD : Krypton.Toolkit.KryptonForm
    {
        public EmployeeCRUD()
        {
            InitializeComponent();
        }

        private void EmployeeCRUD_Load(object sender, EventArgs e)
        {
            DataListViews();
            Departments();
            Sections();
            Statuss();
            Positions();
        }

        void DataListViews()
        {
            listView1.Items.Clear();
            using (SqlConnection connection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand SelectCommand = new SqlCommand("select * from tblEmployee", connection);
                SqlDataReader reader = SelectCommand.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem lv = new ListViewItem(reader["ID"].ToString());
                    lv.SubItems.Add(reader["EmpCode"].ToString());
                    lv.SubItems.Add(reader["Barcode"].ToString());
                    lv.SubItems.Add(reader["LName"].ToString());
                    lv.SubItems.Add(reader["FName"].ToString());
                    lv.SubItems.Add(reader["MName"].ToString());
                    lv.SubItems.Add(reader["Sex"].ToString());
                    lv.SubItems.Add(reader["Department"].ToString());
                    lv.SubItems.Add(reader["Section"].ToString());
                    lv.SubItems.Add(reader["Status"].ToString());
                    lv.SubItems.Add(reader["Subsidy"].ToString());
                    lv.SubItems.Add(reader["Position"].ToString());
                    lv.SubItems.Add(reader["DateHired"].ToString());
                    listView1.Items.Add(lv);
                }
                connection.Close();
            }
        }


        void Departments()
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT Department FROM tblDepartment ", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    cbDept.Items.Add(sqlReader["Department"].ToString());
                }
                sqlReader.Close();
            }
        }

        void Sections()
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT Section FROM tblSection ", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    cbSect.Items.Add(sqlReader["Section"].ToString());
                }
                sqlReader.Close();
            }
        }

        void Statuss()
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT * FROM tblStatus ", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    cbStatus.Items.Add(sqlReader["Status"].ToString());
                    cbSubs.Items.Add(sqlReader["Subsidy"].ToString());
                }
                sqlReader.Close();
            }
        }

        void Positions()
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT Position FROM tblPosition ", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    cbPosition.Items.Add(sqlReader["Position"].ToString());
                }
                sqlReader.Close();
            }
        }

        void enabletxtbox()
        {
            txtECode.Enabled = true;
            txtFName.Enabled = true;
            txtLName.Enabled = true;
            txtMName.Enabled = true;
            cbDept.Enabled = true;
            cbPosition.Enabled = true;
            cbSect.Enabled = true;
            cbSex.Enabled = true;
            cbStatus.Enabled = true;
            cbSubs.Enabled = true;
            dtpDHired.Enabled = true;
        }
        void pointChecked()
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                string ECode = string.Empty;
                string BCode = string.Empty;
                string FCode = string.Empty;
                string LCode = string.Empty;
                string MCode = string.Empty;
                SqlCommand sqlCmd = new SqlCommand("select * from [tblEmployee] where EmpCode ='" + txtECode.Text + "' and Barcode = '" + txtECode.Text + "'", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    ECode = (sqlReader["EmpCode"].ToString());
                    BCode = (sqlReader["Barcode"].ToString());
                }


                if (ECode == txtECode.Text)
                {
                    MessageBox.Show("This DATA is already exist!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                }
                else
                {
                    SavedS();
                }
            }
        }

        void SavedS()
        {
            using (SqlConnection sqlConnection1 = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                if (MessageBox.Show("Are you sure you want to SAVE this Data?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string strQry = "INSERT INTO [TSPHAvailment].[dbo].[tblEmployee] (EmpCode, Barcode, LName, FName, MName, Sex, Department, Section, Status, Subsidy, Position, DateHired) VALUES (@EmpCode, @Barcode, @LName, @FName, @MName, @Sex, @Department, @Section, @Status, @Subsidy, @Position, @DateHired)";
                    SqlCommand sqlCmd1 = new SqlCommand(strQry, sqlConnection1);
                    sqlConnection1.Open();

                    sqlCmd1.Parameters.Add("@EmpCode", SqlDbType.NVarChar).Value = txtECode.Text;
                    sqlCmd1.Parameters.Add("@Barcode", SqlDbType.NVarChar).Value = txtECode.Text;
                    sqlCmd1.Parameters.Add("@LName", SqlDbType.NVarChar).Value = txtLName.Text;
                    sqlCmd1.Parameters.Add("@FName", SqlDbType.NVarChar).Value = txtFName.Text;
                    sqlCmd1.Parameters.Add("@MName", SqlDbType.NVarChar).Value = txtMName.Text;
                    sqlCmd1.Parameters.Add("@Sex", SqlDbType.NVarChar).Value = cbSex.Text;
                    sqlCmd1.Parameters.Add("@Department", SqlDbType.NVarChar).Value = cbDept.Text;
                    sqlCmd1.Parameters.Add("@Section", SqlDbType.NVarChar).Value = cbSect.Text;
                    sqlCmd1.Parameters.Add("@Status", SqlDbType.NVarChar).Value = cbStatus.Text;
                    sqlCmd1.Parameters.Add("@Subsidy", SqlDbType.NVarChar).Value = cbSubs.Text;
                    sqlCmd1.Parameters.Add("@Position", SqlDbType.NVarChar).Value = cbPosition.Text;
                    sqlCmd1.Parameters.Add("@DateHired", SqlDbType.NVarChar).Value = dtpDHired.Text;

                    sqlCmd1.ExecuteNonQuery();
                    sqlConnection1.Close();
                    MessageBox.Show("DATA SAVED!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clear();
                    txtECode.Focus();
                }
            }
        }

        void Clear()
        {
            txtECode.Text = string.Empty;
            txtFName.Text = string.Empty;
            txtLName.Text = string.Empty;
            txtMName.Text = string.Empty;
            cbDept.Text = string.Empty;
            cbPosition.Text = string.Empty;
            cbSect.Text = string.Empty;
            cbSex.Text = string.Empty;
            cbStatus.Text = string.Empty;
            cbSubs.Text = string.Empty;
            lblID.Text = "-";
            txtECode.Focus();
        }

        // EVENTS
        private void kryptonButton1_Click(object sender, EventArgs e)
        {
            Clear();
            enabletxtbox();
            btnSave.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                string strQry = "UPDATE tblEmployee SET EmpCode = @EmpCode, Barcode = @Barcode, LName = @LName, FName = @FName, MName = @MName, Sex = @Sex, Department = @Department, Section = @Section, Status = @Status, Subsidy = @Subsidy, Position = @Position where ID = '" + lblID.Text + "'";
                SqlCommand sqlCmd = new SqlCommand(strQry, sqlConnection);
                sqlConnection.Open();

                sqlCmd.Parameters.Add("@EmpCode", SqlDbType.NVarChar).Value = txtECode.Text;
                sqlCmd.Parameters.Add("@Barcode", SqlDbType.NVarChar).Value = txtECode.Text;
                sqlCmd.Parameters.Add("@LName", SqlDbType.NVarChar).Value = txtLName.Text;
                sqlCmd.Parameters.Add("@FName", SqlDbType.NVarChar).Value = txtFName.Text;
                sqlCmd.Parameters.Add("@MName", SqlDbType.NVarChar).Value = txtMName.Text;
                sqlCmd.Parameters.Add("@Sex", SqlDbType.NVarChar).Value = cbSex.Text;
                sqlCmd.Parameters.Add("@Department", SqlDbType.NVarChar).Value = cbDept.Text;
                sqlCmd.Parameters.Add("@Section", SqlDbType.NVarChar).Value = cbSect.Text;
                sqlCmd.Parameters.Add("@Status", SqlDbType.NVarChar).Value = cbStatus.Text;
                sqlCmd.Parameters.Add("@Subsidy", SqlDbType.NVarChar).Value = cbSubs.Text;
                sqlCmd.Parameters.Add("@Position", SqlDbType.NVarChar).Value = cbPosition.Text;
                MessageBox.Show("DATA UPDATED!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                sqlCmd.ExecuteNonQuery();
                sqlConnection.Close();
                Clear();
                txtECode.Focus();
            }
        }


        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT * FROM tblStatus where Status = '" + cbStatus.Text + "'", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    cbSubs.Text = (sqlReader["Subsidy"].ToString());
                }
                sqlReader.Close();
            }
        }

        private void kryptonTextBox1_TextChanged(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            using (SqlConnection connection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand SelectCommand = new SqlCommand("select * from tblEmployee where EmpCode like '" + txtSearch.Text + "%'", connection);
                SqlDataReader reader = SelectCommand.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem lv = new ListViewItem(reader["ID"].ToString());
                    lv.SubItems.Add(reader["EmpCode"].ToString());
                    lv.SubItems.Add(reader["Barcode"].ToString());
                    lv.SubItems.Add(reader["LName"].ToString());
                    lv.SubItems.Add(reader["FName"].ToString());
                    lv.SubItems.Add(reader["MName"].ToString());
                    lv.SubItems.Add(reader["Sex"].ToString());
                    lv.SubItems.Add(reader["Department"].ToString());
                    lv.SubItems.Add(reader["Section"].ToString());
                    lv.SubItems.Add(reader["Status"].ToString());
                    lv.SubItems.Add(reader["Subsidy"].ToString());
                    lv.SubItems.Add(reader["Position"].ToString());
                    lv.SubItems.Add(reader["DateHired"].ToString());
                    listView1.Items.Add(lv);
                }
                connection.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtECode.Text) ||  String.IsNullOrWhiteSpace(txtLName.Text) || String.IsNullOrEmpty(txtFName.Text) || String.IsNullOrWhiteSpace(txtMName.Text) || String.IsNullOrWhiteSpace(cbSex.Text) | String.IsNullOrEmpty(cbDept.Text) || String.IsNullOrWhiteSpace(cbSect.Text) || String.IsNullOrWhiteSpace(cbStatus.Text) || String.IsNullOrWhiteSpace(cbSubs.Text) || String.IsNullOrWhiteSpace(cbPosition.Text))
            {
                MessageBox.Show("Please Fill Up the blank details.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtECode.Text = String.Empty;
                txtFName.Text = String.Empty;
                txtLName.Text = String.Empty;
                txtMName.Text = String.Empty;
                txtSearch.Text = String.Empty;
                cbDept.Items.Clear();
                cbPosition.Items.Clear();
                cbSect.Items.Clear();
                cbSex.Text = string.Empty;
                cbStatus.Items.Clear();
                cbSubs.Items.Clear();

            }
            else
            {
                pointChecked();
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            btnSave.Enabled = false;
            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            enabletxtbox();

            lblID.Text = listView1.SelectedItems[0].SubItems[0].Text;
            txtECode.Text = listView1.SelectedItems[0].SubItems[1].Text;
            txtLName.Text = listView1.SelectedItems[0].SubItems[3].Text;
            txtFName.Text = listView1.SelectedItems[0].SubItems[4].Text;
            txtMName.Text = listView1.SelectedItems[0].SubItems[5].Text;
            cbSex.Text = listView1.SelectedItems[0].SubItems[6].Text;
            cbDept.Text = listView1.SelectedItems[0].SubItems[7].Text;
            cbSect.Text = listView1.SelectedItems[0].SubItems[8].Text;
            cbStatus.Text = listView1.SelectedItems[0].SubItems[9].Text;
            cbSubs.Text = listView1.SelectedItems[0].SubItems[10].Text;
            cbPosition.Text = listView1.SelectedItems[0].SubItems[11].Text;
            dtpDHired.Value = Convert.ToDateTime(listView1.SelectedItems[0].SubItems[12].Text);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                if (MessageBox.Show("Are you sure you want to delete this?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string strQry = "Delete From tblEmployee where ID='" + lblID.Text + "'";
                    SqlCommand scmd = new SqlCommand(strQry, sqlConnection);
                    SqlDataReader reader;
                    try
                    {
                        sqlConnection.Open();
                        reader = scmd.ExecuteReader();
                        MessageBox.Show("DATA DELETED!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        while (reader.Read())
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    Clear();
                    txtECode.Focus();
                }

            }
        }
    }
}
