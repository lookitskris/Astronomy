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
    public abstract class CelestialBody
    {
        // Only used internally
        private GeographicCoordinate Location { get; set; }

        // Set here, used by descendent classes
        private Time _time;
        protected Time Time
        {
            get
            {
                return _time;
            }
            private set
            {
                _time = value;
            }
        }

        // Set by descendent classes, used here
        private EquatorialCoordinate _equatorialCoordinate;
        protected EquatorialCoordinate EquatorialCoordinate
        {
            private get
            {
                return _equatorialCoordinate;
            }
            set
            {
                _equatorialCoordinate = value;
            }
        }

        // Set here, used external to library
        private HorizontalCoordinate _horizontalCoordinate;
        public HorizontalCoordinate HorizontalCoordinate
        {
            get
            {
                return _horizontalCoordinate;
            }
            private set
            {
                _horizontalCoordinate = value;
            }
        }

        // Used externally to retain screen location
        public float ScreenX;
        public float ScreenY;

        // Called external to update HorizontalCoordinate
        public void Update(Time time, GeographicCoordinate location)
        {
            bool needsUpdate = false;

            if (!this.Time.Equals(time))
            {
                this.Time = time;
                needsUpdate = true;
                OnTimeChanged();
            }

            if (!this.Location.Equals(location))
            {
                this.Location = location;
                needsUpdate = true;
            }

            if (needsUpdate)
                this.HorizontalCoordinate = HorizontalCoordinate.From(this.EquatorialCoordinate, this.Location, this.Time);
        }

        // Overridden by descendent classes to update EquatorialCoordinate
        protected virtual void OnTimeChanged()
        {
        }
    }
}
