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
    
    public partial class controltype
    {
        public controltype()
        {
            this.controls = new HashSet<control>();
        }
    
        public int ControlTypeID { get; set; }
        public int Sequence { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Parameters { get; set; }
        public int Unum { get; set; }
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }
    
        public virtual ICollection<control> controls { get; set; }
    }
}