open FParsec.Primitives
open FParsec.CharParsers

let parserResult = run pfloat "1.2"

match parserResult with
| Success(result, _, _) ->
    printfn "(1) Result: %f" result
| Failure(_) ->
    ()

let parser1: Parser<string, unit> =
    preturn "Hello"
    >>= (fun s ->
        preturn (s + "!"))

match run parser1 "" with
| Success(result, _, _) ->
    printfn "(2) Result: %s" result
| Failure(_) ->
    ()

let identParser: Parser<string, unit> =
    IdentifierOptions(
        isAsciiIdStart = (fun c -> isAsciiLetter c || c = '_'),
        isAsciiIdContinue = fun c -> isAsciiLetter c || isDigit c || c = '_' || c = '\''
    )
    |> identifier

let parser2: Parser<string, unit> =
    pstring "let"
    >>. spaces1
    >>. identParser

match run parser2 "let _ = log 'hello'" with
| Success(result, _, _) ->
    printfn "(3) Result: %s" result
| Failure(_) ->
    ()

open SAT

let expr1 = Not(And(T, Or(F, T)))

printfn "(4) Expr: %O" expr1
printfn "(5) EvaluatedTo: %b" <| expr1.ToBool()

let expr2 = Or(And(Var(0), Not(Var(1))), And(Not(Var(0)), Var(1)))

printfn "(6) Expr: %O" expr2
printfn "(7) env: [true, true] -> %b" <| expr2.EvalWith([true; true])
printfn "(8) env: [true, false] -> %b" <| expr2.EvalWith([true; false])
printfn "(9) env: [false, true] -> %b" <| expr2.EvalWith([false; true])
printfn "(10) env: [false, false] -> %b" <| expr2.EvalWith([false; false])
