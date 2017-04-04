using System;

namespace Sicp.Lisp.Expressions
{
    public enum ArithmeticType
    {
        Plus,
        Minus,
        Miltiply,
        Divide,
    }

    public class ArithmeticExp : Exp
    {
        public ArithmeticExp(string arithmeticSign)
        {
            ArithmeticSign = arithmeticSign;
            ArithmeticType = GetArithmeticType(arithmeticSign);
        }

        public string ArithmeticSign { get; set; }

        public ArithmeticType ArithmeticType { get; set; }

        public override ExpressionType Type => ExpressionType.Arithmetic;

        public Func<int,int,int> GetFunction()
        {
            switch (ArithmeticType)
            {
                case ArithmeticType.Plus:
                    return (x,y) => x + y;
                case ArithmeticType.Minus:
                    return (x, y) => x - y;
                case ArithmeticType.Miltiply:
                    return (x, y) => x * y;
                case ArithmeticType.Divide:
                    return (x, y) => x / y;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ArithmeticType GetArithmeticType(string arithmeticSign)
        {
            switch (arithmeticSign)
            {
                case "+": return ArithmeticType.Plus;
                case "-": return ArithmeticType.Minus;
                case "*": return ArithmeticType.Miltiply;
                case "/": return ArithmeticType.Divide;
                default:
                    throw new InvalidOperationException($"Неизвестный символ арифметической операции {arithmeticSign}");
            }
        }

        public bool IsUnaryMinus()
        {
            return ArithmeticType == ArithmeticType.Minus && Children.Count == 1;
        }
    }
}