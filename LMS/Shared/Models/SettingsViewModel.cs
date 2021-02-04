using LMS.Data.Models;


namespace LMS.Shared.Models
{
    public class SettingsViewModel
    {
        public Account Account { get; set; }
        public Settings Settings { get; set; }
        public Data.Helper.State State = new Data.Helper.State();
        public byte[] ImageUpload { get; set; }
    }
}
