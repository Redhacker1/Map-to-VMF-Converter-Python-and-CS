using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace MapConverter
{
    class HalfSpaceCalculator
    {
        Vector3? Intersection(Side Plane_A, Side Plane_B, Side Plane_C)
        {
            if (Plane_A.Equation.a == Plane_B.Equation.a || Plane_A.Equation.a == Plane_C.Equation.a || Plane_A.Equation.a == Plane_C.Equation.a ||
                Plane_A.Equation.b == Plane_B.Equation.b || Plane_A.Equation.b == Plane_C.Equation.b || Plane_A.Equation.b == Plane_C.Equation.b ||
                Plane_A.Equation.c == Plane_B.Equation.c || Plane_A.Equation.c == Plane_C.Equation.c || Plane_A.Equation.c == Plane_C.Equation.c)
            {
                Console.WriteLine("Bad Plane!");
                return null;
            }
            return Vector3.Zero;
        }
    }
}
