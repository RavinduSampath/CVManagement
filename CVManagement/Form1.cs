using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace CVManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection conn = new SqlConnection(@"Data Source=DESKTOP-TD1B2SN\SQLEXPRESS;Initial Catalog=CVManagement;Integrated Security=True;Persist Security Info=False;Pooling=False");

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String UserName = textBox1.Text;
            String Password = textBox2.Text;

            try
            {
                // Open the connection
                conn.Open();

                // Use parameterized query to prevent SQL Injection
                String query = "SELECT * FROM [User] WHERE UserName = @UserName AND Password = @Password";
                SqlCommand cmd = new SqlCommand(query, conn);

                // Add parameters to the query
                cmd.Parameters.AddWithValue("@UserName", UserName);
                cmd.Parameters.AddWithValue("@Password", Password);

                SqlDataAdapter sda = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                sda.Fill(dt);

                if (dt.Rows.Count == 1) // Single match found
                {
                    // Successful login
                    MessageBox.Show("Login Successful");
                    this.Hide();

                    // Load next page (Home)
                    Home f2 = new Home();
                    f2.Show();
                }
                else
                {
                    // Failed login
                    MessageBox.Show("Login Failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Focus();
                }
            }
            catch (Exception ex)
            {
                // Display the actual error message for better debugging
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            finally
            {
                // Ensure the connection is always closed
                conn.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Clear input fields and set focus back to username
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
            SignUp signUp = new SignUp();
            signUp.Show();
            this.Hide();    
        }
    }
}
