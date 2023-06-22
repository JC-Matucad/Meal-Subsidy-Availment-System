using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace New_Availment_System
{
    public partial class frmNewEmp : Form
    {
        public frmNewEmp()
        {
            InitializeComponent();
        }

        private void frmNewEmp_Load(object sender, EventArgs e)
        {
            Department();
            Section();
            Status();
            Position();
            DataListViews();
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
                SqlCommand sqlCmd = new SqlCommand("select * from [tblEmployee] where EmpCode ='" + txtECode.Text + "' and Barcode = '" + txtBCode.Text + "'", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    ECode = (sqlReader["EmpCode"].ToString());
                    BCode = (sqlReader["Barcode"].ToString());
                    //FCode = (sqlReader["FName"].ToString());
                    //LCode = (sqlReader["LName"].ToString());
                    //MCode = (sqlReader["MName"].ToString());
                }


                if (ECode == txtECode.Text)
                {
                    if (BCode == txtBCode.Text)
                    {
                        if (FCode == txtFName.Text && MCode == txtMName.Text && LCode == txtLName.Text)
                        {
                            MessageBox.Show("This DATA has been Recorded!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Clear();

                        }
                    }
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
                    sqlCmd1.Parameters.Add("@Barcode", SqlDbType.NVarChar).Value = txtBCode.Text;
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
        void Department()
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

        void Section()
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

        void Status()
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

        public string Checked;
        void PIDCheck()
        {
            Checked = cbStatus.Text;
            using (SqlConnection connection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                SqlCommand command = new SqlCommand("select * from [tblStatus] where Status ='" + Checked + "' ", connection);
                connection.Open();

                SqlDataReader read = command.ExecuteReader();

                while (read.Read())
                {
                    cbSubs.Text = (read["Subsidy"].ToString());

                }
            }
        }

        void Position()
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

        void Clear()
        {
            txtBCode.Text = string.Empty;
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

        string cstring = "Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True";
        SqlCommand com = new SqlCommand();
        SqlDataAdapter dpa = new SqlDataAdapter();
        SqlConnection cn = new SqlConnection();
        SqlConnection cn1 = new SqlConnection();
        SqlDataReader read;

        void DataListViews()
        {
            listView1.FullRowSelect = true;
            listView1.MultiSelect = false;
            listView1.View = View.Details;

            listView1.Columns.Clear();
            listView1.Items.Clear();
            listView1.Columns.Add("ID", 60, HorizontalAlignment.Center);
            listView1.Columns.Add("EmpCode", 80, HorizontalAlignment.Center);
            listView1.Columns.Add("Barcode", 80, HorizontalAlignment.Center);
            listView1.Columns.Add("LName", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("FName", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("MName", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Sex", 60, HorizontalAlignment.Center);
            listView1.Columns.Add("Department", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Section", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Status", 140, HorizontalAlignment.Center);
            listView1.Columns.Add("Subsidy", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Position", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("DateHired", 100, HorizontalAlignment.Center);

            cn.Close();
            cn.ConnectionString = cstring;
            SqlCommand SelectCommand = new SqlCommand("select ID,EmpCode,Barcode,LName,FName,MName,Sex,Department,Section,Status,Subsidy,Position,DateHired from tblEmployee", cn);
            cn.Open();
            read = SelectCommand.ExecuteReader();

            while (read.Read())
            {
                ListViewItem lv = new ListViewItem(read["ID"].ToString());
                lv.SubItems.Add(read["EmpCode"].ToString());
                lv.SubItems.Add(read["Barcode"].ToString());
                lv.SubItems.Add(read["LName"].ToString());
                lv.SubItems.Add(read["FName"].ToString());
                lv.SubItems.Add(read["MName"].ToString());
                lv.SubItems.Add(read["Sex"].ToString());
                lv.SubItems.Add(read["Department"].ToString());
                lv.SubItems.Add(read["Section"].ToString());
                lv.SubItems.Add(read["Status"].ToString());
                lv.SubItems.Add(read["Subsidy"].ToString());
                lv.SubItems.Add(read["Position"].ToString());
                lv.SubItems.Add(read["DateHired"].ToString());
                listView1.Items.Add(lv);
            }
            cn.Close();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            btnUpdate.Enabled = true;
            btnSearch.Enabled = true;
            txtBCode.Enabled = true;
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
            btnAdd.Visible = false;
            btnClose.Visible = true;
            dtpDHired.Enabled = true;
        }

        private void btnSClose_Click(object sender, EventArgs e)
        {
            txtSearch.Visible = false;
            txtSearchN.Visible = false;
            btnSearch.Visible = true;
            btnSClose.Visible = false;
            btnDelete.Visible = false;
            listView1.Visible = false;
            txtECode.Focus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txtECode.Text) || String.IsNullOrEmpty(txtECode.Text) || String.IsNullOrWhiteSpace(txtBCode.Text) || String.IsNullOrEmpty(txtBCode.Text) || String.IsNullOrWhiteSpace(txtLName.Text) || String.IsNullOrEmpty(txtLName.Text) || String.IsNullOrWhiteSpace(txtFName.Text) || String.IsNullOrEmpty(txtFName.Text) || String.IsNullOrWhiteSpace(txtMName.Text) || String.IsNullOrEmpty(txtMName.Text) || String.IsNullOrWhiteSpace(cbSex.Text) || String.IsNullOrEmpty(cbSex.Text) || String.IsNullOrWhiteSpace(cbDept.Text) || String.IsNullOrEmpty(cbDept.Text) || String.IsNullOrWhiteSpace(cbSect.Text) || String.IsNullOrEmpty(cbSect.Text) || String.IsNullOrWhiteSpace(cbStatus.Text) || String.IsNullOrEmpty(cbStatus.Text) || String.IsNullOrWhiteSpace(cbSubs.Text) || String.IsNullOrEmpty(cbSubs.Text) || String.IsNullOrWhiteSpace(cbPosition.Text) || String.IsNullOrEmpty(cbPosition.Text))
            {
                MessageBox.Show("Please Fill Up the blank details.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtECode.Text = String.Empty;
                txtBCode.Text = String.Empty;
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                string strQry = "UPDATE tblEmployee SET EmpCode = @EmpCode, Barcode = @Barcode, LName = @LName, FName = @FName, MName = @MName, Sex = @Sex, Department = @Department, Section = @Section, Status = @Status, Subsidy = @Subsidy, Position = @Position where ID = '" + lblID.Text + "'";
                SqlCommand sqlCmd = new SqlCommand(strQry, sqlConnection);
                sqlConnection.Open();

                sqlCmd.Parameters.Add("@EmpCode", SqlDbType.NVarChar).Value = txtECode.Text;
                sqlCmd.Parameters.Add("@Barcode", SqlDbType.NVarChar).Value = txtBCode.Text;
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

        private void txtSearchN_TextChanged(object sender, EventArgs e)
        {
            cn.Close();
            cn.ConnectionString = cstring;
            SqlCommand SelectCommand = new SqlCommand("select * from tblEmployee where Department like '" + txtSearchN.Text + "%'", cn);
            cn.Open();
            listView1.Items.Clear();
            read = SelectCommand.ExecuteReader();
            while (read.Read())
            {

                ListViewItem lv = new ListViewItem(read["ID"].ToString());
                lv.SubItems.Add(read["EmpCode"].ToString());
                lv.SubItems.Add(read["Barcode"].ToString());
                lv.SubItems.Add(read["LName"].ToString());
                lv.SubItems.Add(read["FName"].ToString());
                lv.SubItems.Add(read["MName"].ToString());
                lv.SubItems.Add(read["Sex"].ToString());
                lv.SubItems.Add(read["Department"].ToString());
                lv.SubItems.Add(read["Section"].ToString());
                lv.SubItems.Add(read["Status"].ToString());
                lv.SubItems.Add(read["Subsidy"].ToString());
                lv.SubItems.Add(read["Position"].ToString());
                lv.SubItems.Add(read["DateHired"].ToString());
                listView1.Items.Add(lv);
            }
            cn.Close();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            cn.Close();
            cn.ConnectionString = cstring;
            SqlCommand SelectCommand = new SqlCommand("select * from tblEmployee where EmpCode like '" + txtSearch.Text + "%'", cn);
            cn.Open();
            listView1.Items.Clear();
            read = SelectCommand.ExecuteReader();
            while (read.Read())
            {

                ListViewItem lv = new ListViewItem(read["ID"].ToString());
                lv.SubItems.Add(read["EmpCode"].ToString());
                lv.SubItems.Add(read["Barcode"].ToString());
                lv.SubItems.Add(read["LName"].ToString());
                lv.SubItems.Add(read["FName"].ToString());
                lv.SubItems.Add(read["MName"].ToString());
                lv.SubItems.Add(read["Sex"].ToString());
                lv.SubItems.Add(read["Department"].ToString());
                lv.SubItems.Add(read["Section"].ToString());
                lv.SubItems.Add(read["Status"].ToString());
                lv.SubItems.Add(read["Subsidy"].ToString());
                lv.SubItems.Add(read["Position"].ToString());
                lv.SubItems.Add(read["DateHired"].ToString());
                listView1.Items.Add(lv);
            }
            cn.Close();
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lblID.Text = listView1.SelectedItems[0].SubItems[0].Text;
            txtECode.Text = listView1.SelectedItems[0].SubItems[1].Text;
            txtBCode.Text = listView1.SelectedItems[0].SubItems[2].Text;
            txtLName.Text = listView1.SelectedItems[0].SubItems[3].Text;
            txtFName.Text = listView1.SelectedItems[0].SubItems[4].Text;
            txtMName.Text = listView1.SelectedItems[0].SubItems[5].Text;
            cbSex.Text = listView1.SelectedItems[0].SubItems[6].Text;
            cbDept.Text = listView1.SelectedItems[0].SubItems[7].Text;
            cbSect.Text = listView1.SelectedItems[0].SubItems[8].Text;
            cbStatus.Text = listView1.SelectedItems[0].SubItems[9].Text;
            cbSubs.Text = listView1.SelectedItems[0].SubItems[10].Text;
            cbPosition.Text = listView1.SelectedItems[0].SubItems[11].Text;
            dtpDHired.Text = Convert.ToString(listView1.SelectedItems[0].SubItems[12].Text);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Visible = true;
            txtSearchN.Visible = true;
            btnSearch.Visible = false;
            btnSClose.Visible = true;
            btnDelete.Visible = true;
            listView1.Visible = true;
            listView1.BringToFront();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
