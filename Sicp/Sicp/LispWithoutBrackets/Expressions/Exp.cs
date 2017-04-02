using System.Collections.Generic;

namespace Sicp.LispWithoutBrackets.Expressions
{
    public abstract class Exp
    {
        public List<Exp> Children { get; } = new List<Exp>();

        public abstract ExpressionType Type { get; }
    }
}