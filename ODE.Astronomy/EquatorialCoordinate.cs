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
    public struct EquatorialCoordinate
    {
        public EquatorialCoordinate(Angle rightAscension, Angle declination) : this()
        {
            RightAscension = rightAscension;
            Declination = declination;
        }

        private Angle _rightAscension;
        public Angle RightAscension
        {
            get
            {
                return _rightAscension;
            }
            private set
            {
                _rightAscension = value;
            }
        }

        private Angle _declination;
        public Angle Declination
        {
            get
            {
                return _declination;
            }
            private set
            {
                _declination = value;
            }
        }

        // Time not used in current calculation, but might be need in future refinements
        public static EquatorialCoordinate From(EclipticCoordinate eclipticCoordinate, Time time)
        {
            // Calculate right ascension and declination
            Angle angleEpsilon = Angle.FromDegrees(23.4392911); // obliquity of the ecliptic -- see Meeus, pg. 92

            Angle angleRightAscension = Angle.ArcTangent(eclipticCoordinate.Longitude.Sine * angleEpsilon.Cosine - eclipticCoordinate.Latitude.Tangent * angleEpsilon.Sine, eclipticCoordinate.Longitude.Cosine);

            angleRightAscension.NormalizePositive();

            Angle angleDeclination = Angle.Zero;

            angleDeclination.Sine = eclipticCoordinate.Latitude.Sine * angleEpsilon.Cosine + eclipticCoordinate.Latitude.Cosine * angleEpsilon.Sine * eclipticCoordinate.Longitude.Sine;

            angleDeclination.NormalizeAroundZero();

            return new EquatorialCoordinate(angleRightAscension, angleDeclination);
        }
    }
}
