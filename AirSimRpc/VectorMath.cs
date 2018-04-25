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
using System.Numerics;

namespace AirSimRpc
{
    /// <summary>
    ///     Contains vector math lifted directly from the VectorMath.hpp file in AirSimLib. Would
    ///     love to replace this with a GIS library.
    /// </summary>
    public static class VectorMath
    {
        public static QuaternionR FlipZAxis(QuaternionR quat)
        {
            return new QuaternionR { X = -quat.X, Y = -quat.Y, Z = quat.Z, W = quat.W };
        }

        public static float Magnitude(Vector3R vector)
        {
            Vector3 v = new Vector3(vector.X, vector.Y, vector.Z);
            return v.Length();
        }

        public static QuaternionR Negate(QuaternionR quat)
        {
            return new QuaternionR { X = -quat.X, Y = -quat.Y, Z = -quat.Z, W = -quat.W };
        }

        public static Vector3R Rotate(Vector3R vector, QuaternionR quat, bool assumeUnitQuat)
        {
            Vector3 v = new Vector3(vector.X, vector.Y, vector.Z);
            Quaternion q = new Quaternion(quat.X, quat.Y, quat.Z, assumeUnitQuat ? 1 : quat.W);
            Vector3 result = Vector3.Transform(v, q);

            return new Vector3R() { X = result.X, Y = result.Y, Z = result.Z };
        }

        public static Vector3R RotateReverse(Vector3R vector, QuaternionR quat, bool assumeUnitQuat)
        {
            Vector3 v = new Vector3(vector.X, vector.Y, vector.Z);
            Quaternion q = new Quaternion(quat.X, quat.Y, quat.Z, assumeUnitQuat ? 1 : quat.W);
            Vector3 result = Vector3.Transform(v, Quaternion.Conjugate(q));

            return new Vector3R { X = result.X, Y = result.Y, Z = result.Z };
        }

        public static void ToEulerAngles(QuaternionR q, out double roll, out double pitch, out double yaw)
        {
            double ysqr = q.Y * q.Y;

            double t0 = 2.0 * (q.W * q.X + q.Y * q.Z);
            double t1 = 1.0 - 2.0 * (q.X * q.X + ysqr);
            roll = Math.Atan2(t0, t1);

            double t2 = 2.0 * (q.W * q.Y - q.Z * q.X);
            if (t2 > 1.0) { t2 = 1.0; }
            if (t2 < -1.0) { t2 = -1.0; }
            pitch = Math.Asin(t2);

            double t3 = 2.0 * (q.W * q.Z + q.X * q.Y);
            double t4 = 1.0 - 2.0 * (ysqr + q.Z * q.Z);
            yaw = Math.Atan2(t3, t4);
        }

        public static Vector3R TransformToBodyFrame(Vector3R vWorld, QuaternionR quat, bool assumeUnitQuat = true)
        {
            return RotateReverse(vWorld, quat, assumeUnitQuat);
        }

        public static Vector3R TransformToWorldFrame(Vector3R vBody, QuaternionR quat, bool assumeUnitQuat = true)
        {
            return Rotate(vBody, quat, assumeUnitQuat);
        }

        public static Vector3R TransformToWorldFrame(Vector3R vBody, Pose pose, bool assumeUnitQuat = true)
        {
            Vector3R translated = new Vector3R
            {
                X = vBody.X + pose.Position.X,
                Y = vBody.Y + pose.Position.Y,
                Z = vBody.Z + pose.Position.Z,
            };
            return TransformToWorldFrame(translated, pose.Orientation, assumeUnitQuat);
        }
    }
}