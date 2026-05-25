namespace TestingSystem.Core.Models;

public class Question
{
    public int Id { get; set; }
    public int TestId { get; set; }
    public string Text { get; set; } = string.Empty;
    public TestingSystem.Core.Enums.QuestionType Type { get; set; }
    public int Points { get; set; }
    public List<Answer> Answers { get; set; } = new();
    public string? CorrectAnswerText { get; set; }
}

public class Answer
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsCorrect { get; set; }
}
