namespace Sicp.Lisp.Expressions
{
    public class VariableExp : Exp
    {
        public VariableExp(string variableName)
        {
            VariableName = variableName;
        }

        public string VariableName { get; private set; }

        public override ExpressionType Type => ExpressionType.Variable;
        public override bool IsLeaf => true;
    }
}