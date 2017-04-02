using System;
using System.Collections.Generic;
using System.Linq;
using Sicp.Lisp.Expressions;

namespace Sicp.Lisp
{
    public class TreeTraverser
    {
        private readonly Dictionary<string, DefineExp> _globalContext = new Dictionary<string, DefineExp>();
        private int _lastResult;

        public void TraverseTree(List<Exp> tree)
        {
            foreach (var node in tree)
            {
                Traverse(node);
            }
        }

        public int GetLastResult()
        {
            return _lastResult;
        }

        private void Traverse(Exp exp)
        {
            switch (exp.Type)
            {
                case ExpressionType.Define:
                    _lastResult = TraverseDefine((DefineExp)exp);
                    break;
                case ExpressionType.Int:
                    _lastResult = ((IntExp) exp).Value;
                    break;
                case ExpressionType.Arithmetic:
                    _lastResult = TraverseArithmetic((ArithmeticExp)exp, _globalContext);
                    break;
                case ExpressionType.Identifier:
                    _lastResult = CalculateIdentifierValue(((IdentifierExp) exp), _globalContext);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private int TraverseArithmetic(ArithmeticExp exp, Dictionary<string, DefineExp> context)
        {
            Func<int, int, int> arithmeticFunction = exp.GetFunction();
            return exp
                .Children
                .Select(x => CalculateExp(x, context))
                .Aggregate((x,y) => arithmeticFunction(x,y));
        }

        private int CalculateExp(Exp exp, Dictionary<string, DefineExp> context)
        {
            if (exp.Type == ExpressionType.Int)
            {
                return ((IntExp)exp).Value;
            }
            else if (exp.Type == ExpressionType.Identifier)
            {
                return CalculateIdentifierValue((IdentifierExp)exp, context);
            }
            else if (exp.Type == ExpressionType.Arithmetic)
            {
                return TraverseArithmetic((ArithmeticExp)exp, context);
            }
            else
            {
                throw new InvalidOperationException($"Can't calculate expression");
            }
        }

        private int TraverseDefine(DefineExp exp)
        {
            _globalContext[exp.IdentifierName] = exp;

            return exp.IsVariable 
                ? CalculateExp(exp.Children[1], _globalContext)
                : 0;
        }

        private int CalculateIdentifierValue(IdentifierExp exp, Dictionary<string, DefineExp> context)
        {
            Dictionary<string, DefineExp> localContext = context
                .ToDictionary(x => x.Key, x => x.Value);

            if (localContext.ContainsKey(exp.Name) == false)
            {
                throw new InvalidOperationException($"В списке переменных не найдена переменная с именем {exp.Name}.");
            }

            if (exp.Children.Count == 0)
            {
                return CalculateExp(localContext[exp.Name].Children[1], context);
            }

            var defineExp = localContext[exp.Name];
            var functionSignature = (IdentifierExp)defineExp.Children[0];

            for (int argumentPosition = 0; argumentPosition < functionSignature.Children.Count; argumentPosition++)
            {
                var functionArgument = (IdentifierExp)functionSignature.Children[argumentPosition];
                var argumentSubstitution = new DefineExp();
                argumentSubstitution.Children.Add(functionArgument);
                argumentSubstitution.Children.Add(exp.Children[argumentPosition]);
                localContext[functionArgument.Name] = argumentSubstitution;
            }

            var functionBody = defineExp.Children[1];

            return CalculateExp(functionBody, localContext);
        }
    }
}