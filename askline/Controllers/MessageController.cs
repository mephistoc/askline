using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using log4net;

namespace askline {
    [RoutePrefix("messages")]
    public class MessageController : ApiController
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string AgilePointBaseUri = ConfigurationManager.AppSettings["AgilePointBaseUri"];
        private static readonly string BotBaseUri = ConfigurationManager.AppSettings["BotBaseUri"];

        [Route("")]
        public IHttpActionResult PostMessage() {
            string msgBody = Request.Content.ReadAsStringAsync().Result;
            JObject incomingMsg = null;
            if (!string.IsNullOrEmpty(msgBody)) {
                incomingMsg = JObject.Parse(msgBody);
            }
            Logger.Info("Post");
            Logger.Info(Request.Headers.ToString());
            Logger.Info(msgBody);
            Logger.Info((string)incomingMsg.SelectToken("result[0].content.text"));
            Logger.Info(String.Format("From: {0}, Content is: {1}", 
                (string)incomingMsg.SelectToken("result[0].from"), 
                (string)incomingMsg.SelectToken("result[0].content.text")));

            return new MessageResult(String.Format("Hi there, I got your request: {0}", incomingMsg["text"]), Request);
        }

        [Route("")]
        public IHttpActionResult GetMessage() {
            Logger.Info("Get");
            Logger.Info(Request.Headers.ToString());
            Logger.Info(Request.Content.ReadAsStringAsync().Result);
            return new MessageResult("Get", Request);
        }
    }
}
