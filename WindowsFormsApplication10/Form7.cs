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
    public partial class Form7 : Form
    {
        public Form7()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False");
        private void button1_Click(object sender, EventArgs e)
        {
            con.Open();
            string userID = textBox1.Text;
            string password = textBox2.Text;

            string query_select = "SELECT * FROM Employee WHERE Employee_ID ='" + userID + "'AND Password ='" + password + "'";
            SqlCommand cmnd = new SqlCommand(query_select, con);
            SqlDataReader row = cmnd.ExecuteReader();

            if (row.HasRows)
            {
                this.Hide();
                Form8 obj = new Form8();
                obj.Show();

            }
            else
            {

                MessageBox.Show("Invalid Login Credintials,please check username and password and try again !", "Invalid Login Details", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            con.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Are you sure, Do you really want to EXIT...!", "EXIT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Form1 obj = new Form1();
                obj.Show();
                this.Hide();
            }
            else
            {

            }
        }

        private void Form7_Load(object sender, EventArgs e)
        {

        }
    }
}
