using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	/// <summary>
	/// The exception that is thrown when API call fails
	/// </summary>
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
		public ApiCallFailedException(HttpResponseMessage response)
			: base(String.Format("Api call failed: [{0}][{1}][{2}]", response.RequestMessage.RequestUri, (int)response.StatusCode, response.ReasonPhrase))
		{
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public ApiCallFailedException(HttpRequestException e)
			: base(String.Format("Api call failed: {0}", e.Message))
		{
		}
	}
}
