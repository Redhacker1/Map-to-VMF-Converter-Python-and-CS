using General_libs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;

namespace MapConverter
{
    static class NewParser
    {

        static public Node Break_Entities(string MapFile)
        {
            Node World = new Node("World");
            int Entity_Reset = 0;
            int Entity_Count = 0;

            string[] FileLines = MapFile.Split('\n');

            for (int increment = 0; increment < FileLines.Length; increment++)
            {
                FileLines[increment] += '\n';
            }
            StringBuilder current_entity = new StringBuilder();
            char previous_char = '\0';

            foreach (string line in FileLines)
            {
                foreach (char character in line)
                {
                    if (character == (char)13)
                    {
                        if (previous_char == '}')
                        {
                            --Entity_Reset;
                            if (Entity_Reset == 0)
                            {
                                if (current_entity[current_entity.Length - 1] == '}')
                                {
                                    current_entity.Remove(current_entity.Length - 1, 1);
                                }
                                Node EntityNode = new Node(string.Format("Entity", Entity_Count))
                                {
                                    Keyvalue = current_entity.ToString()
                                };
                                World.Children.Add(EntityNode);
                                Entity_Count++;
                            }
                        }
                        else if (previous_char == '{')
                        {
                            ++Entity_Reset;
                            if (Entity_Reset == 1)
                            {
                                current_entity.Clear();
                            }
                        }
                    }
                    else
                    {
                        current_entity.Append(character);
                        previous_char = character;
                    }
                }

            }

            return World;
        }

        static void PullKeyValuePairs(ref Node Entity)
        {
            StringBuilder String = new StringBuilder();
            bool Instring = false;
            bool Key = true;
            Node KeyValuePair = new Node("Blank");
            for (int i = 0; i < Entity.Keyvalue.Length; i++)
            {

                char Character = Entity.Keyvalue[i];
                if (Character == '"')
                {
                    if (Instring)
                    {
                        if (Key)
                        {
                            KeyValuePair = new Node("Attribute:" + String.ToString().Trim());
                            Key = false;
                        }
                        else
                        {
                            KeyValuePair.Keyvalue = String.ToString().Trim();
                            Entity.Children.Add(KeyValuePair);
                            Key = true;
                        }
                        Instring = false;
                    }
                    else
                    {
                        String.Clear();
                        Instring = true;
                    }
                }
                else
                {
                    String.Append(Character);
                }
            }
        }
        public static Node ImportMapFile(string Path)
        {
            Console.WriteLine("Loading File");
            StreamReader fs = new StreamReader(Path);
            string File_string = fs.ReadToEnd();
            Console.WriteLine("Creating Parse tree");
            var WorldandEntities = Break_Entities(File_string);
            for (int i = 0; i < WorldandEntities.Children.Count; i++)
            {
                var Entity = WorldandEntities.Children[i];
                ParseEntity(ref Entity);
            }
            ConvertMap(WorldandEntities);

            return WorldandEntities;
        }

        static Dictionary<string, string> GetAttributes(Node Entity)
        {
            Node[] Attributes = Entity.GetNodesWithStringInName("Attribute:", true);
            Dictionary<string, string> Attributes_dictionary = new Dictionary<string, string>();
            foreach (Node Attribute in Attributes)
            {
                Attributes_dictionary.Add(Attribute.Name, Attribute.Keyvalue);
            }
            return Attributes_dictionary;
        }

        static bool CheckIsBrushEntity(Node Entity)
        {
            Node Brushes = Entity.GetNodebyName("Brush");
            if(Brushes != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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
                    Node[] Brushes = Entity.GetNodesByName("Brush");
                    List<Brush> Brush_list = new List<Brush>();
                    foreach (Node BrushNode in Brushes)
                    {
                        var CurrentBrush = new Brush
                        {
                            ID = Convert.ToInt32(BrushNode.Keyvalue)
                        };
                        List<Side> Sides = new List<Side>();
                        foreach (Node SideNode in BrushNode.GetNodesWithStringInName("Side", false))
                        {
                            //Console.WriteLine(Side.Childcount(true));
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

                            Sides.Add(new Side(Texture, FaceCoords, Convert.ToSingle(X_Offset), Convert.ToSingle(Y_Offset), Convert.ToSingle(Rot_Angle), Convert.ToSingle(Y_Scale), Convert.ToSingle(X_Scale), Convert.ToInt32(ID)));

                        }
                        CurrentBrush.Sides = Sides.ToArray();
                        Brush_list.Add(CurrentBrush);
                    }
                    string Classname = Attributes_dictionary["classname"];
                    Attributes_dictionary.Remove("classname");
                    BEntities.Add(new BrushEntity(Attributes_dictionary, Classname, Brush_list.ToArray()));
                }
                else
                {
                    string Origin = Attributes_dictionary["origin"];
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

        static void ParseFace(ref Node Brush, string brush)
        {
            string[] Sides = brush.Trim().Split('\n');
            int SideID = 0;
            foreach (string side in Sides)
            {
                Node Sidenode = new Node("Side");
                string[] Side_components = side.Trim().Split(" ");
                var a = new StringBuilder();
                string[] TupleList = new string[3];
                int Index = 0;

                foreach (char Character in side)
                {
                    if (Character == '(')
                    {
                        a.Clear();
                    }

                    else if (Character == ')')
                    {
                        Node Coords = new Node(string.Format("PointLocation_{0}", Index));
                        Coords.Keyvalue = a.ToString();
                        Sidenode.Children.Add(Coords);
                        Index++;
                    }
                    else
                    {
                        a.Append(Character);
                    }
                }

                //Texture
                float value = float.NaN;
                bool Success = float.TryParse(Side_components[15], out _);

                string Texture = string.Empty;
                bool OffsetByOne = false;
                if (Success)
                {
                    Console.WriteLine("Failed!, Trying value before!");
                    Success = float.TryParse(Side_components[14], out _);
                    OffsetByOne = true;
                    if (!Success)
                    {
                        Console.WriteLine("Success, Value needs to be offset by 1");
                        Texture = Side_components[14];
                        Console.WriteLine(Texture);
                    }
                    else
                    {
                        throw new FormatException(message: "The brush side is incorrect!");
                    }
                }
                else
                {
                    Texture = Side_components[15];
                }
                Node TextureNode = new Node("Texture")
                {
                    Keyvalue = Texture
                };
                Sidenode.Children.Add(TextureNode);

                //X offset 
                string X_off;
                if (OffsetByOne)
                {
                    X_off = Side_components[15];
                }
                else
                {
                    X_off = Side_components[16];
                }
                Node XOffsetNode = new Node("X_offset")
                {
                    Keyvalue = X_off
                };
                Sidenode.Children.Add(XOffsetNode);

                // Y Offset
                string Y_off;
                if (OffsetByOne)
                {
                    Y_off = Side_components[16];
                }
                else
                {
                    Y_off = Side_components[17];
                }
                Node YOffsetNode = new Node("Y_offset")
                {
                    Keyvalue = Y_off
                };
                Sidenode.Children.Add(YOffsetNode);

                //Rotation
                string Rotation;
                if (OffsetByOne)
                {
                    Rotation = Side_components[17];
                }
                else
                {
                    Rotation = Side_components[18];
                }
                Node RotationNode = new Node("Rot_angle")
                {
                    Keyvalue = Rotation
                };
                Sidenode.Children.Add(RotationNode);

                //X Scale 
                string X_Scale;
                if (OffsetByOne)
                {
                    X_Scale = Side_components[18];
                }
                else
                {
                    X_Scale = Side_components[19];
                }
                Node XScaleNode = new Node("X_scale")
                {
                    Keyvalue = X_Scale
                };
                Sidenode.Children.Add(XScaleNode);

                //Y Scale 
                string Y_Scale;
                if (OffsetByOne)
                {
                    Y_Scale = Side_components[19];
                }
                else
                {
                    Y_Scale = Side_components[20];
                }
                Node YScaleNode = new Node("Y_scale")
                {
                    Keyvalue = Y_Scale
                };
                Sidenode.Children.Add(YScaleNode);
                Node IDNode = new Node("ID")
                {
                    Keyvalue = SideID.ToString()
                };
                Sidenode.Children.Add(YScaleNode);
                Sidenode.Children.Add(IDNode);
                SideID++;

                // Add Side to Brush
                Brush.Children.Add(Sidenode);
            }
        }
        static Node ParseBrush(string brush, int ID)
        {
            Node brushNode = new Node("Brush");
            brushNode.Keyvalue = ID.ToString();
            ParseFace(ref brushNode, brush);
            return brushNode;
        }

        static void ParseBrushes(ref Node Entity)
        {
            string[] Brushes = BreakBrushes(Entity);
            int BrushID = 0;
            foreach (string brush in Brushes)
            {
                Node brushNode = ParseBrush(brush, BrushID);
                BrushID++;
                Entity.Children.Add(brushNode);
            }
        }

        public static void ParseEntity(ref Node Entity)
        {
            // Actually all we need to parse a Point entity!
            PullKeyValuePairs(ref Entity);
            //Checks to see if we have a brush entity... because they require a LOT more work and a LOT more memory
            if (Entity.Keyvalue.Contains('{') & Entity.Keyvalue.Contains('}'))
            {
                ParseBrushes(ref Entity);
            }
        }
    

        static string[] BreakBrushes(Node Entity)
        {
            StringBuilder current_Brush = new StringBuilder();
            int BrushCount = 0;
            List<string> Brushes = new List<string>();
            char Previous_Value = '\n';
            foreach (char Character in Entity.Keyvalue)
            {
                if (Previous_Value == '{' && Character == '\n')
                {
                    current_Brush.Clear();
                    BrushCount++;
                }
                else if (Character == '}' && Previous_Value == '\n')
                {
                    Brushes.Add(current_Brush.ToString());
                }
                else
                {
                    current_Brush.Append(Character);
                }
                Previous_Value = Character;
            }
            Entity.Keyvalue = string.Empty;
            return Brushes.ToArray();
        }
    }
}
