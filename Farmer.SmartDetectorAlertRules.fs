module FarmerExtension.SmartDetectorAlertRules

open Farmer
open Farmer.Builders
open System
open System.Xml
open ActionGroups

let smartDetectorAlertRules =
    // ResourceType("microsoft.alertsmanagement/smartdetectoralertrules", "2021-04-01")
    ResourceType("microsoft.alertsmanagement/smartdetectoralertrules", "2019-06-01")

type Severity =
    | Sev1
    | Sev2
    | Sev3
    | Sev4

type SmartDetectorAlertRules =
    { Name: ResourceName
      Description: string
      Scope: string
      ActionGroups: ActionGroups seq
      Frequency: TimeSpan
      Severity: Severity }

    interface IArmResource with
        member this.ResourceId =
            smartDetectorAlertRules.resourceId this.Name

        member this.JsonModel =
            {| smartDetectorAlertRules.Create(this.Name, Location.Global) with
                   properties =
                       {| description = this.Description
                          state = "Enabled"
                          severity = this.Severity.GetType
                          frequency = XmlConvert.ToString(this.Frequency)
                          detector = {| id = "FailureAnomaliesDetector" |}
                          scope = [ this.Scope ]
                          actionGroups = {| groupIds = this.ActionGroups |} |} |}
            :> _

    interface IBuilder with
        member this.ResourceId =
            smartDetectorAlertRules.resourceId this.Name

        member this.BuildResources location =
            [ { Name = this.Name
                Description = this.Description
                Scope = this.Scope
                ActionGroups = this.ActionGroups
                Frequency = this.Frequency
                Severity = this.Severity } ]
