function pair(x,y)
{
	return [x,y];
}

function isPair(x)
{
    return (x instanceof Array);
}

function cdr(p)
{
	return p ? p[0] : null;
}

function car(p)
{
	return p ? p[1] : null;
}

function list()
{
	var result = null;
    
    for(var i = arguments.length - 1; i >= 0 ;i--)
    {
    	result = pair(arguments[i], result);
    }
    
    return result;
}

function map(lambda, list )
{
    if(list == null)
    {
    	return null;
    }
    
	return pair(lambda(cdr(list)), map(lambda, car(list)));
}

function treeMap(lambda, tree)
{
	if(tree == null)
    {
    	return null;
    }
    
    if(isPair(tree) == false)
    {
    	return lambda(tree);
    }
     
    return pair(treeMap(lambda, cdr(tree)),
               treeMap(lambda, car(tree))
               );
}

var tree = list(list(5,1),2,3,4,5);
var result = treeMap(function(x){return x*x;}, tree);
alert(cdr(cdr(result)));