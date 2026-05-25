using TestingSystem.Core.Models;

namespace TestingSystem.Core.Interfaces;

public interface IAnswerValidator
{
    Task<AnswerValidationResult> ValidateAnswerAsync(Question question, string userAnswer);
}

public class AnswerValidationResult
{
    public bool IsCorrect { get; set; }
    public bool IsMultipleChoice { get; set; }
    public string? CorrectAnswerText { get; set; }
}
