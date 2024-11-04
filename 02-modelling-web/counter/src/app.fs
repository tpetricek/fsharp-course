module Counter
open FSharpDemos.Html

// ----------------------------------------------------------------------------
// Introducing Elm-style architecture with Fable
// ----------------------------------------------------------------------------

// TODO #1: Look at the sample below. Check what kind of events are there
// (this is the `Update` type), what do we store in the `Model` and look
// at the type signatures of `update` and `render` to understand things!

// TODO #2: Add "Reset" button - to do this:
//  1) Add a new case `Reset` to the `Event` discriminated union
//  2) Implement the handling of `Reset` in the `update` function
//  3) Create the "Reset" button in the `render` function

// TODO #3: Replace the 'Increment' and 'Decrement' events with a single
// 'Change of int' event where the argument can be either +1 or -1 (to
// avoid some unnecessary code duplication in 'update').

// ----------------------------------------------------------------------------
// Domain model - update events and application state
// ----------------------------------------------------------------------------

type State = 
  { Count : int }

type Event = 
  | Increment
  | Decrement

// ----------------------------------------------------------------------------
// Given an old state and update event, produce a new state
// ----------------------------------------------------------------------------

let update state evt = 
  match evt with 
  | Increment -> { state with Count = state.Count + 1 }
  | Decrement -> { state with Count = state.Count - 1 }

// ----------------------------------------------------------------------------
// Render page based on the current state
// ----------------------------------------------------------------------------

let render trigger state = 
  h?div [] [
    h?h1 [] [ text $"Count: {state.Count}" ]
    h?button [ "click" =!> fun _ _ -> trigger(Increment) ] [ text "+1" ]
    h?button [ "click" =!> fun _ _ -> trigger(Decrement) ] [ text "-1" ] 
  ]

// ----------------------------------------------------------------------------
// Start the application with initial state
// ----------------------------------------------------------------------------

let init = { Count = 0 }
createVirtualDomApp "out" init render update 
