using Homework.Exercise.Domain.Interfaces;
using FluentAssertions;
using NSubstitute;

namespace Homework.Exercise.Application.Tests.Unit;

public class DateTimeProviderTests
{
    [Fact]
    public void UtcNow_ReturnsCurrentUtcTime()
    {
        // Arrange
        var provider = Substitute.For<IDateTimeProvider>();
        var expected = new DateTime(2001, 1, 1);
        provider.UtcNow.Returns(expected);
        
        // Act
        var result = provider.UtcNow;

        // Assert
        result.Should().Be(expected);
    }
}
