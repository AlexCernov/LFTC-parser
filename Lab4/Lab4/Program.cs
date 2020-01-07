using Lab4.Models;
using Lab4.Views;
using System;
using System.Collections.Generic;
using System.IO;
using Lab4.Controller;

namespace Lab4
{
    class Program
    {
        public static ProgramInternalForm pif = new ProgramInternalForm();
        public static SymbolTable symbolTableConstants = new SymbolTable();
        public static SymbolTable symbolTableIdentifiers = new SymbolTable();
        public static Grammar Grammar { get; set; }

        public static void Scanner()
        {

            Scanner scanner = new Scanner();
            Utils utils = new Utils();
            string filename = "program.txt";
            string[] lines = File.ReadAllLines(filename);

            int counter = 0;
            foreach (var line in lines)
            {
                counter++;
                char tab = '\u0009';
                string strippedLine = line.Replace(tab.ToString(), "");
                List<string> tokens = scanner.Tokenize(strippedLine, utils.separators);
                tokens.RemoveAll(x => x.Equals(""));
                int i = 0;
                while (i < tokens.Count)
                {
                    if (utils.everything.Contains(tokens[i]))
                    {
                        if (tokens[i] != " " && tokens[i] != "-")
                        {
                            pif.Add(scanner.codeTable[tokens[i]], -1);
                            i++;
                        }
                        else if (tokens[i] == " ")
                            i++;
                        else
                        {
                            pif.Add(scanner.codeTable[tokens[i]], -1);
                            i++;
                        }
                    }
                    else if (scanner.IsIdentifier(tokens[i]))
                    {
                        int? pos = symbolTableIdentifiers.Find(tokens[i]);
                        if (pos == null)
                        {
                            pos = symbolTableIdentifiers.Insert(tokens[i]);
                        }
                        pif.Add(scanner.codeTable["identifier"], pos.Value);
                        i++;
                    }
                    else if (scanner.IsConstant(tokens[i]))
                    {
                        tokens[i] = tokens[i].Replace("'", "");
                        if (!Int32.TryParse(tokens[i], out int numberResult))
                        {
                            int? pos = symbolTableConstants.Find(tokens[i][0]);
                            if (pos == null)
                            {
                                pos = symbolTableConstants.Insert(tokens[i][0]);
                            }
                            pif.Add(scanner.codeTable["constant"], pos.Value);
                            i++;
                        }
                        else
                        {
                            int? pos = symbolTableConstants.Find(numberResult);
                            if (pos == null)
                            {
                                pos = symbolTableConstants.Insert(numberResult);
                            }
                            pif.Add(scanner.codeTable["constant"], pos.Value);
                            i++;
                        }

                    }
                    else throw new Exception("Unknow token " + tokens[i] + "at line " + counter.ToString());
                }
            }
        }

        public static void InitiateGrammar()
        {
            Grammar = Grammar.ReadFromFile("grammar.txt");
        }

        static void Main(string[] args)
        {
            Scanner();
            InitiateGrammar();
            Controller.Controller.Grammar = Grammar;
            pif.CodeList().ForEach(x => Console.Write(x + " "));
            var pifString = new List<string>();
            foreach (var item in pif.CodeList())
            {
                pifString.Add(item.ToString());
            }
            Controller.Controller.Parser(pifString);
            Console.WriteLine("end");

        }
    }
}
