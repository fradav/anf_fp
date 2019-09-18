#r "paket:
storage: packages
framework: netstandard2.0
source https://api.nuget.org/v3/index.json
nuget FSharp.Data
nuget Deedle
nuget XPlot.Plotly
nuget System.Data.SQLite
nuget SQLProvider //"
#load "./.fake/fsharp_ANF.fsx/intellisense.fsx"

#if INTERACTIVE
do fsi.AddPrinter(fun (printer:Deedle.Internal.IFsiFormattable) -> "\n" + (printer.Format()))
#r "System.Data.Linq"
#r "System.Linq"
#endif

/////////////////////////////////////////////////////////////////////////
///                       Seq and Pipes                               ///
/////////////////////////////////////////////////////////////////////////

open FSharp.Data

type Gbpusd = CsvProvider<const(__SOURCE_DIRECTORY__ + "/data/gbpusd.csv")>

////////////////////////
/// Frames

open Deedle

let titanic = Frame.ReadCsv(__SOURCE_DIRECTORY__ + "/data/titanic.csv")

/////////////////////////////////////////////////////////////////////////
///                       Type Providers I                            ///
/////////////////////////////////////////////////////////////////////////

open FSharp.Data
open XPlot.Plotly

[| 1..10 |]  |> Chart.Line |> ignore

let wb = WorldBankData.GetDataContext()


type W = JsonProvider<"http://api.openweathermap.org/data/2.5/forecast/daily?q=Prague&mode=json&units=metric&cnt=10&APPID=cb63a1cf33894de710a1e3a64f036a27">

let getTemps city = 
  let w = W.Load("http://api.openweathermap.org/data/2.5/forecast/daily?q=" + city + "&mode=json&units=metric&cnt=10&APPID=cb63a1cf33894de710a1e3a64f036a27")
  [ for d in w.List -> d.Temp.Day ]


/////////////////////////////////////////////////////////////////////////
///                       Type Providers II                           ///
/////////////////////////////////////////////////////////////////////////

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


/////////////////////////////////////////////////////////////////////////
///                Standard Computation Expressions                   ///
/////////////////////////////////////////////////////////////////////////

// Sequences


// Query
open System.Linq

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

