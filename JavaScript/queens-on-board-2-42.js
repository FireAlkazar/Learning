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

function queens(boardSize)
{
	function queensCol(k)
    {
    	if(k == 0)
        {
        	return list(emptyBoard());
        }
        
        return filter(function (positions)
                        {
                            return isPositionSafe(k, positions);
                        },
                        flatmap(function (restOfQueens)
                                {
                                    return map(function (newRow)
                                                {
                                                    return adjoinPosition(newRow, k, restOfQueens)
                                                },
                                                enumerateInterval(1, boardSize));

                                },
                                queensCol(k - 1)));
    }
    
    function isPositionSafe(k, positions)
    {
    	if(positions == null)
        {
        	return false;
        }
        
        var curPosition = car(positions);
        var otherPositions = cdr(positions);
        
        return false == accumulate(function(x,y)
                          	{
            					return x | y;
        					}, 
                          	false, 
                          	map(function(position)
                               		{
            							var row1 = getPositionRow(position);
            							var row2 = getPositionRow(curPosition);
            
            							var col1 = getPositionColumn(position);
            							var col2 = getPositionColumn(curPosition);
            
            							return (row1 == row2 ) ||
                                            (col1 == col2) ||
                                            ((col2 - col1) == (row2 - row1)) ||
                                            ((col2 - col1) == (row1 - row2));
        							},
                               		otherPositions));
    }
    
    function adjoinPosition(newRow, k, restOfQueens)
    {
    	return append(list(makePosition(k, newRow)), restOfQueens);
    }
    
    function makePosition(col,row)
    {
    	return cons(col,row);
    }
    
    function getPositionRow(p)
    {
    	return cdr(p);
    }
    
    function getPositionColumn(p)
    {
    	return car(p);
    }
    
    function emptyBoard()
    {
    	return null;
    }
    
    return queensCol(boardSize);
}


var result = queens(4);
alert(result);