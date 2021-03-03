using LMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
namespace UnitTest
{
    public class StudentEnrolledCoursesTests
    {
        private readonly ServiceCollection _services;
        private readonly ServiceProvider _provider;
        private readonly AzureDbContext _context;
        private readonly DbService _db;

        public StudentEnrolledCoursesTests()
        {
            _services = new ServiceCollection();
            _services.AddDbContext<AzureDbContext>(options => options.UseSqlServer("Server=tcp:srosy-weber.database.windows.net,1433;Initial Catalog=LMS;Persist Security Info=False;User ID=srosy;Password=GaviSpe64!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
            _provider = _services.BuildServiceProvider();
            _context = _provider.GetRequiredService(typeof(AzureDbContext)) as AzureDbContext;
            _db = new DbService();
        }

        [Fact]
        public async Task StudentEnrollThenDropCourse()
        {
            //Getting n classes student is enrolled in
            var enrollments = await _db.GetEnrollments(_context, 74);
            int count = enrollments.Count();

            enrollments.Add(new LMS.Data.Models.Enrollment
            {
                AccountId = 74,
                CourseId = 11,
                Credits = 4,
                CreateDate = DateTime.Now
            });

            //Enrolling student in n+1 classes
            await _db.UpdateEnrollments(_context, 74, enrollments);
            enrollments = await _db.GetEnrollments(_context, 74);

            //Comparing n+1 to enrolled classes
            enrollments.Count().ShouldBeEquivalentTo(count + 1);

            var courses = await _context.Courses.Where(c => c.CourseId == enrollments.Last().CourseId).ToListAsync();

            //Unenrolling student from class
            await _db.UpdateEnrollmentsOnDeletedCourse(_context, courses.FirstOrDefault());
            enrollments = await _db.GetEnrollments(_context, 74);

            //Comparing n to enrolled classes
            enrollments.Count().ShouldBeEquivalentTo(count);
        }
    }
}
