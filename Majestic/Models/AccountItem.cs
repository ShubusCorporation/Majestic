using System;
using System.Xml;
using System.Xml.Serialization;

namespace Majestic.Models
{
    [Serializable]
    public class AccountItem
    {
        [XmlAttribute]
        public int ID { get; set; }
        [XmlAttribute]
        public string Login { get; set; }
        [XmlAttribute]
        public string Password { get; set; }
        [XmlAttribute]
        public string Role { get; set; }
    }
}
