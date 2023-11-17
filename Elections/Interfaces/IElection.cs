using FluentResults;

namespace Elections.Interfaces;

public interface IElection<TBallot>
    where TBallot : IBallot
{
    Result<ICandidate> Run(IReadOnlyList<TBallot> ballots, IReadOnlyList<ICandidate> candidates);
}