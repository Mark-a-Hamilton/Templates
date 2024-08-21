using Domain.Functions;

namespace UnitTests.Tests;

public class SampleClassTests
{
    private readonly SampleClass _sut;

    #region Constructor
    public SampleClassTests()
    {
        _sut = new SampleClass(); // Create instance of Test class
    }
    #endregion

    /*
     * A simple way to write a Unit test can be done using Artificial Intelligence (AI) by following the steps below
     * 1) Open your favourite text editor I use Notepad++
     * 2) Copy the method code into your text editor
     * 3) Above the code write the request for example "Using xUnit Framework right a Theory Unit Test for C# code below including examples and Exceptions.
     * 4) Copy all the text and paste into your preferred AI I use Co-Pilot.
     * 5) Once the test is generated this provides a starting new inline data can be added to comprehensively test the method.
     * WARNING - AI is a tool don't expect the test to be comprehensive. 
     */

    #region Fact Test
    [Fact]
    public void SampleMethod_String()
    {
        // Initailise - Arrange - Variables for tests
        int a = 1;

        // Call - Act - Method for test
        var result = _sut.SampleMethod(a);

        // Execute - Assert - Test results
       result.Should().ContainAny("OK!", "Failed");
       result.Should().NotBeNullOrEmpty();
    }
    #endregion

    #region Theory Test Example
    [Theory]
    // Initailise - Arrange - Test Data and expected result in each instance
    [InlineData(1, 2, 3)]
    [InlineData(10, 12, 22)]
    [InlineData(7, 2, 9)]
    public void SampleMethod_Int(int a, int b, int expected)
    {
        // Call - Act - Method for test
        // Execute - Assert - Test results}
        _sut.SampleMethod(a, b).Should().Be(expected);
        //result.Should().Be(expected);
    }
    #endregion
}