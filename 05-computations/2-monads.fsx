// ------------------------------------------------------------------
// WALKTHROUGH - introducing monads
// ------------------------------------------------------------------

// Three sample computation types:
//
// * Result<'T> can return a value of type 'T or fail with an error
// * NonDet<'T> can return multiple possible results of type 'T
// * WithLog<'T> returns a value 'T and logs some messages

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

let returnAndLog n = 
  Log.log ($"returning {n}") |> Log.bind (fun _ ->
    Log.unit n)

let addAndLog n1 n2 = 
  Log.log ($"{n1}+{n2}={n1+n2}") |> Log.bind (fun _ ->
    Log.unit (n1 + n2) )

returnAndLog 2 |> Log.bind (fun n1 ->
  returnAndLog 40 |> Log.bind (fun n2 ->
    addAndLog n1 n2
  )
)

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


let tryParse (s:string) = 
  match System.Int32.TryParse s with
  | true, n -> Result.unit n
  | _ -> Result.error $"Not a number: {s}"

let safeDivide n1 n2 =
  if n2 = 0 then Result.error "Division by zero" 
  else Result.unit (n1 / n2) 

let parseAndDivide (s1:string) (s2:string) = 
  failwith "Implement me!"   


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

let range n1 n2 = 
  { Results = [ n1 .. n2 ] } 

let isPythagorean a b c = 
  a*a + b*b = c*c

let allPythagorean () =
  range 1 100 |> NonDet.bind (fun a ->
    range (a+1) 100 |> NonDet.bind (fun b ->
      range (b+1) 100 |> NonDet.bind (fun c ->
        if isPythagorean a b c then 
          NonDet.unit (a, b, c)
        else
          NonDet.zero ())))
