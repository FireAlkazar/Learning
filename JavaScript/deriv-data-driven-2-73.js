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

function deriv(exp, v)
{
	if(exp is Integer)
	{
		return 0;
	}
	
	if(exp is String)
	{
		return (exp == v) ? 1 : 0;
	}
	
	return get('deriv', operator(exp))(operands(exp), v);
}

function operator(exp){return car(exp);}
function operands(exp){return cdr(exp);}

function installDerivPackage()
{
	function derivSum(args, v)
	{
		var arg1 = car(args);
		var arg2 = cadr(args);
		
		return makeSum(deriv(arg1, v), deriv(arg2,v));
	}
	
	function derivProduct(args, v)
	{
		var arg1 = car(args);
		var arg2 = cadr(args);
		
		return makeSum(
				makeProduct(deriv(arg1, v), arg2),
				makeProduct(arg1, deriv(arg1,v)));
	}
	
	put('deriv', '+', derivSum);
	put('deriv', '*', derivProduct);
	
	//g.
	//put('+', 'deriv', derivSum);
	//put('*', 'deriv', derivProduct);
}
		

var right = rightBranch(result);
alert(symbols(right) + ': ' + weight(right));