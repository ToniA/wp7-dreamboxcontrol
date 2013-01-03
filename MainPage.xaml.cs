using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Notification;
using Microsoft.Phone.Shell;
using System.Windows.Navigation;
using Renci.SshNet;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Collections;
using System.Collections.ObjectModel;
using System.IO.IsolatedStorage;

// SEE http://www.codeproject.com/Articles/200910/A-Simple-Windows-Phone-7-MVVM-Tombstoning-Example

namespace DreamboxControl
{ 
    public partial class MainPage : PhoneApplicationPage
    {
        private e2eventlist e2eventlist;
        private e2servicelistrecursive e2servicelistrecursive;
        private e2timerlist e2timerlist;
        private e2locations e2locations;
        private e2filelist e2filelist;
        private e2movielist e2movielist;

        private BackgroundWorker SSHbackgroundWorker = new BackgroundWorker();
        private BackgroundWorker HTTPbackgroundWorker = new BackgroundWorker();

        private static Settings settings = Settings.Instance;

        public Collection<e2movie> movielist = new Collection<e2movie>();

        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (settings.SettingsSavedSetting == true)
            {
                // Show the black screen and start reading the information on a background thread

                ModalUtils.ShowProgressBar((DependencyObject)this, "Muodostetaan SSH-yhteyttä...");
                ModalUtils.ShowModalCanvas(LayoutRoot);

                SSHbackgroundWorker.DoWork += new DoWorkEventHandler(ReadNowrunningAndTimers);
                SSHbackgroundWorker.RunWorkerAsync();
                SSHbackgroundWorker.WorkerReportsProgress = true;
                SSHbackgroundWorker.ProgressChanged += new ProgressChangedEventHandler(ReadNowrunningAndTimers_ProgressChanged);
                SSHbackgroundWorker.WorkerSupportsCancellation = true;
                SSHbackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ReadNowrunningAndTimers_RunWorkerCompleted);

                HTTPbackgroundWorker.DoWork += new DoWorkEventHandler(CheckForHTTPConnection);
                HTTPbackgroundWorker.RunWorkerAsync();
                HTTPbackgroundWorker.WorkerReportsProgress = false;
                HTTPbackgroundWorker.WorkerSupportsCancellation = true;
                HTTPbackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CheckForHTTPConnection_RunWorkerCompleted);
            }
            else
            {
                this.NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
            }
            
            base.OnNavigatedTo(e);
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }

        public void CheckForHTTPConnection(object sender, DoWorkEventArgs e)
        {
 
        }

        void CheckForHTTPConnection_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
//            SSHbackgroundWorker.CancelAsync();
        }

        public void ReadNowrunningAndTimers(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            XmlSerializer e2servicelistrecursive_serializer = new XmlSerializer(typeof(e2servicelistrecursive));
            XmlSerializer e2eventlist_serializer = new XmlSerializer(typeof(e2eventlist));
            XmlSerializer e2timerlist_serializer = new XmlSerializer(typeof(e2timerlist));
            XDocument document;
            string result;

            SSHUtils.SSHConnect();

            worker.ReportProgress(30);

            // Get all services to read the current bouquet reference (bRef)

            result = SSHUtils.SSHExecute("wget -q -O - http://" + settings.DreamboxAddressSetting + "/web/getallservices");
            System.Diagnostics.Debug.WriteLine(result);
            document = XDocument.Parse(result);
            e2servicelistrecursive = (e2servicelistrecursive)e2servicelistrecursive_serializer.Deserialize(document.CreateReader());

            worker.ReportProgress(50);

            // Get the 'now running' information using the bRef

            result = SSHUtils.SSHExecute("wget -q -O - http://" + settings.DreamboxAddressSetting + "/web/epgnow?bRef=" + HttpUtility.UrlEncode(e2servicelistrecursive.e2bouquet.e2servicereference));
            System.Diagnostics.Debug.WriteLine(result);
            document = XDocument.Parse(result);
            e2eventlist = (e2eventlist)e2eventlist_serializer.Deserialize(document.CreateReader());

            worker.ReportProgress(80);

            // Get the timer list

            result = SSHUtils.SSHExecute("wget -q -O - http://" + settings.DreamboxAddressSetting + "/web/timerlist");
            System.Diagnostics.Debug.WriteLine(result);
            document = XDocument.Parse(result);
            e2timerlist = (e2timerlist)e2timerlist_serializer.Deserialize(document.CreateReader());
        }

        private void ReadNowrunningAndTimers_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            switch (e.ProgressPercentage)
            {
                case 30:
                    ModalUtils.ShowProgressBar((DependencyObject)this, "Luetaan kanavalistaa...");
                    break;
                case 50:
                    ModalUtils.ShowProgressBar((DependencyObject)this, "Luetaan ajastuksia...");
                    break;
                case 80:
                    ModalUtils.ShowProgressBar((DependencyObject)this, "Luetaan tallennuksia...");
                    break;
            }
        }


        void ReadNowrunningAndTimers_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ModalUtils.HideModalCanvas();
 
            if (e.Error != null)
            {
                ModalUtils.HideProgressBar((DependencyObject)this);
                MessageBox.Show("SSH-yhteysvirhe:\n" + e.Error.Message);
                return;
            }

            foreach (var e2event in e2eventlist.Collection)
            {
                App.mainViewModel.TVChannels.Add(e2event);
            }


            foreach (var e2timer in e2timerlist.Collection)
            {
                App.mainViewModel.Timers.Add(e2timer);
            }


            // Start reading the movielist

            BackgroundWorker SSHbackgroundWorker = new BackgroundWorker();
            SSHbackgroundWorker.DoWork += new DoWorkEventHandler(ReadMovielist);
            SSHbackgroundWorker.RunWorkerAsync("");
            SSHbackgroundWorker.WorkerReportsProgress = false;
            SSHbackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ReadMovielist_RunWorkerCompleted);
        }

        public void ReadMovielist(object sender, DoWorkEventArgs e)
        {
            XmlSerializer e2locations_serializer = new XmlSerializer(typeof(e2locations));
            XmlSerializer e2filelist_serializer = new XmlSerializer(typeof(e2filelist));
            XmlSerializer e2movielist_serializer = new XmlSerializer(typeof(e2movielist));
            XDocument document;
            string result;

            string requesteddir = e.Argument.ToString();
            string rootdir;

            movielist.Clear();

            // Get the locations

            result = SSHUtils.SSHExecute("wget -q -O - http://" + settings.DreamboxAddressSetting + "/web/getlocations");
            System.Diagnostics.Debug.WriteLine(result);
            document = XDocument.Parse(result);
            e2locations = (e2locations)e2locations_serializer.Deserialize(document.CreateReader());

            rootdir = e2locations.e2location;
                
            
            if (requesteddir.Equals(""))
            {
                requesteddir = rootdir;
            }

            // Get the subdirectories

            result = SSHUtils.SSHExecute("wget -q -O - http://" + settings.DreamboxAddressSetting + "/web/mediaplayerlist?path=" + HttpUtility.UrlEncode(requesteddir));
            System.Diagnostics.Debug.WriteLine(result);
            document = XDocument.Parse(result);
            e2filelist = (e2filelist)e2filelist_serializer.Deserialize(document.CreateReader());
            
            // Strip everything else but subdirectories, do not show the parent directory or the trashcan for the root dir

            foreach ( e2file i in e2filelist.Collection ) {
                if (i.e2isdirectory.Equals("True"))
                {
                    string dirname = System.IO.Path.GetDirectoryName(i.e2servicereference);
                    dirname = System.IO.Path.GetFileName(dirname);

                    if ( ! dirname.Equals(".trashcan" ) && 
                         i.e2servicereference.Length > requesteddir.Length ) 
                    {
                        e2movie directory = new e2movie();

                        directory.isdirectory = 1;
                        directory.issubdirectory = 1;
                        directory.e2description = "hakemisto";
                        directory.e2title = dirname;
                        directory.e2servicereference = i.e2servicereference;

                        movielist.Add(directory);
                    }

                    // Show the parent directory if we have navigated to a subdirectory

                    if ( i.e2servicereference.Length >= rootdir.Length && requesteddir.Length > rootdir.Length )
                    {
                        e2movie directory = new e2movie();

                        directory.isdirectory = 1;
                        directory.issubdirectory = 0;
                        directory.e2description = "hakemisto";
                        directory.e2title = "<ylös>";
                        directory.e2servicereference = i.e2servicereference;

                        movielist.Add(directory);
                    }
                }
            }

            // Get the movie list

            result = SSHUtils.SSHExecute("wget -q -O - http://" + settings.DreamboxAddressSetting + "/web/movielist?dirname=" + HttpUtility.UrlEncode(requesteddir));
            System.Diagnostics.Debug.WriteLine(result);
            document = XDocument.Parse(result);
            e2movielist = (e2movielist)e2movielist_serializer.Deserialize(document.CreateReader());

            foreach (e2movie i in e2movielist.Collection)
            {
                i.isdirectory = 0;
                movielist.Add(i);
            }

            e.Result = "";
        }

        void ReadMovielist_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ModalUtils.HideProgressBar((DependencyObject)this);

            App.mainViewModel.SelectedMovielist.Clear();

            foreach (var e2movie in movielist)
            {
                App.mainViewModel.SelectedMovielist.Add(e2movie);
            }
        }

        private void Exit()
        {
            while (NavigationService.BackStack.Any())
                NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }

// Click actions

        public void Timerlist_SelectionChanged(object sender, EventArgs e)
        {
            // if an item is selected
            if (Timerlist.SelectedIndex != -1)
            {
                e2timer timer = (e2timer)Timerlist.SelectedItem;
                Timerlist.SelectedIndex = -1;

                this.NavigationService.Navigate(new Uri("/TimerPage.xaml?e2eit=" + HttpUtility.UrlEncode(timer.e2eit.ToString()), UriKind.Relative));
            }
        }

        private void TVChannels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // if an item is selected
            if (Channellist.SelectedIndex != -1)
            {
                e2event channel = (e2event)Channellist.SelectedItem;
                Channellist.SelectedIndex = -1;

                this.NavigationService.Navigate(new Uri("/EPGListPage.xaml?sRef=" + HttpUtility.UrlEncode(channel.e2eventservicereference) + 
                                                        "&ChannelName=" + HttpUtility.UrlEncode(channel.e2eventservicename), UriKind.Relative));
            }
        }

        private void Movielist_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // if an item is selected
            if (Movielist.SelectedIndex != -1)
            {
                e2movie movie = (e2movie)Movielist.SelectedItem;
                Movielist.SelectedIndex = -1;

                if (movie.isdirectory != 0)
                {
                    System.Diagnostics.Debug.WriteLine("Need to read directory " + movie.e2servicereference );

                    // Start reading the movielist

                    ModalUtils.ShowProgressBar((DependencyObject)this, "Luetaan tallennuksia...");
                    BackgroundWorker SSHbackgroundWorker = new BackgroundWorker();
                    SSHbackgroundWorker.DoWork += new DoWorkEventHandler(ReadMovielist);
                    SSHbackgroundWorker.RunWorkerAsync(movie.e2servicereference);
                    SSHbackgroundWorker.WorkerReportsProgress = false;
                    SSHbackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ReadMovielist_RunWorkerCompleted);               
                }
                else
                {
                    this.NavigationService.Navigate(new Uri("/MoviePage.xaml?sRef=" + HttpUtility.UrlEncode(movie.e2servicereference), UriKind.Relative));
                }
            }
        }

        // Settings page

        private void Settings_Click(object sender, EventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/SettingsPage.xaml", UriKind.Relative));
        }

        // Back key override on recordings

        protected override void OnBackKeyPress (System.ComponentModel.CancelEventArgs e)
        {
            // Check if we are in Recordings, i.e. on Pivot page 2

            Pivot myPivot = LayoutRoot.FindName("PivotMain") as Pivot;

            if (myPivot.SelectedIndex == 2)
            {
                System.Diagnostics.Debug.WriteLine("Back key pressed on recordings");

                System.Diagnostics.Debug.WriteLine("Need to read directory " + App.mainViewModel.SelectedMovielist[0].e2title);

                if (App.mainViewModel.SelectedMovielist[0].issubdirectory == 0)
                {
                    ModalUtils.ShowProgressBar((DependencyObject)this, "Luetaan tallennuksia...");
                    BackgroundWorker SSHbackgroundWorker = new BackgroundWorker();
                    SSHbackgroundWorker.DoWork += new DoWorkEventHandler(ReadMovielist);
                    SSHbackgroundWorker.RunWorkerAsync(App.mainViewModel.SelectedMovielist[0].e2servicereference);
                    SSHbackgroundWorker.WorkerReportsProgress = false;
                    SSHbackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ReadMovielist_RunWorkerCompleted);

                    e.Cancel = true;
                }
            }
        }
    }
}