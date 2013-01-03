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
    public partial class TimerPage : PhoneApplicationPage
    {
        private static Settings settings = Settings.Instance;

        public TimerPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            int e2eit = Int32.Parse(this.NavigationContext.QueryString["e2eit"]);

            this.DataContext = App.mainViewModel.getTimerListItem(e2eit);
        }

        private void DeleteTimer_Click(object sender, EventArgs e)
        {
            e2timer timer = (e2timer)this.DataContext;

            if (MessageBox.Show(timer.e2name, "Poistetaanko ajastus?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                XmlSerializer e2simplexmlresult_serializer = new XmlSerializer(typeof(e2simplexmlresult));
                XDocument document;
                string result;
                e2simplexmlresult xmlresult;

                ModalUtils.ShowProgressBar((DependencyObject)this, "Poistetaan ajastusta...");

                result = SSHUtils.SSHExecute("wget -q -O - \"http://" + settings.DreamboxAddressSetting + "/web/timerdelete?sRef=" + HttpUtility.UrlEncode(timer.e2servicereference) +
                                             "&begin=" + timer.e2timebegin.ToString() + 
                                             "&end=" + timer.e2timeend.ToString() + "\"");
                System.Diagnostics.Debug.WriteLine(result);
                document = XDocument.Parse(result);
                xmlresult = (e2simplexmlresult)e2simplexmlresult_serializer.Deserialize(document.CreateReader());

                ModalUtils.HideProgressBar((DependencyObject)this);

                // This isn't really removed yet, but the GUI shows it...

                if (xmlresult.e2state.Equals("True"))
                {
                    MessageBox.Show(xmlresult.e2statetext, "Ajastuksen poisto onnistui", MessageBoxButton.OK);
                    // should read timers again...
                }
                else
                {
                    MessageBox.Show(xmlresult.e2statetext, "Ajastuksen poisto ei onnistunut", MessageBoxButton.OK);
                }

//                App.mainViewModel.Timers.Remove(App.mainViewModel.getTimerListItem(timer.e2eit));

                NavigationService.GoBack();
            }
        }
    }
}