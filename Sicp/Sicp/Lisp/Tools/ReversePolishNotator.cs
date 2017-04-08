using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sicp.Lisp.Expressions;

namespace Sicp.Lisp.Tools
{
    public class ReversePolishNotator
    {
        private readonly StringBuilder _result = new StringBuilder();

        public string GetNotation(Exp exp)
        {
            WriteIter(exp);

            return _result
                .ToString()
                .Trim();
        }

        private void WriteIter(Exp exp)
        {

            foreach (var child in exp.Children)
            {
                WriteIter(child);
            }

            _result.Append(" " + GetStringValue(exp));
        }

        private string GetStringValue(Exp exp)
        {
            switch (exp.Type)
            {
                case ExpressionType.Double:
                    return ((DoubleExp)exp).Value.ToString();
                case ExpressionType.Arithmetic:
                    return ((ArithmeticExp)exp).ArithmeticSign;
                default:
                    return string.Empty;
            }
        }
    }
}
