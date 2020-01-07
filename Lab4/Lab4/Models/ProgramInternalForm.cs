using System.Collections.Generic;
using System.Text;

namespace Lab4.Models
{
    public class ProgramInternalForm
    {
        readonly List<KeyValuePair<int, int>> pif;

        public ProgramInternalForm()
        {
            pif = new List<KeyValuePair<int, int>>();

        }

        public void Add(int code, int identification)
        {
            pif.Add(new KeyValuePair<int, int>(code, identification));
        }

        public override string ToString()
        {
            StringBuilder @returnValue = new StringBuilder();
            returnValue.Append("Program internal form \n");
            foreach (KeyValuePair<int, int> pair in pif)
            {
                returnValue.Append(pair.Key + " " + pair.Value + '\n');
            }
            return returnValue.ToString();
        }

        public List<int> CodeList()
        {
            var returnList = new List<int>();
            foreach (var item in pif)
            {
                returnList.Add(item.Key);
            }
            return returnList;
        }
    }
}
