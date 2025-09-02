using ToDo_API.Entities;
using ToDo_API.Models;

namespace ToDo_API.Services
{
    public interface IToDoService
    {
        IEnumerable<ReadToDoDTO> GetAll();
        ReadToDoDTO GetById(int id);
        IEnumerable<ReadToDoDTO> GetIncomingToDo(DateTime startingDate, DateTime endingDate);
        bool Create(ToDoDTO toDoDTO);
        bool Update(int id, ToDoDTO toDoDTO);
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

        public bool Create(ToDoDTO toDoDTO)
        {
            if (CheckValues(toDoDTO))
            {
                ToDo toDo = new ToDo
                {
                    Title = toDoDTO.Title,
                    Description = toDoDTO.Description,
                    Expiration = toDoDTO.Expiration.ToUniversalTime(),
                    PercentageDone = toDoDTO.PercentageDone,
                    IsDone = false,
                    CreatedAt = DateTime.UtcNow
                };
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

        public IEnumerable<ReadToDoDTO> GetAll()
        {
            var toDos = _context.ToDos.Select(t => ConvertFromEntity(t)).ToList();
            return toDos;
        }

        public ReadToDoDTO GetById(int id)
        {
           var toDo = FindToDo(id);
           return ConvertFromEntity(toDo);
        }

        public IEnumerable<ReadToDoDTO> GetIncomingToDo(DateTime startingDate, DateTime endingDate)
        {
            var toDos = _context.ToDos
                .Where(t => t.Expiration >= startingDate && t.Expiration <= endingDate)
                .Select(t => ConvertFromEntity(t))  
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

        public bool Update(int id, ToDoDTO toDoDTO)
        {
            if (CheckValues(toDoDTO))
            {
               var toDoEntity = FindToDo(id);
                  toDoEntity.Title = toDoDTO.Title;
                  toDoEntity.Description = toDoDTO.Description;
                  toDoEntity.PercentageDone = toDoDTO.PercentageDone;
                  toDoEntity.Expiration = toDoDTO.Expiration;
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
        private bool CheckValues(ToDoDTO toDoDTO)
        {
            if (string.IsNullOrWhiteSpace(toDoDTO.Title) || toDoDTO.Title.Length > 70)
                throw new ArgumentException("Title is required and must be less than 70 characters");
            if (string.IsNullOrWhiteSpace(toDoDTO.Description) || toDoDTO.Description.Length > 500)
                throw new ArgumentException("Description is required and must be less than 500 characters");
            if (toDoDTO.PercentageDone<0 || toDoDTO.PercentageDone > 100)
                throw new ArgumentException("PercentageDone must be between 0 and 100");
            if (toDoDTO.Expiration < DateTime.Now)
                throw new ArgumentException("Expiration must be a future date");
            return true;
        }
        private static ReadToDoDTO ConvertFromEntity(ToDo toDo)
        {
            return new ReadToDoDTO(
                toDo.Id,
                toDo.Title,
                toDo.Description,
                toDo.CreatedAt.ToLocalTime(),
                toDo.Expiration.ToLocalTime(),
                toDo.PercentageDone,
                toDo.IsDone
                );
        }
    }
}
