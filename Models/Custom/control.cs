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
    
    public partial class control
    {
        public control()
        {
            this.controldbfields = new HashSet<controldbfield>();
        }
    
        public int ControlID { get; set; }
        public int FormID { get; set; }
        public int ControlTypeID { get; set; }
        public bool IsInSystem { get; set; }
        public bool IsDefined { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public int MaxLength { get; set; }
        public string Description { get; set; }
        public string ToolTip { get; set; }
        public int Unum { get; set; }
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }
    
        public virtual controltype controltype { get; set; }
        public virtual form form { get; set; }
        public virtual ICollection<controldbfield> controldbfields { get; set; }
    }
}
