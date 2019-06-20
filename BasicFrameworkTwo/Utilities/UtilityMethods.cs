using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Net.Http;
using System;
using System.Linq;
using Newtonsoft.Json;
using BasicFrameworkTwo.Templates;

namespace BasicFrameworkTwo.Utilities
{
    public static class UtilityMethods
    {
        public static string GetValueFromConfigKey(string Key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[Key];
        }

        public static string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return sf.GetMethod().Name;
        }
        /// <summary>
        /// This method extends the String class.
        /// The string object that uses it (this) will both call the method and implicitly pass itself in as the parameter
        /// Example: errorText.RemoveSpacesAndNewLines()
        /// </summary>
        /// <param name="textToClean">the calling string that is to be cleaned</param>
        /// <returns>string </returns>
        public static string RemoveSpacesAndNewLines(this string textToClean)
        {
            var cleanedText = Regex.Replace(textToClean, @"\s", "");
            return Regex.Replace(cleanedText, @"\n", "");
        }

        #region HTTP Methods

        /// <summary>
        /// A generic method to call API POST request that take 2 input values in body
        /// </summary>
        /// <param name="endpoint">URL of request</param>
        /// <param name="input">Request body</param>
        /// <returns></returns>
        public static bool HTTPPostRequestToAPI(string endpoint, Dictionary<string, string> input)
        {
            HttpClient _client = new HttpClient();
            _client.BaseAddress = new Uri(Driver.RootURl);
            var requestUri = new Uri(_client.BaseAddress, $@"{endpoint}");
            var requestBody = new FormUrlEncodedContent(input);

            HttpResponseMessage response = _client.PostAsync(requestUri, requestBody).Result;

            return response.IsSuccessStatusCode;
        }

        /// <summary>
        /// A generic method to call API GET request that take an input value in URL
        /// </summary>
        /// <param name="endpoint">URL of request</param>
        /// <returns></returns>
        public static HttpResponseMessage HTTPGetRequestToAPI(string urlTerminator)
        {
            HttpClient _client = new HttpClient();
            _client.BaseAddress = new Uri(Driver.ApiRootUrl);
            var requestUri = new Uri(_client.BaseAddress, $@"{urlTerminator}");

            HttpResponseMessage response = _client.GetAsync(requestUri).Result;

            return response;
        }

        /// <summary>
        /// Gets user's GUID from the given email address
        /// </summary>
        /// <param name="email">Email address used for search</param>
        /// <returns>string</returns>
        public static string GetUserGUID(string email)
        {
            var getEndpoint = $"Account/user/email/{email}/";
            var getResponseContent = UtilityMethods.HTTPGetRequestToAPI(getEndpoint).Content;
            var contentAsString = getResponseContent.ReadAsStringAsync().Result;

            IdentityUser userDetails = JsonConvert.DeserializeObject<IdentityUser>(contentAsString);

            if (userDetails.Id == null)
            {
                throw new NullReferenceException($"GetUserGUID() returned null for user {email}");
            }
            return userDetails.Id;
        }

        #endregion

        #region manipulate text
        /// <summary>
        /// Takes the given list of ; separated account numbers and returns the text
        /// before the ;
        /// </summary>
        /// <param name="account">list of account numers</param>
        /// <param name="index">default to zero</param>
        /// <returns>string</returns>
        public static string SelectedtAccountNumber(string account, int index = 0)
        {
            if (account.Contains(";"))
            {
                return account.Split(';').ToList()[index].Trim();
            }
            else
            {
                return account;
            }
        }

        #endregion
    }
}
