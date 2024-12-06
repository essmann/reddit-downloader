using HtmlAgilityPack;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Reddit_Downloader
{
    public class Http
    {
        // Making HttpClient static to reuse it
        static readonly HttpClient client = new HttpClient();
        public static async Task<string> GetHTTP(Uri uri)
        {
            try
            {
                client.DefaultRequestHeaders.Add("User-Agent", Config.UserAgent);
                using HttpResponseMessage response = await client.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine($"Request error: {e.Message}");
                return "";
            }
            catch (Exception e)
            {
                Console.WriteLine($"An error occurred: {e.Message}");
                return "";
            }
        }

        public static HtmlDocument ParseHTTP(string response)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(response);
            return htmlDoc;
        }
    }
}