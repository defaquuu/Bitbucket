using TestingSystem.Core.Models;
using TestingSystem.Core.Interfaces;

namespace TestingSystem.Infrastructure.Services;

public class AnalyticsService : IAnalyticsService
{
    private readonly ITestRepository _repository;

    public AnalyticsService(ITestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Analytics> GetTestAnalyticsAsync(int testId)
    {
        var results = await _repository.GetResultsByTestAsync(testId);
        var resultsList = results.ToList();
        
        var test = await _repository.GetTestWithQuestionsAsync(testId);
        
        var analytics = new Analytics
        {
            TestId = testId,
            TestTitle = test?.Title ?? "Unknown",
            TotalAttempts = resultsList.Count,
            CompletedAttempts = resultsList.Count(r => r.CompletedAt != default),
            AverageScore = resultsList.Any() ? resultsList.Average(r => r.Percentage) : 0,
            PassRate = resultsList.Any() ? (double)resultsList.Count(r => r.IsPassed) / resultsList.Count * 100 : 0
        };
        
        return analytics;
    }

    public async Task<double> GetAverageScoreByUserAsync(Guid userId)
    {
        var results = await _repository.GetResultsByUserAsync(userId);
        return results.Any() ? results.Average(r => r.Percentage) : 0;
    }
}
