using Eventellect.Elections.UnitTests.Tests;
using Xunit.Abstractions;

public abstract class BaseElectionsTest
{
    private ITestOutputHelper _output;
    private TestConsoleWriter _textWriter;
    
    public BaseElectionsTest(ITestOutputHelper output)
    {
        _output = output;
        _textWriter = new TestConsoleWriter(_output);
        
        Console.SetOut(_textWriter);
    }
}