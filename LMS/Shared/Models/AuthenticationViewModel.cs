﻿using System.ComponentModel.DataAnnotations;

namespace LMS.Shared.Models
{
    public class AuthenticationViewModel
    {
        public string UserName { get; set; }
        [DataType(DataType.Password)] public string Password { get; set; }
        public string ResetCode { get; set; }
        public bool IsVerified { get; set; }
    }
}
