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
    public partial class Form6 : Form
    {
        public Form6()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False");
        private void button4_Click(object sender, EventArgs e)
        {

            con.Open();
            SqlCommand cmd = new SqlCommand("insert into  LeaveBalances values(@employee_ID ,@Annual_Leaves ,@Casual_Leaves,@Short_Leaves,@Short_Leaves_Used)", con);
            cmd.Parameters.AddWithValue("@employee_ID", int.Parse(textBox1.Text));
            cmd.Parameters.AddWithValue("@Annual_Leaves", int.Parse(textBox2.Text));
            cmd.Parameters.AddWithValue("@Casual_Leaves", int.Parse(textBox3.Text));
            cmd.Parameters.AddWithValue("@Short_Leaves", int.Parse(textBox4.Text));
            cmd.Parameters.AddWithValue("@Short_Leaves_Used", int.Parse(textBox5.Text));
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Successfully Inserted");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure, Do you really want to EXIT...!", "EXIT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form3 obj = new Form3();
                obj.Show();
                this.Hide();
            }
            else
            {

            }
        }

        private void Form6_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM LeaveBalances", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM LeaveBalances WHERE Employee_ID=@Employee_ID", con);
            cmd.Parameters.AddWithValue("Employee_ID", textBox1.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            textBox4.Clear();
            textBox5.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sqlstm = "select * from LeaveBalances";
            SqlDataAdapter da = new SqlDataAdapter(sqlstm, con);
            DataSet ds = new System.Data.DataSet();
            da.Fill(ds, "LeaveBalances");
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("DELETE LeaveBalances WHERE  Employee_ID=@Employee_ID", con);
            cmd.Parameters.AddWithValue("Roster_ID", textBox1.Text);
            cmd.ExecuteNonQuery();
            con.Close();

            textBox1.Text = "";
            textBox2.Text = "";

            MessageBox.Show("Successfully Deleted");
        }
    }
}
