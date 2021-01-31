using LMS.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using LMS.Data.Models;
using Blazored.LocalStorage;
using System.Dynamic;
using LMS.Data.Helper;
using MimeKit;

namespace LMS.Data
{
    public interface IDbService
    {
        public Task<bool> Authenticate(ILocalStorageService storage, AzureDbContext db, AuthenticationViewModel model);
        public Task<bool> CreateAccount(AzureDbContext db, AccountViewModel model);
        public Task<bool> DeleteSession(AzureDbContext db, ILocalStorageService storage);
    }
    public class DbService : IDbService
    {
        /// <summary>
        /// Authenticates a user and creates a new session.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="db"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> Authenticate(ILocalStorageService storage, AzureDbContext db, AuthenticationViewModel model)
        {
            try
            {
                var acct = db.Accounts.FirstOrDefault(a => a.Email.ToLower().Equals(model.UserName.ToLower()));
                if (acct == null) return false;

                var auth = db.Authentications.FirstOrDefault(a => a.AccountId == acct.AccountId);
                if (auth == null) return false;

                if (!Encryption.GenerateSaltedHash(model.Password, auth.Salt).Equals(auth.Password)) return false; // case-sensitive

                await DeleteSession(db, storage);
                await SessionObj.CreateSession(db, storage);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// Creates a new account.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> CreateAccount(AzureDbContext db, AccountViewModel model)
        {
            try
            {
                var acctExists = db.Accounts.Any(a => a.Email.ToLower().Equals(model.Email.ToLower()));
                if (acctExists) return false;

                bool savedAcct = false, savedAuth = false;

                var accountToAdd = new Account()
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CreateDate = DateTime.UtcNow,
                    Role = model.Role,
                    DOB = model.Birthday
                };
                db.Accounts.Add(accountToAdd);
                savedAcct = db.SaveChanges() > 0;

                if (savedAcct)
                {
                    model.Auth.UserName = model.Email.ToLower();

                    var salt = Encryption.GenSalt();
                    var resetCode = Encryption.GenSalt(6);
                    db.Authentications.Add(new Authentication()
                    {
                        AccountId = accountToAdd.AccountId,
                        CreateDate = DateTime.UtcNow,
                        Salt = salt,
                        Password = Encryption.GenerateSaltedHash(model.Auth.Password, salt),
                        ResetCode = resetCode
                    });
                    savedAuth = db.SaveChanges() > 0;

                    model.Auth.ResetCode = resetCode;
                }

                return savedAcct && savedAuth;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.InnerException.Message}");
                return false;
            }
        }

        public async Task<bool> SendEmail(AccountViewModel account, AzureDbContext db)
        {
            var acct = db.Accounts.FirstOrDefault(a => a.Email == account.Email);
            var matchingAuth = db.Authentications.FirstOrDefault(a => a.AccountId == acct.AccountId);

            var email = account.Email;
            var code = matchingAuth.ResetCode;

            try
            {
                var mailMessage = new MimeMessage();
                mailMessage.From.Add(new MailboxAddress("Team Git'r Dun", "LMS.GitrDun@gmail.com"));
                mailMessage.To.Add(new MailboxAddress("User", email));

                var textpart = new TextPart("plain");

                if (!string.IsNullOrEmpty(code))
                {
                    mailMessage.Subject = "LMS Verification Code";
                    textpart.Text = $"Your LMS verification code is: \n\n" + code;
                }
                mailMessage.Body = textpart;
                var success = await DoSending(mailMessage);
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> DoSending(MimeMessage mailMessage)
        {
            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
                await client.AuthenticateAsync("CS3750LMS@gmail.com", "GaviSpe64!");
                await client.SendAsync(mailMessage);
                await client.DisconnectAsync(true);
                return true;
            }
        }

        public async Task VerifyEmail(string email, AzureDbContext db)
        {
            var acct = db.Accounts.FirstOrDefault(a => a.Email == email);
            var matchingAuth = db.Authentications.FirstOrDefault(a => a.AccountId == acct.AccountId);

            matchingAuth.EmailVerified = true;
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes a user's session.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSession(AzureDbContext db, ILocalStorageService storage)
        {
            var deleted = await SessionObj.DeleteSession(db, storage); // delete any existing session
            return deleted;
        }
    }
}

