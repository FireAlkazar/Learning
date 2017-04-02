namespace Sicp.Lisp.Expressions
{
    public class IdentifierExp : Exp
    {
        public IdentifierExp(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }

        public override ExpressionType Type => ExpressionType.Identifier;
    }
}