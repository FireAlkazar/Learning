#load "AdventOfCodeInputs.fs"
open AdventOfCodeInputs

//Day 19
let medicineMolecule = "ORnPBPMgArCaCaCaSiThCaCaSiThCaCaPBSiRnFArRnFArCaCaSiThCaCaSiThCaCaCaCaCaCaSiRnFYFArSiRnMgArCaSiRnPTiTiBFYPBFArSiRnCaSiRnTiRnFArSiAlArPTiBPTiRnCaSiAlArCaPTiTiBPMgYFArPTiRnFArSiRnCaCaFArRnCaFArCaSiRnSiRnMgArFYCaSiRnMgArCaCaSiThPRnFArPBCaSiRnMgArCaCaSiThCaSiRnTiMgArFArSiThSiThCaCaSiRnMgArCaCaSiRnFArTiBPTiRnCaSiAlArCaPTiRnFArPBPBCaCaSiThCaPBSiThPRnFArSiThCaSiThCaSiThCaPTiBSiRnFYFArCaCaPRnFArPBCaCaPBSiRnTiRnFArCaPRnFArSiRnCaCaCaSiThCaRnCaFArYCaSiRnFArBCaCaCaSiThFArPBFArCaSiRnFArRnCaCaCaFArSiRnFArTiRnPMgArF"

let rec getMoleculesBySubstitution (molecule:string) (substitution:string*string) =
    let present = fst substitution
    let replacement = snd substitution
    let presentStartIndex = molecule.IndexOf(present)
    if presentStartIndex = -1 then
        []
    else
        let headWithPresentLength = presentStartIndex + present.Length
        let headWithPresent = molecule.Substring(0, headWithPresentLength)
        let rest = molecule.Substring(headWithPresentLength)
        let replaced = headWithPresent.Replace(present, replacement)
        let newMolecule = replaced + rest
        newMolecule::(List.map (fun x -> headWithPresent + x) (getMoleculesBySubstitution rest substitution))

let getAllDistinctMolecules (molecule:string) (subs:(string*string) list) =
    let allMolecules = List.collect (fun x -> getMoleculesBySubstitution molecule x ) subs
    allMolecules
    |> Seq.distinct
    |> Seq.toList

let getMinimumStepsToConstructMolecule (molecule:string) (subs:(string*string) list) =
    let filterRemove (remove:string list) (from:string list) =
        List.filter (fun x -> (List.exists (fun y-> y=x) remove)=false) from

    let reversedSubs = List.map (fun x -> (snd x, fst x)) subs
    let startMolecule = molecule
    let endMolecule = "e"
    let usedSubs = reversedSubs
    let mutable isConstructed = false
    let mutable counter = 0
    let mutable curMolecules = [startMolecule]
    let mutable alreadyDoneMolecules = []
    let mutable nextMolecules = []
    while isConstructed = false do
        counter <- counter + 1
        for m in curMolecules do
            let ms = getAllDistinctMolecules m usedSubs
            nextMolecules <- List.append nextMolecules ms
        let minMoleculeLength = List.minBy (fun (x:string) -> x.Length) nextMolecules
        curMolecules <- 
            nextMolecules
            //|> filterRemove alreadyDoneMolecules
            |> Seq.distinct 
            |> Seq.toList
            |> List.filter (fun x -> x.Length = minMoleculeLength.Length)
        if List.length curMolecules > 100 then
            curMolecules <- 
                curMolecules
                |> Seq.take 100
                |> Seq.toList
        printfn "%A" curMolecules
        printfn "%A" (List.length curMolecules)
        if List.exists (fun x -> x = endMolecule) curMolecules then
            isConstructed <- true
        //alreadyDoneMolecules <- List.append alreadyDoneMolecules curMolecules
    counter    

//let moleculesCount = List.length (getAllDistinctMolecules medicineMolecule MoleculesSubstitutionInput)
//let minimumStepsToConstructMolecule = getMinimumStepsToConstructMolecule medicineMolecule MoleculesSubstitutionInput

//Day 20
let elvesPresentsTotal = 29000000

let getAllDivisitors (num:int) =
    let mutable divs = []
    for i in 1..num do
        if num % i = 0 then
            divs <- i::divs
    divs

let rec findFewestNumWithSumDivisitorGreaterThan (sum:int) (probe:int) (extraFilter:int -> int -> bool) =
    let curSum = (getAllDivisitors probe) |> List.filter (extraFilter probe) |> List.sum
    if curSum > sum then
        probe
    else
        if curSum > 2000000 then
            printfn "probe - %d, sum - %d" probe curSum
        findFewestNumWithSumDivisitorGreaterThan sum (probe+1) extraFilter

//let houseNumberWithRequiredElvesPresents = findFewestNumWithSumDivisitorGreaterThan (elvesPresentsTotal/10) 665000 (fun x y -> true)
//let houseNumberWithRequiredElvesPresents2 = findFewestNumWithSumDivisitorGreaterThan (elvesPresentsTotal/11) 665000 (fun x y -> x <= y*50 )

//Day 21
let TheBoss = [103;9;2]
let Weapons = [(4,8);(5,10);(6,25);(7,40);(8,74)]
let Armor = [(0,0);(1,13);(2,31);(3,53);(4,75);(5,102)]
let Rings = [(1,25);(2,50);(3,100);(-1,20);(-2,40);(-3,80)]
let TwoRings =
    seq {
        for i in 0..Rings.Length - 2 do
            for j in i+1..Rings.Length - 1 do
                yield [Rings.[i];Rings.[j]]
    } |> Seq.toList
let RingsUnified = List.map (fun x -> [x]) Rings
let AllPossibleRings = []::(List.append RingsUnified TwoRings)

let rec willPlayerWin (player:int list) (boss:int list) =
    if boss.[0] <= 0 then 
        true
    elif player.[0] <= 0 then
        false
    else
       let playerStrike = max (player.[1] - boss.[2]) 1
       let bossStrike = max (boss.[1] - player.[2]) 1
       let newPlayerStats = (player.[0] - bossStrike)::(List.tail player)
       let newBossStats = (boss.[0] - playerStrike)::(List.tail boss)
       willPlayerWin newPlayerStats newBossStats

let getStatsAndCost (weapon:int*int) (armor:int*int) (rings:(int*int) list) =
    let mutable damage = fst weapon
    let mutable defense = fst armor
    let mutable cost = (snd weapon) + (snd armor)
    for ring in rings do
        cost <- cost + (snd ring)
        let stat = fst ring
        if stat >= 0 then
            damage <- damage + stat
        else
            defense <- defense  + (-stat)
    ((damage,defense), cost)

let getMinCostPlayerWins (boss:int list) =
    let mutable minCost = 500
    for weapon in Weapons do
        for armor in Armor do
            for rings in AllPossibleRings do
                let curStatAndCost = getStatsAndCost weapon armor rings
                let stats = fst curStatAndCost
                let cost = snd curStatAndCost
                if willPlayerWin [100;(fst stats);(snd stats)] boss then
                    minCost <- min minCost cost
    minCost

let getMaxCostPlayerLose (boss:int list) =
    let mutable maxCost = 0
    for weapon in Weapons do
        for armor in Armor do
            for rings in AllPossibleRings do
                let curStatAndCost = getStatsAndCost weapon armor rings
                let stats = fst curStatAndCost
                let cost = snd curStatAndCost
                if willPlayerWin [100;(fst stats);(snd stats)] boss then
                    ()
                else
                    maxCost <- max maxCost cost
    maxCost 
            
let minCostWin = getMinCostPlayerWins TheBoss
let maxCostLose = getMaxCostPlayerLose TheBoss
