function pair(x,y)
{
	return [x,y];
}

function cons(p)
{
	return p ? p[0] : null;
}

function cars(p)
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

function revert(list)
{
	return revertIter(list, null);
}

function revertIter(list, result)
{
	if(!list)
    {
    	return result;
    }
    
    return revertIter(cars(list), pair(cons(list), result));
}

var li = list(1,2,3,4,5);
var result = revert(li);
alert(cons(result));