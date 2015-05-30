//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IDFWebApp.Models.Custom
{
    using System;
    using System.Collections.Generic;
    
    public partial class messageitem
    {
        public messageitem()
        {
            this.personmessages = new HashSet<personmessage>();
        }
    
        public int MessageItemID { get; set; }
        public int MessageTypeID { get; set; }
        public bool IsPublished { get; set; }
        public Nullable<bool> CanReply { get; set; }
        public string FromAddress { get; set; }
        public System.DateTime DateStored { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public int Unum { get; set; }
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }
    
        public virtual messagetype messagetype { get; set; }
        public virtual ICollection<personmessage> personmessages { get; set; }
    }
}
