using LMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using System;
using System.Threading.Tasks;
using Xunit;

namespace UnitTest
{
    public class DbServiceUnitTests
    {
        private readonly ServiceCollection _services;
        private readonly ServiceProvider _provider;
        private readonly AzureDbContext _context;
        private readonly DbService _db;

        public DbServiceUnitTests()
        {
            _services = new ServiceCollection();
            _services.AddDbContext<AzureDbContext>(options => options.UseSqlServer("Server=tcp:srosy-weber.database.windows.net,1433;Initial Catalog=LMS;Persist Security Info=False;User ID=srosy;Password=GaviSpe64!;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));
            _provider = _services.BuildServiceProvider();
            _context = _provider.GetRequiredService(typeof(AzureDbContext)) as AzureDbContext;
            _db = new DbService();
        }

        [Fact]
        public async Task GetAccount_IsCorrect()
        {
            // Arrange.
            #region Arrange
            var testAcct = new LMS.Data.Models.Account()
            {
                AccountId = 42,
                CreateDate = new DateTime(2021, 01, 27, 02, 05, 35, 503),
                DeleteDate = null,
                UpdateDate = null,
                DOB = new DateTime(1994, 11, 09),
                Email = "srosy@gmail.com",
                Role = 1,
                FirstName = "Spencer",
                LastName = "Rosenvall"
            };
            #endregion

            // Act.
            #region Act
            var acct = await _db.GetAccount(_context, testAcct.AccountId);
            #endregion

            // Assert.
            #region Assert
            acct.ShouldNotBeNull();
            acct.ShouldBeOfType(testAcct.GetType());

            acct.AccountId.ShouldBe(testAcct.AccountId);
            acct.CreateDate.ShouldBe(testAcct.CreateDate);
            acct.DeleteDate.ShouldBe(testAcct.DeleteDate);
            acct.UpdateDate.ShouldBe(testAcct.UpdateDate);
            acct.DOB.ShouldBe(testAcct.DOB);
            acct.Email.ShouldBe(testAcct.Email);
            acct.Role.ShouldBe(testAcct.Role);
            acct.FirstName.ShouldBe(testAcct.FirstName);
            acct.LastName.ShouldBe(testAcct.LastName);
            #endregion
        }
    }
}
