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

function append(list1, list2)
{
	if(list1 == null)
	{
		return list2;
	}

	return pair(cdr(list1), append(car(list1), list2));
}

function subsets(set)
{
	if(set == null)
    {
    	return list(null);
    }
 	   
    var subsetsWithoutFirst = subsets(car(set));
    var rest = map(function(x){ return pair(cdr(set), x)}, subsetsWithoutFirst);
    return append(subsetsWithoutFirst, rest);
}


var result = subsets(list(1,2,3));
alert(result);