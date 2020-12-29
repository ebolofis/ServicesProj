using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace HitHelpersNetCore.Models
{
    public class EmailSendModel
    {
        [EmailAddress]
        /// <summary>
        /// From email address
        /// </summary>
        public string From { get; set; }


        /// <summary>
        /// List of email addresses to send email
        /// </summary>
        public List<string> To { get; set; }

        /// <summary>
        /// Email subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Email body
        /// </summary>
        public string Body { get; set; }
    }
}
