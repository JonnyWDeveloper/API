using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lms.Core.Entities
{
#nullable disable
    public class Tournament
    {
        public int Id
        {
            get; set;
        }
        [Required]
        [MaxLength(30)]
        public string Title
        {
            get; set;
        }

        public DateTime StartDate
        {
            get; set;
        }

        public ICollection<Game> Games
        {
            get; set;
        }
    }
}
