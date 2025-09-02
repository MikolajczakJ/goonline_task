using ToDo_API.Entities;

namespace ToDo_API.Services
{
    public interface IToDoService
    {
        IEnumerable<ToDo> GetAll();
        ToDo GetById(int id);
        IEnumerable<ToDo> GetIncomingToDo(DateTime startingDate, DateTime endingDate);
        bool Create(ToDo toDo);
        bool Update(int id, ToDo toDo);
        bool SetPercentageDone(int id, byte percentage);
        bool Delete(int id);
        bool MarkAsCompleted(int id);
    }
    public class ToDoService:IToDoService
    {
        private readonly ToDoDbContext _context;
        public ToDoService(ToDoDbContext context)
        {
            _context = context; 
        }

        public bool Create(ToDo toDo)
        {
            if (CheckValues(toDo))
            {
                _context.ToDos.Add(toDo);
            }
            return _context.SaveChanges()>0;
        }

        public bool Delete(int id)
        {
            var toDo = FindToDo(id);
            _context.ToDos.Remove(toDo);
            return _context.SaveChanges()>0;

        }

        public IEnumerable<ToDo> GetAll()
        {
            var toDos = _context.ToDos.ToList();
            return toDos;
        }

        public ToDo GetById(int id)
        {
           var toDo = FindToDo(id);
           return toDo;
        }

        public IEnumerable<ToDo> GetIncomingToDo(DateTime startingDate, DateTime endingDate)
        {
            var toDos = _context.ToDos
                .Where(t => t.Expiration >= startingDate && t.Expiration <= endingDate)
                .ToList();
            return toDos;
        }

        public bool MarkAsCompleted(int id)
        {
            var toDo = FindToDo(id);
            toDo.IsDone = true;
            return _context.SaveChanges()>0;
        }

        public bool SetPercentageDone(int id, byte percentage)
        {
            var toDo = FindToDo(id);
            if(percentage<0 || percentage>100)
                throw new ArgumentException("Percentage must be between 0 and 100");
            toDo.PercentageDone = percentage;
            return _context.SaveChanges()>0;
        }

        public bool Update(int id, ToDo toDo)
        {
            if (CheckValues(toDo))
            {
               var toDoEntity = FindToDo(id);
                  toDoEntity.Title = toDo.Title;
                  toDoEntity.Description = toDo.Description;
                  toDoEntity.PercentageDone = toDo.PercentageDone;
                  toDoEntity.Expiration = toDo.Expiration;
            }
            return _context.SaveChanges()>0;

        }

        private ToDo FindToDo(int id)
        {
            var toDo = _context.ToDos.FirstOrDefault(t => t.Id == id);
            if (toDo is null)
                throw new KeyNotFoundException($"ToDo with id {id} not found");
            
            return toDo;
        }
        private bool CheckValues(ToDo toDo)
        {
            if (string.IsNullOrWhiteSpace(toDo.Title) || toDo.Title.Length > 70)
                throw new ArgumentException("Title is required and must be less than 70 characters");
            if (string.IsNullOrWhiteSpace(toDo.Description) || toDo.Description.Length > 500)
                throw new ArgumentException("Description is required and must be less than 500 characters");
            if (toDo.PercentageDone<0 || toDo.PercentageDone > 100)
                throw new ArgumentException("PercentageDone must be between 0 and 100");
            if (toDo.Expiration < DateTime.Now)
                throw new ArgumentException("Expiration must be a future date");
            return true;
        }
    }
}
