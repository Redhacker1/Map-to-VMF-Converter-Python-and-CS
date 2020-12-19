using System;
using System.Collections.Generic;
using System.Numerics;

namespace MapConverter
{
    struct Brush
    {
        public Side[] Sides { get; set; }
        public int ID { get; set; }
        
        public Brush(Side[] sides, int id)
        {
            Sides = sides;
            ID = id;
        }
    }
    struct Side
    {
        public Side(string texture, Vector3[] points, float x_offset, float y_offset, float rot_angle, float y_scale, float x_scale, int id)
        {
            Texture = texture;
            Points = points;
            X_offset = x_offset;
            Y_offset = y_offset;
            Rot_angle = rot_angle;
            Y_scale = y_scale;
            X_scale = x_scale;
            Id = id;
            
            U_axis = new Vector3(0, 0, 0);
            V_axis = new Vector3(0, 0, 0);
            Normal = MathLib.CalulateNormal_UnitVector(points[0], points[1], points[2]);
            Normal_Unnormalized = MathLib.CalculateNormal(points[0], points[1], points[2]);
            Equation = new Plane_equation();
            
            // Distance from origin
            Equation.d = (-1 * (Normal.X * points[0].X)) - (Normal.Y * points[0].Y) - (Normal.Z * points[0].Z);

            Equation.a = Normal_Unnormalized.X;
            Equation.b = Normal_Unnormalized.Y;
            Equation.c = Normal_Unnormalized.Z;


            Console.WriteLine(Equation.d/(Math.Sqrt(Math.Pow(Normal.X, 2) + Math.Pow(Normal.Y, 2) + Math.Pow(Normal.Z, 2))));
        }

        public string Texture { get; set; }
        public Vector3[] Points { get; set; }
        public float X_offset { get; set; }
        public float Y_offset { get; set; }
        public float Rot_angle { get; set; }
        public float Y_scale { get; set; }
        public float X_scale { get; set; }
        public Vector3 U_axis { get; set; }
        public Vector3 V_axis { get; set; }
        public Vector3 Normal { get; set; }
        public Vector3 Normal_Unnormalized { get; set; }
        public int Id { get; set; }

        public Plane_equation Equation { get; set; }

        public class Plane_equation
        {
            // ax + by + cz + d = 0

            public float a;
            public float b;
            public float c;
            public float d;
        };


    }
    class Point_Entity
    {
        public Point_Entity(Vector3 location, Dictionary<string, string> attributes, string classname, bool isbrushentity = false)
        {
            Location = location;
            Attributes = attributes;
            Classname = classname;
            IsBrushEntity = isbrushentity;
        }

        public bool IsBrushEntity { get; set; }
        public Vector3 Location { get; set; }
        public Dictionary<string,string> Attributes { get; set; }
        public string Classname { get; set; }
    }

    class BrushEntity : Point_Entity
    {
        public BrushEntity(Dictionary<string, string> attributes, string classname, Brush[] brushes) : base(new Vector3(0.0f, 0.0f, 0.0f ), attributes, classname, true)
        {
            Brushes = brushes;
        }
        public Brush[] Brushes { get; set; }
    }

    struct Map
    {
        public Map(Point_Entity[] pentities, BrushEntity[] bentities)
        {
            PEntities = pentities;
            BEntities = bentities;
        }

        public Point_Entity[] PEntities { get; set; }
        public BrushEntity[] BEntities { get; set; }

    }
}
