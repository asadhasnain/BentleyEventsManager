/*--------------------------------------------------------------------------------------+
|
|  $Date: 2018/04 $
|  $Author: Muhammad Bilal$
|
|  $Copyright: (c) All rights reserved. $
|
+--------------------------------------------------------------------------------------*/
using BentleyEventsManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;

namespace BentleyEventsManager.Controllers
    {
    public class NotificationsController :Controller
        {
        private static CacheItemRemovedCallback s_onChacheRemoved = null;

        #region JobScheduler

        /*------------------------------------------------------------------------------------**/
        /// <summary></summary>
        /// <remarks></remarks>
        /// <author>Muhammad.Bilal</author>                               <date>04/2018</date>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        private void SendEmail (string jobName, object duration, CacheItemRemovedReason reason)
            {
            Debug.WriteLine ($"Task Name :{jobName} Fired at {DateTime.Now}, Next fire {DateTime.Now.AddSeconds ((int)duration)}");

            SendEmailNotification ();

            ScheduleJob (jobName, (int)duration);
            }

        /*------------------------------------------------------------------------------------**/
        /// <summary></summary>
        /// <remarks></remarks>
        /// <author>Muhammad.Bilal</author>                               <date>04/2018</date>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        public void ScheduleJob (string jobName, int duration)
            {
            s_onChacheRemoved = new CacheItemRemovedCallback (SendEmail);
            HttpRuntime.Cache.Insert (jobName, duration, null, DateTime.Now.AddSeconds (duration), Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, s_onChacheRemoved);
            }
        #endregion 

        /*------------------------------------------------------------------------------------**/
        /// <summary></summary>
        /// <remarks></remarks>
        /// <author>Muhammad.Bilal</author>                               <date>04/2018</date>
        /*--------------+---------------+---------------+---------------+---------------+------*/
        private void SendEmailNotification ()
            {
            SmtpClient smtpClient = new SmtpClient ();
            smtpClient.Port = Constants.NOTIFICATION_HOST_PORT;
            smtpClient.Host = Constants.NOTIFICATION_HOST;
            smtpClient.EnableSsl = true;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Credentials = new NetworkCredential (ConfigLookup.NotificationAdminEmail, ConfigLookup.NotificationAdminPassword);

            MailAddress from = new MailAddress (ConfigLookup.NotificationAdminEmail, ConfigLookup.NotificationAdminEmail.Split ('@').FirstOrDefault (), Encoding.UTF8);

            MailAddress to = new MailAddress ("be2bd648.bentley.onmicrosoft.com@amer.teams.ms");

            MailMessage message = new MailMessage (from, to);
            //message.To.Add (new MailAddress ("asad.bukhari@bentley.com"));
            //message.To.Add (new MailAddress ("Aliraza.Ahmed@bentley.com"));
            //message.To.Add (new MailAddress ("irshad.babar@bentley.com"));
            //message.To.Add (new MailAddress ("Bilal.Farooq@bentley.com"));
            //message.To.Add (new MailAddress ("be2bd648.bentley.onmicrosoft.com@amer.teams.ms"));

            message.Body = "SIG: Bentley Technology Champions please get your assigned tasks done before start of today's session.";

            message.BodyEncoding = Encoding.UTF8;
            message.Subject = "Just a reminder from Bentley Event Manager Notification Service for SIG";
            message.SubjectEncoding = Encoding.UTF8;

            smtpClient.SendCompleted += new SendCompletedEventHandler (SendCompletedCallback);

            smtpClient.SendAsync (message, Constants.SERVICE_NAME);
            Debug.WriteLine (LocalizeableStrings.SendingNotification);
            }

        private void SendCompletedCallback (object sender, AsyncCompletedEventArgs e)
            {
            String token = (string)e.UserState;

            if (e.Cancelled)
                {
                Debug.WriteLine ("[{0}] Send canceled.", token);
                }
            if (e.Error != null)
                {
                Debug.WriteLine ("[{0}] {1}", token, e.Error.ToString ());
                }
            else
                {
                Debug.WriteLine ("Message sent.");
                }
            }
        }
    }