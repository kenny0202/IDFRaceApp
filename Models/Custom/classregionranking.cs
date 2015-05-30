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
    
    public partial class classregionranking
    {
        public classregionranking()
        {
            this.personrankings = new HashSet<personranking>();
            this.racelevelrankings = new HashSet<racelevelranking>();
            this.racetitlerankings = new HashSet<racetitleranking>();
        }
    
        public int ClassRegionRankingID { get; set; }
        public int RaceClassID { get; set; }
        public int RegionID { get; set; }
        public int BuildRankingMethodID { get; set; }
        public int Year { get; set; }
        public string Description { get; set; }
        public int Unum { get; set; }
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }
    
        public virtual buildrankingmethod buildrankingmethod { get; set; }
        public virtual raceclass raceclass { get; set; }
        public virtual region region { get; set; }
        public virtual ICollection<personranking> personrankings { get; set; }
        public virtual ICollection<racelevelranking> racelevelrankings { get; set; }
        public virtual ICollection<racetitleranking> racetitlerankings { get; set; }
    }
}
