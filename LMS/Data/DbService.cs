using LMS.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using LMS.Data.Models;


namespace LMS.Data
{
    public interface IDbService
    {
        public Task<bool> Authenticate(AzureDbContext db, AuthModel model);
        public Task<bool> CreateAccount(AzureDbContext db, AccountModel model);
    }
    public class DbService : IDbService
    {
        public async Task<bool> Authenticate(AzureDbContext db, AuthModel model)
        {
            //determine if login is a success
            try
            {
                var acct = db.Accounts.FirstOrDefault(a => a.Email.ToLower().Equals(model.UserName.ToLower()));
                if (acct == null) return false;

                var auth = db.Authentications.FirstOrDefault(a => a.AccountId == acct.AccountId);
                if (auth == null) return false;

                if (!model.Password.Equals(auth.Password)) return false; // case-sensitive

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        public async Task<bool> CreateAccount(AzureDbContext db, AccountModel model)
        {
            //determine if login is a success
            try
            {
                var acctExists = db.Accounts.Any(a => a.Email.ToLower().Equals(model.Email.ToLower()));
                if (acctExists) return false;

                bool savedAcct = false, savedAuth = false;

                //create account 
                var accountToAdd = new Account()
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };
                db.Accounts.Add(accountToAdd);
                savedAcct = db.SaveChanges() > 0;

                if (savedAcct)
                {
                    model.Auth.UserName = model.Email.ToLower();
                    db.Authentications.Add(new Authentication()
                    {
                        AccountId = accountToAdd.AccountId,
                        Password = model.Auth.Password
                    });
                    savedAuth = db.SaveChanges() > 0;
                }

                return savedAcct && savedAuth;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.InnerException.Message}");
                return false;
            }
        }
    }
}

