// ============================================================================
// INTRO 3: Records, unions and pattern matching
// ============================================================================

#load "setup.fsx"
open Setup
open System

// ----------------------------------------------------------------------------
// WALKTHROUGH: Working with records
// ----------------------------------------------------------------------------

// Record is a type that keeps two or more values of different types
// in labelled fields. This is similar to tuples, but adds labels for
// better readability. It is also similar to simple OOP classes.
type Person =
  { Name : string
    Age : int }

// To create a record, just set the values of the record in curly brackets
let yoda =
  { Name = "Mr. Yoda"
    Age = 700 }

shouldEqual yoda.Name __
shouldEqual yoda.Age __

// The fields of the record are immutable. When you want to change something
// you need to create a *new* instance with modified state. To do this, you
// can use the 'with' keyword which changes only the specified fields:
let olderYoda =
  { yoda with Age = yoda.Age + 200 }

shouldEqual yoda.Name __
shouldEqual yoda.Age __
shouldEqual olderYoda.Name __
shouldEqual olderYoda.Age __

// When writing a function, we do not need type annotations, because the
// compiler can infer the type of the argument from the labels used:
let isAlive person =
  if person.Name = "Mr. Yoda" then person.Age < 800
  else person.Age < 100

shouldEqual (isAlive yoda) __
shouldEqual (isAlive olderYoda) __

// Now, write a function that formats information about person to match
// the following tests. To format strings, you can use the 'sprintf'
// function which takes a format string and an argument:
shouldEqual (sprintf "The answer to %s is %d" "The Question" 42) __
shouldEqual (sprintf "The answer to %s is %.0f" "The Question" 42.0) __

let formatPerson person =
  __

shouldEqual (formatPerson yoda) "Mr. Yoda (700 years)"
shouldEqual (formatPerson olderYoda) "Mr. Yoda (Died)"

// Now, use the 'with' keyword to write a function that
// returns a person older by 1 year
let oneYearOlder person =
  __

shouldEqual (formatPerson (oneYearOlder yoda)) "Mr. Yoda (701 years)"
shouldEqual (formatPerson (oneYearOlder olderYoda)) "Mr. Yoda (Died)"

// ----------------------------------------------------------------------------
// WALKTHROUGH: Working with discriminated unions
// ----------------------------------------------------------------------------

// Discriminated unions are types that represent the choice between multiple
// different cases, each consisting of zero or more properties. For example,
// a calendar item can be scheduled to occur once, repeatedly or never.

type Schedule =
  | Once of DateTime
  | Repeatedly of DateTime * TimeSpan
  | Never

// The names 'Once', 'Repeatedly' and 'Never' are type constructors that
// create a value of type 'Schedule'. The schedule represents one of the cases.

let s0 = Never
let s1 = Once(DateTime.Now.Date.AddDays(5.0))
let s2 = Repeatedly(DateTime.Now.Date.AddDays(2.0), TimeSpan.FromDays(7.0))

// When working with discriminated unions, you can use pattern matching and
// the 'match' keyword to write code that handles the different cases. The
// following formatting function prints details about schedule:

let format i =
  match i with
  | Never -> "Unscheduled"
  | Once(dt) -> sprintf "In %.0f days" (dt - DateTime.Now.Date).TotalDays

shouldEqual (format s0) __
shouldEqual (format s1) __
shouldEqual (format s2) "In 2 days, then every 7 days"

// Now, write a function 'hasExpired' that returns 'true' when the
// event already occurred (at least once) or 'false' when the event
// is unscheduled or occurs in the future.

let hasExpired schedule =
  __

shouldEqual (hasExpired Never) false
shouldEqual (hasExpired (Once(DateTime.Now.AddDays(4.0)))) false
shouldEqual (hasExpired (Once(DateTime.Now.AddDays(-4.0)))) true
shouldEqual (hasExpired s2) false

// ----------------------------------------------------------------------------
// WALKTHROUGH: Working with option types
// ----------------------------------------------------------------------------

// One particularly useful pre-defined discriminated union is the 'option'
// type. This is used to represent values that may, or may not, contain
// a value. The union is generic (this is what the 'T means) and is defined as:
//
//  type option<'T> =
//    | Some of 'T
//    | None
//
// This means that we can create values using the
// two constructors 'None' and 'Some':

let o1 = Some(10)
let o2 = None

let valueOrZero o =
  match o with
  | Some(v) -> v
  | None -> 0

shouldEqual (valueOrZero (Some(10))) __
shouldEqual (valueOrZero None) __

// Options are often used when you are writing some code that may fail. For
// example, if we have a simple function 'tryFindPerson' that returns a person
// based on name, but cannot always return the person, we can write:

let tryFindPerson name =
  match name with
  | "Yoda" -> Some { Name = "Mr. Yoda"; Age = 700 }
  | "Luke" -> Some { Name = "Luke Skywalker"; Age = 25 }
  | _ -> None

shouldEqual (tryFindPerson "Luke") __
shouldEqual (tryFindPerson "Vader") __

// Now, modify the 'tryFindPerson' so that it also supports evli characters
shouldEqual (tryFindPerson "Vader") (Some { Name = "Darth Vader"; Age = 50 })

// Next, write a function 'formatOptionalPerson' that formats information
// about a person represented as 'option<Person>' - i.e. the result of the
// 'tryFindPerson' function. This can call 'formatPerson', but you also need
// to handle the case when the person is not known!
//
// Note that this function takes option value as an argument, so you need
// to use pattern matching with 'match' (like in 'valueOrZero') to handle
// the two possible cases. The compiler is forcing you to write code that
// handles both the "happy path" and possible error!
let formatOptionalPerson (info:option<Person>) =
  __

shouldEqual (formatOptionalPerson (tryFindPerson "Luke")) "Luke Skywalker (25 years)"
shouldEqual (formatOptionalPerson (tryFindPerson "Yoda")) "Mr. Yoda (700 years)"
shouldEqual (formatOptionalPerson (tryFindPerson "R2D2")) "Unknown"

// ============================================================================
// TASK #4: Three-value logic
// ============================================================================

// In a three-state logic, we are working with three values. The Truth and
// Falsehood valuas represent that the value is true/false as usual. The
// additional Unknown value means that we do not know the value (perhaps
// because we do not have enough information).
type ThreeState =
  | Truth
  | Falsehood
  | Unknown

// The logical operations for three-state logics are defined by the following
// tables (basically, if a value is unknown, it could be either true or false
// and we have to consider both cases). For more information see:
// http://en.wikipedia.org/wiki/Three-state_logic
//
//   +-----------+-----------+-----------+-----------+
//   |   AND     | Truth     | Falsehood | Unknown   |
//   +-----------+-----------+-----------+-----------+
//   | Truth     | Truth     | Falsehood | Unknown   |
//   | Falsehood | Falsehood | Falsehood | Falsehood |
//   | Unknown   | Unknown   | Falsehood | Unknown   |
//   +-----------+-----------+-----------+-----------+
//
//   +-----------+-----------+-----------+-----------+
//   |   OR      | Truth     | Falsehood | Unknown   |
//   +-----------+-----------+-----------+-----------+
//   | Truth     | Truth     | Truth     | Truth     |
//   | Falsehood | Truth     | Falsehood | Unknown   |
//   | Unknown   | Truth     | Unknown   | Unknown   |
//   +-----------+-----------+-----------+-----------+

// As a task, implement the 'OR' and 'AND' operations. The easiest way
// to do that is to pattern match on the tuple "a, b". For both of the
// functions, you should need at most 4 cases!

let threeStateOr a b =
  __
let threeStateAnd a b =
  __

// Test which verifies that 'threeStateOr' is implemented correctly
// (Do not worry if you don't understand all the code here!)
for a in [Truth; Falsehood; Unknown] do
  for b in [Truth; Falsehood; Unknown] do
    if a = Truth || b = Truth then
      shouldEqual (threeStateOr a b) Truth
    elif a = Falsehood && b = Falsehood then
      shouldEqual (threeStateOr a b) Falsehood
    else
      shouldEqual (threeStateOr a b) Unknown

// Test which verifies that 'threeStateAnd' is implemented correctly
// (Do not worry if you don't understand all the code here!)
for a in [Truth; Falsehood; Unknown] do
  for b in [Truth; Falsehood; Unknown] do
    if a = Truth && b = Truth then
      shouldEqual (threeStateAnd a b) Truth
    elif a = Falsehood || b = Falsehood then
      shouldEqual (threeStateAnd a b) Falsehood
    else
      shouldEqual (threeStateAnd a b) Unknown
