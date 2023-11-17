using Elections.Interfaces;

namespace Eventellect.Elections.UnitTests.Models;

public class Voter : IVoter
{
    public int Id { get; set; }
    public string Name { get; set; }
}