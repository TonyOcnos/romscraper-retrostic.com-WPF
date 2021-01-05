using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RomScraper_DesktopApp
{
    public class PlatformLibrary : INotifyPropertyChanged
    {
        private string _platformLibraryName;
        public string PlatformLibraryName
        {
            get
            {
                return _platformLibraryName;
            }
            set
            {
                _platformLibraryName = value;
                OnPropertyChanged("PlatformLibraryName");
            }
        }
        public int PlatformLibraryGames { get; set; }

        public PlatformLibrary(string name, int games)
        {
            this.PlatformLibraryName = name;
            this.PlatformLibraryGames = games;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
