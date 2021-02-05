using Xunit;
using Shouldly;
using System.Linq;

namespace UnitTest
{
    public class StateTests
    {
        [Fact]
        public void State_IsNotNull()
        {
            // Arrange.
            #region Arrange
            LMS.Data.Helper.State StateObj;
            #endregion

            // Act.
            #region Act
            StateObj = new LMS.Data.Helper.State();
            #endregion

            // Assert.
            #region Assert
            StateObj.States.ShouldNotBeNull();
            #endregion
        }

        [Fact]
        public void State_IsNotEmpty()
        {
            // Arrange.
            #region Arrange
            LMS.Data.Helper.State StateObj;
            #endregion

            // Act.
            #region Act
            StateObj = new LMS.Data.Helper.State();
            #endregion

            // Assert.
            #region Assert
            StateObj.States.Count.ShouldBeGreaterThanOrEqualTo(50);
            #endregion
        }

        [Fact]
        public void States_AreInitializedCorrectly()
        {
            // Arrange.
            #region Arrange
            LMS.Data.Helper.State StateObj;
            var invalidChars = new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
            #endregion

            // Act.
            #region Act
            StateObj = new LMS.Data.Helper.State();
            #endregion

            // Assert.
            #region Assert
            StateObj.States.ShouldBeUnique();
            StateObj.States.ShouldAllBe(s => s.Name != null);
            StateObj.States.ShouldAllBe(s => s.GetType() == typeof(LMS.Data.Helper.State.US_State));
            StateObj.States.ShouldAllBe(s => !s.Name.Any(c => invalidChars.Contains(c)));
            StateObj.States.ShouldAllBe(s => s.Abbreviations.Length == 2);
            #endregion
        }
    }
}
