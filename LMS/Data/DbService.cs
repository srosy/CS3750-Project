using LMS.Shared.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using LMS.Data.Models;
using Blazored.LocalStorage;
using LMS.Data.Helper;
using System.Collections.Generic;
using MimeKit;
using Microsoft.EntityFrameworkCore;

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
        public Task<List<Enrollment>> GetProfessorCourseEnrollments(AzureDbContext db, int acctId);
        public Task<bool> UpdateEnrollments(AzureDbContext db, int acctId, List<Enrollment> enrollments);
        public Task<Settings> GetSettings(AzureDbContext db, int acctId);
        public Task<List<Payment>> GetPayments(AzureDbContext db, int acctId);
        public Task<bool> UpdateAccount(AzureDbContext db, Account acct);
        public Task<bool> SaveSettings(AzureDbContext db, Settings settings);
        public Task<bool> DoSending(MimeMessage mailMessage);
        public Task<bool> SendEmail(string email, AzureDbContext db);
        public Task VerifyEmail(string email, AzureDbContext db);
        public Task<bool> CheckAccountVerification(AzureDbContext azureDb, string email);
        public Task<string> GetVerificationCode(AzureDbContext db, string email);
        public Task<bool> UpdateEnrollmentsOnDeletedCourse(AzureDbContext db, Course model);
        public Task<bool> AddPayment(AzureDbContext db, Payment payment);
        public Task<bool> SaveAssignment(AzureDbContext db, Assignment assignment);
        public List<Assignment> GetAssignments(AzureDbContext db, List<Course> courses);
        public Task<Assignment> GetAssignment(AzureDbContext db, int assId);
        public Task<bool> SaveSubmission(AzureDbContext db, Submission submission);
        public Task<List<Submission>> GetSubmissions(AzureDbContext db, int acctId);
        public Task<bool> UpdateSubmissionsOnDeletedAssignment(AzureDbContext db, Assignment model);
        public Task<bool> UpdateSubmissionsOnDeletedCourse(AzureDbContext db, Course model);
        public Task<List<AnnouncementViewModel>> GetAnnouncements(AzureDbContext db, int acctId, bool isProfessor = false);
        public Task<bool> SaveAnnouncement(AzureDbContext db, AnnouncementViewModel model);
        public Task<List<AppointmentData>> GetAppointments(AzureDbContext db, int acctId);
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
                var acct = await db.Accounts.FirstOrDefaultAsync(a => a.Email.ToLower().Equals(model.UserName.ToLower()));
                if (acct == null) return false;

                var auth = await db.Authentications.FirstOrDefaultAsync(a => a.AccountId == acct.AccountId);
                if (auth == null) return false;

                if (!Encryption.GenerateSaltedHash(model.Password, auth.Salt).Equals(auth.Password)) return false; // case-sensitive

                await DeleteSession(db, storage);
                await SessionObj.CreateSession(db, storage, acct.AccountId);

                if (auth.EmailVerified)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("Account has not been validated.");
                    return false;
                }

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
                var acctExists = await db.Accounts.AnyAsync(a => a.Email.ToLower().Equals(model.Email.ToLower()));
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

        /// <summary>
        /// Sends an email containing the verification code.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<bool> SendEmail(string email, AzureDbContext db)
        {
            var acct = await db.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            var matchingAuth = await db.Authentications.FirstOrDefaultAsync(a => a.AccountId == acct.AccountId);

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

        /// <summary>
        /// Sends an email.
        /// </summary>
        /// <param name="mailMessage"></param>
        /// <returns></returns>
        public async Task<bool> DoSending(MimeMessage mailMessage)
        {
            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
            await client.AuthenticateAsync("CS3750LMS@gmail.com", "GaviSpe64!");
            await client.SendAsync(mailMessage);
            await client.DisconnectAsync(true);
            return true;
        }

        /// <summary>
        /// Verifies the Email of an account in the Db.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task VerifyEmail(string email, AzureDbContext db)
        {
            var acct = await db.Accounts.FirstOrDefaultAsync(a => a.Email == email);
            var matchingAuth = await db.Authentications.FirstOrDefaultAsync(a => a.AccountId == acct.AccountId);

            matchingAuth.EmailVerified = true;
            await db.SaveChangesAsync();
        }

        /// <summary>
        /// Checks the verification status of an account from the provided email.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<bool> CheckAccountVerification(AzureDbContext db, string email)
        {
            var verified = false;
            try
            {
                var acct = await db.Accounts.FirstOrDefaultAsync(a => a.Email.ToLower().Equals(email.ToLower()));
                if (acct == null) return verified;

                var auth = await db.Authentications.FirstOrDefaultAsync(a => a.AccountId == acct.AccountId);
                if (auth == null) return verified;

                verified = auth.EmailVerified;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return verified;
        }

        /// <summary>
        /// Returns the verification code tied to the email account.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<string> GetVerificationCode(AzureDbContext db, string email)
        {
            var code = string.Empty;
            try
            {
                var acct = await db.Accounts.FirstOrDefaultAsync(a => a.Email.ToLower().Equals(email.ToLower()));
                if (acct == null) return code;

                var auth = await db.Authentications.FirstOrDefaultAsync(a => a.AccountId == acct.AccountId);
                if (auth == null) return code;

                code = auth.ResetCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return code;
        }

        /// <summary>
        /// Deletes a user's session.
        /// </summary>
        /// <param name="storage"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public async Task<bool> DeleteSession(AzureDbContext db, ILocalStorageService storage)
        {
            var deleted = await SessionObj.DeleteSession(db, storage); // delete AnyAsync existing session
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
            return await courses.ToListAsync();
        }

        /// <summary>
        /// Deletes AnyAsync Enrollments that are affiliated with the course to be deleted.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateEnrollmentsOnDeletedCourse(AzureDbContext db, Course model)
        {
            var saved = false;
            try
            {
                // update AnyAsync enrollments that may have been active first because of foreign key
                if (model.DeleteDate != null)
                {
                    var enrollments = await db.Enrollments.Where(e => e.CourseId == model.CourseId && e.DeleteDate == null).ToListAsync();
                    if (enrollments != null && enrollments.Count > 0)
                    {
                        for (int i = 0; i < enrollments.Count; i++)
                        {
                            enrollments[i].DeleteDate = DateTime.UtcNow;
                        }
                        saved = await db.SaveChangesAsync() > 0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{ex.Message}\n{ex.InnerException.Message}");
            }

            return saved;
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
                    var course = await db.Courses.FirstOrDefaultAsync(c => c.CourseId == model.CourseId);
                    if (course == null) return false;

                    course.StartDate = model.StartDate;
                    course.EndDate = model.EndDate;
                    course.UpdateDate = DateTime.UtcNow;
                    course.ProfessorId = model.ProfessorId;
                    course.Name = model.Name;
                    course.Description = model.Description;
                    course.EndTime = model.EndTime;
                    course.StartTime = model.StartTime;
                    course.Credits = model.Credits;
                    course.Markup = model.Markup;

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
        public async Task<List<Account>> GetAccounts(AzureDbContext db, Enum.Role role = Enum.Role.STUDENT) => await db.Accounts.Where(a => a.DeleteDate == null && a.Role == (int)role).ToListAsync();

        /// <summary>
        /// Gets an Account by Id.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public async Task<Account> GetAccount(AzureDbContext db, int acctId) => await db.Accounts.FirstOrDefaultAsync(a => a.AccountId == acctId);

        /// <summary>
        /// Gets the Enrollments an account has.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public async Task<List<Enrollment>> GetEnrollments(AzureDbContext db, int acctId) => await db.Enrollments.Where(e => e.AccountId == acctId && e.DeleteDate == null).ToListAsync();

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
                for (int i = 0; i < enrollmentsForAccount.Count; i++)
                {
                    var e = enrollmentsForAccount[i];
                    // set previously selected to deleted
                    if (enrollments.Any(a => a.EnrollmentId == e.EnrollmentId && a.DeleteDate != null))
                    {
                        e.DeleteDate = DateTime.UtcNow;
                    }
                }

                for (int i = 0; i < enrollments.Count; i++)
                {
                    var e = enrollments[i];
                    if (e.DeleteDate == null && e.EnrollmentId == 0)
                    {
                        db.Enrollments.Add(e);
                    }
                }

                saved = await db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return saved;
        }

        /// <summary>
        /// Gets the Account Settings by AccountId.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public async Task<Settings> GetSettings(AzureDbContext db, int acctId) => await db.Settings.FirstOrDefaultAsync(s => s.AccountId == acctId);

        /// <summary>
        /// Saves the passed Account to the DB.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acct"></param>
        /// <returns></returns>
        public async Task<bool> UpdateAccount(AzureDbContext db, Account acct)
        {
            var saved = false;
            try
            {
                var dbAcct = db.Accounts.First(a => a.AccountId == acct.AccountId);
                dbAcct.UpdateDate = DateTime.UtcNow;
                dbAcct.FirstName = acct.FirstName;
                dbAcct.LastName = acct.LastName;
                dbAcct.Role = acct.Role;
                dbAcct.DOB = acct.DOB;
                dbAcct.DeleteDate = acct.DeleteDate;

                saved = await db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return saved;
        }

        /// <summary>
        /// Saves the passed Settings to the Db.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public async Task<bool> SaveSettings(AzureDbContext db, Settings settings)
        {
            var saved = false;
            try
            {
                // new settings
                if (settings.SettingId == 0)
                {
                    db.Settings.Add(settings);
                    saved = await db.SaveChangesAsync() > 0;
                }
                else
                {
                    var dbSettings = db.Settings.First(s => s.AccountId == settings.AccountId && settings.SettingId == s.SettingId);
                    dbSettings.UpdateDate = DateTime.UtcNow;
                    dbSettings.Address = settings.Address;
                    dbSettings.City = settings.City;
                    dbSettings.State = settings.State;
                    dbSettings.ZipCode = settings.ZipCode;
                    dbSettings.Country = settings.Country;
                    dbSettings.Phone = settings.Phone;
                    dbSettings.SocialMediaLink1 = settings.SocialMediaLink1;
                    dbSettings.SocialMediaLink2 = settings.SocialMediaLink2;
                    dbSettings.SocialMediaLink3 = settings.SocialMediaLink3;
                    dbSettings.Biography = settings.Biography;
                    dbSettings.ProfileImageUrl = settings.ProfileImageUrl;

                    saved = await db.SaveChangesAsync() > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return saved;
        }

        /// <summary>
        /// Gets all the Enrollments for all the Professor's Courses.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public async Task<List<Enrollment>> GetProfessorCourseEnrollments(AzureDbContext db, int acctId)
        {
            var enrollments = new List<Enrollment>();
            try
            {
                var courses = await GetCourses(db, acctId);
                if (courses == null) return enrollments;
                var dbEnrollments = db.Enrollments.Where(e => e.DeleteDate == null).ToArray();
                for (int i = 0; i < dbEnrollments.Length; i++)
                {
                    var e = dbEnrollments[i];
                    if (courses.Any(c => c.CourseId == e.CourseId))
                    {
                        enrollments.Add(e);
                    }
                }
                return enrollments.OrderBy(e => e.CourseId).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return enrollments;
            }
        }

        /// <summary>
        /// Gets the Account Payments by AccountId.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public async Task<List<Payment>> GetPayments(AzureDbContext db, int acctId) => await db.Payments.Where(p => p.AccountId == acctId).ToListAsync();

        /// <summary>
        /// Adds a new Payment to the db.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="payment"></param>
        /// <returns></returns>
        public async Task<bool> AddPayment(AzureDbContext db, Payment payment)
        {
            var saved = false;
            try
            {
                var charge = await new StripeAPI().ChargeCard(payment);
                payment.PaymentAmount = (charge.amount_captured / 100);
                payment.AuthAmount = (charge.amount / 100);
                payment.TransactionDate = charge.paid == true ? DateTime.UtcNow : (DateTime?)null;
                payment.TransactionId = charge.id;
                payment.CardNumber = string.Join("", payment.CardNumber.Take(4)) + "********" + payment.CardNumber[12..];

                db.Payments.Add(payment);
                saved = await db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return saved;
        }

        /// <summary>
        /// Saves an existing or creates a new Assignment in the Db.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="assignment"></param>
        /// <returns></returns>
        public async Task<bool> SaveAssignment(AzureDbContext db, Assignment assignment)
        {
            var saved = false;
            try
            {
                if (assignment.AssignmentId > 0)
                {
                    var ass = db.Assignments.First(a => a.AssignmentId == assignment.AssignmentId);
                    ass.UpdateDate = DateTime.UtcNow;
                    ass.DueDate = assignment.DueDate;
                    ass.DeleteDate = assignment.DeleteDate;
                    ass.MaxScore = assignment.MaxScore;
                    ass.Name = assignment.Name;
                    ass.Type = assignment.Type;
                    ass.Description = assignment.Description;
                    ass.SubmissionType = assignment.SubmissionType;
                }
                else
                {
                    db.Assignments.Add(assignment);
                }

                saved = await db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return saved;
        }

        /// <summary>
        /// Gets Assignments for the passed courses.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="courses"></param>
        /// <returns></returns>
        public List<Assignment> GetAssignments(AzureDbContext db, List<Course> courses)
        {
            var assignments = new List<Assignment>();
            var dbAssignments = db.Assignments.Where(a => a.DeleteDate == null).ToArray();
            for (int i = 0; i < dbAssignments.Length; i++)
            {
                var ass = dbAssignments[i];
                if (courses.Any(c => c.CourseId == ass.CourseId))
                {
                    assignments.Add(ass);
                }
            }
            return assignments;
        }

        /// <summary>
        /// Saves an existing or creates a new Assignment Submission in the Db.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="submission"></param>
        /// <returns></returns>
        public async Task<bool> SaveSubmission(AzureDbContext db, Submission submission)
        {
            var saved = false;
            try
            {
                if (submission.SubmissionId > 0)
                {
                    var sub = db.Submissions.First(a => a.SubmissionId == submission.SubmissionId);
                    sub.UpdateDate = DateTime.UtcNow;
                    sub.DeleteDate = submission.DeleteDate;
                    sub.Comments = submission.Comments;
                    sub.Score = submission.Score;
                    sub.UploadFileName = submission.UploadFileName;
                    sub.UploadFilePath = submission.UploadFilePath;
                    sub.AccountId = submission.AccountId;
                    sub.TextResponse = submission.TextResponse;
                }
                else
                {
                    db.Submissions.Add(submission);
                }

                saved = await db.SaveChangesAsync() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return saved;
        }

        /// <summary>
        /// Gets Submissions for the passed AccountId.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public async Task<List<Submission>> GetSubmissions(AzureDbContext db, int acctId) => await db.Submissions.Where(s => s.AccountId == acctId && s.DeleteDate == null).ToListAsync();

        /// <summary>
        /// Updates records in the Db if an assignment with submissons, TODO: grades, etc is deleted. 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSubmissionsOnDeletedAssignment(AzureDbContext db, Assignment model)
        {
            var updated = false;
            try
            {
                var ass = db.Assignments.First(a => a.AssignmentId == model.AssignmentId);
                ass.DeleteDate = DateTime.UtcNow;
                var assUpdated = await SaveAssignment(db, ass);

                // TODO: update the submissions related to the assignment
                var submissionsUpdated = true;
                var submissions = await db.Submissions.Where(s => s.AssignmentId == ass.AssignmentId && s.DeleteDate == null).ToListAsync();
                if (submissions.Any())
                {
                    for (int i = 0; i < submissions.Count; i++)
                    {
                        submissions[i].DeleteDate = DateTime.UtcNow;
                    }
                    submissionsUpdated = await db.SaveChangesAsync() < 0;
                }

                // TODO: Update the grades
                var gradesUpdated = true; // for now

                updated = assUpdated && submissionsUpdated && gradesUpdated;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return updated;
        }

        /// <summary>
        /// Updates records in the Db if a course that has assignments with submissons, TODO: grades, etc is deleted. 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> UpdateSubmissionsOnDeletedCourse(AzureDbContext db, Course model)
        {
            var updated = false;
            if (model.DeleteDate == null) return updated;

            var course = await db.Courses.FirstAsync(c => c.CourseId == model.CourseId && c.DeleteDate == null);
            if (course == null) return updated;

            var assignments = GetAssignments(db, new List<Course>() { model });

            if (assignments != null && assignments.Any())
            {
                var allUpdated = new bool[assignments.Count];
                var index = 0;

                for (int i = 0; i < assignments.Count; i++)
                {
                    allUpdated[index] = await UpdateSubmissionsOnDeletedAssignment(db, assignments[i]);
                    index++;
                }
                updated = allUpdated.All(x => x == true);
            }

            return updated;
        }

        /// <summary>
        /// Return the Assignment with the matching AssignmentId.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="assId"></param>
        /// <returns></returns>
        public async Task<Assignment> GetAssignment(AzureDbContext db, int assId) => await db.Assignments.FirstOrDefaultAsync(a => a.AssignmentId == assId);

        /// <summary>
        /// Gets Announcements for Students and Professors.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <param name="isProfessor"></param>
        /// <returns></returns>
        public async Task<List<AnnouncementViewModel>> GetAnnouncements(AzureDbContext db, int acctId, bool isProfessor = false)
        {
            var announcements = new List<AnnouncementViewModel>();
            var acct = await db.Accounts.FirstOrDefaultAsync(a => a.AccountId == acctId && a.DeleteDate == null);
            if (acct == null) return announcements;

            try
            {
                if (isProfessor)
                {
                    announcements = await (
                        from Courses in db.Courses
                        join Notifications in db.Notifications
                        on Courses.CourseId equals Notifications.CourseId
                        where Courses.ProfessorId == acctId && Courses.DeleteDate == null
                        select new AnnouncementViewModel()
                        {
                            CourseId = Courses.CourseId,
                            CourseName = Courses.Name,
                            ProfessorAccountId = acctId,
                            ProfessorName = $"{acct.FirstName} {acct.LastName}",
                            AnnouncementDate = Notifications.CreateDate,
                            Title = Notifications.Title,
                            Message = Notifications.Message,
                            Type = Notifications.Type
                        }).OrderBy(a => a.AnnouncementDate).ToListAsync();
                }
                else
                {
                    var enrollments = await GetEnrollments(db, acctId);
                    var courseIds = enrollments.Select(e => e.CourseId);
                    if (enrollments == null || !enrollments.Any()) return announcements;

                    announcements = await (
                        from Courses in db.Courses
                        join Notifications in db.Notifications
                        on Courses.CourseId equals Notifications.CourseId
                        where courseIds.Contains(Courses.CourseId) && Courses.DeleteDate == null
                        select new AnnouncementViewModel()
                        {
                            CourseId = Courses.CourseId,
                            CourseName = Courses.Name,
                            ProfessorAccountId = db.Accounts.First(a => a.AccountId == Courses.ProfessorId).AccountId,
                            ProfessorName = $"{db.Accounts.First(a => a.AccountId == Courses.ProfessorId).FirstName} " +
                            $"{db.Accounts.First(a => a.AccountId == Courses.ProfessorId).LastName}",
                            AnnouncementDate = Notifications.CreateDate,
                            Title = Notifications.Title,
                            Message = Notifications.Message
                        }).OrderBy(a => a.AnnouncementDate).ThenBy(a => a.CourseId)
                        .ToListAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return announcements;
        }

        /// <summary>
        /// Saves a new Notification from an Announcement.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<bool> SaveAnnouncement(AzureDbContext db, AnnouncementViewModel model)
        {
            bool saved;
            var notification = new Notification()
            {
                CourseId = model.CourseId,
                CreateDate = DateTime.UtcNow,
                Message = model.Message,
                Title = model.Title,
                Type = model.Type
            };

            if (model.Deleted)
            {
                notification.DeleteDate = DateTime.UtcNow;
            }

            db.Notifications.Add(notification);
            saved = await db.SaveChangesAsync() > 0;
            return saved;
        }

        /// <summary>
        /// Gets the appointments to populate the calendar in the Dashboard.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public async Task<List<AppointmentData>> GetAppointments(AzureDbContext db, int acctId)
        {
            var rand = new Random();
            var appointments = new List<AppointmentData>();
            for (int i = 0; i < 10; i++)
            {
                var st = DateTime.Today.AddDays(rand.Next(1, 24) * -1).AddHours(rand.Next(1, 12)).AddMinutes(rand.Next(1, 60));
                appointments.Add(new AppointmentData()
                {
                    Id = i,
                    Description = $"Test Appointment {i}",
                    StartTime = st,
                    EndTime = st.AddHours(2),
                    IsAllDay = rand.Next(1, 24) % 3 == 0,
                    Location = $"Location {rand.Next(1, 24)}",
                    Subject = $"Test Subject {i}"
                });
            }
            
            for (int i = 10; i < 20; i++)
            {
                var st = DateTime.Today.AddDays(rand.Next(1, 24)).AddHours(rand.Next(1, 12)).AddMinutes(rand.Next(1, 60));
                appointments.Add(new AppointmentData()
                {
                    Id = i,
                    Description = $"Test Appointment {i}",
                    StartTime = st,
                    EndTime = st.AddHours(2),
                    IsAllDay = rand.Next(1, 24) % 3 == 0,
                    Location = $"Location {rand.Next(1, 24)}",
                    Subject = $"Test Subject {i}"
                });
            }

            return appointments;
        }
    }
}

