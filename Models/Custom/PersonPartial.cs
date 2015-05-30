using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDFWebApp.Models.Custom
{
    [MetadataType(typeof(PersonMetaData))]
    public partial class person
    {
    }

    public class PersonMetaData
    {
        [ScaffoldColumn(false)]
        [HiddenInput(DisplayValue = false)]
        public int PersonID { get; set; }

        [ScaffoldColumn(false)]
        public bool IsDeleted { get; set; }

        [ScaffoldColumn(false)]
        public bool SuperUser { get; set; }

        [ScaffoldColumn(false)]
        public int Unum { get; set; }

        [ScaffoldColumn(false)]
        public int UnumSync { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime UnumTime { get; set; }

        [Required]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public string Password { get; set; }

        [Required]
        [Display(Name = "First Name")]
        [MaxLength(40, ErrorMessage = "Maximum of 40 characters.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "First name can only contain letters")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [MaxLength(40, ErrorMessage = "Maximum of 40 characters.")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Last name can only contain letters")]
        public string LastName { get; set; }

        [Display(Name = "Nickname")]
        [MaxLength(60, ErrorMessage = "Maximum of 60 characters.")]
        public string NickName { get; set; }

        [Required]
        [Display(Name = "Nationality")]
        public int NationalityID { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Birth Date")]
        public System.DateTime BirthDate { get; set; }

        [Required]
        public int Gender { get; set; }

        public virtual ICollection<contact> contacts { get; set; }
        public virtual country country { get; set; }
        public virtual ICollection<identitykey> identitykeys { get; set; }
        public virtual ICollection<messagesubscription> messagesubscriptions { get; set; }
        public virtual ICollection<personcomment> personcomments { get; set; }
        public virtual ICollection<personmessage> personmessages { get; set; }
        public virtual ICollection<personorganizationrole> personorganizationroles { get; set; }
        public virtual ICollection<personproduct> personproducts { get; set; }
        public virtual ICollection<preference> preferences { get; set; }
    }
}
