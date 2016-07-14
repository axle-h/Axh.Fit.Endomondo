using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Axh.Fit.Endomondo.Models;
using Axh.Fit.Endomondo.Properties;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Axh.Fit.Endomondo
{
    public class EndomondoScraper : IDisposable
    {
        private const string HomeUrl = "https://www.endomondo.com/home";
        private const string HistoryUrl = "https://www.endomondo.com/rest/v1/users/{0}/workouts/history?offset=0&limit=9999";
        private const string WorkoutUrl = "https://www.endomondo.com/rest/v1/users/{0}/workouts/{1}/export?format={2}";

        private readonly WebClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndomondoScraper"/> class.
        /// </summary>
        /// <param name="userToken">The user token.</param>
        public EndomondoScraper(string userToken)
        {
            _client = new WebClient { Encoding = Encoding.UTF8 };
            var headers =
                Resources.Chrome_Headers.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                         .Select(x => x.Split(new[] { ':' }, 2, StringSplitOptions.RemoveEmptyEntries));
            foreach (var header in headers)
            {
                _client.Headers.Add(header[0], header[1]);
            }

            _client.Headers.Add("Cookie", $"USER_TOKEN=\"{userToken}\"");
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <returns></returns>
        public int? GetUserId()
        {
            var htmlString = _client.DownloadString(HomeUrl);
            var html = new HtmlDocument();
            html.LoadHtml(htmlString);

            var scriptNode = html.DocumentNode.SelectNodes("//head//script")?.FirstOrDefault(x => x.InnerText.Contains("endoConfig"));
            if (scriptNode == null)
            {
                return null;
            }

            var endoConfigMatch = Regex.Match(scriptNode.InnerText, @".+endoConfig\s*=\s*({.+});");
            if (!endoConfigMatch.Success || !endoConfigMatch.Groups[1].Success)
            {
                return null;
            }

            var json = JObject.Parse(endoConfigMatch.Groups[1].Value);
            return json.SelectToken("$.session.id")?.ToObject<int>();
        }

        /// <summary>
        /// Gets the history.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns></returns>
        public EndomondoHistory GetHistory(int userId)
        {
            var url = string.Format(HistoryUrl, userId);
            var json = _client.DownloadString(url);
            return JsonConvert.DeserializeObject<EndomondoHistory>(json);
        }

        /// <summary>
        /// Gets the workout.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="workoutId">The workout identifier.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public byte[] GetWorkout(int userId, int workoutId, string format)
        {
            var url = string.Format(WorkoutUrl, userId, workoutId, format.ToUpper());
            return _client.DownloadData(url);
        } 

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => _client.Dispose();
        
    }
}
