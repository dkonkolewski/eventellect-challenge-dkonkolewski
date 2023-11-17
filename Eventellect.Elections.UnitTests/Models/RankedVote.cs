using Elections.Interfaces;

namespace Eventellect.Elections.UnitTests.Models;

public record RankedVote(ICandidate Candidate, int Rank) : IRankedVote;
