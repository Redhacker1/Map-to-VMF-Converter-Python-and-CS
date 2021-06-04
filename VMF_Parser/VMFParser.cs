using System;
using System.Collections.Generic;
using System.Text;
using MapConverter_Shared;

namespace VMF_Parser
{
    public class VMFParser
    {
		static bool Iswhitespace(char c)
        {
			if(c == ' ' || c == '\f' || c == '\n' || c == '\r' || c == '\t' || c == '\v')
            {
				return true;
            }
			else
            {
				return false;
            }
        }
        public static List<string> TokenizeString(string source)
        {

			StringBuilder current_token = new StringBuilder();
			List<string> tokens = new List<string>();
			bool toggle_escape = false;

			// tokenize the string
			foreach (char c in source)
			{
				if (c == '"') toggle_escape = !toggle_escape;
				else if (Iswhitespace(c) && toggle_escape == false)
				{
					if (current_token.ToString() == "") continue;
					tokens.Add(current_token.ToString());
					current_token.Clear();
				}
				else
				{
					current_token.Append(c);
				}
			}

			return tokens;
		}


		private enum State
        {
			IsBrush,
			IsEntity,
			IsAwaitingAssignment
        }
		public static Node VMFParser_logic(List<string> Tokens)
        {
			Node WorldNode = new Node("world", null);
			Node CurrentNode;
			for (int T_index = 0; T_index < Tokens.Count; T_index++)
            {
				string Token = Tokens[T_index];
				if(Token == "{")
                {

                }
                else if (Token == "}")
                {

                }
				else
                {

                }

				
            }
        }


    }
}
