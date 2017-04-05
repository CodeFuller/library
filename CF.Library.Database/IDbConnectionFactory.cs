using System.Data;

namespace CF.Library.Database
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
