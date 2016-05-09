using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace CF.Extensions.Json
{
	/// <remarks>
	/// http://byterot.blogspot.com.by/2012/06/asp-net-web-api-mediatypeformatter.html
	/// </remarks>
	public class LegacyJsonMediaTypeConverterDelegatingHandler : DelegatingHandler
	{
		protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			return base.SendAsync(request, cancellationToken)
			 .ContinueWith(task =>
			 {
				 var httpResponseMessage = task.Result;
				 if (httpResponseMessage.Content.Headers.ContentType.MediaType == "text/javascript")
				 {
					 httpResponseMessage.Content.Headers.ContentType.MediaType = "application/json";
				 }
				 return httpResponseMessage;
			 }, cancellationToken);
		}
	}
}
