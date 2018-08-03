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
    /// <summary>Provides a canonical modulo operator.</summary>
    public static class CanonicalModulo
    {
        /// <summary>
        ///     Computes the canonical modulo (the one where -1 % 3 = 2 instead of C#'s version where
        ///     -1 % 3 = -1).
        /// </summary>
        /// <remarks>
        ///     The canonical modulo operator creates infinitely repeating patterns, while C#'s
        ///     operator will create a pattern that is mirrored around the origin. That's frustrating
        ///     for certain things, like angles where -10 degrees is equivalent to 350 degrees.
        /// </remarks>
        public static double Compute(double dividend, double divisor)
        {
            double temp = dividend % divisor;
            return temp < 0 ? temp + divisor : temp;
        }
    }
}