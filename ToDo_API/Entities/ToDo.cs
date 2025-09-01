using System.ComponentModel.DataAnnotations;

namespace ToDo_API.Entities
{
    public class ToDo
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public byte PercentageDone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime Expiration { get; set; }
        public bool IsDone { get; set; }
    }
}
