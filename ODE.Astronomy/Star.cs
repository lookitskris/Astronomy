using System.Xml.Serialization;

namespace ODE.Astronomy
{
    public class Star : CelestialBody
    {
        [XmlAttribute]
        public int Number { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        // J2000 in hours
        [XmlAttribute]
        public double RightAscension { get; set; }

        // J200 in degrees
        [XmlAttribute]
        public double Declination { get; set; }

        [XmlAttribute]
        public double Magnitude { get; set; }

        // J2000 in arcsec / year
        [XmlAttribute]
        public double RightAscensionProperMotion { get; set; }

        // J2000 in arcsec / year
        [XmlAttribute]
        public double DeclinationProperMotion { get; set; }

        [XmlAttribute]
        public string LongName { get; set; }

        [XmlIgnore]
        public bool IsConnectedInConstellation { get; set; }

        protected override void OnTimeChanged()
        {
            int years = this.Time.UniversalTime.Year - 2000;
            Angle ra = Angle.FromHours(this.RightAscension) + Angle.FromHours(0, 0, this.RightAscensionProperMotion * years);

            Angle dec = Angle.FromDegrees(this.Declination) + Angle.FromDegrees(0, 0, this.DeclinationProperMotion * years);

            this.EquatorialCoordinate = new EquatorialCoordinate(ra, dec);

            base.OnTimeChanged();
        }
    }
}