using System;
using System.Collections.Generic;
using System.Linq;
using Sicp.Lisp.Expressions;

namespace Sicp.Lisp
{
    public class TreeTraverser
    {
        private readonly Dictionary<string, int> _variables = new Dictionary<string, int>();
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
                    TraverseDefine((DefineExp)exp);
                    break;
                case ExpressionType.Int:
                    break;
                case ExpressionType.Plus:
                    TraversePlus((PlusExp)exp);
                    break;
                case ExpressionType.Variable:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void TraversePlus(PlusExp exp)
        {
            _lastResult = exp.Children.Sum(childExp => CalculateExp(childExp));
        }

        private int CalculateExp(Exp exp)
        {
            if (exp.Type == ExpressionType.Int)
            {
                return ((IntExp)exp).Value;
            }
            else if (exp.Type == ExpressionType.Variable)
            {
                string variableName = ((VariableExp)exp).VariableName;
                return GetVariableValue(variableName);
            }
            else
            {
                throw new InvalidOperationException($"Can't calculate expression");
            }
        }

        private void TraverseDefine(DefineExp exp)
        {
            var variable = (VariableExp)exp.Children[0];
            var valueExp = exp.Children[1];

            if (valueExp.Type == ExpressionType.Int)
            {
                SetVariable(variable.VariableName, ((IntExp)valueExp).Value);
            }
            else if (valueExp.Type == ExpressionType.Variable)
            {
                string variableName = ((VariableExp)valueExp).VariableName;
                SetVariable(variable.VariableName, GetVariableValue(variableName));
            }
            else
            {
                throw new InvalidOperationException($"Incorrect define statement");
            }
        }

        private void SetVariable(string name, int value)
        {
            _variables[name] = value;
        }

        private int GetVariableValue(string name)
        {
            if (_variables.ContainsKey(name) == false)
            {
                throw new InvalidOperationException($"В списке переменных не найдена переменная с именем {name}.");
            }

            return _variables[name];
        }
    }
}