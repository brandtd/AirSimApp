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

using System;

namespace DotSpatial.Positioning
{
    /// <summary>Extensions for <see cref="CartesianPoint" />.</summary>
    public static class CartesianPointExtensions
    {
        /// <summary>Converts an ECEF point to a NED point, relative to given reference point.</summary>
        public static NedPoint ToNedPoint(this CartesianPoint ecef, Position3D reference)
        {
            return ecef.ToNedPoint(reference, Ellipsoid.Wgs1984);
        }

        /// <summary>Converts an ECEF point to a NED point, relative to given reference point.</summary>
        public static NedPoint ToNedPoint(this CartesianPoint ecef, Position3D reference, Ellipsoid ellipsoid)
        {
            CartesianPoint refEcef = reference.ToCartesianPoint(ellipsoid);

            double sLat = Math.Sin(reference.Latitude.ToRadians().Value);
            double sLon = Math.Sin(reference.Longitude.ToRadians().Value);
            double cLat = Math.Cos(reference.Latitude.ToRadians().Value);
            double cLon = Math.Cos(reference.Longitude.ToRadians().Value);

            double r11 = -sLat * cLon;
            double r12 = -sLat * sLon;
            double r13 = cLat;
            double r21 = -sLon;
            double r22 = cLon;
            double r23 = 0.0;
            double r31 = -cLat * cLon;
            double r32 = -cLat * sLon;
            double r33 = -sLat;

            double v1 = ecef.X.Value - refEcef.X.Value;
            double v2 = ecef.Y.Value - refEcef.Y.Value;
            double v3 = ecef.Z.Value - refEcef.Z.Value;

            double n1 = r11 * v1 + r12 * v2 + r13 * v3;
            double n2 = r21 * v1 + r22 * v2 + r23 * v3;
            double n3 = r31 * v1 + r32 * v2 + r33 * v3;

            Distance n = new Distance(n1, refEcef.X.Units);
            Distance e = new Distance(n2, refEcef.Y.Units);
            Distance d = new Distance(n3, refEcef.Z.Units);

            return new NedPoint(n, e, d);
        }
    }
}