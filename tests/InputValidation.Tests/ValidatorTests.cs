using NUnit.Framework;

namespace InputValidation.Tests;

public class ValidatorTests
{
    private readonly IValidator fixture = new Validator();
    
    [Test]
    public void GIVEN_no_at_WHEN_validating_THEN_return_false()
    {
        // Arrange
        var email = "john.doeexample.com";

        // Act
        var result = fixture.IsValidEmail(email);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void GIVEN_no_user_WHEN_validating_THEN_return_false()
    {
        // Arrange
        var email = "@example.com";

        // Act
        var result = fixture.IsValidEmail(email);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void GIVEN_no_tld_WHEN_validating_THEN_return_false()
    {
        // Arrange
        var email = "john.doe@example";

        // Act
        var result = fixture.IsValidEmail(email);

        // Assert
        Assert.IsFalse(result);
    }

    [Test]
    public void GIVEN_valid_email_WHEN_validating_THEN_return_true()
    {
        // Arrange
        var email = "john.doe@example.com";

        // Act
        var result = fixture.IsValidEmail(email);

        // Assert
        Assert.IsTrue(result);
    }
}
