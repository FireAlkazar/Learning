using System;
using System.Collections.Generic;
using System.Linq;
using Sicp.Lisp.Expressions;

namespace Sicp.Lisp
{
    public class TreeTraverser
    {
        private readonly Dictionary<string, DefineExp> _globalContext = new Dictionary<string, DefineExp>();
        private double _lastResult;

        public void TraverseTree(List<Exp> tree)
        {
            foreach (var node in tree)
            {
                Traverse(node);
            }
        }

        public double GetLastResult()
        {
            return _lastResult;
        }

        private void Traverse(Exp exp)
        {
            switch (exp.Type)
            {
                case ExpressionType.Define:
                    _lastResult = ExecuteDefine((DefineExp)exp);
                    break;
                case ExpressionType.Double:
                case ExpressionType.Arithmetic:
                case ExpressionType.Boolean:
                case ExpressionType.Identifier:
                case ExpressionType.If:
                    _lastResult = CalculateExp(exp, _globalContext);
                    break;
                case ExpressionType.Func:
                    Exp composedExpression = ComposeExpression((FuncExp) exp, _globalContext);
                    _lastResult = CalculateExp(composedExpression, _globalContext);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private Exp ComposeExpression(FuncExp exp, Dictionary<string, DefineExp> context)
        {
            Exp funcToEvaluate = exp.Children[0];
            if (funcToEvaluate is IfExp)
            {
                Exp composedExp = CalculateIf((IfExp) funcToEvaluate, context);
                composedExp.Children.AddRange(exp.Children.Skip(1));
                return composedExp;
            }

            throw new NotImplementedException("ComposeExpression only supports if right now");
        }

        private int CalculateBoolean(BooleanExp exp, Dictionary<string, DefineExp> context)
        {
            Func<double, double, bool> compareFunction = exp.GetFunction();
            var left = CalculateExp(exp.Children[0], context);
            var right = CalculateExp(exp.Children[1], context);

            return compareFunction(left, right) ? 1 : 0;
        }

        private double CalculateArithmetic(ArithmeticExp exp, Dictionary<string, DefineExp> context)
        {
            if (exp.IsUnaryMinus())
            {
                return -(CalculateExp(exp.Children[0], context));
            }

            Func<double, double, double> arithmeticFunction = exp.GetFunction();
            return exp
                .Children
                .Select(x => CalculateExp(x, context))
                .Aggregate((x,y) => arithmeticFunction(x,y));
        }

        private double CalculateExp(Exp exp, Dictionary<string, DefineExp> context)
        {
            if (exp.Type == ExpressionType.Double)
            {
                return ((DoubleExp)exp).Value;
            }
            else if (exp.Type == ExpressionType.Identifier)
            {
                return CalculateIdentifierValue((IdentifierExp)exp, context);
            }
            else if (exp.Type == ExpressionType.Arithmetic)
            {
                return CalculateArithmetic((ArithmeticExp)exp, context);
            }
            else if (exp.Type == ExpressionType.Boolean)
            {
                return CalculateBoolean((BooleanExp)exp, context);
            }
            else if (exp.Type == ExpressionType.If)
            {
                var ifResultExp = CalculateIf((IfExp) exp, context);
                return CalculateExp(ifResultExp, context);
            }
            else
            {
                throw new InvalidOperationException($"Can't calculate expression");
            }
        }

        private Exp CalculateIf(IfExp exp, Dictionary<string, DefineExp> context)
        {
            Exp predicate = exp.Children[0];
            Exp then = exp.Children[1];
            Exp @else = exp.Children[2];

            return CalculateExp(predicate, context) > 0
                ? then
                : @else;
        }

        private double ExecuteDefine(DefineExp exp)
        {
            _globalContext[exp.IdentifierName] = exp;

            return exp.IsVariable 
                ? CalculateExp(exp.Children[1], _globalContext)
                : 0;
        }

        private double CalculateIdentifierValue(IdentifierExp exp, Dictionary<string, DefineExp> context)
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