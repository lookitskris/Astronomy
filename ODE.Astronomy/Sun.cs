using System.Windows.Media;
using Microsoft.Xna.Framework;

namespace ODE.Astronomy
{
    public class Sun : SolarSystemBody
    {
        public Sun() : base("Sun", Colors.White)
        {
            this.HeliocentricLocation = Vector3.Zero;
        }
    }
}