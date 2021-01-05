using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Controls;

namespace RomScraper_DesktopApp
{
    public class MyLog : INotifyPropertyChanged
    {
        private string _logtext;

        public string LogText
        {
            get
            {
                return _logtext;
            }
            set
            {
                if (_logtext != value)
                {
                    _logtext = value;
                    OnPropertyChanged("LogText");
                }
            }
        }

        public ScrollViewer LogBox { get; set; }

        public MyLog(ScrollViewer logBox)
        {
            LogBox = logBox;
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

        public void UpdateLog(string text)
        {
            LogBox.ScrollToEnd();
            LogText += $"{text}\n\n";
        }
    }
}
