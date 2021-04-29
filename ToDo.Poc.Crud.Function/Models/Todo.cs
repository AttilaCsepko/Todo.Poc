using System;

namespace ToDo.Poc.Crud.Function.Models
{
    public class TodoCreateModel
    {
        public string TaskDescription { get; set; }
    }

    public class TodoUpdateModel
    {
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class Todo
    {
        public string Id { get; set; } = Nanoid.Nanoid.Generate(size: 10);
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public string TaskDescription { get; set; }
        public bool IsCompleted { get; set; }
    }
}
