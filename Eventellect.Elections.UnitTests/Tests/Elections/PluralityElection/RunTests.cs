using System.Collections.ObjectModel;
using Elections.Interfaces;
using Eventellect.Elections.UnitTests.Fakes;
using Eventellect.Elections.UnitTests.Models;
using Xunit.Abstractions;

namespace Eventellect.Elections.UnitTests.Tests.Elections.PluralityElection;

public class RunTests : BaseElectionsTest
{
    public RunTests(ITestOutputHelper output) : base(output) { }

    [Fact]
    public void Should_Return_Fail_Given_No_Ballots()
    {
        var election = new global::Elections.Elections.PluralityElection();
        var ballots = new ISingleVoteBallot[]{};
        var candidates = CandidateFake.GenerateWithOrderedId(1);
        
        var result = election.Run(ballots, candidates);

        result.Should().BeFailure();
    }
    
    [Fact]
    public void Should_Fail_Given_No_Candidates()
    {
        var election = new global::Elections.Elections.PluralityElection();
        var ballots = SingleVoteBallotFake.GenerateWithOrderedId(1);
        var candidates = new ICandidate[] { };
        
        var result = election.Run(ballots, candidates);

        result.Should().BeFailure();
    }
    
    [Fact]
    public void Should_Fail_Given_Tie()
    {
        var election = new global::Elections.Elections.PluralityElection();
        var candidates = CandidateFake.GenerateWithOrderedId(3);
        var voters = VoterFake.GenerateWithOrderedId(6);
        var ballots = new ISingleVoteBallot[]
        {
            new SingleVoteBallot {Voter = voters[0], Vote = new Vote(candidates[0])},
            new SingleVoteBallot {Voter = voters[1], Vote = new Vote(candidates[0])},
            new SingleVoteBallot {Voter = voters[2], Vote = new Vote(candidates[0])},
            new SingleVoteBallot {Voter = voters[3], Vote = new Vote(candidates[1])},
            new SingleVoteBallot {Voter = voters[4], Vote = new Vote(candidates[1])},
            new SingleVoteBallot {Voter = voters[5], Vote = new Vote(candidates[1])},
        };
        
        var result = election.Run(ballots, candidates);

        result.Should().BeFailure();
    }

    [Fact]
    public void Should_Succeed_Given_Majority()
    {
        var election = new global::Elections.Elections.PluralityElection();
        var candidates = CandidateFake.GenerateWithOrderedId(3);
        var voters = VoterFake.GenerateWithOrderedId(6);
        var ballots = new ISingleVoteBallot[]
        {
            new SingleVoteBallot {Voter = voters[0], Vote = new Vote(candidates[0])},
            new SingleVoteBallot {Voter = voters[1], Vote = new Vote(candidates[0])},
            new SingleVoteBallot {Voter = voters[2], Vote = new Vote(candidates[0])},
            new SingleVoteBallot {Voter = voters[3], Vote = new Vote(candidates[1])},
            new SingleVoteBallot {Voter = voters[4], Vote = new Vote(candidates[1])},
            new SingleVoteBallot {Voter = voters[5], Vote = new Vote(candidates[2])},
        };
        
        var result = election.Run(ballots, candidates);

        var winner = candidates[0];
        
        result.Should().BeSuccess();
        result.Value.Should().Be(winner);
    }
}