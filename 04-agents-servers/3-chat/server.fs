module Chat.Server
open Giraffe
open Microsoft.AspNetCore.Http

// ------------------------------------------------------------------
// DEMO: Agent for handling chat room state
// ------------------------------------------------------------------

type ChatMessage = 
  | Post of string * string
  | Retrieve of AsyncReplyChannel<string>

let chat = MailboxProcessor.Start(fun inbox ->
    let rec loop messages = async {
      let! msg = inbox.Receive()
      match msg with 
      | Post(who, text) -> 
          return! loop ((who, text)::messages)
      | Retrieve(repl) ->
          let all = messages |> List.rev |> List.map (fun (w, t) ->
            $"<li><strong>{w}</strong>: {t}</li>") 
          let str = "<ul>" + String.concat "" all + "</ul>"
          repl.Reply(str) 
          return! loop messages }
    loop [] 
  )

// ------------------------------------------------------------------
// TASKS: 
// ------------------------------------------------------------------

chat.Post(Post("Tomas", "hi"))
chat.Post(Post("Tomas", "F# is cool!"))

let getChannels = 
  text "<option value='default'>Default</option>"

// TASK #1: Get content and post message to the agent 
// (for simplicity, start with synchronous version!)


let getContent xx = 
  failwith "implement me!"

let postMessage xx = 
  // HINT: The client sends you the contents of the message as HTTP body
  // To get this from 'HttpContext' use: ctx.ReadBodyFromRequestAsync()  
  failwith "implement me!"


let getContentAsync xx = 
  failwith "implement me!"

let postMessageAsync xx = 
  failwith "implement me!"


let server () =
  choose [
    GET >=> route "/" >=> htmlFile "web/index.html"
    GET >=> route "/channels" >=> getChannels
    
    // TASK #1: Handle 'GET' requests to '/content' using 'getContent'
    // TASK #2: Handle 'POST' requests to '/post/%s' using 'postMessage'
    // TASK #3: Implement asynchronous version of those!

    setStatusCode 404 >=> text "Not Found" ]
