using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IDFWebApp.Models.Custom
{
    [MetadataType(typeof(contactMetadata))]
    public partial class contact
    {
    }

    public class contactMetadata
    {
        [ScaffoldColumn(false)]
        public int ContactID { get; set; }
        public int PersonID { get; set; }
        [ScaffoldColumn(false)]
        public int ContactRoleID { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Name can only contain letters")]
        public string Name { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(\\+\\d{1,2}\\s)?\\(?\\d{3}\\)?[\\s.-]\\d{3}[\\s.-]\\d{4}$", ErrorMessage = "Phone can only contain numbers")]
        public string Phone { get; set; }
        [Required]
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "City can only contain letters")]
        public string City { get; set; }
        [Required]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "State can only contain letters")]
        public string State { get; set; }
        [Required]
        public string PostalCode { get; set; }
        [Required]
        public int CountryID { get; set; }
        [ScaffoldColumn(false)]
        public int Unum { get; set; }
        [ScaffoldColumn(false)]
        public int UnumSync { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime UnumTime { get; set; }

        public virtual country country { get; set; }
        public virtual person person { get; set; }
    }
}