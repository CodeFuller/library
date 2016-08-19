using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CF.Extensions.Json
{
	/// <summary>
	/// Implementation of JsonConverter that performs HTML encoding/decoding for the data
	/// </summary>
	public class HtmlEncodingConverter : Newtonsoft.Json.JsonConverter
	{
		/// <summary>
		/// Determines whether this instance can convert the specified object type
		/// </summary>
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(String);
		}

		/// <summary>
		/// Reads the JSON representation of the object
		/// </summary>
		public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
		{
			if (reader == null)
			{
				throw new ArgumentNullException(nameof(reader));
			}

			return System.Web.HttpUtility.HtmlDecode((string)reader.Value);
		}

		/// <summary>
		/// Writes the JSON representation of the object
		/// </summary>
		public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
		{
			if (writer == null)
			{
				throw new ArgumentNullException(nameof(writer));
			}

			writer.WriteRawValue(System.Web.HttpUtility.HtmlEncode((string)value));
		}
	}
}
