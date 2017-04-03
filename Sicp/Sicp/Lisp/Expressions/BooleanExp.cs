using System;

namespace Sicp.Lisp.Expressions
{
    public class BooleanExp : Exp
    {
        public BooleanExp(string compareSign)
        {
            CompareSign = compareSign;
        }

        public string CompareSign { get; set; }

        public override ExpressionType Type => ExpressionType.Boolean;

        public Func<int,int,bool> GetFunction()
        {
            switch (CompareSign)
            {
                case "<":
                    return (x, y) => x < y;
                case ">":
                    return (x, y) => x > y;
                case "=":
                    return (x, y) => x == y;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}