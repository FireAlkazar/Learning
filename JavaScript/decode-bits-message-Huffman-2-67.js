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

function length(x)
{
    if(x == null)
    {
        return 0;
    }

    return 1 + length(cdr(x));
}

function makeLeaf(symbol, weight)
{
    return list('leaf', symbol, weight);
}

function isLeaf(object)
{
    return car(object) == 'leaf';
}

function symbolLeaf(x)
{
    return cadr(x);
}

function weightLeaf(x)
{
    return caddr(x);
}

function makeCodeTree(left, right)
{
    return list(
		left,
		right,
		append( 
			symbols(left), 
			symbols(right),
			weight(left) + weight(right)));
}

function leftBranch(tree)
{
    return car(tree);
}

function rightBranch(tree)
{
    return cadr(tree);
}

function symbols(tree)
{
    if(isLeaf(tree))
    {
        return symbolLeaf(tree);
    }
	
    return caddr(tree);
}

function weight(tree)
{
    if(isLeaf(tree))
    {
        return weightLeaf(tree);
    }
	
    return car(caddr(tree));
}

function decode(bits, tree)
{
    function decode1(bits, currentBranch)
    {
        if(bits == null)
        {
            return null;
        }
		
        var nextBranch = chooseBranch(car(bits), currentBranch);
        if(isLeaf(nextBranch))
        {
            return cons(symbolLeaf(nextBranch), decode1(cdr(bits), tree));
        }
		
        return decode1(cdr(bits), nextBranch);
    }
	
    return decode1(bits, tree);
}

function chooseBranch(bit, branch)
{
    if(bit == 0)
    {
        return leftBranch(branch);
    }
    else if(bit == 1)
    {
        return rightBranch(branch);
    }
	
    error('Wrong bit!');
}

function adjoinSet(x, set)
{
    if(set == null)
    {
        return list(x);
    }
	
    if(weight(x) < weight(car(set)))
    {
        return cons(x, set);
    }
	
    return cons(car(set), adjoinSet(x, cdr(set)));
}

function makeLeafSet(pairs)
{
    if(pairs == null)
    {
        return null;
    }
	
    var pair = car(pairs);
	
    return adjoinSet(
		makeLeaf(car(pair), cadr(pair)),
		makeLeafSet(cdr(pairs)));
}

var sampleTree = makeCodeTree(
	makeLeaf('A',4),
	makeCodeTree(
		makeLeaf('B', 2),
		makeCodeTree(
			makeLeaf('D',1),
			makeLeaf('C', 1))));
			
var sampleMessage = list(0, 1, 1, 0, 0, 1, 0, 1, 0, 1, 1, 1, 0);

var result = decode(sampleMessage, sampleTree);
alert(result);