using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Lab4.Models
{
    public class Scanner
    {

        private string PifFile;
        private string identifiersTableFile;
        private string constantsTableFile;
        public Dictionary<string, int> codeTable = new Dictionary<string, int>();
        private Utils utils = new Utils();



        public Scanner()
        {

            PifFile = "PIF.txt";
            identifiersTableFile = "identifiers.txt";
            constantsTableFile = "constants.txt";

            File.Create(PifFile);
            File.Create(identifiersTableFile);
            File.Create(constantsTableFile);

            populateCodeTable();

        }

        private bool containsOperator(string @operator)
        {
            foreach (var ch in utils.operators)
                if (ch.Contains(@operator))
                    return true;
            return false;
        }

        public bool IsIdentifier(string @identifier)
        {
            return Regex.IsMatch(identifier, "^[a-zA-Z]{1}[a-zA-Z0-9_]{0,127}$");
        }


        public bool IsConstant(string @constant)
        {
            return Regex.IsMatch(constant, @"^-?[1-9]{1}[0-9]*$|^'[a-zA-Z]{1}'$|0");
        }

        private KeyValuePair<string, int> tokenizeCharacters(string line, int index)
        {
            string token = "";
            int quote_count = 0;

            while (index < line.Length && quote_count < 2)
            {
                if (line[index] == '\'')
                    quote_count++;
                token += line[index];
                index++;

            }
            return new KeyValuePair<string, int>(token, index);
        }

        private KeyValuePair<string, int> tokenizeOperator(string line, int index)
        {
            string token = "";

            while (index < line.Length && containsOperator(line[index].ToString()) && containsOperator(token + line[index]))
            {
                token += line[index];
                index++;
            }

            return new KeyValuePair<string, int>(token, index);
        }


        public List<string> Tokenize(string line, List<char> separators)
        {
            string token = "";
            int index = 0;
            List<string> tokens = new List<string>();

            while (index < line.Length)
            {
                if (line[index] == '\'')
                {
                    if (token != null)
                        tokens.Add(token);
                    KeyValuePair<string, int> pair = tokenizeCharacters(line, index);
                    tokens.Add(pair.Key);
                    index = pair.Value;
                    token = "";
                }
                else if (containsOperator(line[index].ToString()))
                {
                    if (token != null)
                        tokens.Add(token);
                    KeyValuePair<string, int> pair = tokenizeOperator(line, index);
                    index = pair.Value;
                    tokens.Add(pair.Key);
                    token = "";
                }
                else if (utils.separators.Contains(line[index]))
                {
                    if (token != null)
                        tokens.Add(token);
                    tokens.Add(line[index].ToString());
                    index++;
                    token = "";
                }
                else
                {
                    token += line[index];
                    index++;
                }
            }


            if (token != null)
                tokens.Add(token);

            int indexOfMinus = tokens.IndexOf("-");
            if (indexOfMinus != -1)
            {
                if (Int32.TryParse(tokens[indexOfMinus + 1], out int number))
                {
                    tokens.Remove("-");
                    int numberToBeAdded = number * (-1);
                    tokens[indexOfMinus] = numberToBeAdded.ToString();
                }
            }

            return tokens;
        }

        private void populateCodeTable()
        {
            try
            {
                string[] lines = File.ReadAllLines("codeTable.txt");
                foreach (string line in lines)
                {
                    string[] pair = line.Split(" ");
                    if (pair[0].Equals("space")) codeTable.Add(" ", Int32.Parse(pair[1]));
                    else codeTable.Add(pair[0], Int32.Parse(pair[1]));
                }
            }
            catch (Exception e)
            {

                Console.WriteLine("File error " + e); ;
            }
        }

    }
}
