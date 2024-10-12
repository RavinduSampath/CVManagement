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

namespace CVManagement
{
    public partial class SignUp : Form
    {
        public SignUp()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox1.Focus();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                Application.Exit();
            }
            else
            {
                this.Show();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form1 signin = new Form1();
            signin.Show();
            this.Hide();
        }
        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-TD1B2SN\SQLEXPRESS;Initial Catalog=CVManagement;Integrated Security=True;Persist Security Info=False;Pooling=False");


        private void button1_Click(object sender, EventArgs e)
        {
            String UserName = textBox1.Text;
            String Password = textBox2.Text;
            String ConfirmPassword = textBox3.Text;


            if (Password != ConfirmPassword)
            {
                MessageBox.Show("Passwords do not match", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                textBox2.Clear();
                textBox3.Clear();
                textBox2.Focus();
                return;
            }
            else {
            try
            {
                // Open the connection
                conn.Open();

                // Use parameterized query to prevent SQL Injection
                String query = "INSERT INTO [User] (UserName, Password) VALUES (@UserName, @Password)";
                SqlCommand cmd = new SqlCommand(query, conn);

                // Add parameters to the query
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Password", Password);

                int result = cmd.ExecuteNonQuery();

                if (result == 1) // Single match found
                {
                    // Successful login
                    MessageBox.Show("Sign Up Successful");
                    this.Hide();

                    // Load next page (Home)
                    Home f2 = new Home();
                    f2.Show();
                }
                else
                {
                    // Failed login
                    MessageBox.Show("Sign Up Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                conn.Close();
            }
            }

        }
    }
}
