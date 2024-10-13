using CVManagement.Models;
using CVManagement.Repositoris;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVManagement
{
    public partial class AddNew : Form
    {
        public AddNew()
        {
            InitializeComponent();
        }

        private void AddNew_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private int CandidateId=0;
        public void EditCandidate(Candidate candidate)
        {
            this.Text = "Edit Candidate";


            textBox1.Text = candidate.FirstName;
            textBox2.Text = candidate.LastName;
            textBox3.Text = candidate.Address;
            textBox4.Text = candidate.Phone;
            textBox5.Text = candidate.Marks.ToString();
            textBox6.Text = candidate.Section;
            textBox7.Text = candidate.Email;
            label10.Text = candidate.Resume;
            dateTimePicker1.Value = candidate.Date;

            this.CandidateId = candidate.Id;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //To where your opendialog box get starting location. My initial directory location is desktop.
            openFileDialog1.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            //Your opendialog box title name.
            openFileDialog1.Title = "Select file to be upload.";
            //which type file format you want to upload in database. just add them.
            openFileDialog1.Filter = "Select Valid Document(*.pdf; *.doc; *.xlsx; *.html)|*.pdf; *.docx; *.xlsx; *.html";
            //FilterIndex property represents the index of the filter currently selected in the file dialog box.
            openFileDialog1.FilterIndex = 1;
            try
            {
                if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if (openFileDialog1.CheckFileExists)
                    {
                        string path = System.IO.Path.GetFullPath(openFileDialog1.FileName);
                        label10.Text = path;
                    }
                }
                else
                {
                    MessageBox.Show("Please Upload document.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button5_Click(object sender, EventArgs e)
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

        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();   
            textBox5.Clear();
            textBox6.Clear();
            textBox7.Clear();
            textBox1.Focus();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = System.IO.Path.GetFileName(openFileDialog1.FileName);
                if (filename == null)
                {
                    MessageBox.Show("Please select a valid document.");
                }
                else
                {
                    //we already define our connection globaly. We are just calling the object of connection.
                   
                    string path = Application.StartupPath.Substring(0, (Application.StartupPath.Length - 10));
                    System.IO.File.Copy(openFileDialog1.FileName, path + "\\Document\\" + filename);
                    
                    MessageBox.Show("Document uploaded.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Candidate candidate = new Candidate();

            // Input validation
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||
                string.IsNullOrWhiteSpace(textBox2.Text) ||
                !int.TryParse(textBox5.Text, out int marks) ||
                string.IsNullOrWhiteSpace(textBox7.Text) ||
                string.IsNullOrWhiteSpace(textBox3.Text) ||
                string.IsNullOrWhiteSpace(textBox4.Text) ||
                string.IsNullOrEmpty(label10.Text)) // Check if a file is selected
            {
                MessageBox.Show("Please fill in all required fields correctly.");
                return;
            }
            candidate.Id = this.CandidateId;
            candidate.FirstName = textBox1.Text;
            candidate.LastName = textBox2.Text;
            candidate.Section = textBox6.Text;
            candidate.Date = dateTimePicker1.Value;
            candidate.Marks = marks; // Assign validated marks
            candidate.Email = textBox7.Text;
            candidate.Address = textBox3.Text;
            candidate.Phone = textBox4.Text;
            candidate.Resume = label10.Text;


            if (CandidateId == 0)
            {
                try
                {
                    var repo = new CandidateRepository();
                    repo.AddCandidate(candidate);
                    MessageBox.Show("Candidate added successfully!");

                    // Navigating to Home
                    Home home = new Home();
                    home.Show(); // Show home form without blocking
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error adding candidate: " + ex.Message);
                }
                
            }

            else
            {
                var repo = new CandidateRepository();
                repo.UpdateCandidate(candidate);
                MessageBox.Show("Candidate updated successfully!");

                // Navigating to Home
                Home home = new Home();
                home.Show(); // Show home form without blocking
                this.Hide();
            }
        }

    }
}
