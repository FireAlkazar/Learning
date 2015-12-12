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

let t = getNextPassword "az"
let t1 = passwordHasDifferentPairs "zzzabbd" 2
        
let SantaPasswordInput = "hepxcrrq"
let SantaNewPasswordResult = getNewSantaPassword SantaPasswordInput
let SantaAnotherNewPassword = getNewSantaPassword SantaNewPasswordResult
