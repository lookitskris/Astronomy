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
    public struct HorizontalCoordinate
    {
        public HorizontalCoordinate(Angle azimuth, Angle altitude, Angle tilt) : this()
        {
            Azimuth = azimuth;
            Altitude = altitude;
            Tilt = tilt;
        }

        public HorizontalCoordinate(Angle azimuth, Angle altitude) : this(azimuth, altitude, Angle.Zero)
        {
        }

        // Eastward from north
        private Angle _azimuth;
        public Angle Azimuth
        {
            get
            {
                return _azimuth;
            }
            private set
            {
                _azimuth = value;
            }
        }

        private Angle _altitude;
        public Angle Altitude
        {
            get
            {
                return _altitude;
            }
            private set
            {
                _altitude = value;
            }
        }

        private Angle _tilt;
        public Angle Tilt
        {
            get
            {
                return _tilt;
            }
            private set
            {
                _tilt = value;
            }
        }

        public static HorizontalCoordinate From(Vector3 vector)
        {
            Angle altitude = Angle.Zero;
            altitude.Sine = vector.Z;

            // Make X and Y components negative for zero azimuth at south
            // with westward increasing values.
            // Then change x and y calculations likewise in ToVector method
            // and remove adjustment in From(EquatorialCoordinates) method.
            Angle azimuth = Angle.ArcTangent(vector.X, vector.Y);

            return new HorizontalCoordinate(azimuth, altitude);
        }

        public static HorizontalCoordinate From(Matrix matrix)
        {
            // Invert the matrix 
            matrix = Matrix.Invert(matrix);

            // Transform (0, 0, -1) -- the vector extending from the lens
            Vector3 zAxisTransformed = Vector3.Transform(-Vector3.UnitZ, matrix);

            // Get the horizontal coordinates
            HorizontalCoordinate horzCoord = From(zAxisTransformed);

            // Find the theoretical HorizontalCoordinate for the transformed +Y vector if the phone is upright
            Angle yUprightAltitude = Angle.Zero;
            Angle yUprightAzimuth = Angle.Zero;

            if (horzCoord.Altitude.Degrees > 0)
            {
                yUprightAltitude = Angle.Right - horzCoord.Altitude;
                yUprightAzimuth = Angle.Straight + horzCoord.Azimuth;
            }
            else
            {
                yUprightAltitude = Angle.Right + horzCoord.Altitude;
                yUprightAzimuth = horzCoord.Azimuth;
            }
            Vector3 yUprightVector = new HorizontalCoordinate(yUprightAzimuth, yUprightAltitude).ToVector();

            // Find the real transformed +Y vector
            Vector3 yAxisTransformed = Vector3.Transform(Vector3.UnitY, matrix);

            // Get the angle between the upright +Y vector and the real transformed +Y vector
            double dotProduct = Vector3.Dot(yUprightVector, yAxisTransformed);
            Vector3 crossProduct = Vector3.Cross(yUprightVector, yAxisTransformed);
            crossProduct.Normalize();

            // Sometimes dotProduct is slightly greater than 1, which 
            // raises an exception in the angleBetween calculation, so....
            dotProduct = Math.Min(dotProduct, 1);

            Angle angleBetween = Angle.FromRadians(Vector3.Dot(zAxisTransformed, crossProduct) * Math.Acos(dotProduct));
            horzCoord.Tilt = angleBetween;

            return horzCoord;
        }

        public static HorizontalCoordinate From(EquatorialCoordinate equatorialCoordinate, GeographicCoordinate geographicCoordinate, Time time)
        {
            // Calculate hour angle
            Angle localHourAngle = time.GreenwichSiderealTime - geographicCoordinate.Longitude - equatorialCoordinate.RightAscension;

            // Calculate azimuth
            Angle azimuth = Angle.ArcTangent(localHourAngle.Sine, localHourAngle.Cosine * geographicCoordinate.Latitude.Sine - equatorialCoordinate.Declination.Tangent * geographicCoordinate.Latitude.Cosine);

            // Adjustment for azimuth eastward from north
            azimuth += Angle.Straight;

            azimuth.NormalizeAroundZero();

            // Calculate altitude
            Angle altitude = Angle.Zero;
            altitude.Sine = geographicCoordinate.Latitude.Sine * equatorialCoordinate.Declination.Sine + geographicCoordinate.Latitude.Cosine * equatorialCoordinate.Declination.Cosine * localHourAngle.Cosine;
            altitude.NormalizeAroundZero();

            return new HorizontalCoordinate(azimuth, altitude);
        }

        public Vector3 ToVector()
        {
            double x = this.Altitude.Cosine * this.Azimuth.Sine;
            double y = this.Altitude.Cosine * this.Azimuth.Cosine;
            double z = this.Altitude.Sine;

            return new Vector3(System.Convert.ToSingle(x), System.Convert.ToSingle(y), System.Convert.ToSingle(z));
        }

        public override string ToString()
        {
            return string.Format("Azi: {0} Alt: {1} Tilt: {2}", this.Azimuth, this.Altitude, this.Tilt);
        }
    }
}
