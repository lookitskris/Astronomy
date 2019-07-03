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

namespace ODE.Astronomy
{
    public struct GeographicCoordinate
    {
        public GeographicCoordinate(Angle longitude, Angle latitude) : this()
        {
            Longitude = longitude;
            Latitude = latitude;
        }

        private Angle _longitude;
        public Angle Longitude
        {
            get
            {
                return _longitude;
            }
            private set
            {
                _longitude = value;
            }
        }

        private Angle _latitude;
        public Angle Latitude
        {
            get
            {
                return _latitude;
            }
            private set
            {
                _latitude = value;
            }
        }

        public static GeographicCoordinate NewYorkCity
        {
            get
            {
                return new GeographicCoordinate(Angle.FromDegrees(73.99), Angle.FromDegrees(40.73));
            }
        }

        // Subtraction operator returns distance along unit sphere.
        public static double operator -(GeographicCoordinate gc1, GeographicCoordinate gc2)
        {
            Angle angBetween = Angle.Zero;
            angBetween.Cosine = gc1.Latitude.Sine * gc2.Latitude.Sine + gc1.Latitude.Cosine * gc2.Latitude.Cosine * (gc1.Longitude - gc2.Longitude).Cosine;
            return angBetween.Radians;
        }

        // Operators and overrides
        public static bool operator ==(GeographicCoordinate geo1, GeographicCoordinate geo2)
        {
            return geo1.Equals(geo2);
        }

        public static bool operator !=(GeographicCoordinate geo1, GeographicCoordinate geo2)
        {
            return !geo1.Equals(geo2);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is GeographicCoordinate))
                return false;

            return Equals((Angle)obj);
        }

        public bool Equals(GeographicCoordinate that)
        {
            return this.Longitude.Equals(that.Longitude) && this.Latitude.Equals(that.Latitude);
        }

        public override int GetHashCode()
        {
            Angle longitude = this.Longitude;
            Angle latitude = this.Latitude;

            longitude.NormalizePositive();
            latitude.NormalizePositive();

            var result = 180 * longitude + latitude;

            return result.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("{0} longitude {1} latitude", Longitude, Latitude);
        }
    }
}
