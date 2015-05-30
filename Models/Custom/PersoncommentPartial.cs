using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IDFWebApp.Models.Custom
{
    [MetadataType(typeof(PersoncommentMetadata))]
    public partial class personcomment
    {
    }

    public class PersoncommentMetadata
    {

        public int PersonCommentID { get; set; }
        public int CommentTypeID { get; set; }
        public int PersonID { get; set; }
        [Required]
        public string Comment { get; set; }

        [ScaffoldColumn(false)]
        public int Unum { get; set; }
        [ScaffoldColumn(false)]
        public int UnumSync { get; set; }
        [ScaffoldColumn(false)]
        public System.DateTime UnumTime { get; set; }

        public virtual commenttype commenttype { get; set; }
        public virtual person person { get; set; }
    }
}