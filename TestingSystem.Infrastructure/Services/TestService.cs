using TestingSystem.Core.Models;
using TestingSystem.Core.Interfaces;
using TestingSystem.Core.Enums;
using TestingSystem.Core.DTOs;

namespace TestingSystem.Infrastructure.Services;

public class TestService : ITestService
{
    private readonly ITestRepository _repository;
    private readonly IAnswerValidator _answerValidator;

    public TestService(ITestRepository repository, IAnswerValidator answerValidator)
    {
        _repository = repository;
        _answerValidator = answerValidator;
    }

    public async Task<Test?> GetTestAsync(int testId)
    {
        return await _repository.GetTestWithQuestionsAsync(testId);
    }

    public async Task<TestSession> StartTestAsync(int testId, Guid userId)
    {
        var test = await _repository.GetTestWithQuestionsAsync(testId);
        if (test == null)
            throw new Exception($"Test {testId} not found");

        if (test.Status != TestStatus.Published)
            throw new Exception($"Test {testId} is not published");

        var maxScore = test.Questions.Sum(q => q.Points);

        var session = new TestSession
        {
            Id = Guid.NewGuid(),
            TestId = testId,
            UserId = userId,
            StartedAt = DateTime.UtcNow,
            Status = TestStatus.InProgress,
            MaxPossibleScore = maxScore,
            Score = 0,
            IsPassed = false,
            UserAnswers = new List<UserAnswer>()
        };

        return await _repository.CreateSessionAsync(session);
    }

    public async Task<UserAnswer> SubmitAnswerAsync(Guid sessionId, int questionId, string answer)
    {
        var session = await _repository.GetSessionAsync(sessionId);
        if (session == null)
            throw new Exception($"Session {sessionId} not found");

        if (session.Status != TestStatus.InProgress)
            throw new Exception($"Session {sessionId} is not in progress");

        var test = await _repository.GetTestWithQuestionsAsync(session.TestId);
        var question = test?.Questions.FirstOrDefault(q => q.Id == questionId);
        if (question == null)
            throw new Exception($"Question {questionId} not found");

        // Проверка ответа (аналог проверки остатков из задания 7.2)
        var validationResult = await _answerValidator.ValidateAnswerAsync(question, answer);
        
        var userAnswer = new UserAnswer
        {
            SessionId = sessionId,
            QuestionId = questionId,
            SelectedAnswerIds = validationResult.IsMultipleChoice ? answer : null,
            TextAnswer = !validationResult.IsMultipleChoice ? answer : null,
            IsCorrect = validationResult.IsCorrect,
            PointsEarned = validationResult.IsCorrect ? question.Points : 0
        };

        var savedAnswer = await _repository.SaveAnswerAsync(userAnswer);
        
        // Обновляем общий счёт сессии (исправлено: убраны операторы ??)
        session.Score = session.Score + savedAnswer.PointsEarned;
        await _repository.UpdateSessionAsync(session);

        return savedAnswer;
    }

    public async Task<TestResult> CompleteTestAsync(Guid sessionId)
    {
        var session = await _repository.GetSessionAsync(sessionId);
        if (session == null)
            throw new Exception($"Session {sessionId} not found");

        var test = await _repository.GetTestWithQuestionsAsync(session.TestId);
        
        session.Status = TestStatus.Completed;
        session.CompletedAt = DateTime.UtcNow;
        session.IsPassed = session.Score >= (test?.PassingScore ?? 0);
        
        await _repository.UpdateSessionAsync(session);

        var result = new TestResult
        {
            SessionId = sessionId,
            TestId = session.TestId,
            UserId = session.UserId,
            Score = session.Score,
            MaxScore = session.MaxPossibleScore,
            Percentage = session.MaxPossibleScore > 0 ? (double)session.Score / session.MaxPossibleScore * 100 : 0,
            IsPassed = session.IsPassed,
            CompletedAt = session.CompletedAt.Value,
            TimeSpentMinutes = (int)(session.CompletedAt.Value - session.StartedAt).TotalMinutes
        };

        return await _repository.SaveResultAsync(result);
    }
}
