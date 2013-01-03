using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.Xml.Linq;
using System.Linq;

namespace DreamboxControl
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
            this.TVChannels = new ObservableCollection<e2event>();
            this.SelectedTVChannel = new ObservableCollection<e2event>();
            this.Timers = new ObservableCollection<e2timer>();
            this.SelectedMediaList = new ObservableCollection<e2file>();
            this.SelectedMovielist = new ObservableCollection<e2movie>();
            this.rootdir = "";
            this.currentdir = "";
        }

        public ObservableCollection<e2event> TVChannels { get; set; }
        public ObservableCollection<e2event> SelectedTVChannel { get; set; }
        public ObservableCollection<e2timer> Timers { get; set; }
        public ObservableCollection<e2file>  SelectedMediaList { get; set; }
        public ObservableCollection<e2movie> SelectedMovielist { get; set; }

        public string rootdir { get; set; }
        public string currentdir { get; set; }


        // get the given event from eventlist
        public e2event getEventListItem(string e2eventid)
        {
            return SelectedTVChannel.SingleOrDefault(item => item._e2eventid == e2eventid);
        }

        // get the given timer from eventlist
        public e2timer getTimerListItem(int e2eit)
        {
            return Timers.SingleOrDefault(item => item.e2eit == e2eit);
        }

        // get the given movie from movie
        public e2movie getMovieListItem(string sRef)
        {
            return SelectedMovielist.SingleOrDefault(item => item.e2servicereference == sRef);
        }

        // Change notifications, without this nothing is displayed

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
