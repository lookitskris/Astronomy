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

namespace ODE.Astronomy
{
    public class Earth : Planet
    {
        static Earth()
        {
            Instance = new Earth("ear");
        }

        protected Earth(string strPlanetAbbreviation) : base(strPlanetAbbreviation)
        {
            this.Name = "Earth";
        }

        private static Earth _instance;
        public static Earth Instance
        {
            get
            {
                return _instance;
            }
            protected set
            {
                _instance = value;
            }
        }
    }
}