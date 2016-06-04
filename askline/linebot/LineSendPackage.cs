using System.Runtime.Serialization;

namespace askline.linebot {
    [DataContract]
    public class LineSendPackage {
        [DataMember(Name = "to")]
        public string[] To { get; set; }
        [DataMember(Name = "toChannel")]
        public int ToChannel { get; } = 1383378250;
        [DataMember(Name = "eventType")]
        public string EventType { get; } = "138311608800106203";
        [DataMember(Name = "content")]
        public LineTextContent Content { get; set; }
    }
}