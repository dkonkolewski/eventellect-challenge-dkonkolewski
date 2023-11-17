using System.Diagnostics;
using Elections.Interfaces;
using FluentResults;

namespace Elections.Elections;

public class RankedChoiceElection : IElection<IRankedBallot>
{ 
    public Result<ICandidate> Run(IReadOnlyList<IRankedBallot> ballots, IReadOnlyList<ICandidate> candidates)
    {
        if (ballots is null) throw new ArgumentNullException(nameof(ballots));
        if (candidates is null) throw new ArgumentNullException(nameof(candidates));
        
        if (ballots.Count == 0) return Result.Fail($"{nameof(ballots)} cannot be empty.");
        if (candidates.Count == 0) return Result.Fail($"{nameof(candidates)} cannot be empty.");
        
         Dictionary<ICandidate, int> totalVotesByCandidate = ballots
            .GroupBy(ballot => ballot.Votes.First().Candidate)
            .ToDictionary(
                keySelector: grouping => grouping.Key,
                elementSelector: grouping => grouping.Count()
            );
        
        // for keeping track of which vote is currently "active" on a ballot 
        int[] ballotVoteIndex = new int[ballots.Count];
        
        int round = 1;
        
        // loop until there is a winner or a tie
        while (true)
        {
            
            var totalVotesThisRound = totalVotesByCandidate.Values.Sum(votes => votes);
            int votesRequiredToWin = (totalVotesThisRound / 2) + 1;

            PrintCurrentVotes(totalVotesByCandidate, totalVotesThisRound, round, votesRequiredToWin);

            var winner = totalVotesByCandidate
                .Where(votes => votes.Value >= votesRequiredToWin)
                .Select(votes => votes.Key)
                .FirstOrDefault();

            if (winner is not null)
            {
                PrintWinner(totalVotesByCandidate, winner, totalVotesThisRound);
                return Result.Ok(winner);
            }

            if (RemainingCandidatesAreTied(totalVotesByCandidate))
            {
                PrintElectionTie();
                return Result.Fail($"Remaining candidates are tied with {totalVotesByCandidate.First().Value} votes each.");
            }

            var lowestVoteCount = totalVotesByCandidate
                .MinBy(votes => votes.Value)
                .Value;
            
            var losers = totalVotesByCandidate
                .Where(votes => votes.Value == lowestVoteCount)
                .Select(votes => votes.Key);
            
            foreach (var loser in losers)
            {
                for (int ballotIndex = 0; ballotIndex < ballots.Count; ballotIndex++)
                {
                    var ballot = ballots[ballotIndex];
                    var voteIndex = ballotVoteIndex[ballotIndex];

                    // skip if all these voter's selections have been eliminated (index will be stored as -1)
                    if (voteIndex == -1) continue;

                    // skip if voter's current selection is still in the race 
                    if (ballot.Votes[voteIndex].Candidate != loser) continue;
                    
                    
                    // this person's current candidate lost
                    // move to their next non-eliminated vote and distribute to that candidate
                    
                    do
                    {
                        voteIndex++;

                        if (voteIndex >= ballot.Votes.Count)
                        {
                            // exhausted the list of candidates for this voter
                            voteIndex = -1;
                            break;
                        }
                        
                    } while (!totalVotesByCandidate.ContainsKey(ballot.Votes[voteIndex].Candidate));

                    if (voteIndex > -1)
                    {
                        // found a valid candidate, distribute the vote
                        totalVotesByCandidate[ballot.Votes[voteIndex].Candidate]++;
                    }
                    
                    // update the ballot's current vote index
                    ballotVoteIndex[ballotIndex] = voteIndex;
                }

                PrintRemovingCandidate(loser);
                
                totalVotesByCandidate.Remove(loser);
            }

            round++;
        }
    }
    
    private bool RemainingCandidatesAreTied(Dictionary<ICandidate, int> totalVotesByCandidate)
    {
        var firstTotalVotes = totalVotesByCandidate.Values.First();
        return totalVotesByCandidate.All(votes => votes.Value == firstTotalVotes);
    }

    [Conditional("DEBUG")]
    private void PrintCurrentVotes(Dictionary<ICandidate, int> totalVotesByCandidate, int totalVotesInRound, int round, int votesRequiredToWin)
    {
        var results = string.Join("\n",
            totalVotesByCandidate
                .Select(votes =>
                    $"{votes.Key.Id}\t{votes.Key.Name}\t{votes.Value}\t{100.0f * votes.Value / totalVotesInRound}"));

        Console.WriteLine($"ROUND {round}\nID\tNAME\t\tVOTES\t%\n{results}\n");
        Console.WriteLine($"Votes required to win this round: {votesRequiredToWin}");
    }
    
    [Conditional("DEBUG")]
    private static void PrintRemovingCandidate(ICandidate loser)
    {
        Console.WriteLine($"Removing candidate {loser.Id}: {loser.Name}\n\n");
    }
    
    [Conditional("DEBUG")]
    private static void PrintWinner(Dictionary<ICandidate, int> totalVotesByCandidate, ICandidate winner, int totalVotesThisRound)
    {
        int winningVotes = totalVotesByCandidate[winner];
        float winningPercent = 100.0f * winningVotes / (float) totalVotesThisRound;
        Console.WriteLine($"A winner has been selected: {winner.Name} with {winningVotes} votes ({winningPercent} %)");
    }

    [Conditional("DEBUG")]
    private static void PrintElectionTie()
    {
        Console.WriteLine($"Election resulted in a tie.");
    }
 
}
