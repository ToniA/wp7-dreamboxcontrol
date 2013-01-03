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

    public static class ModalUtils
    {
        private static Canvas myCanvas;

        public static void ShowProgressBar(DependencyObject DO, string text)
        {
            SystemTray.SetProgressIndicator(DO, new ProgressIndicator
            {
                IsIndeterminate = true,
                IsVisible = true,
                Text = text
            });
        }

        public static void ShowModalCanvas(Grid LayoutRoot)
        {
            myCanvas = LayoutRoot.FindName("modal") as Canvas;
            myCanvas.Visibility = Visibility.Visible;
        }

        public static void HideProgressBar(DependencyObject DO)
        {
            SystemTray.SetProgressIndicator(DO, new ProgressIndicator
            {
                IsVisible = false
            });
        }

        public static void HideModalCanvas()
        {
            myCanvas.Visibility = Visibility.Collapsed;
        }
    }
}