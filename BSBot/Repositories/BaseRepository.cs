using System;
using System.Data.SqlClient;
using System.Diagnostics;

namespace BSBot.Repositories
{
	public class BaseRepository
	{
		public SqlConnection DbConnection { get; set; }
		public SqlCommand DbCommand { get; set; }

		public string ConnectionString { get; set; }

		public BaseRepository(string connectionString)
		{
			ConnectionString = connectionString;
			DbConnection = new SqlConnection(connectionString);
			DbCommand = new SqlCommand("", DbConnection);
		}

		public bool Check()
		{
			bool success = false;
			try
			{
				DbConnection.Open();
				DbConnection.Close();

				success = true;
			}
			catch (Exception ex)
			{
				//TODO: add logging
				Debug.WriteLine(ex);
			}

			return success;
		}
	}
}
