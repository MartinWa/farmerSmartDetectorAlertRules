open Farmer
open FarmerExtension.SmartDetectorAlertRules

let smartDetectorAlertRules =
    { Name = ResourceName "test"
      Severity = Sev1 }

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
