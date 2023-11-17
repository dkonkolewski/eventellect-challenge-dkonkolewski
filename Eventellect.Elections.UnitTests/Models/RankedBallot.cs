using Elections.Interfaces;

namespace Eventellect.Elections.UnitTests.Models;

public class RankedBallot : IRankedBallot
{
    public IVoter Voter { get; set; }
    public IReadOnlyList<IRankedVote> Votes { get; set; }
}