using TestingSystem.Core.Models;

namespace TestingSystem.Core.Interfaces;

public interface IAnalyticsService
{
    Task<Analytics> GetTestAnalyticsAsync(int testId);
    Task<double> GetAverageScoreByUserAsync(Guid userId);
}
