let rec gcd a b =
    if b = 0 then a
    else gcd b ( a % b)

let test1 = gcd 50 40

type ValueSource = 
    | Constant of int
    | RegisterRef of string
    | OperationRef of string

type Button =
    {
        Name : string
        Source : ValueSource
    }

type Register = 
    { 
        Name : string
        Buttons : Button list
    }

let dataPaths =
    [
        { Name = "a"; Buttons = [{ Name = "a<-b"; Source = RegisterRef("b")}]}
        { Name = "b"; Buttons = [{ Name = "b<-t"; Source = RegisterRef("t")}]}
        { Name = "t"; Buttons = [{ Name = "t<-r"; Source = OperationRef("rem")}]}
    ]

type Operation = 
    {
        Name : string
        Inputs : ValueSource list
    }

let operations = 
    [
        { Name = "rem"; Inputs = [RegisterRef("a"); RegisterRef("b")]}
        { Name = "="; Inputs = [RegisterRef("b"); Constant(0)]}
    ]

let controller =
    [
        "test-b"
        "test ="
        "branch label gcd-done"
        "t<-r"
        "a<-b"
        "b<-t"
        "goto label test-b"
        "gcd-done"
    ]

let controller2 =
    [
        "test-b"
        "test (op =) (reg b) (const 0)"
        "branch label gcd-done"
        "assign t (op rem) (reg a) (reg b)"
        "assign a (reg b)"
        "assign b (reg t)"
        "goto label test-b"
        "gcd-done"
    ]