using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDFWebApp.Models.Custom
{

    [MetadataType(typeof(PersonOrganizationRoleMetaData))]
    public partial class personorganizationrole
    {

    }

    public class PersonOrganizationRoleMetaData
    {

        public int PersonOrganizationRoleID { get; set; }
        public int PersonID { get; set; }
        public int OrganizationRoleTypeID { get; set; }
        public int CompetitorLevel { get; set; }
        public int MemberNumber { get; set; }
        public decimal AnnualDuesPaid { get; set; }

        public Nullable<System.DateTime> ExpiryDate { get; set; }
        public string AccountNumber { get; set; }
        public string TransactionNumber { get; set; }
        public bool AutoRenewSubscription { get; set; }
        public bool PriorityStatus { get; set; }

        [ScaffoldColumn(false)]
        public int Unum { get; set; }

        [ScaffoldColumn(false)]
        public int UnumSync { get; set; }

        [ScaffoldColumn(false)]
        public System.DateTime UnumTime { get; set; }
     
    }
}
