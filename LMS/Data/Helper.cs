using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Blazored.LocalStorage;
using BlazorInputFile;
using LMS.Data.Enum;
using LMS.Data.Models;
using Microsoft.EntityFrameworkCore;

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
            return await db.Sessions.FirstOrDefaultAsync(s => s.SessionId == session.SessionId && DateTime.UtcNow <= s.ExpireDate);
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
            States = new List<US_State>(50)
            {
                new US_State("AL", "Alabama"),
                new US_State("AK", "Alaska"),
                new US_State("AZ", "Arizona"),
                new US_State("AR", "Arkansas"),
                new US_State("CA", "California"),
                new US_State("CO", "Colorado"),
                new US_State("CT", "Connecticut"),
                new US_State("DE", "Delaware"),
                new US_State("DC", "District Of Columbia"),
                new US_State("FL", "Florida"),
                new US_State("GA", "Georgia"),
                new US_State("HI", "Hawaii"),
                new US_State("ID", "Idaho"),
                new US_State("IL", "Illinois"),
                new US_State("IN", "Indiana"),
                new US_State("IA", "Iowa"),
                new US_State("KS", "Kansas"),
                new US_State("KY", "Kentucky"),
                new US_State("LA", "Louisiana"),
                new US_State("ME", "Maine"),
                new US_State("MD", "Maryland"),
                new US_State("MA", "Massachusetts"),
                new US_State("MI", "Michigan"),
                new US_State("MN", "Minnesota"),
                new US_State("MS", "Mississippi"),
                new US_State("MO", "Missouri"),
                new US_State("MT", "Montana"),
                new US_State("NE", "Nebraska"),
                new US_State("NV", "Nevada"),
                new US_State("NH", "New Hampshire"),
                new US_State("NJ", "New Jersey"),
                new US_State("NM", "New Mexico"),
                new US_State("NY", "New York"),
                new US_State("NC", "North Carolina"),
                new US_State("ND", "North Dakota"),
                new US_State("OH", "Ohio"),
                new US_State("OK", "Oklahoma"),
                new US_State("OR", "Oregon"),
                new US_State("PA", "Pennsylvania"),
                new US_State("RI", "Rhode Island"),
                new US_State("SC", "South Carolina"),
                new US_State("SD", "South Dakota"),
                new US_State("TN", "Tennessee"),
                new US_State("TX", "Texas"),
                new US_State("UT", "Utah"),
                new US_State("VT", "Vermont"),
                new US_State("VA", "Virginia"),
                new US_State("WA", "Washington"),
                new US_State("WV", "West Virginia"),
                new US_State("WI", "Wisconsin"),
                new US_State("WY", "Wyoming")
            };
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
        public static Image ConvertImageFromByteArray(byte[] imgBytes)
        {
            using var ms = new MemoryStream(imgBytes);
            return Image.FromStream(ms);
        }

        public static async Task<byte[]> ConvertFileToByteArray(IFileListEntry file)
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
        public static string GetImageSrc(byte[] imgBytes) => "data:image/png;base64," + Convert.ToBase64String(imgBytes, Base64FormattingOptions.None);
    }

    public class CustomValidationAttribute : ValidationAttribute
    {
        /// <summary>
        /// Tests the the value (datetime) is a future date.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool IsValid(object value) => Convert.ToDateTime(value) >= DateTime.Now.AddDays(1); // test cc requires a future exp date
    }

    /// <summary>
    /// Class containing methods to retrieve specific file system paths.
    /// </summary>
    public static class KnownFolders
    {
        //https://stackoverflow.com/questions/10667012/getting-downloads-folder-in-c
        private static readonly string[] _knownFolderGuids = new string[]
        {
        "{56784854-C6CB-462B-8169-88E350ACB882}", // Contacts
        "{B4BFCC3A-DB2C-424C-B029-7FE99A87C641}", // Desktop
        "{FDD39AD0-238F-46AF-ADB4-6C85480369C7}", // Documents
        "{374DE290-123F-4565-9164-39C4925E467B}", // Downloads
        "{1777F761-68AD-4D8A-87BD-30B759FA33DD}", // Favorites
        "{BFB9D5E0-C6A9-404C-B2B2-AE6DB6AF4968}", // Links
        "{4BD8D571-6D19-48D3-BE97-422220080E43}", // Music
        "{33E28130-4E1E-4676-835A-98395C3BC3BB}", // Pictures
        "{4C5C32FF-BB9D-43B0-B5B4-2D72E54EAAA4}", // SavedGames
        "{7D1D3A04-DEBB-4115-95CF-2F29DA2920DA}", // SavedSearches
        "{18989B1D-99B5-455B-841C-AB7C74E4DDFC}", // Videos
        };

        /// <summary>
        /// Gets the current path to the specified known folder as currently configured. This does
        /// not require the folder to be existent.
        /// </summary>
        /// <param name="knownFolder">The known folder which current path will be returned.</param>
        /// <returns>The default path of the known folder.</returns>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">Thrown if the path
        ///     could not be retrieved.</exception>
        public static string GetPath(KnownFolder knownFolder)
        {
            return GetPath(knownFolder, false);
        }

        /// <summary>
        /// Gets the current path to the specified known folder as currently configured. This does
        /// not require the folder to be existent.
        /// </summary>
        /// <param name="knownFolder">The known folder which current path will be returned.</param>
        /// <param name="defaultUser">Specifies if the paths of the default user (user profile
        ///     template) will be used. This requires administrative rights.</param>
        /// <returns>The default path of the known folder.</returns>
        /// <exception cref="System.Runtime.InteropServices.ExternalException">Thrown if the path
        ///     could not be retrieved.</exception>
        public static string GetPath(KnownFolder knownFolder, bool defaultUser)
        {
            return GetPath(knownFolder, KnownFolderFlags.DontVerify, defaultUser);
        }

        private static string GetPath(KnownFolder knownFolder, KnownFolderFlags flags,
            bool defaultUser)
        {
            int result = SHGetKnownFolderPath(new Guid(_knownFolderGuids[(int)knownFolder]),
                (uint)flags, new IntPtr(defaultUser ? -1 : 0), out IntPtr outPath);
            if (result >= 0)
            {
                string path = Marshal.PtrToStringUni(outPath);
                Marshal.FreeCoTaskMem(outPath);
                return path;
            }
            else
            {
                throw new ExternalException("Unable to retrieve the known folder path. It may not "
                    + "be available on this system.", result);
            }
        }

        [DllImport("Shell32.dll")]
        private static extern int SHGetKnownFolderPath(
            [MarshalAs(UnmanagedType.LPStruct)] Guid rfid, uint dwFlags, IntPtr hToken,
            out IntPtr ppszPath);

        [Flags]
        private enum KnownFolderFlags : uint
        {
            SimpleIDList = 0x00000100,
            NotParentRelative = 0x00000200,
            DefaultPath = 0x00000400,
            Init = 0x00000800,
            NoAlias = 0x00001000,
            DontUnexpand = 0x00002000,
            DontVerify = 0x00004000,
            Create = 0x00008000,
            NoAppcontainerRedirection = 0x00010000,
            AliasOnly = 0x80000000
        }
    }
}

