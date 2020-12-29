using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace HitHelpersNetCore.Interfaces
{
    public interface IWebApiHelper
    {
        /// <summary>
        /// Post a model to server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="post_url"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        string Post<T>(string post_url, T model, out int returnCode, out string ErrorMess,
            string user = "", Dictionary<string, string> headers = null, string mediaType = "application/json", string authenticationType = "Basic");

        /// <summary>
        /// Calling a get method
        /// </summary>
        /// <param name="post_url"></param>
        /// <param name="returnCode"></param>
        /// <param name="ErrorMess"></param>
        /// <returns></returns>
        string Get(string post_url, out int returnCode, out string ErrorMess,
            string user = "", Dictionary<string, string> headers = null, string mediaType = "application/json", string authenticationType = "Basic");

        /// <summary>
        /// Put a model to server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="post_url"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        string Put<T>(string post_url, T model, out int returnCode, out string ErrorMess,
            string user = "", Dictionary<string, string> headers = null, string mediaType = "application/json", string authenticationType = "Basic");

        /// <summary>
        /// Calling a delete method
        /// </summary>
        /// <param name="post_url"></param>
        /// <param name="returnCode"></param>
        /// <param name="ErrorMess"></param>
        /// <returns></returns>
        string Delete(string post_url, out int returnCode, out string ErrorMess,
            string user = "", Dictionary<string, string> headers = null, string mediaType = "application/json", string authenticationType = "Basic");

    }
}
