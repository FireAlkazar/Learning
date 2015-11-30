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
let getNext (seq : ResizeArray<int>) =
    let mutable needTransfer = true
    let res = new ResizeArray<int>()
    for i in 0 .. (seq.Count - 1) do
        let curBit = seq.[i]
        if needTransfer then
            if curBit = 0 then
                res.Add(1)
                needTransfer <- false
            else
                res.Add(0)
        else
            res.Add(curBit)
    if needTransfer then
        res.Add(1)
    res
let getCurNumValue seq =
    let mutable res = 0;
    let mutable order = 1;
    for bit in seq do
        res <- res + bit*order
        order <- order*10
    res
let getNumberOfOnesAndZeros n =
    let mutable curNumSeq = new ResizeArray<int>([1])
    let mutable result = 0
    while result = 0 do
        let value = getCurNumValue curNumSeq
        if value % n = 0 then
            result <- value
        else 
            curNumSeq <- getNext curNumSeq
    result

let n01 = getNumberOfOnesAndZeros 17