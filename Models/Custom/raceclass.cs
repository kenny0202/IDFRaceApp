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
    
    public partial class raceclass
    {
        public raceclass()
        {
            this.classregionrankings = new HashSet<classregionranking>();
            this.eventclasses = new HashSet<eventclass>();
        }
    
        public int RaceClassID { get; set; }
        public int OrganizationID { get; set; }
        public int Sequence { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CompetitorLevel { get; set; }
        public int MinimumAge { get; set; }
        public int MaximumAge { get; set; }
        public int Gender { get; set; }
        public int Unum { get; set; }
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }
    
        public virtual ICollection<classregionranking> classregionrankings { get; set; }
        public virtual ICollection<eventclass> eventclasses { get; set; }
        public virtual organization organization { get; set; }
    }
}
