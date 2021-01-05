using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RomScraper_DesktopApp
{
    public class LinkListItem : INotifyPropertyChanged
    {
        private string _linkInnertext;
        public string LinkInnerText
        {
            get
            {
                return _linkInnertext;
            }

            set
            {
                _linkInnertext = value;
                OnPropertyChanged("LinkInnerText");
            }
        }
        public string Uri { get; set; }
        public HtmlNodeCollection RomListLinks { get; set; }

        public LinkListItem(string linkInnerText, HtmlNode uri)
        {
            LinkInnerText = linkInnerText;
            Uri = uri.Attributes["href"].Value;
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