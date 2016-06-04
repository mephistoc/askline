using System;
using System.IO;
using System.Text;
using System.Net;
using log4net;

namespace askline {
    public class HTTPOperations {
        string domain = string.Empty;
        string userName = string.Empty;
        string password = string.Empty;
        string appID = string.Empty;
        string locale = string.Empty;
        string contenttype = string.Empty;
        ILog Logger = null;

        public HTTPOperations(string Domain, string UserName, string Password, string AppID, string Locale, ILog logger) {
            domain = Domain;
            userName = UserName;
            password = Password;
            appID = AppID;
            locale = Locale;
            Logger = logger;
        }

        #region Create Http Request

        public HttpWebRequest GetHttpRequest(string URI, string Method) {
            //---------- Creat a request with required URI
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URI);

            request.Method = Method;

            //Set Content Type
            request.ContentType = "application/json; charset=utf-8";
            //Set Accept Type
            request.Accept = "application/json";

            //Setting Header
            //Creating Authorizationheader format (Basic (base64(domain\\username:password))
            request.Headers[HttpRequestHeader.Authorization] =
              "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(domain + "\\" + userName + ":" + password));
            request.Headers["appID"] = appID;
            request.Headers["locale"] = locale;
            request.Timeout = 100000;
            request.KeepAlive = false;
            //ServicePointManager.ServerCertificateValidationCallback += 
            //         new System.Net.Security.RemoteCertificateValidationCallback(ValidateServerCertificate);
            return request;
        }

        #endregion

        #region POST Method

        public string POSTMethod(string URI, string JsonRequestData) {

            HttpWebRequest request = GetHttpRequest(URI, WebRequestMethods.Http.Post);

            //------------POST ing data to Server
            foreach (string key in request.Headers.AllKeys) {
                Logger.Info(string.Format("{0}: {1}", key, request.Headers[key]));
            }
            Logger.Info(String.Format("Endpoint URI: {0}", URI));
            Logger.Info(String.Format("JSON: {0}", JsonRequestData));
            var requestData = Encoding.UTF8.GetBytes(JsonRequestData);
            request.ContentLength = requestData.Length;
            using(Stream reqStream = request.GetRequestStream()) {
                reqStream.Write(requestData, 0, requestData.Length);
            }

            HttpWebResponse webResponse = null;
            try {
                //-------- geting response from request
                Logger.Info(String.Format("About to call the service: {0}", request.RequestUri));
                webResponse = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex) {
                throw ex;
            }

            string jsonResponseData = ReadData(webResponse);

            return jsonResponseData;

        }

        public string PostXml(string url, string xml) {
            try {
                byte[] bytes = Encoding.UTF8.GetBytes(xml);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentLength = bytes.Length;
                request.ContentType = "application/xml";
                using (Stream requestStream = request.GetRequestStream()) {
                    requestStream.Write(bytes, 0, bytes.Length);
                }
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream resStream = response.GetResponseStream();
                StreamReader rdStreamRdr = new StreamReader(resStream);
                if (response.StatusCode != HttpStatusCode.OK) {
                    string message = String.Format("POST failed. Received HTTP {0}",
                    response.StatusCode);
                    throw new ApplicationException(message);
                }
                else {
                    string message = rdStreamRdr.ReadToEnd();
                    return message;
                }
            }
            catch (Exception) {
                throw;
            }

        }

        public string POSTMethod(string URI) {
            HttpWebRequest request = GetHttpRequest(URI, WebRequestMethods.Http.Post);

            //------------POST ing data to Server

            //request.ContentLength = JsonRequestData.Length;
            Stream content = request.GetRequestStream();
            StreamWriter ContentWriter = new StreamWriter(content);
            //ContentWriter.Write();
            ContentWriter.Close();

            HttpWebResponse webResponse = null;
            try {
                //-------- geting response from request
                webResponse = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex) {
                throw ex;
            }

            string jsonResponseData = ReadData(webResponse);

            return jsonResponseData;
        }

        #endregion

        #region GET Method

        public string GetData(string URI) {
            HttpWebRequest request = GetHttpRequest(URI, WebRequestMethods.Http.Get);
            try {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                string jsonResponseData = ReadData(response);

                return jsonResponseData;
            }
            //HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            catch (Exception ex) {
                //LogServiceException(ex);
                throw ex;
            }

        }

        #endregion

        string ReadData(HttpWebResponse webResponse) {
            string jsonResponseData = string.Empty;
            try {
                if (webResponse.StatusCode == HttpStatusCode.OK) {
                    //Stream webStream = webResponse.GetResponseStream();
                    StreamReader responseReader = new StreamReader(webResponse.GetResponseStream());
                    jsonResponseData = responseReader.ReadToEnd();

                    responseReader.Close();
                }
            }
            catch (Exception ex) {
                throw ex;
            }

            return jsonResponseData;
        }

    }
}