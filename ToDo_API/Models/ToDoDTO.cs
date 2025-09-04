namespace ToDo_API.Models
{
    public record ToDoDTO(
        string Title,
        string Description,
        DateTime Expiration,
        byte PercentageDone
        );
    public record ReadToDoDTO(
        int Id,
        string Title,
        string Description,
        DateTime CreatedAt,
        DateTime Expiration,
        byte PercentageDone,
        bool IsDone
        );
}
