function pair(x,y)
{
	return [x,y];
}

function isPair(x)
{
    return (x instanceof Array);
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

function makeMobile(left, right)
{
	return list(left, right);
}

function makeBranch(length, structure)
{
	return list(length, structure);
}

function getLeftBranch(b)
{
	return cons(b);
}

function getRightBranch(b)
{
	return cons(cars(b));
}

function getBranchLength(b)
{
	return cons(b);
}

function getBranchStructure(b)
{
	return cons(cars(b));
}

function getTotalWeight(m)
{
	return getTotalWeightBranch(getLeftBranch(m)) +
        getTotalWeightBranch(getRightBranch(m));
}

function isMobile(s)
{
	return getLeftBranch(s) != null;
}

function getTotalWeightBranch(x)
{
    var s = getBranchStructure(x);
	return isMobile(s) ? getTotalWeight(s) : s;
}
function getMoment(b)
{
	return getBranchLength(b)*getTotalWeightBranch(b);
}

function isBalanced(m)
{
    var left = getLeftBranch(m);
    var right = getRightBranch(m);
    if(getMoment(left) != getMoment(right))
    {
    	return false;
    }
    
	return isBalancedBranch(left) && isBalancedBranch(right);
}

function isBalancedBranch(b)
{
	var s = getBranchStructure(b);
    return isMobile(s) ? isBalanced(s) : true;
}

var subMobile = makeMobile(makeBranch(1, 5), makeBranch(1, 5));
var mobile = makeMobile(makeBranch(2, 5), makeBranch(1, subMobile));
var totalWeight = getTotalWeight(mobile);
var balanced = isBalanced(mobile);
alert(balanced);