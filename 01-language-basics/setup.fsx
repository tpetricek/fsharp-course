// ============================================================================
// SETUP: Some magic tricks for the script Walkthroughs!
// ============================================================================

let __<'T> : 'T =
  failwith "A place holder '__' has not been filled!"

let shouldEqual (a:'T) b =
  if typeof<'T> = typeof<float> then
    let a = unbox<float> a
    let b = unbox<float> b
    if a > b - 0.0000001 && a < b + 0.0000001 then ()
    else failwithf "The 'shouldEqual' operation failed!\nFirst: %A\nSecond: %A" a b
  else if not (a = b) then failwithf "The 'shouldEqual' operation failed!\nFirst: %A\nSecond: %A" a b

type Medal =
 { Games : string
   Year : int
   Sport : string
   Discipline : string
   Athlete : string
   Team : string
   Gender : string
   Event : string 
   Medal : string }

let medals = 
  [ for l in System.IO.File.ReadAllLines(__SOURCE_DIRECTORY__ + "/medals.csv") |> Seq.skip 1 do
      match l.Split(';') with 
      | [|g;y;s;d;a;t;n;e;m|] -> { Games=g; Year=int y; Sport=s; Discipline=d; Athlete=a; Team=t; Gender=n; Event=e; Medal=m } 
      | a -> failwithf "failed to read row '%A' from medals.csv!" a ]
(*      
#r "nuget: FSharp.Data"
open FSharp.Data

type Data = FSharp.Data.CsvProvider< @"C:\Tomas\Materials\Teaching\2024\fsharp-course\01-language-basics\medals-raw.csv" >

type Codes = FSharp.Data.HtmlProvider< @"https://raw.githubusercontent.com/the-gamma/thegamma-services/refs/heads/master/data/countrycodes.html" >
let countries = 
  [ yield "KOS", "Kosovo"
    yield "SRB", "Serbia"
    yield "TTO", "Trinidad and Tobago"
    for r in Codes.GetSample().Tables.``3-Digit Country Codes``.Rows do 
      yield r.Code, r.Country.TrimEnd('*') ]  |> dict

let nls=
  [ for r in Data.GetSample().Rows do
     let l= 
      [ r.Games.Substring(0, r.Games.IndexOf('(')-1)
        string r.Year
        r.Sport
        r.Discipline
        r.Athlete
        countries.[r.Team]
        r.Gender
        r.Event
        r.Medal
        ]
        |> String.concat ";"
     //if l.Contains(';') then failwith "!"
     l
      ]

System.IO.File.WriteAllLines(@"C:\Tomas\Materials\Teaching\2024\fsharp-course\01-language-basics\medals.csv", nls)
*)