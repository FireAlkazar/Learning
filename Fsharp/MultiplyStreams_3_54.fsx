//taken from http://www.fssnip.net/sD
let force (value : Lazy<_>) = value.Force()

// Infinite Stream
type Stream<'T> = Cons of 'T * Lazy<Stream<'T>>
let head (Cons (h, _)) = h
let tail (Cons (_, t)) = force t 

// Lazy fixed-point
let fix : (Lazy<'T> -> 'T) -> Lazy<'T> = fun f ->
 let rec x = lazy (f x) in x 

// Examples  
let ones = fix (fun x -> Cons (1, x))
let map f = fix (fun f' x -> Cons (f (head x), lazy(force f' (tail x)))  )
let nats = fix (fun x -> Cons (1, lazy ( (force (map ((+) 1))) (force x)  )))

//my code

let map2 f = fix (fun f' x y -> Cons (f (head x) (head y), lazy(force f' (tail x) (tail y)))  )

let rec nth stream count =
    if count = 0 then head stream
    else nth (tail stream) (count-1)

let add s1 s2 =
    lazy((force (map2 (+)))  (force s1) (force s2))

let integers = fix(fun x -> Cons(1, add ones x))
let rez3 = nth (force integers) 6

let mul s1 s2 =
    lazy((force (map2 (*)))  (force s1) (force s2))
let factorials = fix(fun x -> Cons(1, mul integers x))
let rez4 = nth (force factorials) 4