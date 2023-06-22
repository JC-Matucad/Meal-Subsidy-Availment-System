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
    public partial class frmUserS : Form
    {
        public frmUserS()
        {
            InitializeComponent();
        }

        private void frmUserS_Load(object sender, EventArgs e)
        {
            DataListViews();
        }

        //FUNCTIONS

        string cstring = "Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True";
        SqlConnection cn = new SqlConnection();
        SqlDataReader read;

        void DataListViews()
        {
            listView1.FullRowSelect = true;
            listView1.MultiSelect = false;
            listView1.View = View.Details;

            listView1.Columns.Clear();
            listView1.Items.Clear();
            listView1.Columns.Add("ID", 60, HorizontalAlignment.Center);
            listView1.Columns.Add("EmpName", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Department", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Username", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("Password", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("SecurityLevel", 100, HorizontalAlignment.Center);
            listView1.Columns.Add("SecurityNumber", 60, HorizontalAlignment.Center);

            cn.Close();
            cn.ConnectionString = cstring;
            SqlCommand SelectCommand = new SqlCommand("select * from tblSecurity", cn);
            cn.Open();
            read = SelectCommand.ExecuteReader();

            while (read.Read())
            {
                ListViewItem lv = new ListViewItem(read["ID"].ToString());
                lv.SubItems.Add(read["EmpName"].ToString());
                lv.SubItems.Add(read["Department"].ToString());
                lv.SubItems.Add(read["Username"].ToString());
                lv.SubItems.Add(read["Password"].ToString());
                lv.SubItems.Add(read["SecurityLevel"].ToString());
                lv.SubItems.Add(read["SecurityNumber"].ToString());
                listView1.Items.Add(lv);
            }
            cn.Close();
        }
        void ClearS()
        {
            txtDept.Text = string.Empty;
            txtEmpName.Text = string.Empty;
            txtPass.Text = string.Empty;
            txtUName.Text = string.Empty;
            cbSLevel.Text = string.Empty;
            lblSLN.Text = "---";
            lblID.Text = "---";
            txtEmpName.Focus();
        }

        // EVENTS
        private void btnOpen_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = true;
            btnUpdate.Enabled = true;
            btnSearch.Enabled = true;
            txtDept.Enabled = true;
            txtEmpName.Enabled = true;
            txtPass.Enabled = true;
            txtUName.Enabled = true;
            cbSLevel.Enabled = true;
            btnOpen.Visible = false;
            btnOClose.Visible = true;
            txtEmpName.Focus();
        }

        private void btnOClose_Click(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            btnUpdate.Enabled = false;
            btnSearch.Enabled = false;
            txtDept.Enabled = false;
            txtEmpName.Enabled = false;
            txtPass.Enabled = false;
            txtUName.Enabled = false;
            cbSLevel.Enabled = false;
            btnOpen.Visible = true;
            btnOClose.Visible = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                if (MessageBox.Show("Are you sure you want to SAVE this Data?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    string strQry = "INSERT INTO [TSPHAvailment].[dbo].[tblSecurity] (EmpName, Department, Username, Password, SecurityLevel, SecurityNumber) VALUES (@EmpName, @Department, @Username, @Password, @SecurityLevel, @SecurityNumber)";
                    SqlCommand sqlCmd = new SqlCommand(strQry, sqlConnection);
                    sqlConnection.Open();

                    sqlCmd.Parameters.Add("@EmpName", SqlDbType.NVarChar).Value = txtEmpName.Text;
                    sqlCmd.Parameters.Add("@Department", SqlDbType.NVarChar).Value = txtDept.Text;
                    sqlCmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = txtUName.Text;
                    sqlCmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = txtPass.Text;
                    sqlCmd.Parameters.Add("@SecurityLevel", SqlDbType.NVarChar).Value = cbSLevel.Text;
                    sqlCmd.Parameters.Add("@SecurityNumber", SqlDbType.NVarChar).Value = lblSLN.Text;

                    sqlCmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    MessageBox.Show("DATA SAVED!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearS();
                }
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                string strQry = "UPDATE tblSecurity SET EmpName = @EmpName, Department = @Dept ,Username = @Username, Password = @Password, SecurityLevel = @SLevel, SecurityNumber = @SNum where ID = '" + lblID.Text + "'";
                SqlCommand sqlCmd = new SqlCommand(strQry, sqlConnection);
                sqlConnection.Open();

                sqlCmd.Parameters.Add("@EmpName", SqlDbType.NVarChar).Value = txtEmpName.Text;
                sqlCmd.Parameters.Add("@Dept", SqlDbType.NVarChar).Value = txtDept.Text;
                sqlCmd.Parameters.Add("@Username", SqlDbType.NVarChar).Value = txtUName.Text;
                sqlCmd.Parameters.Add("@Password", SqlDbType.NVarChar).Value = txtPass.Text;
                sqlCmd.Parameters.Add("@SLevel", SqlDbType.NVarChar).Value = cbSLevel.Text;
                sqlCmd.Parameters.Add("@SNum", SqlDbType.NVarChar).Value = lblSLN.Text;
                MessageBox.Show("DATA UPDATED!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

                sqlCmd.ExecuteNonQuery();
                sqlConnection.Close();
                ClearS();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            txtSearch.Visible = true;
            listView1.Visible = true;
            btnSearch.Visible = false;
            btnSClose.Visible = true;
            btnDelete.Visible = true;
            listView1.BringToFront();
        }

        private void btnSClose_Click(object sender, EventArgs e)
        {
            txtSearch.Visible = false;
            listView1.Visible = false;
            btnSearch.Visible = true;
            btnSClose.Visible = false;
            btnDelete.Visible = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                if (MessageBox.Show("Are you sure you want to delete this?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string strQry = "Delete From tblSecurity where ID='" + lblID.Text + "'";
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

                    ClearS();
                }
            }
        }

        private void cbSLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbSLevel.Text == "Admin")
            {
                lblSLN.Text = "0";
            }
            else if (cbSLevel.Text == "HRD/GSD")
            {
                lblSLN.Text = "1";
            }
            else
            {
                lblSLN.Text = "2";
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            lblID.Text = listView1.SelectedItems[0].SubItems[0].Text;
            txtEmpName.Text = listView1.SelectedItems[0].SubItems[1].Text;
            txtDept.Text = listView1.SelectedItems[0].SubItems[2].Text;
            txtUName.Text = listView1.SelectedItems[0].SubItems[3].Text;
            txtPass.Text = listView1.SelectedItems[0].SubItems[4].Text;
            cbSLevel.Text = listView1.SelectedItems[0].SubItems[5].Text;
            lblSLN.Text = listView1.SelectedItems[0].SubItems[6].Text;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            cn.Close();
            cn.ConnectionString = cstring;
            SqlCommand SelectCommand = new SqlCommand("select * from tblSecurity where EmpName like '" + txtSearch.Text + "%'", cn);
            cn.Open();
            listView1.Items.Clear();
            read = SelectCommand.ExecuteReader();
            while (read.Read())
            {

                ListViewItem lv = new ListViewItem(read["ID"].ToString());
                lv.SubItems.Add(read["EmpName"].ToString());
                lv.SubItems.Add(read["Department"].ToString());
                lv.SubItems.Add(read["Username"].ToString());
                lv.SubItems.Add(read["Password"].ToString());
                lv.SubItems.Add(read["SecurityLevel"].ToString());
                lv.SubItems.Add(read["SecurityNumber"].ToString());
                listView1.Items.Add(lv);
            }
            cn.Close();
        }
    }
}
