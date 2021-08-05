namespace CreasePattern

open Geometry

type GraphElement =
    | VertexElement of Point2D
    | EdgeElement of Edge

[<RequireQualifiedAccess>]
type Axiom =
    | One
    | Two
    | Three

[<RequireQualifiedAccess>]
type AxiomAction =
    | One of Point2D * Point2D
    | Two of Point2D * Point2D
    | Three of Line2D * Line2D


module Axiom =

    open Utilities.Extensions

    (* Builders *)

    /// Get all the axioms that can be performed between the two graph elements
    let betweenElements e1 e2 : AxiomAction list =
        match e1, e2 with
        | VertexElement p1, VertexElement p2 -> [ AxiomAction.One(p1, p2); AxiomAction.Two(p1, p2) ]
        | EdgeElement e1, EdgeElement e2 -> [ AxiomAction.Three(e1.line, e2.line) ]
        | _ -> []

    /// Get the list of axioms that can be performed on a list of graph elements
    let fromElements elements : AxiomAction list =
        List.pairs elements
        |> List.map (Tuple2.uncurry betweenElements)
        |> List.concat


    (* Actions *)

    let private first v1 v2 : Line2D = Line2D.through v1 v2

    let private second v1 v2 : Line2D =
        Line2D.through v1 v2
        |> Line2D.perpThroughPoint (Point2D.midpoint v1 v2)

    (* Try to fold two lines through the angle bisectors. This has three possible cases.
       1. The lines are the same -> No new folds
       2. The lines are parallel -> One fold between the two folds
       3. The lines are intersecting -> Two possible folds, the acute angle bisector and the obtuse angle bisector
    *)
    let private third e1 e2 : Line2D list =
        (* 1. Lines are equal *)
        if e1 = e2 then
            []

        else
            let midpoint p =
                (Point2D.midpoint p (Line2D.pointClosestTo p e2))

            match Line2D.intersect e1 e2 with
            (* 2. Lines are parallel*)
            | None -> [ Line2D.through (midpoint e1.start) (midpoint e1.finish) ]

            (* 3. Lines are intersecting *)
            | Some intersection ->
                let linePoint =
                    if e1.start = intersection then
                        e1.finish
                    else
                        e1.start

                // We need to make sure that we get the acute and the obtuse angle bisectors
                let acuteLine = Line2D.through linePoint intersection

                [ Line2D.perpThroughPoint intersection acuteLine
                  acuteLine ]

    /// Get the result of performing an axiom action
    let perform action : Line2D list =
        match action with
        | AxiomAction.One (p1, p2) -> [ first p1 p2 ]
        | AxiomAction.Two (p1, p2) -> [ second p1 p2 ]
        | AxiomAction.Three (e1, e2) -> third e1 e2
