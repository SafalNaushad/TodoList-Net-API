namespace TodoList.DTOs
{
    public class TodoCreateDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsComplete { get; set; }
    }
}
