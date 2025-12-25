using System.ComponentModel.DataAnnotations;

namespace TestManagement.Dtos
{
    public class TaskDto
    {
        [Required, MinLength(3), MaxLength(100)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        [Range(0, 2)]
        public byte Priority { get; set; } = 1;

        public DateTime? DueDate { get; set; }
    }

    public class UpdateTaskDto : TaskDto
    {
        public bool IsCompleted { get; set; }
    }
}
