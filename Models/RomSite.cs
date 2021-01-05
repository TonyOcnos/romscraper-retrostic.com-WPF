using HtmlAgilityPack;

namespace RomScraper_DesktopApp
{
    public class RomSite
    {
        public HtmlNodeCollection SiteListLinks { get; set; }
        public string MainUri { get; set; }

        public RomSite(string uri)
        {
            this.MainUri = uri;
        }
    }
}