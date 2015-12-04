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

function entry(tree)
{
	return car(tree);
}

function leftBranch(tree)
{
	return cadr(tree);
}

function rightBranch(x)
{
	return caddr(x);
}

function makeTree(entry, left, right)
{
	return list(entry, left, right);
}

function elementOfSet(x, set)
{
	if(set == null)
	{
		return false;
	}
	
	if(x < entry(set))
	{
		return elementOfSet(x, leftBranch(set));
	}
	
	return elementOfSet(x, rightBranch(set));
}

function addjoinSet(x, set)
{
	if(set == null)
	{
		return makeTree(x, null, null);
	}
	
	if(x < entry(set))
	{
		return makeTree(entry(set), adjoinSet(x, leftBranch(set)), rightBranch(set));
	}
	
	return makeTree(entry(set), leftBranch(set), adjoinSet(x, rightBranch(set))); 
}

function treeToList1(tree)
{
	if(tree == null)
	{
		return null;
	}
	
	return append(treeToList1(leftBranch(tree)), 
		cons(entry(tree), treeToList1(rightBranch(tree))));
}

function treeToList2(tree)
{
	function copyToList(tree, resultList)
	{
		if(tree == null)
		{
			return resultList;
		}
		
		return copyToList(leftBranch(tree),
			cons(entry(tree), copyToList(rightBranch(tree), resultList)));
	}
	
	return copyToList(tree, null);
}

function length(x)
{
	if(x == null)
	{
		return 0;
	}

	return 1 + length(cdr(x));
}

function listToTree(elements)
{
	return car(partialTree(elements, length(elements)));
}
    
function partialTree(elts, n)
{
	if(n == 0)
	{
		return cons(null, elts);
	}
	
	var leftSize = Math.floor((n-1)/2);
	var leftResult = partialTree(elts, leftSize);
	var leftTree = car(leftResult);
	var nonLeftElts = cdr(leftResult);
	var rightSize = n - leftSize - 1;
	var thisEntry = car(nonLeftElts);
	var rightResult = partialTree(cdr(nonLeftElts), rightSize);
	var rightTree = car(rightResult);
	var remainingElts = cdr(rightResult);
	
	return cons(makeTree(thisEntry, leftTree, rightTree), remainingElts);
}

function unionList(set1, set2)
{
    if(set1 == null)
    {
        return set2;
    }
    
    if(set2 == null)
    {
    	return set1;
    }
    
    var x1 = car(set1);
    var x2 = car(set2);
    
    if(x1 == x2)
    {
        return cons(x1, unionList(cdr(set1), cdr(set2)));
    }
                    
    if(x1 < x2)
    {
        return cons(x1, unionList(cdr(set1), set2));
    }
            
    return cons(x2, unionList(set1, cdr(set2)));
}

function unionSet(set1, set2)
{
    var list1 = treeToList2(set1);
	var list2 = treeToList2(set2)
	
	var resultList = unionList(list1, list2);
	
	return listToTree(resultList);
}

var result = unionSet(listToTree(list(1,2,4)), listToTree(list(2,3,4,5)));
alert(result);