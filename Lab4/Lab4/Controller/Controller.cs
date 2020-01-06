using Lab4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lab4.Controller
{
    public class Controller
    {
        public static Grammar Grammar { get; set; }

        public Controller()
        {

        }

        public static KeyValuePair<string, List<string>> GetNextProduction(KeyValuePair<string, List<string>> prod, List<KeyValuePair<string, List<string>>> prodList)
        {
            for(int i = 0; i < prodList.Count; i++)
            {
                if (prodList[i].Key == prod.Key && prodList[i].Value.SequenceEqual(prod.Value) && i < prodList.Count - 1)
                    return prodList[i + 1];
            }
            return new KeyValuePair<string, List<string>>();
        }

        public static void Parser(List<int> sequence)
        {
            Configuration config = new Configuration(Grammar.Symbol);
            while( config.State != State.FINAL && config.State != State.ERROR )
            {
                if(config.State == State.NORMAL)
                {
                    if (config.InputStack.Count == 0 && config.Index == sequence.Count)
                        config.State = State.FINAL;
                    else if (config.InputStack.Count == 0)
                        config.State = State.ERROR;
                    else
                    {
                        if(Grammar.NonTerminals.Contains(config.InputStack.Peek()))
                        {
                            string nonTerminal = config.InputStack.Pop();
                            var firstProduction = Grammar.GetProductionsForNonTerminal(nonTerminal)[0];
                            config.WorkStack.Add(nonTerminal);
                            firstProduction.Value.ForEach(x => config.InputStack.Push(x));

                        }
                        else
                        {
                            if (config.Index == sequence.Count)
                                config.State = State.BACK;
                            else if (config.InputStack.Peek().Equals("e"))
                            {
                                config.WorkStack.Add("e");
                                config.InputStack.Pop();
                            }
                            else if (config.InputStack.Peek().Equals(sequence[config.Index]))
                            {
                                config.Index++;
                                config.WorkStack.Add(config.InputStack.Pop());
                               
                            }
                            else
                                config.State = State.BACK;
                        }
                    }
                }
                else
                {
                    if(config.State == State.BACK)
                    {
                        if (config.WorkStack.Peek().GetType() == typeof(string))
                        {
                            if (Grammar.Terminals.Contains(config.WorkStack.Peek()))
                                if (config.WorkStack.Peek().Equals("e"))
                                    config.WorkStack.Pop();
                                else
                                {
                                    config.Index--;
                                    string terminal = config.WorkStack.Pop();
                                    config.InputStack.Push(terminal);
                                }
                        }
                        else
                        {
                            KeyValuePair<string, List<string>> lastProduction = config.WorkStack.Pop();
                            var productions = Grammar.GetProductionsForNonTerminal(lastProduction.Key);
                            var nextProduction = GetNextProduction(lastProduction, productions);
                            if(nextProduction.Key != null)
                            {
                                config.State = State.NORMAL;
                                config.WorkStack.Add(nextProduction);
                                var states = lastProduction.Value.Count;
                                while (states != 0) config.InputStack.Pop();
                                nextProduction.Value.ForEach(x => config.InputStack.Push(x));
                            }
                            else if(config.Index == 0 && lastProduction.Key == Grammar.Symbol)
                                config.State = State.ERROR;
                            else
                            {
                                if (lastProduction.Value.Count == 1 && lastProduction.Value.Contains("e"))
                                    config.InputStack.Push(lastProduction.Key);
                                else
                                {
                                    var states = lastProduction.Value.Count;
                                    while (states != 0) config.InputStack.Pop();
                                    config.InputStack.Push(lastProduction.Key);
                                }
                            }
                        }
                    }
                }
            }
            if(config.State == State.ERROR)
                Console.WriteLine("Error");
            else
                foreach(var prod in config.WorkStack.getForPrint())
                    if(Grammar.Productions.Contains(prod))
                        Console.WriteLine(prod);

        }

    }
}
