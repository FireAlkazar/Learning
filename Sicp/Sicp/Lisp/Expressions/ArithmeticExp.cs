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
            ArithmeticType = GetArithmeticType(arithmeticSign);
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

        public ArithmeticType ArithmeticType { get; set; }

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

        public override ExpressionType Type => ExpressionType.Arithmetic;
    }
}