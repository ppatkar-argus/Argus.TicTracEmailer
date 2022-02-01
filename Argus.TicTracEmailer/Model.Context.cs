﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Argus.TicTracEmailer
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class TicTracEntities : DbContext
    {
        public TicTracEntities()
            : base("name=TicTracEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
    
        public virtual ObjectResult<GetEmailTemplate_Result> GetEmailTemplate(string emailCode)
        {
            var emailCodeParameter = emailCode != null ?
                new ObjectParameter("EmailCode", emailCode) :
                new ObjectParameter("EmailCode", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GetEmailTemplate_Result>("GetEmailTemplate", emailCodeParameter);
        }
    
        public virtual int SendEmail(string emailBody, string emailSubject, string emailRecipient, string emailCC, string emailBCC, string emailFrom, string emailFromName, Nullable<bool> emailBodyHtml)
        {
            var emailBodyParameter = emailBody != null ?
                new ObjectParameter("EmailBody", emailBody) :
                new ObjectParameter("EmailBody", typeof(string));
    
            var emailSubjectParameter = emailSubject != null ?
                new ObjectParameter("EmailSubject", emailSubject) :
                new ObjectParameter("EmailSubject", typeof(string));
    
            var emailRecipientParameter = emailRecipient != null ?
                new ObjectParameter("EmailRecipient", emailRecipient) :
                new ObjectParameter("EmailRecipient", typeof(string));
    
            var emailCCParameter = emailCC != null ?
                new ObjectParameter("EmailCC", emailCC) :
                new ObjectParameter("EmailCC", typeof(string));
    
            var emailBCCParameter = emailBCC != null ?
                new ObjectParameter("EmailBCC", emailBCC) :
                new ObjectParameter("EmailBCC", typeof(string));
    
            var emailFromParameter = emailFrom != null ?
                new ObjectParameter("EmailFrom", emailFrom) :
                new ObjectParameter("EmailFrom", typeof(string));
    
            var emailFromNameParameter = emailFromName != null ?
                new ObjectParameter("EmailFromName", emailFromName) :
                new ObjectParameter("EmailFromName", typeof(string));
    
            var emailBodyHtmlParameter = emailBodyHtml.HasValue ?
                new ObjectParameter("EmailBodyHtml", emailBodyHtml) :
                new ObjectParameter("EmailBodyHtml", typeof(bool));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("SendEmail", emailBodyParameter, emailSubjectParameter, emailRecipientParameter, emailCCParameter, emailBCCParameter, emailFromParameter, emailFromNameParameter, emailBodyHtmlParameter);
        }
    }
}