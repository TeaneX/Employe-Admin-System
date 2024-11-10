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
    public partial class Form13 : Form
    {
        public Form13()
        {
            InitializeComponent();
        }
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False");
        private void Form13_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM LeaveApplications", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM LeaveApplications WHERE Employee_ID=@Employee_ID", con);
            cmd.Parameters.AddWithValue("Employee_ID", textBox2.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False"))
            {
                try
                {
                    con.Open();
                    string statusQuery = "SELECT status FROM LeaveApplications WHERE leave_id = @leave_id";
                    SqlCommand statusCmd = new SqlCommand(statusQuery, con);
                    statusCmd.Parameters.AddWithValue("@leave_id", textBox1.Text);
                    var statusResult = statusCmd.ExecuteScalar();

                    if (statusResult == null)
                    {
                        MessageBox.Show("No application found with the provided leave ID.");
                        return;
                    }

                    string status = statusResult.ToString();

                    if (status != "Pending")
                    {
                        MessageBox.Show($"Cannot delete. Current status is '{status}'. Only pending applications can be deleted.");
                        return;
                    }

                    SqlCommand deleteCmd = new SqlCommand("DELETE FROM LeaveApplications WHERE leave_id = @leave_id", con);
                    deleteCmd.Parameters.AddWithValue("@leave_id", textBox1.Text);
                    int rowsAffected = deleteCmd.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Successfully Deleted");
                    }
                    else
                    {
                        MessageBox.Show("No application found with the provided leave ID.");
                    }
                }
                catch (Exception ex)
                {

                    MessageBox.Show($"An error occurred: {ex.Message}");

                }
                finally
                {
                    con.Close();
                }
            }
            textBox1.Text = "";
            textBox2.Text = "";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string sqlstm = "select * from LeaveApplications";
            SqlDataAdapter da = new SqlDataAdapter(sqlstm, con);
            DataSet ds = new System.Data.DataSet();
            da.Fill(ds, "LeaveApplications");
            dataGridView1.DataSource = ds.Tables[0];
        }

        private void button2_Click(object sender, EventArgs e)
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
