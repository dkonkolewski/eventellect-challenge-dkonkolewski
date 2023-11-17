using Elections.Interfaces;

namespace Eventellect.Elections.UnitTests.Models;

public record Vote(ICandidate Candidate) : IVote;
