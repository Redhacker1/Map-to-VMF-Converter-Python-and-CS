using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.IO;
using General_libs;
using System.Numerics;

namespace MapConverter
{
    static class Map_File_lib
    {
        static Side ConvertSide(Node SideNode)
        {
            string Texture = SideNode.GetNodebyName("Texture").Keyvalue;
            string X_Offset = SideNode.GetNodebyName("X_offset").Keyvalue;
            string Y_Offset = SideNode.GetNodebyName("Y_offset").Keyvalue;
            string X_Scale = SideNode.GetNodebyName("X_scale").Keyvalue;
            string Y_Scale = SideNode.GetNodebyName("Y_scale").Keyvalue;
            string Rot_Angle = SideNode.GetNodebyName("Rot_angle").Keyvalue;
            string ID = SideNode.GetNodebyName("ID").Keyvalue;

            Node[] pointnodes = SideNode.GetNodesWithStringInName("PointLocation", false);
            Vector3[] FaceCoords = new Vector3[3];
            int Index = 0;
            foreach (Node pointnode in pointnodes)
            {
                FaceCoords[Index] = MathLib.ThreeNumberstringsToVector3(pointnode.Keyvalue, ' ');
                Index++;
            }

            return new Side(Texture, FaceCoords, Convert.ToSingle(X_Offset), Convert.ToSingle(Y_Offset), Convert.ToSingle(Rot_Angle), Convert.ToSingle(Y_Scale), Convert.ToSingle(X_Scale), Convert.ToInt32(ID));
        }

        static Brush ConvertBrush(Node BrushNode)
        {
            var CurrentBrush = new Brush
            {
                ID = Convert.ToInt32(BrushNode.Keyvalue)
            };
            List<Side> Sides = new List<Side>();
            foreach (Node SideNode in BrushNode.GetNodesWithStringInName("Side", false))
            {

                Sides.Add(ConvertSide(SideNode));

            }
            CurrentBrush.Sides = Sides.ToArray();
            return CurrentBrush;
        }

        static BrushEntity ConvertBrushEntity(Node Entity, Dictionary<string, string> Attributes_dictionary)
        {
            Node[] Brushes = Entity.GetNodesByName("Brush");
            List<Brush> Brush_list = new List<Brush>();
            foreach (Node BrushNode in Brushes)
            {
                Brush_list.Add(ConvertBrush(BrushNode));
            }
            string Classname = Attributes_dictionary["classname"];
            Attributes_dictionary.Remove("classname");
            return new BrushEntity(Attributes_dictionary, Classname, Brush_list.ToArray());
        }

        public static Map ConvertMap(Node ParseTree)
        {
            var WorldStruct = new Map();
            var PEntities = new List<Point_Entity>();
            var BEntities = new List<BrushEntity>();


            foreach (Node Entity in ParseTree.Children)
            {
                bool isBrushEntity = CheckIsBrushEntity(Entity);

                // Get Attributes!
                var Attributes_dictionary = GetAttributes(Entity);
                if (isBrushEntity)
                {
                    BEntities.Add(ConvertBrushEntity(Entity, Attributes_dictionary));
                }
                else
                {
                    string Origin;
                    if (Attributes_dictionary.ContainsKey("origin"))
                    {
                        Origin = Attributes_dictionary["origin"];
                    }
                    else
                    {
                        Origin = "0 0 0";
                    }
                    string Classname = Attributes_dictionary["classname"];
                    Attributes_dictionary.Remove("classname");
                    Attributes_dictionary.Remove("origin");
                    PEntities.Add(new Point_Entity(MathLib.ThreeNumberstringsToVector3(Origin, " "), Attributes_dictionary, Classname));

                }
            }
            WorldStruct.PEntities = PEntities.ToArray();
            WorldStruct.BEntities = BEntities.ToArray();
            return WorldStruct;
        }

        static Dictionary<string, string> GetAttributes(Node Entity)
        {
            Node[] Attributes = Entity.GetNodesWithStringInName("Attribute:", true);
            Dictionary<string, string> Attributes_dictionary = new Dictionary<string, string>();
            foreach (Node Attribute in Attributes)
            {
                try
                {
                    Attributes_dictionary.Add(Attribute.Name, Attribute.Keyvalue);
                }
                catch(Exception Except)
                {
                    Console.WriteLine("Attibute addition failed! reason: {0}", Except.Message);
                }
            }
            return Attributes_dictionary;
        }

        static bool CheckIsBrushEntity(Node Entity)
        {
            Node Brushes = Entity.GetNodebyName("Brush");
            if (Brushes != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static Map ImportMapFile(string Path)
        {
            Console.WriteLine("Loading File");
            StreamReader fs = new StreamReader(Path);
            string File_string = fs.ReadToEnd();
            Console.WriteLine("Creating Parse tree");
            var WorldandEntities = Preprocessing.Break_Entities(File_string);

            for (int i = 0; i < WorldandEntities.Children.Count; i++)
            {
                var Entity = WorldandEntities.Children[i];
                NewParser.ParseEntity(ref Entity);
            }
            var Worldstruct = ConvertMap(WorldandEntities);

            return Worldstruct;
        }
    }
}

