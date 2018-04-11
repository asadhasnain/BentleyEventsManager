/*--------------------------------------------------------------------------------------+
|
|  $Date: 2018/04 $
|  $Author: Muhammad Bilal$
|
|  $Copyright: (c) All rights reserved. $
|
+--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace BentleyEventsManager
    {
    public class ConfigLookup
        {
        internal static string GetJobName
            {
            get
                {
                return ConfigurationManager.AppSettings["JobName"];
                }
            }

        internal static int GetJobDuration
            {
            get
                {
                int duration = 0;
                string temp = ConfigurationManager.AppSettings["JobDuration"];
                int.TryParse (temp, out duration);
                return duration;
                }
            }

        internal static string NotificationAdminEmail
            {
            get
                {
                return ConfigurationManager.AppSettings["AdminEmail"];
                }
            }
        internal static string NotificationAdminPassword
            {
            get
                {
                return ConfigurationManager.AppSettings["AdminPassword"];
                }
            }
        }
    }