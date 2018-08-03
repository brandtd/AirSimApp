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
    /// <summary>Extensions for <see cref="Distance" />.</summary>
    public static class DistanceExtensions
    {
        /// <summary>Get distance in given unit.</summary>
        public static double In(this Distance distance, DistanceUnit unit)
        {
            return distance.ToUnitType(unit).Value;
        }

        /// <summary>Get distance in centimeters.</summary>
        public static double InCentimeters(this Distance distance)
        {
            return distance.ToCentimeters().Value;
        }

        /// <summary>Get distance in feet.</summary>
        public static double InFeet(this Distance distance)
        {
            return distance.ToFeet().Value;
        }

        /// <summary>Get distance in inches.</summary>
        public static double InInches(this Distance distance)
        {
            return distance.ToInches().Value;
        }

        /// <summary>Get distance in kilometers.</summary>
        public static double InKilometers(this Distance distance)
        {
            return distance.ToKilometers().Value;
        }

        /// <summary>Get distance in meters.</summary>
        public static double InMeters(this Distance distance)
        {
            return distance.ToMeters().Value;
        }

        /// <summary>Get distance in nautical miles.</summary>
        public static double InNauticalMiles(this Distance distance)
        {
            return distance.ToNauticalMiles().Value;
        }

        /// <summary>Get distance in statute miles.</summary>
        public static double InStatuteMiles(this Distance distance)
        {
            return distance.ToStatuteMiles().Value;
        }
    }
}