using System.Diagnostics;
using Elections.Interfaces;
using FluentResults;

namespace Elections.Elections;

public class PluralityElection : IElection<ISingleVoteBallot>
{
    public Result<ICandidate> Run(IReadOnlyList<ISingleVoteBallot> ballots, IReadOnlyList<ICandidate> candidates)
    {
        if (ballots is null) throw new ArgumentNullException(nameof(ballots));
        if (candidates is null) throw new ArgumentNullException(nameof(candidates));
        
        if (ballots.Count == 0) return Result.Fail($"{nameof(ballots)} cannot be empty.");
        if (candidates.Count == 0) return Result.Fail($"{nameof(candidates)} cannot be empty.");

        Dictionary<ICandidate, int> votesPerCandidate = new();

        foreach(var ballot in ballots)
        {
            var vote = ballot.Vote;
            
            if (votesPerCandidate.ContainsKey(vote.Candidate))
            {
                votesPerCandidate[vote.Candidate]++;
            }
            else
            {
                votesPerCandidate.Add(vote.Candidate, 1);
            }
        }
        
        PrintVoteInfo(votesPerCandidate);

        var potentialWinner = votesPerCandidate
            .MaxBy(votes => votes.Value);

        if (votesPerCandidate.All(votes => votes.Value == potentialWinner.Value))
        {
            return Result.Fail($"All candidates are tied with {potentialWinner.Value} votes.");
        }

        return Result.Ok(potentialWinner.Key);
    }

    [Conditional("DEBUG")]
    private static void PrintVoteInfo(Dictionary<ICandidate, int> votesPerCandidate)
    {
        var totalVotes = votesPerCandidate.Values.Sum(votes => votes);
        
        var results = string.Join("\n",
            votesPerCandidate.Select(
                votes =>
                    $"{votes.Key.Id}\t{votes.Key.Name}\t{votes.Value}\t{100.0f * votes.Value / totalVotes}"));

        Console.WriteLine($"PLURALITY ELECTION:\nID\tNAME\t\tVOTES\t%\n{results}\n");
    }
}
