namespace Sicp.Lisp.Expressions
{
    public class DefineExp : Exp
    {
        public override ExpressionType Type => ExpressionType.Define;

        public bool IsVariable => Children[0].Children.Count == 0;

        public string IdentifierName => ((IdentifierExp) Children[0]).Name;
    }
}