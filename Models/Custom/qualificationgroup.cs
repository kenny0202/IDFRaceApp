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
    
    public partial class qualificationgroup
    {
        public qualificationgroup()
        {
            this.qualificationbrackets = new HashSet<qualificationbracket>();
            this.qualificationclasses = new HashSet<qualificationclass>();
            this.qualificationruns = new HashSet<qualificationrun>();
        }
    
        public int QualificationGroupID { get; set; }
        public int RaceEventID { get; set; }
        public string Name { get; set; }
        public int Unum { get; set; }
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }
    
        public virtual ICollection<qualificationbracket> qualificationbrackets { get; set; }
        public virtual ICollection<qualificationclass> qualificationclasses { get; set; }
        public virtual raceevent raceevent { get; set; }
        public virtual ICollection<qualificationrun> qualificationruns { get; set; }
    }
}
