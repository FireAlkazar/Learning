// insert ["a";"b"] 'c'
// lookup ["a";"b"] 
open System
//simple table
let makeSimpleTable() = 
    let list = new Collections.Generic.List<obj>()
    list.Insert(0, "*Table*")
    list

let insertSimple (table : Collections.Generic.List<obj>) (key : obj) (value:obj) =
    if table.Contains(key) then table.Remove((key,value)) |> ignore
    table.Insert(1,(key,value))

let lookupSimple (table: Collections.Generic.List<obj>) (key:obj) =
    let getKey (y:obj) = 
        if y = box "*Table*" then new obj()
        else
            let (a,b) = y :?> obj*obj
            a
    table.Find(fun x -> getKey(x) = key)

let simpTable = makeSimpleTable()
let obj1 = new System.Object()
insertSimple simpTable "a" 1
let res = lookupSimple simpTable "a"