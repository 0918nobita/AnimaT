module Parser

open FParsec.Primitives
open FParsec.CharParsers

type SourcePos = SoucePos of line:int * Column:int

type Expr =
    | IntLit of int
    | DoubleLit of double
    | LetExpr of ident:int * expr1:Expr * expr2:Expr
    | Progn of Expr list

let identParser: Parser<string, unit> =
    IdentifierOptions
        (isAsciiIdStart = (fun c -> isAsciiLetter c || c = '_'),
         isAsciiIdContinue = fun c -> isAsciiLetter c || isDigit c || c = '_' || c = '\'') |> identifier

let letExpr: Parser<string, unit> =
    pstring "let"
    >>. spaces1
    >>. identParser
