using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication10
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "Admin" && textBox2.Text == "1234")
            {
                MessageBox.Show("Welcome to Grifindor Leave Management System", "WELCOME", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form3 obj = new Form3();
                obj.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Login ERROR", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }
    }
}
