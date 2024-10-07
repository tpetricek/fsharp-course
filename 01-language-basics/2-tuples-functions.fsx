// ============================================================================
// INTRO 2: Tuples and functions
// ============================================================================

#load "setup.fsx"
open Setup
open System

// ----------------------------------------------------------------------------
// WALKTHROUGH: Tuples and functions taking tuples
// ----------------------------------------------------------------------------

// Tuple is a simple type that groups two or more values of possibly
// different types. The following sample uses tuples to represent people -
// note that the parentheses are optional.
//
// When decomposing tuple, you can write *pattern* that consists of new
// variables, to be used for individual components of the tuple.
let person1 = ("Ludwig", 56)
let (name1, age1) = person1

let person2 = "Ludwig", "Wittgenstein", 56
let name2, surname2, age2 = person2

shouldEqual name1 __
shouldEqual surname2 __

// F# also provides two simple functions for working with two-element tuples
let person = "Ludwig", 56
shouldEqual (fst person) __
shouldEqual (snd person) __


// Tuples can be useful when you want to return mutliple values as the result
// of a function - for example name and age. The following snippet also
// shows how to use the 'if .. then .. else' construct in F#
let getPerson job =
  if job = "philosopher" then ("Ludwig", 56)
  elif job = "scientist" then ("Albert", 66)
  else ("Someone", 10)

shouldEqual (fst (getPerson "scientist")) __
shouldEqual (snd (getPerson "scientist")) __

// F# uses "structural equality" which means that tuples containing the
// same values are treated as equal. For example, try the following:
shouldEqual ("Joe", 13) ("Joe", 13)
shouldEqual (getPerson "philosopher") __


// When writing functions that take tuples as arguments, the tuple is just
// a single parameter, so you can take e.g. 'person' and then decompose it
// into two values. However, you can write the same thing more compactly
// by using the pattern directly in the argument of the function. The following
// two functions are the same:
let addYear1 person =
  let name, age = person
  name, age + 1

let addYear2 (name, age) =
  name, age + 1

shouldEqual (addYear1 (getPerson __)) ("Albert", __)
shouldEqual (addYear2 (getPerson __)) ("Ludwig", __)

// In some cases, you may need to provide type annotation to specify the
// type explicitly. For example, when you want to call a .NET member,
// the F# compiler needs to know the type (so that it can check whether the
// member exists). You can annotate single variables or composed patterns.
let getLength (name:string) =
  name.Length

let getNameLength ((name, age):string * int) =
  name.Length

shouldEqual (getLength "Ludwig") __
shouldEqual (getNameLength (getPerson "philosopher")) __

// ============================================================================
// TASK #2: Validating inputs
// ============================================================================

// In this task, we want to write a simple validator which tests whether a
// name and age represents a valid person. A valid person details:
//
//  - Have age between 0 and 150 (inclusive)
//  - Start with an upper-case letter
//  - Contains a space & letter after space is upper case
//
// You'll need "str.[index]" to access character at a given index,
// "Char.IsUpper" to check whether character is upper case and string
// operations including "str.IndexOf" and "str.Contains".

let validAge person = __
let validName person = __

let validPerson person =
  validAge person && validName person

shouldEqual (validPerson ("Tomas Petricek", 42)) true
shouldEqual (validPerson ("Tomas Petricek", 242)) false
shouldEqual (validPerson ("Tomas", 42)) false
shouldEqual (validPerson ("Tomas petricek", 42)) false
shouldEqual (validPerson ("tomas Petricek", 42)) false

// ----------------------------------------------------------------------------
// WALKTHROUGH: Using functions as arguments
// ----------------------------------------------------------------------------

// In F#, it is really easy to create a function that takes other
// function as an argument. Functions are just simple values that can
// be passed around in the usual way. The following function takes
// 'f' and applies it to the age of a person
let transformAge f (name, age) =
  (name, f age)

// Functions that increment/decrement age
let increment a = a + 1
let decrement a = a - 1

// Implement a function that caps the age to 0 when it is
// smaller than 0; and to 150 if it is greater than that.
let cap a =
  __

shouldEqual (transformAge increment ("Tomas", 42)) __
shouldEqual (transformAge decrement ("Tomas", 42)) __
shouldEqual (transformAge cap ("Tomas", -1)) ("Tomas", 0)
shouldEqual (transformAge cap ("Tomas", 420)) ("Tomas", 150)

// When calling functions with multiple parameters, it is convenient
// to use the '|>' operator. The operator takes the value on the left
// and passes it as a parameter to the function on the right:

("Tomas", 40)
|> transformAge increment
|> transformAge increment
|> shouldEqual __

// The previous example specified functions (to be used as parameters)
// explicitly using named functions. You can write the same functions
// inline using the lambda function syntax and the 'fun' keyword.

let r1 =
  ("Tomas", 42)
  |> transformAge (fun a -> a + 2)

shouldEqual r1 __

// Lambda functions can contain multi-line code. It just needs to start
// on a new line and be indented further than the previous line
let r2 = ("Tomas", 42) |> transformAge (fun a ->
  printfn "Processing person with age: %d" a
  a + 2)

shouldEqual r2 __

// Rewrite the 'cap' function from a previous example as an inline lambda
let r4 = ("Tomas", -1) |> transformAge (fun _ -> __)

shouldEqual r4 ("Tomas", 0)

// ============================================================================
// TASK #3: Using functions as arguments
// ============================================================================

// The 'transformAge' function applies the specified function to the
// age (second elemnet of the tuple). Implement a similar function that
// transforms the name of a tuple and use it to turn name into upper case.
let transformName f (name, age) =
  __

let r5 =
  ("Tomas", 42)
  |> transformName (fun (n:string) -> n.ToUpper())

shouldEqual r5 ("TOMAS", 42)
