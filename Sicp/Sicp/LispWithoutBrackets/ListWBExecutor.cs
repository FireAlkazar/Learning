using System.Collections.Generic;
using Sicp.LispWithoutBrackets.Expressions;
using Sicp.LispWithoutBrackets.Tokens;

namespace Sicp.LispWithoutBrackets
{
    public class ListWbExecutor
    {
         public int Execute(string program)
         {
             List<Token> tokens = new Tokenizer().Tokenize(program);
             List<Exp> exps = new Parser().Parse(tokens);
             var treeTraverser = new TreeTraverser();
             treeTraverser.TraverseTree(exps);

             return treeTraverser.GetLastResult();
         }
    }
}