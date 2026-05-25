namespace TestingSystem.Core.Models;

public class Analytics
{
    public int TestId { get; set; }
    public string TestTitle { get; set; } = string.Empty;
    public int TotalAttempts { get; set; }
    public int CompletedAttempts { get; set; }
    public double AverageScore { get; set; }
    public double PassRate { get; set; }
    public Dictionary<int, double> QuestionDifficulty { get; set; } = new();
}
