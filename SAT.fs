module SAT

type Expr =
    | T
    | F
    | And of Expr * Expr
    | Or of Expr * Expr
    | Implies of Expr * Expr
    | Not of Expr
    | Var of int

    member inline this.IsSimple =
        match this with
        | T | F | Var(_) -> true
        | _ -> false

    member this.MaxIndex =
        match this with
        | T | F -> -1
        | And(lhs, rhs)
        | Or(lhs, rhs)
        | Implies(lhs, rhs) ->
            max (lhs.MaxIndex) (rhs.MaxIndex)
        | Not(expr) -> expr.MaxIndex
        | Var(index) -> index

    override this.ToString() =
        match this with
        | T -> "T"
        | F -> "F"

        | And(lhs, rhs) when lhs.IsSimple && rhs.IsSimple ->
            sprintf "%O ∧ %O" lhs rhs
        | And(lhs, rhs) when lhs.IsSimple ->
            sprintf "%O ∧ (%O)" lhs rhs
        | And(lhs, rhs) when rhs.IsSimple ->
            sprintf "(%O) ∧ %O" lhs rhs
        | And(lhs, rhs) ->
            sprintf "(%O) ∧ (%O)" lhs rhs

        | Or(lhs, rhs) when lhs.IsSimple && rhs.IsSimple ->
            sprintf "%O ∨ %O" lhs rhs
        | Or(lhs, rhs) when lhs.IsSimple ->
            sprintf "%O ∨ (%O)" lhs rhs
        | Or(lhs, rhs) when rhs.IsSimple ->
            sprintf "(%O) ∨ %O" lhs rhs
        | Or(lhs, rhs) ->
            sprintf "(%O) ∨ (%O)" lhs rhs

        | Implies(lhs, rhs) when lhs.IsSimple && rhs.IsSimple ->
            sprintf "%O → %O" lhs rhs
        | Implies(lhs, rhs) when lhs.IsSimple ->
            sprintf "%O → (%O)" lhs rhs
        | Implies(lhs, rhs) when rhs.IsSimple ->
            sprintf "(%O) → %O" lhs rhs
        | Implies(lhs, rhs) ->
            sprintf "(%O) → (%O)" lhs rhs

        | Not(expr) when expr.IsSimple -> sprintf "￢%O" expr
        | Not(expr) -> sprintf "￢(%O)" expr

        | Var(index) -> string(index)

    member this.ToBool() =
        match this with
        | T -> true
        | F -> false
        | And(lhs, rhs) -> lhs.ToBool() && rhs.ToBool()
        | Or(lhs, rhs) -> lhs.ToBool() || rhs.ToBool()
        | Implies(lhs, rhs) -> if lhs.ToBool() then rhs.ToBool() else true
        | Not(expr) -> not (expr.ToBool())
        | Var(_) -> failwith "cannot convert to boolean because it contains free variable(s)"

    member this.EvalWith(env: bool list) =
        match this with
        | T -> true
        | F -> false
        | And(lhs, rhs) -> lhs.EvalWith(env) && rhs.EvalWith(env)
        | Or(lhs, rhs) -> lhs.EvalWith(env) || rhs.EvalWith(env)
        | Implies(lhs, rhs) -> if lhs.EvalWith(env) then rhs.EvalWith(env) else true
        | Not(expr) -> not (expr.EvalWith(env))
        | Var(index) -> env.[index]

let solve (expr : Expr) =
    let length = expr.MaxIndex
    // TODO: implement SAT solver
    length
