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
    
    public partial class prizeaward
    {
        public int PrizeAwardID { get; set; }
        public int OrganizationID { get; set; }
        public int EventClassID { get; set; }
        public int Place { get; set; }
        public string Description { get; set; }
        public decimal PrizeValue { get; set; }
        public int CurrencyID { get; set; }
        public int Unum { get; set; }
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }
    
        public virtual eventclass eventclass { get; set; }
        public virtual organization organization { get; set; }
    }
}
