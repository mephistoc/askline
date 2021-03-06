﻿using System;
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

        /// <summary>
        /// Get task list from AgilePoint Cloud.
        /// </summary>
        /// <param name="user_id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Mapping LINE mid to AgilePoint user.
        /// </summary>
        /// <param name="mid"></param>
        /// <returns></returns>
        public string GetUserNameByLineMID(string mid) {
            // Put double "\" in ths string or you will get error when send the request.
            string rtnStr = "nxone\\\\VINCENT.CHU";

            return rtnStr;
        }
    }
}