using System;
using System.Globalization;
using Newtonsoft.Json;

namespace CF.Library.Json
{
	/// <remarks>
	/// http://stackoverflow.com/questions/19971494/how-to-deserialize-a-unix-timestamp-μs-to-a-datetime
	/// </remarks>
	public class DateTimeJsonConverter : JsonConverter
	{
		private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		/// <summary>
		/// Determines whether this instance can convert the specified object type
		/// </summary>
		public override bool CanConvert(Type objectType)
		{
			return objectType == typeof(DateTime);
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

			return Epoch.AddSeconds((long)reader.Value);
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

			writer.WriteRawValue(((DateTime)value - Epoch).TotalSeconds.ToString(CultureInfo.InvariantCulture));
		}
	}
}
