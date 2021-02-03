using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
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
        /// Saves a new session in the db.
        /// </summary>
        /// <param name="db"></param>
        /// <returns></returns>
        public bool Save(AzureDbContext db)
        {
            db.Sessions.Add(new Session()
            {
                SessionId = SessionId,
                ExpireDate = ExpireDate
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
        public static async Task<bool> CreateSession(AzureDbContext db, ILocalStorageService storage)
        {
            var session = new SessionObj()
            {
                SessionId = Guid.NewGuid(),
                ExpireDate = DateTime.UtcNow.AddMinutes(30)
            };
            await storage.SetItemAsync("session_lms", session); // create the session
            return session.Save(db);
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
        public static string GenSalt() => new string(Enumerable.Repeat(_chars, _rand.Next(5, 15)).Select(s => s[_rand.Next(s.Length)]).ToArray());

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
}
