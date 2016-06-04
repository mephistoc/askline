using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using log4net;

namespace askline {
    public class AgilePointActionHandler {
        private static string ServerBaseUri = "";
        private static string Domain = "";
        private static string AdminUserName = "";
        private static string AdminPassword = "";
        private static string AppId = "";
        private static string Locale = "";
        private static ILog Logger = null;

        public AgilePointActionHandler(string server_base_uri, 
                                       string domain_name,
                                       string admin_name,
                                       string admin_pass,
                                       string app_id,
                                       string locale,
                                       ILog logger) {
            ServerBaseUri = server_base_uri;
            Domain = domain_name;
            AdminUserName = admin_name;
            AdminPassword = admin_pass;
            AppId = app_id;
            Locale = locale;
            Logger = logger;
        }

        public string GetWorkListByUserID(string user_id) {
            string URI = ServerBaseUri + "/Workflow/GetWorkListByUserID";

            string jsonRequestData = "{\"UserName\":\"" + user_id + "\",\"Status\":\"Assigned\"}";
            string rtnStr = "";
            try {
                HTTPOperations ops = new HTTPOperations(Domain, AdminUserName,
                                     AdminPassword, AppId, Locale, Logger);
                Logger.Info("Operation prepared.");
                //Logger.Info(String.Format("AgilePoint URI: {0}", URI));
                //Logger.Info(String.Format("JSON: {0}", jsonRequestData));

                rtnStr = ops.POSTMethod(URI, jsonRequestData);
                return rtnStr;
            }catch(Exception ex) {
                Logger.Error(String.Format("Error: {0}", ex.Message));
                Logger.Error(String.Format("Stack: {0}", ex.StackTrace));
                throw ex;
            }
        }

        public string GetUserNameByLineMID(string mid) {
            string rtnStr = "nxone\\\\VINCENT.CHU";

            return rtnStr;
        }
    }
}