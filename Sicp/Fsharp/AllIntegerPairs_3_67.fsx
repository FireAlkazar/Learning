let integers = Seq.initInfinite (fun x -> x + 1)

let tail x = Seq.skip 1 x

let rec interleave s1 s2 s3 =
    seq { 
        yield Seq.head s1;
        yield Seq.head s2;
        yield Seq.head s3;
        yield! interleave (tail s2) (tail s3) (tail s1)
    }

let rec pairs s1 s2 =
    let horSeq = Seq.initInfinite (fun x -> (s1 |> Seq.nth (x + 1), Seq.head s2))
    let verSeq = Seq.initInfinite (fun x -> (Seq.head s1, s2 |> Seq.nth (x + 1)))
    seq { 
        yield (Seq.head s1, Seq.head s2);
        yield! interleave horSeq verSeq (pairs (tail s1) (tail s2))
    }

let rez = pairs integers integers |> Seq.filter (fun (x,y) -> x = 5) |> Seq.take 5 |> Seq.toList
