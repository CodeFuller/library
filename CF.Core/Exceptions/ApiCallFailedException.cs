using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CF.Core.Exceptions
{
	public class ApiCallFailedException : BasicException
	{
		public int ApiErrorId { get; set; }
		public string ApiErrorName { get; set; }
		public string ApiErrorMessage { get; set; }

		public ApiCallFailedException(HttpResponseMessage response)
			: base(String.Format("Api call failed: [{0}][{1}][{2}]", response.RequestMessage.RequestUri, (int)response.StatusCode, response.ReasonPhrase))
		{
		}

		public ApiCallFailedException(HttpRequestException e)
			: base(String.Format("Api call failed: {0}", e.Message))
		{
		}
	}
}
