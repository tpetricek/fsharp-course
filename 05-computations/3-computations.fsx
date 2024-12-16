// ------------------------------------------------------------------
// WALKTHROUGH - F# computation expressions
// ------------------------------------------------------------------

// ------------------------------------------------------------------
// Example #1 - monad for logging
// ------------------------------------------------------------------

type WithLog<'T> = 
  { Result : 'T 
    Log : list<string> }

module Log =
  let unit x = { Result = x; Log = [] }
  let bind (f:'T -> WithLog<'R>) (x:WithLog<'T>) =
    let r = f x.Result
    { Result = r.Result; Log = x.Log @ r.Log }

  let log msg = 
    { Result = (); Log = [msg] }

// Adding an F# computation builder
// (supporting let!, do!, return, return!)
type LogBuilder() = 
  member x.Bind(v, f) = Log.bind f v
  member x.Return(v) = Log.unit v
  member x.ReturnFrom(r) = r

let log = LogBuilder()

let returnAndLog n = 
  Log.log ($"returning {n}") |> Log.bind (fun _ ->
    Log.unit n)

let addAndLog n1 n2 = 
  Log.log ($"{n1}+{n2}={n1+n2}") |> Log.bind (fun _ ->
    Log.unit (n1 + n2) )

log { 
  let! n1 = returnAndLog 2 
  let! n2 = returnAndLog 40
  return! addAndLog n1 n2 }

// ------------------------------------------------------------------
// Example #2 - Result/Maybe monad
// ------------------------------------------------------------------

type Result<'T> = 
  | OK of 'T
  | Error of string

module Result = 
  let unit v = OK v
  let bind (f:'T -> Result<'R>) (x:Result<'T>) : Result<'R> = 
    failwith "Implement me!"

  let error e = Error e

type ResultBuilder() = 
  // TODO: Implement me (and delete the next line)
  class end

let result = ResultBuilder()  


let tryParse (s:string) = 
  match System.Int32.TryParse s with
  | true, n -> Result.unit n
  | _ -> Result.error $"Not a number: {s}"

let safeDivide n1 n2 =
  if n2 = 0 then Result.error "Division by zero" 
  else Result.unit (n1 / n2) 

let parseAndDivide (s1:string) (s2:string) = 
  result {
    // TODO: Once you implement the above, this should work!
    let! n1 = tryParse s1
    let! n2 = tryParse s2 
    return! safeDivide n1 n2
  }


parseAndDivide "12" "4" = OK 3
parseAndDivide "12" "0" = Error "Division by zero"
parseAndDivide "12" "four" = Error "Not a number: four"


// ------------------------------------------------------------------
// Example #3 - monad for logging
// ------------------------------------------------------------------

type NonDet<'T> = 
  { Results : list<'T> }

module NonDet = 
  let unit (n:'T) : NonDet<'T> = 
    failwith "Implement me!"

  let bind (f:'T -> NonDet<'R>) (x:NonDet<'T>) : NonDet<'R> = 
    failwith "Implement me!"

  let zero () = 
    { Results = [] }

// DEMO: implementing computation builder for NonDet
type NonDetBuilder () =
  member x.ReturnFrom(r) = r
  member x.Return(v) = NonDet.unit v
  member x.Bind(v, f) = NonDet.bind f v
  // TODO: Uncomment this when you get an error about 'Zero'!
  // member x.Zero() = NonDet.zero ()

let range n1 n2 = 
  { Results = [ n1 .. n2 ] } 

let isPythagorean a b c = 
  a*a + b*b = c*c

let allPythagorean () =
  // TODO: Rewrite the following using the computation builder
  // (you can use 'if' inside a computation but if you do not
  // want to include 'else', you will need 'Zero' in your builder!)
  range 1 100 |> NonDet.bind (fun a ->
    range (a+1) 100 |> NonDet.bind (fun b ->
      range (b+1) 100 |> NonDet.bind (fun c ->
        if isPythagorean a b c then 
          NonDet.unit (a, b, c)
        else
          NonDet.zero ())))
