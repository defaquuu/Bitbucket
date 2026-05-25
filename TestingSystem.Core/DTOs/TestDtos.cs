namespace TestingSystem.Core.DTOs;

public class StartTestRequest
{
    public int TestId { get; set; }
    public Guid UserId { get; set; }
}

public class SubmitAnswerRequest
{
    public Guid SessionId { get; set; }
    public int QuestionId { get; set; }
    public string Answer { get; set; } = string.Empty;
}

public class TestResultResponse
{
    public Guid SessionId { get; set; }
    public int Score { get; set; }
    public int MaxScore { get; set; }
    public double Percentage { get; set; }
    public bool IsPassed { get; set; }
    public List<QuestionResult> QuestionResults { get; set; } = new();
}

public class QuestionResult
{
    public int QuestionId { get; set; }
    public string QuestionText { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
    public int PointsEarned { get; set; }
    public int MaxPoints { get; set; }
    public string? CorrectAnswer { get; set; }
    public string? UserAnswer { get; set; }
}
