using Bogus;
using Eventellect.Elections.UnitTests.Models;

namespace Eventellect.Elections.UnitTests.Fakes;

public static class VoterFake
{
    private static int _voterId = 1;
    
    public static List<Voter> GenerateWithOrderedId(int count)
    {
        var voterFaker = new Faker<Voter>()
            .RuleFor(x => x.Id, _ => _voterId++)
            .RuleFor(x => x.Name, f => f.Name.FullName());

        return voterFaker.Generate(count);
    }
}