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

let rec findFewestNumWithSumDivisitorGreaterThan (sum:int) (probe:int) (extraFilter:int -> bool) =
    let curSum = (getAllDivisitors probe) |> List.filter extraFilter |> List.sum
    if curSum > sum then
        probe
    else
        if curSum > 2000000 then
            printfn "probe - %d, sum - %d" probe curSum
        findFewestNumWithSumDivisitorGreaterThan sum (probe+1) extraFilter

//let houseNumberWithRequiredElvesPresents = findFewestNumWithSumDivisitorGreaterThan (elvesPresentsTotal/10) 665000 (fun x -> true)
let houseNumberWithRequiredElvesPresents2 = findFewestNumWithSumDivisitorGreaterThan (elvesPresentsTotal/11) 665000 (fun x -> x*50 > elvesPresentsTotal/11 )
