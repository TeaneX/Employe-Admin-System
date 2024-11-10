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
    public partial class Form10 : Form
    {
        private string connectionString = @"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False";
    
        public Form10()
        {
            InitializeComponent();
            comboBox1.Items.Add("Approve");
            comboBox1.Items.Add("Reject");
            comboBox1.SelectedIndex = 0;
        }
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False");
        private void button1_Click(object sender, EventArgs e)
        {
            string selectedStatus = comboBox1.SelectedItem.ToString();
            int leave_id = int.Parse(textBox1.Text);
            UpdateLeaveApplicationStatus(leave_id, selectedStatus);

        }
        private void UpdateLeaveApplicationStatus(int leave_id, string status)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                if (status == "Approve" && !CanApproveLeave(leave_id, conn))
                {
                    MessageBox.Show("Leave cannot be approved due to insufficient leave balance.");
                    return;
                }

                string updateStatusQuery = "UPDATE LeaveApplications SET status = @status WHERE leave_id = @leave_id";
                using (SqlCommand cmd = new SqlCommand(updateStatusQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@status", status);
                    cmd.Parameters.AddWithValue("@leave_id", leave_id);
                    cmd.ExecuteNonQuery();
                }
                if (status == "Approve")
                {
                    UpdateLeaveBalance(leave_id, conn);
                }

            }
            MessageBox.Show("Leave application status updated successfully.");
           }

        private bool CanApproveLeave(int leave_id, SqlConnection conn)
        {
            string selectQuery = "SELECT leave_type, Employee_ID, start_date, end_date FROM LeaveApplications WHERE leave_id = @leave_id";
            string leave_type = "";
            int employee_ID = 0;

            using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
            {
                cmd.Parameters.AddWithValue("@leave_id", leave_id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        leave_type = reader["leave_type"].ToString();
                        employee_ID = Convert.ToInt32(reader["Employee_ID"]);
                    }
                }
            }

            string balanceQuery = "SELECT Annual_Leaves, Casual_Leaves, Short_Leaves FROM LeaveBalances WHERE employee_ID = @employee_ID";
            using (SqlCommand cmd = new SqlCommand(balanceQuery, conn))
            {
                cmd.Parameters.AddWithValue("@employee_ID", employee_ID);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int Annual_Leaves = Convert.ToInt32(reader["Annual_Leaves"]);
                        int Casual_Leaves = Convert.ToInt32(reader["Casual_Leaves"]);
                        int Short_Leaves = Convert.ToInt32(reader["Short_Leaves"]);

                        // Check leave balance based on leave type
                        switch (leave_type)
                        {
                            case "Annual":
                                return Annual_Leaves > 0;
                            case "Casual":
                                return Casual_Leaves > 0;
                            case "Short":
                                return Short_Leaves > 0;
                            default:
                                return false;
                        }
                    }
                }
            }
            return false; // Default return false if no balance found
        }
        private void UpdateLeaveBalance(int leave_id, SqlConnection conn)
        {
            string selectQuery = "SELECT leave_type, Employee_ID, start_date, end_date FROM LeaveApplications WHERE leave_id = @leave_id";
            string leave_type = "";
            int Employee_ID = 0;
            DateTime start_date = DateTime.MinValue;
            DateTime end_date = DateTime.MinValue;
            int totalLeaveDays = 0;

            using (SqlCommand cmd = new SqlCommand(selectQuery, conn))
            {

                cmd.Parameters.AddWithValue("@leave_id", leave_id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        leave_type = reader["leave_type"].ToString();
                        Employee_ID = Convert.ToInt32(reader["Employee_ID"]);
                        if (reader["start_Date"] != DBNull.Value)
                        {
                            start_date = Convert.ToDateTime(reader["start_date"]);
                        }

                        if (reader["end_date"] != DBNull.Value)
                        {
                            end_date = Convert.ToDateTime(reader["End_Date"]);
                        }

                    }
                }


            }
            string updateBalanceQuery = "";
            switch (leave_type)
            {
                case "Annual":
                    if (end_date > start_date)
                    {
                        totalLeaveDays = (end_date - start_date).Days + 1;
                    }
                    updateBalanceQuery = "UPDATE LeaveBalances SET Annual_Leaves = Annual_Leaves - @totalLeaveDays WHERE employee_ID = @employee_ID ";
                    break;
                case "Casual":
                    updateBalanceQuery = "UPDATE LeaveBalances SET Casual_Leaves = Casual_Leaves - 1 WHERE employee_ID = @employee_ID ";
                    break;
                case "Short":
                    updateBalanceQuery = "UPDATE LeaveBalances SET Short_Leaves_Used  = Short_Leaves_Used  + 1, Short_Leaves = Short_Leaves - 1 WHERE employee_ID = @employee_ID ";
                    break;
                default:
                    MessageBox.Show("Unknown leave type.");
                    return;
            }
            using (SqlCommand cmd = new SqlCommand(updateBalanceQuery, conn))
            {
                cmd.Parameters.AddWithValue("@Employee_ID", Employee_ID);
                if (leave_type == "Annual")
                {
                    cmd.Parameters.AddWithValue("@totalLeaveDays", totalLeaveDays);
                }
                   
                cmd.ExecuteNonQuery();
            }

            MessageBox.Show("Leave balance updated successfully.");

        }

        private void Form10_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM LeaveApplications", con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM LeaveApplications WHERE Employee_ID=@Employee_ID", con);
            cmd.Parameters.AddWithValue("Employee_ID", textBox2.Text);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
        }

        private void button3_Click(object sender, EventArgs e)
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
    }
}



