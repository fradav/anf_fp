#if !INTERACTIVE
#r "paket:
storage: packages
nuget FSharp.Data
nuget Deedle
nuget XPlot.Plotly
nuget System.Data.SQLite
nuget SQLProvider"
#endif
#load "./.fake/fsharp_ANF.fsx/intellisense.fsx"

#if INTERACTIVE
do fsi.AddPrinter(fun (printer:Deedle.Internal.IFsiFormattable) -> "\n" + (printer.Format()))
#r "System.Data.Linq"
#r "System.Linq"
#endif

////////////////////////
/// Seq et Pipelines

open FSharp.Data

type Gbpusd = CsvProvider<const(__SOURCE_DIRECTORY__ + "/data/gbpusd.csv")>
let gbpusd = Gbpusd.GetSample().Rows

gbpusd
|> Seq.pairwise
|> Seq.filter (fun (before, after) -> before.GbpUsd - after.GbpUsd < 0.2M)
|> Seq.map (fun (before,_) -> before.Date.Date.ToShortDateString())

////////////////////////
/// Frames

open Deedle

let titanic = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/data/titanic.csv")

titanic
|> Frame.filterRows (fun _ row -> row.GetAs "Sex" = "female")
|> Stats.mean

titanic
|> Frame.groupRowsByString "Sex"
|> Frame.applyLevel fst Stats.mean

////////////////////////
/// Type providers I

open FSharp.Data
open XPlot.Plotly

[| 1..10 |]  |> Chart.Line |> ignore

let wb = WorldBankData.GetDataContext()

wb.Countries.France.Indicators.``CO2 emissions (kt)``
|> Chart.Line
|> Chart.Show

wb.Countries.Germany.Indicators.``CO2 emissions (kt)``
|> Chart.Line
|> Chart.Show

wb.Countries.``United States``.Indicators.``CO2 emissions (kt)``
|> Chart.Line
|> Chart.Show

type W = JsonProvider<"http://api.openweathermap.org/data/2.5/forecast/daily?q=Prague&mode=json&units=metric&cnt=10&APPID=cb63a1cf33894de710a1e3a64f036a27">

let getTemps city = 
  let w = W.Load("http://api.openweathermap.org/data/2.5/forecast/daily?q=" + city + "&mode=json&units=metric&cnt=10&APPID=cb63a1cf33894de710a1e3a64f036a27")
  [ for d in w.List -> d.Temp.Day ]

let cities = ["Paris";"Lyon";"Marseille"]

cities 
|> Seq.map (getTemps >> Seq.indexed)
|> Chart.Column
|> Chart.WithLabels cities
|> Chart.Show

////////////////////////
/// Type providers II

[<Literal>]
let ConnectionString = 
    "Data Source=" + 
    __SOURCE_DIRECTORY__ + @"/data/northwindEF.db;" + 
    "Version=3;foreign keys=true"

[<Literal>]
let ResolutionPath = __SOURCE_DIRECTORY__ + @"/local"

open FSharp.Data.Sql

type Sql = SqlDataProvider<
                Common.DatabaseProviderTypes.SQLITE, 
                SQLiteLibrary = Common.SQLiteLibrary.SystemDataSQLite,
                ConnectionString = ConnectionString, 
                ResolutionPath = ResolutionPath, 
                CaseSensitivityChange = Common.CaseSensitivityChange.ORIGINAL>

let db = Sql.GetDataContext()

let customers =
    db.Main.Customers
    |> Seq.map (fun c -> c.ContactName)
    |> Seq.toList


////////////////////////
/// Computations expressions

// Sequences
let naturals =
    let rec nat k = 
        seq {
            yield k
            yield! nat (k + 1)
        }
    nat 0

naturals
|> Seq.map (fun x -> x * x)
|> Seq.take 10
|> Seq.iter (printfn "%d")

open System.Linq

// Query
let customerslinq =
    query {
        for c in db.Main.Customers do
        groupBy c.Country into y
        sortByDescending (y.Count())
        take 5
        select (y.Key, y.Count())
    }

customerslinq
|> Seq.iter (printfn "%A")

// Async
open System.Net
open Microsoft.FSharp.Control.WebExtensions

let urlList = [ "Microsoft.com", "http://www.microsoft.com/"
                "MSDN", "http://msdn.microsoft.com/"
                "Bing", "http://www.bing.com"
              ]

let fetchAsync(name, url:string) =
    async {
        try
            let uri = System.Uri(url)
            let webClient = new WebClient()
            let! html = webClient.AsyncDownloadString(uri)
            printfn "Read %d characters for %s" html.Length name
        with
            | ex -> printfn "%s" (ex.Message);
    }

let runAll() =
    urlList
    |> Seq.map fetchAsync
    |> Async.Parallel
    |> Async.RunSynchronously
    |> ignore

runAll()
