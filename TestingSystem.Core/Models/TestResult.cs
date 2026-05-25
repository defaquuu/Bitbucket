namespace TestingSystem.Core.Models;

public class TestResult
{
    public int Id { get; set; }
    public Guid SessionId { get; set; }
    public int TestId { get; set; }
    public Guid UserId { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public double Percentage { get; set; }
    public bool IsPassed { get; set; }
    public DateTime CompletedAt { get; set; }
    public int TimeSpentMinutes { get; set; }
}
