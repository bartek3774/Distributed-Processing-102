﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace DP.Notifications.Services
{
    public class EmailSender
    {
        public void SendNewUserEmail(string emailAddress, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("sdatabs@gmail.com", "password"),
                EnableSsl = true,
            };

                smtpClient.Send("sdatabs@gmail.com ", emailAddress, subject, body);



        }
    }
}
