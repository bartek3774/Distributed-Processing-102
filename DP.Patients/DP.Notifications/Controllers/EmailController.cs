using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DP.Notifications.Model;
using DP.Notifications.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace DP.Notifications.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailController : ControllerBase
    {
        private readonly EmailSender _sender;

        public EmailController(EmailSender sender)
        {
            _sender = sender;
        }
        
        public void SendMessage(string emailAddress, string subject, string body)
        {
            
            _sender.SendNewUserEmail(emailAddress, subject, body);
        }
    }
}
