﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThemeParkRater.Data
{
    public class ThemePark
    {
        [Key]
        public int ThemeParkID { get; set; }
        [Required]
        public string ThemeParkName { get; set; }
        [Required]
        public string ThemeParkCity { get; set; }
        [Required]
        public string ThemeParkState { get; set; }
        
        public float GoodnessLevel { get; set; }

    }
}
