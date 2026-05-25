using TestingSystem.Core.Models;

namespace TestingSystem.Core.Interfaces;

public interface ITestRepository
{
    Task<Test?> GetTestWithQuestionsAsync(int testId);
    Task<TestSession?> GetSessionAsync(Guid sessionId);
    Task<TestSession> CreateSessionAsync(TestSession session);
    Task<UserAnswer> SaveAnswerAsync(UserAnswer answer);
    Task UpdateSessionAsync(TestSession session);
    Task<TestResult> SaveResultAsync(TestResult result);
    Task<IEnumerable<TestResult>> GetResultsByUserAsync(Guid userId);
    Task<IEnumerable<TestResult>> GetResultsByTestAsync(int testId);
}
