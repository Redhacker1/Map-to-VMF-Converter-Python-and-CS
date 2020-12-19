using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MapConverter
{
    class XMLMapWriter
    {
        public static XmlWriter MakeMapFile(string Path, string Filename, bool XMLTest = false)
        {
            XmlWriterSettings settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t"
            };
            if (Filename.Contains(".vmf"))
            {
                Filename.Replace(".vmf", ".xml");
            }
            else if (Filename.ToLower().Contains(".map"))
            {
                //Hack: Older .map files are in all uppercase.
                Filename.Replace(".map", ".xml");
                Filename.Replace(".MAP", ".xml");
            }
            if (Path != string.Empty)
            {
                _ = System.IO.Directory.CreateDirectory(string.Format("/Converted/"));
            }
            XmlWriter xmlWriter = XmlWriter.Create(Path + Filename, settings);

            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("map");
            return xmlWriter;
        }

        public static void StartEntities(XmlWriter MapFile)
        {
            MapFile.WriteStartElement("entities");
        }
        public static void EndEntities(XmlWriter MapFile)
        {
            MapFile.WriteEndElement();
        }

        public static void EndFile(XmlWriter MapFile)
        {
            MapFile.WriteEndElement();
        }

        static void WriteVector(XmlWriter MapFile, Vector3 Vector3, string Element_Name = "")
        {
            if (Element_Name != "")
            {
                MapFile.WriteStartElement(Element_Name);
            }
            else
            {
                MapFile.WriteStartElement("position");
            }
            MapFile.WriteString(Vector3.X.ToString() + " ");
            MapFile.WriteString(Vector3.Y.ToString() + " ");
            MapFile.WriteString(Vector3.Z.ToString());
            if (Element_Name != "")
            {
                MapFile.WriteEndElement();
            }
        }

        public static void PointEntityToXML(XmlWriter MapFile, Point_Entity entity)
        {

            MapFile.WriteStartElement(entity.Classname);
            WriteVector(MapFile, entity.Location, "origin");
            foreach (string Key in entity.Attributes.Keys)
            {
                MapFile.WriteStartElement(Key);
                MapFile.WriteString(entity.Attributes[Key]);
                MapFile.WriteEndElement();
            }
            MapFile.WriteEndElement();
        }

        public static void BrushEntityToXML(XmlWriter MapFile, BrushEntity entity, bool DontIgnoreWorldSpawn)
        {
            if (entity.Classname.ToLower() != "worldspawn" || DontIgnoreWorldSpawn)
            {
                if(entity.Classname.ToLower() == "worldspawn")
                {
                    entity.Classname = "func_brush";
                    entity.Attributes = new Dictionary<string, string>();
                }
                MapFile.WriteStartElement(entity.Classname);
                foreach (string Key in entity.Attributes.Keys)
                {
                    MapFile.WriteStartElement(Key);
                    MapFile.WriteString(entity.Attributes[Key]);
                    MapFile.WriteEndElement();
                }
                MapFile.WriteStartElement("brushes");
                foreach (Brush brush in entity.Brushes)
                {
                    int side_count = brush.Sides.Length;
                    MapFile.WriteStartElement("brush");
                    MapFile.WriteAttributeString("id", brush.ID.ToString());
                    MapFile.WriteAttributeString("faces", side_count.ToString());
                    int DoneSides = 0;
                    if(brush.Sides.Length == 0)
                    {
                        Console.WriteLine("Invalid Brush");
                    }
                    foreach (Side Face in brush.Sides)
                    {
                        MapFile.WriteStartElement("face");
                        MapFile.WriteAttributeString("id", Face.Id.ToString());
                        MapFile.WriteElementString("material", Face.Texture);
                        for (int i = 0; i < 3; i++)
                        {
                            MapFile.WriteStartElement("HalfSpace_Coord");
                            MapFile.WriteAttributeString("id", i.ToString());
                            MapFile.WriteString(Face.Points[i].X.ToString() + " ");
                            MapFile.WriteString(Face.Points[i].Y.ToString() + " ");
                            MapFile.WriteString(Face.Points[i].Z.ToString());
                            MapFile.WriteEndElement();
                        }
                        MapFile.WriteStartElement("texture_pos");
                        MapFile.WriteAttributeString("tex_x", Face.X_offset.ToString());
                        MapFile.WriteAttributeString("tex_y", Face.Y_offset.ToString());
                        MapFile.WriteAttributeString("x", Face.X_scale.ToString());
                        MapFile.WriteAttributeString("y", Face.Y_scale.ToString());
                        MapFile.WriteEndElement();
                        WriteVector(MapFile, Face.Normal, "normal");
                        WriteVector(MapFile, Face.Normal_Unnormalized, "normal_not_normalized");
                        MapFile.WriteEndElement();
                        ++DoneSides;
                    }
                    if (DoneSides != brush.Sides.Length)
                    {
                        Console.WriteLine("{0} is not equal to {1}, Offending brush {2}", DoneSides + 1, brush.Sides.Length, brush.ID);
                        throw (new IndexOutOfRangeException());
                    }
                    MapFile.WriteEndElement();

                }
                MapFile.WriteEndElement();
                MapFile.WriteEndElement();
            }
        }
    }
}
