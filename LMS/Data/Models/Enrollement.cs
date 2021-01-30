﻿using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Enrollement
    {
        [Key]
        public int EnrollmentId { get; set; }
        public int AccountId { get; set; }
        public int CourseId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime DeleteDate { get; set; }
    }
}
