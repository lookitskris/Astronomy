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
    public class HorizontalCoordinateProjection
    {
        private Vector3 horzAxis, vertAxis;

        private Vector3 _viewCenterVector;
        public Vector3 ViewCenterVector
        {
            get
            {
                return _viewCenterVector;
            }
            protected set
            {
                _viewCenterVector = value;
            }
        }

        public void SetViewCenter(HorizontalCoordinate viewCenterCoord)
        {
            ViewCenterVector = viewCenterCoord.ToVector();
            HorizontalCoordinate vertAxisCoord = new HorizontalCoordinate(viewCenterCoord.Azimuth + Angle.Right, Angle.Zero);
            vertAxis = vertAxisCoord.ToVector();
            horzAxis = Vector3.Cross(this.ViewCenterVector, vertAxis);
        }

        public void GetAngleOffsets(HorizontalCoordinate objectCoord, ref Angle horzAngle, ref Angle vertAngle)
        {
            Vector3 objectVector = objectCoord.ToVector();
            Vector3 horzObjectCross = Vector3.Cross(objectVector, -horzAxis);
            Vector3 vertObjectCross = Vector3.Cross(objectVector, vertAxis);

            horzObjectCross.Normalize();
            vertObjectCross.Normalize();

            double x = Vector3.Dot(horzObjectCross, vertAxis);
            double y = Vector3.Dot(horzObjectCross, this.ViewCenterVector);

            horzAngle = -Angle.ArcTangent(y, x);

            x = Vector3.Dot(vertObjectCross, horzAxis);
            y = Vector3.Dot(vertObjectCross, this.ViewCenterVector);

            vertAngle = -Angle.ArcTangent(y, x);
        }
    }
}
