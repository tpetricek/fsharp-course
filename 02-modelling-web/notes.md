# Interactive web applications with Elm architecture

## Important things to explain

* How to get things to run! (Current setup requires .NET 6, which is a bit outdated;
  this should be updated to something more recent... otherwise things fail)
* How the Elm architecture works (`State` and `Event`; `update` and `render`)
* How the `h?h1 [] [ text "Hello" ]` DSL works

## Important bits of syntax

The arguments to `h?h1 [] []` are just lists, so they can be generated using
`List.map` and such, but this is not always very nice. F# has sequence expressions
(comprehensions) which make this much nicer. Here are basic examples using numbers:

```fsharp
// Generate a sequence of squares
let nums1 =
  [ for i in 1 .. 10 -> i * i ]

// Generate squers but only of even numbers  
let nums2 =
  [ for i in 1 .. 10 do
      if i % 2 = 0 then
        yield i * i ]
```

The same works in HTML:

```fsharp
h?ul [] [
  for i in 1 .. 10 do
    if i % 2 = 0 then
      yield h?li [] [ text $"Item {i}" ]
]
```

## Domain modelling

If we have `Change of int`, how can we make sure that only +1 and -1 can be used as arguments?
This cannot be done with `int`, but we can have a more restrictive model:

```fsharp
type Change =
  | Increment
  | Decrement  

type Event =
  | Change of Change
```

This can still let us simplify the event handling code if we define a helper:

```fsharp
let changeToInt ch =
  match ch with
  | Increment -> +1
  | Decrement -> -1
```

There is always trade-off - if we want more precise domain model, we may have to
write more code in the processing code.

## Using proper Virtual Dom library

The tutorial uses my own minimal version, which (among other things) does not correctly
handle the case when you want to change the value of an `input`. Other than this, it
works well enough (and avoids large dependency), but you should use something more realistic
in real-world apps - the best choice is [Elmish.React](https://elmish.github.io/react/tutorials/browser.html)
