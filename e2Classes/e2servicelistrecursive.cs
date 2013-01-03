using System.Xml.Serialization;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;

namespace DreamboxControl
{

    // http://dream.reichholf.net/wiki/Enigma2:WebInterface#Bouquets

    [XmlRoot("e2servicelistrecursive")]
    public class e2servicelistrecursive
    {
        [XmlElement("e2bouquet")]
        public e2bouquet e2bouquet { get; set; }
    }

    public class e2bouquet
    {
        [XmlElement("e2servicereference")]
        public string e2servicereference { get; set; }

        [XmlElement("e2servicename")]
        public string e2servicename { get; set; }

        [XmlArray("e2servicelist")]
        [XmlArrayItem("e2service")]
        public ObservableCollection<e2service> Collection { get; set; }
    }

    public class e2service
    {
        [XmlElement("e2servicereference")]
        public string e2servicereference { get; set; }

        [XmlElement("e2servicename")]
        public string e2servicename { get; set; }
    }
}
