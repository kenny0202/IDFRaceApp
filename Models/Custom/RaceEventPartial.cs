using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDFWebApp.Models.Custom
{
    [MetadataType(typeof(RaceEventMetaData))]
    public partial class raceevent
    {
       [Required(ErrorMessage = "Region is required.")]
        public int RegionID { get; set; }

    }


    public class RaceEventMetaData
    {
        // DON'T SCAFFOLD
        [ScaffoldColumn(false)]
        public int RaceEventID { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Organization")]
        public int OrganizationID { get; set; }

        [ScaffoldColumn(false)]
        [Required(ErrorMessage="Country is required.")]
        public int CountryID { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Event Level")]
        public int EventLevelID { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Currency")]
        public int CurrencyID { get; set; }

        [ScaffoldColumn(false)]
        public int RaceIndex { get; set; }

        [ScaffoldColumn(false)]
        public int Unum { get; set; }

        [ScaffoldColumn(false)]
        public int UnumSync { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime UnumTime { get; set; }


        //SCAFFOLD


        [Required]
        [StringLength(50, ErrorMessage = "Maximum of 50 characters.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "City name can only contain letters")]
        public string City { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Maximum of 100 characters.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Event name can only contain letters")]
        public string Name { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Maximum of 500 characters.")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        [Display(Name = "Start Date")]
        public System.DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        [Display(Name = "End Date")]
        public System.DateTime EndDate { get; set; }

        [Required]
        [Range(2015, 2100)]
        public int Year { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Priority Registration Date")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        public System.DateTime PriorityRegistrationDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Open Registration Date")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        public System.DateTime OpenRegistrationDate { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Closed Registration Date")]
        [DisplayFormat(DataFormatString = "{0:MMMM dd, yyyy}")]
        public System.DateTime ClosedRegistrationDate { get; set; }

        [Required]
        [Display(Name = "Maximum Competitors per Class")]
        public int MaximumCompetitorClasses { get; set; }

        [Required]
        [Range(0, 9999999999999999999.9999)]
        [Display(Name = "Primary Class Cost")]
        public decimal PrimaryClassCost { get; set; }

        [Required]
        [Range(0, 9999999999999999999.9999)]
        [Display(Name = "Secondary Class Cost")]
        public decimal SecondaryClassCost { get; set; }

    }
}
