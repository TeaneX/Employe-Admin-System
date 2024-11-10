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
    public partial class Form9 : Form
    {
        public Form9()
        {
            InitializeComponent();
            comboBox1.Items.Add("Annual");
            comboBox1.Items.Add("Casual");
            comboBox1.Items.Add("Short");
            comboBox1.SelectedIndex = 0;
        }
        SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False");

        private void Form9_Load(object sender, EventArgs e)
        {

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

        private void button4_Click(object sender, EventArgs e)
        {
            int Employee_ID;
            if (!int.TryParse(textBox1.Text, out Employee_ID))
            {
                MessageBox.Show("Invalid Employee_ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string leave_type = comboBox1.SelectedItem.ToString();
            DateTime start_date = dateTimePicker3.Value;
            DateTime? end_date = leave_type == "Annual" || leave_type == "Casual" ? (DateTime?)dateTimePicker2.Value : null; // End date for Annual or Casual leave
            TimeSpan? short_leave_start_time = leave_type == "Short" ? (TimeSpan?)TimeSpan.Parse(dateTimePicker1.Text) : null;
            TimeSpan? short_leave_end_time = leave_type == "Short" ? (TimeSpan?)TimeSpan.Parse(dateTimePicker5.Text) : null;
            string reason = textBox2.Text;


            if (leave_type == "Annual")
            {
                // Annual leave validation: Ensure start date is at least 7 days in advance
                if ((start_date - DateTime.Now).TotalDays < 7)
                {
                    MessageBox.Show("Annual leave must be applied at least 7 days in advance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!end_date.HasValue)
                {
                    MessageBox.Show("End date must be specified for Annual leave.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (leave_type == "Casual")
            {
                // Casual leave validation: Ensure start date is before the roster start date
                if (!IsRosterStarted(Employee_ID))
                {
                    MessageBox.Show("Casual leave cannot be applied after the roster has started.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!end_date.HasValue)
                {
                    MessageBox.Show("End date must be specified for Casual leave.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (leave_type == "Short")
            {
                // Short leave validation: Ensure times are valid and the leave duration is 1.5 hours max
                if (short_leave_start_time == null || short_leave_end_time == null || (short_leave_end_time - short_leave_start_time)?.TotalMinutes > 90)
                {
                    MessageBox.Show("Short leave duration cannot exceed 1 hour 30 minutes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (end_date.HasValue)
                {
                    MessageBox.Show("End date should not be specified for Short leave.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            // Check leave balances and apply leave
            if (!CheckAndApplyLeave(Employee_ID, leave_type, start_date, end_date, reason, short_leave_start_time, short_leave_end_time))
            {
                MessageBox.Show("Leave application failed. Please check leave balances or try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Leave application submitted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }


        private bool CheckAndApplyLeave(int Employee_ID, string leave_type, DateTime start_date, DateTime? end_date, string reason, TimeSpan? short_leave_start_time, TimeSpan? short_leave_end_time)
        {
            int Annual_Leaves = 0;
            int Casual_Leaves = 0;
            int Short_Leaves_Used = 0;

            // Retrieve current leave balances and short leave usage
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False"))
            {
                string balanceQuery = @"
                SELECT Annual_Leaves, Casual_Leaves, Short_Leaves_Used
                FROM LeaveBalances
                WHERE Employee_ID = @Employee_ID";

                using (SqlCommand cmd = new SqlCommand(balanceQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Employee_ID", Employee_ID);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Annual_Leaves = reader.GetInt32(0);
                            Casual_Leaves = reader.GetInt32(1);
                            Short_Leaves_Used = reader.GetInt32(2);
                        }
                        else
                        {
                            MessageBox.Show("Employee not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                    }
                }

                // Validate leave balances
                if (leave_type == "Annual")
                {
                    int annualLeaveDaysRequested = (end_date.Value - start_date).Days + 1; // inclusive of start and end dates
                    if (annualLeaveDaysRequested > Annual_Leaves)
                    {
                        MessageBox.Show("Insufficient annual leave balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (leave_type == "Casual")
                {
                    int casualLeaveDaysRequested = (end_date.Value - start_date).Days + 1; // inclusive of start and end dates
                    if (casualLeaveDaysRequested > Casual_Leaves)
                    {
                        MessageBox.Show("Insufficient casual leave balance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else if (leave_type == "Short")
                {
                    // Assuming short leave balance is tracked monthly
                    if (Short_Leaves_Used >= 2) // Assuming 2 short leaves per month limit
                    {
                        MessageBox.Show("Short leave balance exceeded for this month.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                // Insert leave application into the database
                string insertQuery = @"
                INSERT INTO LeaveApplications (
                    Employee_ID,
                    leave_type,
                    start_date,
                    end_date,
                    short_leave_start_time,
                    short_leave_end_time,
                    reason,
                    status,
                    application_date
                   
                ) VALUES (
                    @Employee_ID,
                    @leave_type,
                    @start_date,
                    @end_date,
                    @short_leave_start_time,
                    @short_leave_end_time,
                    @reason,
                    'Pending',
                    GETDATE()
                )";

                using (SqlCommand cmd = new SqlCommand(insertQuery, con))
                {
                    cmd.Parameters.AddWithValue("@Employee_ID", Employee_ID);
                    cmd.Parameters.AddWithValue("@leave_type", leave_type);
                    cmd.Parameters.AddWithValue("@start_date", start_date);
                    cmd.Parameters.AddWithValue("@end_date", (object)end_date ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@short_leave_start_time", (object)short_leave_start_time ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@short_leave_end_time", (object)short_leave_end_time ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@reason", reason);

                    cmd.ExecuteNonQuery();
                }
            }


            return true;
        }
        private bool IsRosterStarted(int Employee_ID)
        {
            DateTime currentDate = DateTime.Now;
            using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-LBMM56I\SQLEXPRESS;Initial Catalog=Grifindor_Leave_Management030;Integrated Security=True;Encrypt=False"))
            {
                string query = @"
            SELECT Roster_Start_Date
            FROM Roster
            WHERE Employee_ID = @Employee_ID AND @currentDate >= Roster_Start_Date";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Employee_ID", Employee_ID);
                    cmd.Parameters.AddWithValue("@currentDate", currentDate);

                    con.Open();
                    var result = cmd.ExecuteScalar();

                    return result != null; // Returns true if the roster has started
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }
    }
}

