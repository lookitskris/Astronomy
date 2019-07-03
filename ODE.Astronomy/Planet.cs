using System.Windows.Media;

namespace ODE.Astronomy
{
    public class Planet : SolarSystemBody
    {
        private VsopParser vsop;

        public Planet(string strPlanetAbbreviation, string name, Color color) : this(strPlanetAbbreviation)
        {
            this.Name = name;
            this.Color = color;
        }

        protected Planet(string strPlanetAbbreviation)
        {
            vsop = new VsopParser(strPlanetAbbreviation);
        }

        protected override void OnTimeChanged()
        {
            Angle latitude = Angle.FromRadians(vsop.GetLatitude(this.Time.Tau));
            Angle longitude = Angle.FromRadians(vsop.GetLongitude(this.Time.Tau));
            double radius = vsop.GetRadius(this.Time.Tau);

            this.HeliocentricLocation = new EclipticCoordinate(longitude, latitude, radius).RectangularCoordinates;

            base.OnTimeChanged();
        }
    }
}