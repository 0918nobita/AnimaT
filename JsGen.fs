module JsGen

open JsAst

let generate (program : JsAst.Program) =
    program.Body
    |> List.iter
        (fun stmt ->
            match stmt with
            | :? VariableDeclaration as decl ->
                printf
                    "%s"
                    (match decl.Kind with
                     | Const -> "const"
                     | Let -> "let")

                decl.Declarations
                |> List.iter
                    (fun { Id = id; Init = init } ->
                        printf " %O = %O" id init)

            | _ -> failwith "not implemented"

            printfn ";")
