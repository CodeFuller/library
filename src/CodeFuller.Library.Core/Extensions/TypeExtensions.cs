using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace CodeFuller.Library.Core.Extensions
{
	/// <summary>
	/// Holder for generic types extension methods
	/// </summary>
	public static class TypeExtensions
	{
		/// <summary>
		/// Get the friendly string for enum value
		/// </summary>
		/// <remarks>
		/// Copy/Paste from http://stackoverflow.com/a/479417/5740031
		/// </remarks>
		public static string GetDescription<T>(this T enumerationValue) where T : struct
		{
			Type type = enumerationValue.GetType();
			if (!type.IsEnum)
			{
				throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue");
			}

			// Tries to find a DescriptionAttribute for a potential friendly name for the enum
			MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString());
			if (memberInfo != null && memberInfo.Length > 0)
			{
				object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

				if (attrs != null && attrs.Length > 0)
				{
					// Pull out the description value
					return ((DescriptionAttribute)attrs[0]).Description;
				}
			}

			// If we have no description attribute, just return the ToString of the enum
			return enumerationValue.ToString();
		}

		/// <summary>
		/// Serializes object to the file with binary formatter
		/// </summary>
		public static void Serialize<T>(this T obj, string fileName)
		{
			IFormatter formatter = new BinaryFormatter();
			using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.None))
			{
				formatter.Serialize(stream, obj);
			}
		}

		/// <summary>
		/// Deserializes object from the file with binary formatter
		/// </summary>
		public static T Deserialize<T>(string fileName)
		{
			var formatter = new BinaryFormatter();
			using (Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				T obj = (T)formatter.Deserialize(stream);
				return obj;
			}
		}
	}
}
