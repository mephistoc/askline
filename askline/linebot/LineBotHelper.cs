using System;
using System.IO;
using System.Text;
using System.Net;
using log4net;
using Newtonsoft.Json;

namespace askline.linebot {
    /// <summary>
    /// Helper class for communicate with LINE Bot API.
    /// </summary>
    public class LineBotHelper {
        private ILog Logger = null;
        private string BotBaseUri = "";
        private string ChannelId = "";
        private string ChannelSecret = "";
        private string SenderMid = "";
        private const string SendMsgRoute = "/v1/events";
        private const string ContentType = "application/json; charset=UTF-8";
        private const string HEADER_CHANNEL_ID = "X-Line-ChannelID";
        private const string HEADER_CHANNEL_SEC = "X-Line-ChannelSecret";
        private const string HEADER_CHANNEL_MID = "X-Line-Trusted-User-With-ACL";

        public LineBotHelper(ILog logger,
                             string bot_base_uri,
                             string channel_id,
                             string channel_sec,
                             string sender_mid) {
            Logger = logger;
            BotBaseUri = bot_base_uri;
            ChannelId = channel_id;
            ChannelSecret = channel_sec;
            SenderMid = sender_mid;
        }

        /// <summary>
        /// Send message to a LINE user.
        /// </summary>
        /// <param name="receiver_mid"></param>
        /// <param name="msg"></param>
        public void SendMsg(string receiver_mid, string msg) {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BotBaseUri + SendMsgRoute);
            request.ContentType = ContentType;
            request.Headers.Add(HEADER_CHANNEL_ID, ChannelId);
            request.Headers.Add(HEADER_CHANNEL_SEC, ChannelSecret);
            request.Headers.Add(HEADER_CHANNEL_MID, SenderMid);

            Logger.Info(String.Format("Bot Uri: {0}", request.RequestUri.ToString()));

            LineSendPackage sendMsg = new LineSendPackage {
                To = new string[1] { receiver_mid },
                Content = new LineTextContent {
                    Text = msg
                }
            };
            Logger.Info(String.Format("LineSendPackage composed. Message is: {0}", JsonConvert.SerializeObject(sendMsg)));
            var contentToSend = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(sendMsg));
            Logger.Info(String.Format("Content bytes buffer is ready. Length: {0}", contentToSend.Length));

            try {
                request.ContentLength = contentToSend.Length;
                request.Method = "POST";
                using (Stream requestStream = request.GetRequestStream()) {
                    requestStream.Write(contentToSend, 0, contentToSend.Length);
                }
                Logger.Info("Request prepared.");

                var rep = request.GetResponse();
            }
            catch (Exception ex) {
                Logger.Error(String.Format("Error: {0}", ex.Message));
                Logger.Error(String.Format("Stack: {0}", ex.StackTrace));
            }
        }
    }
}