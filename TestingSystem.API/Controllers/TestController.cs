using Microsoft.AspNetCore.Mvc;
using TestingSystem.Core.Interfaces;
using TestingSystem.Core.DTOs;

namespace TestingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    private readonly ITestService _testService;
    private readonly IAnalyticsService _analyticsService;
    private readonly ILogger<TestController> _logger;

    public TestController(ITestService testService, IAnalyticsService analyticsService, ILogger<TestController> logger)
    {
        _testService = testService;
        _analyticsService = analyticsService;
        _logger = logger;
    }

    [HttpGet("{testId}")]
    public async Task<IActionResult> GetTest(int testId)
    {
        var test = await _testService.GetTestAsync(testId);
        return Ok(test);
    }

    [HttpPost("start")]
    public async Task<IActionResult> StartTest([FromBody] StartTestRequest request)
    {
        _logger.LogInformation("Пользователь {UserId} начал тест {TestId}", request.UserId, request.TestId);
        var session = await _testService.StartTestAsync(request.TestId, request.UserId);
        _logger.LogInformation("Сессия создана: {SessionId}", session.Id);
        return Ok(new { SessionId = session.Id, session.StartedAt, session.MaxPossibleScore });
    }

    [HttpPost("submit")]
    public async Task<IActionResult> SubmitAnswer([FromBody] SubmitAnswerRequest request)
    {
        _logger.LogInformation("Получен ответ на вопрос {QuestionId} для сессии {SessionId}", request.QuestionId, request.SessionId);
        var answer = await _testService.SubmitAnswerAsync(request.SessionId, request.QuestionId, request.Answer);
        return Ok(new { IsCorrect = answer.IsCorrect, PointsEarned = answer.PointsEarned });
    }

    [HttpPost("complete/{sessionId}")]
    public async Task<IActionResult> CompleteTest(Guid sessionId)
    {
        _logger.LogInformation("Завершение теста для сессии {SessionId}", sessionId);
        var result = await _testService.CompleteTestAsync(sessionId);
        _logger.LogInformation("Результат: {Score}/{MaxScore}, Проходной: {IsPassed}", result.Score, result.MaxScore, result.IsPassed);
        return Ok(result);
    }

    [HttpGet("analytics/test/{testId}")]
    public async Task<IActionResult> GetTestAnalytics(int testId)
    {
        var analytics = await _analyticsService.GetTestAnalyticsAsync(testId);
        return Ok(analytics);
    }
}
