using System.Xml.Serialization;

namespace ODE.Astronomy
{
    public class Connector
    {
        [XmlAttribute]
        public int From { get; set; }

        [XmlAttribute()]
        public int To { get; set; }
    }
}