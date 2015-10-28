let makeAccumulator initial = 
    let sum = ref initial
    fun x -> 
        sum := !sum + x
        sum

let acc = makeAccumulator 100

acc 30
acc 25

