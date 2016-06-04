using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace askline.linebot {
    [DataContract]
    public class LineTextContent {
        [DataMember(Name = "contentType")]
        public int ContentType { get; } = 1;
        [DataMember(Name = "toType")]
        public int ToType { get; } = 1;
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }
}