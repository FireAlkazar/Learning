namespace Sicp.LispWithoutBrackets.Expressions
{
    public class IntExp : Exp
    {
        public IntExp(int value)
        {
            Value = value;
        }

        public int Value { get; set; }

        public override ExpressionType Type => ExpressionType.Int;
    }
}