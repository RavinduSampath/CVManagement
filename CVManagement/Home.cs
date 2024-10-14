using CVManagement.Models;
using CVManagement.Repositoris;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CVManagement
{
    public partial class Home : Form
    {


        private void ReadCandidates()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("FirstName");
            dt.Columns.Add("LastName");
            dt.Columns.Add("Section");
            dt.Columns.Add("Date");
            dt.Columns.Add("Marks");
            dt.Columns.Add("Email");
            dt.Columns.Add("Address");
            dt.Columns.Add("Phone");
            dt.Columns.Add("Resume"); // This will represent the resume link

            var repo = new CandidateRepository();
            var candidates = repo.GetAllCandidates();

            foreach (var candidate in candidates)
            {
                var row = dt.NewRow();
                row["Id"] = candidate.Id;
                row["FirstName"] = candidate.FirstName;
                row["LastName"] = candidate.LastName;
                row["Section"] = candidate.Section;
                row["Date"] = candidate.Date;
                row["Marks"] = candidate.Marks;
                row["Email"] = candidate.Email;
                row["Address"] = candidate.Address;
                row["Phone"] = candidate.Phone;
                row["Resume"] = candidate.Resume; // Assuming this is the path to the resume
                dt.Rows.Add(row);
            }

            this.Candidates.DataSource = dt;

            // Check if the Resume column already exists to avoid adding it multiple times
            if (this.Candidates.Columns.Contains("Resume"))
            {
                DataGridViewLinkColumn linkColumn = new DataGridViewLinkColumn();
                linkColumn.HeaderText = "Resume"; // The column header
                linkColumn.DataPropertyName = "Resume"; // Binds to the resume/document path
                linkColumn.Name = "Resume";
                linkColumn.Text = "View Resume";  // The display text in the link
                linkColumn.UseColumnTextForLinkValue = true; // Use the same text for all rows
                this.Candidates.Columns.Add(linkColumn);
            }


            // Ensure the event handler is registered only once
            this.Candidates.CellContentClick -= Candidates_CellContentClick; // Unsubscribe first
            this.Candidates.CellContentClick += Candidates_CellContentClick; // Then subscribe

        }

        private void Candidates_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the click is on the link column
            if (this.Candidates.Columns[e.ColumnIndex] is DataGridViewLinkColumn && e.RowIndex >= 0)
            {
                // Get the document path from the clicked row
                string documentPath = this.Candidates.Rows[e.RowIndex].Cells["Resume"].Value.ToString();

                try
                {
                    // Open the document (PDF, DOC, etc.) with the default system application
                    System.Diagnostics.Process.Start(documentPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening document: " + ex.Message);
                }
            }
        }

        public Home()
        {
            InitializeComponent();
            ReadCandidates();

            label2.Text = Count().ToString();
            label3.Text = MarksCount().ToString();
        }

        private void Home_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddNew signUp = new AddNew();
            signUp.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var val =this.Candidates.SelectedRows[0].Cells["Id"].Value.ToString();
            if (val == null || val.Length == 0) return;
             int candidateId = int.Parse(val);

            var repo = new CandidateRepository();
            var candidate = repo.GetCandidate(candidateId);

            if (candidate == null)return;

            AddNew signUp = new AddNew();
            signUp.EditCandidate(candidate);
            this.Hide();
            if (signUp.ShowDialog() == DialogResult.OK)
            {
                ReadCandidates();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            var val = this.Candidates.SelectedRows[0].Cells["Id"].Value.ToString();
            if (val == null || val.Length == 0) return;
            int CandidateId = int.Parse(val);

            DialogResult dialogResult = MessageBox.Show("Are you sure you want to delete this candidate?", "Delete Candidate", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                var repo = new CandidateRepository();
                repo.RemoveCandidate(CandidateId);
                ReadCandidates();
            }
            else
            {
                return;

            }
        }

        private void button4_Click(object sender, EventArgs e)
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
        public int Count()
        {
            var repo = new CandidateRepository();
            return repo.Count();

        }
        public int MarksCount()
        {
            var repo = new CandidateRepository();
            return repo.MarksCount();

        }
    }
}
