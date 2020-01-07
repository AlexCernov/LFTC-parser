using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab4.Models
{
    public class InputStack
    {
         public List<string> inputStack { get; set; }

        public int Count { get { return inputStack.Count; } }

        public InputStack()
        {
            inputStack = new List<string>();
        }

        public void Push(string item)
        {
            inputStack.Insert(0,item);
        }

        public void Push(List<string> items)
        {
            inputStack.InsertRange(0,items);
        }
        
        public string Peek()
        {
            return inputStack[0];
        }

        public string Pop()
        {
            var returnItem = inputStack[0];
            inputStack = inputStack.GetRange(1, inputStack.Count-1);
            return returnItem;
        }
    }
}
