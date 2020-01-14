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

        public static void Parser(List<string> sequence)
        {
            Configuration config = new Configuration(Grammar.Symbol);
            while( config.State != State.FINAL && config.State != State.ERROR )
            {
                if(config.State == State.NORMAL)
                {
                    // Conditia de oprire: am terminat de procesat
                    if (config.InputStack.Count == 0 && config.Index == sequence.Count)
                        config.State = State.FINAL;
                    else if (config.InputStack.Count == 0)
                        config.State = State.ERROR;
                    else
                    {
                        // Daca pozitia de top din inputStack e nonTerminal
                        if(Grammar.NonTerminals.Contains(config.InputStack.Peek()))
                        {
                            string nonTerminal = config.InputStack.Pop();
                            var firstProduction = Grammar.GetProductionsForNonTerminal(nonTerminal)[0];
                            // adaugam first set of productions in workingstack
                            config.WorkStack.Add(firstProduction);
                            // adaugam terminalii si starile pentru prima productie in inputStack pentru procesare
                            config.InputStack.Push(firstProduction.Value);

                        }
                        // Daca pozitia de top din inputStack e terminal
                        else
                        {
                            // Daca a terminat de parcurs pif-ul
                            if (config.Index == sequence.Count)
                                config.State = State.BACK;

                            // daca intalneste epsilon
                            else if (config.InputStack.Peek().Equals("e"))
                            {
                                config.WorkStack.Add("e");
                                config.InputStack.Pop();
                            }
                            // verifica daca terminalul se regaseste in pif
                            else if (config.InputStack.Peek().Equals(sequence[config.Index]))
                            {
                                // Trecem la urmatorul element
                                config.Index++;
                                // Adaugam terminalul in workStack
                                config.WorkStack.Add(config.InputStack.Pop());
                               
                            }
                            // Altfel trecem la urmatorul element din workStack
                            else
                                config.State = State.BACK;
                        }
                    }
                }
                else
                {
                    if(config.State == State.BACK)
                    {
                        // Daca e terminal ( numai terminali sunt stringuri in workStack)
                        if (config.WorkStack.Peek().GetType() == typeof(string))
                        {
                            // ..
                            if (Grammar.Terminals.Contains(config.WorkStack.Peek()))
                                // daca e epsilon il scoatem si trecem mai departe
                                if (config.WorkStack.Peek().Equals("e"))
                                    config.WorkStack.Pop();
                                else
                                {
                                    // daca nu ne intoarcem cu o pozitie in pif
                                    config.Index--;
                                    string terminal = config.WorkStack.Pop();
                                    // si adaugam terminalul in inputstack pentru verificare
                                    config.InputStack.Push(terminal);
                                }
                        }
                        else
                        {
                            // daca e un production
                            KeyValuePair<string, List<string>> lastProduction = config.WorkStack.Peek();
                            // cautam toate productions pentru nonterminalul din lhs of lastProduction
                            var productions = Grammar.GetProductionsForNonTerminal(lastProduction.Key);
                            var nextProduction = GetNextProduction(lastProduction, productions);
                            // vedem daca am procesat toate productions pentru nonterminalul curent
                            if (nextProduction.Key != null)
                            {
                                // daca nu, atunci adaugam productia care urmeaza in workStack
                                config.State = State.NORMAL;
                                config.WorkStack.Pop();
                                config.WorkStack.Add(nextProduction);
                                // stergem atatea elemente din inputStack cat numarul elementelor din set of production
                                var states = lastProduction.Value.Count;
                                while (states != 0) { states--; config.InputStack.Pop();  }
                                // adaugam set of production to inputStack
                                config.InputStack.Push(nextProduction.Value);
                            }
                            else if(config.Index == 0 && lastProduction.Key == Grammar.Symbol)
                                config.State = State.ERROR;
                            else
                            {
                                config.WorkStack.Pop();
                                if (lastProduction.Value.Count == 1 && lastProduction.Value.Contains("e"))
                                    config.InputStack.Push(lastProduction.Key);
                                else
                                {
                                    var states = lastProduction.Value.Count;
                                    while (states != 0) { states--; config.InputStack.Pop();  }
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
