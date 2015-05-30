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
    
    public partial class raceevent
    {
        public raceevent()
        {
            this.calendarevents = new HashSet<calendarevent>();
            this.eventclasses = new HashSet<eventclass>();
            this.eventorganizations = new HashSet<eventorganization>();
            this.personevents = new HashSet<personevent>();
            this.qualificationgroups = new HashSet<qualificationgroup>();
            this.raceeventregions = new HashSet<raceeventregion>();
        }
    
        public int RaceEventID { get; set; }
        public int OrganizationID { get; set; }
        public int CountryID { get; set; }
        public int EventLevelID { get; set; }
        public int CurrencyID { get; set; }
        public int RaceIndex { get; set; }
        public string City { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public System.DateTime StartDate { get; set; }
        public System.DateTime EndDate { get; set; }
        public int Year { get; set; }
        public System.DateTime PriorityRegistrationDate { get; set; }
        public System.DateTime OpenRegistrationDate { get; set; }
        public System.DateTime ClosedRegistrationDate { get; set; }
        public int MaximumCompetitorClasses { get; set; }
        public decimal PrimaryClassCost { get; set; }
        public decimal SecondaryClassCost { get; set; }
        public int Unum { get; set; }
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }
    
        public virtual ICollection<calendarevent> calendarevents { get; set; }
        public virtual country country { get; set; }
        public virtual currency currency { get; set; }
        public virtual ICollection<eventclass> eventclasses { get; set; }
        public virtual eventlevel eventlevel { get; set; }
        public virtual ICollection<eventorganization> eventorganizations { get; set; }
        public virtual organization organization { get; set; }
        public virtual ICollection<personevent> personevents { get; set; }
        public virtual ICollection<qualificationgroup> qualificationgroups { get; set; }
        public virtual ICollection<raceeventregion> raceeventregions { get; set; }
    }
}