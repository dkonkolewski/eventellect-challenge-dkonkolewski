using Elections.Interfaces;
using Eventellect.Elections.UnitTests.Fakes;
using Eventellect.Elections.UnitTests.Models;
using Xunit.Abstractions;

namespace Eventellect.Elections.UnitTests.Tests.Elections.RankedChoiceElection;


public class RunTests : BaseElectionsTest
{
    public RunTests(ITestOutputHelper output) : base(output) { }
    
    [Fact]
    public void Should_Fail_Given_No_Ballots()
    {
        var election = new global::Elections.Elections.RankedChoiceElection();
        var ballots = new IRankedBallot[]{};
        var candidates = CandidateFake.GenerateWithOrderedId(1);
        
        var result = election.Run(ballots, candidates);

        result.Should().BeFailure();
    }
    
    [Fact]
    public void Should_Fail_Given_No_Candidates()
    {
        var election = new global::Elections.Elections.RankedChoiceElection();
        var ballots = RankedBallotFake.GenerateWithOrderedId(1);
        var candidates = new ICandidate[] { };
        
        var result = election.Run(ballots, candidates);

        result.Should().BeFailure();
    }
    
    [Fact]
    public void Should_Return_Fail_Given_FirstRank_Tie()
    {
        var rankedChoiceElection = new global::Elections.Elections.RankedChoiceElection();
        var voters = VoterFake.GenerateWithOrderedId(3);
        var candidates = CandidateFake.GenerateWithOrderedId(3);
        
        RankedBallot MakeSingleVoteRankedBallot(int index)
        {
            return new RankedBallot
            {
                Voter = voters[index],
                Votes = new []
                {
                    new RankedVote(candidates[index], Rank: 1)
                }
            };
        }

        RankedBallot[] ballots =
        {
            MakeSingleVoteRankedBallot(0),
            MakeSingleVoteRankedBallot(1),
            MakeSingleVoteRankedBallot(2)
        };
            
        var result = rankedChoiceElection.Run(ballots, candidates);

        result.Should().BeFailure();
    }
    
    [Fact]
    public void Should_Succeed_Given_FirstRank_Majority()
    {
        var rankedChoiceElection = new global::Elections.Elections.RankedChoiceElection();
        var voters = VoterFake.GenerateWithOrderedId(3);
        var candidates = CandidateFake.GenerateWithOrderedId(3);

        RankedBallot MakeSingleVoteRankedBallot(int voter, int candidate)
        {
            return new RankedBallot
            {
                Voter = voters[voter],
                Votes = new[]
                {
                    new RankedVote(candidates[candidate], Rank: 1)
                }
            };
        }

        RankedBallot[] ballots =
        {
            MakeSingleVoteRankedBallot(0,0),
            MakeSingleVoteRankedBallot(1, 0),
            MakeSingleVoteRankedBallot(2, 1)
        };
        var winner = candidates[0];
            
        var result = rankedChoiceElection.Run(ballots, candidates);

        result.Should().BeSuccess();
        result.Value.Should().Be(winner);
    }
    
    [Fact]
    public void Should_Fail_Given_Second_Round_Tie()
    {
        var rankedChoiceElection = new global::Elections.Elections.RankedChoiceElection();
        var voters = VoterFake.GenerateWithOrderedId(10);
        var candidates = CandidateFake.GenerateWithOrderedId(3);

        RankedBallot MakeRankedBallot(int voter, int firstChoice, int secondChoice, int thirdChoice)
        {
            return new RankedBallot
            {
                Voter = voters[voter],
                Votes = new[]
                {
                    new RankedVote(candidates[firstChoice], Rank: 1),
                    new RankedVote(candidates[secondChoice], Rank: 2),
                    new RankedVote(candidates[thirdChoice], Rank: 3),
                }
            };
        }

        RankedBallot[] ballots =
        {
            // 50% chose #0, then #1, then #2
            MakeRankedBallot(voter: 0, firstChoice: 0, secondChoice: 1, thirdChoice: 2),
            MakeRankedBallot(voter: 1, firstChoice: 0, secondChoice: 1, thirdChoice: 2),
            MakeRankedBallot(voter: 2, firstChoice: 0, secondChoice: 1, thirdChoice: 2),
            MakeRankedBallot(voter: 3, firstChoice: 0, secondChoice: 1, thirdChoice: 2),
            MakeRankedBallot(voter: 4, firstChoice: 0, secondChoice: 1, thirdChoice: 2),
            
            // 30% chose #1, then #2, then #0
            MakeRankedBallot(voter: 5, firstChoice: 1, secondChoice: 2, thirdChoice: 0),
            MakeRankedBallot(voter: 6, firstChoice: 1, secondChoice: 2, thirdChoice: 0),
            MakeRankedBallot(voter: 7, firstChoice: 1, secondChoice: 2, thirdChoice: 0),
            
            // 20% chose #2, then #1, then #0
            MakeRankedBallot(voter: 8, firstChoice: 2, secondChoice: 1, thirdChoice: 0),
            MakeRankedBallot(voter: 9, firstChoice: 2, secondChoice: 1, thirdChoice: 0),
        };
        
        // first rank run:   no majority winner, #2 eliminated. Second choice votes get passed to #1
        // second rank run:  now there is a tie between #0 (5 votes) and #1 (3 rank1 votes, 2 rank2 votes)
        var result = rankedChoiceElection.Run(ballots, candidates);

        result.Should().BeFailure();
    }
    
    [Fact]
    public void Should_Succeed_Given_SecondRound_Majority()
    {
        var rankedChoiceElection = new global::Elections.Elections.RankedChoiceElection();
        var voters = VoterFake.GenerateWithOrderedId(20);
        var candidates = CandidateFake.GenerateWithOrderedId(3);

        RankedBallot MakeRankedBallot(int voter, int firstChoice, int secondChoice, int thirdChoice)
        {
            return new RankedBallot
            {
                Voter = voters[voter],
                Votes = new[]
                {
                    new RankedVote(candidates[firstChoice], Rank: 1),
                    new RankedVote(candidates[secondChoice], Rank: 2),
                    new RankedVote(candidates[thirdChoice], Rank: 3),
                }
            };
        }

        RankedBallot[] ballots =
        {
            // 35% chose #0, then #1, then #2
            MakeRankedBallot(voter: 0, firstChoice: 0, secondChoice: 1, thirdChoice: 2),
            MakeRankedBallot(voter: 1, firstChoice: 0, secondChoice: 1, thirdChoice: 2),
            MakeRankedBallot(voter: 2, firstChoice: 0, secondChoice: 1, thirdChoice: 2),
            MakeRankedBallot(voter: 3, firstChoice: 0, secondChoice: 1, thirdChoice: 2),
            MakeRankedBallot(voter: 4, firstChoice: 0, secondChoice: 1, thirdChoice: 2),
            MakeRankedBallot(voter: 5, firstChoice: 0, secondChoice: 1, thirdChoice: 2),
            MakeRankedBallot(voter: 6, firstChoice: 0, secondChoice: 1, thirdChoice: 2), 
            
            // 35% chose #1, then #2, then #0
            MakeRankedBallot(voter: 7, firstChoice: 1, secondChoice: 2, thirdChoice: 0),
            MakeRankedBallot(voter: 8, firstChoice: 1, secondChoice: 2, thirdChoice: 0),
            MakeRankedBallot(voter: 9, firstChoice: 1, secondChoice: 2, thirdChoice: 0),
            MakeRankedBallot(voter: 10, firstChoice: 1, secondChoice: 2, thirdChoice: 0),
            MakeRankedBallot(voter: 11, firstChoice: 1, secondChoice: 2, thirdChoice: 0),
            MakeRankedBallot(voter: 12, firstChoice: 1, secondChoice: 2, thirdChoice: 0),
            MakeRankedBallot(voter: 13, firstChoice: 1, secondChoice: 2, thirdChoice: 0),
            
            // 30% chose #2, then #1, then #0
            MakeRankedBallot(voter: 14, firstChoice: 2, secondChoice: 1, thirdChoice: 0),
            MakeRankedBallot(voter: 15, firstChoice: 2, secondChoice: 1, thirdChoice: 0),
            MakeRankedBallot(voter: 16, firstChoice: 2, secondChoice: 1, thirdChoice: 0),
            MakeRankedBallot(voter: 17, firstChoice: 2, secondChoice: 1, thirdChoice: 0),
            MakeRankedBallot(voter: 18, firstChoice: 2, secondChoice: 1, thirdChoice: 0),
            MakeRankedBallot(voter: 19, firstChoice: 2, secondChoice: 1, thirdChoice: 0),
        };
        
        // first rank run:   no majority winner, #2 eliminated. Second choice votes get passed to #1
        // second rank run:  now #1 is the majority winner (7 rank1 votes + 6 rank2 votes) > #0 (7 rank 1 votes)
        var winner = candidates[1];
            
        var result = rankedChoiceElection.Run(ballots, candidates);

        result.Should().BeSuccess();
        result.Value.Should().Be(winner);
    }
}