using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Security.Cryptography;

namespace askline {
    public class MessageHandler {

        /// <summary>
        /// Calvulate signature string using HMAC-SHA256 with given secret and convert it to Base64 string.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string CalculateSignatureMACSHA256(string message,
            string secret) {

            byte[] secretKey = Encoding.UTF8.GetBytes(secret);
            byte[] messageByte = Encoding.UTF8.GetBytes(message);
            byte[] signature = null;

            using(HMACSHA256 hmac = new HMACSHA256(secretKey)) {
                signature = hmac.ComputeHash(messageByte);
            }

            return Convert.ToBase64String(signature);
        }
    }
}