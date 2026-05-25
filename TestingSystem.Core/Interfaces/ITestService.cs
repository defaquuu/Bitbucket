using TestingSystem.Core.Models;

namespace TestingSystem.Core.Interfaces;

public interface ITestService
{
    Task<Test?> GetTestAsync(int testId);
    Task<TestSession> StartTestAsync(int testId, Guid userId);
    Task<UserAnswer> SubmitAnswerAsync(Guid sessionId, int questionId, string answer);
    Task<TestResult> CompleteTestAsync(Guid sessionId);
}
