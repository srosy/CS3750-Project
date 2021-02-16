using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Data.Models
{
    public class Settings
    {
        [Key]
        public int SettingId { get; set; }
        public int AccountId { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        [DataType(DataType.PostalCode)] public string ZipCode { get; set; }
        public string Country { get; set; }
        [DataType(DataType.MultilineText)] public string Biography { get; set; }
        [DataType(DataType.PhoneNumber)] public string Phone { get; set; }
        [DataType(DataType.Url)] public string SocialMediaLink1 { get; set; }
        [DataType(DataType.Url)] public string SocialMediaLink2 { get; set; }
        [DataType(DataType.Url)] public string SocialMediaLink3 { get; set; }
        [DataType(DataType.Url)] public string ProfileImageUrl { get; set; }

        [DataType(DataType.DateTime)] public DateTime UpdateDate { get; set; }
    }
}
