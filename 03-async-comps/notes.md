# Asynchronous programming and web crawling

## Important things to explain

* How to use asynchronous computations in a useful way? 
  Either `Async.Start` (but then you have to return `unit`)
  or by using `Async.Parallel` and `Async.RunSynchronously`.

* Why is there `Async<T>` and how does it differ from `Task<T>`?

* Note that `Task<T>` and `Task` in C# are needed because C#
  does not have `unit` but only `void`! F# avoids this.

## Examples to show

Translation using `async`:


```fsharp
let getTitleDemo (url:string) =
  let client = new HttpClient()
  async.Bind(client.GetAsync(url) |> Async.AwaitTask, fun mff ->
    async.Bind(mff.Content.ReadAsStringAsync() |> Async.AwaitTask, fun mffBody ->
     let res = regexTitle.Match(mffBody).Groups.[1].Value
     async.Return(res) ))

let urls =
  [ "https://www.mff.cuni.cz"
    "https://www.ff.cuni.cz"
    "https://www.prf.cuni.cz"
    "https://natur.cuni.cz" ]

let printAll =
  async.For(urls, fun url ->
    async.Bind(getTitleDemo url, fun res ->
      printfn "%s" res
      async.Zero()  ))

Async.RunSynchronously(printAll)
```