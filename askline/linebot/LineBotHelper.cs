using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace askline.linebot {
    public class LineBotHelper {
        private ILog logger = null;
        private string BotBaseUri = "";
        private string ChannelId = "";
        private string ChannelSecret = "";
        private string SenderMid = "";
        private const string SendMsgRoute = "/v1/enents";

        public LineBotHelper(ILog Logger,
                             string bot_base_uri,
                             string channel_id,
                             string channel_sec,
                             string sender_mid) {
            logger = Logger;
            BotBaseUri = bot_base_uri;
            ChannelId = channel_id;
            ChannelSecret = channel_sec;
            SenderMid = sender_mid;
        }

        public void SendMsg(string receiver_mid, string msg) {

        }
    }
}