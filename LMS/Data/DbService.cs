﻿using LMS.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using LMS.Data.Models;
using Blazored.LocalStorage;
using LMS.Data.Helper;
using System.Collections.Generic;

namespace LMS.Data
{
    public interface IDbService
    {
        public Task<bool> Authenticate(ILocalStorageService storage, AzureDbContext db, AuthenticationViewModel model);
        public Task<bool> CreateAccount(AzureDbContext db, AccountViewModel model);
        public Task<bool> SaveCourse(AzureDbContext db, Course model);
        public Task<bool> DeleteSession(AzureDbContext db, ILocalStorageService storage);
        public Task<List<Course>> GetCourses(AzureDbContext db, int professorId = 0);
        public Task<List<Account>> GetAccounts(AzureDbContext db, Enum.Role role = Enum.Role.STUDENT);
        public Task<Account> GetAccount(AzureDbContext db, int acctId);
        public Task<List<Enrollment>> GetEnrollments(AzureDbContext db, int acctId);
        public Task<bool> UpdateEnrollments(AzureDbContext db, int acctId, List<Enrollment> enrollments);
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
                await SessionObj.CreateSession(db, storage, acct.AccountId);

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
                    db.Authentications.Add(new Authentication()
                    {
                        AccountId = accountToAdd.AccountId,
                        CreateDate = DateTime.UtcNow,
                        Salt = salt,
                        Password = Encryption.GenerateSaltedHash(model.Auth.Password, salt)
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

        /// <summary>
        /// Gets a list of all courses, or filters courses by provided professorId.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="professorId"></param>
        /// <returns></returns>
        public async Task<List<Course>> GetCourses(AzureDbContext db, int professorId = 0)
        {
            var courses = db.Courses.Where(c => c.DeleteDate == null);
            if (professorId > 0)
            {
                courses = courses.Where(c => c.ProfessorId == professorId);
            }
            return courses.ToList();
        }

        /// <summary>
        /// Creates a new Course or updates an existing.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> SaveCourse(AzureDbContext db, Course model)
        {
            var saved = false;

            try
            {
                if (model.CourseId <= 0) // new course
                {
                    model.CreateDate = DateTime.UtcNow;
                    db.Courses.Add(model);

                    saved = await db.SaveChangesAsync() > 0;
                }
                else // save existing
                {
                    var course = db.Courses.FirstOrDefault(c => c.CourseId == model.CourseId);
                    if (course == null) return false;

                    course.StartDate = model.StartDate;
                    course.EndDate = model.EndDate;
                    course.UpdateDate = DateTime.UtcNow;
                    course.ProfessorId = model.ProfessorId;
                    course.Name = model.Name;
                    course.Description = model.Description;

                    saved = await db.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.InnerException.Message}");
            }

            return saved;
        }

        /// <summary>
        /// Gets all accounts; can filter by Role.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<List<Account>> GetAccounts(AzureDbContext db, Enum.Role role = Enum.Role.STUDENT) => db.Accounts.Where(a => a.DeleteDate == null && a.Role == (int)role).ToList();

        /// <summary>
        /// Gets an Account by Id.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public async Task<Account> GetAccount(AzureDbContext db, int acctId) => db.Accounts.FirstOrDefault(a => a.AccountId == acctId);

        /// <summary>
        /// Gets the Enrollments an account has.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public async Task<List<Enrollment>> GetEnrollments(AzureDbContext db, int acctId) => db.Enrollments.Where(e => e.AccountId == acctId && e.DeleteDate == null).ToList();

        /// <summary>
        /// Updates the Enrollments affiliated with an acctId.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <param name="enrollments"></param>
        /// <returns></returns>
        public async Task<bool> UpdateEnrollments(AzureDbContext db, int acctId, List<Enrollment> enrollments)
        {
            var saved = false;
            try
            {
                var enrollmentsForAccount = db.Enrollments.Where(e => e.AccountId == acctId).ToList();
                enrollmentsForAccount.ForEach(e =>
                {
                    if (enrollments.Any(a => a.EnrollmentId == e.EnrollmentId && a.DeleteDate != null))
                    {
                        e.DeleteDate = DateTime.UtcNow; // set previously selected to deleted
                    }
                });

                enrollments.Where(e => e.EnrollmentId == 0).ToList().ForEach(e =>
                {
                    db.Enrollments.Add(e);
                });

                saved = await db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return saved;
        }
    }
}

