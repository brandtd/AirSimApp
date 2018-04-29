#region MIT License (c) 2018 Dan Brandt

// Copyright 2018 Dan Brandt
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and
// associated documentation files (the "Software"), to deal in the Software without restriction,
// including without limitation the rights to use, copy, modify, merge, publish, distribute,
// sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or
// substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT
// NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT
// OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion MIT License (c) 2018 Dan Brandt

using DotSpatial.Positioning;
using System;

namespace DotSpatialExtensions
{
    /// <summary>Extensions for <see cref="CartesianPoint" />.</summary>
    public static class CartesianPointExtensions
    {
        /// <summary>Converts an ECEF point to a NED point, relative to given reference point.</summary>
        public static NedPoint ToNedPoint(this CartesianPoint ecef, Position3D reference)
        {
            CartesianPoint refEcef = reference.ToCartesianPoint();

            double phi = Math.Atan2(refEcef.Z.Value, Math.Sqrt(refEcef.X.Value * refEcef.X.Value + refEcef.Y.Value + refEcef.Y.Value));
            double sPhi = Math.Sin(phi);
            double sLon = Math.Sin(reference.Longitude.ToRadians().Value);
            double cPhi = Math.Cos(phi);
            double cLon = Math.Cos(reference.Longitude.ToRadians().Value);

            double r11 = -sPhi * cLon;
            double r12 = -sPhi * sLon;
            double r13 = cPhi;
            double r21 = -sLon;
            double r22 = cLon;
            double r23 = 0.0;
            double r31 = cPhi * cLon;
            double r32 = cPhi * sLon;
            double r33 = sPhi;

            double v1 = ecef.X.Value - refEcef.X.Value;
            double v2 = ecef.Y.Value - refEcef.Y.Value;
            double v3 = ecef.Z.Value - refEcef.Z.Value;

            double n1 = r11 * v1 + r12 * v2 + r13 * v3;
            double n2 = r21 * v1 + r22 * v2 + r23 * v3;
            double n3 = r31 * v1 + r32 * v2 + r33 * v3;

            Distance n = new Distance(n1, refEcef.X.Units);
            Distance e = new Distance(n2, refEcef.Y.Units);
            Distance d = new Distance(-n3, refEcef.Z.Units);

            return new NedPoint(n, e, d);
        }
    }
}