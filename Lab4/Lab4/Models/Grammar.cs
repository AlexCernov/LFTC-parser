using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab4.Models
{
    public class Grammar
    {
        public List<string> NonTerminals;
        public List<string> Terminals;
        public List<KeyValuePair<string, List<string>>> Productions;
        public string Symbol;
        private string _filename;

        public Grammar()
        {
            Productions = new List<KeyValuePair<string, List<string>>>();

        }

        public Grammar(string filename)
        {
            Productions = new List<KeyValuePair<string, List<string>>>();
            _filename = filename;
            setGrammar();

        }

        public bool IsTerminal(string value)
        {
            return Terminals.Contains(value);
        }
        public bool IsNonTerminal(string value)
        {
            return NonTerminals.Contains(value);
        }
        public static List<string> GetSymbols(string line)
        {
            var symbols = new List<string>();
            var elements = line.Split(',');

            for (int i = 0; i < elements.Length; i++)
            {
                elements[i] = elements[i].Replace("{", "").Replace(" ", "").Replace("}", "");
                if (Regex.IsMatch(elements[i], @"^[0-9]|[a-zA-Z][a-zA-Z]*$"))
                    symbols.Add(elements[i]);
            }

            return symbols;
        }
        public static Grammar ReadFromFile(string filename)
        {
            try
            {
                string[] lines = System.IO.File.ReadAllLines(@filename);
                Grammar grammar = new Grammar();
                grammar.NonTerminals = GetSymbols(lines[0].Split('=')[1]);
                grammar.Terminals = GetSymbols(lines[1].Split('=')[1]);
                grammar.Symbol = GetSymbols(lines[2].Split('=')[1])[0];
                int i = 4;
                while (i < lines.Length - 1)
                {
                    string line = lines[i];
                    string rule = line.Trim(',').Trim('\n').Trim('\t');
                    string lhs = rule.Split("->")[0];
                    string rhsNotParsed = rule.Split("->")[1];
                    string[] rhs = rhsNotParsed.Split('|');
                    foreach (var element in rhs)
                    {
                        var prod_list = element.Split(" ").ToList();
                        prod_list.RemoveAll(x => x.Equals(""));
                        grammar.Productions.Add(new KeyValuePair<string, List<string>>(lhs.Trim(), prod_list));
                    }
                    i++;
                }
                return grammar;
            }
            catch (Exception e)
            {

                throw e;
            }


        }

        public bool IsRegular()
        {
            List<string> usedInRhs = new List<string>();
            bool ok = false;

            foreach (var rule in Productions)
            {
                string lhs = rule.Key;
                var rhs = rule.Value;
                bool hasTerminal = false;
                bool hasNonTerminal = false;
                foreach (var ch in rhs)
                {
                    if (this.IsNonTerminal(ch))
                    {
                        usedInRhs.Add(ch.ToString());
                        hasNonTerminal = true;
                    }
                    else if (this.IsTerminal(ch))
                    {
                        if (hasNonTerminal)
                            return false;
                        hasTerminal = true;
                    }
                    if (ch == "e")
                    {
                        if (lhs != Symbol) return false;
                        else ok = true;
                    }
                }
                if (hasNonTerminal && !hasTerminal)
                    return false;
            }
            if (ok && usedInRhs.Contains(Symbol)) return false;
            return true;
        }
        public List<KeyValuePair<string,List<string>>> GetProductionsForNonTerminal(string symbol)
        {
            if (!IsNonTerminal(symbol))
                return null;
            else
            {
                var returnList = new List<KeyValuePair<string, List<string>>>();
                foreach (var pair in Productions)
                {
                    if (pair.Key.Equals(symbol))
                        returnList.Add(pair);
                }
                return returnList;
            }
        }



        private void setGrammar()
        {
            string[] lines = System.IO.File.ReadAllLines(this._filename);
            this.NonTerminals = GetSymbols(lines[0].Split('=')[1]);
            this.Terminals = GetSymbols(lines[1].Split('=')[1]);
            this.Symbol = GetSymbols(lines[2].Split('=')[1])[0];
            int i = 4;
            while (i < lines.Length - 1)
            {
                string line = lines[i];
                string rule = line.Trim(',').Trim('\n').Trim('\t');
                string lhs = rule.Split("->")[0];
                string rhsNotParsed = rule.Split("->")[1];
                string[] rhs = rhsNotParsed.Split('|');
                foreach (var element in rhs)
                {
                    Productions.Add(new KeyValuePair<string, List<string>>(lhs.Trim(), element.Split(" ").ToList()));
                }
                i++;
            }
        }

    }
}
