using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDFWebApp.Models.Custom
{
    [MetadataType(typeof(EventClassMetaData))]
    public partial class eventclass
    {
        [Display(Name = "Competitor Level")]
        public int CompetitorLevel { get; set; }
    }

    public class EventClassMetaData
    {

        [ScaffoldColumn(false)]
        public int RaceEventID { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "Class")]
        [Required]
        public int RaceClassID { get; set; }

        [ScaffoldColumn(true)]
        [Display(Name = "Build Qualification Method")]
        [Required]
        public int BuildQualificationMethodID { get; set; }

        [Required]
        [ScaffoldColumn(true)]
        [Display(Name = "Build Bracket Method")]
        public int BuildBracketMethodID { get; set; }

        [ScaffoldColumn(false)]
        public int LinkType { get; set; }

        [Required]
        [Display(Name = "Races per Heat")]
        public int RacersPerHeat { get; set; }

        [Required]
        [Display(Name = "Races per class")]
        public int RacersPerClass { get; set; }

        [Required]
        [Display(Name = "Repechage Places")]
        public int RepechagePlaces { get; set; }

        [Required]
        [Display(Name = "Repechage Qualifiers")]
        public int RepechageQualifiers { get; set; }

        [Required]
        [Display(Name = "Primary Class Cost")]
        [Range(0, 9999999999999999999.9999)]
        public decimal PrimaryClassCost { get; set; }

        [Required]
        [Display(Name = "Secondary Class Cost")]
        [Range(0, 9999999999999999999.9999)]
        public decimal SecondaryClassCost { get; set; }

        [ScaffoldColumn(false)]
        public int Unum { get; set; }

        [ScaffoldColumn(false)]
        public int UnumSync { get; set; }
        public System.DateTime UnumTime { get; set; }

       
    }
}
