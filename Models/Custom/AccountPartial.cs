using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace IDFWebApp.Models.Custom
{

    [MetadataType(typeof(AccountMetaData))]
    public partial class account
    {
    }

    public class AccountMetaData
    {
        public int AccountID { get; set; }
        public int OrganizationID { get; set; }
        public int AccountTypeID { get; set; }
        public int CurrencyID { get; set; }

        [Required(ErrorMessage="The account name is required.")]
        [Display(Name="Account Name")]
        [RegularExpression("^[a-zA-Z ]+$", ErrorMessage = "Account name can only contain letters")]
        public string Name { get; set; }

        [Required(ErrorMessage="The billing number is required.")]
        [Display(Name = "Billing Number")]
        public string BillingNumber { get; set; }

        public string Parameters { get; set; }

        [ScaffoldColumn(false)]
        public int Unum { get; set; }

        [ScaffoldColumn(false)]
        public int UnumSync { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime UnumTime { get; set; }

        public virtual accounttype accounttype { get; set; }
        public virtual currency currency { get; set; }
        public virtual organization organization { get; set; }
    }
}
