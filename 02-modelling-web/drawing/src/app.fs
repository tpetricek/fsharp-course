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
  // TODO #1: Implement the state update function as follows:
  //
  // * StartRectangle - set Current to Some rectangle that has the start and 
  //   end point set to the point where the user clicked & has random color
  //
  // * UpdateRectangle - update the End property of the Current rectangle
  //   (if there is None, ignore the event, though this should not happen)
  //
  // * FinishRectangle - add the Current rectangle to Shapes list and
  //   set Current back to None
  state

// ----------------------------------------------------------------------------
// TODO #2: Once you've done the above, you can experiment on your own!
// * Click on a rectangle to change its color?
// * Add different shapes and a button to choose them?
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

let r1 = { Start = (10,10); End = (200, 100); Color = randomColor() }
let init = { Current = None; Shapes = [ r1 ] }
createVirtualDomApp "out" init render update 
