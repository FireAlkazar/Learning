//taken from http://blogs.msdn.com/b/ashleyf/archive/2010/09/24/fscheme-0-0-0.aspx
open System
open System.Numerics 

type Token =
    | Open | Close
    | Number of string
    | String of string
    | Symbol of string

let tokenize source =
    let rec string acc = function
        | '\\' :: '"' :: t -> string (acc + "\"") t // escaped quote becomes quote
        | '"' :: t -> acc, t // closing quote terminates
        | c :: t -> string (acc + (c.ToString())) t // otherwise accumulate chars
        | _ -> failwith "Malformed string."
    let rec token acc = function
        | (')' :: _) as t -> acc, t // closing paren terminates
        | w :: t when Char.IsWhiteSpace(w) -> acc, t // whitespace terminates
        | [] -> acc, [] // end of list terminates
        | c :: t -> token (acc + (c.ToString())) t // otherwise accumulate chars
    let rec tokenize' acc = function
        | w :: t when Char.IsWhiteSpace(w) -> tokenize' acc t // skip whitespace
        | '(' :: t -> tokenize' (Open :: acc) t
        | ')' :: t -> tokenize' (Close :: acc) t
        | '"' :: t -> // start of string
            let s, t' = string "" t
            tokenize' (Token.String(s) :: acc) t'
        | '-' :: d :: t when Char.IsDigit(d) -> // start of negative number
            let n, t' = token ("-" + d.ToString()) t
            tokenize' (Token.Number(n) :: acc) t'
        | '+' :: d :: t | d :: t when Char.IsDigit(d) -> // start of positive number
            let n, t' = token (d.ToString()) t
            tokenize' (Token.Number(n) :: acc) t'
        | s :: t -> // otherwise start of symbol
            let s, t' = token (s.ToString()) t
            tokenize' (Token.Symbol(s) :: acc) t'
        | [] -> List.rev acc // end of list terminates
    tokenize' [] source 

type Expression =
    | Number of BigInteger
    | String of string
    | Symbol of string
    | List of Expression list
    | Function of (Expression list -> Expression)
    | Special of (Expression list -> Expression)

let parse source =
    let map = function
        | Token.Number(n) -> Expression.Number(BigInteger.Parse(n))
        | Token.String(s) -> Expression.String(s)
        | Token.Symbol(s) -> Expression.Symbol(s)
        | _ -> failwith "Syntax error."
    let rec parse' acc = function
        | Open :: t ->
            let e, t' = parse' [] t
            parse' (List(e) :: acc) t'
        | Close :: t -> (List.rev acc), t
        | h :: t -> parse' ((map h) :: acc) t
        | [] -> (List.rev acc), []
    let result, _ = parse' [] (tokenize source)
    result


let rec print = function
    | List(list) -> "(" + String.Join(" ", (List.map print list)) + ")"
    | String(s) ->
        let escape = String.collect (function '"' -> "\\\"" | c -> c.ToString()) // escape quotes
        "\"" + (escape s) + "\""
    | Symbol(s) -> s
    | Number(n) -> n.ToString()
    | Function(_) | Special(_) -> "Function"

let Multiply args =
    let prod a = function Number(b) -> a * b | _ -> failwith "Malformed multiplication argument." 
    Number(List.fold prod 1I args)

let Subtract = function
    | [] -> Number(0I) // (-) == 0
    | [Number(n)] -> Number(-n) // (- a) == –a
    | Number(n) :: ns -> // (- a b c) == a - b – c
        let sub a = function Number(b) -> a - b | _ -> failwith "Malformed subtraction argument."
        Number(List.fold sub n ns)
    | _ -> failwith "Malformed subtraction."

let rec If = function
    | [condition; t; f] ->
        match eval condition with
        | List([]) | String("") -> eval f // empty list or empty string is false
        | Number(n) when n = 0I -> eval f // zero is false
        | _ -> eval t // everything else is true
    | _ -> failwith "Malformed 'if'."

and environment =
    Map.ofList [
        "*", Function(Multiply)
        "-", Function(Subtract)
        "if", Special(If)] 

and eval expression =
    match expression with
    | Number(_) as lit -> lit
    | String(_) as lit -> lit
    | Symbol(s) -> environment.[s] 
    | List([]) -> List([])
    | List(h :: t) -> 
        match eval h with
        | Function(f) -> apply f t
        | Special(f) -> f t 
        | _ -> failwith "Malformed expression."
    | _ -> failwith "Malformed expression."

and apply fn args = fn (List.map eval args) 

let rep = List.ofSeq >> parse >> List.head >> eval >> print

let rez = rep "(* 2 3 5)"