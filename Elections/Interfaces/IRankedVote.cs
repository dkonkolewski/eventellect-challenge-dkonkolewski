﻿namespace Elections.Interfaces;

public interface IRankedVote : IVote
{
    int Rank { get; }
}
