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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace New_Availment_System
{
    public partial class frmGForm : Form
    {
        ReportDocument cry = new ReportDocument();
        public frmGForm()
        {
            InitializeComponent();
        }

        private void frmGForm_Load(object sender, EventArgs e)
        {
            cry.Load(@"\\10.168.0.3\program\ASReport\CrystalReport3.rpt");
            SqlConnection con = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True");
            SqlDataAdapter sda = new SqlDataAdapter("GettblAvailment", con);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet st = new System.Data.DataSet();
            sda.Fill(st, "tblAvailment");
            cry.SetDataSource(st);
            crystalReportViewer1.ReportSource = cry;
            FromDate();
            ToDate();
        }

        void FromDate()
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT  AvailDate FROM tblAvailment", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    dateTimePicker1.Text = (sqlReader["AvailDate"].ToString());
                }

                sqlReader.Close();
            }
        }

        void ToDate()
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                SqlCommand sqlCmd = new SqlCommand("SELECT  AvailDate FROM tblAvailment", sqlConnection);
                sqlConnection.Open();
                SqlDataReader sqlReader = sqlCmd.ExecuteReader();

                while (sqlReader.Read())
                {
                    dateTimePicker1.Text = (sqlReader["AvailDate"].ToString());
                }

                sqlReader.Close();
            }
        }
        private void btnGo_Click(object sender, EventArgs e)
        {
            using (SqlConnection sqlConnection = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True"))
            {
                SqlDataAdapter sda = new SqlDataAdapter("Select * From tblAvailment where AvailDate >='" + dateTimePicker1.Text + "' and AvailDate <= '" + dateTimePicker2.Text + "'", sqlConnection);
                DataSet ds = new DataSet();
                sda.Fill(ds, "tblAvailment");
                cry.SetDataSource(ds);
                crystalReportViewer1.ReportSource = cry;

                TextObject text = (TextObject)cry.ReportDefinition.Sections["Section1"].ReportObjects["Text10"];
                text.Text = dateTimePicker1.Text;
                TextObject text1 = (TextObject)cry.ReportDefinition.Sections["Section1"].ReportObjects["Text11"];
                text1.Text = dateTimePicker2.Text;
            }
        }
    }
}
