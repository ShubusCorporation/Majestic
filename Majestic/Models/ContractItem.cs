using System;
using System.Xml;
using System.Xml.Serialization;

namespace Majestic.Models
{
    [Serializable]
    public class ContractItem
    {
        [XmlAttribute]
        public int ID { get; set; }
        [XmlAttribute]
        public string User { get; set; }
        [XmlAttribute]
        public string Title { get; set; }
        [XmlAttribute]
        public string Status { get; set; }
    }
}
