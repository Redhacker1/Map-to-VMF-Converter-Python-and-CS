using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using MapConverter_Shared;

namespace MapConverter
{
    class Valve_Specific
    {
        public static void ConvertToValveTexFormat(ref Side Side)
        {
            Vector3[] p = Side.Points;
            var subtract1 = p[1] - p[0];
            var subtract2 = p[2] - p[0];
            Vector4 U = new Vector4(subtract1.X, subtract1.Y, subtract1.Z, Side.X_offset);
            Vector4 V = new Vector4(subtract2.X, subtract2.Y, subtract2.Z, Side.Y_offset);

            var angle_x = Math.Acos(Vector3.Dot(new Vector3(0, 0, 1), new Vector3(Side.Normal.X, 0, Side.Normal.Z)) / new Vector3(Side.Normal.X, 0, Side.Normal.Z).Length());
            var angle_y = Math.Acos(Vector3.Dot(new Vector3(0, 0, 1), new Vector3(0, Side.Normal.Y, Side.Normal.Z)) / new Vector3(0, Side.Normal.Y, Side.Normal.Z).Length());

            // Correct our angles
            if (Side.Normal.X < 0)
            {
                angle_x *= -1;
            }
            if (Side.Normal.Y < 0)
            {
                angle_y *= -1;
            }

            // Maybe
            Matrix4x4 rotation = Matrix4x4.CreateRotationY((float)angle_x) * Matrix4x4.CreateRotationX((float)angle_y);
            Vector3 UTransformedVec3 = Vector3.Transform(new Vector3(U.X, U.Y, U.Z), rotation);
            Vector3 VTransformedVec3 = Vector3.Transform(new Vector3(V.X, V.Y, V.Z), rotation);

            // Move the Vector3 values back into Vector4
            U.X = UTransformedVec3.X;
            U.Y = UTransformedVec3.Y;
            U.Z = UTransformedVec3.Z;

            V.X = VTransformedVec3.X;
            V.Y = VTransformedVec3.Y;
            V.Z = VTransformedVec3.Z;

            // Do some math
            U.X = (float)(U.X * Math.Cos(Side.Rot_angle) - U.Y * Math.Sin(Side.Rot_angle));

            U.Y = (float)(U.Y * Math.Cos(Side.Rot_angle) + U.X * Math.Sin(Side.Rot_angle));

            V.X = (float)(V.X * Math.Cos(Side.Rot_angle) - V.Y * Math.Sin(Side.Rot_angle));

            V.Y = (float)(V.Y * Math.Cos(Side.Rot_angle) + V.X * Math.Sin(Side.Rot_angle));

            var Inverse_Mat = Matrix4x4.CreateRotationY((float)-angle_x) * Matrix4x4.CreateRotationX((float)-angle_y);

            UTransformedVec3 = Vector3.Transform(new Vector3(U.X, U.Y, U.Z), Inverse_Mat);
            VTransformedVec3 = Vector3.Transform(new Vector3(V.X, V.Y, V.Z), Inverse_Mat);

            // Move the Vector3 values back into Vector4
            U.X = UTransformedVec3.X;
            U.Y = UTransformedVec3.Y;
            U.Z = UTransformedVec3.Z;

            V.X = VTransformedVec3.X;
            V.Y = VTransformedVec3.Y;
            V.Z = VTransformedVec3.Z;

            Side.U_axis = Vector3.Normalize(UTransformedVec3);
            Side.V_axis = Vector3.Normalize(VTransformedVec3);
        }
    }
}
