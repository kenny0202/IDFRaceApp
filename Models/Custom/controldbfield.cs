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
    
    public partial class controldbfield
    {
        public int ControlDbFieldID { get; set; }
        public int ControlID { get; set; }
        public int DbFieldID { get; set; }
        public string Description { get; set; }
        public int Unum { get; set; }
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }
    
        public virtual control control { get; set; }
        public virtual dbfield dbfield { get; set; }
    }
}