namespace Sicp.Lisp.Expressions
{
    public class FuncExp : Exp
    {
        public override ExpressionType Type => ExpressionType.Func;
    }
}