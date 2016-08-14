using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CF.Extensions.Database
{
	/// <summary>
	/// Interface that accommodates all database specifics
	/// </summary>
	public interface IDbTraits
	{
		/// <summary>
		/// Performs any initialization required by specific DB connector.
		/// </summary>
		void PrepareConnection(IDbConnection connection);

		/// <summary>
		/// Builds parameter name that could be used in SQL query.
		/// </summary>
		string GetParameterId(int index, string name);

		/// <summary>
		/// Converts CLR object to DB object.
		/// </summary>
		object SerializeValue(object value);

		/// <summary>
		/// Converts DB object to CLR object.
		/// </summary>
		object DeserializeValue(object data, Type outputType);

		/// <summary>
		/// Maps a CLR enum type to native database enum type
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Registration is performed for whole enum type, not for specific value")]
		void MapEnum<TEnum>(IDbConnection connection) where TEnum : struct;
	}
}
