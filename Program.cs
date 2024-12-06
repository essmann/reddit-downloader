using HtmlAgilityPack;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using static System.Net.WebRequestMethods;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Reflection.Metadata;
using OpenQA.Selenium.Support.UI;
using System.Runtime.CompilerServices;

namespace Reddit_Downloader
{
    internal class Program
    {
        public static string GetUserURL(string username) {

            string url = $"https://www.reddit.com/user/{username}/";
            return url;

        }
        public static void AgreeCookies(IWebDriver driver) {

            var shadowHost = driver.FindElement(By.TagName("reddit-cookie-banner"));
            var shadowRoot = shadowHost.GetShadowRoot();
            var div = shadowRoot.FindElement(By.CssSelector("div")); // Example: Find a button inside the shadow DOM

            
            var cookieButton = div.FindElement(By.Id("accept-all-cookies-button")).FindElement(By.CssSelector("button"));

            cookieButton.Click();

        }
        static async Task DownloadImageAsync(string imageUrl, string filePath)
        {
            using (HttpClient client = new HttpClient())
            {
                // Send a GET request to the image URL
                HttpResponseMessage response = await client.GetAsync(imageUrl);

                // Ensure the request was successful
                response.EnsureSuccessStatusCode();

                // Read the image content as a byte array
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                // Write the byte array to a file
                await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);
            }
        }
        public static void Login(IWebDriver driver, Config config)
        {

            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;

            IWebElement LogInButton = (IWebElement)jsExecutor.ExecuteScript(
                "return document.querySelector(\"#login-button > span > span\")");
            LogInButton.Click();

         
            Thread.Sleep(1500);

            IWebElement UsernameInputBox = (IWebElement)jsExecutor.ExecuteScript(
               "return document.querySelector(\"#login-username\").shadowRoot.querySelector(\"label > div > span > span.input-container > input[type=text]\")");
            UsernameInputBox.SendKeys(config.BotMail);

            IWebElement PassWordInputBox = (IWebElement)jsExecutor.ExecuteScript(
               "return document.querySelector('#login-password').shadowRoot.querySelector('label > div > span > span.input-container > input[type=password]');");
            PassWordInputBox.SendKeys(config.BotPassword);
            

            IWebElement PostFormButton = (IWebElement)jsExecutor.ExecuteScript(
              "return document.querySelector(\"#login > auth-flow-modal > div.w-100 > faceplate-tracker > button\");");

            PostFormButton.Click();
        }
        static async Task Main(string[] args)
        {
            Config config = new Config();
            var chromeDriverPath = @"C:\Users\Essmann\source\repos\Reddit-Downloader\Reddit-Downloader\chromedriver.exe";
            var options = new ChromeOptions();
            var url = "https://www.reddit.com/user/BENII21/submitted/";
            var driver = new ChromeDriver(chromeDriverPath, options);
            options.AddArgument("--user-agent=Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.37 (KHTML, like Gecko) Chrome/48.0.1693.303 Safari/600");

            // Create a new cookie
            OpenQA.Selenium.Cookie reddit_session = new OpenQA.Selenium.Cookie("reddit_session", "eyJhbGciOiJSUzI1NiIsImtpZCI6IlNIQTI1NjpsVFdYNlFVUEloWktaRG1rR0pVd1gvdWNFK01BSjBYRE12RU1kNzVxTXQ4IiwidHlwIjoiSldUIn0.eyJzdWIiOiJ0Ml8xZWdvd21ibmJnIiwiZXhwIjoxNzQ5MTU0Njk2LjI0MTU4NCwiaWF0IjoxNzMzNTE2Mjk2LjI0MTU4NCwianRpIjoieUVTaXEyYm43dzlPdm5KQXlZQ1FfS1VIS1pISWtnIiwiY2lkIjoiY29va2llIiwibGNhIjoxNzMzNDk0MjYzNjM4LCJzY3AiOiJlSnlLamdVRUFBRF9fd0VWQUxrIiwidjEiOiIxNDIzNjM1MzcxODcwMjAsMjAyNC0xMi0wNlQyMDoxODoxNiwxYjM4NjdhNjQ3MjkwOWY1N2ZiZmIwYWIyYzA3N2RlY2RjMGQ4ODAxIiwiZmxvIjoyfQ.mzSzFlY_kucjPOs9G6jnu2yGAE-iYv9pH8R1_lMoOjwk2sTPPGfHF0_BHJ-3JdK86SQb8zaC_6dAeT0zIhT0-q68l4ukgCXL8_4hsZWu6nkpcr_UrfN7GZ--DBYli9ZzR3sfOvv792FP-LFx_bdjZQPtKlh6qni8DTLs6mmZ4J-2RvoWoXEgjiW8_qURy5coKuYqRB2gp8_b-3eN53ASfakCXpFY2JJtWYG-1HF6gGoxfoFEDcLnJ4EpBZC40pyJIHf9IJLR2pHg1a8lb0M21wMQRMZKUJSE0KamED3MGColwudcQthKtLw6OUOweB8cRH3DrFxqVAR8JU4aTMJNsQ");
            OpenQA.Selenium.Cookie token_v2 = new OpenQA.Selenium.Cookie("token_v2", "eyJhbGciOiJSUzI1NiIsImtpZCI6IlNIQTI1NjpzS3dsMnlsV0VtMjVmcXhwTU40cWY4MXE2OWFFdWFyMnpLMUdhVGxjdWNZIiwidHlwIjoiSldUIn0.eyJzdWIiOiJ1c2VyIiwiZXhwIjoxNzMzNjAyNjk2LjY5MTE3NCwiaWF0IjoxNzMzNTE2Mjk2LjY5MTE3NCwianRpIjoiUVd4bE9faFpWZUhZUXdKcXkxeU12a0hGdHZVakRnIiwiY2lkIjoiMFItV0FNaHVvby1NeVEiLCJsaWQiOiJ0Ml8xZWdvd21ibmJnIiwiYWlkIjoidDJfMWVnb3dtYm5iZyIsImxjYSI6MTczMzQ5NDI2MzYzOCwic2NwIjoiZUp4a2tkR090REFJaGQtRmE1X2dmNVVfbTAxdGNZYXNMUWFvazNuN0RWb2NrNzA3Y0Q0cEhQOURLb3FGRENaWGdxbkFCRmdUclREQlJ1VDluTG0zZzJpTmU4dFlzWm5DQkZtd0ZEcmttTEdzaVFRbWVKSWF5eHNtb0lMTnlGeXV0R05OTFQwUUpxaGNNcmVGSHBjMm9ia2JpNTZkR0ZXNXJEeW9zVmZsMHRqR0ZMWW54amNicXcycHVDNm5Na25MUXZrc1h2VGpOOVczOXZtel9TYTBKOE9LcXVtQjNobEpDRzRzZnBpbTNkOVRrNTZ0Q3hhMTkzcVEydWQ2M0s1OTFpdzBPN2VmNl9sckl4bVhZMmgtSnZ0MzF5LWhBNDg4THpQcUFFYXM0VWNaZG1RZF9sVUhVTG1nSkdNSjR0TUk1TXJsMjM4SnRtdlR2OGJ0RXo5OE0tS21OX3pXRE5SekNlTFFwX0gxR3dBQV9fOFExZVRSIiwicmNpZCI6IjZvZkhRVE85UzY0UzdvQkVNYWNkNEFRTHppMnBWdHAweU1XZnc3SkNEZVkiLCJmbG8iOjJ9.DMDop9huKDR18ZwrHQLi9qnqx0XbE5duyRqZT9TfUy5xFY9TXlO93A2qltHsD6bpKIekPq3yoYmFKWSR2ew62J_pp5cvyt_rQoiBz30aclul5quZpgdWUvVUVq8FjqtVrUiHpz7Orx7_txmUbTAqKhg-fwrBgOt6GQCDLEzhfxfOC1sswd2-8NS8QuegPpDI2FDOlvwrdNUr4ouMtfJgXtMci7kfPTEeeWyKHyHsXppeZxG_s-S-JKP-u2cFv4cQ7H-jcffNFMbC9EXQxuTKwydRvAHK4oRDsiArXyjudOJDJBW-U0BUkusuIaw_rZ3zQhj1t4pm-R9LUYOLmM1OPg");
            
            driver.Navigate().GoToUrl(url);
            // Add the cookie to the browser
            Thread.Sleep(1000);
            driver.Manage().Cookies.AddCookie(reddit_session);
            driver.Manage().Cookies.AddCookie(token_v2);
            
            driver.Navigate().Refresh();
            Thread.Sleep(1500);
           

            // Wait for the page to load
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);

          
            

            
            List<IWebElement> posts = new List<IWebElement>();
            List<string> imageLinks = new List<string>();


            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            long lastHeight = (long)js.ExecuteScript("return document.body.scrollHeight");
            bool? FetchedAllPosts = false;
            int i = 1;
            while (true)
            {
                // Scroll down by a certain amount
                js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight);");

                // Wait for new content to load (if any)
                Thread.Sleep(1700);  // Adjust the delay as needed for the page to load new content
                var redditPosts = driver.FindElements(By.TagName("shreddit-post"));
                foreach (var item in redditPosts)
                {
                    if (!posts.Contains(item))
                    {
                        posts.Add(item);
                        
                        Thread.Sleep(1000);
                        try
                        {
                            var image = item.FindElement(By.CssSelector("img"));
                            var source = image.GetAttribute("src");
                            if (!imageLinks.Contains(source))
                            {
                                imageLinks.Add(source);

                                Console.WriteLine("src");
                                string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                                string filePath = $"{projectRoot}/images/image{i}.png";
                                await DownloadImageAsync(source, filePath);
                            }
                            
                           
                        }
                        catch
                        {
                            Console.WriteLine("Not an image");
                            continue;
                        }
                        
                        i++;
                    }

                }
                // Check the new height of the page
                long newHeight = (long)js.ExecuteScript("return document.body.scrollHeight");

                // If the height hasn't changed, we've likely reached the end of the page
                if (newHeight == lastHeight)
                {
                    Console.WriteLine("Reached the bottom of the page.");
                    FetchedAllPosts = true;
                    break;
                }

                // Update lastHeight for the next iteration
                lastHeight = newHeight;
            }
            /*
             * MAKE THIS SHIT ASYNC LATER SO ITS FAST AF
            int i = 1;
            foreach (var post in posts) {
               
                Console.WriteLine("yo");
                try
                {
                    Thread.Sleep(1000);
                    var image = post.FindElement(By.CssSelector("img"));
                    var source = image.GetAttribute("src");
                    Console.WriteLine("src");
                    string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                    string filePath = $"{projectRoot}/images/image{i}.png";
                    await DownloadImageAsync(source, filePath);
                    i++;
                }
                catch
                {

                }
            
            }
            */
            //window.scrollTo(0, document.body.scrollHeight); 




            // Access the shadow DOM

            // Click the button to accept cookies

            Console.WriteLine("Cookies accepted!");
            string scrollScript = "window.scrollTo(0, document.body.scrollHeight);";

            //relative overflow-hidden pointer-cursor  mb-xs  isolate bg-neutral-background rounded-[16px] = wrapped for censored images

            //document.getElementsBYTagName("xpromo-nsfw-blocking-modal-desktop") html element that hides nsfw content and prompts for login

            //document.getElementsByTagName("xpromo-nsfw-blocking-modal-desktop")[0].remove()

            //document.getElementsByTagName("xpromo-nsfw-blocking-modal-desktop")[0].shadowRoot.children[0].remove()

            //nvm we need an account.
            //driver.Navigate().GoToUrl("https://www.google.com/");

            /*
            string url = GetUserURL("DR650SE");
            var response = await Http.GetHTTP(new Uri(url));
            var html = Http.ParseHTTP(response);
            */


        }
    }
    public class Config
    {
        public const string UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36";
        public string? BotMail = System.Environment.GetEnvironmentVariable("redditbotmail");
        public string? BotPassword = System.Environment.GetEnvironmentVariable("redditbotpw");
        public Config()
        {

        }
    }

   

}