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
    
    public partial class personorganizationrole
    {
        public personorganizationrole()
        {
            this.personevents = new HashSet<personevent>();
            this.personrankings = new HashSet<personranking>();
        }
    
        public int PersonOrganizationRoleID { get; set; }
        public int PersonID { get; set; }
        public int OrganizationRoleTypeID { get; set; }
        public int CompetitorLevel { get; set; }
        public int MemberNumber { get; set; }
        public decimal AnnualDuesPaid { get; set; }
        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionNumber { get; set; }
        public bool AutoRenewSubscription { get; set; }
        public bool PriorityStatus { get; set; }
        public int Unum { get; set; }
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }
    
        public virtual organizationroletype organizationroletype { get; set; }
        public virtual person person { get; set; }
        public virtual ICollection<personevent> personevents { get; set; }
        public virtual ICollection<personranking> personrankings { get; set; }
    }
}
