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
        | AssignFromRegister of string
        | AssignFromOperation of string*OperationSource*OperationSource
        | AssignFromConst of int

    type ControllerInstruction =
        | Label of string
        | Test of string*OperationSource*OperationSource
        | Branch of string
        | Assign of string*AssignSource
        | Goto of string
        | Save of string
        | Restore of string

    type ParsedControllerInstruction = ControllerInstruction*(unit -> unit)

    type Machine() =  
        let registers = new System.Collections.Generic.Dictionary<string,int>()
        let labels = new System.Collections.Generic.Dictionary<string,List<ParsedControllerInstruction>>()
        let stack = new System.Collections.Generic.Stack<int>()
        let instructions : string list = []
        let mutable operations : Operation list = []
        let mutable pc : ParsedControllerInstruction list = []
        let advancePc () =
            if List.isEmpty pc then ()
            pc <- List.tail pc
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
                            | AssignFromRegister(r) -> registers.[r]
                            | AssignFromOperation(op,s1,s2) -> getCalcFunc(op) (getValue(s1)) (getValue(s2))
                            | AssignFromConst(x) -> x
                        registers.[x] <- resolvedValue
                        advancePc ()
                | Test(op,s1,s2) -> 
                    fun() -> 
                       let isTestSucceeded = getPredicateFunc(op) (getValue(s1)) (getValue(s2))
                       registers.["flag"] <- (if isTestSucceeded then 1 else 0)
                       advancePc ()
                | Branch(x) ->
                    fun () -> 
                        let labelInstructions = labels.[x]
                        if registers.["flag"] = 1 then pc <- labelInstructions
                        else advancePc ()
                | Goto(x) ->
                    fun () -> 
                        let labelInstructions = labels.[x]
                        pc <- labelInstructions
                | Save(x) -> 
                    fun () -> 
                        let value = registers.[x]
                        stack.Push(value)
                        advancePc ()
                | Restore(x) ->
                    fun() ->
                        registers.[x] <- stack.Pop()
                        advancePc ()
                | _ -> failwith "The instruction should not be here"
        let rec extractLabels (insts : ControllerInstruction list) =
            match insts with
                | [] -> ([],[])
                | h::t -> 
                    match h with
                        | Label(x) -> 
                            let (l,i) = extractLabels t
                            if labels.ContainsKey(x) then labels.[x] <- i
                            else labels.Add(x,i)
                            (h::l,i)
                        | _ -> 
                            let parsedIntruction = (h,(makeInstruction h))
                            let (l,i) = extractLabels t
                            (l,parsedIntruction::i)
        do
            registers.Add("flag",0)
        member this.AllocateRegister(name:string) = registers.Add(name,0)
        member this.LookupRegister(name:string) = registers.[name]
        member this.SetRegister(name,value) = registers.[name] <- value
        member this.InstallOperations(ops) = operations <- ops
        member this.Assemble(controller) =
            let (labels, insts) = extractLabels controller
            pc <- insts
        member this.Execute() = 
            let rec recursiveExecute () =
                if List.isEmpty pc then printfn "done"
                else 
                    let (_, instructionFunc) = List.head pc
                    instructionFunc ()
                    recursiveExecute ()
            recursiveExecute ()

    let makeMachine (registers: string list) (operations: Operation list) (controller : ControllerInstruction list) =
        let machine = new Machine()
        List.iter (fun x -> machine.AllocateRegister(x)) registers
        machine.InstallOperations(operations)
        machine.Assemble(controller)
        machine
