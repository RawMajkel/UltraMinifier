using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UltraMinifier.ConsoleApp
{
    public static class CssMinifier
    {
        private const string URL_CSS_MINIFIER = "https://cssminifier.com/raw";
        private const string POST_PAREMETER_NAME = "input";

        public static async Task<string> MinifyCss(string inputCss)
        {
            var contentData = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>(POST_PAREMETER_NAME, inputCss)
            };

            using var httpClient = new HttpClient();
            using var content = new FormUrlEncodedContent(contentData);
            using var response = await httpClient.PostAsync(URL_CSS_MINIFIER, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
