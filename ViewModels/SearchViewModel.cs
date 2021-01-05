using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RomScraper_DesktopApp.ViewModels
{
    public class SearchViewModel
    {
        public List<LinkListItem> SearchRows { set; get; }

        public SearchViewModel()
        {
            SearchRows = new List<LinkListItem>();
        }
    }
}
