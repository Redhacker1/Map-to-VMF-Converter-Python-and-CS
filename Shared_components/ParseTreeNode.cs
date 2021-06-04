using System.Collections.Generic;
using System.Linq;

namespace MapConverter_Shared
{
    public class Node
    {
        public Node(string name, Node parent)
        {
            Name = name;
            Parent = parent;
        }
        /*public Node(string name)
        {
            Name = name;
            Parent = null;
        }*/
        public string Name;
        public List<Node> Children = new List<Node>();
        public string Keyvalue;
        public readonly Node Parent;

        public int Childcount(bool Recursive)
        {
            int SelfChildCount = Children.Count;
            if (!Recursive || Children.Count == 0)
            {
                return SelfChildCount;
            }
            else
            {
                int Regularcount = 0;
                foreach (Node Thing in Children)
                {
                    Regularcount += Thing.Childcount(Recursive);
                }
                return SelfChildCount + Regularcount;
            }

        }
        public Node GetNodebyName(string DesiredName, out bool bWasSuccessful)
        {
            foreach(Node Item in Children)
            {
                if(Item.Name == DesiredName)
                {
                    bWasSuccessful = true;
                    return Item;
                }

            }
            bWasSuccessful = false;
            return null;
        }

        public Node GetNodesByPath(string Path, string Seperator = "/")
        {
            List<string> Split_path = Path.Split(Seperator).ToList();
            return GetNodesByPath(Split_path);
        }

        private Node GetNodesByPath(List<string> Path_split)
        {
            if(Path_split[0] == Name)
            {
                Path_split.Remove(Name);
                return GetNodesByPath(Path_split);
            }
            // Checks and sees, maybe you forgot to include the node in the path, significantly more expensive potentially so I would not forget to do that but it does work.
            else if (ContainsNode(Name))
            {
                return GetNodesByPath(Path_split);
            }
            return null;
        }

        public Node[] GetNodesByName(string DesiredName)
        {
            List<Node> Nodes = new List<Node>();
            foreach (Node Item in Children)
            {
                if (Item.Name == DesiredName)
                {
                    Nodes.Add(Item);
                }
            }
            return Nodes.ToArray();
        }

        public bool ContainsNode(string Name)
        {
            foreach(Node Entity in Children)
            {
                if(Entity.Name == Name)
                {
                    return true;
                }
            }
            return false;
        }
        public Node[] GetNodesWithStringInName(string DesiredStringinName, bool Remove_StringInName)
        {
            List<Node> Nodes = new List<Node>();
            foreach (Node Item in Children)
            {
                if (Item.Name.Contains(DesiredStringinName))
                {
                    if(Remove_StringInName)
                    {
                        Item.Name = Item.Name.Replace(DesiredStringinName, "");
                    }
                    Nodes.Add(Item);
                }
            }
            return Nodes.ToArray();
        }
    }
}
