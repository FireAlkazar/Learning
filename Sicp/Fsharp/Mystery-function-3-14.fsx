let mystery (x : 'a list) =
    let rec loop x y =
        if List.isEmpty x then y
        else
            let temp = List.tail x
            let newX = List.head x :: y
            loop temp newX
    loop x List.empty

let argList = ['a';'b';'c';'d']

let result = mystery argList

//so function make reverse
//and in original version also reverses argList