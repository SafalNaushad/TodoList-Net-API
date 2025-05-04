using System.Text.Json.Serialization;
using TodoList.Model;

public class TodoItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsComplete { get; set; }

    [JsonIgnore] // Hides in Swagger input
    public int UserId { get; set; }

    [JsonIgnore] // Prevents Swagger and serialization loops
    public User?User { get; set; }
}
