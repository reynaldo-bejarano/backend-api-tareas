using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tareas.Domain.Entitites
{
    public class User
    {
        public int Id { get; set; }
        [MinLength(3)]
        public required string Name { get; set; }
        [EmailAddress]
        public required string Email { get; set; }
        [MinLength(6)]
        public required string Password { get; set; }
        public string? PasswordConfirmed { get; set; } 


    }
}
