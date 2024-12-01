module Chat.Main
open Chat.Server

open Giraffe
open System.IO
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Cors.Infrastructure
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.Extensions.DependencyInjection

let configureCors (builder:CorsPolicyBuilder) =
  builder.WithOrigins("http://localhost:5000")
    .AllowAnyMethod().AllowAnyHeader() |> ignore

let configureApp (app : IApplicationBuilder) =
  let env = app.ApplicationServices.GetService<IWebHostEnvironment>()
  app.UseDeveloperExceptionPage().UseCors(configureCors)
    .UseStaticFiles().UseGiraffe(server ())

let configureServices (services:IServiceCollection) =
  services.AddCors() |> ignore
  services.AddGiraffe() |> ignore

let configureLogging (builder:ILoggingBuilder) =
  builder.AddConsole().AddDebug() |> ignore

let contentRoot = Directory.GetCurrentDirectory()
let webRoot = Path.Combine(contentRoot, "web")
Host.CreateDefaultBuilder().ConfigureWebHostDefaults(fun builder ->
  builder.UseContentRoot(contentRoot).UseWebRoot(webRoot)
    .Configure(configureApp)
    .UseUrls("http://localhost:5000")
    .ConfigureServices(configureServices)
    .ConfigureLogging(configureLogging) |> ignore)
  .Build()
  .Run()