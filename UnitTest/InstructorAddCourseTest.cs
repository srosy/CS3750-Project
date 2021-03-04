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
    public class InstructorAddCourseTest
    {
        private readonly ServiceCollection _services;
        private readonly ServiceProvider _provider;
        private readonly AzureDbContext _context;
        private readonly DbService _db;

        public InstructorAddCourseTest()
        {
            _services = new ServiceCollection();
            _services.AddDbContext<AzureDbContext>(options => options.UseSqlServer("Server=tcp:srosy-weber.database.windows.net,1433;Initial Catalog=LMS;Persist Security Info=False;User ID=srosy;Password=GaviSpe64!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
            _provider = _services.BuildServiceProvider();
            _context = _provider.GetRequiredService(typeof(AzureDbContext)) as AzureDbContext;
            _db = new DbService();
        }

        [Fact]
        public async Task InstructorAddCourse()
        {
            // Arrange
            #region Arrange
            // Retrieving n classes from the professor's schedule
            var courses = _context.Courses.Where(c => c.DeleteDate == null && c.ProfessorId == 108);
            int count = courses.Count();
            #endregion

            // Act
            #region Act
            // Adding a new class to the professor's schedule
            var testCourse = new LMS.Data.Models.Course()
            {
                Name = "TestCourse",
                ProfessorId = 108,
                StartDate = new DateTime(2021, 1, 1),
                EndDate = new DateTime(2021, 5, 5),
                Credits = 1,
                CreateDate = DateTime.Now
            };
            await _db.SaveCourse(_context, testCourse);
            #endregion

            // Assert
            #region Assert
            // Comparing n+1 classes to the professor's schedule
            courses = _context.Courses.Where(c => c.DeleteDate == null && c.ProfessorId == 108);
            courses.Count().ShouldBeEquivalentTo(count + 1);
            #endregion
        }
    }
}
