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

function permutations(s)
{
	if(s == null)
	{
		return list(null);
	}
	
	return flatmap(function (x){
		return map(function(p){
				return cons(x,p)
			}, 
			permutations(remove(x,s)))
		}, 
		s);
}

function makeAllPairs(n)
{
	var interval = enumerateInterval(1,n);
    
    var allPairs = map(function (i)
        {
        	return map(function (j)
            {
                return list(i,j);
            }, 
            enumerateInterval(1,i - 1));
        }, 
        interval);
    
    return flatmap(function(x){return x;}, allPairs);
}

function makeAllTriples(n)
{
    var allTriples = map(function (i)
        {
        	return map(function (j)
            {
                return map(function (k)
                {
                	return list(k,j,i);
                }, 
                enumerateInterval(1,j - 1));
            }, 
            enumerateInterval(1,i - 1));
        }, 
        enumerateInterval(1,n));
    
    return flatmap(function(x){ return x;}, flatmap(function(x){ return x;}, allTriples));
}

function getTriplesComposingSum(n, sum)
{
	return filter(function(x)
                    {
    					var triplesSum = accumulate(function(y,z)
                                          	{
                        						return y + z;
                        					},
                                            0,
                                         	x);
    					return triplesSum == sum;
					}, 
                    makeAllTriples(n));
}

var result = getTriplesComposingSum(7, 15);
alert(result);