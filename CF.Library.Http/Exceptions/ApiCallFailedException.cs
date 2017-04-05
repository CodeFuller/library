using System;
using System.Globalization;
using System.Net.Http;
using System.Runtime.Serialization;
using CF.Library.Core.Exceptions;

namespace CF.Library.Http.Exceptions
{
	/// <summary>
	/// The exception that is thrown when API call fails
	/// </summary>
	[Serializable]
	public class ApiCallFailedException : BasicException
	{
		/// <summary>
		/// Internal API error id
		/// </summary>
		public int ApiErrorId { get; set; }
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
		/// Constructor
		/// </summary>
		public ApiCallFailedException(HttpResponseMessage response)
			: base(String.Format(CultureInfo.CurrentCulture, "Api call failed: [{0}][{1}][{2}]", response?.RequestMessage.RequestUri, response != null ? (int)response.StatusCode : 0, response?.ReasonPhrase))
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ApiCallFailedException(HttpRequestException e)
			: base(String.Format(CultureInfo.CurrentCulture, "Api call failed: {0}", e?.Message))
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

			info.AddValue(nameof(ApiErrorId), ApiErrorId);
			info.AddValue(nameof(ApiErrorName), ApiErrorName);
			info.AddValue(nameof(ApiErrorMessage), ApiErrorMessage);

			base.GetObjectData(info, context);
		}
	}
}
