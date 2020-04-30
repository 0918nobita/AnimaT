module SAT

type Expr =
    | T
    | F
    | And of Expr * Expr
    | Or of Expr * Expr
    | Not of Expr
    | Var of int

    member private this.IsSimple() =
        match this with
        | T | F | Var(_) -> true
        | _ -> false

    override this.ToString() =
        match this with
        | T -> "T"
        | F -> "F"

        | And(lhs, rhs) when lhs.IsSimple() && rhs.IsSimple() ->
            lhs.ToString() + " ∧ " + rhs.ToString()
        | And(lhs, rhs) when lhs.IsSimple() ->
            lhs.ToString() + " ∧ (" + rhs.ToString() + ")"
        | And(lhs, rhs) when rhs.IsSimple() ->
            "(" + lhs.ToString() + ") ∧ " + rhs.ToString()
        | And(lhs, rhs) ->
            "(" + lhs.ToString() + ") ∧ (" + rhs.ToString() + ")"

        | Or(lhs, rhs) when lhs.IsSimple() && rhs.IsSimple() ->
            lhs.ToString() + " ∨ " + rhs.ToString()
        | Or(lhs, rhs) when lhs.IsSimple() ->
            lhs.ToString() + " ∨ (" + rhs.ToString() + ")"
        | Or(lhs, rhs) when rhs.IsSimple() ->
            "(" + lhs.ToString() + ") ∨ " + rhs.ToString()
        | Or(lhs, rhs) ->
            "(" + lhs.ToString() + ") ∨ (" + rhs.ToString() + ")"

        | Not(expr) when expr.IsSimple() -> "￢" + expr.ToString()
        | Not(expr) -> "￢(" + expr.ToString() + ")"

        | Var(index) -> string(index)

    member this.ToBool() =
        match this with
        | T -> true
        | F -> false
        | And(lhs, rhs) -> lhs.ToBool() && rhs.ToBool()
        | Or(lhs, rhs) -> lhs.ToBool() || rhs.ToBool()
        | Not(expr) -> not (expr.ToBool())
        | Var(_) -> failwith "cannot convert to boolean because it contains free variable(s)"

    member this.EvalWith(env: bool list) =
        match this with
        | T -> true
        | F -> false
        | And(lhs, rhs) -> lhs.EvalWith(env) && rhs.EvalWith(env)
        | Or(lhs, rhs) -> lhs.EvalWith(env) || rhs.EvalWith(env)
        | Not(expr) -> not (expr.EvalWith(env))
        | Var(index) -> env.[index]
