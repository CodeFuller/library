using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Extensions.Database;
using Npgsql;

namespace CF.Database.PostgreSql
{
	/// <summary>
	/// Implements abstract methods of DbConnectionFactory for PostgreSql database.
	/// </summary>
	public class PostgreSqlConnectionFactory : DbConnectionFactory
	{
		/// <summary>
		/// Creates instance of NpgsqlConnection.
		/// </summary>
		/// <returns></returns>
		protected override IDbConnection CreateConnectionInstance()
		{
			return new NpgsqlConnection();
		}

		/// <summary>
		/// Creates instance of PostgreSql.DbTraits.
		/// </summary>
		protected override IDbTraits CreateDbTraitsInstance()
		{
			return new DbTraits();
		}
	}
}
