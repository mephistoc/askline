namespace askline {
    public class ConstantList {
        public const string LineBotApiUri = "https://trialbot-api.line.me/v1/events";
        public const string EventType_Message           = "138311609000106303";
        public const string EventType_Operation         = "138311609100106403";
        public const string RequestHeader_ChannelID     = "X-Line-ChannelID: {0}";
        public const string RequestHeader_ChannelSecret = "X-Line-ChannelSecret: {0}";
        public const string RequestHeader_UserWithACL   = "X-Line-Trusted-User-With-ACL: {0}";
    }
}