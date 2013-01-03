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

namespace DreamboxControl
{
    public partial class EPGListPage : PhoneApplicationPage
    {
        public e2eventlist e2eventlist;

        private static Settings settings = Settings.Instance;

        //EPGViewModel EPGlist;

        public EPGListPage()
        {
            InitializeComponent();
        }

       
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string sRef = this.NavigationContext.QueryString["sRef"];
            string ChannelName = this.NavigationContext.QueryString["ChannelName"];

            // replace with binding?
            TextBlock pagetitle = LayoutRoot.FindName("PageTitle") as TextBlock;
            pagetitle.Text = ChannelName;

            if (App.mainViewModel.SelectedTVChannel.Count == 0 ||
                !App.mainViewModel.SelectedTVChannel[0].e2eventservicereference.Equals(sRef))
            {
                BackgroundWorker SSHbackgroundWorker = new BackgroundWorker();
                SSHbackgroundWorker.DoWork += new DoWorkEventHandler(ReadEPGInfo);
                SSHbackgroundWorker.RunWorkerAsync(sRef);
                SSHbackgroundWorker.WorkerReportsProgress = false;
                SSHbackgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(ReadMovielist_RunWorkerCompleted);

                ModalUtils.ShowProgressBar((DependencyObject)this, "Luetaan EPG:tä...");
                ModalUtils.ShowModalCanvas(LayoutRoot);
            }
         }

        public void ReadEPGInfo(object sender, DoWorkEventArgs e)
        {
            XmlSerializer e2eventlist_serializer = new XmlSerializer(typeof(e2eventlist));
            XDocument document;
            string result;

            result = SSHUtils.SSHExecute("wget -q -O - http://" + settings.DreamboxAddressSetting + "/web/epgservice?sRef=" + HttpUtility.UrlEncode(e.Argument.ToString()));
            System.Diagnostics.Debug.WriteLine(result);
            document = XDocument.Parse(result);
            e2eventlist = (e2eventlist)e2eventlist_serializer.Deserialize(document.CreateReader());
        }

        void ReadMovielist_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            App.mainViewModel.SelectedTVChannel.Clear();
            
            foreach (var e2event in e2eventlist.Collection)
            {
                App.mainViewModel.SelectedTVChannel.Add(e2event);

                if (App.mainViewModel.getTimerListItem(e2event.e2eventid) != null)
                {
                    e2event.hasTimer = true;
                }
                else
                {
                    e2event.hasTimer = false;
                }
            }
            
            //App.mainViewModel.EPGe2eventlist = e2eventlist.Collection;

            //EPGListInfo.ItemsSource = e2eventlist.Collection;
            ModalUtils.HideProgressBar((DependencyObject)this);
            ModalUtils.HideModalCanvas();
        }

        private void EPGEvent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // if an item is selected
            if (EPGListInfo.SelectedIndex != -1)
            {
                e2event program = (e2event)EPGListInfo.SelectedItem;
                
                EPGListInfo.SelectedIndex = -1;

                this.NavigationService.Navigate(new Uri("/EPGEventPage.xaml?e2eventid=" + program._e2eventid , UriKind.Relative));
            }
        }
    }
}