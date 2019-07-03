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
    public struct Time
    {
        private DateTime _universalTime;

        // Cached values
        private DateTime _dynamicalTime;
        private double _julianEphemerisDay;
        private Angle _greenwichSiderealTime;

        // Static creation methods
        public static Time GetNow()
        {
            return CreateTime(DateTime.UtcNow);
        }

        public static Time FromLocalTime(DateTime datetime)
        {
            return CreateTime(datetime.ToUniversalTime());
        }

        public static Time FromUniversalTime(DateTime datetime)
        {
            return CreateTime(datetime);
        }

        // There's obviously some number crunching involved when a Time value is first created,
        // but it's better to do it now instead of for numerous celestial objects.
        public static Time CreateTime(DateTime universalTime)
        {
            Time time = new Time();
            time._universalTime = universalTime;

            // Calculate dynamical time = univesal time + delta T
            // from Meeus, pg 78 for 2000 through 2100
            double century = (universalTime.Year - 2000) / (double)100;
            double dt = 102 + 102 * century + 25.3 * century * century;
            dt += 0.37 * (universalTime.Year - 2100);
            TimeSpan deltat = TimeSpan.FromSeconds(dt);
            time._dynamicalTime = universalTime.Add(deltat);

            // Calculate Julian Ephemeris Day 
            // Meeus, pgs 60-61.
            int Y = time._dynamicalTime.Year;
            int M = time._dynamicalTime.Month;
            double D = time._dynamicalTime.Day + (time._dynamicalTime.Hour + (time._dynamicalTime.Minute + (time._dynamicalTime.Second + time._dynamicalTime.Millisecond / 1000.0) / 60.0) / 60.0) / 24.0;
            if (M == 1 || M == 2)
            {
                Y -= 1;
                M += 12;
            }

            int A = Y / 100;
            int B = 2 - A + A / 4;

            time._julianEphemerisDay = Math.Floor(365.25 * (Y + 4716)) + Math.Floor(30.6001 * (M + 1)) + D + B - 1524.5;

            // Calculate Greenwich Sideral Time
            // Meeus, pg 87
            double t = 10 * time.Tau;

            time._greenwichSiderealTime = Angle.FromDegrees(280.46061837 + 360.98564736629 * (time._julianEphemerisDay - 2451545.0) + 0.000387933 * t * t - t * t * t / 38710000);

            time._greenwichSiderealTime.NormalizePositive();

            return time;
        }

        // Read-only properties
        public DateTime LocalTime
        {
            get
            {
                return _universalTime.ToLocalTime();
            }
        }

        public DateTime UniversalTime
        {
            get
            {
                return _universalTime;
            }
        }

        public DateTime DynamicalTime
        {
            get
            {
                return _dynamicalTime;
            }
        }

        public double Tau
        {
            get
            {
                return (JulianEphemerisDay - 2451545.0) / 365250;
            }
        }

        public double JulianEphemerisDay
        {
            get
            {
                return _julianEphemerisDay;
            }
        }

        public Angle GreenwichSiderealTime
        {
            get
            {
                return _greenwichSiderealTime;
            }
        }

        // Operators and overrides
        public static bool operator ==(Time time1, Time time2)
        {
            return time1._universalTime == time2._universalTime;
        }

        public static bool operator !=(Time time1, Time time2)
        {
            return time1._universalTime != time2._universalTime;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Time))
                return false;

            return Equals((Time)obj);
        }

        public bool Equals(Time that)
        {
            return this._universalTime.Equals(that._universalTime);
        }

        public override int GetHashCode()
        {
            return _universalTime.GetHashCode();
        }
    }
}
