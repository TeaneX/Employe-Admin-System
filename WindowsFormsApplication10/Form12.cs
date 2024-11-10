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

namespace WindowsFormsApplication10
{
    public partial class Form12 : Form
    {
        public Form12()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False");

        private void Form12_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM LeaveBalances", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM LeaveBalances  WHERE employee_ID=@employee_ID", con);
            cmd.Parameters.AddWithValue("employee_ID", textBox1.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sqlstm = "select * from LeaveBalances ";
            SqlDataAdapter da = new SqlDataAdapter(sqlstm, con);
            DataSet ds = new System.Data.DataSet();
            da.Fill(ds, "LeaveBalances");
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure, Do you really want to EXIT...!", "EXIT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form8 obj = new Form8();
                obj.Show();
                this.Hide();
            }
            else
            {

            }
        }
    }
}
