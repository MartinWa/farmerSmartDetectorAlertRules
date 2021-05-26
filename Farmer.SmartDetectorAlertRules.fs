module FarmerExtension.SmartDetectorAlertRules

open Farmer
open Farmer.Arm
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

    member this.AsString =
        match this with
        | Sev1 -> "Sev1"
        | Sev2 -> "Sev2"
        | Sev3 -> "Sev3"
        | Sev4 -> "Sev4"

type SmartDetectorAlertRules =
    { Name: ResourceName
      Description: string
      ApplicationInsightsName: ResourceName
      ActionGroups: ActionGroups seq
      Frequency: TimeSpan  //IsoDateTime 
      Severity: Severity }

    interface IArmResource with
        member this.ResourceId =
            smartDetectorAlertRules.resourceId this.Name

        member this.JsonModel =
            {| smartDetectorAlertRules.Create(this.Name, Location.Global) with
                   properties =
                       {| description = this.Description
                          state = "Enabled"
                          severity = this.Severity.AsString
                          frequency = XmlConvert.ToString(this.Frequency)
                          detector = {| id = "FailureAnomaliesDetector" |}
                          scope =
                              [ (Insights.components.resourceId this.ApplicationInsightsName)
                                    .ArmExpression
                                    .Value ]
                          actionGroups =
                              {| groupIds =
                                     [ for actionGroup in this.ActionGroups do
                                           (ActionGroups.actionGroups.resourceId actionGroup.Name)
                                               .ArmExpression
                                               .Value ] |} |} |}
            :> _

    interface IBuilder with
        member this.ResourceId =
            smartDetectorAlertRules.resourceId this.Name

        member this.BuildResources location =
            [ { Name = this.Name
                Description = this.Description
                ApplicationInsightsName = this.ApplicationInsightsName
                ActionGroups = this.ActionGroups
                Frequency = this.Frequency
                Severity = this.Severity } ]
