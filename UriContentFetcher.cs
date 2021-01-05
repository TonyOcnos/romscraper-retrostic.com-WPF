using HtmlAgilityPack;
using System.Collections.Generic;
using System.Windows;

namespace RomScraper_DesktopApp

{
    //Class to extract the different content (node or collection of nodes) we will use for the downloading service
    public static class UriContentFetcher
    {

        #region RomSiteXPATHRetriever

        public static Dictionary<string, string> RomSiteXPATHRetriever { get; set; } = new Dictionary<string, string>()
        {
            { "PlatformsLinks", "//td[@class='d-block d-sm-none text-center']/a[contains(@href,'/roms/')]" },
            { "RomsLinks", "//td[@class='d-block d-sm-none text-center']/a[contains(@href,'/roms/')]" },
            { "FileName", "//td[contains(text(), '.zip') or contains(text(), '.bin')]" },
            { "Script", "//script[@type='text/javascript']" },
            { "SessionData", "//input[@type='hidden']" },
            { "Uri", "//a[@data-hook='review-title']" }
        };

        #endregion

        //Dynamic function to select node or collection of nodes depending of the boolean parameter "mode".
        public static dynamic GetContent(string uri, string xpath, bool mode)
        {
            var htmlDocument = new HtmlDocument();
            try
            {
                HtmlWeb htmlweb = new HtmlWeb();
                htmlDocument = htmlweb.Load(uri);
            }
            catch
            {
                MessageBox.Show("Connecting to site and retrieving data failed", "RomScraper - Fetching content...", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            
            if (mode)
            {
                return htmlDocument.DocumentNode.SelectNodes(xpath);
            }
            return htmlDocument.DocumentNode.SelectSingleNode(xpath);
        }
    }
}