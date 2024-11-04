module FSharpDemos.Html

open Browser
open Browser.Types
open Fable.Core

// ----------------------------------------------------------------------------
// Minimal implementation of the "Elm" programming model using the 
// JavaScript "virtual-dom" library to handle rendering update. For real-world
// use, it is better to use Fable.React (https://github.com/fable-compiler/fable-react)
// but this works well enough for small things and avoids dependencies.
// ----------------------------------------------------------------------------

module Virtualdom = 
  [<Import("h","virtual-dom")>]
  let h(arg1: string, arg2: obj, arg3: obj[]): obj = failwith "JS only"

  [<Import("diff","virtual-dom")>]
  let diff (tree1:obj) (tree2:obj): obj = failwith "JS only"

  [<Import("patch","virtual-dom")>]
  let patch (node:obj) (patches:obj): Browser.Types.Node = failwith "JS only"

  [<Import("create","virtual-dom")>]
  let createElement (e:obj): Browser.Types.Node = failwith "JS only"

[<Emit("$0[$1]")>]
let private getProperty (o:obj) (s:string) = failwith "!"

[<Emit("event")>]
let private event () : Event = failwith "JS"

type DomAttribute = 
  | Event of (HTMLElement -> Event -> unit)
  | Attribute of string

type DomNode = 
  | Text of string
  | Element of ns:string * tag:string * attributes:(string * DomAttribute)[] * children : DomNode[] 
  
let innerText nd = 
  let sb = System.Text.StringBuilder()
  let rec loop = function
    | Text s -> sb.Append(s) |> ignore
    | Element(_, _, _, cs) -> for c in cs do loop c
  loop nd
  sb.ToString()

let createTree ns tag args children =
    let attrs = ResizeArray<_>()
    let props = ResizeArray<_>()
    for k, v in args do
      match k, v with 
      | k, Attribute v ->
          attrs.Add (k, box v)
      | k, Event f ->
          props.Add ("on" + k, box (fun o -> f (getProperty o "target") (event()) ))
    let attrs = JsInterop.createObj attrs
    let ns = if ns = null || ns = "" then [] else ["namespace", box ns]
    let props = JsInterop.createObj (Seq.append (ns @ ["attributes", attrs]) props)
    let elem = Virtualdom.h(tag, props, children)
    elem

let mutable counter = 0

let rec renderVirtual node = 
  match node with
  | Text(s) -> 
      box s
  | Element(ns, tag, attrs, children) ->
      let children = Array.map renderVirtual children 
      createTree ns tag attrs children

let createVirtualDomApp id initial r u = 
  let event = new Event<'T>()
  let trigger e = event.Trigger(e)  
  let mutable container = document.createElement("div") :> Node
  document.getElementById(id).innerHTML <- ""
  document.getElementById(id).appendChild(container) |> ignore
  let mutable tree = Fable.Core.JsInterop.createObj []
  let mutable state = initial

  let setState newState = 
    state <- newState
    let newTree = r trigger state |> renderVirtual
    let patches = Virtualdom.diff tree newTree
    container <- Virtualdom.patch container patches
    tree <- newTree
  let getState() = 
    state
  
  setState initial
  event.Publish.Add(fun e -> setState (u state e))
  
let text s = Text(s)
let (=>) k v = k, Attribute(v)
let (=!>) k f = k, Event(f)

type El(ns) = 
  member x.Namespace = ns
  static member (?) (el:El, n:string) = fun a b ->
    Element(el.Namespace, n, Array.ofList a, Array.ofList b)

let h = El(null)
