using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab4.Models
{
    public class Grammar
    {
        public List<string> NonTerminals;
        public List<string> Terminals;
        public List<KeyValuePair<string, string>> Productions;
        public string Symbol;
        private string _filename;

        public Grammar()
        {
            Productions = new List<KeyValuePair<string, string>>();

        }

        public Grammar(string filename)
        {
            Productions = new List<KeyValuePair<string, string>>();
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
                if (Regex.IsMatch(elements[i], @"^[a-zA-Z][a-zA-Z]*$"))
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
                        grammar.Productions.Add(new KeyValuePair<string, string>(lhs.Trim(), element.Trim()));
                    }
                    i++;
                }
                return grammar;
            }
            catch (Exception)
            {

                throw;
            }


        }
      
        public bool IsRegular()
        {
            List<string> usedInRhs = new List<string>();
            bool ok = false;

            foreach (var rule in Productions)
            {
                string lhs = rule.Key;
                string rhs = rule.Value;
                bool hasTerminal = false;
                bool hasNonTerminal = false;
                foreach (char ch in rhs)
                {
                    if (this.IsNonTerminal(ch.ToString()))
                    {
                        usedInRhs.Add(ch.ToString());
                        hasNonTerminal = true;
                    }
                    else if (this.IsTerminal(ch.ToString()))
                    {
                        if (hasNonTerminal)
                            return false;
                        hasTerminal = true;
                    }
                    if (ch == 'e')
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
        public List<KeyValuePair<string, string>> GetProductionsForNonTerminal(string symbol)
        {
            if (!IsNonTerminal(symbol))
                return null;
            else
            {
                var returnList = new List<KeyValuePair<string, string>>();
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
                    this.Productions.Add(new KeyValuePair<string, string>(lhs, element));
                }
                i++;
            }
        }

    }
}
