module Counter
open FSharpDemos.Html
open Browser.Types

// ----------------------------------------------------------------------------

type Rectangle =  
  { Start : int * int
    End : int * int
    Color : string }

type State = 
  { Shapes : list<Rectangle> 
    Current : option<Rectangle> }

type Event = 
  | StartRectange of (int * int)
  | UpdateRectange of (int * int)
  | FinishRectangle

// ----------------------------------------------------------------------------

let rnd = System.Random()
let randomColor () = $"rgb({rnd.Next(256)},{rnd.Next(256)},{rnd.Next(256)})"

let update state evt = 
  match evt with 
  | StartRectange start -> 
      let newRect = { Start = start; End = start; Color = randomColor() }
      { state with Current = Some newRect }

  | UpdateRectange finish -> 
      let updRect = state.Current |> Option.map (fun r -> { r with End = finish })
      { state with Current = updRect }

  | FinishRectangle ->
      { Shapes = state.Shapes @ Option.toList state.Current
        Current = None }

// ----------------------------------------------------------------------------

let render trigger state = 
  h?div [] [ 
    s?svg [
      "mouseup" =!> fun _ e -> 
        let e = unbox<MouseEvent> e
        trigger(FinishRectangle)
      "mousedown" =!> fun _ e -> 
        let e = unbox<MouseEvent> e
        trigger(StartRectange(int e.clientX, int e.clientY))
      "mousemove" =!> fun _ e -> 
        let e = unbox<MouseEvent> e
        if int e.buttons = 1 then
          trigger(UpdateRectange(int e.clientX, int e.clientY))
    ] [
      let all = Seq.append state.Shapes (Option.toList state.Current)
      for shape in all ->
        let x1, x2 = min (fst shape.Start) (fst shape.End), max (fst shape.Start) (fst shape.End)
        let y1, y2 = min (snd shape.Start) (snd shape.End), max (snd shape.Start) (snd shape.End)
        s?rect [ 
          "fill" => shape.Color
          "x" => string x1; "y" => string y1; 
          "width" => string (x2 - x1); "height" => string (y2 - y1)
        ] []      
    ]
  ]

// ----------------------------------------------------------------------------

let init = { Current = None; Shapes = [] }
createVirtualDomApp "out" init render update 
