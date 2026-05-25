using TestingSystem.Core.Models;
using TestingSystem.Core.Interfaces;
using TestingSystem.Core.Enums;

namespace TestingSystem.Infrastructure.Repositories;

public class InMemoryTestRepository : ITestRepository
{
    private static Dictionary<int, Test> _tests = new();
    private static Dictionary<Guid, TestSession> _sessions = new();
    private static List<TestResult> _results = new();

    static InMemoryTestRepository()
    {
        var test = new Test
        {
            Id = 1,
            Title = "C# Basics",
            Description = "Test your C# knowledge",
            DurationMinutes = 30,
            PassingScore = 60,
            Status = TestStatus.Published,
            CreatedAt = DateTime.UtcNow,
            Questions = new List<Question>()
        };

        var q1 = new Question
        {
            Id = 1,
            TestId = 1,
            Text = "What is C#?",
            Type = QuestionType.SingleChoice,
            Points = 20,
            Answers = new List<Answer>()
        };
        q1.Answers.Add(new Answer { Id = 1, QuestionId = 1, Text = "Programming language", IsCorrect = true });
        test.Questions.Add(q1);

        _tests[1] = test;
    }

    public Task<Test?> GetTestWithQuestionsAsync(int testId)
    {
        _tests.TryGetValue(testId, out var test);
        return Task.FromResult(test);
    }

    public Task<TestSession?> GetSessionAsync(Guid sessionId)
    {
        _sessions.TryGetValue(sessionId, out var session);
        return Task.FromResult(session);
    }

    public Task<TestSession> CreateSessionAsync(TestSession session)
    {
        _sessions[session.Id] = session;
        return Task.FromResult(session);
    }

    public Task<UserAnswer> SaveAnswerAsync(UserAnswer answer)
    {
        var session = _sessions[answer.SessionId];
        answer.Id = session.UserAnswers.Count + 1;
        session.UserAnswers.Add(answer);
        return Task.FromResult(answer);
    }

    public Task UpdateSessionAsync(TestSession session)
    {
        _sessions[session.Id] = session;
        return Task.CompletedTask;
    }

    public Task<TestResult> SaveResultAsync(TestResult result)
    {
        result.Id = _results.Count + 1;
        _results.Add(result);
        return Task.FromResult(result);
    }

    public Task<IEnumerable<TestResult>> GetResultsByUserAsync(Guid userId)
    {
        return Task.FromResult(_results.Where(r => r.UserId == userId));
    }

    public Task<IEnumerable<TestResult>> GetResultsByTestAsync(int testId)
    {
        return Task.FromResult(_results.Where(r => r.TestId == testId));
    }
}