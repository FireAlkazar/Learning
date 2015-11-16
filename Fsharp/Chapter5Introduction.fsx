let rec gcd a b =
    if b = 0 then a
    else gcd b ( a % b)

let test1 = gcd 50 40

type Function = | Predicate of  (int -> int -> bool)
                | Calc of (int -> int -> int)

type Operation = 
    {
        Name: string
        Func: Function 
    }

let makeMachine (registers: string list) (operations: Operation list) (controller : string list) =
    failwith("ni") |> ignore

let gcdRegisters = ["a";"b";"t"]
let gcdOperations =
     [
        {Name="rem"; Func=Calc(fun x y -> x % y)}
        {Name="="; Func=Predicate(fun x y -> x = y)}
     ]
let gcdController = 
    [
        "test-b"
        "test (op =) (reg b) (const 0)"
        "branch (label gcd-done)"
        "assign t (op rem) (reg a) (reg b)"
        "assign a (reg b)"
        "assign b (reg t)"
        "goto (label test-b)"
        "gcd-done"
    ]

let gcdMachine =
    makeMachine gcdRegisters  gcdOperations gcdController

//setRegister gcdMachine "a" 206
//setRegister gcdMachine "b" 40
//start gcdMachine
//let result = getRegister gcdMachine "a"