using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Lab4.Models
{
    public class WorkStack<dynamic>
    {
        private List<dynamic> workStack;

        public WorkStack()
        {
            workStack = new List<dynamic>();
        }


        public dynamic Peek()
        {
            return workStack[workStack.Count - 1];
        }

        public dynamic Pop()
        {
            var element = Peek();
            workStack = new List<dynamic>(workStack.GetRange(0, workStack.Count - 2));
            return element;
        }

        public void Add(dynamic element)
        {
            workStack.Add(element);
        }

        public List<dynamic> getForPrint()
        {
            return workStack;
        }
    }
}
