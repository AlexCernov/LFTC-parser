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
            try
            {
                return workStack[workStack.Count - 1];

            }
            catch (Exception e)
            {

                Console.WriteLine(e.StackTrace);
                throw e;
            }
        }

        public dynamic Pop()
        {
            if (workStack.Count == 1) return Peek();
            var element = Peek();
            workStack.RemoveAt(workStack.Count-1);
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
