namespace Sicp.Lisp.Expressions
{
    public class DoubleExp : Exp
    {
        public DoubleExp(double value)
        {
            Value = value;
        }

        public double Value { get; set; }

        public override ExpressionType Type => ExpressionType.Double;
    }
}