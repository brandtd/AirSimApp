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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotSpatial.Positioning
{
    /// <summary>Represents a frame-local north/east/down vector.</summary>
    public struct NedPoint : IFormattable, IEquatable<NedPoint>
    {
        private Distance _d;
        private Distance _e;
        private Distance _n;

        #region Constructors

        /// <summary>Creates a new instance using the specified N, E, and D values.</summary>
        /// <param name="n">North component.</param>
        /// <param name="e">East component.</param>
        /// <param name="d">Down component.</param>
        public NedPoint(Distance n, Distance e, Distance d)
        {
            _n = n.ToMeters();
            _e = e.ToMeters();
            _d = d.ToMeters();
        }

        #endregion Constructors

        #region Fields

        /// <summary>Returns a cartesian coordinate with empty values.</summary>
        public static readonly NedPoint Empty = new NedPoint(Distance.Empty, Distance.Empty, Distance.Empty);

        /// <summary>Returns a cartesian point with infinite values.</summary>
        public static readonly NedPoint Infinity = new NedPoint(Distance.Infinity, Distance.Infinity, Distance.Infinity);

        /// <summary>Represents an invalid or unspecified value.</summary>
        public static readonly NedPoint Invalid = new NedPoint(Distance.Invalid, Distance.Invalid, Distance.Invalid);

        #endregion Fields

        #region Public Properties

        /// <summary>Returns the down component of the NED vector.</summary>
        public Distance D => _d;

        /// <summary>Returns the east component of the NED vector.</summary>
        public Distance E => _e;

        /// <summary>Indicates whether the current instance has no value.</summary>
        public bool IsEmpty => _n.IsEmpty && _e.IsEmpty && _d.IsEmpty;

        /// <summary>Indicates whether the current instance is invalid or unspecified.</summary>
        public bool IsInvalid => _n.IsInvalid && _e.IsInvalid && _d.IsInvalid;

        /// <summary>Returns the north component of the NED vector.</summary>
        public Distance N => _n;

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        ///     Returns the distance from the current instance to the specified NED point.
        /// </summary>
        /// <param name="point">
        ///     A <strong>NedPoint</strong> object representing the end of a segment.
        /// </param>
        /// <returns></returns>
        public Distance DistanceTo(NedPoint point)
        {
            return new Distance(
                Math.Sqrt(Math.Pow(point.N.Value - _n.Value, 2)
                        + Math.Pow(point.E.Value - _e.Value, 2)
                        + Math.Pow(point.D.Value - _d.Value, 2)),
                        DistanceUnit.Meters).ToLocalUnitType();
        }

        /// <summary>Converts the current instance to a geodetic (latitude/longitude) coordinate.</summary>
        /// <param name="reference">Origin of NED local-frame.</param>
        /// <returns>A <strong>Position</strong> object containing the converted result.</returns>
        /// <remarks>
        ///     The conversion formula will convert the Cartesian coordinate to latitude and
        ///     longitude using the WGS1984 ellipsoid (the default ellipsoid for GPS coordinates).
        /// </remarks>
        public Position3D ToPosition3D(Position3D reference)
        {
            return ToPosition3D(reference, Ellipsoid.Wgs1984);
        }

        /// <summary>
        ///     Converts the current instance to a geodetic (latitude/longitude) coordinate using the
        ///     specified ellipsoid.
        /// </summary>
        /// <param name="reference">Origin of NED local-frame.</param>
        /// <param name="ellipsoid">The ellipsoid.</param>
        /// <returns>A <strong>Position</strong> object containing the converted result.</returns>
        public Position3D ToPosition3D(Position3D reference, Ellipsoid ellipsoid)
        {
            if (ellipsoid == null)
                throw new ArgumentNullException("ellipsoid");

            CartesianPoint refEcef = reference.ToCartesianPoint(ellipsoid);

            double sLat = Math.Sin(reference.Latitude.ToRadians().Value);
            double sLon = Math.Sin(reference.Longitude.ToRadians().Value);
            double cLat = Math.Cos(reference.Latitude.ToRadians().Value);
            double cLon = Math.Cos(reference.Longitude.ToRadians().Value);

            double r11 = -sLat * cLon;
            double r21 = -sLat * sLon;
            double r31 = cLat;
            double r12 = -sLon;
            double r22 = cLon;
            double r32 = 0.0;
            double r13 = -cLat * cLon;
            double r23 = -cLat * sLon;
            double r33 = -sLat;

            double n1 = _n.Value;
            double n2 = _e.Value;
            double n3 = _d.Value;

            double v1 = r11 * n1 + r12 * n2 + r13 * n3;
            double v2 = r21 * n1 + r22 * n2 + r23 * n3;
            double v3 = r31 * n1 + r32 * n2 + r33 * n3;

            Distance _x = new Distance(v1, refEcef.X.Units) + refEcef.X;
            Distance _y = new Distance(v2, refEcef.Y.Units) + refEcef.Y;
            Distance _z = new Distance(v3, refEcef.Z.Units) + refEcef.Z;

            #region New code

            /*
             * % ECEF2LLA - convert earth-centered earth-fixed (ECEF)
%            cartesian coordinates to latitude, longitude,
%            and altitude
%
% USAGE:
% [lat, lon, alt] = ecef2lla(x, y, z)
%
% lat = geodetic latitude (radians)
% lon = longitude (radians)
% alt = height above WGS84 ellipsoid (m)
% x = ECEF X-coordinate (m)
% y = ECEF Y-coordinate (m)
% z = ECEF Z-coordinate (m)
%
% Notes: (1) This function assumes the WGS84 model.
%        (2) Latitude is customary geodetic (not geocentric).
%        (3) Inputs may be scalars, vectors, or matrices of the same
%            size and shape. Outputs will have that same size and shape.
%        (4) Tested but no warranty; use at your own risk.
%        (5) Michael Kleder, April 2006

function [lat, lon, alt] = ecef2lla(x, y, z)

% WGS84 ellipsoid constants:
a = 6378137;
e = 8.1819190842622e-2;

% calculations:
b   = sqrt(a^2*(1-e^2));
ep  = sqrt((a^2-b^2)/b^2);
p   = sqrt(x.^2+y.^2);
th  = atan2(a*z, b*p);
lon = atan2(y, x);
lat = atan2((z+ep^2.*b.*sin(th).^3), (p-e^2.*a.*cos(th).^3));
N   = a./sqrt(1-e^2.*sin(lat).^2);
alt = p./cos(lat)-N;

% return lon in range [0, 2*pi)
lon = mod(lon, 2*pi);

% correct for numerical instability in altitude near exact poles:
% (after this correction, error is about 2 millimeters, which is about
% the same as the numerical precision of the overall function)

k=abs(x)<1 & abs(y)<1;
alt(k) = abs(z(k))-b;

return
             */

            double x = _x.ToMeters().Value;
            double y = _y.ToMeters().Value;
            double z = _z.ToMeters().Value;

            //% WGS84 ellipsoid constants:
            // a = 6378137;

            double a = ellipsoid.EquatorialRadius.ToMeters().Value;

            // e = 8.1819190842622e-2;

            double e = ellipsoid.Eccentricity;

            //% calculations:
            // b   = sqrt(a^2*(1-e^2));

            double b = Math.Sqrt(Math.Pow(a, 2) * (1 - Math.Pow(e, 2)));

            // ep = sqrt((a^2-b^2)/b^2);

            double ep = Math.Sqrt((Math.Pow(a, 2) - Math.Pow(b, 2)) / Math.Pow(b, 2));

            // p = sqrt(x.^2+y.^2);

            double p = Math.Sqrt(Math.Pow(x, 2) + Math.Pow(y, 2));

            // th = atan2(a*z, b*p);

            double th = Math.Atan2(a * z, b * p);

            // lon = atan2(y, x);

            double lon = Math.Atan2(y, x);

            // lat = atan2((z+ep^2.*b.*sin(th).^3), (p-e^2.*a.*cos(th).^3));

            double lat = Math.Atan2((z + Math.Pow(ep, 2) * b * Math.Pow(Math.Sin(th), 3)), (p - Math.Pow(e, 2) * a * Math.Pow(Math.Cos(th), 3)));

            // N = a./sqrt(1-e^2.*sin(lat).^2);

            double n = a / Math.Sqrt(1 - Math.Pow(e, 2) * Math.Pow(Math.Sin(lat), 2));

            // alt = p./cos(lat)-N;

            double alt = p / Math.Cos(lat) - n;

            //% return lon in range [0, 2*pi)
            // lon = mod(lon, 2*pi);

            lon = lon % (2 * Math.PI);

            //% correct for numerical instability in altitude near exact poles:
            //% (after this correction, error is about 2 millimeters, which is about
            //% the same as the numerical precision of the overall function)

            // k=abs(x)<1 & abs(y)<1;

            bool k = Math.Abs(x) < 1.0 && Math.Abs(y) < 1.0;

            // alt(k) = abs(z(k))-b;

            if (k)
                alt = Math.Abs(z) - b;

            // return

            return new Position3D(
                    Distance.FromMeters(alt),
                    Latitude.FromRadians(lat),
                    Longitude.FromRadians(lon));

            #endregion New code
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <param name="format">The format.</param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(string format)
        {
            return ToString(format, CultureInfo.CurrentCulture);
        }

        #endregion Public Methods

        #region Overrides

        /// <summary>
        ///     Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">Another object to compare to.</param>
        /// <returns>
        ///     <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance;
        ///     otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is NedPoint)
                return Equals((NedPoint)obj);
            return false;
        }

        /// <summary>Returns a hash code for this instance.</summary>
        /// <returns>
        ///     A hash code for this instance, suitable for use in hashing algorithms and data
        ///     structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return _n.GetHashCode() ^ _e.GetHashCode() ^ _d.GetHashCode();
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return ToString("G", CultureInfo.CurrentCulture);
        }

        #endregion Overrides

        #region IFormattable Members

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <param name="format">
        ///     The format to use.-or- A null reference (Nothing in Visual Basic) to use the default
        ///     format defined for the type of the <see cref="T:System.IFormattable" /> implementation.
        /// </param>
        /// <param name="formatProvider">
        ///     The provider to use to format the value.-or- A null reference (Nothing in Visual
        ///     Basic) to obtain the numeric format information from the current locale setting of
        ///            the operating system.
        /// </param>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public string ToString(string format, IFormatProvider formatProvider)
        {
            CultureInfo culture = (CultureInfo)formatProvider ?? CultureInfo.CurrentCulture;

            if (string.IsNullOrEmpty(format))
                format = "G";

            return _n.ToString(format, culture) + culture.TextInfo.ListSeparator
                + _e.ToString(format, culture) + culture.TextInfo.ListSeparator
                + _d.ToString(format, culture);
        }

        #endregion IFormattable Members

        #region IEquatable<CartesianPoint> Members

        /// <summary>
        ///     Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns>
        ///     true if the current object is equal to the <paramref name="other" /> parameter;
        ///     otherwise, false.
        /// </returns>
        public bool Equals(NedPoint other)
        {
            return _n.Equals(other.N)
                && _e.Equals(other.E)
                && _d.Equals(other.D);
        }

        #endregion IEquatable<CartesianPoint> Members

        #region Operators

        /// <summary>Implements the operator -.</summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static NedPoint operator -(NedPoint a, NedPoint b)
        {
            return new NedPoint(a.N.Subtract(b.N), a.E.Subtract(b.E), a.D.Subtract(b.D));
        }

        /// <summary>Implements the operator *.</summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static NedPoint operator *(NedPoint a, NedPoint b)
        {
            return new NedPoint(a.N.Multiply(b.N), a.E.Multiply(b.E), a.D.Multiply(b.D));
        }

        /// <summary>Implements the operator /.</summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static NedPoint operator /(NedPoint a, NedPoint b)
        {
            return new NedPoint(a.N.Divide(b.N), a.E.Divide(b.E), a.D.Divide(b.D));
        }

        /// <summary>Implements the operator +.</summary>
        /// <param name="a">A.</param>
        /// <param name="b">The b.</param>
        /// <returns>The result of the operator.</returns>
        public static NedPoint operator +(NedPoint a, NedPoint b)
        {
            return new NedPoint(a.N.Add(b.N), a.E.Add(b.E), a.D.Add(b.D));
        }

        #endregion Operators
    }
}