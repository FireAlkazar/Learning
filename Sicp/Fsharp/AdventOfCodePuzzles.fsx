// Puzzles from http://adventofcode.com
#load "AdventOfCodeInputs.fs"
open AdventOfCodeInputs

// Day 1
let getFirstBaseFlour (input:string) =
    let mutable down = 0
    let mutable up = 0
    let mutable curIndex = 0;
    let mutable result = 0;
    for char in input do
        match char with
        | '(' -> 
            up <- up + 1
            curIndex <- curIndex + 1
        | ')' -> 
            down <- down + 1
            curIndex <- curIndex + 1
        | x -> ()
        if (up - down) = -1 && result = 0 then
            result <- curIndex
    result
let res = getFirstBaseFlour FlourInput

// Day 2
open System
let getBoxPaper (l,w,h) =
    let boxPaper = 2*l*w + 2*l*h + 2*w*h
    let extra = [l*w;l*h;w*h] |> List.min
    boxPaper + extra

let parseSingleBox (s:string) =
    let dimensions = s.Split('x')
    (Int32.Parse(dimensions.[0]), Int32.Parse(dimensions.[1]), Int32.Parse(dimensions.[2]))

let parseBoxes (b:string) =
    b.Split([|Environment.NewLine|], StringSplitOptions.RemoveEmptyEntries)
    |> Array.map parseSingleBox

let getStaffForElves boxes stuffFunc =
    parseBoxes boxes
    |> Array.map stuffFunc
    |> Array.fold (fun s x -> s + x) 0

let getBoxRibbon (l,w,h) = 
    let max = [l;w;h] |> List.max
    let wrap = 2*(l + w + h - max)
    let bow = l * w * h
    wrap + bow

let test = parseBoxes ElvesBoxes |> Array.length
let test2 = getBoxPaper (2,3,4)
let elvesPaperResult = getStaffForElves ElvesBoxes getBoxPaper
let elvesRibbonResult = getStaffForElves ElvesBoxes getBoxRibbon

//Day 3
let getSantaPathHouses (path:string) =
    let mutable houses = [(0,0)]
    let mutable curX = 0
    let mutable curY = 0
    for move in path do
        match move with
        | '>' -> curX <- curX + 1
        | '<' -> curX <- curX - 1
        | 'v' -> curY <- curY + 1
        | '^' -> curY <- curY - 1
        | _ -> ()

        if houses |> List.contains (curX,curY) then
            ()
        else
            houses <- List.append [(curX,curY)] houses
    List.length houses

let getSantaAndRobotPathHouses (path:string) =
    let mutable houses = [(0,0)]
    let mutable curX = 0
    let mutable curY = 0
    let mutable curRoboX = 0
    let mutable curRoboY = 0
    let mutable isRoboTurn = false
    for move in path do
        let mutable dX = 0
        let mutable dY = 0
        match move with
        | '>' -> dX <- 1
        | '<' -> dX <- -1
        | 'v' -> dY <- 1
        | '^' -> dY <- -1
        | _ -> ()

        if dX <> 0 || dY <> 0 then
            if isRoboTurn then
                curRoboX <- curRoboX + dX
                curRoboY <- curRoboY + dY

                if houses |> List.contains (curRoboX,curRoboY) then
                    ()
                else
                    houses <- List.append [(curRoboX,curRoboY)] houses

                isRoboTurn <- false
            else
                curX <- curX + dX
                curY <- curY + dY

                if houses |> List.contains (curX,curY) then
                    ()
                else
                    houses <- List.append [(curX,curY)] houses

                isRoboTurn <- true


    List.length houses

//let santaPathHousesResult = getSantaPathHouses SantaPathHousesInput
//let santaAndRoboPathHousesResult = getSantaAndRobotPathHouses SantaPathHousesInput
           
            
// Day 4
open System.Text
let calcMD5 (x:string) =
    let md5 = new System.Security.Cryptography.MD5CryptoServiceProvider()
    let encoded = Encoding.UTF8.GetBytes(x)
    md5.ComputeHash(encoded)

let isFiveFirstZeroes (buffer:byte[]) =
    buffer.[0] = 0uy && buffer.[1] = 0uy && ((buffer.[2] >>> 4) = 0uy)

let isSixFirstZeroes (buffer:byte[]) =
    buffer.[0] = 0uy && buffer.[1] = 0uy && buffer.[2] = 0uy

let rec findNumberToProduceMD5 testFunc (secret:string) (number:int) =
    let test = secret + number.ToString()
    if testFunc (calcMD5 test) then 
        number
    else
        findNumberToProduceMD5 testFunc secret (number + 1)

//let fiveFirstZeroesMD5Number = findNumberToProduceMD5 isFiveFirstZeroes SecretAdventKey 0
//let sixFirstZeroesMD5Number = findNumberToProduceMD5 isSixFirstZeroes SecretAdventKey 0

// Day 5
open System.Text.RegularExpressions
let hasAtListThreeVowels (input:string) =
    let mutable vowelCount = 0
    let vowels = ['a';'e';'i';'o';'u'] //aeiou only
    let vowelsCount = input.ToCharArray() |> Array.map (fun x -> if List.contains x vowels then 1 else 0 ) |> Array.sum
    vowelsCount > 2

let hasTwiceLetters (input:string) =
    input.ToCharArray() |> Array.pairwise |> Array.exists (fun x -> match x with | (y,z) -> y = z | _ -> false)

let hasNoBadStrings (input:string) =
    let badStrings = ["ab";"cd";"pq";"xy"]
    badStrings |> List.forall (fun x -> input.Contains(x) = false)

let isNiceString (input:string) =
    hasAtListThreeVowels(input) && hasTwiceLetters(input) && hasNoBadStrings(input)

let calculateNiceStringsCount (bigInput:string) filter =
    let strings = bigInput.Split([|Environment.NewLine|], StringSplitOptions.RemoveEmptyEntries)
    strings |> Array.filter filter |> Array.length

let niceStringsCountResult = calculateNiceStringsCount NiceStringsInput isNiceString

let hasAPairOfTwoLetters (input:string) = 
    let rec hasPair (pair:string) (substr:string) =
        if substr.Contains(pair) then
            true
        elif substr.Length < 2 then
            false
        else
            let newPair = pair.Substring(1) + substr.[0].ToString()
            let newSubstr = substr.Substring(1)
            hasPair newPair newSubstr
    hasPair (input.Substring(0,2)) (input.Substring(2))

let te = hasAPairOfTwoLetters "aaaa"

let hasPairSeparatedByChar (input:string) = 
    let mutable result = false
    let mutable char0 = ' '
    let mutable char1 = ' '
    let mutable char2 = ' '
    let loopRange = [2..input.Length - 1]
    for i in loopRange do
        char0 <- input.[(i-2)]
        char1 <- input.[(i-1)]
        char2 <- input.[i]

        if char0 = char2 then
            result <- true
    result

let isNiceString2 input =
    hasAPairOfTwoLetters(input) && hasPairSeparatedByChar(input)

let te2 = isNiceString2 "uurcxstgmygtbstg"

let niceStrings2CountResult = calculateNiceStringsCount NiceStringsInput isNiceString2

// Day 6
type LightsInst =
    {
        Command: string
        FromX: int
        FromY: int
        ToX: int
        ToY: int
    }
//toggle 461,550 through 564,900
let parseLightsInst (inst:string) =
    let parts = inst.Split(' ')
    if parts.[0] = "turn" then
        parts.[0] <- parts.[0] + " " + parts.[1]
        parts.[1] <- parts.[2]
        parts.[3] <- parts.[4]
    let from = parts.[1].Split(',') |> Array.map Int32.Parse
    let toXY = parts.[3].Split(',') |> Array.map Int32.Parse
    { 
        Command = parts.[0]
        FromX = from.[0]
        FromY = from.[1]
        ToX = toXY.[0]
        ToY = toXY.[1]
    } 

let getLitLightsCount (input:string) = 
    let rawInsts = input.Split([|Environment.NewLine|], StringSplitOptions.RemoveEmptyEntries)
    let grid = Array2D.init 1000 1000 (fun i j -> false) 
    for raw in rawInsts do
        let inst = parseLightsInst raw
        for i in [inst.FromX..inst.ToX] do
            for j in [inst.FromY..inst.ToY] do
                match inst.Command with
                | "toggle" -> grid.[i,j] <- not grid.[i,j]
                | "turn off" -> grid.[i,j] <- false
                | "turn on" -> grid.[i,j] <- true
                | _ -> failwith "Uncknown command"

    grid |> Seq.cast<bool> |> Seq.filter (fun x -> x) |> Seq.length

// Ancient Nordic Elvish!
let getTotalLightBrightness (input:string) = 
    let rawInsts = input.Split([|Environment.NewLine|], StringSplitOptions.RemoveEmptyEntries)
    let grid = Array2D.init 1000 1000 (fun i j -> 0) 
    for raw in rawInsts do
        let inst = parseLightsInst raw
        for i in [inst.FromX..inst.ToX] do
            for j in [inst.FromY..inst.ToY] do
                match inst.Command with
                | "toggle" -> grid.[i,j] <- 2 + grid.[i,j]
                | "turn off" -> 
                    grid.[i,j] <- 
                    if grid.[i,j] = 0 then
                        0
                    else 
                        grid.[i,j] - 1
                | "turn on" -> grid.[i,j] <- grid.[i,j] + 1
                | _ -> failwith "Uncknown command"

    grid |> Seq.cast<int> |> Seq.sum
                
//let litLightsCountResult = getLitLightsCount LightsInstructionsInput
//let lightTotalBrightnessResult = getTotalLightBrightness LightsInstructionsInput

// Day 7

let topologicalSort (graph : Map<string,string list>) = 
    let getZeroKeys (deg: System.Collections.Generic.Dictionary<string,int>) =
        deg.Keys 
        |> Seq.filter (fun x -> deg.[x] = 0)
        |> Seq.toList
    let getInDegree (graph : Map<string,string list>) (alreadyDone: string list) = 
        let inDegreeLocal = new System.Collections.Generic.Dictionary<string,int>()
        let doneAsSet = Set.ofList alreadyDone
        for x in graph do
            let curVertex = x.Key
            if doneAsSet.Contains curVertex then
                ()
            else
                inDegreeLocal.[curVertex] <- 
                    let curVertexSet = 
                        graph.[curVertex]
                        |> Set.ofList
                    Set.difference curVertexSet doneAsSet
                    |> Set.count
        inDegreeLocal

    let mutable result = []
    let mutable inDegree = getInDegree graph result
    while inDegree.Count > 0 do
        let noIn = getZeroKeys inDegree 
        result <- List.append result noIn
        inDegree <- getInDegree graph result
    result

let WireOperators = ["OR";"AND";"LSHIFT";"NOT";"RSHIFT"]

type InstructionPart =
    | WireId of string
    | Operator of string
    | Arrow
    | Const of uint16

let (|WireConst|_|) str =
    match System.Int32.TryParse(str) with
    | (true,int) -> Some(Const(uint16(int)))
    | _ -> None

let (|WireOperator|_|) str =
    if List.contains str WireOperators then
        Some(Operator(str))
    else
        None

let (|WireArrow|_|) str =
    if str = "->" then
        Some(Arrow)
    else
        None

let parseWireInstructionPart (part:string) =
    match part with
    | WireConst x -> x
    | WireOperator x -> x
    | WireArrow x -> x
    | x -> WireId(x)

let rec getWireDestination (parts: InstructionPart list) =
    match parts with
    | [] -> failwith "Wire destination not found"
    | x::xs ->
        if x = Arrow then
            List.head xs
        else
            getWireDestination xs

let getWireSource (parts: InstructionPart list) =
    let rec getWireSourceRec (parts: InstructionPart list) (acc: InstructionPart list) =
        match parts with
            | [] -> failwith "Wire source not found"
            | x::xs ->
                if x = Arrow then
                    List.rev acc
                else
                    getWireSourceRec xs (x::acc)
    getWireSourceRec parts []

let parseWireInstruction (input:string) = 
    let parts = input.Split(' ')
                |> Array.toList
                |> List.map parseWireInstructionPart
    (getWireDestination(parts), getWireSource(parts))

let getWireId (i:InstructionPart) =
    match i with
    | WireId(x) -> x
    | _ -> failwith "Can't get wire Id: Type of InstructionPart is not WireId"

let getWireConst (i:InstructionPart) =
    match i with
    | Const(x) -> x
    | _ -> failwith "Can't get wire Const: Type of InstructionPart is not Const"

let getWireDependencies (insts: InstructionPart list) =
    insts 
    |> List.filter (fun x -> match x with | WireId(x) -> true | _ -> false)
    |> List.map (fun x -> match x with | WireId(x) -> x | _ -> failwith "Should not be here")

let getWireValue (wireId:string) (all: Map<string,InstructionPart list>) (calculated:Map<string,uint16>) =
    let getWireValueSimple (exp:InstructionPart) =
        match exp with
        | Const(x) -> x
        | WireId(x) -> calculated.[x]
        | _ -> failwith "should not be here"

    let insts = all.[wireId]
    if List.length insts = 1 then
        getWireValueSimple insts.[0]
    elif List.length insts = 2 then
        if insts.[0] <> Operator("NOT") then
            failwith "NOT expected"
        else
            ~~~calculated.[getWireId(insts.[1])]
    else    
        let op = insts.[1]
        let firstOperand = getWireValueSimple insts.[0]
        let secondOperand = getWireValueSimple insts.[2]
        match op with 
        | Operator("AND") ->
            firstOperand &&& secondOperand
        | Operator("OR") ->
            firstOperand ||| secondOperand
        | Operator("LSHIFT") ->
            firstOperand <<< int32(secondOperand)
        | Operator("RSHIFT") ->
            firstOperand >>> int32(secondOperand)
        | _ -> failwith "Unknown operator"
    
let parseAllWires (input:string) =
    let insts = 
        input.Split([|Environment.NewLine|], StringSplitOptions.RemoveEmptyEntries)
        |> Array.map parseWireInstruction
        |> Array.map (fun x -> (getWireId(fst x), snd x))
    let mapped = Map(insts)
    let forTsort = insts |> Array.map (fun x -> (fst x, getWireDependencies(snd x)))
    let sorted = topologicalSort (Map(forTsort))
    //printfn "%A" sorted
    let mutable calculated = Map.empty
    for wireId in sorted do
        let wireValue = getWireValue wireId mapped calculated
        printfn "%A -- %A" wireId wireValue
        calculated <- calculated |> Map.add wireId wireValue
    calculated

let testResult = (parseAllWires WiresInstructionsInput)


