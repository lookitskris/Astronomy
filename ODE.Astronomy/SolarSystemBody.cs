
using System.Windows.Media;
using Vector3 = Microsoft.Xna.Framework.Vector3;

namespace ODE.Astronomy
{
    public class SolarSystemBody : CelestialBody
    {
        protected SolarSystemBody()
        {
        }

        protected SolarSystemBody(string name, Color color)
        {
            this.Name = name;
            this.Color = color;
        }

        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            protected set
            {
                _name = value;
            }
        }

        private Color _color;
        public Color Color
        {
            get
            {
                return _color;
            }
            protected set
            {
                _color = value;
            }
        }

        // Set by descendent classes, used here
        private Vector3 _heliocentricLocation;
        protected Vector3 HeliocentricLocation
        {
            private get
            {
                return _heliocentricLocation;
            }
            set
            {
                _heliocentricLocation = value;
            }
        }

        protected override void OnTimeChanged()
        {
            // Calculate geocentric coordinates
            Vector3 bodyLocation = this.HeliocentricLocation - Earth.Instance.HeliocentricLocation;
            EclipticCoordinate geocentricCoordinate = new EclipticCoordinate(bodyLocation);
            this.EquatorialCoordinate = ODE.Astronomy.EquatorialCoordinate.From(geocentricCoordinate, this.Time);

            base.OnTimeChanged();
        }
    }
}