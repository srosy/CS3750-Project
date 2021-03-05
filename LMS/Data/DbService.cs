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
using LMS.Data.Enum;

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
        public Task<List<AppointmentData>> GetAppointments(AzureDbContext db, ILocalStorageService storage);
        public Task<List<GradeViewModel>> GetGrades(AzureDbContext db, int acctId);
        public Task<bool> SaveGrades(AzureDbContext db, List<Submission> gradedSubmissions);
        public Task<BoxPlotChart> GetAssignmentStandingChart(AzureDbContext db, List<Assignment> asses, string chartName);
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
                var enrollments = await GetEnrollments(db, acct.AccountId);
                var courses = await GetCourses(db, acct.Role == (int)Role.PROFESSOR ? acct.AccountId : 0);

                // set local storage stuffz
                var tasks = new List<Task>()
                {
                    BrowserStorage<List<Enrollment>>.SaveObject(storage, "enrollments", enrollments),
                    BrowserStorage<List<Course>>.SaveObject(storage, "courses", courses),
                };
                await Task.WhenAll(tasks);
                var appointments = await GetAppointments(db, storage);
                await BrowserStorage<List<AppointmentData>>.SaveObject(storage, "appointments", appointments);

                if (acct.Role == (int)Role.STUDENT)
                {
                    var submissions = await GetSubmissions(db, acct.AccountId);
                    await BrowserStorage<List<Submission>>.SaveObject(storage, "submissions", submissions);
                }

                if (auth.EmailVerified)
                {
                    return true;
                }

                Console.WriteLine("Account has not been validated.");
                return false;

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
        /// Gets all student grades for all assignments from all courses currently enrolled.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <param name="isProfessor"></param>
        /// <returns></returns>
        public async Task<List<GradeViewModel>> GetGrades(AzureDbContext db, int acctId)
        {
            var grades = new List<GradeViewModel>();

            try
            {
                var enrollments = await GetEnrollments(db, acctId);
                var courses = enrollments.Select(e => new Course() { CourseId = e.CourseId }).ToList();
                var asses = GetAssignments(db, courses);

                for (int i = 0; i < courses.Count; i++)
                {
                    var gradeViewModel = new GradeViewModel()
                    {
                        CourseId = courses[i].CourseId,
                        Assignments = GetAssignments(db, new List<Course>() { new Course() { CourseId = courses[i].CourseId } }),
                        Submissions = await GetSubmissions(db, acctId),
                        Grades = new List<Grade>(),
                        OverallLetterGrade = "F-",
                        OverallPercentageGrade = 0
                    };

                    for (int j = 0; j < gradeViewModel.Assignments.Count; j++)
                    {
                        var grade = new Grade()
                        {
                            AssignmentId = gradeViewModel.Assignments[j].AssignmentId,
                            AssignmentName = gradeViewModel.Assignments[j].Name
                        };
                        var submission = gradeViewModel.Submissions.FirstOrDefault(s => s.AssignmentId == grade.AssignmentId);
                        if (submission == null)
                        {
                            grade.Score = 0;
                            grade.ScoreDisplay = "Not Yet Graded";
                            grade.LetterGrade = string.Empty;

                        }
                        else
                        {
                            grade.Score = Math.Round((submission.Score / (decimal)gradeViewModel.Assignments[i].MaxScore) * 100, 2);
                            grade.ScoreDisplay = $"{submission.Score}/{gradeViewModel.Assignments[j].MaxScore}" +
                                $" ({grade.Score})";
                            grade.LetterGrade = GradeHelper.GenGradeFromPercentage(grade.Score);
                        }

                        gradeViewModel.Grades.Add(grade);
                    }

                    var gradedGrades = gradeViewModel.Grades.Where(g => g.Score > 0);
                    var sum = gradedGrades.Sum(g => g.Score);


                    gradeViewModel.OverallPercentageGrade = gradedGrades.Count() > 0 ? sum / gradedGrades.Count() : 0.00m;
                    gradeViewModel.OverallLetterGrade = gradeViewModel.OverallPercentageGrade > 0 ?
                        GradeHelper.GenGradeFromPercentage(gradeViewModel.OverallPercentageGrade) : "N/A";

                    grades.Add(gradeViewModel);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return grades;
        }

        /// <summary>
        /// Updates the graded Submissions passed.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="gradedSubmissions"></param>
        /// <returns></returns>
        public async Task<bool> SaveGrades(AzureDbContext db, List<Submission> gradedSubmissions)
        {
            var allSaved = new bool[gradedSubmissions.Count];
            try
            {
                var index = 0;
                var tasks = gradedSubmissions.Select(s => SaveSubmission(db, s));
                allSaved = await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return allSaved.All(s => s == true);
        }

        /// <summary>
        /// Gets the appointments to populate the calendar in the Dashboard.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="acctId"></param>
        /// <returns></returns>
        public async Task<List<AppointmentData>> GetAppointments(AzureDbContext db, ILocalStorageService storage)
        {
            var appointments = new List<AppointmentData>();
            var courses = await BrowserStorage<List<Course>>.GetObject(storage, "courses", new List<Course>());
            var asses = GetAssignments(db, courses);

            try
            {
                var index = 0;
                foreach (var c in courses)
                {
                    var st = DateTime.ParseExact(c.StartTime, "H:mm", null, System.Globalization.DateTimeStyles.None);
                    var et = DateTime.ParseExact(c.EndTime, "H:mm", null, System.Globalization.DateTimeStyles.None);
                    var sd = c.StartDate;
                    var ed = c.EndDate;

                    var today = DateTime.UtcNow;
                    while (today < ed)
                    {
                        appointments.Add(new AppointmentData()
                        {
                            Id = index++,
                            Description = $"{c.Description}",
                            StartTime = st,
                            EndTime = et,
                            IsAllDay = false,
                            Location = $"Zoom Meeting",
                            Subject = $"{c.Name}"
                        });

                        st = st.AddDays(7);
                        et = et.AddDays(7);
                        today = today.AddDays(7);
                    }
                }
                foreach (var a in asses)
                {
                    appointments.Add(new AppointmentData()
                    {
                        Id = index++,
                        Description = $"{a.Name} DUE",
                        StartTime = a.DueDate ?? DateTime.UtcNow,
                        EndTime = a.DueDate ?? DateTime.UtcNow.AddHours(2),
                        IsAllDay = false,
                        Location = $"Assignment",
                        Subject = $"Assignment"
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return appointments;
        }

        /// <summary>
        /// Generates a Bell Chart based off the passed assignments' grades, if any.
        /// </summary>
        /// <param name="db"></param>
        /// <param name="asses"></param>
        /// <param name="chartName"></param>
        /// <returns></returns>
        public async Task<BoxPlotChart> GetAssignmentStandingChart(AzureDbContext db, List<Assignment> asses, string chartName = "Standing")
        {
            var chart = new BoxPlotChart()
            {
                Name = chartName,
                Series = new Series[asses.Count]
            };

            for (int i = 0; i < asses.Count; i++)
            {
                var ass = asses[i];

                var gradedSubmissions = await db.Submissions.Where(s => s.AssignmentId == asses[i].AssignmentId && s.Score > 0)
                .Select(s => s.Score)
                .OrderBy(s => s)
                .ToArrayAsync();

                var quartileLength = (gradedSubmissions.Length / 2) - 1;
                var quartileMedian = (int)(Math.Round(quartileLength / 2d, 0));
                var median = (int)(Math.Round(gradedSubmissions.Length / 2d, 0));
                var q1 = gradedSubmissions.Take(quartileLength).ToArray()[quartileMedian];
                var q3 = gradedSubmissions.TakeLast(quartileLength).ToArray()[quartileMedian];

                var seriesData = gradedSubmissions.Select(s => new SeriesData
                {
                    low = gradedSubmissions.Min(),
                    q1 = q1,
                    median = gradedSubmissions[median],
                    q3 = q3,
                    high = gradedSubmissions.Max()
                }).ToArray();

                var series = new Series()
                {
                    AssignmentId = ass.AssignmentId,
                    Name = ass.Name,
                    PointsPossible = ass.MaxScore,
                    Data = seriesData.First()
                };

                chart.Series[i] = series;
            }

            return chart;
        }
    }
}

