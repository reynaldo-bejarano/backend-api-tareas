using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tareas.Domain.Entitites;

namespace Tareas.Application.Interfaces
{
    public interface ITareas
    {
        Task<string> CreateTaskAsync(Tarea tarea);
        Task<string> DeleteTaskAsync(string id);
        Task<Tarea?> GetTaskAsync(string id);
        Task<string> UpdateTaskAsync(string id, Tarea tarea);
        Task<List<Tarea>> GetAllTaskAsync();
    }
}
