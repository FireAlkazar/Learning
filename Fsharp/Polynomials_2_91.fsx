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

let rec divTerms l1 l2 =
    if isEmptyTermList l1 then [theEmptyTermList;theEmptyTermList]
    else
        let t1 = firstTerm l1
        let t2 = firstTerm l2
        if getOrder(t1) < getOrder(t2) then [[];l1]
        else
            let newC = (coeff t1) / (coeff t2)
            let newO = getOrder(t1) - getOrder(t2)
            let curTerm = makeTerm newO newC
            let restOfResult = divTerms (subTerms l1 (mulTerms [curTerm] l2)) l2
            let result = adjoinTerm curTerm restOfResult.[0]
            [result; restOfResult.[1]]

let m1 = makeTerm 3 1
let m2 = makeTerm 2 1
let c1 = makeTerm 0 -1
let m3 = addTerms [m2] [c1]

let mm = divTerms [m1] m3
printf "%A" mm

