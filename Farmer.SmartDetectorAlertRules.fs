module FarmerExtension.SmartDetectorAlertRules

open Farmer
open Farmer.Builders

let smartDetectorAlertRules =
    // ResourceType("microsoft.alertsmanagement/smartdetectoralertrules", "2021-04-01")
    ResourceType("microsoft.alertsmanagement/smartdetectoralertrules", "2019-06-01")

type Severity =
    | Sev1
    | Sev2

type SmartDetectorAlertRules =
    { Name: ResourceName
      Severity: Severity }

    interface IArmResource with
        member this.ResourceId =
            smartDetectorAlertRules.resourceId this.Name

        member this.JsonModel =
            {| smartDetectorAlertRules.Create(this.Name, Location.Global) with
                   properties =
                       {| description = "s"
                          state = "Enabled"
                          severity = this.Severity.ToString
                          frequency = ""
                          detector = ""
                          scope = ""
                          actionGroups = "" |} |}
            :> _

    interface IBuilder with
        member this.ResourceId =
            smartDetectorAlertRules.resourceId this.Name

        member this.BuildResources location =
            [ { Name = this.Name
                Severity = this.Severity } ]
