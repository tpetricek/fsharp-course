open System
open System.Net.Http
open System.Text.RegularExpressions

// Regular expressions for extracting HTML elements :-)
let regexLink = Regex("\<a href=\"(/wiki/[^\"]*)\"")
let regexTitle = Regex("\<title\>([^\<]*)\<")

// ------------------------------------------------------------------
// WALKTHROUGH: Random walk through Wikipedia
// ------------------------------------------------------------------

let download (url:string) =
  let client = new HttpClient()
  let html = client.GetStringAsync(url).Result
  
  // Extract the contents of <title> from the page
  let title = regexTitle.Match(html).Groups.[1].Value
  let title =
    if not (title.Contains("-")) then title
    else title.Substring(0, title.LastIndexOf('-')-1)

  // Extract all links to proper Wikipedia pages
  let allLinks = 
    [ for link in regexLink.Matches(html) do
        let ahref = link.Groups.[1].Value
        if not (ahref.Contains(":")) then
          yield "http://en.wikipedia.org" + ahref ]
  
  // Return the page title together with all the links
  title, allLinks


let rec randomWalk (rnd:Random) count url = 
  if count = 5 then
    []
  else     
    let title, links = download url

    // --------------------------------------------------------------
    // TASK #1 
    //  - Use 'rnd.Next' to get a random number from 0 to links-1
    //  - Pick the next link from the 'links' array 
    //  - Call 'randomWalk' recursively with random link!
    // --------------------------------------------------------------

    let rest = randomWalk rnd (count+1) url
    title :: rest
  
let startRandomWalk seed url =
  let rnd = new Random(seed)
  randomWalk rnd 0 url

startRandomWalk 1 "https://en.wikipedia.org/wiki/Charles_University"

// ------------------------------------------------------------------

// TASK #2: Make the code asynchronous! What is the primitive 
// operation that we can turn into async? Then, add 'async { .. }'
// blocks as needed; add 'return' as needed and add 'let!' when
// calling another asynchronous operation.

// TASK #3: Parallelize the operation using 'Async.Parallel'
// Let the funniest random walk through Wikipedia win :-)
// (Make sure to pass unique seed to each random number generator! 
// We cannot have one global, because it's not thread safe.)

// ------------------------------------------------------------------