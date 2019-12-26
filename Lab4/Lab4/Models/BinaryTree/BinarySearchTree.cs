using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4.Models.BinaryTree
{
    class BinarySearchTree
    {
        public List<dynamic> Tree { get; private set; }

        public BinarySearchTree()
        {
            Tree = new List<dynamic>();
        }

        public int Insert(dynamic value)
        {
            if (Tree.Count == 0)
            {
                Tree.Add(value);
                return 0;
            }
            else
            {
                int position = 1;
                bool inserted = false;
                while (position <= Tree.Count && inserted == false)
                {
                    if (value.GetType().Equals(typeof(string)))
                    {
                        if (string.Compare(value, Tree[position - 1]) == -1)
                        {
                            position = position * 2;
                            if (position <= Tree.Count)
                                if (Tree.ElementAtOrDefault(position - 1) == null)
                                {
                                    Tree[position - 1] = value;
                                    inserted = true;
                                }

                        }
                        else
                        {
                            position = position * 2 + 1;
                            if (position <= Tree.Count)
                                if (Tree.ElementAtOrDefault(position - 1) == null)
                                {
                                    Tree[position - 1] = value;
                                    inserted = true;
                                }
                        }
                    }
                    else if (value.GetType().Equals(typeof(char)) || value.GetType().Equals(typeof(int)))
                    {
                        if (Compare(value, Tree[position - 1]) < 0)
                        {
                            position *= 2;
                            if (position <= Tree.Count)
                                if (Tree.ElementAtOrDefault(position - 1) == null)
                                {
                                    Tree[position - 1] = value;
                                    inserted = true;
                                }

                        }
                        else
                        {
                            position = position * 2 + 1;
                            if (position <= Tree.Count)
                                if (Tree.ElementAtOrDefault(position - 1) == null)
                                {
                                    Tree[position - 1] = value;
                                    inserted = true;
                                }
                        }
                    }

                }
                if (inserted == false)
                {
                    while (Tree.Count < position)
                        Tree.Add(null);
                    Tree[Tree.Count - 1] = value;
                }
                return position - 1;
            }
        }

        public int? Find(dynamic value)
        {
            if (Tree.Count == 0)
                return null;
            else
            {
                if (Tree[0].GetType().Equals(typeof(string)))
                {
                    int pos = 1;
                    while (pos <= Tree.Count)
                    {
                        if (Tree.ElementAtOrDefault(pos - 1) == null)
                            return null;
                        if (value.Equals(Tree.ElementAtOrDefault(pos - 1)))
                            return pos;
                        if (string.Compare(value, Tree.ElementAtOrDefault(pos - 1)) == -1)
                            pos *= 2;
                        else pos = pos * 2 + 1;

                    }
                }
                else
                {
                    int pos = 1;
                    while (pos <= Tree.Count)
                    {
                        if (Tree.ElementAtOrDefault(pos - 1) == null)
                            return null;
                        if (Compare(value, Tree.ElementAtOrDefault(pos - 1)) == 0)
                            return pos;
                        if (Compare(value, Tree.ElementAtOrDefault(pos - 1)) < 0)
                            pos *= 2;
                        else pos = pos * 2 + 1;

                    }
                }

                return null;
            }
        }

        //
        private int Compare(dynamic a, dynamic b)
        {

            if (a.GetType() != b.GetType())
            {

                if (a.GetType().Equals(typeof(char)) && b.GetType().Equals(typeof(int)))
                {
                    if ((int)a < b)
                        return -1;
                    else if ((int)a == b)
                        return 0;
                    else return 1;
                }
                else if (a.GetType().Equals(typeof(int)) && b.GetType().Equals(typeof(char)))
                {
                    if ((int)b < a)
                        return 1;
                    else if ((int)b == a)
                        return 0;
                    else return -1;
                }
            }
            else if (a.GetType().Equals(typeof(char)) && b.GetType().Equals(typeof(char)))
            {
                return a.CompareTo(b);

            }
            else if (a.GetType().Equals(typeof(int)) && b.GetType().Equals(typeof(int)))
            {
                if (a < b)
                    return -1;
                else if (a == b)
                    return 0;
                else return 1;

            }

            else throw new Exception("The types: " + a.GetType() + " " + b.GetType() + "are not handled by the bst");
            return 0;

        }

        public override string ToString()
        {
            StringBuilder returnValue = new StringBuilder();
            for (int i = 0; i < Tree.Count; i++)
            {
                if (Tree[i] != null)
                    returnValue.Append(i.ToString() + "" + Tree[i].ToString());

            }
            return returnValue.ToString();
        }



    }
}
