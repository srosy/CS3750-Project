using LMS.Data;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Shouldly;
using LMS.Data.Models;
using static LMS.Data.StripeAPI;
using System.Threading.Tasks;

namespace UnitTest
{
    public class StripeAPI_Tests
    {
        StripeAPI API = new StripeAPI();

        [Fact]
        public async Task Stripe_Token_IsValid()
        {
            /*
             * https://stripe.com/docs/api/tokens/create_card
             * https://stripe.com/docs/testing
             */

            // Arrange.
            #region Arrange
            var payment = new Payment() {
                CardNumber = "4242424242424242",
                ExpDate = DateTime.UtcNow.AddDays(1),
                CVC = 123,
            };
            #endregion

            // Act.
            #region Act
            var token = await API.GenToken(payment);
            #endregion

            // Assert.
            #region Assert
            token.ShouldNotBeNull();
            token.ShouldBeOfType(typeof(Token));
            token.used.ShouldBeFalse();
            #endregion
        }

        [Fact]
        public async Task Stripe_Charge_IsValid()
        {
            /*
             * https://stripe.com/docs/api/charges/create
             * https://stripe.com/docs/testing
             */

            // Arrange.
            #region Arrange
            var payment = new Payment()
            {
                CardNumber = "4242424242424242",
                ExpDate = DateTime.UtcNow.AddDays(1),
                CVC = 123,
                AttemptAmount = 25.00m
            };
            #endregion

            // Act.
            #region Act
            var charge = await API.ChargeCard(payment);
            #endregion

            // Assert.
            #region Assert
            charge.ShouldNotBeNull();
            charge.ShouldBeOfType(typeof(Charge));
            charge.source.ShouldNotBeNull();
            charge.paid.ShouldBeTrue();
            charge.amount_captured.ToString().ShouldMatch(payment.AttemptAmount.ToString().Replace(".", ""));
            charge.failure_code.ShouldBeNull();
            charge.failure_message.ShouldBeNull();
            #endregion
        }
    }
}
