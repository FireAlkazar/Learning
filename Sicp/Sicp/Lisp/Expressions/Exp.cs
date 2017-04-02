using System.Collections.Generic;

namespace Sicp.Lisp.Expressions
{
    public abstract class Exp
    {
        public List<Exp> Children { get; } = new List<Exp>();

        public abstract ExpressionType Type { get; }
        public virtual bool IsLeaf => false;
    }
}