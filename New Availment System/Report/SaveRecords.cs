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
using CrystalDecisions.Shared;
using Excel = Microsoft.Office.Interop.Excel;
using CrystalDecisions.CrystalReports.Engine;
using Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace New_Availment_System
{
    public partial class NewSaveRecords : Form
    {
        public NewSaveRecords()
        {
            InitializeComponent();
        }

        // LOAD Form
        private void SaveRecords_Load(object sender, EventArgs e)
        {
            showsaveddata();

            if (listView3.Items.Count > 0)
            {
                btnSearch.Enabled = false;
                showunswiped();
            }
            else
            {
                btnSearch.Enabled = true;
            }
        }

        // FUNCTIONS
        void checkswipedata()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("SELECT COUNT(*) FROM tblUnswiped", connection);
                SqlDataReader reader = selectCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    btnSearch.Enabled = false;
                    showsaveddata();
                }
                else
                {
                    btnSearch.Enabled = true;
                }
            }
        }

        void saveswipedata()
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                if (listView3.Items.Count > 0)
                {
                    for (int i = 0; i < listView3.Items.Count; i++)
                    {
                        string EmpCode = listView3.Items[i].SubItems[0].Text;
                        string LastName = listView3.Items[i].SubItems[1].Text;
                        string FirstName = listView3.Items[i].SubItems[2].Text;
                        string Department = listView3.Items[i].SubItems[3].Text;
                        string Subsidy = listView3.Items[i].SubItems[4].Text;

                        string strQry = "INSERT INTO tblUnswiped (EmpCode, LastName, FirstName, Department, Subsidy, UDate) VALUES (@EmpCode, @LastName, @FirstName, @Department, @Subsidy, @UDate)";
                        SqlCommand sqlCmd = new SqlCommand(strQry, sqlConnection);
                        sqlConnection.Open();

                        sqlCmd.Parameters.Add("@EmpCode", SqlDbType.NVarChar).Value = EmpCode;
                        sqlCmd.Parameters.Add("@LastName", SqlDbType.NVarChar).Value = LastName;
                        sqlCmd.Parameters.Add("@FirstName", SqlDbType.NVarChar).Value = FirstName;
                        sqlCmd.Parameters.Add("@Department", SqlDbType.NVarChar).Value = Department;
                        sqlCmd.Parameters.Add("@Subsidy", SqlDbType.NVarChar).Value = Subsidy;
                        sqlCmd.Parameters.Add("@UDate", SqlDbType.NVarChar).Value = DateTime.Now.ToString("M/d/yyyy");

                        sqlCmd.ExecuteNonQuery();
                        sqlConnection.Close();

                    }
                    MessageBox.Show("DATA SAVED!", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        void showsaveddata()
        {
            listView3.Items.Clear();
            using (SqlConnection connection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                connection.Open();
                SqlCommand SelectCommand = new SqlCommand("select * from tblUnswiped order by Department", connection);
                SqlDataReader reader = SelectCommand.ExecuteReader();
                while (reader.Read())
                {
                    ListViewItem lv = new ListViewItem(reader["Department"].ToString());
                    lv.SubItems.Add(reader["LastName"].ToString() + " " + reader["FirstName"].ToString());
                    lv.SubItems.Add(reader["UDate"].ToString());
                    lv.SubItems.Add(reader["Subsidy"].ToString());
                    lv.SubItems.Add(reader["Remarks"].ToString());
                    listView3.Items.Add(lv);
                }
                connection.Close();
            }

        }
        void showunswiped()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                connection.Close();
                connection.Open();
                using (SqlCommand command = new SqlCommand("GetViewUnswiped2", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ListViewItem lv = new ListViewItem(reader["EmpCode"].ToString());
                            lv.SubItems.Add(reader["LName"].ToString());
                            lv.SubItems.Add(reader["FName"].ToString());
                            lv.SubItems.Add(reader["Department"].ToString());
                            lv.SubItems.Add(reader["Subsidy"].ToString());
                            lv.SubItems.Add(reader["Remarks"].ToString());
                            listView2.Items.Add(lv);
                        }
                        connection.Close();
                        listView2.LabelEdit = true;
                    }
                }
            }
        }


        // EVENTS 
        private void btnSearch_Click(object sender, EventArgs e)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                connection.Close();
                connection.Open();
                SqlCommand selectCommand = new SqlCommand("SELECT DISTINCT EmpCode, LName, Fname, Department, Amount FROM tblAvailment WHERE AvailDate = '" + dateTimePicker1.Text + "' ORDER BY Department", connection);
                SqlDataReader reader = selectCommand.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ListViewItem lv = new ListViewItem(reader["EmpCode"].ToString());
                        lv.SubItems.Add(reader["LName"].ToString());
                        lv.SubItems.Add(reader["FName"].ToString());
                        lv.SubItems.Add(reader["Department"].ToString());
                        lv.SubItems.Add(reader["Amount"].ToString());
                        listView3.Items.Add(lv);
                    }

                    listView3.Columns[0].Text = "Empcode";
                    listView3.Columns[1].Text = "Firstname";
                    listView3.Columns[2].Text = "Lastname";
                    listView3.Columns[3].Text = "Department";
                    listView3.Columns[4].Text = "Amount";

                    button3.Visible = true;
                    btnSearch.Visible = false;
                }
                else
                {
                    MessageBox.Show("No Record has found");
                }

                listView3.LabelEdit = true;
            }


            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            showunswiped();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                if (MessageBox.Show("Are you sure you want to delete this?", System.Windows.Forms.Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {

                    string strQry = "Delete From tblUnswiped";
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
                        MessageBox.Show(ex.Message, System.Windows.Forms.Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            listView3.Items.Clear();
            listView2.Items.Clear();
            btnSearch.Enabled = true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            saveswipedata();
            showunswiped();
            button3.Visible = false;
            btnSearch.Visible = true;
            btnSearch.Enabled = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            frmGeneral frm1 = new frmGeneral();
            frm1.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            frmReport frm1 = new frmReport();
            frm1.Show();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            // Create a new Excel application
            Excel.Application excelApp = new Excel.Application();

            // Create a new workbook
            Excel.Workbook workbook = excelApp.Workbooks.Add();

            // Load the first Crystal Report
            ReportDocument report1 = new ReportDocument();
            report1.Load(@"\\10.168.0.3\program\ASReport\CrystalReport2.rpt");

            // Set the data source for the first report
            SqlConnection con1 = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True");
            SqlDataAdapter sda1 = new SqlDataAdapter("GetViewUnswiped2", con1);
            sda1.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds1 = new DataSet();
            sda1.Fill(ds1, "ViewUnSwiped2");
            report1.SetDataSource(ds1);

            // Export the first report to a temporary file
            string tempFilePath1 = Path.GetTempFileName();
            report1.ExportToDisk(ExportFormatType.Excel, tempFilePath1);

            // Open the temporary file in Excel
            Excel.Workbook tempWorkbook1 = excelApp.Workbooks.Open(tempFilePath1);
            Excel.Worksheet worksheet1 = (Excel.Worksheet)tempWorkbook1.Sheets[1];

            // Change the sheet name for the first report
            worksheet1.Name = "Unswiped";

            // Copy the worksheet to the main workbook
            worksheet1.Copy(workbook.Sheets[workbook.Sheets.Count]);

            // Close and release the resources for the first report
            tempWorkbook1.Close(false);
            Marshal.ReleaseComObject(worksheet1);
            Marshal.ReleaseComObject(tempWorkbook1);

            // Load the second Crystal Report
            ReportDocument report2 = new ReportDocument();
            report2.Load(@"\\10.168.0.3\program\ASReport\CrystalReport1.rpt");

            // Set the data source for the second report
            SqlConnection con2 = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True");
            SqlDataAdapter sda2 = new SqlDataAdapter("Select DISTINCT * From tblAvailment where AvailDate = @StartDate", con2);
            sda2.SelectCommand.Parameters.AddWithValue("@StartDate", dateTimePicker1.Text);
            DataSet ds2 = new DataSet();
            sda2.Fill(ds2, "tblAvailment");
            report2.SetDataSource(ds2);

            // Update the date in the Crystal Report
            TextObject text = (TextObject)report2.ReportDefinition.Sections["Section1"].ReportObjects["Text10"];
            text.Text = dateTimePicker1.Text;
            TextObject text1 = (TextObject)report2.ReportDefinition.Sections["Section1"].ReportObjects["Text11"];
            text1.Text = dateTimePicker1.Text;

            // Export the second report to a temporary file
            string tempFilePath2 = Path.GetTempFileName();
            report2.ExportToDisk(ExportFormatType.Excel, tempFilePath2);

            // Open the temporary file in Excel
            Excel.Workbook tempWorkbook2 = excelApp.Workbooks.Open(tempFilePath2);
            Excel.Worksheet worksheet2 = (Excel.Worksheet)tempWorkbook2.Sheets[1];

            // Change the sheet name for the second report
            worksheet2.Name = "Swiped";

            // Copy the worksheet to the main workbook
            worksheet2.Copy(workbook.Sheets[workbook.Sheets.Count]);

            // Close and release the resources for the second report
            tempWorkbook2.Close(false);
            Marshal.ReleaseComObject(worksheet2);
            Marshal.ReleaseComObject(tempWorkbook2);

            // Load the third Crystal Report
            ReportDocument report3 = new ReportDocument();
            report3.Load(@"\\10.168.0.3\program\ASReport\CrystalReport3.rpt");

            // Set the data source for the third report
            SqlConnection con3 = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True");
            SqlDataAdapter sda3 = new SqlDataAdapter("Select * From tblAvailment where AvailDate ='" + dateTimePicker1.Text + "' ", con3);
            DataSet ds3 = new DataSet();
            sda3.Fill(ds3, "tblAvailment");
            report3.SetDataSource(ds3);

            // Update the date in the Crystal Report
            TextObject text2 = (TextObject)report3.ReportDefinition.Sections["Section1"].ReportObjects["Text10"];
            text2.Text = dateTimePicker1.Text;
            TextObject text3 = (TextObject)report3.ReportDefinition.Sections["Section1"].ReportObjects["Text11"];
            text3.Text = dateTimePicker1.Text;

            // Export the third report to a temporary file
            string tempFilePath3 = Path.GetTempFileName();
            report3.ExportToDisk(ExportFormatType.Excel, tempFilePath3);

            // Open the temporary file in Excel
            Excel.Workbook tempWorkbook3 = excelApp.Workbooks.Open(tempFilePath3);
            Excel.Worksheet worksheet3 = (Excel.Worksheet)tempWorkbook3.Sheets[1];

            // Change the sheet name for the third report
            worksheet3.Name = "General Report";

            // Copy the worksheet to the main workbook
            worksheet3.Copy(workbook.Sheets[workbook.Sheets.Count]);

            // Close and release the resources for the third report
            tempWorkbook3.Close(false);
            Marshal.ReleaseComObject(worksheet3);
            Marshal.ReleaseComObject(tempWorkbook3);

            // Load the fourth Crystal Report
            ReportDocument report4 = new ReportDocument();
            report4.Load(@"\\10.168.0.3\program\ASReport\CrystalReport4.rpt");

            // Set the data source for the fourth report
            SqlConnection con4 = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True");
            SqlDataAdapter sda4 = new SqlDataAdapter("GetViewUnswiped2", con4);
            sda4.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet ds4 = new DataSet();
            sda4.Fill(ds4, "ViewUnSwiped2");
            report4.SetDataSource(ds4);

            // Export the fourth report to a temporary file
            string tempFilePath4 = Path.GetTempFileName();
            report4.ExportToDisk(ExportFormatType.Excel, tempFilePath4);

            // Open the temporary file in Excel
            Excel.Workbook tempWorkbook4 = excelApp.Workbooks.Open(tempFilePath4);
            Excel.Worksheet worksheet4 = (Excel.Worksheet)tempWorkbook4.Sheets[1];

            // Change the sheet name for the fourth report
            worksheet4.Name = "Total Unswipe Report";

            // Copy the worksheet to the main workbook
            worksheet4.Copy(workbook.Sheets[workbook.Sheets.Count]);

            // Close and release the resources for the fourth report
            tempWorkbook4.Close(false);
            Marshal.ReleaseComObject(worksheet4);
            Marshal.ReleaseComObject(tempWorkbook4);

            // Remove the default "Sheet1" sheet
            workbook.Sheets["Sheet1"].Delete();

            // Create a SaveFileDialog to choose the save location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Excel Workbook|*.xls";
            saveFileDialog.Title = "Save Excel File";
            saveFileDialog.ShowDialog();

            if (!string.IsNullOrEmpty(saveFileDialog.FileName))
            {
                // Save the workbook to the selected location
                workbook.SaveAs(saveFileDialog.FileName, Excel.XlFileFormat.xlWorkbookNormal);

                // Close and release the resources for the main workbook and Excel application
                workbook.Close(false);
                excelApp.Quit();
                Marshal.ReleaseComObject(workbook);
                Marshal.ReleaseComObject(excelApp);

                // Clean up the temporary files
                File.Delete(tempFilePath1);
                File.Delete(tempFilePath2);
                File.Delete(tempFilePath3);

                MessageBox.Show("Reports exported successfully.", "Export Reports", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
