using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Axh.Fit.Endomondo.Models;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Axh.Fit.Endomondo
{
    public class EndomondoScraper : IDisposable
    {
        private const string EndomondoUrl = "https://www.endomondo.com";
        private readonly HttpClient _client;

        /// <summary>
        /// Initializes a new instance of the <see cref="EndomondoScraper"/> class.
        /// </summary>
        /// <param name="userToken">The user token.</param>
        public EndomondoScraper(string userToken)
        {
            var cookieContainer = new CookieContainer();
            var handler = new HttpClientHandler {CookieContainer = cookieContainer};
            _client = new HttpClient(handler, true)
                      {
                          BaseAddress = new Uri(EndomondoUrl)
                      };

            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; WOW64)");
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("AppleWebKit/537.36 (KHTML, like Gecko)");
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Chrome/51.0.2704.103");
            _client.DefaultRequestHeaders.UserAgent.ParseAdd("Safari/537.36");

            _client.DefaultRequestHeaders.Accept.ParseAdd("text/html");
            _client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
            _client.DefaultRequestHeaders.Accept.ParseAdd("*/*;q=0.8");

            _client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-GB");
            _client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en-US;q=0.8");
            _client.DefaultRequestHeaders.AcceptLanguage.ParseAdd("en;q=0.6");
            _client.DefaultRequestHeaders.Add("DNT", "1");

            cookieContainer.Add(_client.BaseAddress, new Cookie("USER_TOKEN", userToken));
        }

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <returns></returns>
        public async Task<int?> GetUserIdAsync()
        {
            var htmlString = await _client.GetStringAsync("/home");
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
        public async Task<History> GetHistoryAsync(int userId)
        {
            var url = $"/rest/v1/users/{userId}/workouts/history?offset=0&limit=9999";
            var json = await _client.GetStringAsync(url);
            return JsonConvert.DeserializeObject<History>(json);
        }

        /// <summary>
        /// Gets the workout.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="workoutId">The workout identifier.</param>
        /// <param name="format">The format.</param>
        /// <returns></returns>
        public async Task<WorkoutData> GetWorkout(int userId, int workoutId, string format)
        {
            var url = $"/rest/v1/users/{userId}/workouts/{workoutId}/export?format={format.ToUpper()}";
            var result = await _client.GetAsync(url);
            result.EnsureSuccessStatusCode();

            return new WorkoutData
                   {
                       Length = (int) result.Content.Headers.ContentLength,
                       Stream = await result.Content.ReadAsStreamAsync()
                   };
        } 

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() => _client.Dispose();
        
    }
}
