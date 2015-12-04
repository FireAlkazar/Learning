#load "Chapter5RegisterMachine.fs"

open RegisterMachine

let registers = ["a";"b";"t"]

let ambiguityLabelController : ControllerInstruction list = 
    [
        Goto("here")
        Label("here")
        Assign("a", FromConst(3))
        Goto("there")
        Label("here")
        Assign("a", FromConst(4))
        Goto("there")
        Label("there")
    ]

let machine =
    makeMachine registers [] ambiguityLabelController

machine.Execute()
let result = machine.LookupRegister("a")

// if we need fail with error on same labels, then subsitute
// A:
//if labels.ContainsKey(x) then labels.[x] <- i
//else labels.Add(x,i)
// for B:
//labels.Add(x,i)

