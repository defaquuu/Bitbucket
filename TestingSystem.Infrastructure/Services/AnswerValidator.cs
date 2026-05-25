using System;
using System.Threading.Tasks;
using Polly;
using Serilog;
using TestingSystem.Core.Models;
using TestingSystem.Core.Interfaces;

namespace TestingSystem.Infrastructure.Services;

public class AnswerValidator : IAnswerValidator
{
    private static int _errorCounter = 0;
    private static readonly Random _rand = new();

    // Retry: 3 попытки, задержка 2,4,8 сек
    private static readonly IAsyncPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (ex, time, retryCount, ctx) =>
            {
                Log.Warning("Retry {RetryCount} after {Time}s - {Error}", retryCount, time.TotalSeconds, ex.Message);
            });

    // Circuit Breaker: после 3 ошибок открыт 30 сек
    private static readonly IAsyncPolicy _circuitBreaker = Policy
        .Handle<Exception>()
        .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
            onBreak: (ex, duration, ctx) =>
            {
                Log.Error("Circuit BREAKER OPEN for {Duration}s", duration.TotalSeconds);
            },
            onReset: (ctx) =>
            {
                Log.Information("Circuit breaker CLOSED");
            });

    private static readonly IAsyncPolicy _policy = _circuitBreaker.WrapAsync(_retryPolicy);

    public async Task<AnswerValidationResult> ValidateAnswerAsync(Question question, string userAnswer)
    {
        return await _policy.ExecuteAsync(async () =>
        {
            // Имитация случайной ошибки для демонстрации 9.3
            _errorCounter++;
            if (_rand.Next(1, 5) <= 4) // 2 из 4 вызовов падают
            {
                Log.Error("Validation failed (simulated) for question {QuestionId}", question.Id);
                throw new Exception("Answer validation service error");
            }

            // === НОРМАЛЬНАЯ ПРОВЕРКА (твоя логика) ===
            var result = new AnswerValidationResult();

            if (question.Type == Core.Enums.QuestionType.SingleChoice)
            {
                var isCorrect = userAnswer == question.Answers.First(a => a.IsCorrect).Id.ToString();
                result.IsCorrect = isCorrect;
            }
            else if (question.Type == Core.Enums.QuestionType.MultipleChoice)
            {
                var correctIds = question.Answers.Where(a => a.IsCorrect).Select(a => a.Id.ToString());
                var userIds = userAnswer.Split(',');
                result.IsCorrect = correctIds.OrderBy(x => x).SequenceEqual(userIds.OrderBy(x => x));
            }

            Log.Information("Answer validated. Correct: {IsCorrect}", result.IsCorrect);
            return result;
        });
    }
}