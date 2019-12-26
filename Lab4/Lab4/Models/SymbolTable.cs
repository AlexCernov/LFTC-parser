using Lab4.Models.BinaryTree;

namespace Lab4.Models
{
    public class SymbolTable
    {
        private BinarySearchTree bst;

        public SymbolTable()
        {
            bst = new BinarySearchTree();
        }

        public int? Insert(dynamic @value)
        {
            return bst.Insert(value);
        }

        public int? Find(dynamic @value)
        {
            return bst.Find(value);
        }

        public override string ToString()
        {
            return "Symbol table: \n" + bst.ToString();
        }

    }
}
