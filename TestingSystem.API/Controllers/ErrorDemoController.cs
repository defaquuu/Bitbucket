using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Polly;
using Polly.CircuitBreaker;
using Serilog;

namespace TestingSystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ErrorDemoController : ControllerBase
{
    private static int _attempt = 0;
    private static readonly Random _random = new();

    private static readonly IAsyncPolicy _retryPolicy = Policy
        .Handle<Exception>()
        .WaitAndRetryAsync(3,
            retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
            onRetry: (ex, time, retryCount, ctx) =>
            {
                Log.Information("Retry {RetryCount} after {Time}s - {Error}", retryCount, time.TotalSeconds, ex.Message);
            });

    private static readonly AsyncCircuitBreakerPolicy _circuitBreaker = Policy
        .Handle<Exception>()
        .CircuitBreakerAsync(3, TimeSpan.FromSeconds(30),
            onBreak: (ex, duration, ctx) =>
            {
                Log.Warning("Circuit BREAKER OPEN for {Duration}s", duration.TotalSeconds);
            },
            onReset: (ctx) =>
            {
                Log.Information("Circuit breaker CLOSED");
            });

    private static readonly IAsyncPolicy _policy = _circuitBreaker.WrapAsync(_retryPolicy);

    [HttpPost("simulate-error")]
    public async Task<IActionResult> SimulateError()
    {
        try
        {
            var result = await _policy.ExecuteAsync(async () =>
            {
                _attempt++;
                Log.Information("Attempt #{Attempt}", _attempt);

                if (_random.Next(1, 4) <= 2)
                {
                    throw new Exception("Simulated network error");
                }

                return true;
            });

            return Ok(new { success = result, message = "OK" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = "Service unavailable", details = ex.Message });
        }
    }
}
