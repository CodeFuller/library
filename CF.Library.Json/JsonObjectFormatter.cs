using System.Web.Script.Serialization;

namespace CF.Library.Json
{
	/// <summary>
	/// Holder for ToJson() extension method
	/// </summary>
	public static class JsonObjectFormatter
	{
		/// <summary>
		/// Is used for human-friendly dump of any object
		/// </summary>
		public static string ToJson(this object value)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			return serializer.Serialize(value);
		}
	}
}
