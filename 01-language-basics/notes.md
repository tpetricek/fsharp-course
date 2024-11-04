# Primitive types, expressions and calculating

## Important things to explain

* How things evaluate - call by value unlike in Haskell
* Everything is an expression - you can nest things in funny ways
* Things that appear like statements e.g. `printfn "Hello"` actually return unit
* There is only one value `()` of type `unit` - it is not useful to ever assign this to a variable, but you can do it
* Sequencing is done using the `;` operator which takes two expressions, evaluates both (left-to-right) and returns the result of the right one. It ignores the result of the left one.
* You get a compiler warning if you ignore something useful!

## Expressions and scoping

This is valid syntax, using the fact that everything is an expression:

```fsharp
let x = 1
100 * (let z = x + x)
```

The following prints hello just once, because `foo` is a value that is evaluated just once and then accessed twice:

```fsharp
let foo = 
  printfn "Hello world"
  21

foo + foo
```  

If we turn `foo` into a function that takes `unit` and returns `int`, then it prints Hello twice:

```fsharp
let foo () = 
  printfn "Hello world"
  21

foo () + foo ()
```  

The following shows why the way `;` works is useful. It looks like we are taking a string, dropping the first character and printing the rest. Actually, strings are immutable! F# gives us a helpful warning:

```fsharp
let sayHello (s:string) =
  s.Substring(1) // FS0020: The result of this expression is implicitly ignored
  printfn $"{s}"

sayHello "!Hello world"
```

## Functions and arguments

We can use tuple `int * int` to represent points:

```fsharp
let pt = (10, 20)
```

Now we can define various functions for working with points:

```fsharp
let area (x, y) = x * y
let swap (x, y) = (y, x)
let moveX dx (x, y) = 
  (x + dx, y)
```

Interestingly, `(x, y)` in the function declaration is not a parameter list
but a pattern - F# matches the value given as an argument against the pattern
and decomposes it into `x` and `y`. You can use patterns directly in the function
declaration, but also in `let`:

```
let area2 arg = 
  let (x, y) = arg
  x * y
```

Another thing to note is how types of functions look. The type of `moveX` is:

```fsharp
int -> (int * int) -> (int * int)
```

You can see this as a function taking two arguments, but it can also be read
as a function taking an `int` and returning another function:


```fsharp
int -> ((int * int) -> (int * int))
```

When you call functions like this:

```fsharp
area (swap (moveX 1 pt))
```

... your code can get ugly soon! But fortunately, we can use the `|>` operator:

```fsharp
pt
|> moveX 1
|> swap
|> area
```

The operator takes the value on the left and passes it to the function on the right.
You can write this yourself!

```fsharp
let (|>) x f = f x
```

And finally, here is an example of a function taking another function as an argument:

```fsharp
let transformX f (x, y) = 
  (f x, y)

pt
|> transformX (fun x -> x * 2)
```

You can hide the standard `+` operator if you want - probably not a good idea! But you
can also define local operators - sometimes useful trick:

```fsharp
let foo () = 
  let (++) a b = a + "," + b
  "hi" ++ "there" ++ "people"
```

## Options and the billion dollar mistake

Tony Hoare famously admits that he invented the `null` value:

> I call it my billion-dollar mistake. It was the invention of the 
> null reference in 1965. (...) I couldn't resist the temptation to 
> put in a null reference, simply because it was so easy to implement. 

In F#, you can use `null` as a value of .NET types that you are accessing
(no way around this), but you cannot use `null` as a value of F# types:

```fsharp
type Person =
  { Name : string
    Age : int }
```

This does not work:

```fsharp
let getPerson name = 
  if name = "Yoda" then{ Name = "Mr. Yoda"; Age = 700}
  else null

getPerson("Luke").Name 
```

We have to return `option<Person>` instead:

```fsharp
let getPerson name = 
  if name = "Yoda" then Some({ Name = "Mr. Yoda"; Age = 700})
  else None
```

And now you cannot access `Name` without checking whether the value is `None` first!

```fsharp
let p = getPerson "Luke"
p.Name
```

## Pipe operator and type inference

F# type checking proceeds from left to right. In case when
we are calling members of a .NET object, the compiler will
need to know what the type of the object is. For example:

```fsharp
let names = 
  [ "Luke Skywalker"; "Darth Vader"; 
    "Mr Yoda"; "Darth Sidious" ]

let isEvil name = 
  name.StartsWith("Darth") // Does not know what `name` is

let isEvil (name:string) = 
  name.StartsWith("Darth") // OK! We have a type annotation
```

Interestingly, the `|>` operator helps you avoid type
annotations. For example:

```fsharp
// This is OK - isEvil has a known type
List.filter isEvil names

// We do not know the type of `n` here
List.filter (fun n -> n.StartsWith("Darth")) names

// OK! Because we know `n` must be `string` by the time
// we get to the checking of `StartsWith`
names |> List.filter (fun n -> n.StartsWith("Darth"))
```