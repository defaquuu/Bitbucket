using TestingSystem.Core.Enums;

namespace TestingSystem.Core.Models;

public class TestSession
{
    public Guid Id { get; set; }
    public int TestId { get; set; }
    public Guid UserId { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public TestStatus Status { get; set; }
    public int Score { get; set; }
    public int MaxPossibleScore { get; set; }
    public bool IsPassed { get; set; }
    public List<UserAnswer> UserAnswers { get; set; } = new();
}

public class UserAnswer
{
    public int Id { get; set; }
    public Guid SessionId { get; set; }
    public int QuestionId { get; set; }
    public string? SelectedAnswerIds { get; set; }
    public string? TextAnswer { get; set; }
    public bool IsCorrect { get; set; }
    public int PointsEarned { get; set; }
}
