﻿module TradeOps.Output

open System
open System.IO
open System.Reflection
open RazorEngine.Configuration
open RazorEngine.Templating
open TradeOps.Models

//-------------------------------------------------------------------------------------------------

let private folderOutput = Environment.GetEnvironmentVariable("UserProfile") + @"\Desktop\Output\"
let private folderStyles = Path.Combine(folderOutput, "css")

let private templateResolver name =
    use stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name)
    if (stream = null) then failwith ("Cannot load resource: " + name)
    use reader = new StreamReader(stream)
    reader.ReadToEnd()

let private config = new TemplateServiceConfiguration()
config.TemplateManager <- new DelegateTemplateManager(fun x -> templateResolver x)
config.CachingProvider <- new DefaultCachingProvider(fun x -> ignore x)
config.DisableTempFileLocking <- true

let private service = RazorEngineService.Create(config)

//-------------------------------------------------------------------------------------------------

let writeTransactionListing (model : TransactionListing.Model) =

    Directory.CreateDirectory(folderOutput) |> ignore
    Directory.CreateDirectory(folderStyles) |> ignore

    let filename = "Report.css"
    let contents = service.RunCompile(filename, null, ())
    let path = Path.Combine(folderStyles, filename)
    File.WriteAllText(path, contents)

    let filename = "TransactionListing.html"
    let contents = service.RunCompile(filename, null, model)
    let path = Path.Combine(folderOutput, filename)
    File.WriteAllText(path, contents)

//-------------------------------------------------------------------------------------------------

let writeStatementPositions (model : StatementPositions.Model) =

    Directory.CreateDirectory(folderOutput) |> ignore
    Directory.CreateDirectory(folderStyles) |> ignore

    let filename = "Report.css"
    let contents = service.RunCompile(filename, null, ())
    let path = Path.Combine(folderStyles, filename)
    File.WriteAllText(path, contents)

    let filename = "StatementPositions.html"
    let contents = service.RunCompile(filename, null, model)
    let path = Path.Combine(folderOutput, filename)
    File.WriteAllText(path, contents)
