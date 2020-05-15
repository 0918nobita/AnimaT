module JsAst

type IExpression =
    interface
    end

type IStatement =
    interface
    end

type ILVal =
    interface
    end

type VariableDeclarator = { Id: ILVal; Init: IExpression }

type Identifier =
    { Name: string }
    interface IExpression
    interface ILVal
    override this.ToString() = this.Name

type BooleanLiteral =
    { Value: bool }
    interface IExpression
    override this.ToString() = if this.Value then "true" else "false"

type VariableKind =
    | Const
    | Let

type ExpressionStatement =
    { Expression: IExpression }
    interface IStatement

type VariableDeclaration =
    { Declarations: VariableDeclarator list
      Kind: VariableKind }
    interface IStatement

type Program = { Body: IStatement list }
