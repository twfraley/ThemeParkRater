using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThemeParkRater.Models.ThemeParkRatingModels
{
    class ThemeParkRatingDetails
    {
        public int ThemeParkRatingID { get; set; }
        public float GoodnessLevel { get; set; }
        public int ThemeParkID { get; set; }
        public string ThemeParkName { get; set; }
    }
}
