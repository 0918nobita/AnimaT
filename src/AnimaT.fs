open SAT

let expr1 = NotE(AndE(T, OrE(F, T)))

printfn "(1) Expr: %O" expr1
printfn "(2) EvaluatedTo: %b" <| expr1.ToBool()

let showExpr2 (expr: Expr) =
    let pairs =
        [ [ true; true ]
          [ true; false ]
          [ false; true ]
          [ false; false ] ]

    let printRow (pair: bool list) = printfn "%5b %5b | %5b" pair.[0] pair.[1] <| expr.EvalWith(pair)
    printfn "\nExpr: %O" expr
    printfn "-------------------"
    List.iter printRow pairs
    printfn "-------------------"

let expr2 = OrE(AndE(Var(0), NotE(Var(1))), AndE(NotE(Var(0)), Var(1)))

showExpr2 expr2

let expr3 = NotE(Implies(Var(0), Var(1)))

showExpr2 expr3

open JsAst

let varDecltr: VariableDeclarator =
    let ident: Identifier = { Name = "foo" }
    let init: BooleanLiteral = { Value = true }
    { Id = ident
      Init = init }

let varDeclStmt: VariableDeclaration =
    { Declarations = [ varDecltr ]
      Kind = Const }

let program: Program = { Body = [ varDeclStmt ] }

JsGen.generate program

open TypeSys

let t1 = TFun(TFloat, TFun(TInt, TInt))
let t2 = TFun(TFun(TFloat, TInt), TInt)
printfn "(3) Type: %O" t1
printfn "(4) Type: %O" t2
