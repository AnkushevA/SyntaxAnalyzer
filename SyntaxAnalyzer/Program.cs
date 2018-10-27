using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace SyntaxAnalyzer
{
    
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var sr = new StreamReader(@"C:\Users\AHTOH\source\repos\SyntaxAnalyzer\inputFunction.txt"))
                {
                    FinalFunc(sr.ReadLine());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }

        static bool CalculateExpression(Token[] tokens)
        {
            Stack<Token> operandStack = new Stack<Token>();

            foreach (var token in tokens)
            {
                if (token.tokenType == Token.TokenType.Number)
                {
                    operandStack.Push(token);
                }
                else
                {
                    if (operandStack.Count < token.parameterCount)
                    {
                        throw new Exception("Error!");
                    }
                    List<Token> operands = new List<Token>();
                    for (int i = 0; i < token.parameterCount; i++)
                    {
                        operands.Add(operandStack.Pop());
                    }

                    operandStack.Push(EvaluateOperator(token, operands));
                }
            }
            if (operandStack.Count == 1)
            {
                return operandStack.Pop().value;
            }
            else
            {
                throw new Exception("Error!");
            }
        }

        static Token EvaluateOperator(Token oper, List<Token> operands)
        {
            switch (oper.symbol)
            {
                case '!':
                    operands[0].value = Not(operands[0].value);
                    break;
                case '&':
                    operands[0].value = And(operands[1].value, operands[0].value);
                    break;
                case 'V':
                    operands[0].value = Or(operands[1].value, operands[0].value);
                    break;
                case '>':
                    operands[0].value = Impl(operands[1].value, operands[0].value);
                    break;
                case '~':
                    operands[0].value = BiImpl(operands[1].value, operands[0].value);
                    break;
                case '+':
                    operands[0].value = SumModTwo(operands[1].value, operands[0].value);
                    break;
                case '|':
                    operands[0].value = Pierce(operands[1].value, operands[0].value);
                    break;
                default:
                    throw new Exception($"Operand {oper.symbol} not defined."); 
            }
            operands[0].symbol = operands[0].value == false ? '0' : '1';

            return operands[0];
        }
        static string[] SplitForTokenize(string str)
        {
            str = str.Replace("!", " ! ");
            str = str.Replace("&", " & ");
            str = str.Replace("V", " V ");
            str = str.Replace(">", " > ");
            str = str.Replace("~", " ~ ");
            str = str.Replace("+", " + ");
            str = str.Replace("|", " | ");
            str = str.Replace("(", " ( ");
            str = str.Replace(")", " ) ");
            while (str.Contains("  "))
            {
                str = str.Replace("  ", " ");
            }
            str = str.Trim();
            return str.Split(' ');
        }


        static Token[] Tokenize(string str)
        {
            string[] split = SplitForTokenize(str);
            List<Token> tokens = new List<Token>();

            foreach (string s in split)
            {
                tokens.Add(Token.StringToToken(s));
            }

            return tokens.ToArray();
        }

        static void FinalFunc(string func)
        {
            Console.WriteLine(func);
            Console.WriteLine();

            var permutations = GetPermutations(4);

            foreach (var permutation in permutations)
            {
                var funcWithReplacement = func.Replace('x', permutation[0]).Replace('y', permutation[1]).Replace('z', permutation[2]).Replace('k', permutation[3]);
                Console.WriteLine($"{permutation[0]} {permutation[1]} {permutation[2]} {permutation[3]}\t{CalculateExpression(ShuntingYard(Tokenize(funcWithReplacement)))}");
            }
            return;
        }

        static Token[] ShuntingYard(Token[] tokens)
        {
            Queue<Token> outputQueue = new Queue<Token>();
            Stack<Token> operatorStack = new Stack<Token>();

            foreach (var token in tokens)
            {
                if (token.tokenType == Token.TokenType.Number)
                {
                    outputQueue.Enqueue(token);
                }
                else if (token.tokenType == Token.TokenType.Operator)
                {
                    while (operatorStack.Count > 0)
                    {
                        var o2 = operatorStack.Peek();
                        if ((token.precedence < o2.precedence || (o2.assoc == Token.Associativity.Left && token.precedence <= o2.precedence)) 
                            && o2.tokenType != Token.TokenType.LeftBrace)
                        {
                            outputQueue.Enqueue(operatorStack.Pop());
                        }
                        else
                        {
                            break;
                        }
                    }
                    operatorStack.Push(token);
                }
                else if (token.tokenType == Token.TokenType.LeftBrace)
                {
                    operatorStack.Push(token);
                }
                else if (token.tokenType == Token.TokenType.RightBrace)
                {
                    while (operatorStack.Peek().tokenType != Token.TokenType.LeftBrace)
                    {
                        outputQueue.Enqueue(operatorStack.Pop());
                    }

                    if (operatorStack.Pop().tokenType != Token.TokenType.LeftBrace)
                    {
                        throw new Exception("No matching right parenthesis.");
                    }
                    
                }
            }

            while (operatorStack.Count > 0)
            {
                var top = operatorStack.Pop();
                if (top.tokenType == Token.TokenType.LeftBrace || top.tokenType == Token.TokenType.RightBrace)
                {
                    throw new Exception("Error!");
                }
                outputQueue.Enqueue(top);
            }
            return outputQueue.ToArray();
        }


        //основные логические функции
        static bool And(bool firstBool, bool secondBool)
        {
            if (firstBool == true && secondBool == true)
            {
                return true;
            }

            return false;
        }

        static bool Or(bool firstBool, bool secondBool)
        {
            if (firstBool == true || secondBool == true)
            {
                return true;
            }

            return false;
        }

        static bool BiImpl(bool firstBool, bool secondBool)
        {
            var firstInt = System.Convert.ToInt32(firstBool);
            var secondInt = System.Convert.ToInt32(secondBool);

            if ((firstBool == true && secondBool == true) || (firstBool == false && secondBool == false))
            {
                return true;
            }

            return false;
        }

        static bool Impl(bool firstBool, bool secondBool)
        {
            if (firstBool == true && secondBool == false)
            {
                return false;
            }

            return true;
        }

        static bool SumModTwo(bool firstBool, bool secondBool) => !BiImpl(firstBool, secondBool);

        static bool Sheffer(bool firstBool, bool secondBool)
        {
            if (firstBool == true && secondBool == true)
            {
                return false;
            }

            return true;
        }

        static bool Pierce(bool firstBool, bool secondBool)
        {
            if (firstBool == false && secondBool == false)
            {
                return true;
            }

            return false;
        }

        static bool Not(bool inputBool) => inputBool == true ? false : true;

        static List<string> GetPermutations(int numberOfVariables)
        {
            var numberOfPermutations = System.Convert.ToInt32(Math.Pow(2, numberOfVariables));
            var inMind = false;
            var temp = String.Concat(Enumerable.Repeat("0", numberOfVariables));
            var resList = new List<string>(numberOfPermutations);

            for (int i = 0; i < numberOfPermutations; i++)
            {
                resList.Add(temp);
                temp = PlusOne(temp);
            }

            return resList;
        }

        static string PlusOne(string a)
        {
            var inMind = false;
            var res = "";
            var b = String.Concat(Enumerable.Repeat("0", a.Length - 1)) + "1";
            for (int i = a.Length - 1; i >= 0; i--)
            {
                if (a[i] == '0' && b[i] == '0' && !inMind)
                {
                    res = "0" + res;
                }
                else if (((a[i] == '0' && b[i] == '1') || (a[i] == '1' && b[i] == '0')) && !inMind)
                {
                    res = "1" + res;
                }
                else if ((a[i] == '1' && b[i] == '1') && !inMind)
                {
                    res = "0" + res;
                    inMind = true;
                }
                else if (a[i] == '0' && b[i] == '0' && inMind)
                {
                    res = "1" + res;
                    inMind = false;
                }
                else if (((a[i] == '0' && b[i] == '1') || (a[i] == '1' && b[i] == '0')) && inMind)
                {
                    res = "0" + res;
                }
                else if ((a[i] == '1' && b[i] == '1') && inMind)
                {
                    res = "1" + res;
                }
            }

            if (inMind)
            {
                res = "1" + res;
            }
            return res;
        }
    }
}
