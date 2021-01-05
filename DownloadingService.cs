using HtmlAgilityPack;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;

namespace RomScraper_DesktopApp
{
    public class DownloadingService
    {
        public string PlatformDirectory { get; set; }
        public int totalRomsDownloaded = 0;

        public DownloadingService(string platform)
        {
            PlatformDirectory = $@"{DirectoryFetcher.currentDirectory}/roms/{platform}";
            totalRomsDownloaded = 0;
        }

        public string RomDownloader(HtmlNode node)
        {
            string uri = node.Attributes["href"].Value;
            var sessionData = UriContentFetcher.GetContent(@"https://www.retrostic.com" + uri, UriContentFetcher.RomSiteXPATHRetriever["SessionData"], true);
            var fileNameFetcher = UriContentFetcher.GetContent(@"https://www.retrostic.com" + uri, UriContentFetcher.RomSiteXPATHRetriever["FileName"], false);

            if (!DirectoryFetcher.CheckRedundantRom(node, PlatformDirectory))
            {
                try
                {
                    string romDownloadPageContent = PullRequest(@"https://www.retrostic.com" + uri + "/download", sessionData[0].Attributes["value"].Value, sessionData[1].Attributes["value"].Value, sessionData[2].Attributes["value"].Value);
                    string urlDownload = GetDownloadUrl(GetScript(romDownloadPageContent));

                    using var wClient = new WebClient();
                    Uri uriRom = new Uri(urlDownload);

                    string fileName = PlatformDirectory + "/" + fileNameFetcher.InnerText;

                    try
                    {
                        wClient.DownloadFile(@uriRom, fileName);
                        totalRomsDownloaded++;

                        return $"Rom: {node.InnerText} ---> {totalRomsDownloaded} rom(s) downloaded!";
                    }
                    catch (Exception e)
                    {
                        return $"Rom: {node.InnerText} - Download Failed // Error: {e.Message}";
                    }
                }
                catch (Exception e)
                {
                    return $"Rom: {node.InnerText} - Download Failed // Error: {e.Message}";
                }
            }
            return $"Rom: {node.InnerText} exist already in your library!";
        }

        public static string PullRequest(string website, string param1, string param2, string param3)
        {
            using var client = new WebClient();
            var values = new NameValueCollection
            {
                ["rom_url"] = param1,
                ["console_url"] = param2,
                ["session"] = param3
            };
            var response = client.UploadValues(website, values);
            return Encoding.Default.GetString(response);
        }

        public static string GetScript(string romContent)
        {
            var romPage = new HtmlDocument();
            romPage.LoadHtml(romContent);
            var romLink = romPage.DocumentNode.SelectSingleNode(UriContentFetcher.RomSiteXPATHRetriever["Script"]);
            return romLink.OuterHtml;
        }

        public static string GetDownloadUrl(string romScript)
        {
            int lengthSubs;
            if (romScript.IndexOf(".zip") != -1)
            {
                lengthSubs = romScript.IndexOf(".zip") - romScript.IndexOf("https");
            }
            else
            {
                lengthSubs = romScript.IndexOf(".bin") - romScript.IndexOf("https");
            }
            return romScript.Substring(romScript.IndexOf("https"), lengthSubs + 4);
        }
    }
}
