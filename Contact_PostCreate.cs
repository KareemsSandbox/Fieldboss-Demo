

Unclassified / Non classifié

using System;
using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using FieldbossDemo.Plugins;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace FieldbossDemo
{
    public class Contact_PostCreate : PluginBase
    {
       public Contact_PostCreate(string unsecure, string secure)

           : base(typeof(Contact_PostCreate))

        {


        }

 
        protected override void ExecuteCrmPlugin(LocalPluginContext localContext)

        {
            //check localcontext

            if (localContext == null)

                throw new InvalidPluginExecutionException("localContext Missing");

            IPluginExecutionContext context = localContext.PluginExecutionContext;
            IOrganizationService service = localContext.OrganizationService;
            ITracingService tracingService = localContext.TracingService;

            try

            {

                //extract target from the input parameter

                tracingService.Trace("Checking if Target Exist");

                if (localContext.PluginExecutionContext.InputParameters.Contains("Target") &&

                    localContext.PluginExecutionContext.InputParameters["Target"] is Entity)

                {

                    //extract entity from target

                    tracingService.Trace("Extract Target entity from Target params");

                    var entity = (Entity)localContext.PluginExecutionContext.InputParameters["Target"]; 

                    //check if entity is type contact

                    localContext.Trace("Check the entity type");

 

                    if (entity.LogicalName.Equals("contact"))

                    {

                        localContext.Trace("We have a contact record.");

 

                        // Get Contact Attributes We Need Later

                        //EntityReference to newly created contact record
                        EntityReference refContact = entity.ToEntityReference();
                        //EntityReference to owner of contact record
                        EntityReference refContactOwner = entity.GetAttributeValue<EntityReference>("ownerid");
                        //Contact full name
                        string contactFullname = entity.GetAttributeValue<String>("fullname");
                        //Schedule task to be due in 7 days
                        DateTime dueDate = DateTime.Now.AddDays(7);
                        //Task subject
                        string taskSubject = $"Follow-up with {contactFullname}.";
                        //Task description
                        string taskDescription = $"This is a follow-up task with {contactFullname} and is due on {dueDate.ToString("dd-MMM-yyyy")}";

                        //1. Prepare Task Entity Record
                        Entity followupTask = new Entity("task");

                        followupTask["scheduledend"] = dueDate;
                        followupTask["regardingobjectid"] = refContact;
                        followupTask["subject"] = taskSubject;
                        followupTask["description"] = taskDescription;
                        followupTask["ownerid"] = refContactOwner;

                        //2. Create Task Record, get new Task Id
                        Guid taskId = service.Create(followupTask);

                        localContext.Trace("Task record created.");

                    }

                }

            }

            catch (InvalidPluginExecutionException ex)
            {
                localContext.Trace($"An error has occurred in FieldbossDemo.Contact_PostCreate Plugin: {ex.ToString()}");

                throw new InvalidPluginExecutionException("An error has occurred in FieldbossDemo.Contact_PostCreate Plugin.", ex);
            }

        }

    }

}

 
