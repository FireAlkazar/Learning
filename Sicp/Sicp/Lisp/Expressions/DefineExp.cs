namespace Sicp.Lisp.Expressions
{
    public class DefineExp : Exp
    {
        public override ExpressionType Type => ExpressionType.Define;
    }
}