using Xunit;
using Shouldly;

namespace UnitTest
{
    public class EncryptionTests
    {
        [Fact]
        public void PasswordHash_IsCorrect()
        {
            // Arrange.
            #region Arrange
            var pass = "password";
            var salt = "salt1234abc_Protoss";
            var expected = "AyZjj/9SE0X5aJn5m1VTaOuHOsp4he72MYaSohCQDcs=";
            #endregion

            // Act.
            #region Act
            var hashedPass = LMS.Data.Helper.Encryption.GenerateSaltedHash(pass, salt);
            #endregion

            // Assert.
            #region Assert
            hashedPass.ShouldBe(expected);
            #endregion
        }

        [Fact]
        public void PasswordHash_IsNotEmpty()
        {
            // Arrange.
            #region Arrange
            var pass = "password";
            var salt = "salt1234abc_Protoss";
            #endregion

            // Act.
            #region Act
            var hashedPass = LMS.Data.Helper.Encryption.GenerateSaltedHash(pass, salt);
            #endregion

            // Assert.
            #region Assert
            hashedPass.ShouldNotBe(string.Empty);
            #endregion
        }

        [Fact]
        public void GenSalt_IsNotEmpty()
        {
            // Arrange.
            #region Arrange
            #endregion

            // Act.
            #region Act
            var salt = LMS.Data.Helper.Encryption.GenSalt();
            #endregion

            // Assert.
            #region Assert
            salt.ShouldNotBe(string.Empty);
            #endregion
        }

        [Fact]
        public void GenSalt_IsRandom()
        {
            // Arrange.
            #region Arrange
            var firstSalt = string.Empty;
            var secondSalt = string.Empty;
            #endregion

            // Act.
            #region Act
            firstSalt = LMS.Data.Helper.Encryption.GenSalt();
            secondSalt = LMS.Data.Helper.Encryption.GenSalt();
            #endregion

            // Assert.
            #region Assert
            firstSalt.ShouldNotBe(secondSalt);
            #endregion
        }
    }
}
