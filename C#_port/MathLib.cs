using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;

namespace MapConverter
{
    static class MathLib
    {
        static public Vector3 CalculateNormal(Vector3 vector1, Vector3 vector2, Vector3 vector3)
        {
            return Vector3.Cross(vector2 - vector1, vector3 - vector1);
        }

        static public Vector3 CalulateNormal_UnitVector(Vector3 vector1, Vector3 vector2, Vector3 vector3)
        {
            return Vector3.Normalize(CalculateNormal(vector1, vector2, vector3));
        }

        static public double GetScalarValue(Vector3 Vector, Vector3 NormalVector)
        {
            return (NormalVector.X * Vector.X) + (NormalVector.Y * Vector.Y) + (NormalVector.Z * Vector.Z) - ShortestSignedDistance(NormalVector, Vector);
        }

        static public double ShortestSignedDistance(Vector3 Normal, Vector3 Point0)
        {
            return -(Point0.X * Normal.X + Point0.Z * Normal.Y + Point0.Z * Normal.Z);
        }

        static public double DistanceFromOrigin(Vector3 Normal, Vector3 point)
        {
            return Vector3.Dot(Normal, point);
        }
        static public Vector3 ThreeNumberstringsToVector3(string x, string y, string z)
        {
            return new Vector3(Convert.ToInt32(x.Trim()), Convert.ToInt32(y.Trim()), Convert.ToInt32(z.Trim()));
        }
        static public Vector3 ThreeNumberstringsToVector3(string SpacedXYZ, string Splitter)
        {
            string[] XYZArray = SpacedXYZ.Split(Splitter);
            return new Vector3(Convert.ToInt32(XYZArray[0].Trim()), Convert.ToInt32(XYZArray[1].Trim()), Convert.ToInt32(XYZArray[2].Trim()));
        }
        static public Vector3 ThreeNumberstringsToVector3(string SpacedXYZ, char Splitter)
        {
            string[] XYZArray = SpacedXYZ.Trim().Split(Splitter);
            return new Vector3(Convert.ToInt32(XYZArray[0].Trim()), Convert.ToInt32(XYZArray[1].Trim()), Convert.ToInt32(XYZArray[2].Trim()));
        }

        static public Matrix4x4 RotateMatrix(Vector3 origin, Vector3 Rotation, Matrix4x4 Matrix = new Matrix4x4())
        {
            var Rotation_1 = Matrix4x4.CreateRotationX(Rotation.X, origin);
            var Rotation_2 = Matrix4x4.CreateRotationY(Rotation.Y, origin);
            var Rotation_3 = Matrix4x4.CreateRotationZ(Rotation.Z, origin);

            var Rotation_Final = Rotation_1 * Rotation_2 * Rotation_3;

            Matrix = Matrix * Rotation_Final;

            return Matrix;
        }

    }
}
