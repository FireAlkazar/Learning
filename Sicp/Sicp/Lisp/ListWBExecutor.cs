using System.Collections.Generic;
using Sicp.Lisp.Expressions;
using Sicp.Lisp.Tokens;

namespace Sicp.Lisp
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