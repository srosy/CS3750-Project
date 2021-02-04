using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorInputFile;
using LMS.Data.Models;

namespace LMS.Data.Helper
{
    public class SessionObj
    {
        public Guid SessionId { get; set; }
        public DateTime ExpireDate { get; set; }

        /// <summary>
        /// Checks if a session is still valid.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        public static async Task<bool> VerifySession(AzureDbContext db, ILocalStorageService storage)
        {
            var session = await storage.GetItemAsync<SessionObj>("session_lms");
            if (session == null) return false;
            if (session.ExpireDate == DateTime.MinValue) return false;
            return db.Sessions.Any(s => s.SessionId == session.SessionId && DateTime.UtcNow <= s.ExpireDate);
        }

        /// <summary>
        /// Gets the current session.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        public static async Task<Session> GetSession(AzureDbContext db, ILocalStorageService storage)
        {
            var session = await storage.GetItemAsync<SessionObj>("session_lms");
            if (session == null) return null;
            if (session.ExpireDate == DateTime.MinValue) return null;
            return db.Sessions.FirstOrDefault(s => s.SessionId == session.SessionId && DateTime.UtcNow <= s.ExpireDate);
        }

        /// <summary>
        /// Saves a new session in the db.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Save(AzureDbContext db, int acctId)
        {
            db.Sessions.Add(new Session()
            {
                SessionId = SessionId,
                ExpireDate = ExpireDate,
                AccountId = acctId
            });

            var saved = db.SaveChanges() > 0;
            return saved;
        }

        /// <summary>
        /// Deletes a session from the db.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<bool> Delete(AzureDbContext db)
        {
            var session = db.Sessions.FirstOrDefault(s => s.SessionId == SessionId);
            if (session != null)
            {
                db.Sessions.Remove(session);
                await db.SaveChangesAsync();
            }

            return true;
        }

        /// <summary>
        /// Static helper method to create a session in local storage and db.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        public static async Task<bool> CreateSession(AzureDbContext db, ILocalStorageService storage, int acctId)
        {
            var session = new SessionObj()
            {
                SessionId = Guid.NewGuid(),
                ExpireDate = DateTime.UtcNow.AddMinutes(30)
            };
            await storage.SetItemAsync("session_lms", session); // create the session
            return session.Save(db, acctId);
        }

        /// <summary>
        /// Static helper method to delete a session from local storage and db.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="storage"></param>
        /// <returns></returns>
        public static async Task<bool> DeleteSession(AzureDbContext db, ILocalStorageService storage)
        {
            var existing = await storage.ContainKeyAsync("session_lms");
            if (existing)
            {
                var sessionToDelete = await storage.GetItemAsync<SessionObj>("session_lms");
                await sessionToDelete.Delete(db);
                await storage.RemoveItemAsync("session_lms");
            }

            return true;
        }
    }

    public class Encryption
    {
        const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static readonly Random _rand = new Random();

        /// <summary>
        /// Returns a random string of random length: 5 <= length <= 15.
        /// </summary>
        /// <returns></returns>
        public static string GenSalt(int length = 15) => new string(Enumerable.Repeat(_chars, _rand.Next(5, length)).Select(s => s[_rand.Next(s.Length)]).ToArray());

        /// <summary>
        /// Generates a hashed+salted password.
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns></returns>
        public static string GenerateSaltedHash(string password, string salt)
        {
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var saltBytes = Encoding.UTF8.GetBytes(salt);

            HashAlgorithm hashAlg = new SHA256Managed();
            byte[] passwordPlusSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];

            for (int i = 0; i < passwordBytes.Length; i++)
                passwordPlusSaltBytes[i] = passwordBytes[i];
            for (int i = 0; i < saltBytes.Length; i++)
                passwordPlusSaltBytes[passwordBytes.Length + i] = saltBytes[i];

            var hashedAndSaltedPassword = Convert.ToBase64String(hashAlg.ComputeHash(passwordPlusSaltBytes));
            return hashedAndSaltedPassword;
        }
    }

    /// <summary>
    /// Class State sets and returns all the US States.
    /// </summary>
    public class State
    {
        public List<US_State> States { get; set; }
        public State()
        {
            States = new List<US_State>(50);
            States.Add(new US_State("AL", "Alabama"));
            States.Add(new US_State("AK", "Alaska"));
            States.Add(new US_State("AZ", "Arizona"));
            States.Add(new US_State("AR", "Arkansas"));
            States.Add(new US_State("CA", "California"));
            States.Add(new US_State("CO", "Colorado"));
            States.Add(new US_State("CT", "Connecticut"));
            States.Add(new US_State("DE", "Delaware"));
            States.Add(new US_State("DC", "District Of Columbia"));
            States.Add(new US_State("FL", "Florida"));
            States.Add(new US_State("GA", "Georgia"));
            States.Add(new US_State("HI", "Hawaii"));
            States.Add(new US_State("ID", "Idaho"));
            States.Add(new US_State("IL", "Illinois"));
            States.Add(new US_State("IN", "Indiana"));
            States.Add(new US_State("IA", "Iowa"));
            States.Add(new US_State("KS", "Kansas"));
            States.Add(new US_State("KY", "Kentucky"));
            States.Add(new US_State("LA", "Louisiana"));
            States.Add(new US_State("ME", "Maine"));
            States.Add(new US_State("MD", "Maryland"));
            States.Add(new US_State("MA", "Massachusetts"));
            States.Add(new US_State("MI", "Michigan"));
            States.Add(new US_State("MN", "Minnesota"));
            States.Add(new US_State("MS", "Mississippi"));
            States.Add(new US_State("MO", "Missouri"));
            States.Add(new US_State("MT", "Montana"));
            States.Add(new US_State("NE", "Nebraska"));
            States.Add(new US_State("NV", "Nevada"));
            States.Add(new US_State("NH", "New Hampshire"));
            States.Add(new US_State("NJ", "New Jersey"));
            States.Add(new US_State("NM", "New Mexico"));
            States.Add(new US_State("NY", "New York"));
            States.Add(new US_State("NC", "North Carolina"));
            States.Add(new US_State("ND", "North Dakota"));
            States.Add(new US_State("OH", "Ohio"));
            States.Add(new US_State("OK", "Oklahoma"));
            States.Add(new US_State("OR", "Oregon"));
            States.Add(new US_State("PA", "Pennsylvania"));
            States.Add(new US_State("RI", "Rhode Island"));
            States.Add(new US_State("SC", "South Carolina"));
            States.Add(new US_State("SD", "South Dakota"));
            States.Add(new US_State("TN", "Tennessee"));
            States.Add(new US_State("TX", "Texas"));
            States.Add(new US_State("UT", "Utah"));
            States.Add(new US_State("VT", "Vermont"));
            States.Add(new US_State("VA", "Virginia"));
            States.Add(new US_State("WA", "Washington"));
            States.Add(new US_State("WV", "West Virginia"));
            States.Add(new US_State("WI", "Wisconsin"));
            States.Add(new US_State("WY", "Wyoming"));
        }
        public class US_State
        {
            public US_State()
            {
                Name = null;
                Abbreviations = null;
            }
            public US_State(string ab, string name)
            {
                Name = name;
                Abbreviations = ab;
            }
            public string Name { get; set; }
            public string Abbreviations { get; set; }
            public override string ToString()
            {
                return string.Format("{0} - {1}", Abbreviations, Name);
            }
        }
    }

    public static class LMS_Image
    {
        public static async Task<Image> ConvertImageFromByteArray(byte[] imgBytes)
        {
            using (var ms = new MemoryStream(imgBytes))
            {
                return Image.FromStream(ms);
            }
        }

        public static async Task<byte[]> ConvertImageUploadToByteArray(IFileListEntry file)
        {
            if (file == null) return null;

            var ms = new MemoryStream();
            await file.Data.CopyToAsync(ms);
            var imgBytes = ms.ToArray();

            return imgBytes;
        }

        /// <summary>
        /// Converts the Image Byte[] into a source an image html element can use.
        /// </summary>
        /// <param name="imgBytes"></param>
        /// <returns></returns>
        public static async Task<string> GetImageSrc(byte[] imgBytes) => "data:image/png;base64," + Convert.ToBase64String(imgBytes, Base64FormattingOptions.None);

    }
}
