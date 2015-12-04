type ListBuilder() =
   member o.Bind( lst, f ) = List.concat( List.map (fun x -> f x) lst )
   member o.Return( x ) = [x]
   member o.Zero() = []
 
let list = ListBuilder()
 
let amb = id
 
// last element of a sequence
let last s = Seq.nth ((Seq.length s) - 1) s
 
// is the last element of left the same as the first element of right?
let joins left right = last left = Seq.head right
 
let example = list { let! w1 = amb ["the"; "that"; "a"]
                     let! w2 = amb ["frog"; "elephant"; "thing"]
                     let! w3 = amb ["walked"; "treaded"; "grows"]
                     let! w4 = amb ["slowly"; "quickly"]
                     if joins w1 w2 &&
                        joins w2 w3 &&
                        joins w3 w4
                     then
                        return String.concat " " [w1; w2; w3; w4]
                   }

//
let list1 = ListBuilder()

let people = 
    list1 { 
        let! baker = amb [1;2;3;4;5]
        let! cooper = amb [1;2;3;4;5]
        let! fletcher = amb [1;2;3;4;5]
        let! miller = amb [1;2;3;4;5]
        let! smith = amb [1;2;3;4;5]
        let req1 = 
            Seq.distinct [baker;cooper;fletcher;miller;smith] |> Seq.length = 5
        let req2 = baker <> 5
        let req3 = cooper <> 1
        let req4 = fletcher <> 5 && fletcher <> 1
        let req5 = miller > cooper
        //let req6 = (System.Math.Abs (smith - fletcher)) <> 1
        let req7 = (System.Math.Abs (cooper - fletcher)) <> 1
        
        if req1 &&
           req2 &&
           req3 &&
           req4 &&
           req5 &&
           //req6 &&
           req7 
        then 
            return [baker;cooper;fletcher;miller;smith]
          }


