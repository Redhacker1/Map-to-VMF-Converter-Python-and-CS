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

                //HACK: Set Texture coordinates to 0 when dealing with Vave 220 format
                bool Valve = false;
                float value = float.NaN;
                bool Success = float.TryParse(Side_components[15], out _);

                if (side.Contains("[") && side.Contains("]"))
                {
                    Valve = true;

                }

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
                    if(!Valve)
                    {
                        X_off = Side_components[15];
                    }
                    else
                    {
                        X_off = "0";                //HACK: Set Texture coordinates to 0 when dealing with Vave 220 format
                    }
                }
                else
                {
                    if(!Valve)
                    {
                        X_off = Side_components[16];
                    }
                    else                 //HACK: Set Texture coordinates to 0 when dealing with Vave 220 format
                    {
                        X_off = "0";
                    }
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
                    if(!Valve)
                    {
                        Y_off = Side_components[16];
                    }
                    else                 //HACK: Set Texture coordinates to 0 when dealing with Vave 220 format
                    {
                        Y_off = "0";
                    }
                }
                else
                {
                    if(!Valve)
                    {
                        Y_off = Side_components[17];
                    }
                    else
                    {
                        Y_off = "0";                //HACK: Set Texture coordinates to 0 when dealing with Vave 220 format
                    }
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
            Console.WriteLine(Entity.Keyvalue);
            return Brushes.ToArray();
        }
    }
}
