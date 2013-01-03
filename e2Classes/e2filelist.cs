using System;
using System.Net;
using System.Windows;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace DreamboxControl
{

    // http://e2devel.com/apidoc/webif/#mediaplayerlist

    [XmlRoot("e2filelist")]
    public class e2filelist
    {
        [XmlElement("e2file")]
        public ObservableCollection<e2file> Collection { get; set; }
    }

    public class e2file
    {
        [XmlElement("e2servicereference")]
        public string e2servicereference { get; set; }

        [XmlElement("e2isdirectory")]
        public string e2isdirectory { get; set; }

        [XmlElement("e2root")]
        public string e2root { get; set; }
    }
}
