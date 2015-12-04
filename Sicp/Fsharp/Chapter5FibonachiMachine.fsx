#load "Chapter5RegisterMachine.fs"

open RegisterMachine

let fibRegisters = ["continue";"n";"val"]
let fibOperations =
     [
        {Name="+"; Func=Calc(fun x y -> x + y)}
        {Name="-"; Func=Calc(fun x y -> x - y)}
        {Name="<"; Func=Predicate(fun x y -> x < y)}
     ]
let fibController : ControllerInstruction list = 
    [
        Assign("continue", FromLabel("fib-done"))
        Label("fib-loop")
        Test("<", RegisterRef("n"), Const(2))
        Branch("immediate-answer")
        Save("continue")
        Assign("continue", FromLabel("afterfib-n-1"))
        Save("n")
        Assign("n",FromOperation("-",RegisterRef("n"), Const(1)))
        Goto("fib-loop")
        Label("afterfib-n-1")
        Restore("n")
        Restore("continue")
        Assign("n",FromOperation("-",RegisterRef("n"),Const(2)))
        Save("continue")
        Assign("continue", FromLabel("afterfib-n-2"))
        Save("val")
        Goto("fib-loop")
        Label("afterfib-n-2")
        Assign("n", FromRegister("val"))
        Restore("val")
        Restore("continue")
        Assign("val", FromOperation("+", RegisterRef("val"), RegisterRef("n")))
        GotoReg("continue")
        Label("immediate-answer")
        Assign("val", FromRegister("n"))
        GotoReg("continue")
        Label("fib-done")
    ]

let fibMachine =
    makeMachine fibRegisters fibOperations fibController

fibMachine.SetRegister("n", 8)
fibMachine.Execute()
let result = fibMachine.LookupRegister("val")
