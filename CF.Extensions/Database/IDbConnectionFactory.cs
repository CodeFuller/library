using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace CF.Extensions.Database
{
	/// <summary>
	/// 
	/// </summary>
	public interface IDbConnectionFactory
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="connectionString"></param>
		/// <returns></returns>
		IDbConnection CreateConnection(string connectionString);

		/// <summary>
		/// Creates instance of IDbTraits.
		/// </summary>
		IDbTraits CreateDbTraits();
	}
}
