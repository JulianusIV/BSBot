using System;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Collections.Generic;

using BSBot.Objects;

namespace BSBot.Repositories
{
	public class ExamRepository : BaseRepository
	{
		public ExamRepository(string connectionString) : base(connectionString) { }

		public bool GetAll(out List<Exam> list)
		{
			bool result = false;
			list = null;

			try
			{
				DbCommand.CommandText = "SELECT * FROM Exams;";
				DbCommand.Parameters.Clear();
				DbConnection.Open();
				using SqlDataReader reader = DbCommand.ExecuteReader();
				list = new List<Exam>();
				while (reader.Read())
				{
					list.Add(new Exam
					{
						Id = (int)reader["Id"],
						DueDate = Date.Parse((string)reader["DueDate"]),
						MessageId = (ulong)(long)reader["MessageId"],
						Subject = (string)reader["Subject"],
						Text = (string)reader["Text"]
					});
				}
				DbConnection.Close();

				result = true;
			}
			catch (Exception e)
			{
				//TODO: add logging
				Debug.WriteLine(e.Message);
			}
			finally
			{
				if (DbConnection.State == System.Data.ConnectionState.Open)
				{
					DbConnection.Close();
				}
			}

			return result;
		}

		public bool GetByMessageId(ulong messageId, out Exam entity)
		{
			bool result = false;
			entity = null;

			try
			{
				DbCommand.CommandText = "SELECT * FROM Exams WHERE MessageId = @id;";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("id", (long)messageId);
				DbConnection.Open();
				using SqlDataReader reader = DbCommand.ExecuteReader();
				reader.Read();
				entity = new Exam
				{
					Id = (int)reader["ID"],
					DueDate = Date.Parse((string)reader["DueDate"]),
					MessageId = messageId,
					Subject = (string)reader["Subject"],
					Text = (string)reader["Text"]
				};
				DbConnection.Close();

				result = true;
			}
			catch (Exception e)
			{
				//TODO: add logging
				Debug.WriteLine(e.Message);
			}
			finally
			{
				if (DbConnection.State == System.Data.ConnectionState.Open)
				{
					DbConnection.Close();
				}
			}

			return result;
		}

		public bool Create(ref Exam entity)
		{
			bool result = false;

			try
			{
				DbCommand.CommandText = "INSERT INTO Exams (DueDate, MessageId, Text, Subject) OUTPUT INSERTED.Id VALUES (@date, @message, @text, @subject);";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("date", entity.DueDate.ToString());
				DbCommand.Parameters.AddWithValue("message", (long)entity.MessageId);
				DbCommand.Parameters.AddWithValue("text", entity.Text);
				DbCommand.Parameters.AddWithValue("subject", entity.Subject);
				DbConnection.Open();
				entity.Id = (int)DbCommand.ExecuteScalar();
				DbConnection.Close();

				result = true;
			}
			catch (Exception e)
			{
				//TODO: add logging
				Debug.WriteLine(e.Message);
			}
			finally
			{
				if (DbConnection.State == System.Data.ConnectionState.Open)
				{
					DbConnection.Close();
				}
			}

			return result;
		}

		public bool Read(int id, out Exam entity)
		{
			bool result = false;
			entity = null;

			try
			{
				DbCommand.CommandText = "SELECT * FROM Exams WHERE Id = @id;";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("id", id);
				DbConnection.Open();
				using SqlDataReader reader = DbCommand.ExecuteReader();
				reader.Read();
				long temp = (long)reader["MessageId"];
				entity = new Exam
				{
					Id = id,
					DueDate = Date.Parse((string)reader["DueDate"]),
					MessageId = (ulong)temp,
					Subject = (string)reader["Subject"],
					Text = (string)reader["Text"]
				};
				DbConnection.Close();

				result = true;
			}
			catch (Exception e)
			{
				//TODO: add logging
				Debug.WriteLine(e.Message);
			}
			finally
			{
				if (DbConnection.State == System.Data.ConnectionState.Open)
				{
					DbConnection.Close();
				}
			}

			return result;
		}

		public bool Update(Exam entity)
		{
			bool result = false;

			try
			{
				DbCommand.CommandText = "UPDATE Exams SET DueDate = @date, MessageId = @message, Text = @text, Subject = @subj WHERE Id = @id;";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("date", entity.DueDate.ToString());
				DbCommand.Parameters.AddWithValue("message", (long)entity.MessageId);
				DbCommand.Parameters.AddWithValue("text", entity.Text);
				DbCommand.Parameters.AddWithValue("subj", entity.Subject);
				DbCommand.Parameters.AddWithValue("id", entity.Id);
				DbConnection.Open();
				DbCommand.ExecuteNonQuery();
				DbConnection.Close();

				result = true;
			}
			catch (Exception e)
			{
				//TODO: add logging
				Debug.WriteLine(e.Message);
			}
			finally
			{
				if (DbConnection.State == System.Data.ConnectionState.Open)
				{
					DbConnection.Close();
				}
			}

			return result;
		}

		public bool Delete(int id)
		{
			bool result = false;

			try
			{
				DbCommand.CommandText = "DELETE FROM Exams WHERE Id = @id;";
				DbCommand.Parameters.Clear();
				DbCommand.Parameters.AddWithValue("id", id);
				DbConnection.Open();
				DbCommand.ExecuteNonQuery();
				DbConnection.Close();

				result = true;
			}
			catch (Exception e)
			{
				//TODO: add logging
				Debug.WriteLine(e.Message);
			}
			finally
			{
				if (DbConnection.State == System.Data.ConnectionState.Open)
				{
					DbConnection.Close();
				}
			}

			return result;
		}
	}
}
