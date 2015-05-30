using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IDFWebApp.Models.Custom
{
    [MetadataType(typeof(PreferenceMetaData))]
    public partial class preference
    {
        public string RaceEvent { get; set; }

        public string RaceClass { get; set; }

        public string QualificationMethod { get; set; }

        public string BracketMethod { get; set; }

        public string RacesPerHeat { get; set; }

        public string RacersPerClass { get; set; }

        public string Year { get; set; }

        public string TweetQualifications { get; set; }

        public string TweetBrackets { get; set; }

        public string TweetForm { get; set; }

        public string HotSpotWebSite { get; set; }

        public string EventLevelID { get; set; }

    }

    public class PreferenceMetaData
    {

    }
}
