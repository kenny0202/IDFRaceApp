using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDFWebApp.Models.Custom
{
    [MetadataType(typeof(RaceClassMetaData))]
    public partial class raceclass
    {
    }

    public class RaceClassMetaData
    {
        [ScaffoldColumn(false)]
        public int RaceClassID { get; set; }

        [ScaffoldColumn(false)]
        public int OrganizationID { get; set; }

        public int Sequence { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Maximum of 5 characters.")]
        public string Code { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Maximum of 50 characters.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Class name can only contain letters")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Maximum of 500 characters.")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Competitor Level")]
        public int CompetitorLevel { get; set; }

        [Required]
        [Display(Name = "Minimum age")]
        [Range(0, 100)]
        public int MinimumAge { get; set; }

        [Required]
        [Display(Name = "Maximum age")]
        [Range(0, 100)]
        public int MaximumAge { get; set; }


        public int Gender { get; set; }

        [ScaffoldColumn(false)]
        public int Unum { get; set; }

        [ScaffoldColumn(false)]
        public int UnumSync { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime UnumTime { get; set; }
    }
}
