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
        private static readonly string SenderId = ConfigurationManager.AppSettings["lineSenderChannelId"];
        private static readonly string SenderSecret = ConfigurationManager.AppSettings["lineSenderChannelSecret"];
        private static readonly string SenderMid = ConfigurationManager.AppSettings["lineSenderMID"];
        private static readonly string AdminDomain = ConfigurationManager.AppSettings["AdminDomain"];
        private static readonly string AdminName = ConfigurationManager.AppSettings["AdminName"];
        private static readonly string AdminPass = ConfigurationManager.AppSettings["AdminPass"];
        private static readonly string AppId = ConfigurationManager.AppSettings["AppId"];
        private static readonly string AppLocale = ConfigurationManager.AppSettings["AppLocale"];
        private static readonly string FormUrl = ConfigurationManager.AppSettings["AgilePointFormBaseUrl"];

        public MessageController() {
            Logger.Info("Constractor been called.");
        }

        [Route("")]
        public IHttpActionResult PostMessage() {
            string msgBody = Request.Content.ReadAsStringAsync().Result;
            JObject incomingPkg = null;
            if (!string.IsNullOrEmpty(msgBody)) {
                incomingPkg = JObject.Parse(msgBody);
            }

            // The real "from" is under content node, not like what descriped in Line Bot API document.
            string incomingSender = (string)incomingPkg.SelectToken("result[0].content.from");
            string incomingMsg = (string)incomingPkg.SelectToken("result[0].content.text");

            Logger.Info(Request.Headers.ToString());
            Logger.Info(msgBody);
            //Logger.Info(String.Format("First from: {0}", (string)incomingPkg.SelectToken("result[0].from")));
            //Logger.Info(String.Format("Content from: {0}", (string)incomingPkg.SelectToken("result[0].content.from")));
            Logger.Info(String.Format("From: {0}, Content is: {1}",
                incomingSender,
                incomingMsg));

            try {
                JArray rtnTasks = null;
                AgilePointActionHandler handler = new AgilePointActionHandler(AgilePointBaseUri, AdminDomain, AdminName, AdminPass, AppId, AppLocale, Logger);
                linebot.LineBotHelper bot = new linebot.LineBotHelper(Logger, BotBaseUri, SenderId, SenderSecret, SenderMid);

                if (incomingMsg.Trim().ToLower() == "task") {
                    rtnTasks = JArray.Parse(handler.GetWorkListByUserID(handler.GetUserNameByLineMID(incomingSender)));

                    if (rtnTasks != null && rtnTasks.Count != 0) {
                        foreach (JObject t in rtnTasks) {
                            StringBuilder taskMsg = new StringBuilder();
                            taskMsg.Append(String.Format("Process Name: {0} \n", t.SelectToken("ProcInstName")));
                            taskMsg.Append(String.Format("Step: {0} \n", t.SelectToken("DisplayName")));
                            taskMsg.Append(String.Format("Due Date: {0} \n", (DateTime)t.SelectToken("DueDate")));
                            taskMsg.Append(String.Format("Link: {0}{1}", FormUrl, t.SelectToken("WorkItemID")));

                            bot.SendMsg(incomingSender, taskMsg.ToString());
                        }
                        bot.SendMsg(incomingSender, String.Format("You have been assigned {0} task(s).", rtnTasks.Count));
                    }
                    else {
                        bot.SendMsg(incomingSender, "You have no task need to worry about.");
                    }
                }
            }
            catch (Exception ex) {
                Logger.Error(String.Format("Error: {0}", ex.Message));
                Logger.Error(String.Format("Stack: {0}", ex.StackTrace));
            }
            return new MessageResult(String.Format("{0}", incomingMsg), Request);
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
