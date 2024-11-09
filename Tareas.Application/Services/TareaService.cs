using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tareas.Application.Interfaces;
using Tareas.Domain.Entitites;
using Tareas.Infrastructure.Context;

namespace Tareas.Application.Services
{
    public class TareaService : ITareas
    {
        private readonly ApplicationDbContext _context;
        public TareaService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<string> CreateTaskAsync(Tarea tarea)
        {
            try
            {
                _context.Tareas.Add(tarea);

                await _context.SaveChangesAsync();

                return "200";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<string> DeleteTaskAsync(string id)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(int.Parse(id));

                if (tarea == null) throw new Exception("Tarea not found");

                _context.Tareas.Remove(tarea);
                await _context.SaveChangesAsync();

                return "200";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<List<Tarea>> GetAllTaskAsync()
        {
            try
            {
                var tareas = await _context.Tareas.ToListAsync();

                return tareas;
            }
            catch (Exception)
            {
                return [];
            }
        }

        public async Task<Tarea?> GetTaskAsync(string id)
        {
            try
            { 
                var tarea = await _context.Tareas.FindAsync(int.Parse(id)) ?? throw new Exception();

                return tarea;
            }
            catch(Exception) {
                return null;
            }
        }

        public async Task<string> UpdateTaskAsync(string id, Tarea tarea)
        {
            try
            {
                var tareaFound = await _context.Tareas.FindAsync(int.Parse(id)) ?? throw new KeyNotFoundException("Tarea not found");

                tareaFound.Description = tarea.Description;
                tareaFound.Title = tarea.Title;

                await _context.SaveChangesAsync();

                return "200";
            }
            catch(KeyNotFoundException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
               return ex.Message;
            }
        }
    }
}
