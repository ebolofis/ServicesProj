using HitHelpersNetCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace HitHelpersNetCore.Classes
{
    public class WebApiHelper : IWebApiHelper
    {
        /// <summary>
        /// Calling a delete method
        /// </summary>
        /// <param name="post_url"></param>
        /// <param name="returnCode"></param>
        /// <param name="ErrorMess"></param>
        /// <returns></returns>
        public string Delete(string post_url, out int returnCode, out string ErrorMess, string user = "", Dictionary<string, string> headers = null, string mediaType = "application/json", string authenticationType = "Basic")
        {
            ErrorMess = "";
            string result = "";
            try
            {
                HttpClient client = new HttpClient();

                setHeaders(client, authenticationType, user, headers);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //client.DefaultRequestHeaders.Add(new MediaTypeWithQualityHeaderValue(mediaType));

                Task<HttpResponseMessage> response = client.DeleteAsync(post_url);

                returnCode = (int)response.Result.StatusCode;
                if (returnCode < 200 || returnCode > 299)
                    ErrorMess = response.ToString() + " \r\n" + response.Result.Content.ReadAsStringAsync().Result;
                else
                    result = response.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                returnCode = 400;
                ErrorMess = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// Calling a get method
        /// </summary>
        /// <param name="post_url"></param>
        /// <param name="returnCode"></param>
        /// <param name="ErrorMess"></param>
        /// <returns></returns>
        public string Get(string post_url, out int returnCode, out string ErrorMess, string user = "", Dictionary<string, string> headers = null, string mediaType = "application/json", string authenticationType = "Basic")
        {
            ErrorMess = "";
            string result = "";
            try
            {
                HttpClient client = new HttpClient();

                setHeaders(client, authenticationType, user, headers);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                //client.DefaultRequestHeaders.Add(new MediaTypeWithQualityHeaderValue(mediaType));

                Task<HttpResponseMessage> response = client.GetAsync(post_url);

                returnCode = (int)response.Result.StatusCode;
                if (returnCode < 200 || returnCode > 299)
                    ErrorMess = response.ToString() + " \r\n" + response.Result.Content.ReadAsStringAsync().Result;
                else
                    result = response.Result.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                returnCode = 400;
                ErrorMess = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// Post a model to server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="post_url"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Post<T>(string post_url, T model, out int returnCode, out string ErrorMess, string user = "", Dictionary<string, string> headers = null, string mediaType = "application/json", string authenticationType = "Basic")
        {
            ErrorMess = "";
            string result = "";
            try
            {
                HttpClient client = new HttpClient();

                setHeaders(client, authenticationType, user, headers);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, post_url);
                string jsonString;
                jsonString = JsonSerializer.Serialize(model);
                requestMessage.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult();

                returnCode = (int)response.StatusCode;
                if (returnCode < 200 || returnCode > 299)
                    ErrorMess = response.ToString() + " \r\n" + response.Content.ReadAsStringAsync().Result;
                else
                    result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                returnCode = 400;
                ErrorMess = ex.ToString();
                result = null;
            }
            return result;
        }

        /// <summary>
        /// Put a model to server
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="post_url"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public string Put<T>(string post_url, T model, out int returnCode, out string ErrorMess, string user = "", Dictionary<string, string> headers = null, string mediaType = "application/json", string authenticationType = "Basic")
        {
            ErrorMess = "";
            string result = "";
            try
            {
                HttpClient client = new HttpClient();

                setHeaders(client, authenticationType, user, headers);

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Put, post_url);
                string jsonString;
                jsonString = JsonSerializer.Serialize(model);
                requestMessage.Content = new StringContent(jsonString, Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult();

                returnCode = (int)response.StatusCode;
                if (returnCode < 200 || returnCode > 299)
                    ErrorMess = response.ToString() + " \r\n" + response.Content.ReadAsStringAsync().Result;
                else
                    result = response.Content.ReadAsStringAsync().Result;
            }
            catch (Exception ex)
            {
                returnCode = 400;
                ErrorMess = ex.ToString();
                result = "";
            }
            return result;
        }

        /// <summary>
        /// Create Headers for the rest client
        /// </summary>
        /// <param name="client">HttpClient</param>
        /// <param name="authenticationType">type of authentication (Basic or OAuth2)</param>
        /// <param name="user">user and password or token for Authentication Header. Format for Basic: "Username:Password", Format for OAuth2: "Bearer  ZTdmZmY1Zjc5MTQ4NDQ5ZTEzMzIyZTOQ"</param>
        /// <param name="headers">custom headers </param>
        private void setHeaders(HttpClient client, string authenticationType, string user, Dictionary<string, string> headers)
        {
            //1. Greate Authorization header
            if (!string.IsNullOrEmpty(user))
            {
                switch (authenticationType)
                {
                    case "Basic":
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(user)));
                        break;
                    case "OAuth2":
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", user);
                        break;
                }
            }

            //2. Greate custom headers
            if (headers != null)
            {
                foreach (string key in headers.Keys)
                {
                    if (key != null && headers[key] != null)
                        client.DefaultRequestHeaders.Add(key, headers[key]);
                }
            }
        }
    }
}
