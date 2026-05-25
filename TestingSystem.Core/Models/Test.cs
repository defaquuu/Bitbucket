using TestingSystem.Core.Enums;

namespace TestingSystem.Core.Models;

public class Test
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int DurationMinutes { get; set; }
    public int PassingScore { get; set; }
    public TestStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public List<Question> Questions { get; set; } = new();
}
