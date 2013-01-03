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
using System.Threading;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.Collections;
using System.Collections.ObjectModel;

namespace DreamboxControl
{
    public partial class EPGEventPage : PhoneApplicationPage
    {
        private static Settings settings = Settings.Instance;

        public EPGEventPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string e2eventid = this.NavigationContext.QueryString["e2eventid"];

            this.DataContext = App.mainViewModel.getEventListItem(e2eventid);
        }

        private void SetTimer_Click(object sender, EventArgs e)
        {
            e2event EPGEvent = (e2event)this.DataContext;
            if (MessageBox.Show(EPGEvent.e2eventtitle, "Ajastetaanko tallennus?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                XmlSerializer e2simplexmlresult_serializer = new XmlSerializer(typeof(e2simplexmlresult));
                XDocument document;
                string result;
                e2simplexmlresult xmlresult;

                ModalUtils.ShowProgressBar((DependencyObject)this, "Luodaan ajastusta...");

                result = SSHUtils.SSHExecute("wget -q -O - \"http://" + settings.DreamboxAddressSetting + "/web/timeraddbyeventid?sRef=" + HttpUtility.UrlEncode(EPGEvent.e2eventservicereference) + 
                                             "&eventid=" + EPGEvent.e2eventid.ToString() + "\"");
                System.Diagnostics.Debug.WriteLine(result);
                document = XDocument.Parse(result);
                xmlresult = (e2simplexmlresult)e2simplexmlresult_serializer.Deserialize(document.CreateReader());

                ModalUtils.HideProgressBar((DependencyObject)this);

                if (xmlresult.e2state.Equals("True"))
                {
                    MessageBox.Show(xmlresult.e2statetext, "Ajastus onnistui", MessageBoxButton.OK);
                    // should read timers again...
                }
                else
                {
                    MessageBox.Show(xmlresult.e2statetext, "Ajastuksen luonti ei onnistunut", MessageBoxButton.OK);
                }
                
                NavigationService.GoBack();
            }
        }
    }
}