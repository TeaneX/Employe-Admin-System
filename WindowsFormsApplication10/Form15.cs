using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;


namespace WindowsFormsApplication10
{
    public partial class Form15 : Form
    {


        public Form15()
        {
            InitializeComponent();
        }

        private void Form15_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False");
            con.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM LeaveApplications", con);
            SqlDataAdapter sd = new SqlDataAdapter(com);
            DataTable dt = new DataTable();
            sd.Fill(dt);
            reportViewer1.LocalReport.DataSources.Clear();
            ReportDataSource source = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.ReportPath = @"C:\Users\Vishwa\documents\visual studio 2015\Projects\WindowsFormsApplication10\WindowsFormsApplication10\Report1.rdlc";
            reportViewer1.LocalReport.DataSources.Add(source);
            reportViewer1.RefreshReport();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure, Do you really want to EXIT...!", "EXIT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form14 obj = new Form14();
                obj.Show();
                this.Hide();
            }
            else
            {

            }
        }
    }
}
        

        
  

