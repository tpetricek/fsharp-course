module Chat.Server
open Giraffe
open System.Threading.Tasks

// ------------------------------------------------------------------
// DEMOS: Some basic Giraffe HTTP handlers
// ------------------------------------------------------------------

let randomNumber next ctx = 
  let rnd = System.Random().Next()
  text (string rnd) next ctx

let slowHello next ctx = task { 
  do! Task.Delay(4000)
  return! text "Slow hello world!" next ctx
}

let demos = 
  [ "<a href='/hello'>Normal hello world</a>"
    "<a href='/hello/fsharp'>Hello F#</a>"
    "<a href='/random'>Random number</a>"
    "<a href='/add/2/40'>2+40</a>"
    "<a href='/slow'>Slow hello world</a>" ]
  |> String.concat "<br>"

let demoServer =
  choose [
    GET >=> route "/" >=> htmlString demos 
    GET >=> route "/hello" >=> text "Hello world!"
    GET >=> route "/random" >=> randomNumber
    GET >=> routef "/hello/%s" (fun n -> text $"Hello {n}!")
    GET >=> routef "/add/%i/%i" (fun (n1, n2) -> text (string (n1 + n2))) 
    GET >=> route "/slow" >=> slowHello
  ]

// ------------------------------------------------------------------
// TASKS: Write a couple of your own HTTP handlers!
// ------------------------------------------------------------------

let tasks = 
  [ "<a href='/repeat/10/Hello+world'>Say hello world 10 times</a>"
    "<a href='/repeat/100/Hello+world'>Say hello world 100 times</a>" 
    "<a href='/sleep/1000'>Sleep 1 second, then say hello</a>"
    "<a href='/sleep/5000'>Sleep 5 seconds, then say hello</a>" ]
  |> String.concat "<br>"

let taskServer =
  choose [
    // TODO: Add your handlers here!
  ]

// ------------------------------------------------------------------
// DEMO: Composing HTTP handlers
// ------------------------------------------------------------------

let server () = choose [
  demoServer
  taskServer
  setStatusCode 404 >=> text "Not Found"
]
