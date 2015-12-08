let set : int list = [1;2;3;4]

let rec remove list index =
    match index, list with
    | 0, x::xs -> xs
    | i, x::xs -> x::remove xs (i - 1) 
    | i, [] -> failwith "index out of range"

let rec parseOrder (list : 'a list) =
    match List.rev list with
    | [] -> ([],[])
    | [x] -> ([],[x])
    | [z;w] -> 
        if z > w then ([w],[z])
        else ([],[w;z])
    | h::t ->
        let m = List.head t
        if h > m then (List.rev t,[h])
        else 
            let (u,v) = parseOrder (List.rev t)
            (u, v@[h])

let rec printPermutations (x : int list) =
    printfn "%A" x
    let (untouched,tail) = parseOrder x
    if List.isEmpty untouched then 1 |> ignore
    else 
        let restLastElement = List.nth untouched ((List.length untouched) - 1)
        let reversedTail =  (List.rev tail)
        let swappedElementIndex = List.findIndex (fun x -> x > restLastElement) reversedTail
        let swappedElement = List.nth reversedTail swappedElementIndex
        let withoutSwappedElement = remove reversedTail swappedElementIndex
        let restWithoutLastElement = remove untouched ((List.length untouched) - 1)
        let nextList = restWithoutLastElement@[swappedElement]@(List.sort ([restLastElement]@withoutSwappedElement))
        printPermutations nextList

printPermutations set



//Get min length subsequence, sub sequence can be in any order
open System;

let getMinLengthFromStartElement given required = 
    if List.isEmpty given then -1
    else
        try
            let allIndexes = List.map (fun x -> (List.findIndex (fun y -> y = x) given) + 1) required
            List.max allIndexes
        with
            | _ -> -1

let rec getMinLength given required =
    if List.isEmpty given then -1
    else
        let ind1 = getMinLengthFromStartElement given required
        let ind2 = getMinLength (List.tail given) required
        if ind1 > -1 && ind2 > -1 then 
            Math.Min(ind1,ind2)
        else
            Math.Max(ind1,ind2)
 
getMinLength [1;1;3;2;1;1] [1;2;3] //gives 3
    


//For given N get the number costisting only of ones and zeros, divided by N (for N=7 its 1001)
open System.Collections.Generic
let getEncodedValue (encoded : int64) =
    let mutable res = 0L;
    let mutable order = 1L;
    let mutable shifted = encoded
    for shift in 0..63 do
        let bit = shifted &&& 1L
        res <- res + bit*order
        order <- order*10L
        shifted <- shifted >>> 1
    res

let getNumberOfOnesAndZeros n =
    let mutable curNumEncoded = 1L
    let mutable result = 0L
    while result = 0L do
        let value = getEncodedValue curNumEncoded
        if value % int64(n) = 0L then
            result <- value
        else 
            curNumEncoded <- curNumEncoded + 1L
    result

let n01 = getNumberOfOnesAndZeros 7

//Topological sort

let topologicalSort (graph : Map<string,string list>) = 
    let getZeroKeys (deg: System.Collections.Generic.Dictionary<string,int>) =
        deg.Keys 
        |> Seq.filter (fun x -> deg.[x] = 0)
        |> Seq.toList
    let getInDegree (graph : Map<string,string list>) (alreadyDone: string list) = 
        let inDegreeLocal = new System.Collections.Generic.Dictionary<string,int>()
        let doneAsSet = Set.ofList alreadyDone
        for x in graph do
            let curVertex = x.Key
            if doneAsSet.Contains curVertex then
                ()
            else
                inDegreeLocal.[curVertex] <- 
                    let curVertexSet = 
                        graph.[curVertex]
                        |> Set.ofList
                    Set.difference curVertexSet doneAsSet
                    |> Set.count
        inDegreeLocal

    let mutable result = []
    let mutable inDegree = getInDegree graph result
    while inDegree.Count > 0 do
        let noIn = getZeroKeys inDegree 
        result <- List.append result noIn
        printfn "%A" result
        inDegree <- getInDegree graph result
    result
let graphToSort = 
    Map.ofList [
        "a", ["b"]
        "c", ["a"]
        "b", []
    ]

let sortedGraph = topologicalSort graphToSort


