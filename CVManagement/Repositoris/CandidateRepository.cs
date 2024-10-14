using CVManagement.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace CVManagement.Repositoris
{
    public class CandidateRepository
    {
        private readonly string connectionString = "Data Source=DESKTOP-TD1B2SN\\SQLEXPRESS;Initial Catalog=CVManagement;Integrated Security=True";

        public List<Candidate> GetAllCandidates()
        {
            List<Candidate> candidates = new List<Candidate>();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Details ORDER BY Marks DESC";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Candidate candidate = new Candidate
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Section = reader.GetString(reader.GetOrdinal("Section")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("Interviw_date")),
                                    Marks = reader.GetInt32(reader.GetOrdinal("Marks")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                    Resume = reader.GetString(reader.GetOrdinal("Document"))
                                };

                                candidates.Add(candidate);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                // Consider logging the exception
            }
            return candidates;
        }

        public Candidate? GetCandidate(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "SELECT * FROM Details WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                return new Candidate
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                    FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                                    LastName = reader.GetString(reader.GetOrdinal("LastName")),
                                    Section = reader.GetString(reader.GetOrdinal("Section")),
                                    Date = reader.GetDateTime(reader.GetOrdinal("Interviw_date")),
                                    Marks = reader.GetInt32(reader.GetOrdinal("Marks")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Address = reader.GetString(reader.GetOrdinal("Address")),
                                    Phone = reader.GetString(reader.GetOrdinal("Phone")),
                                    Resume = reader.GetString(reader.GetOrdinal("Document"))
                                };
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                // Consider logging the exception
            }

            return null;
        }

        public void AddCandidate(Candidate candidate)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Details (FirstName, LastName, Section, Interviw_date, Marks, Email, Address, Phone, Document) " +
                               "VALUES (@FirstName, @LastName, @Section, @Date, @Marks, @Email, @Address, @Phone, @Resume)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@FirstName", candidate.FirstName);
                    command.Parameters.AddWithValue("@LastName", candidate.LastName);
                    command.Parameters.AddWithValue("@Section", candidate.Section);
                    command.Parameters.AddWithValue("@Date", candidate.Date);
                    command.Parameters.AddWithValue("@Marks", candidate.Marks);
                    command.Parameters.AddWithValue("@Email", candidate.Email);
                    command.Parameters.AddWithValue("@Address", candidate.Address);
                    command.Parameters.AddWithValue("@Phone", candidate.Phone);
                    command.Parameters.AddWithValue("@Resume", candidate.Resume);

                    int result = command.ExecuteNonQuery(); // Executes the command
                    if (result <= 0)
                    {
                        throw new Exception("Failed to add candidate to the database.");
                    }
                }
            }
        }

        public void UpdateCandidate(Candidate candidate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "UPDATE Details SET FirstName = @FirstName, LastName = @LastName, Section = @Section, " +
                                   "Interviw_date = @Date, Marks = @Marks, Email = @Email, Address = @Address, Phone = @Phone, Document = @Resume " +
                                   "WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", candidate.Id);
                        cmd.Parameters.AddWithValue("@FirstName", candidate.FirstName);
                        cmd.Parameters.AddWithValue("@LastName", candidate.LastName);
                        cmd.Parameters.AddWithValue("@Section", candidate.Section);
                        cmd.Parameters.AddWithValue("@Date", candidate.Date);
                        cmd.Parameters.AddWithValue("@Marks", candidate.Marks);
                        cmd.Parameters.AddWithValue("@Email", candidate.Email);
                        cmd.Parameters.AddWithValue("@Address", candidate.Address);
                        cmd.Parameters.AddWithValue("@Phone", candidate.Phone);
                        cmd.Parameters.AddWithValue("@Resume", candidate.Resume);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                // Consider logging the exception
            }
        }

        public void RemoveCandidate(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    string query = "DELETE FROM Details WHERE Id = @Id";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Id", id);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                // Consider logging the exception
            }
        }
        public int Count()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Details";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    int count = (int)cmd.ExecuteScalar();
                    return count;
                }
            }
        }
        public int MarksCount()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM Details WHERE Marks > 60;";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    int MarksCount = (int)cmd.ExecuteScalar();
                    return MarksCount;
                }
            }
        }
    }
}
