using Bogus;
using Eventellect.Elections.UnitTests.Models;

namespace Eventellect.Elections.UnitTests.Fakes;

public static class CandidateFake
{
    private static int _candidateId = 1;
    
    public static List<Candidate> GenerateWithOrderedId(int count)
    { 
        var candidateFaker = new Faker<Candidate>()
            .RuleFor(x => x.Id, _ => _candidateId++)
            .RuleFor(x => x.Name, f => f.Name.FullName());

        return candidateFaker.Generate(count);
    }
}