// ------------------------------------------------------------------
// MOTIVATION #1 - working with option values
// ------------------------------------------------------------------

let tryParse (s:string) = 
  match System.Int32.TryParse s with
  | true, n -> Some n
  | _ -> None

let safeDivide n1 n2 =
  if n2 = 0 then None 
  else Some(n1 / n2) 

let parseAndDivide (s1:string) (s2:string) = 
  failwith "Implement me!"   


parseAndDivide "12" "4" = Some 3
parseAndDivide "12" "0" = None
parseAndDivide "12" "four" = None


// ------------------------------------------------------------------
// MOTIVATION #2 - working with lists
// ------------------------------------------------------------------

let range n1 n2 = [ n1 .. n2 ] 

let isPythagorean a b c = 
  a*a + b*b = c*c

let allPythagorean () =
  // TODO: Generate all Pythagorean triples - to do this
  // - iterate over 'a' from 1 to 100,
  // - iterate over 'b' from a+1 to 100, 
  // - iterate over 'c' from b+1 to 100,
  // - return the triple if a,b,c is Pythagorean
  // Ideally, you'd return a list of all such triples
  // but it may be easier to just print the results!
  failwith "Implement me!"
