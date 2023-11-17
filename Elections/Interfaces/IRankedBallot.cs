﻿namespace Elections.Interfaces;

public interface IRankedBallot : IBallot
{
    IReadOnlyList<IRankedVote> Votes { get; }
}
