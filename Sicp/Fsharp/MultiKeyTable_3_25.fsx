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

//Two key table
let makeTwoKeyTable() =
    let dict = new Collections.Generic.Dictionary<obj,Collections.Generic.Dictionary<obj,obj>>()
    dict

let insertByTwoKey (dict : Collections.Generic.Dictionary<obj,Collections.Generic.Dictionary<obj,obj>>) k1 k2 value =
    if dict.ContainsKey(k1) then dict.[k1].[k2] <- value
    else 
        dict.[k1] <- new Collections.Generic.Dictionary<obj,obj>()
        dict.[k1].[k2] <- value

let lookupByTwoKey (dict : Collections.Generic.Dictionary<obj,Collections.Generic.Dictionary<obj,obj>>) k1 k2 =
    if dict.ContainsKey(k1) then
        let subDict = dict.[k1]
        if subDict.ContainsKey(k2) then subDict.[k2]
        else null
    else null

let dict = makeTwoKeyTable()
insertByTwoKey dict "subject" "math" 1
insertByTwoKey dict "subject" "biology" "lion"
let res2 = lookupByTwoKey dict "subject" "biology"