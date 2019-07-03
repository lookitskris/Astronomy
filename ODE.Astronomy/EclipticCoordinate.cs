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
using Microsoft.Xna.Framework;

namespace ODE.Astronomy
{
    public struct EclipticCoordinate
    {
        public EclipticCoordinate(Angle longitude, Angle latitude, double radius) : this()
        {
           Longitude = longitude;
           Latitude = latitude;
           Radius = radius;
        }

        public EclipticCoordinate(Vector3 point) : this()
        {
            RectangularCoordinates = point;
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

        private double _radius;
        public double Radius
        {
            get
            {
                return _radius;
            }
            private set
            {
                _radius = value;
            }
        }

        public Vector3 RectangularCoordinates
        {
            set
            {
                this.Radius = Math.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);

                this.Longitude = Angle.ArcTangent(-value.Z, value.X);
                this.Latitude = Angle.ArcTangent(value.Y, Math.Sqrt(value.X * value.X + value.Z * value.Z));
            }
            get
            {
                double x = Radius * Latitude.Cosine * Longitude.Cosine;
                double y = Radius * Latitude.Sine;
                double z = -Radius * Latitude.Cosine * Longitude.Sine;

                return new Vector3(System.Convert.ToSingle(x), System.Convert.ToSingle(y), System.Convert.ToSingle(z));
            }
        }

        public override string ToString()
        {
            return string.Format("({0}, {1}, {2})", this.Longitude, this.Latitude, this.Radius);
        }
    }
}
