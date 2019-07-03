using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Serialization;

namespace ODE.Astronomy
{
    public class Constellations
    {
        public Constellations()
        {
            this.ConstellationList = new List<Constellation>();
        }

        public static Constellations Load()
        {
            // File must be flagged as 'Resource'
            Uri uri = new Uri("/ODE.Astronomy;component/Data/constellations.xml", UriKind.Relative);
            StreamResourceInfo resourceInfo = Application.GetResourceStream(uri);
            XmlSerializer serializer = new XmlSerializer(typeof(Constellations));
            Constellations constellations = null;

            using (Stream stream = resourceInfo.Stream)
            {
                constellations = serializer.Deserialize(stream) as Constellations;
            }

            foreach (Constellation constellation in constellations.ConstellationList)
                constellation.Initialize();

            return constellations;
        }

        [XmlElement(ElementName = "Constellation")]
        public List<Constellation> ConstellationList { get; set; }
    }
}