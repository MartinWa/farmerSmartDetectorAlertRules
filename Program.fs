open Farmer
open Farmer.Builders
open FarmerExtension.SmartDetectorAlertRules
open System

let ai = appInsights { name "myAI" }

let smartDetectorAlertRules =
    { Name = ResourceName "test"
      Description =
          "Failure Anomalies notifies you of an unusual rise in the rate of failed HTTP requests or dependency calls."
      Scope = ai.InstrumentationKey.Value
      ActionGroups = []
      Frequency = TimeSpan.FromMinutes(1.0)
      Severity = Severity.Sev3 }

let deployment =
    arm {
        location Location.WestEurope
        add_resources [ smartDetectorAlertRules ]
    }

[<EntryPoint>]
let main argv =
    printf "Generating ARM template..."

    deployment
    |> Writer.quickWrite "generated-template"

    printfn "all done! Template written to generated-template.json"
    0 // return an integer exit code
