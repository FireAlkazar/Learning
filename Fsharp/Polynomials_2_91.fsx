let getOrder term = List.nth term 0
let coeff term = List.nth term 1

let adjoinTerm term termList =
    if coeff term = 0 then
        termList
    else
        term::termList

let theEmptyTermList = []
let firstTerm termList = List.nth termList 0
let restTerms termList = List.tail termList
let isEmptyTermList termList = List.length termList = 0
let makeTerm order coeff = [order;coeff]



let rec addTerms l1 l2 = 
    if isEmptyTermList l1 then l2
    elif isEmptyTermList l2 then l1
    else
        let t1 = firstTerm l1
        let t2 = firstTerm l2
        if getOrder(t1) > getOrder(t2) then adjoinTerm t1 (addTerms (restTerms l1) l2)
        elif getOrder(t1) < getOrder(t2) then adjoinTerm t2 (addTerms l1 (restTerms l2))
        else adjoinTerm (makeTerm (getOrder t1) ((coeff t1) + (coeff t2))) (addTerms (restTerms l1) (restTerms l2))

let rec mulTermByAllTerms t1 L =
    if isEmptyTermList L then theEmptyTermList
    else
        let t2 = firstTerm L
        adjoinTerm (makeTerm (getOrder(t1) + getOrder(t2)) (coeff(t1)*coeff(t2))) (mulTermByAllTerms t1 (restTerms L))

let rec mulTerms L1 L2 = 
    if isEmptyTermList L1 then theEmptyTermList
    else addTerms (mulTermByAllTerms (firstTerm L1) L2) (mulTerms (restTerms L1) L2)

let mulOnMinusOne L =
    mulTerms L [makeTerm 0 -1]

let subTerms L1 L2 =
    addTerms L1 (mulOnMinusOne L2)

let m1 = makeTerm 3 1
let m2 = makeTerm 2 1

let mm = subTerms [m1] [m2]
printf "%A" mm


//let term1 = makeTerm 3 1
//let terms = adjoinTerm (makeTerm 4 1) [(makeTerm 3 3)]
//let res = addTerms [term1] terms
//printfn "%A" res
//
//let makePoly var terms = (var,terms)
//let getVariable poly = 
//    let (a,b) = poly
//    a
//let getTermList poly = 
//    let (a,b) = poly
//    b
//let isVariable v = System.Char.IsLetter(v)
//let isSameVariable (v1:char, v2:char) = v1 = v2
//
//let rec divTerms l1 l2 =
//    if isEmptyTermList l1 then (theEmptyTermList,theEmptyTermList)
//    else
//        let t1 = firstTerm l1
//        let t2 = firstTerm l2
//        if getOrder(t1) > getOrder(t2) then ([],l1)
//        else
//            let newC = (coeff t1) / (coeff t2)
//            let newO = getOrder(t1) - getOrder(t2)
//            let restOfResult = divTerms 
//
//makePoly 'x' terms

