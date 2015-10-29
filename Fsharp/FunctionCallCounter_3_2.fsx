type MonitoredArgType = 
    | Str of string
    | Float of float

let makeMonitored (f : float -> float) =
    let count = ref 0.0
    fun (x:MonitoredArgType) -> 
        match x with
            | Str str  ->
                match str with
                    | "showCalls" -> !count
                    | "resetCalls" ->
                        count := 0.0
                        !count
                    | _ -> 
                        printfn "error"
                        800001.0
            | Float y -> 
                count := !count + 1.0
                f y

let monitoredSqrt = makeMonitored System.Math.Sqrt
let arg1 = Float(100.0)
let arg2 = Str("showCalls")
let arg3 = Str("resetCalls")

let sqrtOf100 = monitoredSqrt arg1
let callsNum = monitoredSqrt arg2
let afterResetCalls = monitoredSqrt arg3
//monitoredSqrt arg3
//monitoredSqrt arg2


