module RegisterMachine
    type Function = 
    | Predicate of  (int -> int -> bool)
    | Calc of (int -> int -> int)

    type Operation = 
        {
            Name: string
            Func: Function 
        }

    type OperationSource =
        | RegisterRef of string
        | Const of int

    type AssignSource =
        | FromRegister of string
        | FromOperation of string*OperationSource*OperationSource
        | FromConst of int
        | FromLabel of string

    type ControllerInstruction =
        | Label of string
        | Test of string*OperationSource*OperationSource
        | Branch of string
        | Assign of string*AssignSource
        | Goto of string
        | GotoReg of string
        | Save of string
        | Restore of string

    type ParsedControllerInstruction = ControllerInstruction*(unit -> unit)

    type Machine() =  
        let registers = new System.Collections.Generic.Dictionary<string,int>()
        let labelToIndexMap = new System.Collections.Generic.Dictionary<string,int>()
        let mutable instructionIndexCounter = -1;
        let indexToInstructionMap = new System.Collections.Generic.Dictionary<int,ParsedControllerInstruction>()
        let stack = new System.Collections.Generic.Stack<int>()
        let instructions : string list = []
        let mutable operations : Operation list = []
        let advancePc () =
            if registers.["pc"] = -1 then ()
            registers.["pc"] <- registers.["pc"] - 1
        let getCalcFunc (operationName:string) =
            let operation = List.find (fun x-> x.Name = operationName) operations
            match operation.Func with
                | Predicate(_) -> failwith "unexpected Calc func - predicate"
                | Calc(x) -> x
        let getPredicateFunc (operationName:string) =
            let operation = List.find (fun x-> x.Name = operationName) operations
            match operation.Func with
                | Predicate(x) -> x
                | Calc(_) -> failwith "unexpected Predicate func - calc"
        let getValue (s: OperationSource) =
            match s with
                | RegisterRef(x) -> registers.[x]
                | Const(x) -> x
        let makeInstruction (inst : ControllerInstruction) =
            match inst with
                | Assign(x,y) ->
                    fun () ->
                        let resolvedValue =
                            match y with
                            | FromRegister(r) -> registers.[r]
                            | FromOperation(op,s1,s2) -> getCalcFunc(op) (getValue(s1)) (getValue(s2))
                            | FromConst(x) -> x
                            | FromLabel(x) -> labelToIndexMap.[x]
                        registers.[x] <- resolvedValue
                        advancePc ()
                | Test(op,s1,s2) -> 
                    fun() -> 
                       let isTestSucceeded = getPredicateFunc(op) (getValue(s1)) (getValue(s2))
                       registers.["flag"] <- (if isTestSucceeded then 1 else 0)
                       advancePc ()
                | Branch(x) ->
                    fun () -> 
                        let labelInstructions = labelToIndexMap.[x]
                        if registers.["flag"] = 1 then registers.["pc"] <- labelToIndexMap.[x]
                        else advancePc ()
                | Goto(x) ->
                    fun () -> 
                        registers.["pc"] <- labelToIndexMap.[x]
                | GotoReg(x) ->
                    fun () ->
                        let regValue = registers.[x]
                        registers.["pc"] <- regValue
                | Save(x) -> 
                    fun () -> 
                        let value = registers.[x]
                        stack.Push(value)
                        printfn "saved register %A %A" x value
                        advancePc ()
                | Restore(x) ->
                    fun() ->
                        registers.[x] <- stack.Pop()
                        printfn "restored register %A %A" x (registers.[x])
                        advancePc ()
                | _ -> failwith "The instruction should not be here"
        let rec extractLabels (insts : ControllerInstruction list) =
            match insts with
                | [] -> ([],[])
                | h::t -> 
                    match h with
                        | Label(x) -> 
                            let (l,i) = extractLabels t
                            if labelToIndexMap.ContainsKey(x) then labelToIndexMap.[x] <- instructionIndexCounter
                            else labelToIndexMap.Add(x,instructionIndexCounter)
                            (h::l,i)
                        | _ -> 
                            let (l,i) = extractLabels t
                            let parsedIntruction = (h,(makeInstruction h))
                            instructionIndexCounter <- instructionIndexCounter + 1
                            indexToInstructionMap.Add(instructionIndexCounter, parsedIntruction)
                            (l,parsedIntruction::i)
        do
            registers.Add("flag",0)
            registers.Add("pc",0)
        member this.AllocateRegister(name:string) = registers.Add(name,0)
        member this.LookupRegister(name:string) = registers.[name]
        member this.SetRegister(name,value) = registers.[name] <- value
        member this.InstallOperations(ops) = operations <- ops
        member this.Assemble(controller) =
            let (labels, insts) = extractLabels controller
            registers.["pc"] <- instructionIndexCounter
        member this.Execute() = 
            let rec recursiveExecute () =
                if registers.["pc"] = -1 then printfn "done"
                else 
                    let index = registers.["pc"]
                    let (_, instructionFunc) = indexToInstructionMap.[index]
                    instructionFunc ()
                    recursiveExecute ()
            recursiveExecute ()

    let makeMachine (registers: string list) (operations: Operation list) (controller : ControllerInstruction list) =
        let machine = new Machine()
        List.iter (fun x -> machine.AllocateRegister(x)) registers
        machine.InstallOperations(operations)
        machine.Assemble(controller)
        machine
