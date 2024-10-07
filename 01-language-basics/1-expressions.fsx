// ============================================================================
// INTRO 1: Primitive types, expressions and calculating
// ============================================================================

#load "setup.fsx"
open Setup

// ----------------------------------------------------------------------------
// WALKTHROUGH: Primitive types and type inference
// ----------------------------------------------------------------------------

// In this script, you're going to walk through the basic F# concepts.
// The snippets in the file are written as simple tests - you need to
// fill in the missing pieces and then you can run the snippet to check
// whether it works correctly. Start by selecting the following two lines
// and sending them to F# Interactive (using Alt+Enter in Visual Studio
// or using right click - Send to F# Interactive).
let answer = 42
shouldEqual answer 42


// Now try running a snippet with a placeholder "__" and then fill the
// placeholder with the correct answer and run the snippet again.
let demo = 21 * 2
shouldEqual demo __


// F# uses type inference and so all variables have a static type
// (see this by putting mouse pointer over the identifier). There
// are two basic numeric types:
let integers = 32 + 10
let floats = 31.5 + 10.5

shouldEqual integers __
shouldEqual floats __


// You can use standard logical operations when working with numbers:
let high = 100.0
let low = 50.0
let check1 = 75.0 > low && 75.0 < high
let check2 = __ > low && __ < high

shouldEqual check1 true
shouldEqual check2 false

// ----------------------------------------------------------------------------
// WALKTHROUGH: Calling .NET objects and mutation
// ----------------------------------------------------------------------------

// F# gives you full access to the .NET ecosystem. This means that you
// can access all standard .NET libraries such as System.Random (here,
// fill the rest of the condition so that the snippet always works!)
open System

let rnd = new Random()
let num = rnd.Next(4)
let rndCheck = num = 0 || num = 1 || __

shouldEqual rndCheck true


// By default, F# values are immutable, but you can mark them as mutable.
// Try removing the 'mutable' keyword and see what happens!
let mutable message1 = "Hello "
message1 <- message1 + "world!"
message1 <- message1 + " How are you?"

shouldEqual message1 __


// ----------------------------------------------------------------------------
// WALKTHROUGH: Writing and calling functions
// ----------------------------------------------------------------------------

// The 'let' keyword is not only used for defining variables, but also for
// defining functions. In 'let f x = ...' the identifier 'f' is the name
// of the function and 'x' is the argument of the function.
let twoTimes num =
  num * 2

shouldEqual (twoTimes 21) __

// Note that the type of the function is inferred. In the previous example,
// the function body used '2' and so it is a function 'int -> int' (you can
// see that in the tool tips). F# is indentation-sensitive, so you do not need
// to write brackets, but the body of the function needs to be indented further.
let twoTimesFloat num =
  __
let twoTimesString str =
  __ + " " + __

shouldEqual (twoTimesFloat 21.0) 42.0
shouldEqual (twoTimesString "Hi") "Hi Hi"

// When writing functions of multiple arguments, the arguments are separated
// by spaces. This is both in the declaration and when calling the function.
// However, you need parentheses when the argument is a more complex expression.
let add a b =
  a + b
let mul a b =
  a * b

let r1 = mul (add 3 4) (mul 2 __)
let r2 = add (mul 3 (add 4 __)) 21
let r3 = add (rnd.Next(2)) 41
shouldEqual r1 42
shouldEqual r2 42
shouldEqual (r3 = __ || r3 = __) true

// For the task at the end, you also need to know how to
// calculate exponentials using F# -- use the ** operator!
let exp = 2.0 ** __
shouldEqual exp 32.0


// ============================================================================
// TASK #1: Calculating compound interest
// ============================================================================

// As an example, calculate compound interest formula. That is,
// when borrowing money, the interest is added to the total sum
// and the next interest is calculated from the borrowed money,
// *including* the added interest.

let interest = 0.02     // Interest rate is 2%
let periods = 12.0      // Number of times the interest is compounded per year
let years = 10.0        // Number of years the money is borrowed for

// Calculate compound interest using the formula from:
// http://en.wikipedia.org/wiki/Compound_interest#Compound_Interest
let compound investment =
  __

let res = compound 1000.0
shouldEqual (round res) 1221.0

printfn "Compound interest (full): %f" res
printfn "Compound interest (two digits): %.2f" res
