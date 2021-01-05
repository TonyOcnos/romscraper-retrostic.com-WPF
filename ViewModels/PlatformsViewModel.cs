using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RomScraper_DesktopApp 
{ 
    public class PlatformsViewModel
    {
        public List<LinkListItem> PlatformRows { get; set; }

        public PlatformsViewModel()
        {
            PlatformRows = new List<LinkListItem>();
        }
    }
}
