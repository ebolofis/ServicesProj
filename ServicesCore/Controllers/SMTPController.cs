using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HitHelpersNetCore.Helpers;
using HitHelpersNetCore.Models;
using Microsoft.Extensions.Logging;
using HitHelpersNetCore.Classes;

namespace HitServicesCore.Controllers
{
    public class SMTPController : Controller
    {
        private readonly SmtpHelper _smhelper;
        ILogger<SMTPController> _logger;
        private readonly EmailHelper ehelper;
        public SMTPController(SmtpHelper smhelper, ILogger<SMTPController> logger, EmailHelper ehelp)
        {
            this._smhelper = smhelper;
            this._logger = logger;
            this.ehelper = ehelp;
        }
        public IActionResult Configuration()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SaveConfiguration(SmtpModel smtpModel)
        {
            foreach (KeyValuePair<string, SmtpModel> pair in _smhelper._smhelper)
            {

                pair.Value.password = smtpModel.password;
                pair.Value.username = smtpModel.username;
                pair.Value.ssl = smtpModel.ssl;
                pair.Value.smtp = smtpModel.smtp;
                pair.Value.port = smtpModel.port;
                pair.Value.sender = smtpModel.sender;
                break;
            }


            _logger.LogInformation("Initiating Smtp Saving of Configuration");
            try
            {
                _smhelper.SaveSmtps(_smhelper._smhelper);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while saving Smtp Configuration " + Convert.ToString(ex));
            }
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> TestEmail(TestEmail model)
        {
            _logger.LogInformation("Initiating Smtp Email Test");
            var err = "";
            try
            {
                ehelper.Init(model.smtp, Convert.ToInt32(model.port), Convert.ToBoolean(model.ssl), model.username, model.password);
                EmailSendModel email = new EmailSendModel();
                List<string> emailList = new List<string>();
                email.Subject = " Hit Services Core Email Test Subject";
                email.Body = " Hit Services Core Email Test Body";
                emailList.Add(model.testemail);
                email.From = model.sender;
                email.To = emailList;

                ehelper.Send(email);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error while testing email through SMTP" + Convert.ToString(ex.Message));
                err = "Error while testing email through SMTP" + Convert.ToString(ex.Message);
            }
            return Ok(err);
        }
    }
}

public class TestEmail
{
    public string smtp { get; set; }
    public string port { get; set; }
    public string ssl { get; set; }
    public string username { get; set; }
    public string password { get; set; }
    public string sender { get; set; }
    public string testemail { get; set; }
}
