using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4.Models
{
    public class Utils
    {
        public readonly List<char> separators = new List<char> { ' ', '[', ']', '{', '}', '(', ')', ';' };
        public readonly List<string> operators = new List<string> { "+", "-", "*", "/", "%", "<", "<=", ">", ">=", "==", "||", "&&", "=", "!", "!=" };
        public readonly List<string> reservedWords = new List<string> { "int", "char", "main", "if", "while", "else", "read", "print", "return" };

        public readonly List<string> everything = new List<string>();

        public Utils()
        {
            everything.AddRange(separators.Select(x => { return x.ToString(); }).ToList());
            everything.AddRange(operators);
            everything.AddRange(reservedWords);
        }
    }
}
