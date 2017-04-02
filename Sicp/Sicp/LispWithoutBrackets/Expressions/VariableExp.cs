namespace Sicp.LispWithoutBrackets.Expressions
{
    public class VariableExp : Exp
    {
        public VariableExp(string variableName)
        {
            VariableName = variableName;
        }

        public string VariableName { get; set; }

        public override ExpressionType Type => ExpressionType.Variable;
    }
}