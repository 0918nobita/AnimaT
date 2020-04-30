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

let showExpr2 (expr : Expr) =
    let pairs = [[true; true]; [true; false]; [false; true]; [false; false]]
    let printRow (pair : bool list) =
        printfn "%5b %5b | %5b" pair.[0] pair.[1] <| expr.EvalWith(pair)
    printfn "\nExpr: %O" expr
    printfn "-------------------"
    List.iter printRow pairs
    printfn "-------------------"

let expr2 = Or(And(Var(0), Not(Var(1))), And(Not(Var(0)), Var(1)))
showExpr2 expr2

let expr3 = Not(Implies(Var(0), Var(1)))
showExpr2 expr3
