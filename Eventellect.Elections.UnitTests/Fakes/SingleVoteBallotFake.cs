using Bogus;
using Eventellect.Elections.UnitTests.Models;

namespace Eventellect.Elections.UnitTests.Fakes;

public static class SingleVoteBallotFake
{
    public static List<SingleVoteBallot> GenerateWithOrderedId(int count)
    {
        var candidateFaker = new Faker<SingleVoteBallot>()
            .RuleFor(x => x.Voter, _ => VoterFake.GenerateWithOrderedId(1).First())
            .RuleFor(x => x.Vote, _ => new Vote(CandidateFake.GenerateWithOrderedId(1).First()));
            
        return candidateFaker.Generate(count);
    }
}