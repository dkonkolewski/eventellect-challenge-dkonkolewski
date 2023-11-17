using Elections.Interfaces;

namespace Eventellect.Elections.UnitTests.Models;

public class Candidate : ICandidate
{
    public int Id { get; set; }
    public string Name { get; set; }
}