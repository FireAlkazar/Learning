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


