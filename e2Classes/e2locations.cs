using System;
using System.Net;
using System.Windows;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace DreamboxControl
{

    // http://e2devel.com/apidoc/webif/#getlocations

    [XmlRoot("e2locations")]
    public class e2locations
    {
        [XmlElement("e2location")]
        public string e2location { get; set; }
    }
}
