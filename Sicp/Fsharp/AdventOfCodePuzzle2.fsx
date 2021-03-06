﻿// Day 9
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

type HappyLevelRecord = { Person: string; Level:int; Neighbor:string}

let parseHappy (line:string) =
    let parts = line.Split(' ')
    { Person=parts.[0]; Level=Int32.Parse(parts.[1]); Neighbor= parts.[2]}

let calculateDinnerHappiness (table: HappyLevelRecord list) (persons:string list) =
    let numberOfPersons = List.length persons
    let mutable totalHappiness = 0
    for positionIndex in [0..numberOfPersons - 1] do
        let person = persons.[positionIndex]
        let neighborIndex1 = if positionIndex = 0 then numberOfPersons - 1 else positionIndex - 1
        let neighborIndex2 = if positionIndex = numberOfPersons - 1 then 0 else positionIndex + 1
        let neighbor1 = persons.[neighborIndex1]
        let neighbor2 = persons.[neighborIndex2]
        totalHappiness <- totalHappiness + (List.find (fun x -> x.Person = person && x.Neighbor = neighbor1) table).Level
        totalHappiness <- totalHappiness + (List.find (fun x -> x.Person = person && x.Neighbor = neighbor2) table).Level
    totalHappiness

let parseDinnerTable (input:string) =
    let happinessLines = input.Split([|Environment.NewLine|], StringSplitOptions.RemoveEmptyEntries)
    happinessLines |> Array.map parseHappy |> Array.toList

let calcMaxDinnerHappiness  (table: HappyLevelRecord list) = 
    let persons = table |> List.map (fun x -> x.Person ) |> List.distinct
    let personPerms = perms persons
    let mutable hap = Int32.MinValue
    for positions in personPerms do
        let curHap = calculateDinnerHappiness table positions
        hap <- max hap curHap
    hap

let addMyselfToDinnerTable (table: HappyLevelRecord list) =
    let persons = table |> List.map (fun x -> x.Person ) |> List.distinct
    let recordsWithMe = persons |> List.map (fun x -> {Person="Me";Level=0;Neighbor=x})
    let recordsWithMeAsNeighbor = persons |> List.map (fun x -> {Person=x;Level=0;Neighbor="Me"})
    List.append (List.append table recordsWithMe) recordsWithMeAsNeighbor

//let maxDinnerHappiness = calcMaxDinnerHappiness (parseDinnerTable DinnerTableInput)
//let maxDinnerHappinessWithMe = calcMaxDinnerHappiness (addMyselfToDinnerTable (parseDinnerTable DinnerTableInput))
            
//Day 14
let getMaxDistanse (paramss:int list) =
    let speed = paramss.[0]
    let flyTime = paramss.[1]
    let restTime = paramss.[2]
    let totalTime = 2503
    let fullTimeFliesCount = totalTime / (flyTime + restTime)
    let partialTimeFly = totalTime % (flyTime + restTime)
    let lastFlyTime = min partialTimeFly flyTime
    fullTimeFliesCount*flyTime*speed + lastFlyTime*speed

let getMaxDistanceWithBonus (deers:int list list) =
    let totalDeers = List.length deers
    let totalTime = 2503
    let curDistance = Array.init totalDeers (fun x -> 0)
    let bonus = Array.init totalDeers (fun x -> 0)
    for curTime=1 to totalTime do
        for deerIndex=0 to totalDeers - 1 do
            let speed = deers.[deerIndex].[0]
            let flyTime = deers.[deerIndex].[1]
            let restTime = deers.[deerIndex].[2]
            let isFlying = 
                (curTime % (flyTime + restTime)) > 0
                && (curTime % (flyTime + restTime)) <= flyTime
            if isFlying then
                curDistance.[deerIndex] <- curDistance.[deerIndex] + speed

        let curMaxDist = curDistance |> Array.max
        for i=0 to curDistance.Length - 1 do
            if curDistance.[i] = curMaxDist then
                bonus.[i] <- bonus.[i] + 1
    bonus

let DeerParamsInput = [
    [27;5;132]
    [22;2;41]
    [11;5;48]
    [28;5;134]
    [4;16;55]
    [14;3;38]
    [3;21;40]
    [18;6;103]
    [18;5;84]
]

//let deerMaxPath = List.map getMaxDistanse DeerParamsInput |> List.max
//let deerMaxPathWithBonus = getMaxDistanceWithBonus DeerParamsInput |> Array.max

// Day 15
let getHighScoreCookie (ingredients:int list list) = 
    let sugar = ingredients.[0]
    let sprinkles = ingredients.[1]
    let candy = ingredients.[2]
    let chocolate = ingredients.[3]
    let totalSpoonCount = 100;
    let mutable maxScore = 0;
    let mutable maxScoreCalories = 0;
    for e1 in [0..totalSpoonCount] do
        for e2 in [0..totalSpoonCount] do
            for e3 in [0..totalSpoonCount] do
                let e4 = totalSpoonCount - e1 - e2 - e3
                if e4 < 0 then
                    ()
                else
                    let capacity = max 0 (sugar.[0]*e1 + sprinkles.[0]*e2 + candy.[0]*e3 + chocolate.[0]*e4)
                    let durability = max 0 (sugar.[1]*e1 + sprinkles.[1]*e2 + candy.[1]*e3 + chocolate.[1]*e4)
                    let flavor = max 0 (sugar.[2]*e1 + sprinkles.[2]*e2 + candy.[2]*e3 + chocolate.[2]*e4)
                    let texture = max 0 (sugar.[3]*e1 + sprinkles.[3]*e2 + candy.[3]*e3 + chocolate.[3]*e4)
                    let calories = max 0 (sugar.[4]*e1 + sprinkles.[4]*e2 + candy.[4]*e3 + chocolate.[4]*e4)
                    if calories = 500 then
                        maxScoreCalories <- max maxScoreCalories (capacity*durability*flavor*texture)
                    maxScore <- max maxScore (capacity*durability*flavor*texture)
    (maxScore,maxScoreCalories)
let HighScoreCookieInput = 
    [
        [3;0;0;-3;2]
        [-3;3;0;0;9]
        [-1;0;4;0;1]
        [0;0;-2;2;8]
    ]
//let highScoredCookie = getHighScoreCookie HighScoreCookieInput

// Day 16
let parseAuntsInput (input:string) =
    let toSeq (str:string[]) =
        str
        |> Array.map (fun x -> 
            let p = x.Split(':')
            (p.[0].Trim(), int(p.[1].Trim())))
    let rows = input.Split([|Environment.NewLine|], StringSplitOptions.RemoveEmptyEntries)
    rows
    |> Array.map (fun x -> x.Split(','))
    |> Array.map (fun x -> toSeq x)
let auntToGift = 
    Map.ofList [
        ("children", 3)
        ("cats", 7)
        ("samoyeds", 2)
        ("pomeranians", 3)
        ("akitas", 0)
        ("vizslas", 0)
        ("goldfish", 5)
        ("trees", 3)
        ("cars", 2)
        ("perfumes", 1)
    ]

let getAuntsSueNumber () =
    let satisfies (pair: string*int) =   
        auntToGift.[(fst pair)] = (snd pair)
    let satisfiesRange (pair: string*int) =
        match fst pair with
        | "cats" -> auntToGift.[(fst pair)] < (snd pair)
        | "trees" -> auntToGift.[(fst pair)] < (snd pair)
        | "pomeranians" -> auntToGift.[(fst pair)] > (snd pair)
        | "goldfish" -> auntToGift.[(fst pair)] > (snd pair)
        | _ -> auntToGift.[(fst pair)] = (snd pair)
    let parsed = parseAuntsInput AuntsSueInput
    let index = parsed |> Array.findIndex (fun x -> Array.forall satisfiesRange x)
    index + 1
//let AuntsSueNumberResult = getAuntsSueNumber()

// Day 17
let CointainersInput = 
    [
        11
        30
        47
        31
        32
        36
        3
        1
        5
        3
        32
        36
        15
        11
        46
        26
        28
        1
        19
        3
    ]

let getContainersCombinations (input:int list) (lambda:List<List<int>> -> int)=
    let rec getCombs (rest:int list) (curConts:int list) =
        let curSum = List.sum curConts
        if curSum = 150 then
            [curConts]
        elif curSum > 150 then
            []
        else
            match rest with
            | [] -> []
            | h::t ->
                List.append (getCombs t (h::curConts)) (getCombs t curConts)
    let combs = getCombs input []
    lambda combs

let containersCombinationsResult = getContainersCombinations CointainersInput List.length
let containersCombinationsResult2 = 
    getContainersCombinations CointainersInput (fun (x:int list list) -> 
        let minCount = List.length (List.minBy (fun y -> List.length y) x)
        let rez = x |> List.filter (fun y -> List.length y = minCount) 
        rez |> List.length)

//Day 18
let getLightsGifBliking (input:int[,]) =
    let size = Array2D.length1 input
    let getCellValue (x:int) (y:int) (lights:int[,]) =
        if x < 0 || x > (size-1) then
            0
        elif y < 0 || y > (size-1) then
            0
        else
            lights.[x,y]
    let getOnNeighboursCount (x:int) (y:int) (lights:int[,]) =
        let top = getCellValue x (y-1) lights
        let bottom = getCellValue x (y+1) lights
        let left = getCellValue (x-1) (y) lights
        let right = getCellValue (x+1) (y) lights
        let topLeft = getCellValue (x-1) (y-1) lights
        let topRight = getCellValue (x+1) (y-1) lights
        let bottomLeft = getCellValue (x-1) (y+1) lights
        let bottomRight = getCellValue (x+1) (y+1) lights
        top + bottom + left + right + topLeft + topRight + bottomLeft + bottomRight
        
    let stepsCount = 100
    let mutable cur = input
    let mutable next = Array2D.copy input
    for i in [1..stepsCount] do
        for x=0 to size - 1 do
            for y=0 to size - 1 do
                if (x=0 && y=0) || (x=0 && y=99) || (x=99 && y=0) || (x=99 && y=99) then
                    next.[x,y] <- 1
                else
                    let curLight = getCellValue x y cur
                    let onNeighboursCount = getOnNeighboursCount x y cur
                    if curLight = 1 then
                        if List.contains onNeighboursCount [2;3] then
                            next.[x,y] <- 1
                        else
                            next.[x,y] <- 0
                    else
                        if onNeighboursCount=3 then
                            next.[x,y] <- 1
                        else
                            next.[x,y] <- 0
        cur <- Array2D.copy next
    let mutable count = 0
    Array2D.iter (fun x -> if x =1 then count <- count + 1) cur
    count

let LightsGifBlikingResult = getLightsGifBliking LightsGifInput