 
function OnSave(executionContext) {
    validateEmailFormat(executionContext);
}

function FieldbossEmail_OnChange(executionContext)
{
validateEmailFormat(executionContext);
}

function validateEmailFormat(executionContext)
{
const formContext = executionContext.getFormContext();
    const emailRegEx = "^[a-zA-Z0-9._]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
    const fieldbossEmailNotification = "NOTIFICATION_FIELDBOSS_EMAIL";

    formContextGlobalRef = formContext;

    var fieldbossEmail = formContext.getAttribute("emailaddress1").getValue();

    var eventTrigger = executionContext.getEventArgs();

    if(fieldbossEmail)
    {
        formContext.getControl("emailaddress1").clearNotification(fieldbossEmailNotification);

        if(fieldbossEmail.match(emailRegEx) != null)
        {
            //We have a valid email, clear error if any and let it go through
            formContext.getControl("emailaddress1").clearNotification("2");
            //alert("Valid");
        }
        else
        {
            //Not a valid email format, put up a form notification type error
            formContext.getControl("emailaddress1").setNotification("Please Enter A Valid Email Address.","2");
            //alert("Not Valid");
        }

    }

}
