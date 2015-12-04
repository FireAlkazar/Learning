function cons(x,y)
{
    return [x,y];
}

function isPair(x)
{
    return (x instanceof Array);
}

function car(p)
{
    return p ? p[0] : null;
}

function cdr(p)
{
    return p ? p[1] : null;
}

function list()
{
    var result = null;
    
    for(var i = arguments.length - 1; i >= 0 ;i--)
    {
        result = cons(arguments[i], result);
    }
    
    return result;
}

function map(lambda, list )
{
    if(list == null)
    {
        return null;
    }
    
    return cons(lambda(car(list)), map(lambda, cdr(list)));
}

function append(list1, list2)
{
    if(list1 == null)
    {
        return list2;
    }

    return cons(car(list1), append(cdr(list1), list2));
}

function filter(predicate, sequence)
{
    if(sequence == null)
    {
        return null;
    }
	
    if(predicate(car(sequence)))
    {
        return cons(car(sequence), filter(predicate, cdr(sequence)));
    }
	
    return filter(predicate, cdr(sequence));
}

function accumulate(op, initial, sequence)
{
    if(sequence == null)
    {
        return initial;
    }
	
    return op(car(sequence), accumulate(op, initial, cdr(sequence)));
}

function enumerateInterval(low, high)
{
    if(low > high)
    {
        return null;
    }
	
    return cons(low, enumerateInterval(low + 1, high));
}

function flatmap(proc, seq)
{
    return accumulate(append, null, map(proc, seq));
}

function remove(item, sequence)
{
    return filter(function(x){ 
        return x != item;
    }, 
		sequence);
}

function cadr(x)
{
    return car(cdr(x));
}

function caddr(x)
{
    return car(cdr(cdr(x)));
}

function cadddr(x)
{
    return car(cdr(cdr(cdr(x))));
}

function length(x)
{
    if(x == null)
    {
        return 0;
    }

    return 1 + length(cdr(x));
}

function installRaiseFunctions()
{
	function raiseSchemaNumber(n)
	{
		return makeRat(n, 1);
	}

	function raiseRat(r)
	{
		return makeComplexFromRealImag(r, 0);
	}
	
	put('raise', list('schemaNumber'), raiseSchemaNumber);
	put('raise', list('rational'), raiseRat);
}

var right = rightBranch(result);
alert(symbols(right) + ': ' + weight(right));