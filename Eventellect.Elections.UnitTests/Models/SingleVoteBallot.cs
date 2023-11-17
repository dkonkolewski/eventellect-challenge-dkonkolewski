using Elections.Interfaces;

namespace Eventellect.Elections.UnitTests.Models;

public record SingleVoteBallot : ISingleVoteBallot
{
    public IVoter Voter { get; set; }
    public IVote Vote { get; set; }
}