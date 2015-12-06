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



//It contains a pair of any two letters that appears at least twice in the string without overlapping, like xyxy (xy) or aabcdefgaa (aa), but not like aaa (aa, but it overlaps).
//It contains at least one letter which repeats with exactly one letter between them, like xyx, abcdefeghi (efe), or even aaa.
