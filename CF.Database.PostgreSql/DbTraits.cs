using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CF.Extensions.Database;
using CF.Core.Exceptions.Database;
using Npgsql;
using static System.FormattableString;
using static CF.Core.FormattableStringExtensions;

namespace CF.Database.PostgreSql
{
	public class DbTraits : IDbTraits
	{
		/// <summary>
		/// Implementation of IDbTraits.MapEnum()
		/// </summary>
		public void MapEnum<TEnum>(IDbConnection connection) where TEnum : struct
		{
			NpgsqlConnection pgSqlConnection = (NpgsqlConnection)connection;
			if (pgSqlConnection == null)
			{
				throw new ArgumentException(Current($"Connection should has type of {typeof(NpgsqlConnection)}"));
			}

			pgSqlConnection.MapEnum<TEnum>();
		}
	}
}
