namespace Fold

open Geometry

type Vertices =
    { coords: Point2D list
      vertices: int list
      faces: int list list }

module Vertices =

    let create a: Vertices = a

    let empty: Vertices =
        { coords = []
          vertices = []
          faces = [] }
