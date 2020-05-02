module SAT

type Expr =
    | T
    | F
    | AndE of Expr * Expr
    | OrE of Expr * Expr
    | Implies of Expr * Expr
    | NotE of Expr
    | Var of int

    member inline this.IsSimple =
        match this with
        | T
        | F
        | Var(_) -> true
        | _ -> false

    member this.MaxIndex =
        match this with
        | T
        | F -> -1
        | AndE(lhs, rhs)
        | OrE(lhs, rhs)
        | Implies(lhs, rhs) -> max (lhs.MaxIndex) (rhs.MaxIndex)
        | NotE(expr) -> expr.MaxIndex
        | Var(index) -> index

    override this.ToString() =
        match this with
        | T -> "T"
        | F -> "F"

        | AndE(lhs, rhs) when lhs.IsSimple && rhs.IsSimple -> sprintf "%O ∧ %O" lhs rhs
        | AndE(lhs, rhs) when lhs.IsSimple -> sprintf "%O ∧ (%O)" lhs rhs
        | AndE(lhs, rhs) when rhs.IsSimple -> sprintf "(%O) ∧ %O" lhs rhs
        | AndE(lhs, rhs) -> sprintf "(%O) ∧ (%O)" lhs rhs

        | OrE(lhs, rhs) when lhs.IsSimple && rhs.IsSimple -> sprintf "%O ∨ %O" lhs rhs
        | OrE(lhs, rhs) when lhs.IsSimple -> sprintf "%O ∨ (%O)" lhs rhs
        | OrE(lhs, rhs) when rhs.IsSimple -> sprintf "(%O) ∨ %O" lhs rhs
        | OrE(lhs, rhs) -> sprintf "(%O) ∨ (%O)" lhs rhs

        | Implies(lhs, rhs) when lhs.IsSimple && rhs.IsSimple -> sprintf "%O → %O" lhs rhs
        | Implies(lhs, rhs) when lhs.IsSimple -> sprintf "%O → (%O)" lhs rhs
        | Implies(lhs, rhs) when rhs.IsSimple -> sprintf "(%O) → %O" lhs rhs
        | Implies(lhs, rhs) -> sprintf "(%O) → (%O)" lhs rhs

        | NotE(expr) when expr.IsSimple -> sprintf "￢%O" expr
        | NotE(expr) -> sprintf "￢(%O)" expr

        | Var(index) -> string (index)

    member this.ToBool() =
        match this with
        | T -> true
        | F -> false
        | AndE(lhs, rhs) -> lhs.ToBool() && rhs.ToBool()
        | OrE(lhs, rhs) -> lhs.ToBool() || rhs.ToBool()
        | Implies(lhs, rhs) ->
            if lhs.ToBool() then rhs.ToBool() else true
        | NotE(expr) -> not (expr.ToBool())
        | Var(_) -> failwith "cannot convert to boolean because it contains free variable(s)"

    member this.EvalWith(env: bool list) =
        match this with
        | T -> true
        | F -> false
        | AndE(lhs, rhs) -> lhs.EvalWith(env) && rhs.EvalWith(env)
        | OrE(lhs, rhs) -> lhs.EvalWith(env) || rhs.EvalWith(env)
        | Implies(lhs, rhs) ->
            if lhs.EvalWith(env) then rhs.EvalWith(env) else true
        | NotE(expr) -> not (expr.EvalWith(env))
        | Var(index) -> env.[index]

/// Literal of Conjunctive Normal Form
type Lit =
    | Sym of int
    | Not of int

/// Clause of Conjunctive Normal Form
type Cl =
    | Lit of Lit
    | Or of Cl * Cl

/// Conjunctive Normal Form
type CNF = Cl list

/// x ↔ ￢y ⇔ (x ∨ y) ∧ (￢x ∨ ￢y)
let pattern1: CNF =
    [ Or(Lit(Sym(0)), Lit(Sym(1)))
      Or(Lit(Not(0)), Lit(Not(1))) ]

/// x ↔ (y ∨ z) ⇔ (x ∨ ￢y) ∧ (x ∨ ￢z) ∧ (￢x ∨ y ∨ z)
let pattern2: CNF =
    [ Or(Lit(Sym(0)), Lit(Not(1)))
      Or(Lit(Sym(0)), Lit(Not(2)))
      Or(Lit(Not(0)), Or(Lit(Sym(1)), Lit(Sym(2)))) ]

/// x ↔ (y ∧ z) ⇔ (￢x ∨ y) ∧ (￢x ∨ z) ∧ (x ∨ ￢y ∨ ￢z)
let pattern3: CNF =
    [ Or(Lit(Not(0)), Lit(Sym(1)))
      Or(Lit(Not(0)), Lit(Sym(2)))
      Or(Lit(Sym(0)), Or(Lit(Not(1)), Lit(Not(2)))) ]

/// x ↔ (y → z) ⇔ (x ∨ y) ∧ (x ∨ ￢z) ∧ (￢x ∨ ￢y ∨ z)
let pattern4: CNF =
    [ Or(Lit(Sym(0)), Lit(Sym(1)))
      Or(Lit(Sym(0)), Lit(Not(2)))
      Or(Lit(Not(0)), Or(Lit(Not(1)), Lit(Sym(2)))) ]

// Expr -> bool
let sat expr =
    // TODO: implement SAT solver
    true
