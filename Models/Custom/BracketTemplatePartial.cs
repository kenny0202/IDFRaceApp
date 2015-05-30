using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDFWebApp.Models
{

    [MetadataType(typeof(BracketTemplateMetaData))]
    public partial class brakettemplate
    {

    }

    public class BracketTemplateMetaData
    {

        public int BracketTemplateID { get; set; }

        [Display(Name = "Bracket Size")]
        public int BracketSize { get; set; }

        [Display(Name = "Bracket Riders")]
        public int BracketRiders { get; set; }

        [Display(Name = "Bracket Number")]
        public int BracketNumber { get; set; }

        [Display(Name = "Bracket Seed")]
        public int BracketSeed { get; set; }

        [ScaffoldColumn(false)]
        public int Unum { get; set; }

        [ScaffoldColumn(false)]
        public int UnumSync { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime UnumTime { get; set; }

    }
}
