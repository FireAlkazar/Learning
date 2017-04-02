namespace Sicp.LispWithoutBrackets.Expressions
{
    public class DefineExp : Exp
    {
        public override ExpressionType Type => ExpressionType.Define;
    }
}