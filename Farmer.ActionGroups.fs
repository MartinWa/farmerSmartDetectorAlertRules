module FarmerExtension.ActionGroups

open Farmer

let actionGroups =
    ResourceType("microsoft.insights/actionGroups", "2019-06-01")

type EmailReceiver = { Name: string; EmailAddress: string }

type ActionGroups =
    { Name: ResourceName
      Location: Location
      ShortName: string
      EmailReceivers: seq<EmailReceiver> }

    interface IArmResource with
        member this.ResourceId = actionGroups.resourceId this.Name

        member this.JsonModel =
            {| actionGroups.Create(this.Name, this.Location) with
                   properties =
                       {| groupShortName = this.ShortName
                          enabled = true
                          emailReceivers =
                              [ for emailReceiver in this.EmailReceivers do
                                    {| name = emailReceiver.Name
                                       emailAddress = emailReceiver.EmailAddress
                                       useCommonAlertSchema = false |} ] |} |}
            :> _

    interface IBuilder with
        member this.ResourceId = actionGroups.resourceId this.Name

        member this.BuildResources location =
            [ { Name = this.Name
                Location = location
                ShortName = this.ShortName
                EmailReceivers = this.EmailReceivers } ]
