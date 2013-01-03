using System;
using System.Net;
using System.Windows;
using System.Xml.Serialization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace DreamboxControl
{

    // http://e2devel.com/apidoc/webif/#timeradd

    [XmlRoot("e2simplexmlresult")]
    public class e2simplexmlresult
    {
        [XmlElement("e2state")]
        public string e2state { get; set; }

        [XmlElement("e2statetext")]
        public string e2statetext { get; set; }
    }
}
