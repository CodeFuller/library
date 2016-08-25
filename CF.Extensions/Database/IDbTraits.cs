using System.Data;

namespace CF.Extensions.Database
{
	/// <summary>
	/// Interface that accommodates all database specifics.
	/// </summary>
	public interface IDbTraits
	{
		/// <summary>
		/// Maps a CLR enum type to native database enum type.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Registration is performed for whole enum type, not for specific value")]
		void MapEnum<TEnum>(IDbConnection connection) where TEnum : struct;
	}
}
