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
using Microsoft.Phone.Controls;
using Renci.SshNet;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
using System.IO.IsolatedStorage;

namespace DreamboxControl
{
    public static class SSHUtils {

        private static SshClient client;
        private static Settings settings = Settings.Instance;

        public static void SSHConnect()
        {
            string host = settings.SSHServerSetting;
            int port = Convert.ToInt32(settings.SSHPortSetting);
            string username = settings.SSHAccountSetting;

            if (settings.SSHUseKeySetting)
            {
                string sshkey = settings.SSHKeySetting;

                byte[] s = System.Text.Encoding.GetEncoding("iso-8859-1").GetBytes(sshkey);
                MemoryStream m = new MemoryStream(s);

                client = new SshClient(host, port, username, new PrivateKeyFile(m));
            }
            else
            {
                string password = settings.SSHPasswordSetting;

                client = new SshClient(host, port, username, password);
            }

            System.Diagnostics.Debug.WriteLine("SSH client done");
            client.Connect();
            System.Diagnostics.Debug.WriteLine("SSH connect done");
        }

        public static string SSHExecute(string sshcommand)
        {
            SshCommand cmd;
            string result;

            System.Diagnostics.Debug.WriteLine("CMD: " + sshcommand);

            cmd = client.CreateCommand(sshcommand); // Dreambox uses Unicode
            cmd.CommandTimeout = TimeSpan.FromSeconds(30);
            result = cmd.Execute();

            System.Diagnostics.Debug.WriteLine("RES: " + result);

            return result;
        }

        public static void SSHDisconnect()
        {
            client.Disconnect();
            System.Diagnostics.Debug.WriteLine("SSH disconnect done");
        }
    }
}