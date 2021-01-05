using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace RomScraper_DesktopApp
{
    public class LibraryViewModel
    {
        public ObservableCollection<PlatformLibrary> LibraryRows { get; set; }

        public LibraryViewModel()
        {
            LibraryRows = new ObservableCollection<PlatformLibrary>();
        }
    }
}
