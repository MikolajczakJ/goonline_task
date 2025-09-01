using ToDo_API.Entities;

namespace ToDo_API.Services
{
    public interface IToDoService
    {
        IEnumerable<ToDo> GetAll();
        ToDo? GetById(int id);
        IEnumerable<ToDo> GetIncomingToDo();
        ToDo Create(ToDo toDo);
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

        public ToDo Create(ToDo toDo)
        {
            throw new NotImplementedException();
        }

        public bool Delete(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ToDo> GetAll()
        {
            throw new NotImplementedException();
        }

        public ToDo? GetById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ToDo> GetIncomingToDo()
        {
            throw new NotImplementedException();
        }

        public bool MarkAsCompleted(int id)
        {
            throw new NotImplementedException();
        }

        public bool SetPercentageDone(int id, byte percentage)
        {
            throw new NotImplementedException();
        }

        public bool Update(int id, ToDo toDo)
        {
            throw new NotImplementedException();
        }
    }
}
