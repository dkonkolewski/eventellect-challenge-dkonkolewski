using Xunit.Abstractions;

namespace Eventellect.Elections.UnitTests.Tests;

public class TestConsoleWriter : StringWriter
{
    private ITestOutputHelper _output;
    
    public TestConsoleWriter(ITestOutputHelper output)
    {
        _output = output;
    }

    public override void WriteLine(string m)
    {
        _output.WriteLine(m);
    }
}
