using System.Collections.Generic;
using System.Numerics;

namespace MapConverter
{
    struct Brush
    {
        Side[] Sides { get; set; }

        public Brush(Side[] sides)
        {
            Sides = sides;
        }
    }
    struct Side
    {
        public Side(string texture, Vector3[] points, float x_offset, float y_offset, float rot_angle, float y_scale, float x_scale, Vector3 u_axis, Vector3 v_axis, int id)
        {
            Texture = texture;
            this.Points = points;
            this.X_offset = x_offset;
            this.Y_offset = y_offset;
            this.Rot_angle = rot_angle;
            this.Y_scale = y_scale;
            this.X_scale = x_scale;
            this.U_axis = u_axis;
            this.V_axis = v_axis;
            this.Id = id;
        }

        string Texture { get; set; }
        public Vector3[] Points { get; set; }
        float X_offset { get; set; }
        float Y_offset { get; set; }
        float Rot_angle { get; set; }
        float Y_scale { get; set; }
        float X_scale { get; set; }
        Vector3 U_axis { get; set; }
        Vector3 V_axis { get; set; }
        int Id { get; set; }

    }
    class Point_Entity
    {
        public Point_Entity(Vector3 location, Dictionary<string, string> attributes, string classname)
        {
            Location = location;
            Attributes = attributes;
            Classname = classname;
        }

        public Vector3 Location { get; set; }
        public Dictionary<string,string> Attributes { get; set; }
        public string Classname { get; set; }
    }

    class BrushEntity : Point_Entity
    {
        public BrushEntity(Dictionary<string, string> attributes, string classname, Brush[] brushes) : base(new Vector3(0.0f, 0.0f, 0.0f ), attributes, classname)
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
