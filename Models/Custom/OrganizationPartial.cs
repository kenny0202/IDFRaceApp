using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDFWebApp.Models.Custom
{

    [MetadataType(typeof(OrganizationMetadata))]
    public partial class organization
    {
    }

    public class OrganizationMetadata
    {

        public int OrganizationID { get; set; }

        public int OrganizationTypeID { get; set; }

        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }

        [Required]
        public int Sequence { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Maximum 10 characters allowed.")]
        public string Code { get; set; }

        [Required]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name can only contain letters")]
        [MaxLength(60, ErrorMessage = "Maximum 60 characters allowed.")]
        public string Name { get; set; }

        public string Description { get; set; }

        [ScaffoldColumn(false)]
        public int Unum { get; set; }

        [ScaffoldColumn(false)]
        public int UnumSync { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime UnumTime { get; set; }

        public virtual ICollection<account> accounts { get; set; }
        public virtual ICollection<calendarevent> calendarevents { get; set; }
        public virtual ICollection<eventlevel> eventlevels { get; set; }
        public virtual ICollection<eventorganization> eventorganizations { get; set; }
        public virtual ICollection<raceclass> raceclasses { get; set; }
        public virtual ICollection<raceevent> raceevents { get; set; }
        public virtual organizationtype organizationtype { get; set; }
        public virtual ICollection<organizationroletype> organizationroletypes { get; set; }
        public virtual ICollection<placepointmatrix> placepointmatrices { get; set; }
        public virtual ICollection<preference> preferences { get; set; }
        public virtual ICollection<prizeaward> prizeawards { get; set; }
        public virtual ICollection<product> products { get; set; }
        public virtual ICollection<region> regions { get; set; }
    }
}
