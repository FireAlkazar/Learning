// Day 9
type LocationsRoutes = Map<string*string,int>

let getDistanceBetween (city1:string) (city2:string) (routes:LocationsRoutes) =
    if Map.containsKey (city1,city2) routes then
        routes.[(city1,city2)]
    elif Map.containsKey (city2,city1) routes then
        routes.[(city2,city1)]
    else
        failwith "Route not found"

let getAllCities (routes:LocationsRoutes) =
    Map.toList routes
    |> List.map fst
    |> List.collect (fun x -> [fst x;snd x])
    |> Seq.distinct
    |> Seq.toList

let getNearestCity (city:string) (routes:LocationsRoutes)(cityList: string list) =
    let mutable nearestCity = ""
    let mutable minDistance = System.Int32.MaxValue
    for curCity in cityList do
        let curDistance = getDistanceBetween city curCity routes
        if curDistance < minDistance then
            nearestCity <- curCity
            minDistance <- curDistance
    (nearestCity, minDistance)

let getFarestCity (city:string) (routes:LocationsRoutes)(cityList: string list) =
    let mutable farestCity = ""
    let mutable maxDistance = 0
    for curCity in cityList do
        let curDistance = getDistanceBetween city curCity routes
        if curDistance > maxDistance then
            farestCity <- curCity
            maxDistance <- curDistance
    (farestCity, maxDistance)

let rec getShortestPathWithStartCity (startCity:string) (routes:LocationsRoutes) (cityList: string list) =
    if cityList = [] then
        0
    else
        let nearest = getNearestCity startCity routes cityList
        let visitedCity = (fst nearest)
        let distance = snd nearest
        let newCityList = cityList |> List.filter ((<>) visitedCity)
        distance + (getShortestPathWithStartCity visitedCity routes newCityList)

let rec getLongestPathWithStartCity (startCity:string) (routes:LocationsRoutes) (cityList: string list) =
    if cityList = [] then
        0
    else
        let farest = getFarestCity startCity routes cityList
        let visitedCity = (fst farest)
        let distance = snd farest
        let newCityList = cityList |> List.filter ((<>) visitedCity)
        distance + (getLongestPathWithStartCity visitedCity routes newCityList)

let routesNative =
    [
    (("AlphaCentauri","Snowdin"),66)
    (("AlphaCentauri","Tambi"),28)
    (("AlphaCentauri","Faerun"),60)
    (("AlphaCentauri","Norrath"),34)
    (("AlphaCentauri","Straylight"),34)
    (("AlphaCentauri","Tristram"),3)
    (("AlphaCentauri","Arbre"),108)
    (("Snowdin","Tambi"),22)
    (("Snowdin","Faerun"),12)
    (("Snowdin","Norrath"),91)
    (("Snowdin","Straylight"),121)
    (("Snowdin","Tristram"),111)
    (("Snowdin","Arbre"),71)
    (("Tambi","Faerun"),39)
    (("Tambi","Norrath"),113)
    (("Tambi","Straylight"),130)
    (("Tambi","Tristram"),35)
    (("Tambi","Arbre"),40)
    (("Faerun","Norrath"),63)
    (("Faerun","Straylight"),21)
    (("Faerun","Tristram"),57)
    (("Faerun","Arbre"),83)
    (("Norrath","Straylight"),9)
    (("Norrath","Tristram"),50)
    (("Norrath","Arbre"),60)
    (("Straylight","Tristram"),27)
    (("Straylight","Arbre"),81)
    (("Tristram","Arbre"),90)
    ]

let getShortestPath =
    let routes = LocationsRoutes(routesNative)
    let allCities = getAllCities routes
    let mutable minDistance = System.Int32.MaxValue
    for curCity in allCities do
        let citiesToVisit = allCities |> List.filter ((<>) curCity)
        let curCityMin = getShortestPathWithStartCity curCity routes citiesToVisit
        minDistance <- min minDistance curCityMin
    minDistance

let getLongestPath =
    let routes = LocationsRoutes(routesNative)
    let allCities = getAllCities routes
    let mutable maxDistance = 0
    for curCity in allCities do
        let citiesToVisit = allCities |> List.filter ((<>) curCity)
        let curCityMax = getLongestPathWithStartCity curCity routes citiesToVisit
        maxDistance <- max maxDistance curCityMax
    maxDistance

// Day 10
let lookAndSayInput = "1113222113"

let lookAndSayOnce (look:string) = 
    let mutable curDigit = look.[0];
    let result = new System.Text.StringBuilder();
    let mutable curCount = 0;
    for digit in look do
        if curDigit = digit then
            curCount <- curCount + 1
        else
            result.Append(curCount).Append(curDigit) |> ignore
            curDigit <- digit
            curCount <- 1
    result.Append(curCount).Append(curDigit) |> ignore
    result.ToString()

let lookAndSay (input:string) (applyTimes:int) =
    let mutable r = input
    for i in [1..applyTimes] do
        r <- lookAndSayOnce r
    r.Length

//let lookAndSayResult = lookAndSay lookAndSayInput 50

// Day 11
let getNextChar (ch:char) =
    if ch = 'z' then
        'a'
    else
        char(int(ch) + 1)
let getNextPassword (input:string) = 
    let chars = input.ToCharArray()
    let mutable continueLoop = true
    let mutable index = chars.Length-1 
    while continueLoop do
        let curChar = chars.[index]
        let nextChar = getNextChar curChar
        chars.[index] <- nextChar
        if nextChar = 'a' then
            index <- index - 1
        else
            continueLoop <- false
    new System.String(chars)

let rec passwordHasThreeConsequtiveChars (input:string) =
    if input.Length < 3 then
        false
    else
        if int(input.[0]) + 1 = int(input.[1]) && int(input.[1]) = int(input.[2]) - 1 then
            true
        else
             passwordHasThreeConsequtiveChars (input.Substring(1))

let passwordHasNoForbiddenChars (input:string) =
    let forbidden = ["i";"o";"l"]
    forbidden
    |> List.exists (fun x -> input.Contains(x) = false)

let rec passwordHasDifferentPairs (input: string) (numberOfPairs:int) =
    if numberOfPairs = 0 then
        true
    elif input.Length < 2 then
        false
    else
        if input.[0] = input.[1] then
            passwordHasDifferentPairs (input.Substring(2)) (numberOfPairs - 1)
        else
            passwordHasDifferentPairs (input.Substring(1)) numberOfPairs

let getNewSantaPassword (oldSantaPassword:string) = 
    let rec getNew (password:string) =
        if passwordHasThreeConsequtiveChars password &&
            passwordHasNoForbiddenChars password &&
            passwordHasDifferentPairs password 2 then
                password
        else
            getNew (getNextPassword password)
    getNew (getNextPassword oldSantaPassword)
        
let SantaPasswordInput = "hepxcrrq"
let SantaNewPasswordResult = getNewSantaPassword SantaPasswordInput
let SantaAnotherNewPassword = getNewSantaPassword SantaNewPasswordResult

// Day 12
open System
type Json = 
    | JsonObject of Map<string,Json>
    | JsonArray of Json list
    | JsonString of string
    | JsonInt of int

let isJsonArray (exp:char list) =
    exp.[0] = '['
    
let isJsonObject (exp:char list) = 
    exp.[0] = '{'

let toStr (chars: char list) = 
    let charArray = chars |> List.toArray
    new String(charArray) 

let rec extractJsonInt (exp:char list) =
    match exp with
    | '-'::xs -> 
        let num,rest = extractJsonInt(xs)
        ('-'::num,rest)
    | d::xs when System.Char.IsDigit(d) -> 
        let num,rest = extractJsonInt(xs)
        (d::num,rest)
    | _ -> ([], exp)

let extractJsonString (exp:char list)=
    let rec extract (exp:char list) (acc: char list) =
        match exp with
        | '"'::xs -> 
            if acc = [] then
                extract xs []
            else
                ([], xs)
        | d::xs -> 
            if Char.IsWhiteSpace(d) && acc = [] then
                extract xs acc
            else
                let str,rest = extract xs (d::acc)
                (d::str,rest)
        | _ -> failwith "extactJsonString fail"
    extract exp []
let t = extractJsonString ("\"abc\", xxx".ToCharArray() |> Array.toList)

let rec removeHeadingColon (input:char list) =
    match input with
    | x::xs when Char.IsWhiteSpace(x) ->
        removeHeadingColon xs
    | ':'::xs -> 
        xs
    | _ -> failwith "removeHeadingSemicolon fail"

let rec extractJsonObject (obj:char list) (acc:(string*Json) list) =
    match obj with
    | x::xs when System.Char.IsWhiteSpace(x) ->
        extractJsonObject xs acc
    | '{'::xs ->
        let keyValue,rest = extractJsonObjectKeyValuePair xs
        extractJsonObject rest (keyValue::acc)
    | ','::xs ->
        let keyValue,rest = extractJsonObjectKeyValuePair xs
        extractJsonObject rest (keyValue::acc)
    | '}'::xs ->
        (acc,xs)
    | _ -> failwith "extractJsonObject fail"
and extractJsonObjectKeyValuePair (obj:char list) =
    let key,restAfterKey= extractJsonString obj
    let afterColon = removeHeadingColon restAfterKey
    let json,rest = parseJson afterColon
    ((toStr(key),json),rest)
and extractJsonArray (ar:char list) (acc:Json list) =
    //printfn "%A" ar
    match ar with
    | x::xs when System.Char.IsWhiteSpace(x) ->
        extractJsonArray xs acc
    | '['::xs ->
        let json,rest = parseJson xs
        extractJsonArray rest (json::acc)
    | ','::xs ->
        let json,rest = parseJson xs
        extractJsonArray rest (json::acc)
    | ']'::xs ->
        (acc,xs)
    | _ -> failwith "extractJsonArray fail"
and parseJson (input:char list) =
    if isJsonArray input then
        let jsonArray,rest = extractJsonArray input []
        (JsonArray(jsonArray),rest)
    elif isJsonObject input then
        let jsonObjPairs,rest = extractJsonObject input []
        (JsonObject(Map(jsonObjPairs)),rest)
    else
        let num,rest = extractJsonInt input
        if num = [] then
            let str,rest = extractJsonString input
            (JsonString(toStr(str)),rest)
        else
            (JsonInt(System.Int32.Parse(toStr(num))), rest)
let parseJsonString (input:string) =
    parseJson (input.ToCharArray() |> Array.toList )

let rec calculateJsonSum (json:Json) : int = 
    match json with
    | JsonObject(x) ->
        x |> Seq.sumBy (fun y -> calculateJsonSum (y.Value))
    | JsonArray(x) ->
        x |> Seq.sumBy (fun y -> calculateJsonSum y)
    | JsonString(x) -> 0
    | JsonInt(x) -> x

let rec calculateJsonSumExludingRedObject (json:Json) : int = 
    match json with
    | JsonObject(x) ->
        if Map.exists (fun y z -> z = JsonString("red")) x then
            0
        else
            x |> Seq.sumBy (fun y -> calculateJsonSumExludingRedObject (y.Value))
    | JsonArray(x) ->
        x |> Seq.sumBy (fun y -> calculateJsonSumExludingRedObject y)
    | JsonString(x) -> 0
    | JsonInt(x) -> x

#load "AdventOfCodeInputs.fs"
open AdventOfCodeInputs
//let AccountantJsonResult = calculateJsonSum (fst (parseJsonString AccountantJsonInput))
//let AccountantJsonResult2 = calculateJsonSumExludingRedObject (fst (parseJsonString AccountantJsonInput))

// Day 13
let distrib e L =
    let rec aux pre post = 
        seq {
            match post with
            | [] -> yield (L @ [e])
            | h::t -> yield (List.rev pre @ [e] @ post)
                      yield! aux (h::pre) t 
        }
    aux [] L

let rec perms = function 
    | [] -> Seq.singleton []
    | h::t -> Seq.collect (distrib h) (perms t)

type HappyLevelRecord = { Person: string; Level:int; IfNeigbor:string}

let parseHappy (line:string) =
    let parts = line.Split(' ')
    { Person=parts.[0]; Level=Int32.Parse(parts.[1]); IfNeigbor= parts.[1]}

let calcMaxDinnerHappiness (input:string) = 
    let happinessLines = input.Split([|Environment.NewLine|], StringSplitOptions.RemoveEmptyEntries)
    let table = happinessLines |> Array.map parseHappy |> Array.toList
    let persons = table |> List.map (fun x -> x.Person ) |> List.distinct
    persons

let calculateDinnerHappiness (table: HappyLevelRecord) (permutation:int list) (persons:string list) =
    

let rez = calcMaxDinnerHappiness DinnerTableInput
            
    

