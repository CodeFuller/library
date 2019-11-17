using System;
using System.Runtime.Serialization;

namespace CF.Library.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when API call fails
	/// </summary>
	[Serializable]
	public class ApiCallFailedException : BasicException
	{
		public Uri RequestUri { get; set; }

		public int? HttpStatusCode { get; set; }

		/// <summary>
		/// Internal API error id
		/// </summary>
		public int? ApiErrorId { get; set; }
		
		/// <summary>
		/// Internal API error name
		/// </summary>
		public string ApiErrorName { get; set; }
		/// <summary>
		/// API error message describing the failure
		/// </summary>
		public string ApiErrorMessage { get; set; }

		/// <summary>
		/// Constructor
		/// </summary>
		public ApiCallFailedException()
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ApiCallFailedException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ApiCallFailedException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		protected ApiCallFailedException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		/// <summary>
		/// Implementation for ISerializable.GetObjectData()
		/// </summary>
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
			{
				throw new ArgumentNullException(nameof(info));
			}

			info.AddValue(nameof(RequestUri), RequestUri);
			info.AddValue(nameof(HttpStatusCode), HttpStatusCode);
			info.AddValue(nameof(ApiErrorId), ApiErrorId);
			info.AddValue(nameof(ApiErrorName), ApiErrorName);
			info.AddValue(nameof(ApiErrorMessage), ApiErrorMessage);

			base.GetObjectData(info, context);
		}
	}
}
