using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace New_Availment_System
{
    public partial class frmReport : Form
    {
        ReportDocument cry = new ReportDocument();
        public frmReport()
        {
            InitializeComponent();
        }

        private void frmReport_Load(object sender, EventArgs e)
        {
            cry.Load(@"\\10.168.0.3\program\ASReport\CrystalReport2.rpt");
            SqlConnection con = new SqlConnection("Data Source=SVPH_DBSERVER4;Initial Catalog=TSPHAvailment;Integrated Security=True");
            SqlDataAdapter sda = new SqlDataAdapter("GetViewUnswiped2", con);
            sda.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataSet st = new System.Data.DataSet();
            sda.Fill(st, "ViewUnSwiped2");
            cry.SetDataSource(st);
            crystalReportViewer1.ReportSource = cry;

        }
    }
}
