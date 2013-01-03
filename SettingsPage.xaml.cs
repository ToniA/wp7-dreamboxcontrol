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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.IO.IsolatedStorage;

namespace DreamboxControl
{
    public partial class SettingsPage : PhoneApplicationPage
    {
        private Settings settings = Settings.Instance;

        public SettingsPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void SaveSettings_Click(object sender, EventArgs e)
        {
            settings.DreamboxAddressSetting = DreamboxAddress.Text;
            settings.SSHServerSetting = SSHServer.Text;
            settings.SSHPortSetting = SSHPort.Text;
            settings.SSHAccountSetting = SSHAccount.Text;
            settings.SSHPasswordSetting = SSHPassword.Password;
            settings.SSHUsePasswordSetting = (Boolean)SSHUsePassword.IsChecked;
            settings.SSHUseKeySetting = (Boolean)SSHUseKey.IsChecked;
            settings.SSHKeySetting = SSHKey.Text;
            settings.Save();

            NavigationService.GoBack();
        }
    }
}