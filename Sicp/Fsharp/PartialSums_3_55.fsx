let integers = Seq.initInfinite (fun x -> x + 1)

let rec partialSums s = 
    let rec parSums = seq { yield Seq.head s; yield! Seq.initInfinite (fun x -> (parSums |> Seq.nth x ) + (s |> Seq.nth (x + 1) )) }
    parSums

let integerPartialSums = partialSums integers

let rez = integerPartialSums |> Seq.take 5 |> Seq.toList
