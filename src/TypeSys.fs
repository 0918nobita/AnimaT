module TypeSys

type Type =
    | TBool
    | TInt
    | TFloat
    | TChar
    | TString
    | TFun of Type * Type
    | TVar of int

    member inline this.IsSimple =
        match this with
        | TFun _ -> false
        | _ -> true

    override this.ToString() =
        match this with
        | TBool -> "Bool"
        | TInt -> "Int"
        | TFloat -> "Float"
        | TChar -> "Char"
        | TString -> "String"
        | TFun (arg, ret) when not arg.IsSimple -> sprintf "(%O) -> %O" arg ret
        | TFun (arg, ret) -> sprintf "%O -> %O" arg ret
        | TVar (index) -> sprintf "t%i" index

type Env = Map<int, Type>

let apply (exprType: Type) (funcType: Type) =
    match funcType with
    | TFun (arg, ret) ->
        // TODO: implement type checker for the argument
        Ok(ret)
    | _ -> Error("cannot apply")
