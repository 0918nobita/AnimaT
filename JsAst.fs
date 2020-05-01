module JsAst

type IExpression = interface end

type IStatement = interface end

type ILVal = interface end

type IVariableDeclarator =
    abstract member Id : ILVal
    abstract member Init : IExpression

type VariableKind = Const | Let

type VariableDeclaration =
    { Declarations : IVariableDeclarator list
      Kind: VariableKind }
    interface IStatement

type Program =
    { Body: IStatement list }
