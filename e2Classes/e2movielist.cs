using System;
using System.Net;
using System.Windows;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace DreamboxControl
{

    // http://e2devel.com/apidoc/webif/#movielist

    [XmlRoot("e2movielist")]
    public class e2movielist
    {
        [XmlElement("e2movie")]
        public ObservableCollection<e2movie> Collection { get; set; }
    }

    public class e2movie
    {
        [XmlElement("e2servicereference")]
        public string e2servicereference { get; set; }

        [XmlElement("e2title")]
        public string e2title { get; set; }

        [XmlElement("e2description")]
        public string e2description { get; set; }

        [XmlElement("e2descriptionextended")]
        public string e2descriptionextended { get; set; }

        [XmlElement("e2servicename")]
        public string e2servicename { get; set; }

        [XmlElement("e2time")]
        public int e2time { get; set; }

        [XmlElement("e2length")]
        public string e2length { get; set; }

        [XmlElement("e2tags")]
        public string e2tags { get; set; }

        [XmlElement("e2filename")]
        public string e2filename { get; set; }

        [XmlElement("e2filesize")]
        public Int64 e2filesize { get; set; }

        // not a part of the XML structure, but handy for representing directories
        [XmlIgnore]
        public int isdirectory { get; set; }

        [XmlIgnore]
        public int issubdirectory { get; set; }

        [XmlIgnore]
        public string movieheader
        {
            get
            {
                if (isdirectory == 1)
                {
                    return e2description;
                }
                else
                {
                    return unixtimeToString(e2time) + " - " +
                           e2length + "\n" +
                           e2description;
                }
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
