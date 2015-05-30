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
    
    public partial class organization
    {
        public organization()
        {
            this.accounts = new HashSet<account>();
            this.calendarevents = new HashSet<calendarevent>();
            this.eventlevels = new HashSet<eventlevel>();
            this.eventorganizations = new HashSet<eventorganization>();
            this.raceclasses = new HashSet<raceclass>();
            this.raceevents = new HashSet<raceevent>();
            this.organizationroletypes = new HashSet<organizationroletype>();
            this.placepointmatrices = new HashSet<placepointmatrix>();
            this.preferences = new HashSet<preference>();
            this.prizeawards = new HashSet<prizeaward>();
            this.products = new HashSet<product>();
            this.regions = new HashSet<region>();
        }
    
        public int OrganizationID { get; set; }
        public int OrganizationTypeID { get; set; }
        public bool IsDeleted { get; set; }
        public int Sequence { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Unum { get; set; }
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }
    
        public virtual ICollection<account> accounts { get; set; }
        public virtual ICollection<calendarevent> calendarevents { get; set; }
        public virtual ICollection<eventlevel> eventlevels { get; set; }
        public virtual ICollection<eventorganization> eventorganizations { get; set; }
        public virtual ICollection<raceclass> raceclasses { get; set; }
        public virtual ICollection<raceevent> raceevents { get; set; }
        public virtual organizationtype organizationtype { get; set; }
        public virtual ICollection<organizationroletype> organizationroletypes { get; set; }
        public virtual ICollection<placepointmatrix> placepointmatrices { get; set; }
        public virtual ICollection<preference> preferences { get; set; }
        public virtual ICollection<prizeaward> prizeawards { get; set; }
        public virtual ICollection<product> products { get; set; }
        public virtual ICollection<region> regions { get; set; }
    }
}