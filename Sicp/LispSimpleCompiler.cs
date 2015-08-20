using System;
using System.Collections.Generic;
using System.Text;

namespace Sicp
{
    public class LispSimpleCompiler
    {
        public static int Execute(string expression)
        {
            Expression parsedExpression = Parse(expression);

            return parsedExpression.Eval();
        }

        private static Expression Parse(string expression)
        {
            string[] parts = SplitEpression(expression);

            if (parts[0] == "+")
            {
                return new ArithmeticExpression
                {
                    Operator = parts[0],
                    Arg1 = Parse(parts[1]),
                    Arg2 = Parse(parts[2]),
                };
            }

            return new ConstExpression
            {
                Value = parts[0]
            };
        }

        private static List<string> ParseArgs(string substring)
        {
            int openCount = 0;
            var argsBuilder = new StringBuilder();
            var result = new List<string>();

            foreach (char symbol in substring)
            {
                if(symbol == '(')
                {
                    openCount++;
                }

                if(symbol == ')')
                {
                    openCount--;
                }

                if(symbol == ' ' && openCount == 0)
                {
                    if(argsBuilder.Length != 0)
                    {
                        result.Add(argsBuilder.ToString());
                    }
                    argsBuilder.Length = 0;
                }
                else
                {
                    argsBuilder.Append(symbol);    
                }
            }

            if (argsBuilder.Length != 0)
            {
                result.Add(argsBuilder.ToString());
            }

            return result;
        }

        private static string[] SplitEpression(string expression)
        {
            if (expression[0] == '(' && expression[expression.Length - 1] == ')')
            {
                expression = expression.Substring(1, expression.Length - 2);
            }

            List<string> args = ParseArgs(expression);

            return args.ToArray();
        }
    }

    public abstract class Expression
    {
        public abstract int Eval();
    }

    public sealed class ConstExpression : Expression
    {
        public string Value { get; set; }

        public override int Eval()
        {
            return int.Parse(Value);
        }
    }

    public sealed class ArithmeticExpression : Expression
    {
        public Expression Arg1 { get; set; }
        public Expression Arg2 { get; set; }
        public string Operator { get; set; }

        public override int Eval()
        {
            if (Operator == "+")
            {
                return Arg1.Eval() + Arg2.Eval();
            }

            throw new NotImplementedException();
        }
    }
}
