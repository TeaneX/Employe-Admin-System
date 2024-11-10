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
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
            comboBox1.Items.Add("New");
            comboBox1.Items.Add("Permenent");
            comboBox1.SelectedIndex = 0;
        }
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False");

        private void button4_Click(object sender, EventArgs e)
        {
            con.Open();
            SqlCommand cmd = new SqlCommand("insert into  Employee values(@Employee_ID,@Name,@Password,@Date_of_Joined,@Employee_State)", con);
            cmd.Parameters.AddWithValue("@Employee_ID", int.Parse(textBox1.Text));
            cmd.Parameters.AddWithValue("@Name", textBox2.Text);
            cmd.Parameters.AddWithValue("@Password", int.Parse(textBox3.Text));
            cmd.Parameters.AddWithValue("@Date_of_Joined", dateTimePicker1.Value.Date);
            cmd.Parameters.AddWithValue("@Employee_State", comboBox1.SelectedItem.ToString());
            cmd.ExecuteNonQuery();
            con.Close();

            MessageBox.Show("Successfully Registered");
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

        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();

            SqlCommand cmd = new SqlCommand("DELETE Employee WHERE Employee_ID=@Employee_ID", con);
            cmd.Parameters.AddWithValue("Employee_ID", textBox1.Text);
            cmd.ExecuteNonQuery();
            con.Close();

            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            MessageBox.Show("Successfully Deleted");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employee WHERE Employee_ID=@Employee_ID", con);
            cmd.Parameters.AddWithValue("Employee_ID", textBox1.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sqlstm = "select * from  Employee ";
            SqlDataAdapter da = new SqlDataAdapter(sqlstm, con);
            DataSet ds = new System.Data.DataSet();
            da.Fill(ds, " Employee");
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
           
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM Employee", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }
    }
}
