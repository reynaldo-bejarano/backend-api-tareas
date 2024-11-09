using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tareas.Domain.Entitites
{
    public class Tarea
    {
        public int ID { get; set; }
        [MinLength(5)]
        public required string Description { get; set; }
        [MinLength(5)]
        public required string Title { get; set; }


    }
}
