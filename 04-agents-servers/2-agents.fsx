// ------------------------------------------------------------------
// DEMO #1: Introducing agents
// ------------------------------------------------------------------

// The following example implements a simple counting agent that
// keeps a number as a state (using a recursive function) and
// handles two messages - one to 'Increment' and another to 'Print'

type CounterMessage = 
  | Increment
  | Print

let counter =
  MailboxProcessor.Start(fun inbox -> 
    let rec loop count = async { 
      let! msg = inbox.Receive()
      match msg with
      | Increment -> 
          return! loop (count + 1)
      | Print ->
          printfn "%d" count
          return! loop count }
    loop 0)

counter.Post(Increment)
counter.Post(Increment)
counter.Post(Print)

// TASK #1: Change the message type so that the agent handles 
// 'Update of int' which adds the specified number (which can
// also be negative) and 'Reset' which resets the state of the
// agent's state to zero. The following should work:
//
//   counter.Post(Update(+5))
//   counter.Post(Update(-15))
//   counter.Post(Print)
//   counter.Post(Reset)

// ------------------------------------------------------------------
// DEMO #2: Getting data from an agent
// ------------------------------------------------------------------

type StatsMessage = 
  | Add of float
  | Get of AsyncReplyChannel<float>

let stats =
  MailboxProcessor.Start(fun inbox -> 
    let rec loop sum = async { 
      let! msg = inbox.Receive()
      match msg with
      | Add(num) -> 
          return! loop (sum + num)
      | Get(repl) ->
          repl.Reply(sum)
          return! loop sum }
    loop 0.0)

stats.Post(Add 123.2)
stats.Post(Add 42.0)

stats.PostAndAsyncReply(fun ch -> Get(ch))
|> Async.RunSynchronously

// TASK #2: Change the agent so that it calculates the average
// of all the values that it received so far. You need to change
// the internals so that the agent keeps not just the current 
// 'sum' but also something more (e.g. count)
//
// So the following (right after creating the agent) should hold:
//
//   stats.Post(Add 1.2)
//   stats.Post(Add 2.3)
//   stats.Post(Add 3.7)
//
//   ( stats.PostAndAsyncReply(fun ch -> Average(ch))
//     |> Async.RunSynchronously ) = 2.4

// ------------------------------------------------------------------
// TASK #3: Writing a chatroom agent
// ------------------------------------------------------------------

type ChatMessage = 
  | Post of string * string
  | Retrieve of AsyncReplyChannel<string>

let chat = MailboxProcessor.Start(fun inbox ->
    failwith "TODO!"
  )

chat.Post(Post("Tomas", "hi"))
chat.Post(Post("Tomas", "F# is cool!"))

chat.PostAndAsyncReply(Retrieve)
|> Async.RunSynchronously

// Should return:
//
//   <ul>
//   <li><strong>Tomas</strong>: hi</li>
//   <li><strong>Tomas</strong>: F# is cool!</li>
//   </ul>

