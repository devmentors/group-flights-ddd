using System;
using System.Runtime.InteropServices;
using GroupFlights.Shared.Types;
using Xunit;

namespace GroupFlights.Shared.UnitTests.Types;

public class EmailTests
{
    [Theory]
    [InlineData("test@top-level-domain")]
    [InlineData("no-domain-email")]
    internal void Email_ShouldFailOnCreation_ForInvalidEmailFormat(string invalidEmail)
    {
        Assert.Throws<Email.InvalidEmailFormatException>(() =>
        {
            new Email(invalidEmail);
        });
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    internal void Email_ShouldFailOnCreation_ForEmptyValue(string emptyEmail)
    {
        Assert.Throws<ArgumentNullException>(() =>
        {
            new Email(emptyEmail);
        });
    }

    [Theory]
    [InlineData("my-mail@gmail.com")]
    [InlineData("my-mail-with_v3ry_l0ng_name@gmail.com")]
    [InlineData("my-mail@very.complicated.sub.domain.com")]
    internal void Email_ShouldSucceedOnCreation_WithValidValue(string validEmail)
    {
        new Email(validEmail);
    }
}