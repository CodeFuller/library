using System.Data;

namespace CF.Library.Database
{
	/// <summary>
	/// Factory class that creates instances of IDbConnection and opens connection to the database.
	/// </summary>
	public abstract class DbConnectionFactory : IDbConnectionFactory
	{
		/// <summary>
		/// Returns instance of opened DB connection.
		/// </summary>
		public IDbConnection CreateConnection(string connectionString)
		{
			IDbConnection connection = CreateConnectionInstance();
			connection.ConnectionString = connectionString;
			connection.Open();

			return connection;
		}

		/// <summary>
		/// Implementation of IDbConnectionFactory.CreateDbTraits().
		/// </summary>
		/// <returns></returns>
		public IDbTraits CreateDbTraits()
		{
			return CreateDbTraitsInstance();
		}

		/// <summary>
		/// Creates instance of IDbConnection.
		/// </summary>
		protected abstract IDbConnection CreateConnectionInstance();

		/// <summary>
		/// Creates instance of IDbTraits.
		/// </summary>
		protected abstract IDbTraits CreateDbTraitsInstance();
	}
}
