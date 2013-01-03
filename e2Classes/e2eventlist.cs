using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Serialization;
using System.Collections.ObjectModel;

namespace DreamboxControl
{
    // http://e2devel.com/apidoc/webif/#getcurrent

    [XmlRoot("e2eventlist")]
    public class e2eventlist
    {
        [XmlElement("e2event")]
        public ObservableCollection<e2event> Collection { get; set; }
    }

    public class e2event
    {
        [XmlElement("e2eventid")] // these can be 'none' for services which are not running -> map to 0
        public string _e2eventid { get; set; }

        [XmlIgnore]
        public int e2eventid
        {
            get
            {
                return parsetime(_e2eventid);
            }
        }

        [XmlElement("e2eventstart")]
        public string _e2eventstart { get; set; }

        [XmlIgnore]
        public int e2eventstart
        {
            get
            {
                return parsetime(_e2eventstart);
            }
        }

        [XmlElement("e2eventduration")]
        public string _e2eventduration { get; set; }

        [XmlIgnore]
        public int e2eventduration
        {
            get
            {
                return parsetime(_e2eventduration);
            }
        }

        [XmlElement("e2eventcurrenttime")]
        public string _e2eventcurrenttime { get; set; }

        [XmlIgnore]
        public int e2eventcurrenttime
        {
            get
            {
                return parsetime(_e2eventcurrenttime);
            }
        }

        [XmlElement("e2eventtitle")]
        public string e2eventtitle { get; set; }

        [XmlElement("e2eventdescription")]
        public string e2eventdescription { get; set; }

        [XmlElement("e2eventdescriptionextended")]
        public string e2eventdescriptionextended { get; set; }

        [XmlElement("e2eventservicereference")]
        public string e2eventservicereference { get; set; }

        [XmlElement("e2eventservicename")]
        public string e2eventservicename { get; set; }

        [XmlIgnore]
        public Boolean hasTimer { get; set; }


        [XmlIgnore]
        public string serviceheader
        {
            get {
                if (e2eventstart > 0)
                {
                    return unixtimeToString(e2eventstart) + " - " +
                           unixtimeToString(e2eventstart + e2eventduration) + "\n" +
                           e2eventtitle;
                }
                else
                {
                    return "";
                }
            }
        }

        [XmlIgnore]
        public string descriptionheader
        {
            get
            {
                if (e2eventstart > 0)
                {
                    return unixtimeToString(e2eventstart) + " - " +
                           unixtimeToString(e2eventstart + e2eventduration) + "\n" +
                           e2eventdescription;
                }
                else
                {
                    return "";
                }
            }
        }

        public int parsetime(string timestring)
        {
            try
            {
                return Int32.Parse(timestring);
            }
            catch
            {
                return 0;
            }
        }
        
        // Convert Unix timestamp to local time

        private string unixtimeToString(int unixtime)
        {
            if (unixtime > 0)
            {
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                dateTime = dateTime.AddSeconds(unixtime).ToLocalTime();
                return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
            }
            else
            {
                return "";
            }
        }
    }           
}   
