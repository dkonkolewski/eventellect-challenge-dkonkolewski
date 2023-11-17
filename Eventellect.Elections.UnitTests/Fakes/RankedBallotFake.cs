using Bogus;
using Eventellect.Elections.UnitTests.Models;

namespace Eventellect.Elections.UnitTests.Fakes;

public class RankedBallotFake
{
    public static List<RankedBallot> GenerateWithOrderedId(int count)
    {
        var candidateFaker = new Faker<RankedBallot>()
            .RuleFor(x => x.Voter, _ => VoterFake.GenerateWithOrderedId(1).First())
            .RuleFor(x => x.Votes, f => new[]
            {
                new RankedVote(
                    Candidate: CandidateFake.GenerateWithOrderedId(1).First(),
                    Rank: 1)
            });
            
        return candidateFaker.Generate(count);
    }
}