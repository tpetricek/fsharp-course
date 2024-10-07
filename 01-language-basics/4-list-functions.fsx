// ============================================================================
// INTRO 4: List processing functions
// ============================================================================

#load "setup.fsx"
open System
open Setup

// ----------------------------------------------------------------------------
// WALKTHROUGH: List processing functions
// ----------------------------------------------------------------------------

// In this sample, we're going to be working with historical Olympic medals.
// To explore the data set, we can get the first and lst value and print
// some details about it using the 'Seq.head' and 'Seq.last' functions.
let af = medals |> List.head
let al = medals |> List.last

// In this part, we're going to be using mostly functions from the 'List' module.
// The module has some basic functions, e.g. to get the length of the list:

let len = medals |> List.length


// There is a number of useful functions that take other functions as argument. 
// For example, we can get years of when the medals were awarded:

let getYear (item:Medal) =
  item.Year

let minYear1 =
  medals
  |> List.map getYear
  |> List.min

// The same code can be written more easily using inline lambda function
let minYear2 =
  medals
  |> List.map (fun item -> item.Year)
  |> List.min


// You can do this even more easily using the 'List.minBy' function that takes
// a function as an argument and returns the itme with the smallest returned value.
let minItem = 
  medals 
  |> List.minBy __

shouldEqual (int minYear1) (int minItem.Year)


// Another useful function is 'filter' which returns only items that satisfy
// the specified condition. For example, we can list Czech medalists from 2016:
medals
|> List.filter (fun item -> item.Team = "Czech Republic" && item.Year = 2016)
|> List.map (fun item -> item.Athlete)


// Now, count the number of Gold medals won by athletes from 'Soviet Union'
// and 'United States' between the years 1950 and 1990!

let usGolds = __
let sovietGolds = __

shouldEqual usGolds 730
shouldEqual sovietGolds 838


// For more interesting operations with medals, we can use the 'groupBy' 
// function. This turns the input list into a list of groups based on the 
// given key selector. Alongside with 'sort', we can get some interesting 
// statistics! For example, find top countries by the total number of medals:

let topCountries = 
  medals
  |> List.groupBy (fun item -> item.Team)
  |> List.map (fun (team, items) -> 
      // NOTE: It is useful to write this as multi-line lambda. If you want
      // to do more complicated processing of the items that belong to 
      // the current group, you will need a nested |> here!
      let medals = List.length items
      team, medals)
  |> List.sortByDescending snd
  |> List.take 3

shouldEqual topCountries 
  [ ("United States", 4858); ("Soviet Union", 2049); 
    ("United Kingdom (Great Britain)", 1874) ]

// ============================================================================
// TASKS: Working with lists
// ============================================================================

// TASK #1: What are the sports (based on item.Sport) for which 
// less than 10 medals were awarded over the entire history?

let oddSports = 
  __<string list>

// Your code above should return a list of sport names. To avoid spoilers,
// the following checks you got a result with the right set of letters!
shouldEqual (set (String.concat "" oddSports)) 
  (set " BCJMPRWadelmopqrstu")


// TASK #2: It turns out that some names appear multiple times in 
// the results, even if this clearly cannot be the same person.
// But let's try this anyway! What name of an athlete appears
// with greatest distance between the first and the last medal
// awarded to them? To avoid getting boring "David Smith", group the 
// athletes by both their name and their team!

let namesWithYearDiffs = 
  __<(string * int) list>

// Of course, the person winning medals 76 years apart is not the same 
// person, but google them to find out about an interesting family :-)
shouldEqual 76 (snd (Seq.head namesWithYearDiffs))