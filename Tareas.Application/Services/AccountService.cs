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
    public class AccountService : IAccount
    {

        private readonly ApplicationDbContext _context;
        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<string> CreateAccountAsync(User user) {
            try {
                var isUserAlreadyExists = await _context.Users.SingleOrDefaultAsync(u => u.Email == user.Email);

                if (isUserAlreadyExists != null) throw new Exception("401: User already exists");

                user.Password = HashPassword(user.Password);

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return "200";
            }
            catch (Exception ex)
            {
                return ex.Message;
            } 
        }

        public async Task<User?> GetAccountAsync(string email, string password)
        {
            try
            {
                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);

                if (user == null || !VerifyPassword(password, user.Password)) throw new Exception();

                return user;
            }
            catch(Exception) {
                return null;
            }
        }

        public Task<string> UpdateAccountAsync(string id, User user)
        {
            throw new NotImplementedException();
        }


        public Task<string> DeleteAccountAsync(string id)
        {
            throw new NotImplementedException();
        }

        private  string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        // Método para verificar la contraseña ingresada
        private  bool VerifyPassword(string enteredPassword, string storedHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedHash);
        }
    }
}
