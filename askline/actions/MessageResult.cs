using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace askline {
    public class MessageResult : IHttpActionResult {

        private string _value;
        private HttpRequestMessage _request;

        public MessageResult(string value, HttpRequestMessage request) {
            _value = value;
            _request = request;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) {
            var response = new HttpResponseMessage() {
                Content = new StringContent(string.Format("Hello {0}.", _value)),
                RequestMessage = _request
            };

            return Task.FromResult(response);
        }
    }
}