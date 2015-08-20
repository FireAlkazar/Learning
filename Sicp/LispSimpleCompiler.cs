using System;

namespace Sicp
{
    public class LispSimpleCompiler
    {
        public static int Execute(string expression)
        {
            ParsedExpression parsedExpression = Parse(expression);

            return ExecuteCore(parsedExpression);
        }

        private static int ExecuteCore(ParsedExpression parsedExpression)
        {
            if (parsedExpression.Operator == "+")
            {
                return int.Parse(parsedExpression.Arg1) + int.Parse(parsedExpression.Arg2);
            }

            throw new NotImplementedException();
        }

        private static ParsedExpression Parse(string expression)
        {
            string[] parts = SplitEpression(expression);

            return new ParsedExpression
            {
                Operator = parts[0], Arg1 = parts[1], Arg2 = parts[2],
            };
        }

        private static string[] SplitEpression(string expression)
        {
            if (expression[0] != '(' || expression[expression.Length - 1] != ')')
            {
                throw new ArgumentException("Incorrect expression");
            }

            return expression.Substring(1, expression.Length - 2).Split(' ');
        }
    }

    internal sealed class ParsedExpression
    {
        public string Arg1 { get; set; }
        public string Arg2 { get; set; }
        public string Operator { get; set; }
    }
}
