using System.Collections.Generic;

namespace MapConverter
{
    class Node
    {
        public Node(string name)
        {
            Name = name;
        }
        public string Name;
        public List<Node> Children = new List<Node>();
        public string Keyvalue;

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
        public Node GetNodebyName(string DesiredName)
        {
            foreach(Node Item in Children)
            {
                if(Item.Name == DesiredName)
                {
                    return Item;
                }

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
