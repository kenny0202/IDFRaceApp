using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDFWebApp.Models.Custom
{
    [MetadataType(typeof(OrganizationRoleTypeMetadata))]
    public partial class organizationroletype
    {
    }

    public class OrganizationRoleTypeMetadata
    {
        
        public int OrganizationRoleTypeID { get; set; }

        [Display(Name = "Organization")]
        public int OrganizationID { get; set; }

        [Display(Name = "Usage Type")]
        public int UsageType { get; set; }

        [Display(Name = "Competitor Level")]
        public int CompetitorLevel { get; set; }
   
        public int Sequence { get; set; }

        [Required]
        [MaxLength(10, ErrorMessage = "Maximum 10 characters allowed.")]
        public string Code { get; set; }

        [Required]
        [Display(Name="Role")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "The role name can only contain letters")]
        [MaxLength(20, ErrorMessage = "Maximum 20 characters allowed.")]
        public string Name { get; set; }

        public string Parameters { get; set; }

        [Display(Name = "Currency")]
        public int CurrencyID { get; set; }

        [Required]
        [Display(Name = "Dues Amount")]
        public decimal DuesAmount { get; set; }

        [Required]
        [Display(Name = "Renewal Type")]
        public int RenewalType { get; set; }

        [ScaffoldColumn(false)]
        public int Unum { get; set; }

        [ScaffoldColumn(false)]
        public int UnumSync { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime UnumTime { get; set; }

        public virtual currency currency { get; set; }
        public virtual organization organization { get; set; }
        public virtual ICollection<personorganizationrole> personorganizationroles { get; set; }
    }
}
