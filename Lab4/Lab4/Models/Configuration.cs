using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4.Models
{
    public class Configuration
    {

        public State State { get; set; }
        public int Index { get; set; }
        public WorkStack<dynamic> WorkStack { get; set; }
        public Stack<string> InputStack { get; set; }

        public Configuration(string symbol)
        {
            State = State.NORMAL;
            Index = 0;
            WorkStack = new WorkStack<dynamic>();
            InputStack = new Stack<string>();
            InputStack.Push(symbol);
        }
    }
}
