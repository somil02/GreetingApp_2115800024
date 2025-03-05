using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace RepositoryLayer.Entity
{
    public class GreetingEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string GreetingMessage { get; set; }
    }
}
