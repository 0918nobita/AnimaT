module AnimaT

open FParsec.CharParsers

let parserResult = run pfloat "1.2"

match parserResult with
| Success(result, _, _) ->
    printfn "Result: %f" result
| Failure(_) ->
    ()
