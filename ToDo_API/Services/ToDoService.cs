using ToDo_API.Entities;
using ToDo_API.Models;

namespace ToDo_API.Services
{
    public interface IToDoService
    {
        IEnumerable<ReadToDoDTO> GetAll();
        ReadToDoDTO GetById(int id);
        IEnumerable<ReadToDoDTO> GetIncomingToDo(DateTime startingDate, DateTime endingDate);
        int Create(ToDoDTO toDoDTO);
        void Update(int id, ToDoDTO toDoDTO);
        void SetPercentageDone(int id, byte percentage);
        void Delete(int id);
        void MarkAsCompleted(int id);
    }
    public class ToDoService:IToDoService
    {
        private readonly ToDoDbContext _context;
        public ToDoService(ToDoDbContext context)
        {
            _context = context; 
        }

        public int Create(ToDoDTO toDoDTO)
        {
            int id = 0;
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
                id = toDo.Id;
            }
            _context.SaveChanges();
            return id;
        }

        public void Delete(int id)
        {
            var toDo = FindToDo(id);
            _context.ToDos.Remove(toDo);
            _context.SaveChanges();
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

        public void MarkAsCompleted(int id)
        {
            var toDo = FindToDo(id);
            toDo.IsDone = true;
            toDo.PercentageDone = 100;
            _context.SaveChanges();
        }

        public void SetPercentageDone(int id, byte percentage)
        {
            var toDo = FindToDo(id);
            if(percentage<0 || percentage>100)
                throw new ArgumentException("Percentage must be between 0 and 100");
            toDo.PercentageDone = percentage;
            _context.SaveChanges();
        }

        public void Update(int id, ToDoDTO toDoDTO)
        {
            if (CheckValues(toDoDTO))
            {
               var toDoEntity = FindToDo(id);
                  toDoEntity.Title = toDoDTO.Title;
                  toDoEntity.Description = toDoDTO.Description;
                  toDoEntity.PercentageDone = toDoDTO.PercentageDone;
                  toDoEntity.Expiration = toDoDTO.Expiration;
            }
            _context.SaveChanges();

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
