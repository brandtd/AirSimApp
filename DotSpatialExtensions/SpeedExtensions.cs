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

namespace DotSpatial.Positioning
{
    /// <summary>Extensions for <see cref="Speed" />.</summary>
    public static class SpeedExtensions
    {
        /// <summary>Get speed in given unit.</summary>
        public static double In(this Speed speed, SpeedUnit unit)
        {
            return speed.ToUnitType(unit).Value;
        }

        /// <summary>Get speed in ft/s.</summary>
        public static double InFeetPerSecond(this Speed speed)
        {
            return speed.ToFeetPerSecond().Value;
        }

        /// <summary>Get speed in km/hr.</summary>
        public static double InKilometersPerHour(this Speed speed)
        {
            return speed.ToKilometersPerHour().Value;
        }

        /// <summary>Get speed in km/s.</summary>
        public static double InKilometersPerSecond(this Speed speed)
        {
            return speed.ToKilometersPerSecond().Value;
        }

        /// <summary>Get speed in knots.</summary>
        public static double InKnots(this Speed speed)
        {
            return speed.ToKnots().Value;
        }

        /// <summary>Get speed in m/s.</summary>
        public static double InMetersPerSecond(this Speed speed)
        {
            return speed.ToMetersPerSecond().Value;
        }

        /// <summary>Get speed in mph.</summary>
        public static double InStatuteMilesPerHour(this Speed speed)
        {
            return speed.ToStatuteMilesPerHour().Value;
        }
    }
}