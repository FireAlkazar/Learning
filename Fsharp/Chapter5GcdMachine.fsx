#load "Chapter5RegisterMachine.fs"

open RegisterMachine

let gcdRegisters = ["a";"b";"t"]
let gcdOperations =
     [
        {Name="rem"; Func=Calc(fun x y -> x % y)}
        {Name="="; Func=Predicate(fun x y -> x = y)}
     ]
//вычисление НОД двух чисел
let gcdController : ControllerInstruction list = 
    [
        Label("test-b")
        Test("=", RegisterRef("b"),Const(0))
        Branch("gcd-done")
        Assign("t", AssignFromOperation("rem",RegisterRef("a"),RegisterRef("b")))
        Assign("a",AssignFromRegister("b"))
        Assign("b", AssignFromRegister("t"))
        Goto("test-b")
        Label("gcd-done")
    ]

let gcdMachine =
    makeMachine gcdRegisters  gcdOperations gcdController

gcdMachine.SetRegister("a", 18)
gcdMachine.SetRegister("b", 12)
gcdMachine.Execute()
let result = gcdMachine.LookupRegister("a")
