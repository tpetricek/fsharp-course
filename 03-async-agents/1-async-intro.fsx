open System
open System.IO
open System.Net.Http
open System.Text.RegularExpressions
open System.Threading.Tasks

// ------------------------------------------------------------------
// HELPERS: For demonstration purposes
// ------------------------------------------------------------------

// The standard ContinueWith method for .NET tasks has many
// overloads, which makes using it from F# difficult. This defines
// an extension method with just one overload, which will be much
// easier to use!

type System.Threading.Tasks.Task<'T> with 
  member x.Then(f:Action<Task<'T>>) = 
    x.ContinueWith(f) |> ignore

// ------------------------------------------------------------------
// DEMO: Why asynchronous programming is hard?
// ------------------------------------------------------------------

let regexTitle = Regex("\<title\>([^\<]*)\<")

// The following extracts the title of a web page synchronously
// (This blocks the current thread, which may be a problem!)

let client = new HttpClient()
let mff = client.GetAsync("https://www.mff.cuni.cz")
let mffBody = mff.Result.Content.ReadAsStringAsync()
let res = regexTitle.Match(mffBody.Result).Groups.[1].Value
printfn "TITLE: %s" res

// Non-blocking code using "continuations" can get very ugly
// very soon - but it does not block threads!

client.GetAsync("https://www.mff.cuni.cz").Then(fun mff ->
  let res = mff.Result.Content.ReadAsStringAsync()
  res.Then(fun mffBody ->
    let res = regexTitle.Match(mffBody.Result).Groups.[1].Value
    printfn "TITLE: %s" res
  )
)

// ------------------------------------------------------------------
// TASK: Writing callbacks by hand
// ------------------------------------------------------------------

// TASK #1: Turn the above into a reusable function and then
// call it with multiple different URLs as an argument. In what
// order do we get the result?

let printTitle url = 
  ()

printTitle "https://www.mff.cuni.cz"
printTitle "https://www.ff.cuni.cz"
printTitle "https://www.prf.cuni.cz"
printTitle "https://natur.cuni.cz"


// ------------------------------------------------------------------
// DEMO: Using asynchronous workflows (synchronous version)
// ------------------------------------------------------------------

/// Read a stream into the memory and then return it as a string
let readToEndSync (stream:Stream) = 
  // Allocate 1kb buffer for downloading dat
  let buffer = Array.zeroCreate 1024
  use output = new MemoryStream()
  let mutable finished = false
  
  while not finished do
    // Download one (at most) 1kb chunk and copy it
    let count = stream.Read(buffer, 0, 1024)
    output.Write(buffer, 0, count)
    finished <- count <= 0

  // Read all data into a string
  output.Seek(0L, SeekOrigin.Begin) |> ignore
  use sr = new StreamReader(output)
  sr.ReadToEnd()


/// Downlaod content of a web site using 'readToEnd'
let downloadSync (url:string) = 
  use client = new HttpClient()
  let mff = client.GetAsync(url)
  let stream = mff.Result.Content.ReadAsStream()
  let res = readToEndSync stream
  res


do
  let html = downloadSync "http://prgprg.org"
  printfn "%d" html.Length


// ------------------------------------------------------------------
// WALKTHROUGH: Using asynchronous workflows (asynchronous version)
// ------------------------------------------------------------------

let readToEndAsync (stream:Stream) = async {
  let buffer = Array.zeroCreate 1024
  use output = new MemoryStream()
  let mutable finished = false
  
  while not finished do
    // TASK #1: Use 'ReadAsync' and 'Async.AwaitTask' with 'let! here
    let count = stream.Read(buffer, 0, 1024)     
    // TASK #2: Use 'WriteAsync' and 'Async.AwaitTask' with 'do!' here
    output.Write(buffer, 0, count) 
    finished <- count <= 0

  output.Seek(0L, SeekOrigin.Begin) |> ignore
  use sr = new StreamReader(output)
  return sr.ReadToEnd() } 


let downloadAsync (url:string) = async {
  use client = new HttpClient()
  let! mff = client.GetAsync(url) |> Async.AwaitTask
  // TASK #3: Use 'ReadAsStreamAsync' and 'Async.AwaitTask' with 'let!' here also!
  let stream = mff.Content.ReadAsStream() 
  let! res = readToEndAsync stream
  return res }

async {
  // DEMO: When the asynchronous operation returns a value, we call it with 'let!'
  let! html = downloadAsync "http://prgprg.org"
  // DEMO: When the asynchronous operation returns 'unit' we call it with 'do!'
  do! Async.Sleep(1000)
  printfn "%d" html.Length }
|> Async.Start
