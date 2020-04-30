module SAT

type BaseExpr =
    | T
    | F
    | And of BaseExpr * BaseExpr
    | Or of BaseExpr * BaseExpr
    | Not of BaseExpr

    member private this.IsTOrF() = this = T || this = F

    override this.ToString() =
        match this with
        | T -> "T"
        | F -> "F"

        | And(lhs, rhs) when lhs.IsTOrF() && rhs.IsTOrF() ->
            lhs.ToString() + " ∧ " + rhs.ToString()
        | And(lhs, rhs) when lhs.IsTOrF() ->
            lhs.ToString() + " ∧ (" + rhs.ToString() + ")"
        | And(lhs, rhs) when rhs.IsTOrF() ->
            "(" + lhs.ToString() + ") ∧ " + rhs.ToString()
        | And(lhs, rhs) ->
            "(" + lhs.ToString() + ") ∧ (" + rhs.ToString() + ")"

        | Or(lhs, rhs) when lhs.IsTOrF() && rhs.IsTOrF() ->
            lhs.ToString() + " ∨ " + rhs.ToString()
        | Or(lhs, rhs) when lhs.IsTOrF() ->
            lhs.ToString() + " ∨ (" + rhs.ToString() + ")"
        | Or(lhs, rhs) when rhs.IsTOrF() ->
            "(" + lhs.ToString() + ") ∨ " + rhs.ToString()
        | Or(lhs, rhs) ->
            "(" + lhs.ToString() + ") ∨ (" + rhs.ToString() + ")"

        | Not(expr) when expr.IsTOrF() -> "￢" + expr.ToString()
        | Not(expr) -> "￢(" + expr.ToString() + ")"

    member this.ToBool() =
        match this with
        | T -> true
        | F -> false
        | And(lhs, rhs) -> lhs.ToBool() && rhs.ToBool()
        | Or(lhs, rhs) -> lhs.ToBool() || rhs.ToBool()
        | Not(expr) -> not (expr.ToBool())
