using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CF.Extensions.Json
{
	/// <summary>
	/// Holder for ToJson() extension method
	/// </summary>
	public static class JsonObjectFormatter
	{
		/// <summary>
		/// Is used for human-friendly dump of any object
		/// </summary>
		public static string ToJson(this object obj)
		{
			JavaScriptSerializer serializer = new JavaScriptSerializer();
			return serializer.Serialize(obj);
		}
	}
}
