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

function elementOfSet(x, set)
{
	if(set == null)
	{
		return false;
	}
	
	if(car(set) == x)
	{
		return true;
	}
	
	return elementOfSet(x, cdr(set));
}

function addjoinSet(x, set)
{
	return cons(x, set);
}

function intersectionSet(set1, set2)
{
	if(set1 == null || set2 == null)
	{
		return null;
	}
	
	if(elementOfSet(car(set1), set2))
	{
		return cons(car(set1), intersectionSet(cdr(set1), set2));
	}
	
	return intersectionSet(cdr(set1), set2);
}

function unionSet(set1, set2)
{
    return append(set1, set2);
}

var result = intersectionSet(list(1,2,4), list(2,3,4,5));
alert(result);