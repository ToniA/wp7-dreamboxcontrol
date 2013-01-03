using System;
using System.Net;
using System.Windows;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace DreamboxControl
{

    // http://e2devel.com/apidoc/webif/#timerlist

    [XmlRoot("e2timerlist")]
    public class e2timerlist
    {
        [XmlElement("e2timer")]
        public ObservableCollection<e2timer> Collection { get; set; }
    }

    public class e2timer
    {
        [XmlElement("e2servicereference")]
        public string e2servicereference { get; set; }

        [XmlElement("e2servicename")]
        public string e2servicename { get; set; }

        [XmlElement("e2name")]
        public string e2name { get; set; }

        [XmlElement("e2eit")]
        public int e2eit { get; set; }

        [XmlElement("e2timebegin")]
        public int e2timebegin { get; set; }

        [XmlElement("e2timeend")]
        public int e2timeend { get; set; }

        [XmlElement("e2duration")]
        public int e2duration { get; set; }

        [XmlElement("e2description")]
        public string e2description { get; set; }

        [XmlIgnore]
        public string timerheader
        {
            get
            {
                return unixtimeToString(e2timebegin) + " - " +
                       unixtimeToString(e2timeend) + "\n" +
                       e2description;
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
