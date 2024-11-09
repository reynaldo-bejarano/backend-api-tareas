using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tareas.Domain.Entitites;

namespace Tareas.Application.Interfaces
{
    public interface IAccount
    {
        Task<string> CreateAccountAsync(User user);
        Task<string> UpdateAccountAsync(string id, User user);
        Task<string> DeleteAccountAsync(string id);
        Task<User?> GetAccountAsync(string email, string password);
    }
}
