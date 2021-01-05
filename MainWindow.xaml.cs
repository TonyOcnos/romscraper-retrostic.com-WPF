using HtmlAgilityPack;
using RomScraper_DesktopApp.ViewModels;
using System;
using System.Threading.Tasks;
using System.Windows;


namespace RomScraper_DesktopApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public bool cancellationToken;
        public MyLog RuntimeLog;
        public RomSite Rs { get; set; }
        public LinkListItem SelectedItem { get; set; }
        public DownloadingService Dw { get; set; }
        public HtmlNodeCollection RomSearchLinks { get; set; }
        public LibraryViewModel LibraryViewModel = new LibraryViewModel();
        public PlatformsViewModel PlatformsViewModel = new PlatformsViewModel();
        public SearchViewModel Sr = new SearchViewModel();
        
        public MainWindow()
        {            
            InitializeComponent();
            RuntimeLog = new MyLog(logBox);

            this.Title = "RomScraper > retrostic.com";
            this.SizeToContent = SizeToContent.WidthAndHeight;
            this.DataContext = RuntimeLog;

            RuntimeLog.LogText = "Log...\n\n";

            #region RelayCommands
            RelayCommand ConnectToSiteCommand = new RelayCommand(obj => ConnectToSite(), obj =>
            platformsView.Items.Count == 0
            );
            buttonConnect.Command = ConnectToSiteCommand;

            RelayCommand DownloadPlatformCommand = new RelayCommand(obj => DownloadPlatform(), obj =>
            platformsView.Items.Count != 0 &&
            platformsView.SelectedIndex != -1
            );
            buttonDownloadPlatform.Command = DownloadPlatformCommand;

            RelayCommand SearchRomCommand = new RelayCommand(obj => SearchRom(), obj =>
            platformsView.Items.Count != 0
            );
            buttonSearch.Command = SearchRomCommand;

            RelayCommand DownloadSingleRomCommand = new RelayCommand(obj => DownloadSingleRom(), obj =>
            platformsView.Items.Count != 0 &&
            searchView.SelectedIndex != -1
            );
            buttonDownloadRom.Command = DownloadSingleRomCommand;

            RelayCommand RefreshLibraryCommand = new RelayCommand(obj => RefreshLibrary());
            buttonRefreshLibrary.Command = RefreshLibraryCommand;

            RelayCommand CancelDownloadCommand = new RelayCommand(obj => CancelDownload(), obj =>
            platformsView.Items.Count != 0 &&
            cancellationToken == true
            );
            buttonDownloadPlatformCancel.Command = CancelDownloadCommand;

            RelayCommand CloseProgrammCommand = new RelayCommand(obj => this.Close());
            buttonClose.Command = CloseProgrammCommand;

            #endregion
        }

        private async void ConnectToSite()
        {
            try
            {
                Rs = new RomSite(@"https://www.retrostic.com/roms");

                await Task.Run(() => AddPlatformsLinks());

                LoadPlatforms();
            }
            catch (Exception ex)
            {
                RuntimeLog.UpdateLog($"Exception: {ex.GetType()}");
            }
            RuntimeLog.UpdateLog("Connected to retrostic.com\n" +
                    "Please select a platform to dowload, a game to search or have a look at your library");
        }

        private void DownloadPlatform()
        {
            cancellationToken = true;
            var pageCountingValidation = true;
            int page = 1;

            SelectedItem = platformsView.SelectedItem as LinkListItem;
            Dw = new DownloadingService(SelectedItem.LinkInnerText);

            RuntimeLog.UpdateLog(DirectoryFetcher.CheckPlatformDirectory(SelectedItem.LinkInnerText));

            while (pageCountingValidation)
            {
                try
                {
                    AddRomsLinks(page);
                    RuntimeLog.UpdateLog(page.ToString());
                }
                catch
                {
                    RuntimeLog.UpdateLog($"Total roms downloaded: {Dw.totalRomsDownloaded}");
                    pageCountingValidation = false;
                }

                ExecuteMultipleDowloads();
                if (wholeCheck.IsChecked == true && Dw.totalRomsDownloaded % 50 == 0)
                {
                    page++;
                }
                else
                {
                    pageCountingValidation = false;
                }
            }
        }

        private void SearchRom()
        {
            RomSearchLinks = UriContentFetcher.GetContent($@"https://www.retrostic.com/search?search_term_string={tbSearch.Text}", UriContentFetcher.RomSiteXPATHRetriever["RomsLinks"], true);
            if(RomSearchLinks != null)
            {
                LoadSearch(RomSearchLinks);
            }
            else 
            { 
            MessageBox.Show("No search results!", "RomScraper - Search results", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DownloadSingleRom()
        {
            var selectedIndex = searchView.SelectedIndex;
            SelectedItem = searchView.SelectedItem as LinkListItem;
            Dw = new DownloadingService("");
            ExecuteSingleDownload(RomSearchLinks[selectedIndex]);
        }

        private async void ExecuteMultipleDowloads()
        {
            foreach (var RomListLink in SelectedItem.RomListLinks)
            {
                if (cancellationToken) { 
                RuntimeLog.UpdateLog(await Task.Run(() => 
                    Dw.RomDownloader(RomListLink)
                    ));
                }
            }
        }

        private async void ExecuteSingleDownload(HtmlNode SelectedRom)
        {
            RuntimeLog.UpdateLog(await Task.Run(() =>
                Dw.RomDownloader(SelectedRom)));
        }

        private void CancelDownload()
        {
            RuntimeLog.UpdateLog("Cancelling download...");
            cancellationToken = false;
        }

        private void RefreshLibrary()
        {
            LibraryViewModel.LibraryRows.Clear();
            DirectoryFetcher.LibraryFecther(LibraryViewModel);
            libraryView.DataContext = LibraryViewModel;
        }

        public void AddRomsLinks(int page)
        {
            SelectedItem.RomListLinks = UriContentFetcher.GetContent(@"https://www.retrostic.com" + SelectedItem.Uri + "/page/" + page, UriContentFetcher.RomSiteXPATHRetriever["RomsLinks"], true);
        }

        public void AddPlatformsLinks()
        {
            Rs.SiteListLinks = UriContentFetcher.GetContent(Rs.MainUri, UriContentFetcher.RomSiteXPATHRetriever["PlatformsLinks"], true);
        }

        public void LoadPlatforms()
        {
            PlatformsViewModel.PlatformRows.Clear();
            foreach (var platformLink in Rs.SiteListLinks)
            {
                PlatformsViewModel.PlatformRows.Add(new LinkListItem(platformLink.InnerText, platformLink));

            }
            PlatformsViewModel.PlatformRows.Sort((x, y) => x.LinkInnerText.CompareTo(y.LinkInnerText));
            platformsView.DataContext = PlatformsViewModel;
        }

        private void LoadSearch(HtmlNodeCollection RomListLinks)
        {
            Sr.SearchRows.Clear();
            foreach (var item in RomListLinks)
            {
                Sr.SearchRows.Add(new LinkListItem(item.InnerText, item));

            }
            searchView.DataContext = Sr;
        }
    }
}