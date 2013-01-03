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

namespace DreamboxControl
{
    public partial class MoviePage : PhoneApplicationPage
    {
        public MoviePage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            string sRef = this.NavigationContext.QueryString["sRef"];

            this.DataContext = App.mainViewModel.getMovieListItem(sRef);
        }

        private void DeleteMovie_Click(object sender, EventArgs e)
        {
            e2movie movie = (e2movie)this.DataContext;

            if (MessageBox.Show(movie.e2title, "Poistetaanko tallennus?", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                ModalUtils.ShowProgressBar((DependencyObject)this, "Poistetaan tallennusta...");
                Thread.Sleep(2000);
                ModalUtils.HideProgressBar((DependencyObject)this);

                // This isn't really removed yet, but the GUI shows it...

                App.mainViewModel.SelectedMovielist.Remove(App.mainViewModel.getMovieListItem(movie.e2servicereference));

                NavigationService.GoBack();
            }
        }
    }
}